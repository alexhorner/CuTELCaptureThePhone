using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CutelCaptureThePhone.Web.Areas.Api
{
    [Area("Api")]
    public class EmfCampController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache) : Controller
    {
        private const string VillagesGeoJsonCacheKey = "EmfCamp.VillagesGeoJson";
        private const string VillagesGeoJsonUrl = "https://www.emfcamp.org/api/villages.geojson";

        [HttpGet]
        public async Task<IActionResult> VillagesGeoJson()
        {
            string geoJson = await memoryCache.GetOrCreateAsync(VillagesGeoJsonCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                HttpClient httpClient = httpClientFactory.CreateClient();

                return await httpClient.GetStringAsync(VillagesGeoJsonUrl);
            }) ?? throw new InvalidOperationException("Failed to retrieve villages GeoJSON");

            return Content(geoJson, "application/json");
        }
    }
}
