using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models.Api
{
    public class LeaderboardPlayerStatsApiModel : ApiPlayerModel
    {
        public uint Position { get; set; }
        public int UniqueCaptures { get; set; }
        public int TotalCaptures { get; set; }
        public string FirstCapture { get; set; } = null!;
        public string LatestCapture { get; set; } = null!;
        public LeaderboardPlayerPeriodsApiModel Periods { get; set; } = null!;
        
        public static LeaderboardPlayerStatsApiModel FromModel(PlayerModel model, uint position, int uniqueCaptures, int totalCaptures, string firstCapture, string latestCapture, LeaderboardPlayerPeriodsApiModel periods) => new()
        {
            Pin = model.Pin,
            RegisteredFromNumber = model.RegisteredFromNumber,
            Position = position,
            UniqueCaptures = uniqueCaptures,
            TotalCaptures = totalCaptures,
            FirstCapture = firstCapture,
            LatestCapture = latestCapture,
            Periods = periods
        };
    }
}