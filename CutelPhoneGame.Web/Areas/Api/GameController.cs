using System.Text.RegularExpressions;
using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Web.ApiAuthentication.Attributes;
using CutelPhoneGame.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace CutelPhoneGame.Web.Areas.Api
{
    [Area("Api")]
    [ApiAuthenticatedOnly]
    public class GameController(IConfiguration configuration, IBlacklistProvider blacklistProvider, IWhitelistProvider whitelistProvider, IPlayerProvider playerProvider, ICaptureProvider captureProvider) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromQuery] string fromNumber)
        {
            //Validate from number
            if (!Regex.IsMatch(fromNumber, "[0-9]+") || blacklistProvider.MatchesNumberAsync(fromNumber) || !whitelistProvider.MatchesNumberAsync(fromNumber)) return Unauthorized(new SimpleResponseModel
            {
                Message = "This number is not allowed to be used in the game"
            });
            
            //Generate maximum possible pin
            int pinLength = configuration.GetValue<int>("PinLength");
            
            string maxPin = "";
            
            for (int i = 0; i < pinLength; i++) maxPin += "9";
            
            //Generate valid pins until we hit an available one, then create the player and send back the result
            ushort tries = 100;
            uint generatedPin = 0;

            while (tries != 0) //TODO AH: This is a crap implementation which will get slower, more resource intensive and eventually break as more people register
            {
                tries--;
                
                generatedPin = (uint) Random.Shared.Next(1, int.Parse(maxPin));

                PlayerModel? existingPlayer = await playerProvider.GetByPinAsync(generatedPin);

                if (existingPlayer is null) break;
            }

            if (tries == 0) return StatusCode(500, new SimpleResponseModel
            {
                Message = "Failed to generate a pin"
            });
            
            PlayerModel createdPlayer = await playerProvider.CreateAsync(new PlayerModel
            {
                Pin = generatedPin,
                RegisteredFromNumber = fromNumber
            });

            return Ok(new RegisteredPlayerApiModel
            {
                Pin = createdPlayer.Pin
            });
        }

        [HttpPost]
        public async Task<IActionResult> Capture([FromQuery] string fromNumber, [FromQuery] uint pin)
        {
            //Validate from number
            if (!Regex.IsMatch(fromNumber, "[0-9]+") || blacklistProvider.MatchesNumberAsync(fromNumber) || !whitelistProvider.MatchesNumberAsync(fromNumber)) return Unauthorized(new SimpleResponseModel
            {
                Message = "This number is not allowed to be used in the game"
            });
            
            //Retrieve player by pin
            PlayerModel? player = await playerProvider.GetByPinAsync(pin);

            if (player is null) return BadRequest(new SimpleResponseModel
            {
                Message = "This pin is invalid"
            });
            
            //See if player has captured this number already within the player timer
            DateTime now = DateTime.UtcNow;
            
            CaptureModel? latestPlayerNumberCapture = await captureProvider.GetLatestByPlayerIdAndNumberAsync(player.Id, fromNumber);

            if (latestPlayerNumberCapture is not null)
            {
                DateTime nextAllowedPlayerNumberCapture = latestPlayerNumberCapture.Created + TimeSpan.FromSeconds(configuration.GetSection("CaptureDelays").GetValue<uint>("PlayerTimerSeconds"));

                if (nextAllowedPlayerNumberCapture > now)
                {
                    TimeSpan waitTime = nextAllowedPlayerNumberCapture - now;
                    
                    double waitHours = waitTime.TotalHours;
                    double waitMinutes = waitHours * 60 % 60;
                    double waitSeconds = waitHours * 3600 % 60;
                    
                    return Ok(new CapturedApiModel
                    {
                        Captured = false,
                        WaitHours = (uint)Math.Floor(waitHours),
                        WaitMinutes = (uint)waitMinutes,
                        WaitSeconds = (uint)waitSeconds,
                        PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                        PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                        PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id)
                    });
                }
            }
            
            //Get the most recent capture for this number and verify the user can capture it right now within the number timer
            CaptureModel? latestNumberCapture = await captureProvider.GetLatestByNumberAsync(fromNumber);

            if (latestNumberCapture is not null)
            {
                DateTime nextAllowedNumberCapture = latestNumberCapture.Created + TimeSpan.FromSeconds(configuration.GetSection("CaptureDelays").GetValue<uint>("NumberTimerSeconds"));

                if (nextAllowedNumberCapture > now)
                {
                    TimeSpan waitTime = nextAllowedNumberCapture - now;
                    
                    double waitHours = waitTime.TotalHours;
                    double waitMinutes = waitHours * 60 % 60;
                    double waitSeconds = waitHours * 3600 % 60;
                    
                    return Ok(new CapturedApiModel
                    {
                        Captured = false,
                        WaitHours = (uint)Math.Floor(waitHours),
                        WaitMinutes = (uint)waitMinutes,
                        WaitSeconds = (uint)waitSeconds,
                        PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                        PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                        PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id)
                    });
                }
            }
            
            //Capture the number
            await captureProvider.CreateAsync(new CaptureModel
            {
                PlayerId = player.Id,
                FromNumber = fromNumber
            });

            return Ok(new CapturedApiModel
            {
                Captured = true,
                PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id)
            });
        }
    }
}