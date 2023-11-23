namespace OpenGEWindows
{
    public class DialogSing : Dialog
    {
        public DialogSing ()
        {}

        public DialogSing(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            this.ButtonOkDialog = new Point(939 - 5 + xx, 652 - 5 + yy);                           //нажимаем на Ок в диалоге
            this.pointDialog1 = new PointColor(939 - 5 + xx, 652 - 5 + yy, 11800000, 5);     //22-11
            this.pointDialog2 = new PointColor(939 - 5 + xx, 653 - 5 + yy, 11800000, 5);     //22-11
            this.pointDialog3 = new PointColor(939 - 5 + xx, 652 - 5 + yy, 11900000, 5);     //23-11
            this.pointDialog4 = new PointColor(939 - 5 + xx, 653 - 5 + yy, 11900000, 5);     //23-11
        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public override void PressStringDialog(int number)
        {
            iPoint pointString = new Point(685 - 5 + xx, 493 - 5 + yy - (number - 1) * 28);   //22-11
            pointString.PressMouseLL();
            Pause(1000);
        }



    }
}
