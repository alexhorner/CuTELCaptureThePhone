using System.Data;
using CutelCaptureThePhone.Web.Authentication.Attributes;
using CutelCaptureThePhone.Web.Models;
using CutelCaptureThePhone.Core.Enums;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CutelCaptureThePhone.Web.Controllers
{
    [AuthenticatedOnly]
    public class ListController(ILogger<ListController> logger, IWhitelistProvider whitelistProvider, IBlacklistProvider blacklistProvider, IMapPinProvider mapPinProvider) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Whitelist([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<WhitelistEntryModel> WhitelistEntries, PaginationModel Pagination) paginatedWhitelist = await whitelistProvider.GetAllPaginatedAsync(page.Value);
            
            return View(new WhitelistViewModel
            {
                WhitelistEntries = paginatedWhitelist.WhitelistEntries,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedWhitelist.Pagination.CurrentPage,
                    MaxPage = paginatedWhitelist.Pagination.MaxPage,
                    PageSwitchController = nameof(ListController),
                    PageSwitchAction = nameof(Whitelist)
                }
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> WhitelistCreate([FromForm] string? value, [FromForm] ValueInterpretation? interpretation)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                TempData["Error"] = "No value was provided";
                return RedirectToAction("Whitelist");
            }
            
            if (interpretation is null || !Enum.IsDefined(typeof(ValueInterpretation), interpretation))
            {
                TempData["Error"] = "No interpretation was provided";
                return RedirectToAction("Whitelist");
            }

            try
            {
                await whitelistProvider.CreateAsync(new WhitelistEntryModel
                {
                    Value = value,
                    Interpretation = interpretation.Value
                });
            }
            catch (DuplicateNameException)
            {
                TempData["Error"] = "An entry with this value and interpretation combination already exists";
                return RedirectToAction("Whitelist");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to create a new whitelist entry with value '{value}' and interpretation {interpretation.Value}");
                
                TempData["Error"] = "An error occured creating the entry. Please try again";
                return RedirectToAction("Whitelist");
            }

            TempData["Message"] = "The entry has been successfully created";
            return RedirectToAction("Whitelist");
        }
        
        [HttpPost]
        public async Task<IActionResult> WhitelistDelete([FromForm] uint? id)
        {
            if (id is null or 0)
            {
                TempData["Error"] = "No entry was provided";
                return RedirectToAction("Whitelist");
            }
            
            if (!await whitelistProvider.ExistsByIdAsync(id.Value))
            {
                TempData["Error"] = "The specified entry doesn't exist";
                return RedirectToAction("Whitelist");
            }
            
            try
            {
                await whitelistProvider.DeleteAsync(id.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to delete a whitelist entry with id '{id}'");
                
                TempData["Error"] = "An error occured deleting the entry. Please try again";
                return RedirectToAction("Whitelist");
            }

            TempData["Message"] = "The entry has been successfully deleted";
            return RedirectToAction("Whitelist");
        }
        
        [HttpGet]
        public async Task<IActionResult> Blacklist([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<BlacklistEntryModel> BlacklistEntries, PaginationModel Pagination) paginatedWhitelist = await blacklistProvider.GetAllPaginatedAsync(page.Value);
            
            return View(new BlacklistViewModel
            {
                BlacklistEntries = paginatedWhitelist.BlacklistEntries,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedWhitelist.Pagination.CurrentPage,
                    MaxPage = paginatedWhitelist.Pagination.MaxPage,
                    PageSwitchController = nameof(ListController),
                    PageSwitchAction = nameof(Blacklist)
                }
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> BlacklistCreate([FromForm] string? value, [FromForm] ValueInterpretation? interpretation)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                TempData["Error"] = "No value was provided";
                return RedirectToAction("Blacklist");
            }
            
            if (interpretation is null || !Enum.IsDefined(typeof(ValueInterpretation), interpretation))
            {
                TempData["Error"] = "No interpretation was provided";
                return RedirectToAction("Blacklist");
            }

            try
            {
                await blacklistProvider.CreateAsync(new BlacklistEntryModel
                {
                    Value = value,
                    Interpretation = interpretation.Value
                });
            }
            catch (DuplicateNameException)
            {
                TempData["Error"] = "An entry with this value and interpretation combination already exists";
                return RedirectToAction("Blacklist");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to create a new blacklist entry with value '{value}' and interpretation {interpretation.Value}");
                
                TempData["Error"] = "An error occured creating the entry. Please try again";
                return RedirectToAction("Blacklist");
            }

            TempData["Message"] = "The entry has been successfully created";
            return RedirectToAction("Blacklist");
        }
        
        [HttpPost]
        public async Task<IActionResult> BlacklistDelete([FromForm] uint? id)
        {
            if (id is null or 0)
            {
                TempData["Error"] = "No entry was provided";
                return RedirectToAction("Blacklist");
            }
            
            if (!await blacklistProvider.ExistsByIdAsync(id.Value))
            {
                TempData["Error"] = "The specified entry doesn't exist";
                return RedirectToAction("Blacklist");
            }
            
            try
            {
                await blacklistProvider.DeleteAsync(id.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to delete a blacklist entry with id '{id}'");
                
                TempData["Error"] = "An error occured deleting the entry. Please try again";
                return RedirectToAction("Blacklist");
            }

            TempData["Message"] = "The entry has been successfully deleted";
            return RedirectToAction("Blacklist");
        }
        
        [HttpGet]
        public async Task<IActionResult> MapPins([FromQuery] int? page)
        {
            page ??= 0;
            
            if (page < 0) page = 0;

            (List<MapPinModel> Pins, PaginationModel Pagination) paginatedWhitelist = await mapPinProvider.GetAllPaginatedAsync(page.Value);
            
            return View(new MapPinsViewModel
            {
                MapPins = paginatedWhitelist.Pins,
                Pagination = new PaginatorPartialViewModel
                {
                    CurrentPage = paginatedWhitelist.Pagination.CurrentPage,
                    MaxPage = paginatedWhitelist.Pagination.MaxPage,
                    PageSwitchController = nameof(ListController),
                    PageSwitchAction = nameof(MapPins)
                }
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> MapPinCreate([FromForm] string name, [FromForm] string number, [FromForm] decimal latitude, [FromForm] decimal longitude, [FromForm] string? edit)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "No name was provided";
                return RedirectToAction("MapPins");
            }
            
            if (string.IsNullOrWhiteSpace(number))
            {
                TempData["Error"] = "No number was provided";
                return RedirectToAction("MapPins");
            }

            if (latitude == 0 && longitude == 0)
            {
                TempData["Error"] = "The provided latitude and longitude pointed to null island";
                return RedirectToAction("MapPins");
            }

            if (!string.IsNullOrWhiteSpace(edit))
            {
                try
                {
                    await mapPinProvider.UpdateAsync(edit, new MapPinModel
                    {
                        Name = name,
                        Number = number,
                        Lat = latitude,
                        Long = longitude
                    });
                }
                catch (DuplicateNameException)
                {
                    TempData["Error"] = "A map pin with this number already exists";
                    return RedirectToAction("MapPins");
                }
                catch (KeyNotFoundException)
                {
                    TempData["Error"] = "A map pin with the original number could not be found";
                    return RedirectToAction("MapPins");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed to update a map pin with original number {edit} to name '{name}' and number {number} at lat/long {latitude}/{longitude}");
                
                    TempData["Error"] = "An error occured updating the map pin. Please try again";
                    return RedirectToAction("MapPins");
                }
                
                TempData["Message"] = "The map pin has been successfully updated";
            }
            else
            {
                try
                {
                    await mapPinProvider.CreateAsync(new MapPinModel
                    {
                        Name = name,
                        Number = number,
                        Lat = latitude,
                        Long = longitude
                    });
                }
                catch (DuplicateNameException)
                {
                    TempData["Error"] = "A map pin with this number already exists";
                    return RedirectToAction("MapPins");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed to create a new map pin with name '{name}' and number {number} at lat/long {latitude}/{longitude}");
                
                    TempData["Error"] = "An error occured creating the map pin. Please try again";
                    return RedirectToAction("MapPins");
                }
                
                TempData["Message"] = "The map pin has been successfully created";
            }
            
            return RedirectToAction("MapPins");
        }
        
        [HttpPost]
        public async Task<IActionResult> MapPinDelete([FromForm] uint? id)
        {
            if (id is null or 0)
            {
                TempData["Error"] = "No pin was provided";
                return RedirectToAction("MapPins");
            }
            
            if (!await mapPinProvider.ExistsByIdAsync(id.Value))
            {
                TempData["Error"] = "The specified map pin doesn't exist";
                return RedirectToAction("MapPins");
            }
            
            try
            {
                await mapPinProvider.DeleteAsync(id.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to delete a map pin with id '{id}'");
                
                TempData["Error"] = "An error occured deleting the map pin. Please try again";
                return RedirectToAction("MapPins");
            }

            TempData["Message"] = "The map pin has been successfully deleted";
            return RedirectToAction("MapPins");
        }
    }
}