using GEBot.Data;


namespace OpenGEWindows
{
    public class SingTownLosToldos : Town
    {


        public SingTownLosToldos()
        {
        }

        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownLosToldos(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 5000;
            this.TELEPORT_N = 2;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                      //не проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                        //вторая закладка карты   //не проверено
            this.pointTraderOnMap = new Point(880 - 5 + xx, 295- 5 + yy);                    //торговец на карте                          ===========постоянная коррекция
            this.pointButtonMoveOnMap = new Point(835 - 5 + xx, 635 - 5 + yy);                //кнопка Move на карте    проверено
            this.pointHeadTrader = new Point(365 - 5 + xx, 474 - 5 + yy);                     //голова торговца          //не проверено
            //this.pointSellOnMenu = new Point(520 + xx, 654 + yy);
            //this.pointOkOnMenu = new Point(902 + xx, 674 + yy);
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //не проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(695 - 5 + xx, 143 - 5 + yy, 16600000, 5);      //проверено
            this.pointOpenMap2 = new PointColor(695 - 5 + xx, 144 - 5 + yy, 16100000, 5);      //проверено
            this.pointBookmark1 = new PointColor(870 - 5 + xx, 47 - 5 + yy, 16500000, 5);      //не проверено
            this.pointBookmark2 = new PointColor(870 - 5 + xx, 48 - 5 + yy, 16400000, 5);      //не проверено
            this.pointOpenTownTeleport1 = new PointColor(100 - 5 + xx, 295 - 5 + yy, 13000000, 5);  //не проверено
            this.pointOpenTownTeleport2 = new PointColor(100 - 5 + xx, 296 - 5 + yy, 13000000, 5);  //не проверено

            this.pointOldManOnMap = new Point(740 - 5 + xx, 212 - 5 + yy);      //строка на карте Старый мужик 
            this.pointOldMan1 = new Point(531 - 5 + xx, 343 - 5 + yy);          //голова Старого мужика

            DialogFactory tf = new DialogFactory(botwindow);
            this.dialog = tf.createDialog();
            this.globalParam = new GlobalParam();
        }

        // ===========================   Методы  ======================================
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
