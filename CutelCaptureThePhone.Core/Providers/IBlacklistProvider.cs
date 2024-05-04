using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Core.Providers
{
    public interface IBlacklistProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<BlacklistEntryModel?> GetByIdAsync(uint id);
        Task<int> GetCountAsync();
        Task<List<BlacklistEntryModel>> GetAllAsync();
        Task<(List<BlacklistEntryModel> BlacklistEntries, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10);
        bool MatchesNumberAsync(string number);
        Task<BlacklistEntryModel> CreateAsync(BlacklistEntryModel model);
        Task DeleteAsync(uint id);
    }
}