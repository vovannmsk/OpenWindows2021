using OpenGEWindows;

namespace States
{
    public class StateGT151 : IState
    {
        private botWindow botwindow;
        private Server server;                 
        private ServerFactory serverFactory;
        private int tekStateInt;


        public StateGT151()
        {

        }

        public StateGT151(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 151;
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
                {
                    if (this.tekStateInt == other.getTekStateInt()) result = true;
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
        /// метод осуществляет действия для перехода из состояния GT151 в GT02
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            botwindow.PressEscThreeTimes();   // ================= убирает все лишние окна с экрана =================================
            botwindow.Pause(500);

            server.runAway();
            server.TeleportToTownAltW(1);            //летим в Ребольдо

            //ожидание загрузки города
            int counter = 0;
            while ((!server.isTown()) && (counter < 30))                  
            { botwindow.Pause(1000); counter++; }

            botwindow.Pause(12000);         //10-04-2019   ожидание подгрузки всех NPC в городе
            botwindow.PressEscThreeTimes(); //29.04.17
            botwindow.Pause(500);
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход из состояния GT151 в GT152
        /// </summary>
        public void elseRun()            
        {
            botwindow.PressEscThreeTimes();
            botwindow.Pause(500);
        }

        /// <summary>
        /// проверяет, получилось ли перейти к состоянию GT02
        /// </summary>
        /// <returns> true, если получилось перейти к состоянию GT02 </returns>
        public bool isAllCool()          // получилось ли перейти к следующему состоянию. true, если получилось
        {
            return  server.isTown(); 
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        { 
            return new StateGT152(botwindow);  
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
