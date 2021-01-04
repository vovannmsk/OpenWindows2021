using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEBot.Data;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            IScriptDataBot scriptDataBot = new ScriptDataBotDB(2);       //делаем объект репозитория с реализацией чтения из базы данных
            DataBot databot = scriptDataBot.GetDataBot();  //в этом объекте все данные по данному окну бота

            Console.WriteLine("{0}", databot.Login);




            //GEBot.Data.GEContext ff = new GEBot.Data.GEContext();

            //List<BotsNew> bots = ff.BotsNew.ToList <BotsNew>();

            //foreach (BotsNew bot in bots)
            //{
            //    Console.WriteLine(bot.Login);
            //}



        }
    }
}
