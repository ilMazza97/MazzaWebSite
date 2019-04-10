using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace MazzaWebSite.Models
{
    [Table("telegramaccountgroup")]
    public partial class TelegramAccountGroup : BaseEntity
    {
        public int GroupId { get; set; }

        public int TelegramAccountId { get; set; }

        public virtual TelegramAccount TelegramAccounts { get; set; }


        public DateTime? EnterDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public bool IsEvaluating { get; set; }

    }
}
