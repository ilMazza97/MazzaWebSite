using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("deposit")]
    public class Deposit : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User UserEntity { get; set; }

        public int BusinessId { get; set; }

        public decimal Amount { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int PaymentTypeId { get; set; }
        //public virtual PaymentType PaymentTypeEntity { get; set; }

        public DateTime DepositDate { get; set; }
        

    }
}
