using GEBot.Data;


namespace OpenGEWindows
{
    public class SingTownKielce : Town
    {

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownKielce(botWindow botwindow)   
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 6000;
            this.TELEPORT_N = 1;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);
            this.pointBookmark = new Point(890 - 5 + xx, 48 - 5 + yy);                                              // проверено

            const int TraderLine = 9;    //номер строки торговца в списке на карте
            this.pointTraderOnMap = new Point(840 - 5 + xx, 69 - 5 + (TraderLine - 1) * 15 + yy);                   // проверено

            this.pointButtonMoveOnMap = new Point(935 - 5 + xx, 728 - 5 + yy);                                      //проверено
            this.pointHeadTrader = new Point(289 - 5 + xx, 450 - 5 + yy);                                            //проверено
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);                 //проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(868 - 5 + xx, 30 - 5 + yy, 8100000, 5);                     //проверено
            this.pointOpenMap2 = new PointColor(869 - 5 + xx, 30 - 5 + yy, 8100000, 5);                     //проверено
            this.pointBookmark1 = new PointColor(873 - 5 + xx, 41 - 5 + yy, 7700000, 5);                    //проверено
            this.pointBookmark2 = new PointColor(874 - 5 + xx, 41 - 5 + yy, 7700000, 5);                    //проверено
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
