namespace CutelPhoneGame.Web.Models.Api
{
    public class RegisteredPlayerApiModel
    {
        public uint Pin { get; set; }
        public string NamePartA { get; set; } = null!;
        public string NamePartB { get; set; } = null!;
        public string NamePartC { get; set; } = null!;
    }
}