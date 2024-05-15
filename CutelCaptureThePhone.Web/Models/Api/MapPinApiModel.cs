namespace CutelCaptureThePhone.Web.Models.Api
{
    public class MapPinApiModel
    {
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public int UniquePlayers { get; set; }
        public int TotalCaptures { get; set; }
        public string? FirstCapturingPlayer { get; set; }
        public string? LatestCapturingPlayer { get; set; }
    }
}