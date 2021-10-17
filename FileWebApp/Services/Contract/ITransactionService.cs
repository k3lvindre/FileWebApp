using FileWebApp.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileWebApp.Services
{
    public interface ITransactionService
    {
        Task<bool> UploadTransactionsAsync(List<Transaction> transactions);
    }
}
