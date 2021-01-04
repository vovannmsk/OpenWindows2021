using GEBot.Data;

namespace OpenGEWindows
{
    public class EuropaTownReboldo : Town
    {
 

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public EuropaTownReboldo(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX(); 
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 3000;                                                                                         // работает
            this.TELEPORT_N = 2;   //номер городского телепорта                                                                 // работает
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);
            this.pointBookmark = new Point(871 - 5 + xx, 49 - 5 + yy);                                          // работает
            this.pointTraderOnMap = new Point(857 - 5 + xx, 280 - 5 + yy);                              // работает   +15 пискелей одна строчка
            this.pointButtonMoveOnMap = new Point(927 - 5 + xx, 728 - 5 + yy);                                  // работает
            this.pointHeadTrader = new Point(357 + xx, 502 + yy);                                       // работает
            this.pointBulletAutomat = new Point(440 - 30 + xx, 375 - 30 + yy);                //автомат с пулями
            this.PAUSE_TIME_Bullet = 3000;

            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);                                     // работает
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);                                       // работает
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);                                // работает
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(808 - 5 + xx, 41 - 5 + yy, 7700000, 4);                    //работает      
            this.pointOpenMap2 = new PointColor(809 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            this.pointBookmark1 = new PointColor(856 - 5 + xx, 41 - 5 + yy, 7700000, 4);                    //работает       
            this.pointBookmark2 = new PointColor(857 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            //this.pointBookmark1 = new PointColor(932 - 5 + xx, 255 - 5 + yy, 16300000, 5);                 //  работает  //
            //this.pointBookmark2 = new PointColor(931 - 5 + xx, 255 - 5 + yy, 16400000, 5);
            this.pointOpenTownTeleport1 = new PointColor(93 - 5 + xx, 295 - 5 + yy, 11710000, 4);         // работает
            this.pointOpenTownTeleport2 = new PointColor(94 - 5 + xx, 295 - 5 + yy, 11710000, 4);
            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }

        /// <summary>
        /// нажать Sell и  Ok в меню входа в магазин
        /// </summary>
        public override void SellAndOk()
        {
            dialog.PressStringDialog(1);  ////========= тыкаем в "Sell/Buy Items" ======================================
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
