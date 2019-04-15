using MySql.Data.Entity;
using System.Data.Entity;

namespace MazzaWebSite.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MazzaDbContext : DbContext
    {
        public MazzaDbContext()
             : base("name=MazzaDbContext")
        { }
        public virtual DbSet<Business> Businesss { get; set; }

        //public virtual DbSet<CommissionPercentage> CommissionPercentages { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<GoogleAuthentication> GoogleAuths { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        //public virtual DbSet<Investment> Investments { get; set; }
        public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        //public virtual DbSet<Percentage> Percentages { get; set; }
        //public virtual DbSet<Profit> Profits { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TelegramAccount> TelegramAccounts { get; set; }
        public virtual DbSet<TelegramAccountGroup> TelegramAccountGroups { get; set; }
        public virtual DbSet<TelegramGroup> TelegramGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Withdrawal> Withdrawals { get; set; }
        //public virtual DbSet<InvestmentReport> InvestmentReports { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
