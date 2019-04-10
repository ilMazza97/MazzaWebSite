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

        public void SendEmailFromTemplate(NotificationTemplateTypes notificationTemplateTypeId, EmailEntity emailEntity, MazzaDbContext db)
        {
            string emailTo = string.Empty;
            var host = new RazorEngine<RazorTemplateBase>();
            host.AddAssembly("System.Data.dll");

            var template = GetTemplate(notificationTemplateTypeId, emailEntity.Culture, db);
            object razorContext = GetNotificationRazorContext(notificationTemplateTypeId, emailEntity, out emailTo, db);
            string compiledId = host.CompileTemplate(template.MessageTemplate);
            string message = host.RenderTemplateFromAssembly(compiledId, razorContext);
            Send(emailTo, template.Subject, message, db);
        }

        private dynamic GetTemplate(NotificationTemplateTypes notificationTemplateTypeId, string culture, MazzaDbContext db)
        {
            var cultureid=db.Cultures.Where(c=>c.Code.Equals(culture)).FirstOrDefault();
            if (cultureid == null)
                cultureid.Id = 1;
            var template=db.NotificationTemplates.Where(n => n.NotificationTypeId == (int)notificationTemplateTypeId && n.CultureId== 1).FirstOrDefault();

            return template;
        }

        private object GetNotificationRazorContext(NotificationTemplateTypes notificationTemplateTypes, EmailEntity emailEntity, out string emailTo, MazzaDbContext db)
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

        public static void Send(string to, string subject, string message, MazzaDbContext db)
        {
            var email = db.Settings.Where(s => s.AttributeKey.Equals("email")).FirstOrDefault();
            var password = db.Settings.Where(s => s.AttributeKey.Equals("password")).FirstOrDefault();
            if (email != null)
            {
                MailMessage mm = new MailMessage(string.Concat(General.OMPTitle.ToUpper(), email.AttributeValue), to, subject, message)
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
                    Credentials = new System.Net.NetworkCredential(email.AttributeValue, password.AttributeValue)
                })

                client.Send(mm);

                
            }
        }

    }
}