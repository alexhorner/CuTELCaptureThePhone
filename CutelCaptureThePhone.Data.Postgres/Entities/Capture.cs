using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Data.Postgres.Entities
{
    public class Capture : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public uint PlayerId { get; set; }
        public Player? Player { get; set; }

        public string FromNumber { get; set; } = null!;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public CaptureModel ToModel(bool skipModels = true) => new()
        {
            Id = Id,
            PlayerId = PlayerId,
            Player = skipModels ? null : Player?.ToModel(),
            FromNumber = FromNumber,
            Created = Created,
            Updated = Updated
        };

        public static Capture FromModel(CaptureModel model) => new()
        {
            PlayerId = model.PlayerId,
            FromNumber = model.FromNumber
        };
    }
}