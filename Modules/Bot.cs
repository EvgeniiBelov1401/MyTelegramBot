using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Polling;
using MyTelegramBot.Controllers;

namespace MyTelegramBot.Modules
{
    internal class Bot:BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private TextMessageController _textMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private DefaultMessageController _defaultMessageController;
        public Bot(ITelegramBotClient telegramClient,
            TextMessageController textMessageController,
            InlineKeyboardController inlineKeyboardController,
            DefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _textMessageController = textMessageController;
            _inlineKeyboardController = inlineKeyboardController;
            _defaultMessageController = defaultMessageController;
        }

        //Метод для обработки обычных событий
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }
            if (update.Type == UpdateType.Message)
            {
                //Подсчет символов в строке
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default: 
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }

        //Метод для обработки ошибок
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }

        //Метод для постоянно активного сервиса
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, 
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }
    }
}
