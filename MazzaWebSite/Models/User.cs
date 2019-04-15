using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace MazzaWebSite.Models
{
    [Table("AspNetUsers")]
    public partial class User : BaseEntity
    {
        public User()
        {
            Deposits = new HashSet<Deposit>();
            TelegramAccounts = new HashSet<TelegramAccount>();
            GoogleAuths = new HashSet<GoogleAuthentication>();
        }
        public int ReferentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }  
        
        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int CountryId { get; set; }

        public DateTime RegisterOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool EmailConfirmed { get; set; }


        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<TelegramAccount> TelegramAccounts { get; set; }
        public virtual ICollection<GoogleAuthentication> GoogleAuths { get; set; }

    }
}
