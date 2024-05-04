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
    public class ListController(ILogger<ListController> logger, IWhitelistProvider whitelistProvider, IBlacklistProvider blacklistProvider) : Controller
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
    }
}