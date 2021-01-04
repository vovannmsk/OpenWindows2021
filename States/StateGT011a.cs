using OpenGEWindows;


namespace States
{
    public class StateGT011a : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
        private Market market;
        private MarketFactory marketFactory;
        private int tekStateInt;

        public StateGT011a()
        {

        }

        public StateGT011a(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.marketFactory = new MarketFactory(botwindow);
            this.market = marketFactory.createMarket();
            this.tekStateInt = 11;
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
            server.WriteToLogFileBH("11a");
            //server.Botton_Sell();             // Нажимаем на кнопку Sell
            //botwindow.Pause(1500);
            //server.Button_Close();            // Нажимаем на кнопку Close
            //town.ExitFromTrader();               // дополнительные нажатия при выходе из магазина
            //botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown
            //botwindow.Pause(2000);

            market.Botton_Sell();             // Нажимаем на кнопку Sell
            botwindow.Pause(1500);
            market.Button_Close();            // Нажимаем на кнопку Close
            town.ExitFromTrader();               // дополнительные нажатия при выходе из магазина
            botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown
            botwindow.Pause(2000);

        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
            //botwindow.PressEscThreeTimes();
            //botwindow.Pause(500);
        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()
        {
            return server.isTown();   
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT012(botwindow);  //, gototrade);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            if (!botwindow.isHwnd()) return new StateGT028(botwindow);  //последнее состояние движка, чтобы движок сразу тормознулся
            if (server.isLogout())
            {
                return new StateGT015(botwindow);  //коннект и далее
            }
            else
            {
                return this;
            }
        }

        public int getTekStateInt()
        {
            return this.tekStateInt;
        }
    }
}
