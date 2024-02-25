using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Core.Providers
{
    public interface IPlayerProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<bool> ExistsByPinAsync(uint pin);
        Task<PlayerModel?> GetByIdAsync(uint id);
        Task<PlayerModel?> GetByPinAsync(uint pin);
        Task<int> GetCountAsync();
        Task<List<PlayerModel>> GetAllAsync();
        Task<(List<PlayerModel> Players, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10, bool orderByLeaderboard = false);
        Task<int?> GetLeaderboardPositionAsync(uint id);
        Task<PlayerModel> CreateAsync(PlayerModel model);
        Task DeleteByIdAsync(uint id);
        Task DeleteByPinAsync(uint pin);
    }
}