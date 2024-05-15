using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class MapPinsViewModel
    {
        public List<MapPinModel> MapPins { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}