using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models
{
    public class UsersViewModel
    {
        public List<UserModel> Users { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}