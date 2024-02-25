using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Authentication
{
    public interface IAuthenticationManager
    {
        UserModel? CurrentUser { get; }
        bool IsLoggedIn { get; }

        Task LogInAsync(string username, string password);

        Task LogOutAsync();
        
        Task ChangePasswordAsync(string password);
    }
}