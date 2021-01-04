using GEBot.Data;

namespace OpenGEWindows
{
    public class AmericaTownArmonia : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public AmericaTownArmonia(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 2000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                                                //проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                        //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(458 - 5 + xx, 464 - 5 + yy);                     //торговец на карте        //проверено                     ===========постоянная коррекция
            this.pointButtonMoveOnMap = new Point(930 - 5 + xx, 728 - 5 + yy);                //кнопка Move на карте    //проверено
//            this.pointHeadTrader = new Point(550 - 5 + xx, 381 - 5 + yy);                     //голова торговца         //проверено
            this.pointHeadTrader = new Point(268 - 5 + xx, 328 - 5 + yy);                     //голова торговца         //проверено
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //не проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(843 - 5 + xx, 44 - 5 + yy, 15800000, 5);            //проверено
            this.pointOpenMap2 = new PointColor(843 - 5 + xx, 45 - 5 + yy, 15800000, 5);            //проверено
            this.pointBookmark1 = new PointColor(861 - 5 + xx, 40 - 5 + yy, 7700000, 5);            //проверено
            this.pointBookmark2 = new PointColor(862 - 5 + xx, 40 - 5 + yy, 7700000, 5);            //проверено
            this.pointOpenTownTeleport1 = new PointColor(97 - 5 + xx, 284 - 5 + yy, 8000000, 5);   //проверено
            this.pointOpenTownTeleport2 = new PointColor(98 - 5 + xx, 284 - 5 + yy, 8000000, 5);   //проверено

            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }

        /// <summary>
        /// нажать Sell и  Ok в меню входа в магазин
        /// </summary>
        public override void SellAndOk()
        {
            dialog.PressStringDialog(4);  ////========= тыкаем в "Sell/Buy Items" ======================================
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
