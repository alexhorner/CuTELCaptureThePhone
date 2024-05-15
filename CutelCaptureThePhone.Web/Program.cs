using System.Data;
using CutelCaptureThePhone.Core;
using CutelCaptureThePhone.Core.Extensions;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using CutelCaptureThePhone.Data.Postgres;
using CutelCaptureThePhone.Web.Authentication;

//Initialise application builder
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSystemd();

//Initialise application configuration
string persistedDataPath = $"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}PersistedData";

if (!Directory.Exists(persistedDataPath)) Directory.CreateDirectory(persistedDataPath);

string mainConfigurationPath = $"{persistedDataPath}{Path.DirectorySeparatorChar}appsettings.json";
string environmentConfigurationPath = $"{persistedDataPath}{Path.DirectorySeparatorChar}appsettings.{builder.Environment.EnvironmentName}.json";

//Copy default configuration into persistent storage
if (!File.Exists(mainConfigurationPath)) File.Copy($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}appsettings.Default.json", mainConfigurationPath);

//Copy environment configuration into persistent storage, if it exists
if (!File.Exists(environmentConfigurationPath) && File.Exists($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}appsettings.{builder.Environment.EnvironmentName}.json")) File.Copy($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}appsettings.{builder.Environment.EnvironmentName}.json", environmentConfigurationPath);

builder.Configuration.AddJsonFile(mainConfigurationPath, reloadOnChange: true, optional: false);
builder.Configuration.AddJsonFile(environmentConfigurationPath, reloadOnChange: true, optional: true);

builder.Configuration.AddEnvironmentVariables();

//Initialise logging
if (!Directory.Exists($"{persistedDataPath}{Path.DirectorySeparatorChar}logs")) Directory.CreateDirectory($"{persistedDataPath}{Path.DirectorySeparatorChar}logs");

LoggerConfiguration logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File($"{persistedDataPath}{Path.DirectorySeparatorChar}logs{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);

Log.Logger = logger.CreateLogger();

builder.Services.AddLogging(l => l.ClearProviders().AddSerilog());

//Build DI Container
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddHttpContextAccessor();

//Session & Cookies
builder.Services
    .AddDistributedMemoryCache()
    .AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromDays(1);
        options.Cookie.MaxAge = TimeSpan.FromDays(1);
        options.IOTimeout = TimeSpan.FromMinutes(1);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.Name = "token";
    })
    .AddAntiforgery(options =>
    {
        options.FormFieldName = "CSRF";
        options.HeaderName = "CSRF";
        options.Cookie.Name = "CSRF";
    })
    .AddSingleton(new AuthenticationConfiguration
    {
        SessionUserIdKey = "UserId",
        SessionStateCodeKey = "StateCode",
        UnauthorisedErrorRedirectAction = "NotAuthorised",
        UnauthorisedRedirectActionController = "Auth"
    })
    .AddScoped<IAuthenticationManager, AuthenticationManager>();

//Other services
builder.Services.AddCutelCaptureThePhonePostgres(builder.Configuration.GetConnectionString("Default")!);

builder.Services.AddSingleton<PlayerUniquePinGenerator>();
builder.Services.AddSingleton<PlayerUniqueNamesetGenerator>();
builder.Services.AddSingleton<CaptureMessageRandomiser>();

//Final initialisation
WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();

    //app.UseHttpsRedirection();
}

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});

app.UseRouting();
app.UseSession();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("areas", "{area:exists}/{controller}/{action=Index}/{id?}");

#region Run Migrations

using (IServiceScope scope = app.Services.CreateScope())
{
    app.Logger.LogInformation("Checking for available pending database migrations...");
                
    foreach (CutelCaptureThePhoneDbContext context in scope.ServiceProvider.GetServices<CutelCaptureThePhoneDbContext>())
    {
        List<string> migrations = context.Database.GetPendingMigrations().ToList();

        if (!migrations.Any())
        {
            app.Logger.LogInformation($"No migrations found for DB Context '{context.GetType().FullName}'");
            continue;
        }
                    
        app.Logger.LogWarning($"{migrations.Count} migration{(migrations.Count == 1 ? "" : "s")} found for {context.GetType().FullName}. Executing...");
                    
        context.Database.SetCommandTimeout(160);
        context.Database.Migrate();
        
        if (context.Database.GetDbConnection() is NpgsqlConnection connection)
        {
            if (connection.State is not ConnectionState.Open) await connection.OpenAsync();
            
            try
            {
                connection.ReloadTypes();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
                
    app.Logger.LogInformation("Migration check complete");
}

#endregion

//Configure pin generator
using (IServiceScope scope = app.Services.CreateScope())
{
    PlayerUniquePinGenerator playerUniquePinGenerator = scope.ServiceProvider.GetRequiredService<PlayerUniquePinGenerator>();
    IPlayerProvider playerProvider = scope.ServiceProvider.GetRequiredService<IPlayerProvider>();

    int pinLength = builder.Configuration.GetValue<int>("PinLength");
            
    string maxPin = "";
            
    for (int i = 0; i < pinLength; i++) maxPin += "9";

    uint[] usedPins = (await playerProvider.GetAllPinsAsync()).ToArray();

    playerUniquePinGenerator.Configure(int.Parse(maxPin), usedPins);
}

//Configure nameset and nameset generator
using (IServiceScope scope = app.Services.CreateScope())
{
    PlayerUniqueNamesetGenerator playerUniqueNamesetGenerator = scope.ServiceProvider.GetRequiredService<PlayerUniqueNamesetGenerator>();
    IPlayerProvider playerProvider = scope.ServiceProvider.GetRequiredService<IPlayerProvider>();
    
    List<string> nameAParts = Directory.GetFiles(builder.Configuration.GetValue<string>("NameAPartsDirectory")!)
        .Select(f => Path.GetFileName(f).Split('.')[0].ToUpperFirstLetter())
        .Order()
        .ToList();
    
    List<string> nameBParts = Directory.GetFiles(builder.Configuration.GetValue<string>("NameBPartsDirectory")!)
        .Select(f => Path.GetFileName(f).Split('.')[0].ToUpperFirstLetter())
        .Order()
        .ToList();
    
    List<string> nameCParts = Directory.GetFiles(builder.Configuration.GetValue<string>("NameCPartsDirectory")!)
        .Select(f => Path.GetFileName(f).Split('.')[0].ToUpperFirstLetter())
        .Order()
        .ToList();

    NamesetParts namesetParts = new NamesetParts(nameAParts, nameBParts, nameCParts);

    (int PartA, int PartB, int PartC)[] usedNamesets = (await playerProvider.GetAllNamesAsync())
        .Select(namesetParts.GetNamesetFromName)
        .ToArray();
    
    playerUniqueNamesetGenerator.Configure(namesetParts, usedNamesets);
}

//Configure capture positive/negative generator
CaptureMessageRandomiser captureMessageRandomiser = app.Services.GetRequiredService<CaptureMessageRandomiser>();
    
List<string> positives = Directory.GetFiles(builder.Configuration.GetValue<string>("CaptureSuccessDirectory")!)
    .Select(f => Path.GetFileName(f).Split('.')[0].ToLower())
    .Order()
    .ToList();
    
List<string> negatives = Directory.GetFiles(builder.Configuration.GetValue<string>("CaptureFailureDirectory")!)
    .Select(f => Path.GetFileName(f).Split('.')[0].ToLower())
    .Order()
    .ToList();
    
captureMessageRandomiser.Configure(positives, negatives);

//Run application
app.Run();