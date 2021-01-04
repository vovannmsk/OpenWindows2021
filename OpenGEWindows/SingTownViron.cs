using GEBot.Data;


namespace OpenGEWindows
{
    public class SingTownViron : Town
    {
 
        /// <summary>
        /// конструктор для класса
        /// </summary>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        public SingTownViron(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.PAUSE_TIME = 10000;
            this.TELEPORT_N = 1;   //номер городского телепорта
            //точки для нажимания на них
            this.pointMaxHeight = new Point(545 - 5 + xx, 500 - 5 + yy);                                                //проверено
            this.pointBookmark = new Point(880 - 5 + xx, 48 - 5 + yy);                        //вторая закладка карты   //проверено
            this.pointTraderOnMap = new Point(880 - 5 + xx, 224 - 130 + yy);                        //торговец на карте Jaina      //проверено                     ===========постоянная коррекция
            this.pointButtonMoveOnMap = new Point(930 - 5 + xx, 728 - 5 + yy);                //кнопка Move на карте    //проверено
            this.pointHeadTrader = new Point(1080 - 130 + xx, 620 - 130 + yy);                      //голова торговца         //проверено
            this.pointTownTeleport = new Point(115 - 5 + xx, 333 - 5 + (TELEPORT_N - 1) * 30 + yy);    //сюда тыкаем, чтобы улететь на торговую улицу   //проверено
            //точки для проверки цвета
            this.pointOpenMap1 = new PointColor(794 - 5 + xx, 45 - 5 + yy, 16700000, 5);            //проверено
            this.pointOpenMap2 = new PointColor(794 - 5 + xx, 46 - 5 + yy, 16700000, 5);            //проверено
            this.pointBookmark1 = new PointColor(870 - 5 + xx, 42 - 5 + yy, 7900000, 5);            //проверено
            this.pointBookmark2 = new PointColor(871 - 5 + xx, 42 - 5 + yy, 7900000, 5);            //проверено
            this.pointOpenTownTeleport1 = new PointColor(95 - 5 + xx, 297 - 5 + yy, 13000000, 5);   //проверено
            this.pointOpenTownTeleport2 = new PointColor(95 - 5 + xx, 298 - 5 + yy, 13000000, 5);   //проверено

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


//        /// <summary>
//        /// проверяет, открыт ли городской телепорт (Alt + F3)                             
//        /// </summary>
//        /// <returns> true, если телепорт  (Alt + F3) открыт </returns>
//        public bool isOpenTownTeleport()
//        {
//            //return isColor2(105 - 5, 292 - 5, 12500000, 105 - 5, 296 - 5, 13030000, 4);
//            return true;
//        }

//        /// <summary>
//        /// удаляем камеру (поднимаем максимально вверх)                           
//        /// </summary>
//        public void MaxHeight()
//        {
//            const int NUMBER_OF_ITERATION = 10;
//            for (int j = 1; j <= NUMBER_OF_ITERATION; j++)
//            {
//                //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(548 - 5 + botwindow.getX(), 479 - 5 + botwindow.getY(), 9);
//                botwindow.PressMouseWheelUp(548 - 5, 479 - 5);
//                botwindow.Pause(200);
//            }
//        }

//        /// <summary>
//        /// перелететь по городскому телепорту на торговую улицу                            
//        /// </summary>
//        public void TownTeleportW()
//        {
//            //ребо
////            TownTeleport(2);
//            pointTownTeleport.PressMouse();

//        }

//        /// <summary>
//        /// проверяет, открыта ли карта Alt+Z в городе                                        
//        /// </summary>
//        /// <returns> true, если карта уже открыта </returns>
//        public bool isOpenMap()
//        {
//            return botwindow.isColor2(801 - 5, 46 - 5, 16440000, 804 - 5, 46 - 5, 16510000, 4);
//        }

//        /// <summary>
//        /// проверяет, открыласть ли вторая закладка карты местности (Alt + Z)          
//        /// </summary>
//        /// <returns> true, если вторая закладка открыта </returns>
//        public bool isSecondBookmark()
//        {
//            return botwindow.isColor2(850 - 5, 43 - 5, 7700000, 860 - 5, 43 - 5, 7700000, 4);
//        }

//        /// <summary>
//        /// открыть вторую закладку в уже открытой карте Alt+Z       
//        /// </summary>
//        public void SecondBookmark()                                                             // сделать проверку, открылась ли вторая закладка
//        {
//            botwindow.PressMouse(875, 45);
//            botwindow.PressMouse(875, 45);
//        }

//        /// <summary>
//        /// переход к торговцу, который стоит рядом с нужным нам торговцем. карта местности Alt+Z открыта 
//        /// </summary>
//        public void GoToTraderMap()
//        {
//            //ребо  Металл трейдер (как в европе)
//            botwindow.Pause(500);
//            botwindow.PressMouse(875, 259);
//        }

//        /// <summary>
//        /// нажимаем на кнопку "Move" в открытой карте Alt+Z             
//        /// </summary>
//        public void ClickMoveMap()
//        {
//            botwindow.Pause(500);
//            botwindow.PressMouse(925, 723);
//            botwindow.Pause(200);
//        }

//        /// <summary>
//        /// Делаем паузу, чтобы бот успел добежать до торговца           
//        /// </summary>
//        public void PauseToTrader()
//        {
//            botwindow.Pause(2000);
//        }

//        /// <summary>
//        /// тыкаем мышкой в голову торговца, чтобы войти в магазин              
//        /// </summary>
//        public void Click_ToHeadTrader()
//        {
//            botwindow.Pause(200);
//            //ребо
//            botwindow.PressMouseL(352, 498);
//            botwindow.Pause(200);
//        }

//        /// <summary>
//        /// Кликаем на строчку Sell и кнопку "Ok" в магазине   
//        /// </summary>
//        public void ClickSellAndOkInTrader()
//        {
//            //ребо, коимбра, ош, багама
//            //========= тыкаем в "Sell/Buy Items" ======================================
//            botwindow.PressMouseL(520, 654);
//            botwindow.Pause(200);
//            botwindow.PressMouseL(520, 654);
//            botwindow.Pause(500);
//            //========= тыкаем в OK =======================
//            botwindow.PressMouseL(902, 674);
//            botwindow.Pause(2500);
//        }

        /// <summary>
        /// дополнительные нажатия для выхода из магазина (бывает в магазинах необходимо что-то нажать дополнительно, чтобы выйти)
        /// </summary>
        public override void ExitFromTrader()
        {


        }
    }
}
