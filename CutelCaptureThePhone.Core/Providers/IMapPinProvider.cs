using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Core.Providers
{
    public interface IMapPinProvider
    {
        Task<bool> ExistsByIdAsync(uint id);
        Task<MapPinModel?> GetByIdAsync(uint id);
        Task<MapPinModel?> GetByNumberAsync(string number);
        Task<List<MapPinModel>> GetAllAsync();
        Task<(List<MapPinModel> Pins, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10);
        Task<MapPinModel> CreateAsync(MapPinModel model);
        Task<MapPinModel> UpdateAsync(string currentNumber, MapPinModel model);
        Task DeleteAsync(uint id);
    }
}