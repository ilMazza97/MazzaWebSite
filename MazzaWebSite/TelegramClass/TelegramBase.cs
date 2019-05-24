using MazzaWebSite.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web.Mvc;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MazzaWebSite.TelegramClass
{
    public class TelegramBase : Controller, ITelegramBase
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient(ConfigurationManager.AppSettings["TelegramBotKey"]);

        private readonly List<int> messageToRemove = new List<int>();

        public TelegramBase()
        {

        }
        public void InsertBot()
        {
            Bot.StartReceiving();
            Bot.OnMessage += StartMessage;
            Bot.OnMessageEdited += StartMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;
        }

        private void StartMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Chat.Type == ChatType.Private)
                {
                    if (Login(e))
                    {
                        switch (Command.GetCommand(e.Message.Text.ToLower()))
                        {
                            case CommandType.start:
                            case CommandType.home:
                                Button(e);
                                break;
                            case CommandType.wrongcommmand:
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.WrongCommand);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (e.Message.NewChatMembers != null || e.Message.LeftChatMember != null)
                {
                    ControlGroup(e);
                }
            }
            catch (Exception ex)
            {
                SendEmail.Send("lmazzaferro6@gmail.com", "Errore Telegram Bot", ex.Message + ex.InnerException + ex.Data + ex.Source);
            }
        }

        private bool Login(MessageEventArgs e)
        {
            using (var dbContext = new MazzaDbContext())
            {
                if (dbContext.TelegramAccounts.Any(u => u.UserChatId == e.Message.Chat.Id))
                    return true;

                if (e.Message.Chat.Username == string.Empty)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.UserNameEmpty);
                    return false;
                }

                messageToRemove.Add(Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.DoLogin).Result.MessageId);
                messageToRemove.Add(Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.InsertUsername).Result.MessageId);
                var userName = GetText(e);
                if (dbContext.TelegramAccounts.Any(t => t.TelegramUserName.Equals(userName)))
                {
                    SendEmail.Send("lmazzaferro6@gmail.com", "Furbetto " + userName, e.Message.From.Id + e.Message.From.Username);
                    return false;
                }
                while (!dbContext.Users.Any(u => u.UserName.Equals(userName)))
                {
                    messageToRemove.Add(Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.InvalidUsername).Result.MessageId);
                    userName = GetText(e);
                }

                //messageToRemove.Add(Bot.SendTextMessageAsync(e.Message.Chat.Id, "Insert your password, please").Result.MessageId);
                //var password = GetText(e);

                var result = true;// SignInManager.PasswordSignInAsync(userName, password,false,false);
                if (result)
                {
                    var userAccountId = dbContext.Users.FirstOrDefault(u => u.UserName.Equals(userName)).Id;
                    dbContext.TelegramAccounts.Add(new TelegramAccount
                    {
                        TelegramUserName = e.Message.Chat.Username,
                        UserChatId = (int)e.Message.Chat.Id,
                        UserId = userAccountId,
                        InsertDate = DateTime.UtcNow
                    });
                    dbContext.SaveChanges();
                }
            }
            Welcome(e);
            return true;
        }

        private void Welcome(MessageEventArgs e)
        {
            foreach (var messageid in messageToRemove)
            {
                Bot.DeleteMessageAsync(e.Message.Chat.Id, messageid);
            }
            Bot.SendTextMessageAsync(e.Message.Chat.Id, string.Format(Resources.Bot.Welcome, e.Message.Chat.Username));
            Thread.Sleep(1000);
        }

        private string GetText(MessageEventArgs e)
        {
            var pippo = true;
            var date = DateTime.UtcNow;
            while (pippo && !(DateTime.UtcNow.Subtract(date).TotalMinutes > 1))
            {
                var update = Bot.GetUpdatesAsync(0).Result.LastOrDefault(u => u.Message.Date > date && u.Message.Chat.Id == e.Message.Chat.Id);
                if (update != null)
                {
                    messageToRemove.Add(update.Message.MessageId);
                    return update.Message.Text;
                }
            }
            return "";
        }

        private static void Button(MessageEventArgs e)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(Resources.Bot.Info),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(Resources.Bot.Deposit),
                    InlineKeyboardButton.WithCallbackData(Resources.Bot.Settings),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(Resources.Bot.Groups)

                }
            });
            Bot.SendTextMessageAsync(e.Message.Chat.Id, Resources.Bot.ChooseOption, replyMarkup: inlineKeyboard);
        }

        private static void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            switch (GetResources(e.CallbackQuery.Data))
            {
                case ButtonType.Info:
                    Info(e);
                    Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "In fase di costruzione");
                    break;
                case ButtonType.Deposit:
                    Deposit(e);
                    Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "In fase di costruzione");
                    break;
                case ButtonType.Settings:
                    Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "In fase di costruzione");
                    break;
                case ButtonType.Groups:
                    Groupmethod(e);
                    break;
            }
        }

        private static void Groupmethod(CallbackQueryEventArgs e)
        {
            List<InlineKeyboardButton> urlbuttons = new List<InlineKeyboardButton>();
            using (var dbContext = new MazzaDbContext())
            {
                foreach (var group in dbContext.TelegramGroups.Where(t => t.IsActive == true).ToList())
                {
                    urlbuttons.Add(new InlineKeyboardButton { Text = group.Title, Url = group.InvitationLink });
                }
            }
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                {
                       new []
                       {
                           urlbuttons[0]
                       },
                       new []
                       {
                           urlbuttons[1],
                           urlbuttons[2],
                       }
                    });
            Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, Resources.Bot.EnterGroup, replyMarkup: inlineKeyboard);
        }

        private static void Info(CallbackQueryEventArgs e)
        {/*
            ReportDocument rpt = new ReportDocument();
            rpt.SetParameterValue("@pUserAccountId", 0);
            string reportFileName = Path.Combine("~/../../../Report/NewFolder1/", DateTime.Now.ToString("yyyyMMddHHmm") + "_" + e.CallbackQuery.Message.Chat.Username + ".pdf");
            string png_filename = Path.Combine("~/../../../Report/NewFolder2/", DateTime.Now.ToString("yyyyMMddHHmm") + "_" + e.CallbackQuery.Message.Chat.Username + ".png");
            rpt.Load("~/../../../Report/CrystalReport1.rpt");
            //rpt.SetDataSource(datatablesource);
            CrystalDecisions.Shared.ExportOptions rptExportOption;
            DiskFileDestinationOptions rptFileDestOption = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions rptFormatOption = new PdfRtfWordFormatOptions();
            rptFileDestOption.DiskFileName = reportFileName;
            rptExportOption = rpt.ExportOptions;
            {
                rptExportOption.ExportDestinationType = ExportDestinationType.DiskFile;
                rptExportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                rptExportOption.ExportDestinationOptions = rptFileDestOption;
                rptExportOption.ExportFormatOptions = rptFormatOption;
            }
            rpt.Export();
            cs_pdf_to_image.Pdf2Image.PrintQuality = 100;
            cs_pdf_to_image.Pdf2Image.Convert(reportFileName, png_filename);
            var stream = new FileStream(png_filename, FileMode.Open);
            Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, stream, "");
            File.Delete(reportFileName);
            //File.Delete(png_filename);
            */
        }

        private static void Deposit(CallbackQueryEventArgs e)
        {
            //var FileUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Image\deposit.png");
            ////var stream = new FileStream(FileUrl, FileMode.Open, FileAccess.Read)
            //using (FileStream stream = File.Open(FileUrl, FileMode.Open, FileAccess.Read))
            //{
            //    Bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, stream, "1Gk3THiKweoH5n2Zd4GssUHJGxMcDXrL9D");
            //}
        }

        internal static string GetResources(string value)
        {
            ResourceManager rm = new ResourceManager("MazzaWebSite.Resources.Bot", Assembly.GetExecutingAssembly());
            var entry =
                rm.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(a => a.Value.ToString().Equals(value));

            return entry.Key.ToString();
        }

        private void ControlGroup(MessageEventArgs e)
        {
            using (var dbContext = new MazzaDbContext())
            {
                if (e.Message.NewChatMembers != null)
                {
                    var NewChatMembers = e.Message.NewChatMembers.FirstOrDefault().Username;

                    if (dbContext.TelegramAccounts.Any(u => u.TelegramUserName.Equals(NewChatMembers)) && !dbContext.Deposits.FirstOrDefault().UserEntity.TelegramAccounts.Any(t => t.TelegramUserName.Equals(NewChatMembers)))
                    {
                        dbContext.TelegramAccountGroups.Add(new TelegramAccountGroup
                        {
                            GroupId = dbContext.TelegramGroups.FirstOrDefault(t => t.ChatId == e.Message.Chat.Id).Id,
                            TelegramAccountId = dbContext.TelegramAccounts.FirstOrDefault(t => t.TelegramUserName.Equals(NewChatMembers)).Id,
                            EnterDate = DateTime.UtcNow,
                            IsEvaluating = true
                        });
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        Bot.KickChatMemberAsync(e.Message.Chat.Id, e.Message.NewChatMembers.First().Id);
                    }
                }
                else if (e.Message.LeftChatMember != null)
                {
                    var result = dbContext.TelegramAccountGroups.SingleOrDefault(t => t.TelegramAccounts.TelegramUserName.Equals(e.Message.LeftChatMember.Username));
                    result.LeaveDate = DateTime.UtcNow;
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
