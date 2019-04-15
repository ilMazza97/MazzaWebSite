namespace MazzaWebSite.Models
{
    public class Enums
    {
        public enum NotificationTemplateTypes
        {
            UserRegistration = 1,
            NewAffiliateRegistration = 2,
            ForgotPassword = 3,
        }
        public const string Success = "success";
        public const string Danger = "danger";

        //Language
        public const string Italiano = "italiano";
        public const string Inglese = "english";
        public const string Tedesco = "deutsch";
        public const string Francese = "français";
    }
}