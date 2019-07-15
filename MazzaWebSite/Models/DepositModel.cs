using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Models
{
    public class DepositModel
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Coin { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
    }
}
