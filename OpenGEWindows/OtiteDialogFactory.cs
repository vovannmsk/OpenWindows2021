namespace OpenGEWindows
{
    public class OtiteDialogFactory
    {
        private OtiteDialog dialog;
        private botWindow botwindow;

        public OtiteDialogFactory()
        { }

        public OtiteDialogFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
        }

        /// <summary>
        /// возвращает ноывй экземпляр класса указанного сервера
        /// </summary>
        /// <returns></returns>
        public OtiteDialog create()
        {
            dialog = new OtiteDialogSing(botwindow);
            switch (botwindow.getParam())
            {
                case "C:\\America\\":
                    dialog = new OtiteDialogAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    dialog = new OtiteDialogEuropa(botwindow);
                    break;
                case "C:\\Europa2\\":
                    dialog = new OtiteDialogEuropa2(botwindow);
                    break;
                case "C:\\SINGA\\":
                    dialog = new OtiteDialogSing(botwindow);
                    break;
                case "C:\\America2\\":
                    dialog = new OtiteDialogAmerica2(botwindow);
                    break;
                default:
                    dialog = new OtiteDialogSing(botwindow);
                    break;
            }
            return dialog;
        }
    }
}
