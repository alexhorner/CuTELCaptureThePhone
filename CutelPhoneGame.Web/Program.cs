using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using CutelPhoneGame.Data.Postgres;
using CutelPhoneGame.Web.Authentication;

//Initialise application builder
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSystemd();

//Initialise logging
if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");

LoggerConfiguration logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File($"logs{Path.DirectorySeparatorChar}log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);

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
builder.Services.AddCutelPhoneGamePostgres(builder.Configuration.GetConnectionString("Default")!);

//Final initialisation
WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();

    //app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("areas", "{area:exists}/{controller}/{action=Index}/{id?}");

#region Run Migrations

using (IServiceScope scope = app.Services.CreateScope())
{
    app.Logger.LogInformation("Checking for available pending database migrations...");
                
    foreach (CutelPhoneGameDbContext context in scope.ServiceProvider.GetServices<CutelPhoneGameDbContext>())
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

//Run application
app.Run();