using CutelCaptureThePhone.Web.Authentication;
using CutelCaptureThePhone.Web.Authentication.Attributes;
using CutelCaptureThePhone.Web.Authentication.Exceptions;
using CutelCaptureThePhone.Web.Models;
using CutelCaptureThePhone.Core.Extensions;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AuthController(ILogger<AuthController> logger, IAuthenticationManager authenticationManager, IUserProvider userProvider) : Controller
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
        public IActionResult NotAuthorised([FromQuery] string? returnUrl, [FromQuery] bool suppressMessages)
        {
            if (!suppressMessages) TempData["Warning"] = "You do not have permission to view this page";
            
            return RedirectToAction("Index", new { Area = "", ReturnUrl = returnUrl.EnsureValidUrl() });
        }
    }
}