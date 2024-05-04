using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Core.Providers
{
    public interface IPlayerProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<bool> ExistsByPinAsync(uint pin);
        Task<bool> ExistsByNameAsync(string name);
        Task<PlayerModel?> GetByIdAsync(uint id);
        Task<PlayerModel?> GetByPinAsync(uint pin);
        Task<PlayerModel?> GetByNameAsync(string name);
        Task<int> GetCountAsync();
        Task<List<PlayerModel>> GetAllAsync();
        Task<List<uint>> GetAllPinsAsync();
        Task<List<string>> GetAllNamesAsync();
        Task<(List<PlayerModel> Players, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10, bool orderByLeaderboard = false);
        Task<int?> GetLeaderboardPositionAsync(uint id);
        Task<PlayerModel> CreateAsync(PlayerModel model);
        Task DeleteByIdAsync(uint id);
        Task DeleteByPinAsync(uint pin);
    }
}