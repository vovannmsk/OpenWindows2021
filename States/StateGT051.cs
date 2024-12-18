﻿using OpenGEWindows;


namespace States
{
    public class StateGT051 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
        private Pet pet;
        private PetFactory petFactory;
        private int tekStateInt;

        public StateGT051()
        {

        }

        public StateGT051(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.petFactory = new PetFactory(botwindow);
            this.pet = petFactory.createPet();
            this.tekStateInt = 51;
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
            if (!pet.isActivePet()) pet.ActivatePet();
            //botwindow.PressMouse(408, 405);  //Click Active Pet
            //botwindow.Pause(2500);
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
            return (!server.isKillHero() && pet.isActivePet());     //если никто не убит и активирован пет, то дальше
            // botwindow.isColor2(495 - 5, 310 - 5, 13200000, 496 - 5, 308 - 5, 13600000, 5);
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT052(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            if (!server.isHwnd()) return new StateGT058(botwindow);  //последнее состояние движка, чтобы движок сразу тормознулся
            if (server.isLogout())
            {
                return new StateGT042(botwindow);  //коннект и далее
            }
            else
            {
                if (!server.isKillHero()) { return this; }          //если никто не убит, то остаемся в этом же состоянии и пытаемся активировать пета вновь
                else { return new StateGT058(botwindow); }           // если кто-то из героев убит, то присваиваем конечное состояние, а именно GT01, чтобы на следующем круге реанимировать бота
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
