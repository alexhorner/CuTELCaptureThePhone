using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class WhitelistViewModel
    {
        public List<WhitelistEntryModel> WhitelistEntries { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}