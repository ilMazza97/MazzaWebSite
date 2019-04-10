using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("job")]
    public class Job : BaseEntity
    {
        public string JobName { get; set; }

        public string JobCronExpression { get; set; }

        public bool Active { get; set; }

    }
}
