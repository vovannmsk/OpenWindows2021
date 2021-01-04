using GEBot.Data;

namespace OpenGEWindows
{
    public class AmericaTownUstiar: Town
    {
         /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public AmericaTownUstiar(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 3000;
            this.TELEPORT_N = 4;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 483 - 5 + yy);                    //545 - 5, 483 - 5);
            this.pointBookmark = new Point(775 + xx, 141 + yy);                              //775, 141);
            this.pointTraderOnMap = new Point(730 + xx, 313 + yy);                          //  одна строчка от другой различается на 15 пикселей
            this.pointButtonMoveOnMap = new Point(822 + xx, 631 + yy);                      //822, 631);
            this.pointHeadTrader = new Point(681 + xx, 426 + yy);                           //681, 426);
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(692 - 5 + xx, 141 - 5 + yy, 16710000, 4);    //692 - 5, 141 - 5, 16710000, 695 - 5, 147 - 5, 15920000, 4);
            this.pointOpenMap2 = new PointColor(695 - 5 + xx, 147 - 5 + yy, 15920000, 4);
            this.pointBookmark1 = new PointColor(830 - 5 + xx, 140 - 5 + yy, 7900000, 5);                      //проверено
            this.pointBookmark2 = new PointColor(831 - 5 + xx, 140 - 5 + yy, 7900000, 5);                       //проверено
            this.pointOpenTownTeleport1 = new PointColor(105 - 5 + xx, 292 - 5 + yy, 12000000, 6);             //проверено
            this.pointOpenTownTeleport2 = new PointColor(105 - 5 + xx, 296 - 5 + yy, 13000000, 6);              //проверено
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
