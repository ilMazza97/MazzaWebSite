using MazzaWebSite.Models;

namespace MazzaWebSite
{
    public interface ISendEmail
    {
        void SendEmailFromTemplate(Enums.NotificationTemplateTypes notificationTemplateTypeId,EmailEntity emailEntity);
    }
}