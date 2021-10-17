using FileWebApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileWebApp.API.Services;

namespace FileWebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private ITransactionService _transactionService { get; }

        public TransactionController(ILogger<TransactionController> logger,
                                          ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpPost("SaveList")]
        public async Task<IActionResult> CreateTransactions([FromBody] List<Transaction> transactions)
        {
            int result = await _transactionService.SaveTransactionsAsync(transactions);
            if (result > 0)
            {
                return Accepted();
            }
            else return BadRequest();
        }
    }
}
