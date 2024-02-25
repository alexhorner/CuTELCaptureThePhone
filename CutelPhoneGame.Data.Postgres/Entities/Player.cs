using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Data.Postgres.Entities
{
    public class Player : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public uint Pin { get; set; }
        public string RegisteredFromNumber { get; set; } = null!;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        //Relationships
        public IEnumerable<Capture>? Captures { get; set; }

        public PlayerModel ToModel(bool skipModels = true) => new()
        {
            Id = Id,
            Pin = Pin,
            RegisteredFromNumber = RegisteredFromNumber,
            Created = Created,
            Updated = Updated,
            
            Captures = skipModels ? null : Captures?.Select(c => c.ToModel()).ToList()
        };

        public static Player FromModel(PlayerModel model) => new()
        {
            Pin = model.Pin,
            RegisteredFromNumber = model.RegisteredFromNumber
        };
    }
}