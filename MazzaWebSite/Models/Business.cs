using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("business")]
    public class Business : BaseEntity
    {
        public string BusinessName { get; set; }

        public bool IsActive { get; set; }
    }
}
