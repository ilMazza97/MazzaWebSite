using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("commissionpercentage")]
    public partial class CommissionPercentage : BaseEntity
    {
        public int ReferentId { get; set;}
        public int AffiliateId { get; set; }
        //public virtual User UserAccountEntity { get; set; }

        public int Percentage { get; set; }
    }
}
