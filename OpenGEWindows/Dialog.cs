namespace OpenGEWindows
{
    public abstract class Dialog : Server2
    {
        protected iPoint ButtonOkDialog;
        protected iPointColor pointDialog1;
        protected iPointColor pointDialog2;
        protected iPointColor pointDialog3;
        protected iPointColor pointDialog4;
        // ============  методы  ========================

        /// <summary>
        /// Диалог у Lucia. Красное слово Serendbite в диалоге
        /// </summary>
        /// <returns></returns>
        public bool isRedSerendbite()
        {
            return  new PointColor(553 - 5 + xx, 638 - 5 + yy, 4670000, 4).isColor() &&
                    new PointColor(553 - 5 + xx, 639 - 5 + yy, 4670000, 4).isColor();
        }

        /// <summary>
        /// проверяем, находимся ли мы в диалоге
        /// </summary>
        public bool isDialog()
        {
            return (pointDialog1.isColor() && pointDialog2.isColor()) || 
                (pointDialog3.isColor() && pointDialog4.isColor());
        }

        /// <summary>
        /// проверяем, находимся ли мы в диалоге
        /// </summary>
        public bool isDialogClassic()
        {
            return new PointColor(793 - 5 + xx, 389 - 5 + yy, 8800000, 5).isColor() &&
                    new PointColor(795 - 5 + xx, 389 - 5 + yy, 8800000, 5).isColor();
        }

        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public void PressStringDialogClassic(int number)
        {
            iPoint pointString = new Point(828 - 5 + xx, 338 - 5 + yy - (number - 1) * 19);   //22-11
            pointString.PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// нажимаем на кнопку Ок в диалоге указанное количество раз. GE Classic
        /// </summary>
        /// <param name="number">количество нажатий</param>
        public void PressOkButtonClassic(int number)
        {
            for (int j = 1; j <= number; j++)
            {
                if (isDialogClassic())
                {
                    new Point(955 - 5 + xx, 368 - 5 + yy).PressMouseL();    // Нажимаем на Ok в диалоге
                    Pause(1500);
                }
            }
        }



        /// <summary>
        /// нажимаем на кнопку Ок в диалоге указанное количество раз
        /// </summary>
        /// <param name="number">количество нажатий</param>
        public void PressOkButton(int number)
        {
            for (int j = 1; j <= number; j++)
            {
                //ButtonOkDialog.DoubleClickL();    // Нажимаем на Ok в диалоге
                if (isDialog())
                {
                    ButtonOkDialog.PressMouseL();    // Нажимаем на Ok в диалоге
                    Pause(1500);
                }
            }
        }



        /// <summary>
        /// нажать указанную строку в диалоге. Отсчет снизу вверх
        /// </summary>
        /// <param name="number"></param>
        public abstract void PressStringDialog(int number);

    }
}
