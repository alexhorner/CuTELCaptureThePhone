namespace CutelPhoneGame.Web.Models.Api
{
    public class CapturedApiModel
    {
        public bool Captured { get; set; }
        public uint? WaitHours { get; set; }
        public uint? WaitMinutes { get; set; }
        public uint? WaitSeconds { get; set; }
        public int PlayerTotalCaptures { get; set; }
        public int PlayerUniqueCaptures { get; set; }
        public int? PlayerLeaderboardPosition { get; set; }
    }
}