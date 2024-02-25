using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class PaginatorPartialViewModel : PaginationModel
    {
        public string PageSwitchController { get; set; } = null!;
        public string PageSwitchAction { get; set; } = null!;
    }
}