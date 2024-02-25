using CutelPhoneGame.Core.Enums;

namespace CutelPhoneGame.Core.Models
{
    public class WhitelistEntryModel : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public string Value { get; set; } = null!;
        public ValueInterpretation Interpretation { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}