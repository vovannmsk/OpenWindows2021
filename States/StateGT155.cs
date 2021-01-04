using OpenGEWindows;


namespace States
{
    public class StateGT155 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
        //        GotoTrade gototrade;
        private int tekStateInt;

        public StateGT155()
        {

        }

        public StateGT155(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.tekStateInt = 155;
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
                if (other.getTekStateInt() == 1)         //2155.04.1155
                {
                    if (this.getTekStateInt() == other.getTekStateInt()) result = true;
                }
                else   //2155.04.1155
                {
                    if (this.getTekStateInt() >= other.getTekStateInt()) result = true;  //2155.04.1155
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
        /// метод осуществляет действия для перехода из состояния GT0155 в GT156
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            ////тыкаем в другого торговца (который стоит рядом с нужным нам)
            town.GoToTraderMap();

            ////тыкаем "Move"
            town.ClickMoveMap();

            // закрываем все окна
            botwindow.PressEscThreeTimes();

            //время, чтобы добежать до нужного торговца (разное время для разных городов) 
            town.PauseToTrader();

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
        public bool isAllCool()          // получилось ли перейти к следующему состоянию. true, если получилось
        {
            return !town.isOpenMap();    //если нет меню Alt+Z, то значит добежали до торговца
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT156(botwindow);  //, gototrade);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
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
