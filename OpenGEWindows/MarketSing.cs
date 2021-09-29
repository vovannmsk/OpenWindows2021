using GEBot.Data;

namespace OpenGEWindows
{
    public class MarketSing : Market
    {
        public MarketSing ()
        {}

        public MarketSing(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            #region Shop

            this.pointIsSale1 = new PointColor(912 - 5 + xx, 681 - 5 + yy, 7700000, 5);     //проверено
            this.pointIsSale2 = new PointColor(912 - 5 + xx, 682 - 5 + yy, 7700000, 5);

            this.pointIsSale21 = new PointColor(846 - 5 + xx, 668 - 5 + yy, 7900000, 5);              //для isSale2  по кнопке Close
            this.pointIsSale22 = new PointColor(846 - 5 + xx, 669 - 5 + yy, 7900000, 5);     //проверено

            this.pointIsClickSale1 = new PointColor(736 - 5 + xx, 670 - 5 + yy, 7900000, 5);          //нажата ли закладка "Sell" (по кнопке Sell)
            this.pointIsClickSale2 = new PointColor(736 - 5 + xx, 671 - 5 + yy, 7900000, 5);        //проверено

            this.pointIsClickPurchase1 = new PointColor(730 - 5 + xx, 666 - 5 + yy, 7900000, 5);      //нажата ли закладка "Purchase"
            this.pointIsClickPurchase2 = new PointColor(730 - 5 + xx, 667 - 5 + yy, 7900000, 5);

            this.pointBookmarkSell = new Point(247 + xx, 173 + yy);     //проверено
            this.pointButtonBUY = new Point(733 + xx, 672 + yy);            //проверено
            this.pointButtonSell = new Point(733 + xx, 672 + yy);           //проверено
            this.pointButtonClose = new Point(857 + xx, 672 + yy);          //проверено
            this.pointAddProduct = new Point(384 - 5 + xx, 224 - 5 + yy);   //проверено


            //не используется
            this.pointWheelDown = new Point(375 + xx, 220 + yy);            //345 + 30 + botwindow.getX(), 190 + 30 + botwindow.getY(), 3);        // колесо вниз
            this.pointBuyingMitridat1 = new Point(360 + xx, 537 + yy);      //360, 537
            this.pointBuyingMitridat2 = new Point(517 + xx, 433 + yy);      //1392 - 875, 438 - 5
            this.pointBuyingMitridat3 = new Point(517 + xx, 423 + yy);      //1392 - 875, 428 - 5
            this.pointSaleToTheRedBottle = new Point(335 + xx, 220 + yy);
            this.pointSaleOverTheRedBottle = new Point(335 + xx, 220 + yy);


            #endregion


            DialogFactory dialogFactory = new DialogFactory(this.botwindow);
            dialog = dialogFactory.createDialog();
            this.globalParam = new GlobalParam();
            this.botParam = new BotParam(botwindow.getNumberWindow());
        }

    }
}
