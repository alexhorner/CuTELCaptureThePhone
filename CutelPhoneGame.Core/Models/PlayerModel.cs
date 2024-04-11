namespace CutelPhoneGame.Core.Models
{
    public class PlayerModel : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public uint Pin { get; set; }
        public string Name { get; set; } = null!;
        public string RegisteredFromNumber { get; set; } = null!;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        //Relationships
        public List<CaptureModel>? Captures { get; set; }
    }
}