using MazzaWebSite.Models;
using MazzaWebSite.Resources;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Westwind.RazorHosting;
using static MazzaWebSite.Models.Enums;

namespace MazzaWebSite
{
    public class SendEmail : ISendEmail
    {
        public SendEmail() { }

        public void SendEmailFromTemplate(NotificationTemplateTypes notificationTemplateTypeId, EmailEntity emailEntity)
        {
            string emailTo = string.Empty;
            var host = new RazorEngine<RazorTemplateBase>();
            host.AddAssembly("System.Data.dll");

            var template = GetTemplate(notificationTemplateTypeId, emailEntity.Culture);
            object razorContext = GetNotificationRazorContext(notificationTemplateTypeId, emailEntity, out emailTo);
            string compiledId = host.CompileTemplate(template.MessageTemplate);
            string message = host.RenderTemplateFromAssembly(compiledId, razorContext);
            Send(emailTo, template.Subject, message);
        }

        private dynamic GetTemplate(NotificationTemplateTypes notificationTemplateTypeId, string culture)
        {
            using (var dbContext = new MazzaDbContext())
            {
                var cultureid = dbContext.Cultures.FirstOrDefault(c => c.Code.Equals(culture));
                if (cultureid == null)
                    cultureid.Id = 1;
                var template = dbContext.NotificationTemplates.FirstOrDefault(n => n.NotificationTypeId == (int)notificationTemplateTypeId && n.CultureId == 1);

                return template;
            }
        }

        private object GetNotificationRazorContext(NotificationTemplateTypes notificationTemplateTypes, EmailEntity emailEntity, out string emailTo)
        {
            object razorContext=null;
            emailTo = string.Empty;
            switch (notificationTemplateTypes)
            {
                case NotificationTemplateTypes.UserRegistration:
                    emailTo = emailEntity.Affiliate.Email;
                    razorContext = emailEntity;
                    break;
                case NotificationTemplateTypes.NewAffiliateRegistration:
                    emailTo = emailEntity.Referent.Email;
                    razorContext = emailEntity;
                    break;
                case NotificationTemplateTypes.ForgotPassword:
                    emailTo = emailEntity.Affiliate.Email;
                    razorContext = emailEntity;
                    break;
            }
            return razorContext;
        }

        public static void Send(string to, string subject, string message)
        {
            var email = string.Empty;
            var password = string.Empty;
            using (var dbContextContext = new MazzaDbContext())
            {
                email = dbContextContext.Settings.FirstOrDefault(s => s.AttributeKey.Equals("email")).AttributeValue;
                password = dbContextContext.Settings.FirstOrDefault(s => s.AttributeKey.Equals("password")).AttributeValue;
            }
            if (email != null)
            {
                MailMessage mm = new MailMessage(string.Concat(General.OMPTitle.ToUpper(), email), to, subject, message)
                {
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    IsBodyHtml = true
                };

                using (SmtpClient client = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(email, password)
                })

                client.Send(mm);

                
            }
        }

    }
}