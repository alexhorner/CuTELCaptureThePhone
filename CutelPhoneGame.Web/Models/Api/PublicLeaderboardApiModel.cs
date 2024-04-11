namespace CutelPhoneGame.Web.Models.Api
{
    public class PublicLeaderboardApiModel
    {
        public PublicPlayerApiModel? MostUniqueCapturesPlayer { get; set; }
        public PublicPlayerApiModel? MostCapturesOverallPlayer { get; set; }
        public List<LeaderboardStatsPublicPlayerApiModel> PlayerStats { get; set; } = new();
    }
}