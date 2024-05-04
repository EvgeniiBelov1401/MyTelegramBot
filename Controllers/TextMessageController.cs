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
        private readonly IFunction _chosenFunction;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient,IFunction choosenFunction,IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _chosenFunction = choosenFunction;
            _memoryStorage = memoryStorage;
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
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте нужный текст.", cancellationToken: ct);
                    break;
            }
            

            string userChoosenExercise= _memoryStorage.GetSession(message.Chat.Id).Exercise;
            _chosenFunction.Process(userChoosenExercise,);
        }
    }
}
