namespace CutelCaptureThePhone.Web.Models
{
    public class MapPinImportPreviewItemModel
    {
        public string Name { get; set; } = null!;
        public string Number { get; set; } = null!;
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool WillUpdateExisting { get; set; }
    }
}
