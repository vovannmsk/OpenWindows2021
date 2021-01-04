using OpenGEWindows;


namespace States
{
    public class StateGT108 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private int tekStateInt;

        public StateGT108()
        {

        }

        public StateGT108(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 108;
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
            //никаких действий, только проверка того, на какую миссию попали и распределение по состояниям
            server.WriteToLogFileBH("108 выбираем, в какую миссию идти");
            //botwindow.PressMitridatBH();
            botwindow.setStatusOfAtk(1);                      // статус атаки = 1 (начинаем атаковать босса)
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
            botwindow.setStatusOfAtk(1);                      // статус атаки = 1 (начинаем атаковать босса)
            int result = server.NumberOfMissionBH();
            switch (result)
            {
                case 1:  return new StateGT109(botwindow);    // круглая арена с колоннами и квадратные плиты на полу. босс по центру      109
                case 2:  return new StateGT110(botwindow);    // сетка, а под ней зеленая вода                                             110  самая жопа!!!
                case 3:  return new StateGT111(botwindow);    // тринити с текущей водой                                                   111+
                case 4:  return new StateGT112(botwindow);    // лава + малиновый грунт                                                    112+
                case 5:  return new StateGT113(botwindow);    // вода + плиты на полу                                                      113
                case 6:  return new StateGT114(botwindow);    // квадратная арена. идти в обход до босса. лучше справа                     114  еще одна жопа!!!
                case 7:  return new StateGT115(botwindow);    // Раффлезия                                                                 115+
                case 8:  return new StateGT116(botwindow);    // синий пол, синие колонны в виде буквы Л                                   116+
                case 9:  return new StateGT117(botwindow);    // Золотой голем                                                             117+
                case 10: return new StateGT118(botwindow);    // Море. Прибой                                                              118+
                case 11: return new StateGT119(botwindow);    // Красные свечи                                                             119+
                case 12: return new StateGT120(botwindow);    // Серый пол, арнамент на полу в виде змейки. Муфаса                         120
                case 13: return new StateGT121(botwindow);    // Синий пол, синие кристаллы                                                121
                case 14: return new StateGT122(botwindow);    // Две темные арены со столбом посредине. Босс в дальней арене               122
                case 15: return new StateGT123(botwindow);    // Желто-коричневая неровная плитка (посредине крутящаяся хрень)             123         
                case 16: return new StateGT124(botwindow);    // Лава-босс. арка посредине                                                 124
                case 17: return new StateGT125(botwindow);    // Плитка ромбами, босс близко впереди                                       125
                case 18: return new StateGT126(botwindow);    // Темная арена, земляной пол                                                126
                    //повторные
                case 19: return new StateGT119(botwindow);    // Красные свечи                                                             119+
                case 20: return new StateGT122(botwindow);    // Две темные арены со столбом посредине. Босс в дальней арене               122
                case 21: return new StateGT113(botwindow);    // вода + плиты на полу                                                      113+
                case 22: return new StateGT114(botwindow);    // квадратная арена. идти в обход до босса. лучше справа                                                   113+
                case 23: return new StateGT114(botwindow);    // квадратная арена. идти в обход до босса. лучше справа                                                   113+
                case 24: return new StateGT118(botwindow);    // Море. Прибой                                                              113+
                case 25: return new StateGT115(botwindow);    // Раффлезия                                                                 115+
                case 26: return new StateGT124(botwindow);    // Лава-босс. арка посредине                                                 124
                case 31: return new StateGT126(botwindow);    // Темная арена, земляной пол                                                126
                case 32: return new StateGT111(botwindow);    // тринити с текущей водой                                                   111+
                case 33: return new StateGT119(botwindow);    // Красные свечи                                                             119+
                case 34: return new StateGT125(botwindow);    // Плитка ромбами, босс близко впереди                                       125
                case 35: return new StateGT126(botwindow);    // Темная арена, земляной пол                                                126
                case 36: return new StateGT125(botwindow);    // Плитка ромбами, босс близко впереди                                       125
                case 37: return new StateGT111(botwindow);    // тринити с текущей водой                                                   111+
            }
            server.MissionNotFoundBH();
            return new StateGT129(botwindow);         //если карта не найдена (не может быть такого), то идём в конец цикла
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            server.WriteToLogFileBH("108 ELSE ");

            return new StateGT108(botwindow);
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
