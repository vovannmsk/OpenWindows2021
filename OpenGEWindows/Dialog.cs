namespace OpenGEWindows
{
    public abstract class Dialog : Server2
    {
        protected iPoint ButtonOkDialog;
        protected iPointColor pointDialog1;
        protected iPointColor pointDialog2;

        // ============  методы  ========================

        /// <summary>
        /// Диалог у Lucia. Красное слово Serendbite в диалоге
        /// </summary>
        /// <returns></returns>
        public bool isRedSerendbite()
        {
            return  new PointColor(595 - 5 + xx, 631 - 5 + yy, 4210000, 4).isColor() &&
                    new PointColor(595 - 5 + xx, 632 - 5 + yy, 4210000, 4).isColor();
        }

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
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public abstract void PressStringDialog(int number);

    }
}
