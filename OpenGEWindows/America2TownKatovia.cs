using GEBot.Data;


namespace OpenGEWindows
{
    public class America2TownKatovia : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public America2TownKatovia(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 5000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                      //проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                        //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(880 - 5 + xx, 280 - 5 + yy);                    //торговец на карте                          ===========постоянная коррекция  шаг 15
            this.pointButtonMoveOnMap = new Point(930 - 5 + xx, 728 - 5 + yy);                //кнопка Move на карте    проверено
//            this.pointHeadTrader = new Point(561 - 5 + xx, 282 - 5 + yy);                     //голова торговца          // проверено
            //this.pointHeadTrader = new Point(595 - 5 + xx, 212 - 5 + yy);                     //голова торговца          //проверено
            this.pointHeadTrader = new Point(736 - 130 + xx, 352 - 130 + yy);                     //голова торговца          //проверено
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);
            //this.pointOkOnMenu = new Point(907 - 5 + xx, 679 - 5 + yy);
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(795 - 5 + xx, 43 - 5 + yy, 16000000, 6);      //проверено
            this.pointOpenMap2 = new PointColor(795 - 5 + xx, 44 - 5 + yy, 15000000, 6);      //проверено
            this.pointBookmark1 = new PointColor(870 - 5 + xx, 47 - 5 + yy, 16000000, 6);      //проверено
            this.pointBookmark2 = new PointColor(870 - 5 + xx, 48 - 5 + yy, 16000000, 6);      //проверено
            this.pointOpenTownTeleport1 = new PointColor(100 - 5 + xx, 295 - 5 + yy, 13000000, 5);  //проверено
            this.pointOpenTownTeleport2 = new PointColor(100 - 5 + xx, 296 - 5 + yy, 13000000, 5);  //проверено

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
            dialog.PressOkButton(1);      ////========= тыкаем в OK =======================
        }
    }
}
