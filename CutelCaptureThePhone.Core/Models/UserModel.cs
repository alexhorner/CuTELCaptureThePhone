namespace CutelCaptureThePhone.Core.Models
{
    public class UserModel : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }
        
        public string Username { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}