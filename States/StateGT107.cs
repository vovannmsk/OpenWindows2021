﻿using OpenGEWindows;


namespace States
{
    public class StateGT107 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private BHDialog BHdialog;
        private BHDialogFactory dialogFactory;
        private int tekStateInt;

        public StateGT107()
        {

        }

        public StateGT107(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.dialogFactory = new BHDialogFactory(botwindow);
            this.BHdialog = dialogFactory.create();   // создали конкретный экземпляр класса BHDialog по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 107;
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
            //начинаем из пятого состояния, т.е. isGateBH5 = true
            BHdialog.PressOkButton(1);

            ////ожидание загрузки BH
//            int counter = 0;
//            while (!(server.isBH()) && (counter < 30))
//            { botwindow.Pause(500); counter++; }

//            server.WriteToLogFileBH("107 состояние ворот 5. нажали кнопку Ок и подождали загрузки БХ");
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
            //return server.isBH();
            return true;
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return new StateGT108(botwindow);   //идем в конец цикла
//            return new StateGT101(botwindow);     //идём в состояние 101, чтобы снова зайти в ворота BH         если не получиться, то можно вернуть старый вариант
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            server.WriteToLogFileBH("107 ELSE ");

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
