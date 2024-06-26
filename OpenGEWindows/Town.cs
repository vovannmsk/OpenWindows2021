﻿using System.Threading;
using GEBot.Data;

namespace OpenGEWindows
{
    public abstract class Town
    {
        #region общие
            protected int xx;
            protected int yy;
            protected iPoint pointMaxHeight;
            protected const int NUMBER_OF_ITERATION = 10;
            protected botWindow botwindow;
            protected Dialog dialog;
            protected GlobalParam globalParam;
        #endregion

        #region Los Toldos
            protected iPoint pointOldMan1;
        #endregion

        #region Town Teleport
        /// <summary>
        /// номер городского телепорта для продажи
        /// </summary>
        protected int TELEPORT_N;   
        /// <summary>
        /// номер городского телепорта для патронов
        /// </summary>
        protected int TELEPORT_bullets;
        protected iPoint pointTownTeleport;
        protected iPoint pointTownTeleportBullets;
        protected iPointColor pointOpenTownTeleport1;
        protected iPointColor pointOpenTownTeleport2;

        #endregion

        #region Shop
            protected iPoint pointHeadTrader;

            //protected iPoint pointSellOnMenu;
            //protected iPoint pointOkOnMenu;
            protected int PAUSE_TIME;

        #endregion

        #region Bullet

        protected iPoint pointBulletAutomat;
        protected int PAUSE_TIME_Bullet;
        /// <summary>
        /// торговец на карте для перехода к патронам
        /// </summary>
        protected iPoint pointTraderOnMapBullet;

        #endregion

        #region Map

        protected iPointColor pointOpenMap1;
        protected iPointColor pointOpenMap2;
        protected iPointColor pointBookmark1;
        protected iPointColor pointBookmark2;
        protected iPoint pointOldManOnMap;
        protected iPoint pointBookmark;
        protected iPoint pointTraderOnMap;
        protected iPoint pointButtonMoveOnMap;
        protected iPoint pointExitFromTrader;
        protected iPoint FirstStringOfMap;
        protected int NumberOfLuciaOnMap;
        protected iPoint pointLucia1;
        protected iPoint pointHeadOfLucia;

        #endregion

        // ======================= Методы ====================================

        #region общие

        /// <summary>
        /// удаляем камеру (поднимаем максимально вверх)                           
        /// </summary>
        public void MaxHeight()
            {
                if (globalParam.Samara)
                //if (botwindow.getIsServer())
                {
                    // крутим колесико мыши 1 раз + пауза + колесо обратно    (для самарских серверов)
                    pointMaxHeight.PressMouseWheelUp();
                    Pause(2000);
                    pointMaxHeight.PressMouseWheelDown();
                }
                else
                {
                    // крутим колесико мыши 10 раз
                    for (int j = 1; j <= NUMBER_OF_ITERATION; j++) pointMaxHeight.PressMouseWheelUp();
                }


            }

            /// <summary>
            /// удаляем камеру (поднимаем максимально вверх)                           
            /// </summary>
            public void MinHeight()
            {
                for (int j = 1; j <= NUMBER_OF_ITERATION; j++)
                {
                    pointMaxHeight.PressMouseWheelDown();
                }
            }

            /// <summary>
            /// Останавливает поток на некоторый период
            /// </summary>
            /// <param name="ms"> ms - период в милисекундах </param>
            protected void Pause(int ms)
            {
                //Class_Timer.Pause(ms);
                Thread.Sleep(ms);
            }

        #endregion

        //#region Los Toldos

        //    /// <summary>
        //    /// тыкаем на старого мужика
        //    /// </summary>
        //    public void PressOldMan1()
        //    {
        //        pointOldMan1.PressMouseL();
        //    }

        //#endregion

        #region Town Teleport (Alt + F3)

        /// <summary>
        /// проверяет, открыто ли меню городского телепорта (Alt + F3)                             
        /// </summary>
        /// <returns> true, если телепорт  (Alt + F3) открыт </returns>
        public bool isOpenTownTeleport()
        {
            return ((pointOpenTownTeleport1.isColor()) & (pointOpenTownTeleport2.isColor()));
        }

        /// <summary>
        /// перелететь по городскому телепорту поближе к торговцу                            
        /// </summary>
        public void TownTeleportW()
        {
            pointTownTeleport.PressMouse();
        }


        /// <summary>
        /// перелететь по городскому телепорту поближе к аппарату с патронами                  
        /// </summary>
        public void TownTeleportBullets()
        {
            pointTownTeleportBullets.PressMouse();
        }

