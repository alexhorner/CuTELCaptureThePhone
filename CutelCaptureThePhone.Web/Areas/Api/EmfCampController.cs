using CutelCaptureThePhone.Web.EmfCamp;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Areas.Api
{
    [Area("Api")]
    public class EmfCampController(EmfCampDataService emfCampDataService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> VillagesGeoJson()
        {
            string geoJson = await emfCampDataService.GetVillagesGeoJsonAsync();

            return Content(geoJson, "application/json");
        }
    }
}
