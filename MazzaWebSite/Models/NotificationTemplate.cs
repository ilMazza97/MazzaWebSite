using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("notificationtemplate")]
    public class NotificationTemplate : BaseEntity
    {
        public int NotificationTypeId { get; set; }
        public int CultureId { get; set; }
        public string Subject { get; set; }
        public string MessageTemplate { get; set; }
    }
}