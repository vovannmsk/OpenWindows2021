namespace OpenGEWindows
{
    public class BulletDialogSing : BulletDialog
    {
        public BulletDialogSing ()
        {}

        public BulletDialogSing (botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            this.ButtonOkDialog = new Point(978 - 30 + xx, 394 - 30 + yy);                           //кнопка Ок в диалоге
            this.pointDialog1 = new PointColor(979 - 30 + xx, 390 - 30 + yy, 7700000, 5);
            this.pointDialog2 = new PointColor(979 - 30 + xx, 391 - 30 + yy, 7700000, 5);
        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public override void PressStringDialog(int number)
        {
//            iPoint pointString = new Point(829 - 30 + xx, 230 - 30 + yy + (number - 1) * 19);   //сверху вниз
            iPoint pointString = new Point(829 - 30 + xx, 363 - 30 + yy - (number - 1) * 19);   //снизу вверх

            pointString.PressMouseLL();
            Pause(1000);
        }



    }
}
