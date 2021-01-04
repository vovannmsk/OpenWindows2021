using OpenGEWindows;


namespace States
{
    public class StateGT062 : IState
    {
        private botWindow botwindow;
        private Server server;
        private int tekStateInt;

        public StateGT062()
        {

        }

        public StateGT062(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;

            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)

            this.tekStateInt = 62;
        }


        /// <summary>
        /// метод осуществляет действия для перехода в следующее состояние
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            //============ выбор персонажей  ===========
            server.TeamSelection();
            botwindow.Pause(500);

            //============ выбор канала ===========
            botwindow.SelectChannel(3);            //идем на 3 канал
            botwindow.Pause(500);

            //============ выход в город  ===========
            server.NewPlace();                //начинаем в Ребольдо

            botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown

            botwindow.Pause(2000);
            int i = 0;
            while (i < 50)      // ожидание загрузки города, проверка по двум видам оружия
            {
                botwindow.Pause(500);
                i++;
                if (server.isTown()) break;    // проверяем успешный переход в город, проверка по ружью и дробовику
            }
            botwindow.Pause(7000);       //поставил по Колиной просьбе
            botwindow.PressEscThreeTimes();
            botwindow.Pause(1000);
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
            return !server.isBarack();    //проверяем, что уже не казармы
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT063(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            return new StateGT062(botwindow);
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
