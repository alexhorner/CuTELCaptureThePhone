using System.Text.Json.Serialization;

namespace CutelCaptureThePhone.Web.EmfCamp
{
    public class PhonesGeoJsonFeatureCollection
    {
        [JsonPropertyName("features")]
        public List<PhonesGeoJsonFeature> Features { get; set; } = [];
    }
}
