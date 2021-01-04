using GEBot.Data;


namespace OpenGEWindows
{
    public class AmericaTownBagama: Town
    {
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public AmericaTownBagama(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 6000;
            this.TELEPORT_N = 3;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(538 - 5 + xx, 464 - 5 + yy);
            this.pointBookmark = new Point(775 + xx, 141 + yy);
            this.pointTraderOnMap = new Point(735 - 5 + xx, 288 - 5 + yy);                       //730, 268);
            this.pointButtonMoveOnMap = new Point(822 + xx, 631 + yy);   //822, 631);
            this.pointHeadTrader = new Point(611 + xx, 411 + yy);      //611, 411);
            this.pointTownTeleport = new Point(110 + xx, 328 + (TELEPORT_N - 1) * 30 + yy);
            
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(812 - 5 + xx, 632 - 5 + yy, 7920000, 4);                       //проверено
            this.pointOpenMap2 = new PointColor(812 - 5 + xx, 633 - 5 + yy, 7920000, 4);                       //проверено
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
