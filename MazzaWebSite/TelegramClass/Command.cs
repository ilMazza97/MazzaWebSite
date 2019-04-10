using System;
using System.Linq;

namespace MazzaWebSite.TelegramClass
{
    public class Command
    {
        public static string GetCommand(string nome) 
        {
            CommandType side = new CommandType();
            if (!nome.Contains('/'))
                return string.Empty;
            if (side.GetType().GetField(nome.Trim('/')) == null)
                return CommandType.wrongcommmand;
            return nome;
        }
    }
}
