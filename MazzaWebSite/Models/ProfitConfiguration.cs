using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class ProfitConfiguration : EntityTypeConfiguration<Profit>
    {
        public ProfitConfiguration()
        {
            //TODO
            /*builder.HasMany(e => e.UserAccountEntity)
                .WithOne(e => e.TelegramAccountEntity)
                .HasForeignKey(e => e.TelegramAccountId);*/
        }
    }
}
