using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("culture")]
    public class Culture : BaseEntityWithSimpleId
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LanguageName { get; set; }
    }
}