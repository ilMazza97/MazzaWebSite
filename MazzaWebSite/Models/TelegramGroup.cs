using System.ComponentModel.DataAnnotations.Schema;
namespace MazzaWebSite.Models
{
    [Table("telegramgroup")]
    public partial class TelegramGroup : BaseEntity
    {
        public long ChatId { get; set; }

        public string Title { get; set; }

        public string InvitationLink { get; set; }

        public bool IsActive { get; set; }

    }
}
