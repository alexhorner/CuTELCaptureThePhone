using CutelCaptureThePhone.Web.Authentication.Attributes;
using CutelCaptureThePhone.Web.Models;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    [AuthenticatedOnly]
    public class PlayersController(ILogger<PlayersController> logger, IPlayerProvider playerProvider) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<PlayerModel> Players, PaginationModel Pagination) paginatedUsers = await playerProvider.GetAllPaginatedAsync(page.Value);
            
            return View(new PlayersViewModel
            {
                Players = paginatedUsers.Players,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedUsers.Pagination.CurrentPage,
                    MaxPage = paginatedUsers.Pagination.MaxPage,
                    PageSwitchController = nameof(PlayersController),
                    PageSwitchAction = nameof(Index)
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] uint? pin)
        {
            if (pin is null or 0)
            {
                TempData["Error"] = "No pin was provided";
                return RedirectToAction("Index");
            }
            
            if (!await playerProvider.ExistsByPinAsync(pin.Value))
            {
                TempData["Error"] = "A player with this pin doesn't exist";
                return RedirectToAction("Index");
            }
            
            try
            {
                await playerProvider.DeleteByPinAsync(pin.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to delete a player with pin '{pin.Value}'");
                
                TempData["Error"] = "An error occured deleting the player. Please try again";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "The player has been successfully deleted";
            return RedirectToAction("Index");
        }
    }
}