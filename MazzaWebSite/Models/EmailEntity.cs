namespace MazzaWebSite.Models
{
    public class EmailEntity
    {
        public User Referent { get; set; }

        public ApplicationUser Affiliate { get; set; }

        public string Culture { get; set; }

        public string CallbackUrl { get; set; }

    }
}