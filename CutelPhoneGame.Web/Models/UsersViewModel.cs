using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models
{
    public class UsersViewModel
    {
        public List<UserModel> Users { get; set; } = null!;
        public PaginatorPartialViewModel Pagination { get; set; } = null!;
    }
}