using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.Utilities
{
    internal interface IFunction
    {
        string Process(string exercise,string inputText);
    }
}
