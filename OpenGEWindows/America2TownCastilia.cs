using GEBot.Data;


namespace OpenGEWindows
{
    public class America2TownCastilia : Town
    {

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public America2TownCastilia(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 12000;
            this.TELEPORT_N = 4;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);
            this.pointBookmark = new Point(800 - 5 + xx, 145 - 5 + yy);                                              // проверено
            this.pointTraderOnMap = new Point(760 - 5 + xx, 300 - 5 + yy);                                              // проверено
            this.pointButtonMoveOnMap = new Point(835 - 5 + xx, 635 - 5 + yy);                                              //проверено
            this.pointHeadTrader = new Point(76 - 5 + xx, 160 - 5 + yy);                                              //не проверено
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);               //проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(695 - 5 + xx, 143 - 5 + yy, 16700000, 5);                    //проверено
            this.pointOpenMap2 = new PointColor(695 - 5 + xx, 144 - 5 + yy, 16700000, 5);                    //проверено
            this.pointBookmark1 = new PointColor(778 - 5 + xx, 140 - 5 + yy, 7900000, 5);                     //проверено
            this.pointBookmark2 = new PointColor(779 - 5 + xx, 140 - 5 + yy, 7900000, 5);                     //проверено
            this.pointOpenTownTeleport1 = new PointColor(95 - 5 + xx, 298 - 5 + yy, 13000000, 6);           // работает
            this.pointOpenTownTeleport2 = new PointColor(95 - 5 + xx, 299 - 5 + yy, 13000000, 6);

            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }

        /// <summary>
        /// нажать Sell и  Ok в меню входа в магазин
        /// </summary>
        public override void SellAndOk()
        {
            dialog.PressStringDialog(2);  ////========= тыкаем в "Sell/Buy Items" ======================================
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
