﻿using GEBot.Data;

namespace OpenGEWindows
{
    public class SingTownArmonia : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownArmonia(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 2000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                                        //проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(830 - 5 + xx, 83 - 5 + yy);             //торговец на карте        //проверено        
            this.pointButtonMoveOnMap = new Point(930 - 5 + xx, 728 - 5 + yy);        //кнопка Move на карте    //проверено
            this.pointHeadTrader = new Point(489 - 5 + xx, 243 - 5 + yy);             //голова торговца         //проверено
            this.pointBulletAutomat = new Point(653 - 5 + xx, 514 - 5 + yy);          //автомат с пулями
            this.PAUSE_TIME_Bullet = 7000;
            this.pointTraderOnMapBullet = new Point(830 - 5 + xx, 143 - 5 + yy);      //торговец на карте для перехода к патронам     

            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //не проверено
            //точки для проверки цвета
            //this.pointOpenMap1 = new PointColor(795 - 5 + xx, 45 - 5 + yy, 16700000, 5);          //проверено
            //this.pointOpenMap2 = new PointColor(795 - 5 + xx, 46 - 5 + yy, 16700000, 5);          //проверено
            this.pointOpenMap1 = new PointColor(600 - 5 + xx, 30 - 5 + yy, 8000000, 6);             //проверено
            this.pointOpenMap2 = new PointColor(605 - 5 + xx, 30 - 5 + yy, 8000000, 6);             //проверено
            this.pointBookmark1 = new PointColor(870 - 5 + xx, 42 - 5 + yy, 7700000, 5);            //проверено
            this.pointBookmark2 = new PointColor(871 - 5 + xx, 42 - 5 + yy, 7700000, 5);            //проверено
            this.pointOpenTownTeleport1 = new PointColor(95 - 5 + xx, 297 - 5 + yy, 13000000, 5);   //проверено
            this.pointOpenTownTeleport2 = new PointColor(95 - 5 + xx, 298 - 5 + yy, 13000000, 5);   //проверено

            //для lucia (какашки)
            this.FirstStringOfMap = new Point(815 - 5 + xx, 69 - 5 + yy);
            this.NumberOfLuciaOnMap = 0;

            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }

        /// <summary>
        /// нажать Sell и  Ok в меню входа в магазин
        /// </summary>
        public override void SellAndOk()
        {
            dialog.PressStringDialog(5);  ////========= тыкаем в "Sell/Buy Items" ======================================
            dialog.PressOkButton(1);      ////========= тыкаем в OK =======================

        }

        /// <summary>
        /// дополнительные нажатия для выхода из магазина (бывает в магазинах необходимо что-то нажать дополнительно, чтобы выйти)
        /// </summary>
        public override void ExitFromTrader()
        {


        }
    }
}
