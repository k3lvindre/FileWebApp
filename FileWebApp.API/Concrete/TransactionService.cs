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

        public CustomerRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       
    }
}
