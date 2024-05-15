using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Data.Postgres.Entities
{
    public class MapPin : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }
        
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Name { get; set; } = null!;
        public string Number { get; set; } = null!;
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public MapPinModel ToModel() => new()
        {
            Id = Id,
            Lat = Lat,
            Long = Long,
            Name = Name,
            Number = Number,
            Created = Created,
            Updated = Updated
        };

        public static MapPin FromModel(MapPinModel model) => new()
        {
            Lat = model.Lat,
            Long = model.Long,
            Name = model.Name,
            Number = model.Number
        };
    }
}