using System;
using System.Linq;


namespace GEBot.Data
{
    /// <summary>
    /// =========================================================== класс не используется =====================================
    /// </summary>
    public class ScriptDataBotDB:IScriptDataBot
    {
        private int numberOfWindow;
        private BotParam databot;
        private GlobalParam globalParam;

        public ScriptDataBotDB(int numberOfWindow)
        {
            this.globalParam = new GlobalParam();
            this.numberOfWindow = numberOfWindow;
            this.databot = new BotParam(this.numberOfWindow);

            BotsNew bot = new BotsNew();
            bot = GetBots();                                 //подчитываем из БД одну строку                                                   программа спотыкается на этом месте
            //this.databot.X = Koord_X();
            //this.databot.Y = Koord_Y();
            //this.databot.Login = bot.Login;
            //this.databot.Password = bot.Password;
            //this.databot.Hwnd = (UIntPtr)uint.Parse(bot.HWND);
            //this.databot.Param = bot.Server;
            //this.databot.Kanal = bot.Channel;
            //this.databot.NomerTeleport = bot.TeleportForSale;
            //this.databot.NameOfFamily = bot.Family;

            //this.databot.TriangleX = GetCoordinatesX();
            //this.databot.TriangleY = GetCoordinatesY();
        }

        public ScriptDataBotDB()
        {
            throw new NotImplementedException("Номер окна должен быть указан обязательно!!!");
        }

        /// <summary>
        /// возвращает данные для бота, заданные пользователем
        /// </summary>
        /// <returns></returns>
        public BotParam GetDataBot()
        {
            return this.databot;
        }

        /// <summary>
        /// изменяем Hwnd окна и записываем в Db
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(UIntPtr hwnd)
        {
            databot.Hwnd = hwnd;
            // обязательно прописать запись hwnd в базу данных Entity Framework
            var context = new GEContext();
            IQueryable<BotsNew> query = context.BotsNew.Where(c => c.NumberOfWindow == this.numberOfWindow);
            BotsNew bots = query.Single<BotsNew>();
            bots.HWND = databot.Hwnd.ToString();
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
            GEContext context = new GEContext();
            BotsNew singleBot = new BotsNew();

            IQueryable<BotsNew> query = context.BotsNew.Where(c => c.NumberOfWindow == this.numberOfWindow);
            singleBot = query.Single();

            return singleBot;
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
