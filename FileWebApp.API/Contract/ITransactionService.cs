using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileWebApp.API.Services
{
    public interface ITransactionService
    {
        void SaveTransaction();
        void GetByCurrency();
        void GetByDateRange();
        void GetByStatus();
    }
}
