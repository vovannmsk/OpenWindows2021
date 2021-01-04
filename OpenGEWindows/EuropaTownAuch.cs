using GEBot.Data;


namespace OpenGEWindows
{
    public class EuropaTownAuch : Town
    {
  
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public EuropaTownAuch(botWindow botwindow) 
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 2000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);
            this.pointBookmark = new Point(884-5 + xx, 49 - 5 + yy);                                          // работает
            this.pointTraderOnMap = new Point(843 - 5 + xx, 190 - 5 + yy);                          // работает ***********************************
            this.pointButtonMoveOnMap = new Point(927 - 5 + xx, 729 - 5 + yy);                              // работает
            this.pointHeadTrader = new Point(296 - 5 + xx, 228 - 5 + yy);                                  // не работает
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);                                 // не работает
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);                                // не работает
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);                                // работает
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(804 - 5 + xx, 41 - 5 + yy, 7700000, 4);                    //работает       //проверяем первую закладку
            this.pointOpenMap2 = new PointColor(805 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            this.pointBookmark1 = new PointColor(875 - 5 + xx, 41 - 5 + yy, 7700000, 4);                    //работает       //проверяем вторую закладку
            this.pointBookmark2 = new PointColor(876 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            this.pointOpenTownTeleport1 = new PointColor(93 - 5 + xx, 295 - 5 + yy, 11000000, 6);         //  работает
            this.pointOpenTownTeleport2 = new PointColor(94 - 5 + xx, 295 - 5 + yy, 11000000, 6);
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
