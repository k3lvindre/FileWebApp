using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileWebApp.API.Model
{
    public class TransactionXmlModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }

    public class PaymentDetails
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
