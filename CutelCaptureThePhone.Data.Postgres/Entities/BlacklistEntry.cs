using CutelCaptureThePhone.Core.Enums;
using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Data.Postgres.Entities
{
    public class BlacklistEntry : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public string Value { get; set; } = null!;
        public ValueInterpretation Interpretation { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public BlacklistEntryModel ToModel() => new()
        {
            Id = Id,
            Value = Value,
            Interpretation = Interpretation,
            Created = Created,
            Updated = Updated
        };

        public static BlacklistEntry FromModel(BlacklistEntryModel model) => new()
        {
            Value = model.Value,
            Interpretation = model.Interpretation
        };
    }
}