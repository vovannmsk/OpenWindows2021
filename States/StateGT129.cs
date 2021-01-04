using OpenGEWindows;
using GEBot.Data;


namespace States
{
    public class StateGT129 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private GlobalParam globalParam;
        private BotParam botParam;
        private int tekStateInt;

        public StateGT129()
        {

        }

        public StateGT129(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            globalParam = new GlobalParam();
            botParam = new BotParam(botwindow.getNumberWindow());
            this.tekStateInt = 129;
        }

        /// <summary>
        /// задаем метод Equals для данного объекта для получения возможности сравнения объектов State
        /// </summary>
        /// <param name="other"> объект для сравнения </param>
        /// <returns> true, если номера состояний объектов равны </returns>
        public bool Equals(IState other)
        {
            bool result = false;
            if (!(other == null))            //если other не null, то проверяем на равенство
                if (other.getTekStateInt() == 1)         //27.04.17
                {
                    if (this.getTekStateInt() == other.getTekStateInt()) result = true;
                }
                else   //27.04.17
                {
                    if (this.getTekStateInt() >= other.getTekStateInt()) result = true;  //27.04.17
                }
            return result;
        }

        /// <summary>
        /// геттер, возвращает текущее состояние
        /// </summary>
        /// <returns></returns>
        public IState getTekState()
        {
            return this;
        }

        /// <summary>
        /// метод осуществляет действия для перехода в следующее состояние
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            if (server.isBoxOverflow())
            {
                //botwindow.setStatusOfSale(1);
                botParam.StatusOfSale = 1;

            }

            //botwindow.PressEscThreeTimes();   // ================= убирает все лишние окна с экрана =================================
            //botwindow.Pause(500);

            server.WriteToLogFileBH("сост 129 в сторону и телепорт в БХ");
            if (server.isBoxOverflow())
            {
                //botwindow.setStatusOfSale(1);
                botParam.StatusOfSale = 1;
            }



            server.Teleport(2, false);                  // телепорт в БХ (без проверки открытия меню с телепортами)

            if (server.isBoxOverflow())
            {
                //botwindow.setStatusOfSale(1);
                botParam.StatusOfSale = 1;
            }

            botwindow.PressEscThreeTimes();   // ================= убирает все лишние окна с экрана =================================

            if (server.isBoxOverflow())
            {
                //botwindow.setStatusOfSale(1);
                botParam.StatusOfSale = 1;
            }

            ////ожидание загрузки BH
            //int counter = 0;
            //while ((!server.isBH()) && (counter < 12))
            //{ botwindow.Pause(1000); counter++; }
            //server.WriteToLogFileBH("сост 129 дождались загрузки БХ");


        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
            botwindow.PressEscThreeTimes();
            botwindow.Pause(500);
        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()
        {
            return true;
//            return server.isBH();
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT130(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            server.WriteToLogFileBH("сост 129 не дождались загрузки БХ                ELSE");

            return this;
        }

        /// <summary>
        /// геттер. возвращает номер текущего состояния
        /// </summary>
        /// <returns> номер состояния </returns>
        public int getTekStateInt()
        {
            return this.tekStateInt;
        }
    }
}
