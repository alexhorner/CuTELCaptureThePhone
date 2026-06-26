using Microsoft.Extensions.Caching.Memory;

namespace CutelCaptureThePhone.Web.EmfCamp
{
    public class EmfCampDataService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        private const string VillagesGeoJsonUrl = "https://www.emfcamp.org/api/villages.geojson";
        private const string PhonesGeoJsonUrl = "https://phones.emf.camp/phones.geojson";

        public Task<string> GetVillagesGeoJsonAsync() => GetCachedJsonAsync("EmfCamp.VillagesGeoJson", VillagesGeoJsonUrl);

        //Not cached - phones can be updated live, and the import tool needs to see those changes immediately
        public async Task<string> GetPhonesGeoJsonAsync()
        {
            HttpClient httpClient = httpClientFactory.CreateClient();

            return await httpClient.GetStringAsync(PhonesGeoJsonUrl);
        }

        private async Task<string> GetCachedJsonAsync(string cacheKey, string url)
        {
            return await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                HttpClient httpClient = httpClientFactory.CreateClient();

                return await httpClient.GetStringAsync(url);
            }) ?? throw new InvalidOperationException($"Failed to retrieve JSON from '{url}'");
        }
    }
}
