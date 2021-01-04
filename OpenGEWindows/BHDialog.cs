using System.Windows.Forms;


namespace OpenGEWindows
{
    public abstract class BHDialog : Server2
    {
        protected iPoint ButtonOkDialog;
        protected iPoint pointInputBox;
        protected iPoint pointInputBoxBottonOk;
        protected iPointColor pointsBottonGateBH1;
        protected iPointColor pointsBottonGateBH2;
        protected iPointColor pointsGateBH1;
        protected iPointColor pointsGateBH3;
        protected iPointColor pointsIsLess11_1;
        protected iPointColor pointsIsLevelfrom11to19_1;
        protected iPointColor pointsIsLevelfrom11to19_2;
        protected iPointColor pointsIsLevelAbove20_1;
        protected iPointColor pointsIsLevelAbove20_2;
        protected iPointColor pointIsInitialize1;
        protected iPointColor pointIsInitialize2;

        // ============  методы  ========================

        /// <summary>
        /// проверяем, находимся ли в воротах в Infinity (Гильдии Охотников)
        /// </summary>
        /// <returns></returns>
        public bool isGateBH()
        {
            //ворота могут находиться в разных состояниях. нужно проверить их все
            return isGateBH1() || isGateBH3();   //при переходе из BH к воротам может получиться только состояние №1 или №3.
                                                 //состояние №1 бывает при первых пяти бесплатных заходах в миссию. 
                                                 //Состояние №3 бывает во всех остальных случаях
        }

        /// <summary>
        /// проверить, лежит ли уровень ворот в диапозоне 1-10
        /// </summary>
        /// <returns></returns>
        public bool isGateLevelLessThan11()
        {
            return pointsIsLess11_1.isColor(); 
        }

        /// <summary>
        /// проверить, лежит ли уровень ворот в диапозоне 11-19
        /// </summary>
        /// <returns></returns>
        public bool isGateLevelFrom11to19()
        {
            return pointsIsLevelfrom11to19_1.isColor() && pointsIsLevelfrom11to19_2.isColor();
        }

        /// <summary>
        /// проверить, лежит ли уровень ворот в диапозоне выше 20
        /// </summary>
        /// <returns></returns>
        public bool isGateLevelAbove20()
        {
            return pointsIsLevelAbove20_1.isColor() && pointsIsLevelAbove20_2.isColor();
        }

        /// <summary>
        /// проверить, активна ли форма, на которой надо вводить слово Initialize
        /// </summary>
        /// <returns></returns>
        public bool isInitialize()
        {
            return pointIsInitialize1.isColor() && pointIsInitialize2.isColor();
        }

        /// <summary>
        /// проверяем, находимся ли в воротах в Infinity (Гильдии Охотников) 
        /// проверяем открыт ли диалог с кнопкой Ок
        /// </summary>
        /// <returns></returns>
        public bool isBottonGateBH()
        {
            return (pointsBottonGateBH1.isColor() && pointsBottonGateBH2.isColor());
        }

        /// <summary>
        /// проверяем, находимся ли в воротах в Infinity (Гильдии Охотников) 
        /// проверяем то состояние ворот, где написано "You currently have 5 tries remaining" когда есть попытки (действия - нажать Ок)
        /// </summary>
        /// <returns></returns>
        public bool isGateBH1()
        {
            return (isBottonGateBH() && pointsGateBH1.isColor());
        }

        /// <summary>
        /// проверяем, находимся ли в воротах в Infinity (Гильдии Охотников) 
        /// проверяем то состояние ворот, где написано "You cannot enter for free today",
        /// когда уже нет бесплатного прохода (действия - выбрать нижнюю строку и нажать Ок)
        /// </summary>
        /// <returns></returns>
        public bool isGateBH3()
        {
            return (isBottonGateBH() && pointsGateBH3.isColor());
        }

        /// <summary>
        /// пишем слово "Initialize" в поле ввода и нажимаем кнопку Ок
        /// </summary>
        public void WriteInitialize()
        {

            pointInputBox.DoubleClickL();    // Нажимаем на поле ввода данных
            Pause(1500);

            SendKeys.SendWait("Initialize");
            Pause(1000);

            pointInputBoxBottonOk.PressMouseL();    // Нажимаем на кнопку Ок
            Pause(1500);

        }

        /// <summary>
        /// нажимаем на кнопку Ок в диалоге указанное количество раз
        /// </summary>
        /// <param name="number">количество нажатий</param>
        public void PressOkButton(int number)
        {
            for (int j = 1; j <= number; j++)
            {
//                ButtonOkDialog.DoubleClickL();    // Нажимаем на Ok в диалоге
                ButtonOkDialog.PressMouseL();       // Нажимаем на Ok в диалоге
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
