using OpenGEWindows;


namespace States
{
    public class StateGT016new2 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private int tekStateInt;

        public StateGT016new2()
        {

        }

        public StateGT016new2(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 16;
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
            server.WriteToLogFileBH("Казарма");
            //============ выбор персонажей  ===========
            
            server.TeamSelection();
            botwindow.Pause(1000);

            //============ выбор канала ===========
            botwindow.SelectChannel();
            //botwindow.Pause(1000);

            //============ выход в город  ===========
            server.NewPlace();                //начинаем в ребольдо  

            //botwindow.ToMoveMouse();          //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown
            new Point(500, 500).Move();

            botwindow.Pause(2000);
            int i = 0;
            while ((i < 50) && (!server.isTown()))      // ожидание загрузки города
            { 
                botwindow.Pause(500); 
                i++;
                //if (server.isTown())  break;    // проверяем успешный переход в город
            }
            //botwindow.Pause(12000);       //поставил по Колиной просьбе
            botwindow.Pause(1000);       //проба

            //server.GetGifts();
            //server.TaskOff();

            botwindow.PressEsc();
            botwindow.Pause(1000);
        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
            ///???
        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()
        {
            return !server.isBarack();
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
                return new StateGT017(botwindow);              //если не надо покупать патроны
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
