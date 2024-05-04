using System.Text.RegularExpressions;
using CutelCaptureThePhone.Core;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Web.ApiAuthentication.Attributes;
using CutelCaptureThePhone.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Areas.Api
{
    [Area("Api")]
    [ApiAuthenticatedOnly]
    public class GameController(IConfiguration configuration, IBlacklistProvider blacklistProvider, IWhitelistProvider whitelistProvider, IPlayerProvider playerProvider, ICaptureProvider captureProvider, PlayerUniquePinGenerator playerUniquePinGenerator, PlayerUniqueNamesetGenerator playerUniqueNamesetGenerator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromQuery] string fromNumber)
        {
            //Validate from number
            if (!Regex.IsMatch(fromNumber, "[0-9]+") || blacklistProvider.MatchesNumberAsync(fromNumber) || !whitelistProvider.MatchesNumberAsync(fromNumber)) return Unauthorized(new SimpleResponseModel
            {
                Message = "This number is not allowed to be used in the game"
            });
            
            //Generate unique pin
            ushort tries = 5;
            uint generatedPin = 0;

            while (tries != 0)
            {
                tries--;

                generatedPin = playerUniquePinGenerator.GenerateNewPin();

                if (!await playerProvider.ExistsByPinAsync(generatedPin)) break;
            }

            if (tries == 0) return StatusCode(500, new SimpleResponseModel
            {
                Message = "Failed to generate a pin"
            });
            
            //Generate unique name
            tries = 5;
            string generatedName = "";
            
            while (tries != 0)
            {
                tries--;

                generatedName = playerUniqueNamesetGenerator.GenerateNewName();

                if (!await playerProvider.ExistsByNameAsync(generatedName)) break;
            }

            if (tries == 0) return StatusCode(500, new SimpleResponseModel
            {
                Message = "Failed to generate a name"
            });
            
            //Create the player
            PlayerModel createdPlayer;
            
            try
            {
                createdPlayer = await playerProvider.CreateAsync(new PlayerModel
                {
                    Pin = generatedPin,
                    Name = generatedName,
                    RegisteredFromNumber = fromNumber
                });
            }
            catch
            {
                playerUniquePinGenerator.ReleaseFailedPin(generatedPin);
                throw;
            }

            string[] nameParts = generatedName.ToLower().Split(' ');

            return Ok(new RegisteredPlayerApiModel
            {
                Pin = createdPlayer.Pin,
                NamePartA = nameParts[0],
                NamePartB = nameParts[1],
                NamePartC = nameParts[2]
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
            
            string[] nameParts = player.Name.ToLower().Split(' ');
            
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
                        NamePartA = nameParts[0],
                        NamePartB = nameParts[1],
                        NamePartC = nameParts[2],
                        WaitHours = (uint)Math.Floor(waitHours),
                        WaitMinutes = (uint)waitMinutes,
                        WaitSeconds = (uint)waitSeconds,
                        PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                        PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                        PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id) + 1 //We add 1 as this is the 1 indexed position, even though we store it as 0
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
                        NamePartA = nameParts[0],
                        NamePartB = nameParts[1],
                        NamePartC = nameParts[2],
                        WaitHours = (uint)Math.Floor(waitHours),
                        WaitMinutes = (uint)waitMinutes,
                        WaitSeconds = (uint)waitSeconds,
                        PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                        PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                        PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id) + 1 //We add 1 as this is the 1 indexed position, even though we store it as 0
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
                NamePartA = nameParts[0],
                NamePartB = nameParts[1],
                NamePartC = nameParts[2],
                WaitHours = 0,
                WaitMinutes = 0,
                WaitSeconds = 0,
                PlayerTotalCaptures = await captureProvider.GetCountByPlayerIdAsync(player.Id),
                PlayerUniqueCaptures = await captureProvider.GetUniqueCountByPlayerIdAsync(player.Id),
                PlayerLeaderboardPosition = await playerProvider.GetLeaderboardPositionAsync(player.Id) + 1 //We add 1 as this is the 1 indexed position, even though we store it as 0
            });
        }
    }
}