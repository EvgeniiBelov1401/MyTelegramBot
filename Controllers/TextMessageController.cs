using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using MyTelegramBot.Configuration;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MyTelegramBot.Utilities;
using MyTelegramBot.Services;

namespace MyTelegramBot.Controllers
{
    internal class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        public static string command;

        public TextMessageController(ITelegramBotClient telegramBotClient,IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Счет символов в строке", $"countChars"),
                        InlineKeyboardButton.WithCallbackData($"Сумма целых чисел" , $"sumInt")
                    });

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Бот имеет 2 функции:</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}<b>1.</b> <i>Считает количество символов в тексте.</i>" +
                        $"{Environment.NewLine}<b>2.</b> <i>Суммирует целые числа, записанные через пробел.</i>{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:                 
                break;
                case "/about":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"MyBotByBelovBot {Environment.NewLine}" +
                        $"{Environment.NewLine}Бот создан в образовательных целях." +
                        $"{Environment.NewLine}Автор: Белов Евгений Александрович.{Environment.NewLine}");
                    break;
            }
            switch(command)
            {
                case "countChars":
                    await _telegramClient.SendTextMessageAsync(message.From.Id,Calculate.CalcLength(message.Text));
                    break;
                case "sumInt":
                    await _telegramClient.SendTextMessageAsync(message.From.Id, Calculate.CalcSum(message.Text));
                    break;
                    default : 
                    break;
            }            
        }
    }
}
