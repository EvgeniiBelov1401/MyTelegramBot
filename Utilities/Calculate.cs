using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.Utilities
{
    internal static class Calculate
    {
        public static string Calc(string inputText)
        {
            int sum = 0;
            string[] numbers = inputText.Split(' ');
            try
            {
                foreach (var item in numbers)
                {
                    int num;
                    if (int.TryParse(item, out num))
                    {
                        sum += num;
                    }
                    else throw new FormatException("Не допустимое значение");
                }
                return $"Cумма чисел: {sum.ToString()}";
            }
            catch (FormatException ex)
            {
                return ex.Message;
            }
        }
    }
}
