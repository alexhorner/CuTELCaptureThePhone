namespace CutelPhoneGame.Web.Models.Api
{
    public class LeaderboardPlayerPeriodsApiModel
    {
        public int UniqueCapturesWithin1Hour { get; set; }
        public int UniqueCapturesWithin12Hours { get; set; }
        public int UniqueCapturesWithin24Hours { get; set; }
        
        public int TotalCapturesWithin1Hour { get; set; }
        public int TotalCapturesWithin12Hours { get; set; }
        public int TotalCapturesWithin24Hours { get; set; }
    }
}