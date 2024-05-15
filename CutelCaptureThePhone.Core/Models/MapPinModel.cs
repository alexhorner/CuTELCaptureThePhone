namespace CutelCaptureThePhone.Core.Models
{
    public class MapPinModel
    {
        public uint Id { get; set; }
        
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Name { get; set; } = null!;
        public string Number { get; set; } = null!;
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}