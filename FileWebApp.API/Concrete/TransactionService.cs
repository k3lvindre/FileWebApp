using FileWebApp.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileWebApp.API.Data;

namespace FileWebApp.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetByCurrencyAsync(string code) => await _dbContext.Transactions.Where(x => x.CurrencyCode == code).ToListAsync();

        public Task<List<Transaction>> GetByDateRange(DateTime from, DateTime to)
        {
            //sql function
            throw new NotImplementedException();
        }

        public async Task<List<Transaction>> GetByStatusAsync(string status) => await _dbContext.Transactions.Where(x => x.Status == status).ToListAsync();

        public async Task<int> SaveTransactionsAsync(List<Transaction> transactions)
        {
            _dbContext.Transactions.AddRange(transactions);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
