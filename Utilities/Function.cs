using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.Utilities
{
    internal class Function : IFunction
    {
        public string Process(string exercise, string inputText)
        {
            if (exercise == "countChars")
            {
                return CountCharsInText(inputText).ToString();
            }
            else if (exercise == "sumInt")
            {
                return SumIntInText(inputText);
            }
            else return "!!!";
        }

        private int CountCharsInText(string inputText)
        {
            if (!string.IsNullOrEmpty(inputText)) return inputText.Length;
            else return 0;
        }

        private string SumIntInText(string inputText)
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
                return sum.ToString();
            }
            catch (FormatException ex)
            {
                return ex.Message;
            }            
        }

    }
}
