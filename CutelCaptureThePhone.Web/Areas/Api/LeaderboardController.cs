using CutelCaptureThePhone.Web.Models.Api;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Areas.Api
{
    [Area("Api")]
    public class LeaderboardController(IPlayerProvider playerProvider, IMapPinProvider mapPinProvider) : Controller
    {
        public async Task<IActionResult> Public()
        {
            (List<PlayerModel> Players, PaginationModel Pagination) paginatedPlayers = await playerProvider.GetAllPaginatedAsync(0, 10, true);
            
            PlayerModel? mostUniqueCapturesPlayer = paginatedPlayers.Players.MaxBy(p => p.Captures!.GroupBy(c => c.FromNumber).Count());
            PlayerModel? mostCapturesOverall = paginatedPlayers.Players.MaxBy(p => p.Captures!.Count);

            uint count = 1;
            
            List<LeaderboardStatsPublicPlayerApiModel> playerStats = [];
            
            foreach (PlayerModel player in paginatedPlayers.Players)
            {
                int uniqueCaptures = player.Captures!.GroupBy(c => c.FromNumber).Count();
                    
                CaptureModel? firstCapture = player.Captures!.MinBy(c => c.Created);
                CaptureModel? latestCapture = player.Captures!.MaxBy(c => c.Created);
                    
                MapPinModel? firstCaptureMapPin = firstCapture is null ? null : await mapPinProvider.GetByNumberAsync(firstCapture.FromNumber);
                MapPinModel? latestCaptureMapPin = latestCapture is null ? null : await mapPinProvider.GetByNumberAsync(latestCapture.FromNumber);
                    
                string? firstCaptureText = firstCaptureMapPin is not null ? firstCaptureMapPin.Name : firstCapture?.FromNumber;
                string? latestCaptureText = latestCaptureMapPin is not null ? latestCaptureMapPin.Name : latestCapture?.FromNumber;
                
                playerStats.Add(LeaderboardStatsPublicPlayerApiModel.FromModel(player, count++, uniqueCaptures, player.Captures!.Count, firstCaptureText, latestCaptureText, new LeaderboardPlayerPeriodsApiModel
                {
                    UniqueCapturesWithin1Hour = player.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(1)).GroupBy(c => c.FromNumber).Count(),
                    UniqueCapturesWithin12Hours = player.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(12)).GroupBy(c => c.FromNumber).Count(),
                    UniqueCapturesWithin24Hours = player.Captures!.Where(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(24)).GroupBy(c => c.FromNumber).Count(),
                        
                    TotalCapturesWithin1Hour = player.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(1)),
                    TotalCapturesWithin12Hours = player.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(12)),
                    TotalCapturesWithin24Hours = player.Captures!.Count(c => c.Created > DateTime.UtcNow - TimeSpan.FromHours(24))
                }));
            }

            return Ok(new PublicLeaderboardApiModel
            {
                MostUniqueCapturesPlayer = mostUniqueCapturesPlayer is null ? null : PublicPlayerApiModel.FromModel(mostUniqueCapturesPlayer),
                MostCapturesOverallPlayer = mostCapturesOverall is null ? null : PublicPlayerApiModel.FromModel(mostCapturesOverall),
                PlayerStats = playerStats
            });
        }
    }
}