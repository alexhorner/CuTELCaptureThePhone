using System.Data;
using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Web.Authentication;
using CutelPhoneGame.Web.Authentication.Attributes;
using CutelPhoneGame.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutelPhoneGame.Web.Controllers
{
    [AuthenticatedOnly]
    public class UsersController(ILogger<UsersController> logger, IAuthenticationManager authenticationManager, IUserProvider userProvider) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<UserModel> Users, PaginationModel Pagination) paginatedUsers = await userProvider.GetAllPaginatedAsync(page.Value);
            
            return View(new UsersViewModel
            {
                Users = paginatedUsers.Users,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedUsers.Pagination.CurrentPage,
                    MaxPage = paginatedUsers.Pagination.MaxPage,
                    PageSwitchController = nameof(UsersController),
                    PageSwitchAction = nameof(Index)
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string? username, [FromForm] string? password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["Error"] = "No username was provided";
                return RedirectToAction("Index");
            }
            
            if (string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "No password was provided";
                return RedirectToAction("Index");
            }
            
            if (await userProvider.ExistsByUsernameAsync(username))
            {
                TempData["Error"] = "A user with this username already exists";
                return RedirectToAction("Index");
            }

            try
            {
                await userProvider.CreateAsync(new UserModel
                {
                    Username = username,
                    HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
                });
            }
            catch (DuplicateNameException)
            {
                TempData["Error"] = "A user with this username already exists";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to create a new user with username '{username}'");
                
                TempData["Error"] = "An error occured creating the user. Please try again";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "The user has been successfully created";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["Error"] = "No username was provided";
                return RedirectToAction("Index");
            }
            
            if (!await userProvider.ExistsByUsernameAsync(username))
            {
                TempData["Error"] = "A user with this username doesn't exist";
                return RedirectToAction("Index");
            }
            
            try
            {
                await userProvider.DeleteByUsernameAsync(username);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to delete a user with username '{username}'");
                
                TempData["Error"] = "An error occured deleting the user. Please try again";
                return RedirectToAction("Index");
            }

            if (username == authenticationManager.CurrentUser!.Username) await authenticationManager.LogOutAsync();

            TempData["Message"] = "The user has been successfully deleted";
            return RedirectToAction("Index");
        }
    }
}