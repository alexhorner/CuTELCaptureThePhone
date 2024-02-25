﻿using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Web.Authentication.Attributes;
using CutelPhoneGame.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutelPhoneGame.Web.Controllers
{
    public class LeaderboardController(IPlayerProvider playerProvider) : Controller
    {
        [HttpGet]
        [AuthenticatedOnly]
        public async Task<IActionResult> Index([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<PlayerModel> Players, PaginationModel Pagination) paginatedUsers = await playerProvider.GetAllPaginatedAsync(page.Value, orderByLeaderboard: true);
            
            return View(new BasicLeaderboardViewModel
            {
                Players = paginatedUsers.Players,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedUsers.Pagination.CurrentPage,
                    MaxPage = paginatedUsers.Pagination.MaxPage,
                    PageSwitchController = nameof(LeaderboardController),
                    PageSwitchAction = nameof(Index)
                }
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Public()
        {
            (List<PlayerModel> Players, PaginationModel Pagination) paginatedUsers = await playerProvider.GetAllPaginatedAsync(0, 10, true);
            
            return View(new PublicBasicLeaderboardViewModel
            {
                Players = paginatedUsers.Players
            });
        }
    }
}