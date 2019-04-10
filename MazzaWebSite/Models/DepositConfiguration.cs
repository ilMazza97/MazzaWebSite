using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class DepositConfiguration : EntityTypeConfiguration<Deposit>
    {
        public DepositConfiguration()
        {
            //TODO
            //HasMany(e => e.UserAccountEntity)
            //    .WithMany(e => e.TelegramAccountEntity)
            //    .HasForeignKey(e => e.TelegramAccountId);
        }
    }
}
