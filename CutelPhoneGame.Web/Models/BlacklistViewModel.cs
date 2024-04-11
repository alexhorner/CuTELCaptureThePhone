using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class BlacklistViewModel
    {
        public List<BlacklistEntryModel> BlacklistEntries { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}