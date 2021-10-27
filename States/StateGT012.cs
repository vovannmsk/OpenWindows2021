using OpenGEWindows;
using GEBot.Data;


namespace States
{
    public class StateGT012 : IState
    {
        private botWindow botwindow;
        private Server server;
        //private Market market;
        private int tekStateInt;
        private GlobalParam globalParam;

        public StateGT012()
        {

        }

        public StateGT012(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            //MarketFactory marketFactory = new MarketFactory(botwindow);
            //this.market = marketFactory.createMarket();
            this.globalParam = new GlobalParam();
            this.tekStateInt = 12;
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
            server.WriteToLogFileBH("12");
            if (globalParam.Windows10)
                //server.CloseSandboxie();     //для себя (win 10)
                server.Logout();
            else
                server.Logout();            //для Наташи (win 7)
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
            botwindow.PressEscThreeTimes();
            botwindow.Pause(500);

            //если из-за тормозов интернета мы не вышли из магазина после продажи (и из-за этого не смогли тыкнуть в логаут), то делаем это здесь
            //market.Button_Close();
            //town.ExitFromTrader();
        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()
        {
            return true;        //для себя
            //return !botwindow.isHwnd();   // для GotoEnd и server.CloseSandboxie();
            //return server.isLogout();      // для Наташи
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT014(botwindow);  
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
          //  if (!botwindow.isHwnd()) return new StateGT28(botwindow);  //последнее состояние движка, чтобы движок сразу тормознулся
            //if (server.isLogout())
            //{
            //    return new StateGT15(botwindow);  //коннект и далее
            //}
            //else
            //{
                return this;
            //}
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