        /// <summary>
        /// перелететь по городскому телепорту в указанное место
        /// </summary>
        /// <param name="NumberOfString">номер строки телепорта</param>
        public void TownTeleport(int NumberOfString)
        {
            new Point(115 - 5 + xx, 333 - 5 + (NumberOfString - 1) * 30 + yy).PressMouseL();
        }


        #endregion

        #region Bullet

        /// <summary>
        /// Делаем паузу, чтобы бот успел добежать до торговца           
        /// </summary>
        public void PauseToBullet()
        {
            Pause(PAUSE_TIME_Bullet);
        }

        /// <summary>
        /// тыкаем мышкой в голову торговца, чтобы войти в магазин              
        /// </summary>
        public void Click_ToBulletAutomat()
        {
            pointBulletAutomat.PressMouseL();
        }

        /// <summary>
        /// переход к торговцу, который стоит рядом с нужным нам торговцем. карта местности Alt+Z открыта 
        /// </summary>
        public void GoToTraderMapBullet()
        {
            //pointTraderOnMapBullet.PressMouseLL();
            pointTraderOnMapBullet.DoubleClickL();
        }

        #endregion

        #region Shop

        /// <summary>
        /// Делаем паузу, чтобы бот успел добежать до торговца           
        /// </summary>
        public void PauseToTrader()
            {
                Pause(PAUSE_TIME);
            }

            /// <summary>
            /// тыкаем мышкой в голову торговца, чтобы войти в магазин              
            /// </summary>
            public void Click_ToHeadTrader()
            {
                pointHeadTrader.PressMouseL();
            }

            /// <summary>
            /// дополнительные нажатия для выхода из магазина (бывает в магазинах необходимо что-то нажать дополнительно, чтобы выйти)
            /// </summary>
            public abstract void ExitFromTrader();

            /// <summary>
            /// нажать Sell и  Ok в меню входа в магазин (зависит от города)
            /// </summary>
            public abstract void SellAndOk();

        #endregion

        #region Map

        /// <summary>
        /// нажимаем на голову Lucia 
        /// </summary>
        public void PressHeadOfLucia()
        {
            pointHeadOfLucia.PressMouseL();
            //new Point(349 - 5 + xx, 445 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// нажимаем на место немного в стороне от Lucia 
        /// </summary>
        public void PressLucia1()
        {
            pointLucia1.PressMouseL();
        }

        /// <summary>
        /// нажимаем на Lucia в списке на карте и далее кнопка Move
        /// </summary>
        public void PressLuciaOnMap()
        {
            MapPressString(NumberOfLuciaOnMap);
            ClickMoveMap();
        }

        /// <summary>
        /// на карте города в списке справа нажимаем на строку с номером N
        /// </summary>
        /// <param name="N">номер строки в списке</param>
        public void MapPressString(int N)
        {
            new Point(FirstStringOfMap.getX(), FirstStringOfMap.getY() + (N - 1) * 15).DoubleClickL();
        }

        /// <summary>
        /// тыкаем на открытой карте в строчку со старым мужиком
        /// </summary>
        public void PressOldManonMap()
            {
                pointOldManOnMap.DoubleClickL();
            }

        /// <summary>
        /// открыть вторую закладку в уже открытой карте Alt+Z       
        /// </summary>
        public void SecondBookmark()
        {
            pointBookmark.PressMouseLL();
//                pointBookmark.PressMouseL();
        }

        /// <summary>
        /// переход к торговцу, который стоит рядом с нужным нам торговцем. карта местности Alt+Z открыта 
        /// </summary>
        public void GoToTraderMap()
        {
            pointTraderOnMap.PressMouseLL();
        }

        /// <summary>
        /// нажимаем на кнопку "Move" в открытой карте Alt+Z             
        /// </summary>
        public void ClickMoveMap()
        {
            pointButtonMoveOnMap.DoubleClickL();
        }

        /// <summary>
        /// проверяет, открыта ли карта Alt+Z в городе                                        
        /// </summary>
        /// <returns> true, если карта уже открыта </returns>
        public bool isOpenMap()
        {
            return pointOpenMap1.isColor() && pointOpenMap2.isColor();
        }

        /// <summary>
        /// проверяет, открыласть ли вторая закладка карты местности (Alt + Z)          
        /// </summary>
        /// <returns> true, если вторая закладка открыта </returns>
        public bool isSecondBookmark()
        {
            return ((pointBookmark1.isColor()) && (pointBookmark2.isColor()));
        }

        #endregion

    }
}
