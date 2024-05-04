using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class WhitelistViewModel
    {
        public List<WhitelistEntryModel> WhitelistEntries { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}