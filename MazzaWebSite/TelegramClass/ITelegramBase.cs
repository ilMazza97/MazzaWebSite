using MazzaWebSite.Models;

namespace MazzaWebSite.TelegramClass
{
    public interface ITelegramBase
    {
        void InsertBot(MazzaDbContext database);
    }
}