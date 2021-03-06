﻿using OpenGEWindows;


namespace States
{
    public class StateGT021 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private Town town;
        private Pet pet;
        private PetFactory petFactory;

        private int tekStateInt;

        public StateGT021()
        {

        }

        public StateGT021(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.petFactory = new PetFactory(botwindow);
            this.pet = petFactory.createPet();
            this.tekStateInt = 21;
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
            if (!server.isKillHero())             //если никто не убит, то можно призывать пета
                pet.buttonSummonPet();
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
            //bool pet1 = pet.isSummonPet();
            //bool kill = server.isKillHero();
            //botwindow.Pause(1);
            return pet.isSummonPet() && !server.isKillHero();     //пет призван и никто не убит
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT022(botwindow);
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            if (!server.isHwnd()) return new StateGT028(botwindow);  //последнее состояние движка, чтобы движок сразу тормознулся
            if (server.isLogout())
            {
                //                return new StateGT015(botwindow);  //коннект и далее
                return new StateGT023(botwindow);                  //в конец движка
            }
            else
            {
                if (!server.isKillHero()) { return this; }          //если никто не убит, то остаемся в этом же состоянии и пытаемся призвать пета вновь
                else 
//                    return new StateGT001(botwindow);            // если кто-то из героев убит присваиваем конечное состояние, а именно GT01, чтобы на следующем круге реанимировать бота
                    return new StateGT023(botwindow);                  //если убит один из героев, то в конец движка
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
