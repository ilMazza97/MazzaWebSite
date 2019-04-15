using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazzaWebSite.Models
{
    [Table("googleauthentication")]
    public class GoogleAuthentication : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User UserEntity { get; set; }

        public string QrCodeUrl { get; set; }

        public string ManualEntryKey { get; set; }

        public string AccountSecretKey { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
