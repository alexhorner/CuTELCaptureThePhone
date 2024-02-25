using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Core.Providers
{
    public interface IUserProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<UserModel?> GetByIdAsync(uint id);
        Task<UserModel?> GetByUsernameAsync(string username);
        Task<int> GetCountAsync();
        Task<List<UserModel>> GetAllAsync();
        Task<(List<UserModel> Users, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10);
        Task<UserModel> CreateAsync(UserModel model);
        Task UpdateHashedPasswordAsync(uint id, string hashedPassword);
    }
}