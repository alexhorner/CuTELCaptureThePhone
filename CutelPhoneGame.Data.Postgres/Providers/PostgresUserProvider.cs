using System.Data;
using CutelPhoneGame.Core.Models;
using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace CutelPhoneGame.Data.Postgres.Providers
{
    public class PostgresUserProvider(CutelPhoneGameDbContext db) : IUserProvider
    {
        public Task<bool> ExistsByIdAsync(uint id) => db.Users.AnyAsync(e => e.Id == id);

        public Task<bool> ExistsByUsernameAsync(string username) => db.Users.AnyAsync(e => e.Username == username);

        public async Task<UserModel?> GetByIdAsync(uint id)
        {
            User? user = await db.Users.SingleOrDefaultAsync(e => e.Id == id);

            return user?.ToModel();
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            User? user = await db.Users.SingleOrDefaultAsync(e => e.Username == username);

            return user?.ToModel();
        }

        public Task<int> GetCountAsync() => db.Users.CountAsync();

        public Task<List<UserModel>> GetAllAsync() => db.Users.OrderBy(e => e.Username).Select(e => e.ToModel()).ToListAsync();

        public async Task<(List<UserModel> Users, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10)
        {
            int offset = page * limit;

            int totalUsers = await db.Users.CountAsync();
            
            IQueryable<User> usersQuery = db.Users.OrderBy(e => e.Username).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalUsers) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await usersQuery.Select(e => e.ToModel()).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalUsers
            });
        }

        public async Task<UserModel> CreateAsync(UserModel model)
        {
            User newUser = User.FromModel(model);

            db.Users.Add(newUser);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting user already exists");

                throw;
            }

            return newUser.ToModel();
        }

        public async Task UpdateHashedPasswordAsync(uint id, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentException("hashedPassword cannot be null or whitespace", nameof(hashedPassword));
            
            User? user = await db.Users.SingleOrDefaultAsync(e => e.Id == id);

            if (user is null) throw new KeyNotFoundException($"Unable to find user with id '{id}'");

            user.HashedPassword = hashedPassword;
            
            await db.SaveChangesAsync();
        }
    }
}