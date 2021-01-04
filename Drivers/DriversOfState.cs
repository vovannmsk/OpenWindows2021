using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGEWindows;
using States;

namespace States
{
    /// <summary>
    /// класс хранит движки состояний для выполнения основных последовательностей действий (переход к продаже и обратно, продажа бота, восстановление бота из логаута и проч.)
    /// </summary>
    public class DriversOfState
    {
        private botWindow botwindow;

        public DriversOfState()
        { 
            
        }

        public DriversOfState(int numberOfWindow)
        {
            botwindow = new botWindow(numberOfWindow);

        }



        #region движки для запуска перехода по состояниям

        /// <summary>
        /// перевод из состояния 01 (на работе) в состояние 14 (нет окна). Цель  - продажа после переполнения инвентаря
        /// </summary>
        public void StateGotoTrade()
        {
            botwindow.ReOpenWindow();
            StateDriverRun(new StateGT01(botwindow), new StateGT14(botwindow));
        }

        /// <summary>
        /// перевод из состояния 14 (нет окна бота) в состояние 01 (на работе)
        /// </summary>
        public void StateGotoWork()
        {
            StateDriverRun(new StateGT14(botwindow), new StateGT01(botwindow)); //
        }

        /// <summary>
        /// перевод из состояния 15 (логаут) в состояние 01 (на работе)
        /// </summary>
        public void StateRecovery()
        {
            StateDriverRun(new StateGT15(botwindow), new StateGT01(botwindow));
        }

        /// <summary>
        /// перевод из состояния 14 (нет окна) в состояние 15 (логаут)              
        /// </summary>
        public void StateReOpen()
        {
            StateDriverRun(new StateGT14(botwindow), new StateGT15(botwindow));
        }

        /// <summary>
        /// перевод из состояния 09 (в магазине) в состояние 12 (всё продано, в городе)                 // аква кнопка
        /// </summary>
        public void StateSelling()
        {
            if (botwindow.getserver().isSale())                                 //проверяем, находимся ли в магазине
                StateDriverRun(new StateGT09(botwindow), new StateGT12(botwindow));
        }

        /// <summary>
        /// создание новой семьи, выход в ребольдо, получение и надевание брони 35 лвл, выполнение квеста Доминго, разговор с Линдоном, получение Кокошки и еды 100 шт.
        /// перевод из состояния 30 (логаут) в состояние 41 (пет Кокошка получен)          // розовая кнопка
        /// </summary>
        public void StateNewAcc()
        {
            StateDriverRun(new StateGT30(botwindow), new StateGT41(botwindow));
        }

        /// <summary>
        /// вновь созданный аккаунт бежит через лавовое плато в кратер, становится на выделенное ему место, сохраняет телепорт и начинает ботить
        /// </summary>
        public void StateToCrater()
        {
            StateDriverRun(new StateGT42(botwindow), new StateGT58(botwindow));
        }

        /// <summary>
        /// перевод из состояния 10 (в магазине) в состояние 14 (нет окна)
        /// </summary>
        public void StateExitFromShop()
        {
            StateDriverRun(new StateGT10(botwindow), new StateGT14(botwindow));
        }

        /// <summary>
        /// перевод из состояния 09 (в магазине, на странице входа) в состояние 14 (нет окна)
        /// </summary>
        public void StateExitFromShop2()
        {
            StateDriverRun(new StateGT09(botwindow), new StateGT14(botwindow));
        }

        /// <summary>
        /// перевод из состояния 12 (в городе) в состояние 14 (нет окна) 
        /// </summary>
        public void StateExitFromTown()
        {
            StateDriverRun(new StateGT12(botwindow), new StateGT14(botwindow));
        }

        /// <summary>
        /// если бот стоит на работе, то он направляется на продажу, а потом обратно
        /// </summary>
        public void StateGotoTradeAndWork()
        {
            if (botwindow.getserver().isWork())
            {
                StateGotoTrade();                                          // по паттерну "Состояние".  01-14       (работа - продажа - нет окна)
                botwindow.Pause(2000);
                StateGotoWork();                                           // по паттерну "Состояние".  14-01       (нет окна - логаут - казарма - город - работа)
            }
        }


        /// <summary>
        /// запускает движок состояний StateDriver от пункта stateBegin до stateEnd
        /// </summary>
        /// <param name="stateBegin"> начальное состояние </param>
        /// <param name="stateEnd"> конечное состояние </param>
        public void StateDriverRun(IState stateBegin, IState stateEnd)
        {
            StateDriver stateDriver = new StateDriver(botwindow, stateBegin, stateEnd);    //botwindow в данном случае есть экземпляр класса botWindow, здесь stateDriver - это начальное состояние движка
            while (!stateDriver.Equals(stateEnd))
            {
                stateDriver.run();
                stateDriver.setState();
            }
        }

        /// <summary>
        /// запускает движок состояний StateDriver от пункта stateBegin до stateEnd
        /// </summary>
        /// <param name="stateBegin"> начальное состояние </param>
        /// <param name="stateEnd"> конечное состояние </param>
        public void StateDriverDealerRun(IState stateBegin, IState stateEnd)
        {
            StateDriverTrader stateDriverTrader = new StateDriverTrader(stateBegin);
            while (!stateDriverTrader.Equals(stateEnd))
            {
                stateDriverTrader.run();
                stateDriverTrader.setState();
            }
        }



        #endregion





    }
}
