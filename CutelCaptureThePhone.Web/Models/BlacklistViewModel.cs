using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class BlacklistViewModel
    {
        public List<BlacklistEntryModel> BlacklistEntries { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}