using CutelPhoneGame.Web.Authentication.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CutelPhoneGame.Web.Controllers
{
    public class LeaderboardController : Controller
    {
        [AuthenticatedOnly]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Public()
        {
            return View();
        }
    }
}