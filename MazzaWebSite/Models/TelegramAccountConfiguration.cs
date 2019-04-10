using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class TelegramAccountConfiguration : EntityTypeConfiguration<TelegramAccount>
    {
        public TelegramAccountConfiguration()
        {
            //HasMany(e => e.UserAccountEntity)
            //    .WithOptional(e => e.TelegramAccountEntity)
            //    .HasForeignKey(e => e.TelegramAccountId);
        }
    }
}
