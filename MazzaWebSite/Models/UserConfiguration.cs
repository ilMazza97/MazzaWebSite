using System.Data.Entity.ModelConfiguration;

namespace MazzaWebSite.Models
{
    public class UserConfiguration  : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            //Da finire
            /*HasMany(e => e.CommissionPercentageEntity)
                .WithOne(e => e.UserAccountEntity)
                .HasForeignKey(e => e.ReferentId);

            /*builder.HasMany(e => e.CommissionPercentageEntity)
                .WithOne(e => e.UserAccountEntity)
                .HasForeignKey(e => e.AffiliateId);*/
            HasMany(e => e.Deposits)
                .WithRequired(e => e.UserEntity)
                .HasForeignKey(e => e.Id)
                .WillCascadeOnDelete(false);
        }
    }
}
