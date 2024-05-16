using CutelCaptureThePhone.Web.Authentication;
using CutelCaptureThePhone.Web.Authentication.Attributes;
using CutelCaptureThePhone.Web.Authentication.Exceptions;
using CutelCaptureThePhone.Web.Models;
using CutelCaptureThePhone.Core.Extensions;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Web.Bruteforce;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AuthController(ILogger<AuthController> logger, IAuthenticationManager authenticationManager, IUserProvider userProvider, AntiBruteforceStore antiBruteforceStore) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? returnUrl)
        {
            //Validate redirect URL. Nulls if invalid
            returnUrl = returnUrl.EnsureValidUrl();
            
            //Present login form if not logged in
            if (!authenticationManager.IsLoggedIn)
            {
                if (await userProvider.GetCountAsync() <= 0) TempData["Warning"] = "No accounts currently exist. Log in using your desired credentials and the first account will be created for you automatically";
                
                return View(new LoginViewModel
                {
                    ReturnUrl = returnUrl
                });
            }
            
            //Redirect to return URL is set after validation
            if (returnUrl is not null) return Redirect(returnUrl);
            
            //Redirect home is no return URL is set
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ActionName(nameof(Index))]
        public async Task<IActionResult> IndexPost([FromForm] string? username, [FromForm] string? password, [FromForm] string? returnUrl)
        {
            username = username?.Trim();
            returnUrl = returnUrl.EnsureValidUrl();
            
            if (!authenticationManager.IsLoggedIn)
            {
                if (antiBruteforceStore.IsBlocked(HttpContext.Connection.RemoteIpAddress!.ToString()))
                {
                    TempData["Error"] = "Your username or password was incorrect";
                    logger.LogInformation($"Blocked login attempt from {HttpContext.Connection.RemoteIpAddress!.ToString()} due to bruteforce blocking");
                    return RedirectToAction("Index", new { ReturnUrl = returnUrl });
                }
                
                try
                {
                    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) throw new UsernameOrPasswordIncorrectException();

                    if (await userProvider.GetCountAsync() <= 0)
                    {
                        await userProvider.CreateAsync(new UserModel
                        {
                            Username = username,
                            HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
                        });
                    }
                    
                    await authenticationManager.LogInAsync(username, password);
                }
                catch (UsernameOrPasswordIncorrectException)
                {
                    TempData["Error"] = "Your username or password was incorrect";
                    
                    antiBruteforceStore.LogFailedAttempt(HttpContext.Connection.RemoteIpAddress!.ToString());
                    logger.LogInformation($"Logged failed login attempt for {HttpContext.Connection.RemoteIpAddress!.ToString()}");
                    
                    return RedirectToAction("Index", new { ReturnUrl = returnUrl });
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"An error occured logging in a user with username '{username}'");
                    
                    TempData["Error"] = "An error occured logging you in. Please try again";
                    return RedirectToAction("Index", new { ReturnUrl = returnUrl });
                }
            }
            
            if (returnUrl is not null) return Redirect(returnUrl);
            
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [AuthenticatedOnly]
        public async Task<IActionResult> LogOut()
        {
            if (authenticationManager.IsLoggedIn) await authenticationManager.LogOutAsync();
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthenticatedOnly]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ActionName(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePasswordPost([FromForm] string oldPassword, [FromForm] string newPassword, [FromForm] string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
            {
                TempData["Error"] = "No old password was provided";
                return RedirectToAction("ChangePassword");
            }
            
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                TempData["Error"] = "No new password was provided";
                return RedirectToAction("ChangePassword");
            }
            
            if (string.IsNullOrWhiteSpace(confirmNewPassword))
            {
                TempData["Error"] = "No confirmation password was provided";
                return RedirectToAction("ChangePassword");
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["Error"] = "The new and confirmation passwords do not match";
                return RedirectToAction("ChangePassword");
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(oldPassword, authenticationManager.CurrentUser!.HashedPassword))
            {
                TempData["Error"] = "Your old password is incorrect";
                return RedirectToAction("ChangePassword");
            }
            
            try
            {
                await authenticationManager.ChangePasswordAsync(newPassword);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to update password for user {authenticationManager.CurrentUser!.Id}");
                
                TempData["Error"] = "An error occured updating your password. Please try again";
                return RedirectToAction("ChangePassword");
            }

            TempData["Message"] = "Your password has successfully been updated";
            return RedirectToAction("ChangePassword");
        }
        
        [HttpGet]
        public IActionResult NotAuthorised([FromQuery] string? returnUrl, [FromQuery] bool suppressMessages)
        {
            if (!suppressMessages) TempData["Warning"] = "You do not have permission to view this page";
            
            return RedirectToAction("Index", new { Area = "", ReturnUrl = returnUrl.EnsureValidUrl() });
        }
    }
}