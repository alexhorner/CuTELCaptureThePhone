using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Web.Authentication.Exceptions;

namespace CutelPhoneGame.Web.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        public UserModel? CurrentUser { get; internal set; }
        public bool IsLoggedIn => CurrentUser is not null;
        
        private readonly IHttpContextAccessor _context;
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IUserProvider _userProvider;

        public AuthenticationManager(IHttpContextAccessor context, AuthenticationConfiguration authenticationConfiguration, IUserProvider userProvider)
        {
            authenticationConfiguration.Validate();
            
            _context = context;
            _authenticationConfiguration = authenticationConfiguration;
            _userProvider = userProvider;

            if (context.HttpContext is not null && context.HttpContext.Session.IsAvailable)
            {
                string? userIdString = context.HttpContext.Session.GetString(authenticationConfiguration.SessionUserIdKey);

                if (userIdString is not null) CurrentUser = userProvider.GetByIdAsync(Convert.ToUInt32(userIdString)).GetAwaiter().GetResult();
            }
        }

        public async Task LogInAsync(string username, string password)
        {
            if (_context.HttpContext is null || IsLoggedIn || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) throw new UsernameOrPasswordIncorrectException();
            
            UserModel? userProfile = await _userProvider.GetByUsernameAsync(username);

            if (userProfile is null) throw new UsernameOrPasswordIncorrectException();
            
            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, userProfile.HashedPassword)) throw new UsernameOrPasswordIncorrectException();
            
            _context.HttpContext.Session.SetString(_authenticationConfiguration.SessionUserIdKey, userProfile.Id.ToString());
            CurrentUser = userProfile;
        }

        public Task LogOutAsync()
        {
            _context.HttpContext?.Session.Remove(_authenticationConfiguration.SessionUserIdKey);
            CurrentUser = null;
            
            return Task.CompletedTask;
        }

        public async Task ChangePasswordAsync(string password)
        {
            if (!IsLoggedIn) throw new InvalidOperationException("No user is currently logged in");
            
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            
            await _userProvider.UpdateHashedPasswordAsync(CurrentUser!.Id, hashedPassword);
        }
    }
}