﻿using System.Data;
using System.Text.RegularExpressions;
using CutelCaptureThePhone.Core.Enums;
using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CutelCaptureThePhone.Data.Postgres.Providers
{
    public class PostgresBlacklistProvider(CutelCaptureThePhoneDbContext db) : IBlacklistProvider
    {
        public Task<bool> ExistsByIdAsync(uint id) => db.NumberBlacklist.AnyAsync(e => e.Id == id);

        public async Task<BlacklistEntryModel?> GetByIdAsync(uint id)
        {
            BlacklistEntry? entry = await db.NumberBlacklist.SingleOrDefaultAsync(e => e.Id == id);

            return entry?.ToModel();
        }

        public Task<int> GetCountAsync() => db.NumberBlacklist.CountAsync();

        public Task<List<BlacklistEntryModel>> GetAllAsync() => db.NumberBlacklist.Select(e => e.ToModel()).ToListAsync();

        public async Task<(List<BlacklistEntryModel> BlacklistEntries, PaginationModel Pagination)> GetAllPaginatedAsync(int page, int limit = 10)
        {
            int offset = page * limit;

            int totalNumberListEntries = await db.NumberBlacklist.CountAsync();
            
            IQueryable<BlacklistEntry> numberListEntriesQuery = db.NumberBlacklist.OrderBy(e => e.Id).Skip(offset).Take(limit);
            
            int maxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalNumberListEntries) / Convert.ToDouble(limit)) - 1);
            
            if (maxPage < 0) maxPage = 0;
            
            return (await numberListEntriesQuery.Select(e => e.ToModel()).ToListAsync(), new PaginationModel
            {
                CurrentPage = page,
                MaxPage = maxPage,
                TotalItems = totalNumberListEntries
            });
        }

        public bool MatchesNumberAsync(string number)
        {
            bool matches = false;
            
            foreach (BlacklistEntry entry in db.NumberBlacklist)
            {
                switch (entry.Interpretation)
                {
                    case ValueInterpretation.Exact:
                        if (entry.Value == number) matches = true;
                        break;
                    
                    case ValueInterpretation.Prefix:
                        if (number.StartsWith(entry.Value)) matches = true;
                        break;
                    
                    case ValueInterpretation.Suffix:
                        if (number.EndsWith(entry.Value)) matches = true;
                        break;
                    
                    case ValueInterpretation.Regex:
                        if (Regex.IsMatch(number, entry.Value)) matches = true;
                        break;
                }

                if (matches) break;
            }

            return matches;
        }

        public async Task<BlacklistEntryModel> CreateAsync(BlacklistEntryModel model)
        {
            BlacklistEntry newBlacklistEntry = BlacklistEntry.FromModel(model);

            db.NumberBlacklist.Add(newBlacklistEntry);
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key")) throw new DuplicateNameException("A conflicting entry already exists");

                throw;
            }

            return newBlacklistEntry.ToModel();
        }

        public async Task DeleteAsync(uint id)
        {
            await using IDbContextTransaction tran = await db.Database.BeginTransactionAsync();

            try
            {
                BlacklistEntry? numberListEntry = await db.NumberBlacklist.SingleOrDefaultAsync(e => e.Id == id);

                if (numberListEntry is null) throw new KeyNotFoundException($"Unable to find entry with id '{id}'");
            
                db.Remove(numberListEntry);
            
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