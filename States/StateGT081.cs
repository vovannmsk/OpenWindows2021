using OpenGEWindows;


namespace States
{
    public class StateGT081 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Otit otit;
        private int tekStateInt;

        public StateGT081()
        {

        }

        public StateGT081(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            OtitFactory otitFactory = new OtitFactory(botwindow);
            this.otit = otitFactory.createOtit();

            this.tekStateInt = 81;
        }


        /// <summary>
        /// метод осуществляет действия для перехода в следующее состояние
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            otit.RouteNextPoint().PressMouseL();    // отбегаем в сторону, чтобы сделать логаут
            botwindow.Pause(1000);

            server.Logout();

            int i = 0;
            while (!server.isLogout())
            {
                botwindow.Pause(1000);
                i++; if (i > 10) break;
            }
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()
        {
            return server.isLogout();
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT082(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            return new StateGT081(botwindow);
        }

        #region стандартные служебные методы для паттерна Состояния

        /// <summary>
        /// задаем метод Equals для данного объекта для получения возможности сравнения объектов State
        /// </summary>
        /// <param name="other"> объект для сравнения </param>
        /// <returns> true, если номера состояний объектов равны </returns>
        public bool Equals(IState other)
        {
            bool result = false;
            if (!(other == null))            //если other не null, то проверяем на равенство
                if (this.getTekStateInt() == other.getTekStateInt()) result = true;
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
        /// геттер. возвращает номер текущего состояния
        /// </summary>
        /// <returns> номер состояния </returns>
        public int getTekStateInt()
        {
            return this.tekStateInt;
        }

        #endregion
    }
}
