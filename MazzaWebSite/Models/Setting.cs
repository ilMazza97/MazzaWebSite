using System.ComponentModel.DataAnnotations.Schema;
namespace MazzaWebSite.Models
{
    [Table("setting")]
        public class Setting : BaseEntity
        {
            public string AttributeKey { get; set; }
            public string AttributeValue { get; set; }
        }
 
}