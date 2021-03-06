﻿using OpenGEWindows;


namespace States
{
    public class StateGT156 : IState
    {
        private botWindow botwindow;
        private Server server;
        private Town town;
        private ServerFactory serverFactory;
        private KatoviaMarket market;
        private KatoviaMarketFactory kMarketFactory;
        private int tekStateInt;

        public StateGT156()
        {

        }

        public StateGT156(botWindow botwindow)   //, GotoTrade gototrade)
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            this.kMarketFactory = new KatoviaMarketFactory(botwindow);
            this.market = kMarketFactory.createMarket();
            this.tekStateInt = 156;
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
            // ============= тыкаем в голову торговца, чтобы войти в магазин  ===================================================
            botwindow.Pause(3000);         //ждем пока подгрузятся все объекты на карте

             town.Click_ToHeadTrader(); //

            botwindow.Pause(5000);

            int i = 0;
            while ((!market.isSale()) && (i < 30))        //время, чтобы загрузился магазин
            { botwindow.Pause(500); i++; }

            //botwindow.Pause(5000);   //время, чтобы загрузился магазин

        }

        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход 
        /// </summary>
        public void elseRun()
        {
            //если не удалось зайти в магазин, то делаем логаут и идем в конец цикла (состояние 162)
            //botwindow.PressEscThreeTimes();
            //botwindow.Pause(500);
            //server.Logout();
            botwindow.Pause(8000);

        }

        /// <summary>
        /// проверяет, получилось ли перейти к следующему состоянию 
        /// </summary>
        /// <returns> true, если получилось перейти к следующему состоянию </returns>
        public bool isAllCool()     
        {
            return market.isSale();    
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT157(botwindow);  
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            return new StateGT161(botwindow); 
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
