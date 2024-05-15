using System.Data;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace CutelCaptureThePhone.Data.Postgres.Providers
{
    public class PostgresCaptureProvider(CutelCaptureThePhoneDbContext db) : ICaptureProvider
    {
        public Task<bool> ExistsByIdAsync(uint id) => db.Captures.AnyAsync(e => e.Id == id);

        public async Task<CaptureModel?> GetByIdAsync(uint id)
        {
            Capture? capture = await db.Captures.SingleOrDefaultAsync(e => e.Id == id);

            return capture?.ToModel();
        }

        public async Task<CaptureModel?> GetLatestByNumberAsync(string fromNumber)
        {
            Capture? capture = await db.Captures.Where(e => e.FromNumber == fromNumber).OrderByDescending(e => e.Created).FirstOrDefaultAsync();

            return capture?.ToModel();
        }

        public async Task<CaptureModel?> GetLatestByPlayerIdAsync(uint playerId)
        {
            Capture? capture = await db.Captures.Where(e => e.PlayerId == playerId).OrderByDescending(e => e.Created).FirstOrDefaultAsync();

            return capture?.ToModel();
        }

        public async Task<CaptureModel?> GetLatestByPlayerIdAndNumberAsync(uint playerId, string fromNumber)
        {
            Capture? capture = await db.Captures.Where(e => e.PlayerId == playerId && e.FromNumber == fromNumber).OrderByDescending(e => e.Created).FirstOrDefaultAsync();

            return capture?.ToModel();
        }

        public Task<int> GetCountAsync() => db.Captures.CountAsync();

        public Task<int> GetCountByPlayerIdAsync(uint playerId) => db.Captures.CountAsync(e => e.PlayerId == playerId);
        
        public Task<int> GetCountByNumberAsync(string fromNumber) => db.Captures.CountAsync(e => e.FromNumber == fromNumber);

        public Task<int> GetCountByPlayerIdAndNumberAsync(uint playerId, string fromNumber) => db.Captures.CountAsync(e => e.PlayerId == playerId && e.FromNumber == fromNumber);

        //TODO AH: This will be different from count if we ever implement combos
        public Task<int> GetScoreByPlayerIdAsync(uint playerId) => GetCountByPlayerIdAsync(playerId);

        //TODO AH: This will be different from count if we ever implement combos
        public Task<int> GetScoreByPlayerIdAndNumberAsync(uint playerId, string fromNumber) => GetCountByPlayerIdAndNumberAsync(playerId, fromNumber);
        
        public Task<int> GetUniqueCountByPlayerIdAsync(uint playerId) => db.Captures.Where(e => e.PlayerId == playerId).GroupBy(e => e.FromNumber).CountAsync();
        
        public Task<int> GetUniqueCountByNumberAsync(string fromNumber) => db.Captures.Where(e => e.FromNumber == fromNumber).GroupBy(e => e.PlayerId).CountAsync();

        public async Task<PlayerModel?> GetFirstCapturingPlayerByNumberAsync(string fromNumber)
        {
            Player? player = await db.Captures.Where(e => e.FromNumber == fromNumber).OrderBy(e => e.Created).Select(e => e.Player).FirstOrDefaultAsync();

            return player?.ToModel();
        }

        public async Task<PlayerModel?> GetLatestCapturingPlayerByNumberAsync(string fromNumber)
        {
            Player? player = await db.Captures.Where(e => e.FromNumber == fromNumber).OrderByDescending(e => e.Created).Select(e => e.Player).FirstOrDefaultAsync();

            return player?.ToModel();
        }

        public Task<List<CaptureModel>> GetAllAsync() => db.Captures.OrderBy(e => e.Created).Select(e => e.ToModel(true)).ToListAsync();

        public Task<List<CaptureModel>> GetAllByPlayerIdAsync(uint playerId) => db.Captures.Where(e => e.PlayerId == playerId).OrderBy(e => e.Created).Select(e => e.ToModel(true)).ToListAsync();

        public Task<List<CaptureModel>> GetAllByPlayerIdAndNumberAsync(uint playerId, string fromNumber) => db.Captures.Where(e => e.PlayerId == playerId && e.FromNumber == fromNumber).OrderBy(e => e.Created).Select(e => e.ToModel(true)).ToListAsync();

        public async Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10)
        {
            int offset = page * limit;

            int totalCaptures = await db.Captures.CountAsync();
            
            IQueryable<Capture> capturesQuery = db.Captures.OrderBy(e => e.Created).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalCaptures) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await capturesQuery.Select(e => e.ToModel(true)).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalCaptures
            });
        }

        public async Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedByPlayerIdAsync(uint playerId, int page, int limit = 10)
        {
            int offset = page * limit;

            int totalCaptures = await db.Captures.Where(e => e.PlayerId == playerId).CountAsync();
            
            IQueryable<Capture> capturesQuery = db.Captures.Where(e => e.PlayerId == playerId).OrderBy(e => e.Created).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalCaptures) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await capturesQuery.Select(e => e.ToModel(true)).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalCaptures
            });
        }

        public async Task<(List<CaptureModel> Captures, PaginationModel Pagination)> GetAllPaginatedByPlayerIdAndNumberAsync(uint playerId, string fromNumber, int page, int limit = 10)
        {
            int offset = page * limit;

            int totalCaptures = await db.Captures.Where(e => e.PlayerId == playerId && e.FromNumber == fromNumber).CountAsync();
            
            IQueryable<Capture> capturesQuery = db.Captures.Where(e => e.PlayerId == playerId && e.FromNumber == fromNumber).OrderBy(e => e.Created).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalCaptures) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await capturesQuery.Select(e => e.ToModel(true)).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalCaptures
            });
        }

        public async Task<CaptureModel> CreateAsync(CaptureModel model)
        {
            Capture newCapture = Capture.FromModel(model);

            db.Captures.Add(newCapture);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting capture already exists");

                throw;
            }

            return newCapture.ToModel();
        }
    }
}