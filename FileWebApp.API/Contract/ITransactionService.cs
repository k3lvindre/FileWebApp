using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileWebApp.API.Model;

namespace FileWebApp.API.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetByCurrencyAsync(string code);
        Task<List<Transaction>> GetByDateRange(DateTime from, DateTime to);
        Task<List<Transaction>> GetByStatusAsync(string status);
        Task<int> SaveTransactionAsync(List<Transaction> transactions);
    }
}
