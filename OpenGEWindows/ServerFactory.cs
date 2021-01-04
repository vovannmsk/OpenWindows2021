using GEBot.Data;

namespace OpenGEWindows
{
    /// <summary>
    /// класс для реализация паттерна "Фабрика" (семейство классов server: serverAmerica,serverEuropa,serverSing)
    /// </summary>
    public class ServerFactory

    {
        private Server server;
        private botWindow botwindow;
        private string param;
        //private GlobalParam globalParam;
        private int numberOfWindow;

        /// <summary>
        /// конструктор
        /// </summary>
        //public ServerFactory(int numberOfWindow)
        //{
        //    botwindow = new botWindow(numberOfWindow);
        //    this.numberOfWindow = numberOfWindow;
        //    BotParam botParam = new BotParam(numberOfWindow);
        //    param = botParam.Parametrs[botParam.NumberOfInfinity];
        //    botwindow = new botWindow(numberOfWindow);
        //}
        public ServerFactory(botWindow botwindow) 
        {
            this.botwindow = botwindow;
            this.numberOfWindow = botwindow.getNumberWindow();
            BotParam botParam = new BotParam(numberOfWindow);
            param = botParam.Parametrs[botParam.NumberOfInfinity];

        }

        


        public Server create()
        { 
            switch (param)    
            {
                case "C:\\America\\":
                    server = new ServerAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    server = new ServerEuropa(botwindow);
                    break;
                case "C:\\Europa2\\":
                    server = new ServerEuropa2(botwindow);
                    break;
                case "C:\\SINGA\\":
                    server = new ServerSing(botwindow);
                    break;
                case "C:\\America2\\":
                    server = new ServerAmerica2(botwindow);
                    break;
                default:
                    server = new ServerSing(botwindow);
                    break;
            }
            return server;
        }

    }
}
