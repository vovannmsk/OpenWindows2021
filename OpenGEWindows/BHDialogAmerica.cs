namespace OpenGEWindows
{
    public class BHDialogAmerica : BHDialog
    {
        public BHDialogAmerica()
        { }

        public BHDialogAmerica(botWindow botwindow) 
        {

            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            this.ButtonOkDialog = new Point(953 - 5 + xx, 369 - 5 + yy);                           //нажимаем на Ок в диалоге

        }

        // ======================  методы  ========================

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
