using FileWebApp.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FileWebApp.API.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;

        public FileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task UploadFileAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
