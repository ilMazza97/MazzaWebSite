using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("withdrawal")]
    public class Withdrawal : BaseEntity
    {

        public int UserId { get; set; }
        //public virtual User UserAccountEntity { get; set; }

        public int BusinessId { get; set; }

        public decimal Amount { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentTypeEntity { get; set; }

        public DateTime WithdrawalDate { get; set; }

    }
}
