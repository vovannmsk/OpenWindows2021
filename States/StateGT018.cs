using OpenGEWindows;


namespace States
{
    public class StateGT018 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private Pet pet;
        private PetFactory petFactory;
        private int tekStateInt;

        public StateGT018()
        {

        }

        public StateGT018(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.petFactory = new PetFactory(botwindow);
            this.pet = petFactory.createPet();
            this.tekStateInt = 18;
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
            botwindow.PressMitridat();            //пьем митридат

            //botwindow.Pause(500);
            //server.GoToChannel();                                                            //новое 30-06-2017
            //botwindow.Pause(500);


            botwindow.ClickSpace();     // К бою!!!!!!!! 
            botwindow.Pause(1000);

            server.TopMenu(9, 2);   //открываем верхнее меню пункт "пет"

            //int i = 0;
            //while ((!pet.isOpenMenuPet()) & (i < 10))         //ожидание загрузки меню пета
            //{ botwindow.Pause(300); i++; }

            botwindow.Pause(1000);                                                              
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
            return pet.isOpenMenuPet();     //сделать проверку, открыто ли окно с петом Alt+P
            //return true;
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            if (pet.isActivePet())
            {
                if (server.isKillHero())
                {
                    return new StateGT001(botwindow);                  //если убит один из героев, то в конец движка
                }
                else
                {
                    return new StateGT023(botwindow);             // если пет уже активирован, то идем на расстановку
                }
            }
            else
            {
                return new StateGT020(botwindow);             //если пет не активирован, то идем обычным порядком
            }
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
