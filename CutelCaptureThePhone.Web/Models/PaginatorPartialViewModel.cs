using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class PaginatorPartialViewModel : PaginationModel
    {
        public string PageSwitchController { get; set; } = null!;
        public string PageSwitchAction { get; set; } = null!;
    }
}