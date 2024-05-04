using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using MyTelegramBot.Configuration;
using MyTelegramBot.Services;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).Exercise = callbackQuery.Data;

            string exerciseText = callbackQuery.Data switch
            {
                "countChars" => "Счет символов в строке",
                "sumInt" => "Сумма целых чисел",
                _ => String.Empty
            };

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбранная функция - {exerciseText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
            if (exerciseText == "Счет символов в строке") TextMessageController.command = "countChars";
            else if (exerciseText == "Сумма целых чисел") TextMessageController.command = "sumInt";
        }        
    }
}
