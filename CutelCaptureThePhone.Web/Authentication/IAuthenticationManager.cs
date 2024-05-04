using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Authentication
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