using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("country")]
    public class Country : BaseEntityWithSimpleId
    {

        public int CurrencyId { get; set; }

        public string Name { get; set; }

        public string ISO2LetterCode { get; set; }

        public string ISO3LetterCode { get; set; }

        public string InternationalCallPrefix { get; set; }

    }
}
