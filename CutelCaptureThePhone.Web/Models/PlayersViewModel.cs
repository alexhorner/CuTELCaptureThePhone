using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class PlayersViewModel
    {
        public List<PlayerModel> Players { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}