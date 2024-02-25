using CutelPhoneGame.Web.Authentication.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CutelPhoneGame.Web.Controllers
{
    [AuthenticatedOnly]
    public class PlayersController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}