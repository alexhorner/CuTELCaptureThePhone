using CutelCaptureThePhone.Web.Authentication.Attributes;
using CutelCaptureThePhone.Web.Models;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    public class LeaderboardController(IConfiguration configuration, IPlayerProvider playerProvider) : Controller
    {
        [HttpGet]
        [AuthenticatedOnly]
        public async Task<IActionResult> Index([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<PlayerModel> Players, PaginationModel Pagination) paginatedPlayers = await playerProvider.GetAllPaginatedAsync(page.Value, orderByLeaderboard: true);
            
            return View(new BasicLeaderboardViewModel
            {
                Players = paginatedPlayers.Players,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedPlayers.Pagination.CurrentPage,
                    MaxPage = paginatedPlayers.Pagination.MaxPage,
                    PageSwitchController = nameof(LeaderboardController),
                    PageSwitchAction = nameof(Index)
                }
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Public()
        {
            (List<PlayerModel> Players, PaginationModel Pagination) paginatedUsers = await playerProvider.GetAllPaginatedAsync(0, configuration.GetValue<int>("PublicLeaderboardEntries"), true);
            
            return View(new PublicBasicLeaderboardViewModel
            {
                Players = paginatedUsers.Players
            });
        }
    }
}