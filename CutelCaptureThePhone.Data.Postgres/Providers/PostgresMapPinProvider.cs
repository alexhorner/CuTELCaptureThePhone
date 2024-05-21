using System.Data;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CutelCaptureThePhone.Data.Postgres.Providers
{
    public class PostgresMapPinProvider(CutelCaptureThePhoneDbContext db) : IMapPinProvider
    {
        public Task<bool> ExistsByIdAsync(uint id) => db.MapPins.AnyAsync(e => e.Id == id);

        public async Task<MapPinModel?> GetByIdAsync(uint id)
        {
            MapPin? mapPin = await db.MapPins.SingleOrDefaultAsync(e => e.Id == id);

            return mapPin?.ToModel();
        }

        public async Task<MapPinModel?> GetByNumberAsync(string number)
        {
            MapPin? mapPin = await db.MapPins.SingleOrDefaultAsync(e => e.Number == number);

            return mapPin?.ToModel();
        }

        public Task<List<MapPinModel>> GetAllAsync() => db.MapPins.OrderBy(e => e.Created).Select(e => e.ToModel()).ToListAsync();

        public async Task<(List<MapPinModel> Pins, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10)
        {
            int offset = page * limit;

            int totalPins = await db.MapPins.CountAsync();
            
            IQueryable<MapPin> pinsQuery = db.MapPins.OrderBy(e => e.Created).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalPins) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await pinsQuery.Select(e => e.ToModel()).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalPins
            });
        }

        public async Task<MapPinModel> CreateAsync(MapPinModel model)
        {
            MapPin newPin = MapPin.FromModel(model);

            db.MapPins.Add(newPin);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting map pin already exists");

                throw;
            }

            return newPin.ToModel();
        }

        public async Task<MapPinModel> UpdateAsync(string currentNumber, MapPinModel model)
        {
            IDbContextTransaction transaction = await db.Database.BeginTransactionAsync();

            MapPin? existingPin;
            
            try
            {
                existingPin = await db.MapPins.SingleOrDefaultAsync(e => e.Number == currentNumber);

                if (existingPin is null) throw new KeyNotFoundException("the map pin number could not be found");

                existingPin.Lat = model.Lat;
                existingPin.Long = model.Long;
                existingPin.Name = model.Name;
                existingPin.Number = model.Number;

                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting map pin already exists");

                throw;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }

            return existingPin.ToModel();
        }

        public async Task DeleteAsync(uint id)
        {
            await using IDbContextTransaction tran = await db.Database.BeginTransactionAsync();

            try
            {
                MapPin? mapPin = await db.MapPins.SingleOrDefaultAsync(e => e.Id == id);

                if (mapPin is null) throw new KeyNotFoundException($"Unable to find map pin with id '{id}'");
            
                db.Remove(mapPin);
            
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                await tran.RollbackAsync();
                throw;
            }

            await tran.CommitAsync();
        }
    }
}