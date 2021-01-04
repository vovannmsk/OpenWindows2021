using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
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
            BotsNew bot = GetBots();
            databot.x = Koord_X();
            databot.y = Koord_Y();
            databot.Login = bot.Login;
            databot.Password = bot.Password;
            databot.hwnd = (UIntPtr) uint.Parse(bot.HWND);
            databot.param = bot.Server;
            databot.Kanal = bot.Channel;
            databot.nomerTeleport = bot.TeleportForSale;
            databot.nameOfFamily = bot.Family;

            databot.triangleX = GetCoordinatesX();
            databot.triangleY = GetCoordinatesY();
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
            return databot;
        }

        /// <summary>
        /// изменяем Hwnd окна и записываем в Db
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(UIntPtr hwnd)
        {
            databot.hwnd = hwnd;
            // обязательно прописать запись hwnd в базу данных Entity Framework
            var context = new GEContext();
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
            var context = new GEContext();

            //var bb = context.BotsNew.ToList <BotsNew>();

            //return bb.First();
            IQueryable<BotsNew> query = context.BotsNew.Where(c => c.NumberOfWindow == this.numberOfWindow);
            BotsNew singleBot = query.Single();
            return singleBot;
            


            ////List<BotsNew> bot1 = context.BotsNew.ToList();
            ////BotsNew [] bot = context.BotsNew.ToArray();

            ////int j = 0;
            ////int i = 0;

            ////foreach  (BotsNew bot_ in bot)
            ////{
            ////    if (bot_.NumberOfWindow == this.numberOfWindow)
            ////    {
            ////        j = i;
            ////    }
            ////    i++;
            ////}
            ////return bot[j];

            
        }

        /// <summary>
        /// читаем из базы координаты X (икс) расстановки ботов на карте
        /// </summary>
        /// <returns></returns>
        private int[] GetCoordinatesX()
        {
            var context = new GEContext();

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
            var context = new GEContext();

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
