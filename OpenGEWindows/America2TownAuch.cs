using GEBot.Data;


namespace OpenGEWindows
{
    public class America2TownAuch : Town
    {

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public America2TownAuch(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 3000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                      //проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                        //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(840 - 5 + xx, 220 - 5 + yy);                    //торговец на карте                         Firearms  Lorch
            this.pointButtonMoveOnMap = new Point(930 - 5 + xx, 728 - 5 + yy);                //кнопка Move на карте
            this.pointHeadTrader = new Point(296 - 5 + xx, 228 - 5 + yy);                     //голова торговца          // проверено
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //проверено
            //точки для проверки цвета
            //this.pointOpenMap1 = new PointColor(847 - 5 + xx, 44 - 5 + yy, 16000000, 6);      //проверено
            //this.pointOpenMap2 = new PointColor(847 - 5 + xx, 45 - 5 + yy, 16000000, 6);      //проверено
            this.pointOpenMap1 = new PointColor(600 - 5 + xx, 30 - 5 + yy, 7000000, 6);      //проверено
            this.pointOpenMap2 = new PointColor(610 - 5 + xx, 30 - 5 + yy, 7000000, 6);      //проверено
            this.pointBookmark1 = new PointColor(873 - 5 + xx, 41 - 5 + yy, 7700000, 5);      //проверено
            this.pointBookmark2 = new PointColor(874 - 5 + xx, 41 - 5 + yy, 7700000, 5);      //проверено
            this.pointOpenTownTeleport1 = new PointColor(100 - 5 + xx, 295 - 5 + yy, 13000000, 6);  //проверено
            this.pointOpenTownTeleport2 = new PointColor(100 - 5 + xx, 296 - 5 + yy, 13000000, 6);  //проверено 

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
