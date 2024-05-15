using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Core.Providers
{
    public interface ICaptureProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<CaptureModel?> GetByIdAsync(uint id);
        Task<CaptureModel?> GetLatestByNumberAsync(string fromNumber);
        Task<CaptureModel?> GetLatestByPlayerIdAsync(uint playerId);
        Task<CaptureModel?> GetLatestByPlayerIdAndNumberAsync(uint playerId, string fromNumber);
        Task<int> GetCountAsync();
        Task<int> GetCountByPlayerIdAsync(uint playerId);
        Task<int> GetCountByNumberAsync(string fromNumber);
        Task<int> GetCountByPlayerIdAndNumberAsync(uint playerId, string fromNumber);
        Task<int> GetScoreByPlayerIdAsync(uint playerId);
        Task<int> GetScoreByPlayerIdAndNumberAsync(uint playerId, string fromNumber);
        Task<int> GetUniqueCountByPlayerIdAsync(uint playerId);
        Task<int> GetUniqueCountByNumberAsync(string fromNumber);
        Task<PlayerModel?> GetFirstCapturingPlayerByNumberAsync(string fromNumber);
        Task<PlayerModel?> GetLatestCapturingPlayerByNumberAsync(string fromNumber);
        Task<List<CaptureModel>> GetAllAsync();
        Task<List<CaptureModel>> GetAllByPlayerIdAsync(uint playerId);
        Task<List<CaptureModel>> GetAllByPlayerIdAndNumberAsync(uint playerId, string fromNumber);
        Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10);
        Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedByPlayerIdAsync(uint playerId, int page, int limit = 10);
        Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedByPlayerIdAndNumberAsync(uint playerId, string fromNumber, int page, int limit = 10);
        Task<CaptureModel> CreateAsync(CaptureModel model);
    }
}