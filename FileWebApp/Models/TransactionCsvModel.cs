using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileWebApp.Models
{
    public class TransactionCsvModel
    {
        [Name("transactionid")]
        public string TransactionId { get; set; }
        [Name("transactiondate")]
        public string TransactionDate { get; set; }
        [Name("status")]
        public string Status { get; set; }
        [Name("amount")]
        public string Amount { get; set; }
        [Name("currencycode")]
        public string CurrencyCode { get; set; }
    }
}
