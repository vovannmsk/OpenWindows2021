using OpenGEWindows;


namespace States
{
    public class StateGT114 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private int tekStateInt;

        public StateGT114()
        {

        }

        public StateGT114(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 114;
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
            // квадратная арена. идти в обход до босса. лучше справа
            //MessageBox.Show("6 квадратная арена. идти в обход до босса. лучше справа");
            server.WriteToLogFileBH("сост 114 в бой");

            //старый рабочий вариант
            //server.FightToPoint(997+25, 160+25, 3);
            //server.FightToPoint(1034, 308, 3);
            //server.FightToPoint(1011, 80, 5);
            //server.FightToPoint(510 + 25, 86 + 25, 3);
            //server.FightToPoint(510 + 25, 86 + 25, 3);
            //server.FightToPoint(510 + 25, 86 + 25, 3);
            //server.FightToPoint(293, 166, 0);

            
            
            server.FightToPoint(997 + 25, 160 + 25, 3);
            server.FightToPoint(1034, 308, 3);
            server.FightToPoint(1011, 80, 6);
            server.TurnL(1);
            server.TurnUp();
            server.FightToPoint(94, 383, 1);
            server.TurnDown();

            server.waitToCancelAtak();

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
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT129(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            server.WriteToLogFileBH("114 ELSE ");

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
