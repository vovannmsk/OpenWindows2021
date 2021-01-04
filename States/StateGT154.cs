using OpenGEWindows;


namespace States
{
    public class StateGT154 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
        private int tekStateInt;

        public StateGT154()
        {

        }

        public StateGT154(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.tekStateInt = 154;
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
        /// метод осуществляет действия для перехода из состояния GT0154 в GT155
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            // тыкаем во вторую закладку карты города
            town.SecondBookmark();
            botwindow.Pause(1000);
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход из состояния GT0154 в GT07
        /// </summary>
        public void elseRun()
        {
        }

        /// <summary>
        /// проверяет, получилось ли перейти к состоянию GT02
        /// </summary>
        /// <returns> true, если получилось перейти к состоянию GT02 </returns>
        public bool isAllCool()          // получилось ли перейти к следующему состоянию. true, если получилось
        {
            return town.isSecondBookmark();
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT155(botwindow);
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
