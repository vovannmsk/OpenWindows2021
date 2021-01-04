namespace OpenGEWindows
{
    public class DialogEuropa : Dialog
    {
        public DialogEuropa()
        { }

        public DialogEuropa(botWindow botwindow)
        {

            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            this.ButtonOkDialog = new Point(910 - 5 + xx, 680 - 5 + yy);                           //нажимаем на Ок в диалоге
            this.pointDialog1 = new PointColor(907 - 5 + xx, 675 - 5 + yy, 7800000, 5);
            this.pointDialog2 = new PointColor(907 - 5 + xx, 676 - 5 + yy, 7800000, 5);

        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет с низу вверх
        /// </summary>
        /// <param name="number"></param>
        public override void PressStringDialog(int number)
        {
            iPoint pointString = new Point(520 - 5 + xx, 660 - 5 + yy - (number - 1) * 20);
            pointString.PressMouseLL();
            Pause(1000);
        }


    }
}
