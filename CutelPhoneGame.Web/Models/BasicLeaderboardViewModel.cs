using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class BasicLeaderboardViewModel
    {
        public List<PlayerModel> Players { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}