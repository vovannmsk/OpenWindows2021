﻿using OpenGEWindows;
using GEBot.Data;


namespace States
{
    public class StateGT102 : IState
    {
        private botWindow botwindow;
        private Server server;
        private ServerFactory serverFactory;
        private BHDialog BHdialog;
        private BHDialogFactory dialogFactory;
        private BotParam botParam;

        private int tekStateInt;

        public StateGT102()
        {

        }

        public StateGT102(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.botParam = new BotParam(botwindow.getNumberWindow());
            this.serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.dialogFactory = new BHDialogFactory(botwindow);
            this.BHdialog = dialogFactory.create();   // создали конкретный экземпляр класса BHDialog по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.tekStateInt = 102;
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
            //ничего не выполняем на этом шаге, а только распределяем по потокам с помощью StateNext
            server.WriteToLogFileBH("102 Распределение по состояниям ворот 1 или 3");
            //botwindow.setStatusOfAtk(0);
            botParam.StatusOfAtk = 0;
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
            return BHdialog.isGateBH();   //проверяем на всякий случай, что мы в воротах BH
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            if (BHdialog.isGateBH1())                   //если есть попытки (можно сходить в Инфинити всего 5 раз в день)
            {
                return new StateGT103(botwindow);
            }
            if (BHdialog.isGateBH3())                   //если попытки закончились (можно сходить в Инфинити всего 5 раз в день)
            {
                return new StateGT103a(botwindow);
            }
            else
            {
                return new StateGT108(botwindow);     // в конец цикла (если непонятные ворота)
            }
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            server.WriteToLogFileBH("102 ELSE");
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
