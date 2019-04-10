using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class InvestmentConfiguration : EntityTypeConfiguration<Investment>
    {
        public InvestmentConfiguration()
        {
            //TODO
            /*builder.HasMany(e => e.UserAccountEntity)
                .WithOne(e => e.TelegramAccountEntity)
                .HasForeignKey(e => e.TelegramAccountId);*/
        }
    }
}
