namespace OpenGEWindows
{
    public class BulletDialogFactory
    {
        private BulletDialog dialog;
        private botWindow botwindow;

        public BulletDialogFactory()
        { }

        public BulletDialogFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
        }

        /// <summary>
        /// возвращает ноывй экземпляр класса указанного сервера
        /// </summary>
        /// <returns></returns>
        public BulletDialog create()
        {
            //dialog = new BulletDialogSing(botwindow);
            switch (botwindow.getParam())
            {
                case "C:\\America\\":
                    dialog = new BulletDialogAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    dialog = new BulletDialogEuropa(botwindow);
                    break;
                case "C:\\SINGA\\":
                    dialog = new BulletDialogSing(botwindow);
                    break;
                case "C:\\America2\\":
                    dialog = new BulletDialogAmerica2(botwindow);
                    break;
                default:
                    dialog = new BulletDialogSing(botwindow);
                    break;
            }
            return dialog;
        }
    }
}
