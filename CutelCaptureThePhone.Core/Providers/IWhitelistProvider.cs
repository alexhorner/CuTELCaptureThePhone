using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Core.Providers
{
    public interface IWhitelistProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<WhitelistEntryModel?> GetByIdAsync(uint id);
        Task<int> GetCountAsync();
        Task<List<WhitelistEntryModel>> GetAllAsync();
        Task<(List<WhitelistEntryModel> WhitelistEntries, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10);
        bool MatchesNumberAsync(string number);
        Task<WhitelistEntryModel> CreateAsync(WhitelistEntryModel model);
        Task DeleteAsync(uint id);
    }
}