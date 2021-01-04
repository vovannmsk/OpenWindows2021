using OpenGEWindows;


namespace States
{
    public class StateGT092 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Otit otit;
        private int tekStateInt;

        public StateGT092()
        {

        }

        public StateGT092(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();                 // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            OtitFactory otitFactory = new OtitFactory(botwindow);
            this.otit = otitFactory.createOtit();

            this.tekStateInt = 92;
        }


        /// <summary>
        /// метод осуществляет действия для перехода в следующее состояние
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            for (int j = 1; j <= 10; j++)
            {
                server.PressButtonAlchemy();
                botwindow.Pause(300);
            }
            botwindow.Pause(600);
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
            return (  server.isInventoryFull() || 
                      server.isOutOfIngredients() || 
                     !server.isAlchemy()  ) ;  //выходим из процесса алхимичивания, если переполнился инвентарь или закончился один из ингредиентов 
                                                                                                            //или пользователь вышел из режима алхимииалх
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT093(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            return new StateGT092(botwindow);
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
