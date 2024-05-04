using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class BasicLeaderboardViewModel
    {
        public List<PlayerModel> Players { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}