using System.Diagnostics;
using CutelPhoneGame.Web.Authentication.Attributes;
using Microsoft.AspNetCore.Mvc;
using CutelPhoneGame.Web.Models;

namespace CutelPhoneGame.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        [AuthenticatedOnly(suppressMessages: true)]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}