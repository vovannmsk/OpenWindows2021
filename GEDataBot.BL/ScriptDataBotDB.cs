using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GEDataBot.BL
{
    public class ScriptDataBotDB:IScriptDataBot
    {
        private const String KATALOG_MY_PROGRAM = "C:\\!! Суперпрограмма V&K\\";
        private int numberOfWindow;
        private DataBot databot;

        public ScriptDataBotDB(int numberOfWindow)
        {
            this.numberOfWindow = numberOfWindow;
            this.databot = new DataBot();
            BotsNew bot = GetBots();               //подчитываем из БД одну строку                                                   программа спотыкается на этом месте
            this.databot.x = Koord_X();
            this.databot.y = Koord_Y();
            this.databot.Login = bot.Login;
            this.databot.Password = bot.Password;
            //this.databot.hwnd = (UIntPtr)uint.Parse(bot.HWND);
            this.databot.param = bot.Server;
            this.databot.Kanal = bot.Channel;
            this.databot.nomerTeleport = bot.TeleportForSale;
            this.databot.nameOfFamily = bot.Family;

            this.databot.triangleX = GetCoordinatesX();
            this.databot.triangleY = GetCoordinatesY();
        }

        public ScriptDataBotDB()
        {
            throw new NotImplementedException("Номер окна должен быть указан обязательно!!!");
        }

        /// <summary>
        /// возвращает данные для бота, заданные пользователем
        /// </summary>
        /// <returns></returns>
        public DataBot GetDataBot()
        {
            return this.databot;
        }

        /// <summary>
        /// изменяем Hwnd окна и записываем в Db
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(UIntPtr hwnd)
        {
            databot.hwnd = hwnd;
            // обязательно прописать запись hwnd в базу данных Entity Framework
            var context = new GEContextBots();
            IQueryable<BotsNew> query = context.BotsNew.Where(c => c.NumberOfWindow == this.numberOfWindow);
            BotsNew bots = query.Single<BotsNew>();
            bots.HWND = databot.hwnd.ToString();
            context.SaveChanges();
        }

        /// <summary>
        /// функция возвращает смещение по оси X окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_X()
        {
            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 875, 850, 825, 800, 775, 750, 725, 700 };
            return koordX[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает смещение по оси Y окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_Y()   // каталог и номер окна
        {
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 5, 30, 55, 80, 105, 130, 155, 180 };
            return koordY[this.numberOfWindow - 1];
        }


        #region методы Entity Framework, которые читают из БД значения для последующего присваивания переменным класса

        /// <summary>
        /// чтение из БД одной строки с пользовательскими параметрами бота
        /// </summary>
        /// <param name="i"> номер окна бота</param>
        /// <returns>пользовательские параметры бота</returns>
        private BotsNew GetBots()
        {
            GEContextBots context = new GEContextBots();

            BotsNew singleBot = new BotsNew();

            try
            {
                IQueryable<BotsNew> query = context.BotsNew.Where(c => c.NumberOfWindow == this.numberOfWindow);
                singleBot = query.Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return singleBot;
        }

        /// <summary>
        /// читаем из базы координаты X (икс) расстановки ботов на карте
        /// </summary>
        /// <returns></returns>
        private int[] GetCoordinatesX()
        {
            var context = new GEContextBots();

            //IQueryable<CoordinatesNew> query = context.CoordinatesNew.Where(c => c.Id_Bots == i);

            var query = from c in context.CoordinatesNew
                        where c.Id_Bots == this.numberOfWindow
                        orderby c.NumberOfHeroes
                        select c.X;

            var coordinates = query.ToArray();

            return coordinates;
        }

        /// <summary>
        /// читаем из базы координаты Y (игрек) расстановки ботов на карте
        /// </summary>
        /// <returns></returns>
        private int[] GetCoordinatesY()
        {
            var context = new GEContextBots();

            //IQueryable<CoordinatesNew> query = context.CoordinatesNew.Where(c => c.Id_Bots == i);

            var query = from c in context.CoordinatesNew
                        where c.Id_Bots == this.numberOfWindow
                        orderby c.NumberOfHeroes
                        select c.Y;

            var coordinates = query.ToArray();

            return coordinates;
        }


        ///// <summary>
        ///// читаем из базы координаты расстановки ботов на карте
        ///// </summary>
        ///// <returns></returns>
        //private List<CoordinatesNew> GetCoordinates(int i)
        //{
        //    var context = new GEContext();

        //    //IQueryable<CoordinatesNew> query = context.CoordinatesNew.Where(c => c.Id_Bots == i);

        //    var query = from c in context.CoordinatesNew
        //                where c.Id_Bots == i
        //                orderby c.NumberOfHeroes
        //                select c;

        //    var coordinates = query.ToList();

        //    return coordinates;
        //}


        #endregion


    }
}
