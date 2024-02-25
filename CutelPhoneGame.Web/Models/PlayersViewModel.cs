using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class PlayersViewModel
    {
        public List<PlayerModel> Players { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}