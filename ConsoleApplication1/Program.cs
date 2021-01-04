using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
                //<add name="DbConnect" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=GE;Integrated Security=True" providerName="System.Data.SqlClient" />

            IScriptDataBot scriptDataBot = new ScriptDataBotDB(2);       //делаем объект репозитория с реализацией чтения из базы данных
            DataBot databot = scriptDataBot.GetDataBot();                //в этом объекте все данные по данному окну бота

            Console.WriteLine("{0} {1} {2} {3}", databot.hwnd , databot.Kanal, databot.Login, databot.Password);

            //var context22 = new GEContext();

            //List<BotsNew> bots = context22.BotsNew.ToList();

            //foreach (BotsNew bot in bots)
            //{
            //    Console.WriteLine("{0} {1} {2} {3}", bot.HWND, bot.NumberOfWindow, bot.Login, bot.Password);
            //}

            Console.ReadKey();
        }
    }
}
