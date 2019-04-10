using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace MazzaWebSite.Models
{
    [Table("telegramaccount")]
    public partial class TelegramAccount : BaseEntity 
    {
        public TelegramAccount()
        {
            TelegramAccountGroups = new HashSet<TelegramAccountGroup>();
        }

        public int UserId { get; set; }
        public virtual User UserEntity { get; set; }

        public string TelegramUserName { get; set;}

        public int UserChatId { get; set; }

        public DateTime? InsertDate { get; set; }

        public virtual ICollection<TelegramAccountGroup> TelegramAccountGroups { get; set; }

    }
}
