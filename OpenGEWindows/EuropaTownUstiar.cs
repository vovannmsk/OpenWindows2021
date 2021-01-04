using GEBot.Data;


namespace OpenGEWindows
{
    public class EuropaTownUstiar : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public EuropaTownUstiar(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 2000;
            this.TELEPORT_N = 4;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                                           //проверено
            this.pointBookmark = new Point(686 - 5 + xx, 74 - 5 + yy);                                              //проверено
            this.pointTraderOnMap = new Point(663 - 5 + xx, 245 - 5 + yy);                                              //проверено
            this.pointButtonMoveOnMap = new Point(728 + xx, 563 + yy);                                              //проверено
            this.pointHeadTrader = new Point(671 - 5 + xx, 440 - 5 + yy);                                             //проверено
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);                 //проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(740 - 5 + xx, 562 - 5 + yy, 7660000, 4);                    //проверено
            this.pointOpenMap2 = new PointColor(740 - 5 + xx, 563 - 5 + yy, 7660000, 4);                    //проверено
            this.pointBookmark1 = new PointColor(706 - 5 + xx, 68 - 5 + yy, 7900000, 5);                    //проверено
            this.pointBookmark2 = new PointColor(707 - 5 + xx, 68 - 5 + yy, 7900000, 5);                    //проверено
            //this.pointOpenTownTeleport1 = new PointColor(105 - 5 + xx, 292 - 5 + yy, 12500000, 4);
            //this.pointOpenTownTeleport2 = new PointColor(105 - 5 + xx, 296 - 5 + yy, 13030000, 4);
            this.pointOpenTownTeleport1 = new PointColor(98 - 5 + xx, 296 - 5 + yy, 12500000, 5);             //проверено
            this.pointOpenTownTeleport2 = new PointColor(98 - 5 + xx, 297 - 5 + yy, 12500000, 5);              //проверено
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
