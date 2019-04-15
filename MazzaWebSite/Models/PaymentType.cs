using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("paymenttype")]
    public class PaymentType : BaseEntity
    {
        public PaymentType()
        {
            PaymentMethods = new HashSet<PaymentMethod>();
        }

        public string Code { get; set; }

        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
