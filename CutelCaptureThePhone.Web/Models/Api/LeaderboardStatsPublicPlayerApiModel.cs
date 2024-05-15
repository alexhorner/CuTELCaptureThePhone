using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models.Api
{
    public class LeaderboardStatsPublicPlayerApiModel : PublicPlayerApiModel
    {
        public uint Position { get; set; }
        public int UniqueCaptures { get; set; }
        public int TotalCaptures { get; set; }
        public string? FirstCapture { get; set; }
        public string? LatestCapture { get; set; }
        public LeaderboardPlayerPeriodsApiModel Periods { get; set; } = null!;
        
        public static LeaderboardStatsPublicPlayerApiModel FromModel(PlayerModel model, uint position, int uniqueCaptures, int totalCaptures, string? firstCapture, string? latestCapture, LeaderboardPlayerPeriodsApiModel periods) => new()
        {
            Name = model.Name,
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