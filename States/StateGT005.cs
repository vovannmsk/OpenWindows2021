using OpenGEWindows;


namespace States
{
    public class StateGT005 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
//        GotoTrade gototrade;
        private int tekStateInt;

        public StateGT005()
        {

        }

        public StateGT005(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
//            this.gototrade = gototrade;
            this.tekStateInt = 5;
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
        /// метод осуществляет действия для перехода из состояния GT05 в GT06
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            server.WriteToLogFileBH("05");
            // ============= открыть карту через верхнее меню ============================================================
            server.OpenMapForState();   // без проверок и while
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход из состояния GT01 в GT02
        /// </summary>
        public void elseRun()
        {
            botwindow.PressEscThreeTimes();
            botwindow.Pause(500);
        }

        /// <summary>
        /// проверяет, получилось ли перейти к состоянию GT06
        /// </summary>
        /// <returns> true, если получилось перейти к состоянию GT06 </returns>
        public bool isAllCool()          // получилось ли перейти к следующему состоянию. true, если получилось
        {
            //bool ff = town.isOpenMap();
            return town.isOpenMap();     // проверяем, открыта ли карта (метод зависит от города)
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT006(botwindow);
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
