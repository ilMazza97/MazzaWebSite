using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class WithdrawalConfiguration : EntityTypeConfiguration<Withdrawal>
    {
        public WithdrawalConfiguration()
        {
            //TODO
            /*builder.HasMany(e => e.UserAccountEntity)
                .WithOne(e => e.TelegramAccountEntity)
                .HasForeignKey(e => e.TelegramAccountId);*/
        }
    }
}
