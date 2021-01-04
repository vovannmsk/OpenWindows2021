using GEBot.Data;


namespace OpenGEWindows
{
    public class SingTownKatovia : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownKatovia(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 5000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                      //не проверено
            this.pointBookmark = new Point(785 - 5 + xx, 146 - 5 + yy);                        //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(727 - 5 + xx, 242 - 5 + yy);                    //торговец на карте          проверено                        ===========постоянная коррекция  шаг 15
            this.pointButtonMoveOnMap = new Point(832 - 5 + xx, 637 - 5 + yy);                //кнопка Move на карте     //проверено
//            this.pointHeadTrader = new Point(561 - 5 + xx, 282 - 5 + yy);                     //голова торговца          // проверено
            //this.pointHeadTrader = new Point(739 - 130 + xx, 338 - 130 + yy);                     //голова торговца          //проверено
            //this.pointHeadTrader = new Point(736 - 130 + xx, 352 - 130 + yy);                     //голова торговца          //проверено
              this.pointHeadTrader = new Point(595 - 5 + xx, 212 - 5 + yy);                     //голова торговца          //проверено

            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //не нужно
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(716 - 5 + xx, 140 - 5 + yy, 7900000, 5);       //проверено
            this.pointOpenMap2 = new PointColor(717 - 5 + xx, 140 - 5 + yy, 7900000, 5);       //проверено
            this.pointBookmark1 = new PointColor(870 - 5 + xx, 41 - 5 + yy, 7700000, 5);       //не нужно
            this.pointBookmark2 = new PointColor(871 - 5 + xx, 41 - 5 + yy, 7700000, 5);       //не нужно
            this.pointOpenTownTeleport1 = new PointColor(100 - 5 + xx, 295 - 5 + yy, 13000000, 5);  //не нужно
            this.pointOpenTownTeleport2 = new PointColor(100 - 5 + xx, 296 - 5 + yy, 13000000, 5);  //не нужно

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
