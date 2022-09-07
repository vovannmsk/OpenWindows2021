using OpenGEWindows;
//using GEBot.Data;

namespace States
{
    public class StateGT401 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        //private BHDialog BHdialog;
        //private BHDialogFactory BHdialogFactory;
        //private BotParam botParam;

        private int tekStateInt;

        public StateGT401()
        {

        }

        public StateGT401(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            //this.botParam = new BotParam(botwindow.getNumberWindow());
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            //this.BHdialogFactory = new BHDialogFactory(botwindow);
            //this.BHdialog = BHdialogFactory.create();   // создали конкретный экземпляр класса BHDialog по паттерну "простая Фабрика" (Америка, Европа или Синг)

            this.tekStateInt = 401;
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
            //server.WriteToLogFileBH("БХ");

            //перемещаем бутылки на левую панель   //убрал 26-09-2021 (эксперимент)
            //if (!server.isBottlesOnLeftPanel()) server.MoveBottlesToTheLeftPanel();
            //botwindow.PressEsc();

            //можно не применять хрин и рочю баффы, они применяются в бою на каждом ходе
            //botwindow.ActiveAllBuffBH();
            //botwindow.PressEscThreeTimes();

            server.AddBullets();           

            //if (server.isBulletOff())
            //    server.WriteToLogFileBH("Нет патронов. Аккаунт №" + botParam.NumberOfInfinity);

            server.GoToInfinityGateDem();

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
            return true;
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT499(botwindow);       
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
