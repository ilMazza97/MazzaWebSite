using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("wallettype")]
    public class WalletType : BaseEntity
    {
        public string Type { get; set; }

    }
}