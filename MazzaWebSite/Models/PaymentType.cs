using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("paymenttype")]
    public class PaymentType : BaseEntity
    {

        public string Code { get; set; }

        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<Deposit> DepositEntity { get; set; }
    }
}
