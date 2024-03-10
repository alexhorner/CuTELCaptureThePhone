namespace CutelPhoneGame.Web.Models.Api
{
    public class PublicLeaderboardApiModel
    {
        public ApiPlayerModel? MostUniqueCapturesPlayer { get; set; }
        public ApiPlayerModel? MostCapturesOverallPlayer { get; set; }
        public List<LeaderboardPlayerStatsApiModel> PlayerStats { get; set; } = new();
    }
}