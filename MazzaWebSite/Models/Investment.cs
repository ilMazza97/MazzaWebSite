using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("investment")]
    public class Investment : BaseEntity
    {
        public int Amount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PercentageId { get; set; }
        
    }
}
