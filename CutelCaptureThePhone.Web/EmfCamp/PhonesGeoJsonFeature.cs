using System.Text.Json.Serialization;

namespace CutelCaptureThePhone.Web.EmfCamp
{
    public class PhonesGeoJsonFeature
    {
        [JsonPropertyName("geometry")]
        public PhonesGeoJsonGeometry Geometry { get; set; } = null!;

        [JsonPropertyName("properties")]
        public PhonesGeoJsonProperties Properties { get; set; } = null!;
    }
}
