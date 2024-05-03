﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyTelegramBot.Modules;
using System.Text;
using Telegram.Bot;

namespace MyTelegramBot
{
    internal class Program
    {
        //Входная точка
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) 
                .UseConsoleLifetime() 
                .Build(); 

            Console.WriteLine("Сервис запущен");

            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        //Конфигурация
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6804454204:AAGjSJvJdiI2QxeqIWDN6DdpgUJD4l-W8GY"));
            services.AddHostedService<Bot>();
        }
    }
}
