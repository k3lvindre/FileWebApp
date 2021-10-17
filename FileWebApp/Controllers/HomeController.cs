using CsvHelper;
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
        private readonly long megabyteConversionValue = 1024 * 1024; //we should put static variables to config class


        public HomeController(ILogger<HomeController> logger
                              ,ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            bool uploaded = false;
            string message = string.Empty;
            try
            {
                if (uploadedFile.Length > 0)
                {
                    double fileSizeInMb = Math.Round((double)uploadedFile.Length / megabyteConversionValue, 2);
                    
                    if (fileSizeInMb <= 1)
                    {
                        List<Transaction> transactions = new List<Transaction>();
                        string fileExtension = Path.GetExtension(uploadedFile.FileName);
                        string tempPath = Path.GetTempFileName();

                        using (var stream = System.IO.File.Create(tempPath))
                        {
                            await uploadedFile.CopyToAsync(stream);
                        }

                        //we can separate the convertion to single abstraction with generic type constraint
                        //but for simplicity I intend to do all the logic here
                        if (fileExtension == ".xml")
                        {
                            XDocument xDoc = XDocument.Load(tempPath);

                            transactions = xDoc.Descendants("transactions").Descendants("transaction")
                                .Select(transaction => new Transaction
                                {
                                    TransactionId = Convert.ToInt32(transaction.Attribute("id").Value.Replace("inv", "")), //we should put static variables to config class
                                    TransactionDate = Convert.ToDateTime(transaction.Element("transactiondate").Value),
                                    Status = transaction.Element("status").Value,
                                    Amount = Convert.ToDecimal(transaction.Element("paymentdetails").Element("amount").Value),
                                    CurrencyCode = transaction.Element("paymentdetails").Element("currencycode").Value
                                }).ToList();

                        }
                        else if (fileExtension == ".csv")
                        {
                            using (var reader = new StreamReader(tempPath))
                            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                transactions = csv.GetRecords<TransactionCsvModel>().Select(x=> new Transaction()
                                {
                                    TransactionId = Convert.ToInt32(x.TransactionId.Replace("inv", "")),
                                    Amount = Convert.ToDecimal(x.Amount),
                                    CurrencyCode = x.CurrencyCode,
                                    Status = x.Status,
                                    TransactionDate = Convert.ToDateTime(x.TransactionDate)
                                }).ToList();
                            }
                        }
                        //we can add custom predicate to filter for items that doesn't contains null
                        //properties and if it doesn't return any item we can return diff.
                        //message for badrequest
                        uploaded = await _transactionService.UploadTransactionsAsync(transactions);
                    }
                    else
                    {
                        message = "File size must not exceed 1 mb.";
                    }
                }
                else message = "File has no content.";
            }
            catch (Exception ex) //exception are hard to trap with specific exception type so we used the source but these can be improve with custom exception
            {
                _logger.LogInformation(ex.Message);
                string source = ex.Source;

                if(source.ToLower().Contains("csv"))
                {
                    message = "Csv is not in correct format.";
                } 
                else if (source.ToLower().Contains("xml"))
                {
                    message = "XML is not in correct format. Make sure that it matches the template";
                }
                else if(source.ToLower().Contains("http"))
                {
                    message = "Check your api config or check your connection.";
                }
                else if(ex is NullReferenceException)
                {
                    message = "check xml element names.";
                }
                else
                {
                    message = "Unknown error";
                }
            }

            if (string.IsNullOrEmpty(message))
            {
                return Ok("Uploaded.");
            }
            else return BadRequest(message);
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
