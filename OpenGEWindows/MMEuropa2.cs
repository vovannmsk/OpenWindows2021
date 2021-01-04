namespace OpenGEWindows
{
    public class MMEuropa2 : MM
    {
        public MMEuropa2()
        {}

        public MMEuropa2(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            pointIsMMSell1 = new PointColor(549 - 5 + xx, 606 - 5 + yy, 4370000, 4);
            pointIsMMSell2 = new PointColor(549 - 5 + xx, 607 - 5 + yy, 4370000, 4);
            pointIsMMBuy1 = new PointColor(572 - 5 + xx, 604 - 5 + yy, 7850000, 4);
            pointIsMMBuy2 = new PointColor(572 - 5 + xx, 605 - 5 + yy, 7850000, 4);
            pointGotoBuySell = new Point(670 - 5 + xx, 181 - 5 + yy);
            pointInitializeButton = new Point(670 - 5 + xx, 207 - 5 + yy);
            pointSearchButton = new Point(670 - 5 + xx, 232 - 5 + yy);
            pointSearchString = new Point(430 - 5 + xx, 207 - 5 + yy);
            pointIsFirstString1 = new PointColor(232 - 5 + xx, 292 - 5 + yy, 12000000, 6);
            pointIsFirstString2 = new PointColor(283 - 5 + xx, 301 - 5 + yy, 12000000, 6);
            pointQuantity1 = new Point(225 - 5 + xx, 240 - 5 + yy);
            pointQuantity2 = new Point(185 - 5 + xx, 240 - 5 + yy);
            pointPrice = new Point(185 - 5 + xx, 265 - 5 + yy);
            pointShadow = new Point(485 - 5 + xx, 265 - 5 + yy);
            pointTime = new Point(462 - 5 + xx, 240 - 5 + yy);
            pointTime48Hours = new Point(375 - 5 + xx, 260 - 5 + yy);
            pointTime = new Point(462 - 5 + xx, 240 - 5 + yy);
            pointButtonRegistration = new Point(670 - 5 + xx, 265 - 5 + yy);
            pointYesRegistration = new Point(470 - 5 + xx, 420 - 5 + yy);
            pointOkRegistration = new Point(525 - 5 + xx, 455 - 5 + yy);
            pointIsHideFamily1 = new PointColor(483 - 5 + xx, 267 - 5 + yy, 11700000, 5);
            pointIsHideFamily2 = new PointColor(485 - 5 + xx, 267 - 5 + yy, 11200000, 5);
            //pointFirstStringList = new Point(115 - 5 + xx, 333 - 5 + yy);
            //pointLastStringList = new Point(115 - 5 + xx, 576 - 5 + yy);             //последняя строка списка выставленных на рынок товаров

            product = new Product();



            // ============  методы  ========================


        }
    }
}
