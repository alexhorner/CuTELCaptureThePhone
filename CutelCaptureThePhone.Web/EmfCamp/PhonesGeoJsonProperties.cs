using System.Text.Json.Serialization;

namespace CutelCaptureThePhone.Web.EmfCamp
{
    public class PhonesGeoJsonProperties
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
