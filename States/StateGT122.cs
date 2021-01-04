using OpenGEWindows;


namespace States
{
    public class StateGT122 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private int tekStateInt;

        public StateGT122()
        {

        }

        public StateGT122(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 122;
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
            //14 Две темные арены со столбом посредине. Босс в дальней арене
            //MessageBox.Show("14 Две темные арены со столбом посредине. Босс в дальней арене");
            server.WriteToLogFileBH("сост 122 в бой");

            server.FightToPoint(905, 87, 3);  //вправо наискосок
            server.TurnUp();
            server.TurnR(1);
            //botwindow.Pause(50000);
            server.FightToPoint(142-75, 535-75, 1);
            server.TurnDown();



            //старый рабочий вариант (но очень долгий)
            //server.FightToPoint(905, 87, 3);  //вправо наискосок
            //server.FightToPoint(813, 84, 3);  //вправо наискосок
            //server.FightToPoint(87 , 114, 5);  //влево наискосок
            //server.FightToPoint(478, 114, 3);  //прямо
            //server.FightToPoint(478, 114, 3);
            //server.FightToPoint(478, 114, 0);

            //новый вариант
            //server.FightToPoint(905, 87, 3);  //вправо наискосок
            //server.TurnUp();
            //botwindow.Pause(30000);
            //server.FightToPoint(478, 114, 0);  //уточнить
            //server.TurnDown();


            server.waitToCancelAtak();
            //botwindow.Pause(30000);
            //MessageBox.Show("122 куда тыкать?");/
            

            //server.runAway();

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
            server.WriteToLogFileBH("122 ELSE ");

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
