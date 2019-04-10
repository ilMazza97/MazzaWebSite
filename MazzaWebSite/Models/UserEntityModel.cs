using System.Collections.Generic;

namespace MazzaWebSite.Models
{
    public class UserEntityModel
    {
        public List<User> Users { get; set; }
        public List<Deposit> Deposits { get; set; }
        public List<Withdrawal> Withdrawals { get; set; }
        public List<string> InstagramUrlImage { get; set; }
    }
}