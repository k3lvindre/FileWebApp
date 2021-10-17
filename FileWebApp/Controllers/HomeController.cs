using FileWebApp.API.Model;
using FileWebApp.Models;
using FileWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly long megabyteConversionValue = 1024 * 1024;


        public HomeController(ILogger<HomeController> logger
                              ,ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            try
            {

                if(uploadedFile.Length > 0)
                {
                    double fileSizeInMb = Math.Round((double)uploadedFile.Length / megabyteConversionValue, 2);
                    
                    if (fileSizeInMb <= 1)
                    {
                        string fileExtension = Path.GetExtension(uploadedFile.FileName);

                        //we can separate the convertion to single abstraction with generic type constraint
                        //but for simplicity I intend to do all the logic here
                        if (fileExtension == ".xml")
                        {
                            string tempPath = Path.GetTempFileName();

                            using (var stream = System.IO.File.Create(tempPath))
                            {
                                await uploadedFile.CopyToAsync(stream);
                            }

                            XDocument xDoc = XDocument.Load(tempPath);

                            List<Transaction> transactions = xDoc.Descendants("transactions").Descendants("transaction")
                                .Select(transaction => new Transaction
                                {
                                    TransactionId = Convert.ToInt32(transaction.Attribute("id").Value.Replace("inv", "")),
                                    TransactionDate = Convert.ToDateTime(transaction.Element("transactiondate").Value),
                                    Status = transaction.Element("status").Value,
                                    Amount = Convert.ToDecimal(transaction.Element("paymentdetails").Element("amount").Value),
                                    CurrencyCode = transaction.Element("paymentdetails").Element("currencycode").Value
                                }).ToList();

                            await _transactionService.UploadTransactionsAsync(transactions);
                        }
                        else if (fileExtension == ".csv")
                        {

                        }
                    }
                    else
                    {
                        return BadRequest("File size must not exceed 1 mb.");
                    }
                }
                else return BadRequest("File has no content.");


                return Ok("Uploaded.");
            }
            catch (Exception ex)
            {
                if(ex is NullReferenceException || ex is System.Xml.XmlException)
                {
                    _logger.LogInformation(ex.Message);
                    return BadRequest("XML is not in correct format. Make sure that it matches the template");
                }
                else 
                { 
                    _logger.LogInformation(ex.Message);
                    return BadRequest();
                }
            }
          
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
