using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("percentage")]
    public class Percentage : BaseEntity
    {

        public int StartRange { get; set; }

        public int EndRange { get; set; }

        public int PercentageValue { get; set; }

    }
}
