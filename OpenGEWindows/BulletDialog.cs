namespace OpenGEWindows
{
    public abstract class BulletDialog : Server2
    {
        protected iPoint ButtonOkDialog;
        protected iPointColor pointDialog1;
        protected iPointColor pointDialog2;

        // ============  методы  ========================

        /// <summary>
        /// проверяем, находимся ли мы в диалоге
        /// </summary>
        public bool isDialog()
        {
            return (pointDialog1.isColor() && pointDialog2.isColor());
        }

        /// <summary>
        /// нажимаем на кнопку Ок в диалоге указанное количество раз
        /// </summary>
        /// <param name="number">количество нажатий</param>
        public void PressOkButton(int number)
        {
            for (int j = 1; j <= number; j++)
            {
                ButtonOkDialog.DoubleClickL();    // Нажимаем на Ok в диалоге
                Pause(1500);
            }
        }

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет с низу вверх
        /// </summary>
        /// <param name="number"></param>
        public abstract void PressStringDialog(int number);

    }
}
