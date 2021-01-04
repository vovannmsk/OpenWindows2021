using GEBot.Data;

namespace OpenGEWindows
{
    public class SingTownBagama : Town
    {

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownBagama(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 6000;
            this.TELEPORT_N = 3;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(538 - 5 + xx, 464 - 5 + yy);
            this.pointBookmark = new Point(800 - 5 + xx, 145 - 5 + yy);
            this.pointTraderOnMap = new Point(745 - 5 + xx, 288 - 5 + yy);
            this.pointButtonMoveOnMap = new Point(822 + xx, 631 + yy);   //822, 631);
            this.pointHeadTrader = new Point(611 + xx, 411 + yy);      //611, 411);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(695 - 5 + xx, 143 - 5 + yy, 16700000, 5);                       //проверено
            this.pointOpenMap2 = new PointColor(695 - 5 + xx, 144 - 5 + yy, 16700000, 5);                       //проверено
            this.pointBookmark1 = new PointColor(775 - 5 + xx, 140 - 5 + yy, 7900000, 5);                      //проверено
            this.pointBookmark2 = new PointColor(776 - 5 + xx, 140 - 5 + yy, 7900000, 5);                      //проверено
            this.pointOpenTownTeleport1 = new PointColor(105 - 5 + xx, 296 - 5 + yy, 12000000, 6);             //проверено
            this.pointOpenTownTeleport2 = new PointColor(105 - 5 + xx, 297 - 5 + yy, 13000000, 6);             //проверено

            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }


        // ===========  Методы ================== 
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
