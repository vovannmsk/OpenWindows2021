namespace OpenGEWindows
{
    public class BHDialogAmerica2 : BHDialog
    {
        public BHDialogAmerica2 ()
        {}

        public BHDialogAmerica2 (botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            this.ButtonOkDialog = new Point(953 - 5 + xx, 369 - 5 + yy);                           //нажимаем на Ок в диалоге

            //проверяем наличие кнопки Ок в открытом диалоге
            this.pointsBottonGateBH1 = new PointColor(979 - 30 + xx, 390 - 30 + yy, 7700000, 5);            //Ok
            this.pointsBottonGateBH2 = new PointColor(979 - 30 + xx, 391 - 30 + yy, 7700000, 5);            //Ok

            //проверяем то состояние ворот, где написано "Now. you can try N times for free" (N = 1..5)
            this.pointsGateBH1 = new PointColor(649 - 30 + xx, 310 - 30 + yy, 4600000, 5);            //Possible

            //проверяем то состояние ворот, где написано "You cannot ener for free today"
            this.pointsGateBH3 = new PointColor(716 - 30 + xx, 249 - 30 + yy, 13000000, 6);           //Next

            pointInputBox = new Point(310 - 30 + xx, 675 - 30 + yy);                                    //нажимаем на поле ввода
            pointInputBoxBottonOk = new Point(933 - 30 + xx, 704 - 30 + yy);                            //нажимаем на Ок в диалоге (Initialize)

            //проверяем экран, на котором надо вводить слово Initialize
            this.pointIsInitialize1 = new PointColor(673 - 5 + xx, 613 - 5 + yy, 4671486, 0);         // буква I в слове Initialize
            this.pointIsInitialize2 = new PointColor(673 - 5 + xx, 614 - 5 + yy, 4671486, 0);         // буква I в слове Initialize

        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет с низу вверх
        /// </summary>
        /// <param name="number"></param>
        public override void PressStringDialog(int number)
        {
            iPoint pointString = new Point(814 - 5 + xx, 338 - 5 + yy - (number - 1) * 19);
            pointString.PressMouseLL();
            Pause(1000);
        }



    }
}
