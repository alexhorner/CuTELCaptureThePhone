using CutelCaptureThePhone.Web.Models.Api;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Areas.Api
{
    [Area("Api")]
    public class LeaderboardController(IPlayerProvider playerProvider) : Controller
    {
        public async Task<IActionResult> Public()
        {
            (List<PlayerModel> Players, PaginationModel Pagination) paginatedPlayers = await playerProvider.GetAllPaginatedAsync(0, 10, true);
            
            PlayerModel? mostUniqueCapturesPlayer = paginatedPlayers.Players.MaxBy(p => p.Captures!.GroupBy(c => c.FromNumber).Count());
            PlayerModel? mostCapturesOverall = paginatedPlayers.Players.MaxBy(p => p.Captures!.Count);

            uint count = 1;

            return Ok(new PublicLeaderboardApiModel
            {
                MostUniqueCapturesPlayer = mostUniqueCapturesPlayer is null ? null : PublicPlayerApiModel.FromModel(mostUniqueCapturesPlayer),
                MostCapturesOverallPlayer = mostCapturesOverall is null ? null : PublicPlayerApiModel.FromModel(mostCapturesOverall),
                PlayerStats = paginatedPlayers.Players.Select(p =>
                {
                    int uniqueCaptures = p.Captures!.GroupBy(c => c.FromNumber).Count();
                    
                    CaptureModel? firstCapture = p.Captures!.MinBy(c => c.Created);
                    CaptureModel? latestCapture = p.Captures!.MaxBy(c => c.Created);
                    
                    return LeaderboardStatsPublicPlayerApiModel.FromModel(p, count++, uniqueCaptures, p.Captures!.Count, firstCapture?.FromNumber, latestCapture?.FromNumber, new LeaderboardPlayerPeriodsApiModel
                    {
                        UniqueCapturesWithin1Hour = p.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(1)).GroupBy(c => c.FromNumber).Count(),
                        UniqueCapturesWithin12Hours = p.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(12)).GroupBy(c => c.FromNumber).Count(),
                        UniqueCapturesWithin24Hours = p.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(24)).GroupBy(c => c.FromNumber).Count(),
                        
                        TotalCapturesWithin1Hour = p.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(1)),
                        TotalCapturesWithin12Hours = p.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(12)),
                        TotalCapturesWithin24Hours = p.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(24))
                    });
                }).ToList()
            });
        }
    }
}