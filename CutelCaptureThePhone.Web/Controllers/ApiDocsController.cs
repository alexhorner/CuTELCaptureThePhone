using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ApiDocsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}