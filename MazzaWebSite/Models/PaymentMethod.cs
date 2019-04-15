using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("paymentmethod")]
    public class PaymentMethod : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User UserEntity { get; set; }

        public string PaymentValue { get; set; }
    
        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentTypeEntity { get; set; }

    }
}
