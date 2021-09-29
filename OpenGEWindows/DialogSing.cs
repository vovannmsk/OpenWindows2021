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

            this.ButtonOkDialog = new Point(913 - 5 + xx, 683 - 5 + yy);                           //нажимаем на Ок в диалоге
            //this.pointDialog1 = new PointColor(907 - 5 + xx, 675 - 5 + yy, 7700000, 5);
            //this.pointDialog2 = new PointColor(907 - 5 + xx, 676 - 5 + yy, 7700000, 5);
            this.pointDialog1 = new PointColor(912 - 5 + xx, 680 - 5 + yy, 7700000, 5);     //проверено
            this.pointDialog2 = new PointColor(912 - 5 + xx, 681 - 5 + yy, 7700000, 5);     //проверено
        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public override void PressStringDialog(int number)
        {
            iPoint pointString = new Point(520 - 5 + xx, 663 - 5 + yy - (number - 1) * 19);
            pointString.PressMouseLL();
            Pause(1000);
        }



    }
}
