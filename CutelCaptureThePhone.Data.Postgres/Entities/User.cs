using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Data.Postgres.Entities
{
    public class User : IIdentifiable, IDateCreated, IDateUpdated
    {
        public uint Id { get; set; }
        
        public string Username { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public UserModel ToModel() => new()
        {
            Id = Id,
            Username = Username,
            HashedPassword = HashedPassword,
            Created = Created,
            Updated = Updated
        };

        public static User FromModel(UserModel model) => new()
        {
            Username = model.Username,
            HashedPassword = model.HashedPassword
        };
    }
}