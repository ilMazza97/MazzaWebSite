using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("profit")]
    public class Profit : BaseEntity
    {

        public int UserAccountId { get; set; }

        public int Amount { get; set; }

        public int InvestmentId { get; set; }
    }
}
