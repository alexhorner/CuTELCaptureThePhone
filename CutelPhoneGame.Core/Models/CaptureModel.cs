namespace CutelPhoneGame.Core.Models
{
    public class CaptureModel : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }

        public uint PlayerId { get; set; }
        public PlayerModel? Player { get; set; }

        public string FromNumber { get; set; } = null!;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}