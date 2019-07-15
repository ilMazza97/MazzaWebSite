using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("deposittransaction")]
    public class DepositTransaction : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User UserEntity { get; set; }
        public string Address { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string StatusUrl { get; set; }
        public int Timeout { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
