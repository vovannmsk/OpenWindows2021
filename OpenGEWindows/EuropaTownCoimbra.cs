using GEBot.Data;


namespace OpenGEWindows
{
    public class EuropaTownCoimbra : Town
    {

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public EuropaTownCoimbra(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 5000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);
            this.pointBookmark = new Point(875 + xx, 45 + yy);
            this.pointTraderOnMap = new Point(880 - 5 + xx, 220 - 5 + yy);
            this.pointButtonMoveOnMap = new Point(925 + xx, 723 + yy);
            this.pointHeadTrader = new Point(516 - 5 + xx, 524 - 5 + yy);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);
            this.pointExitFromTrader = new Point(906 - 5 + xx, 679 - 5 + yy);
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(500 - 5 + xx, 30 - 5 + yy, 7700000, 4);
            this.pointOpenMap2 = new PointColor(501 - 5 + xx, 30 - 5 + yy, 7700000, 4);
            this.pointBookmark1 = new PointColor(880 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            this.pointBookmark2 = new PointColor(881 - 5 + xx, 41 - 5 + yy, 7700000, 4);
            this.pointOpenTownTeleport1 = new PointColor(93 - 5 + xx, 284 - 5 + yy, 8030000, 4);         // работает
            this.pointOpenTownTeleport2 = new PointColor(94 - 5 + xx, 284 - 5 + yy, 8030000, 4);
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
            Pause(1500);
            pointExitFromTrader.PressMouseL();

        }





    }
}
