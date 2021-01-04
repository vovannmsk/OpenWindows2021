using GEBot.Data;


namespace OpenGEWindows
{
    public class AmericaTownCoimbra: Town
    {
  
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public AmericaTownCoimbra(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 10000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(546 - 5 + xx, 503 - 5 + yy);       //546 - 5, 503 - 5);
            this.pointBookmark = new Point(875 + xx, 45 + yy);                  //875, 45);
            this.pointTraderOnMap = new Point(875 + xx, 200 + yy);              //875, 200);
            this.pointButtonMoveOnMap = new Point(925 + xx, 723 + yy);          //925, 723);
            this.pointHeadTrader = new Point(512 + xx, 530 + yy);               //512, 530);
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);
            this.pointExitFromTrader = new Point(902 + xx, 675 + yy);           //902, 675);

            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(801 - 5 + xx, 46 - 5 + yy, 16440000, 4);    //801 - 5, 46 - 5, 16440000, 804 - 5, 46 - 5, 16510000, 4);
            this.pointOpenMap2 = new PointColor(804 - 5 + xx, 46 - 5 + yy, 16510000, 4);
            this.pointBookmark1 = new PointColor(850 - 5 + xx, 43 - 5 + yy, 7700000, 4);    //850 - 5, 43 - 5, 7700000, 860 - 5, 43 - 5, 7700000, 4);
            this.pointBookmark2 = new PointColor(860 - 5 + xx, 43 - 5 + yy, 7700000, 4);
            this.pointOpenTownTeleport1 = new PointColor(105 - 5 + xx, 292 - 5 + yy, 12500000, 4); //105 - 5, 292 - 5, 12500000, 105 - 5, 296 - 5, 13030000, 4);
            this.pointOpenTownTeleport2 = new PointColor(105 - 5 + xx, 296 - 5 + yy, 13030000, 4);
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
            //коимбра
  //            pointExitFromTrader.PressMouseL();
            dialog.PressOkButton(1);      ////========= тыкаем в OK =======================
        }

    }
}
