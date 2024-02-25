using CutelPhoneGame.Core.Enums;
using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Data.Postgres.Entities
{
    public class WhitelistEntry : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public string Value { get; set; } = null!;
        public ValueInterpretation Interpretation { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public WhitelistEntryModel ToModel() => new()
        {
            Id = Id,
            Value = Value,
            Interpretation = Interpretation,
            Created = Created,
            Updated = Updated
        };

        public static WhitelistEntry FromModel(WhitelistEntryModel model) => new()
        {
            Value = model.Value,
            Interpretation = model.Interpretation
        };
    }
}