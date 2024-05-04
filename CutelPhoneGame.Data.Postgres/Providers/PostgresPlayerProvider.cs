using System.Data;
using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace CutelPhoneGame.Data.Postgres.Providers
{
    public class PostgresPlayerProvider(CutelPhoneGameDbContext db) : IPlayerProvider
    {
        public Task<bool> ExistsByIdAsync(uint id) => db.Players.AnyAsync(e => e.Id == id);

        public Task<bool> ExistsByPinAsync(uint pin) => db.Players.AnyAsync(e => e.Pin == pin);
        public Task<bool> ExistsByNameAsync(string name) => db.Players.AnyAsync(e => e.Name.ToLower() == name.ToLower());

        public async Task<PlayerModel?> GetByIdAsync(uint id)
        {
            Player? player = await db.Players.SingleOrDefaultAsync(e => e.Id == id);

            return player?.ToModel();
        }
        
        public async Task<PlayerModel?> GetByPinAsync(uint pin)
        {
            Player? player = await db.Players.SingleOrDefaultAsync(e => e.Pin == pin);

            return player?.ToModel();
        }

        public async Task<PlayerModel?> GetByNameAsync(string name)
        {
            Player? player = await db.Players.SingleOrDefaultAsync(e =>  e.Name.ToLower() == name.ToLower());

            return player?.ToModel();
        }

        public Task<int> GetCountAsync() => db.Players.CountAsync();

        public Task<List<PlayerModel>> GetAllAsync() => db.Players.Include(e => e.Captures).OrderBy(e => e.Pin).Select(e => e.ToModel(false)).ToListAsync();

        public Task<List<uint>> GetAllPinsAsync() => db.Players.Select(e => e.Pin).ToListAsync();

        public Task<List<string>> GetAllNamesAsync() => db.Players.Select(e => e.Name).ToListAsync();

        public async Task<(List<PlayerModel> Players, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10, bool orderByLeaderboard = false)
        {
            int offset = page * limit;

            int totalPlayers = await db.Players.CountAsync();
            
            IQueryable<Player> playersQuery = db.Players.Include(e => e.Captures);

            playersQuery = orderByLeaderboard ? playersQuery.OrderByDescending(e => e.Captures!.Count()) : playersQuery.OrderBy(e => e.Pin);
                
            playersQuery = playersQuery.Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalPlayers) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await playersQuery.Select(e => e.ToModel(false)).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalPlayers
            });
        }

        public async Task<int?> GetLeaderboardPositionAsync(uint id)
        {
            Player? player = await db.Players.SingleOrDefaultAsync(e => e.Id == id);

            if (player is null) return null;
            
            return db.Players.OrderByDescending(e => e.Captures!.Count()).Select(e => e.Id).ToList().IndexOf(player.Id);
        }

        public async Task<PlayerModel> CreateAsync(PlayerModel model)
        {
            Player newPlayer = Player.FromModel(model);

            db.Players.Add(newPlayer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting player already exists");

                throw;
            }

            return newPlayer.ToModel();
        }

        public async Task DeleteByIdAsync(uint id)
        {
            Player? player = await db.Players.Include(e => e.Captures).SingleOrDefaultAsync(e => e.Id == id);

            if (player is null) throw new KeyNotFoundException($"Unable to find player with id '{id}'");

            db.RemoveRange(player.Captures!);
            db.Remove(player);
            
            await db.SaveChangesAsync();
        }

        public async Task DeleteByPinAsync(uint pin)
        {
            Player? player = await db.Players.Include(e => e.Captures).SingleOrDefaultAsync(e => e.Pin == pin);

            if (player is null) throw new KeyNotFoundException($"Unable to find player with pin '{pin}'");

            db.RemoveRange(player.Captures!);
            db.Remove(player);
            
            await db.SaveChangesAsync();
        }
    }
}