using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("albero")]
    public partial class Albero : BaseEntity
    {
        public int Number { get; set; }
    }
}
