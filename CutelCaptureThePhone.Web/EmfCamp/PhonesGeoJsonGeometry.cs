using System.Text.Json.Serialization;

namespace CutelCaptureThePhone.Web.EmfCamp
{
    public class PhonesGeoJsonGeometry
    {
        //[longitude, latitude], per the GeoJSON spec
        [JsonPropertyName("coordinates")]
        public decimal[] Coordinates { get; set; } = [];
    }
}
