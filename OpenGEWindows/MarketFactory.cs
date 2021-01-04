using GEBot.Data;

namespace OpenGEWindows
{
    public class MarketFactory
    {
        private Market market;
        private botWindow botwindow;
        private string param;
        private int numberOfWindow;

        public MarketFactory()
        { }

        public MarketFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.numberOfWindow = botwindow.getNumberWindow();
            BotParam botParam = new BotParam(numberOfWindow);
            param = botParam.Parametrs[botParam.NumberOfInfinity];
        }

        public Market createMarket()
        {
            //switch (botwindow.getParam())
            switch (param)
            {
                case "C:\\America\\":
                    market = new MarketAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    market = new MarketEuropa(botwindow);
                    break;
                case "C:\\Europa2\\":
                    market = new MarketEuropa2(botwindow);
                    break;
                case "C:\\SINGA\\":
                    market = new MarketSing(botwindow);
                    break;
                case "C:\\America2\\":
                    market = new MarketAmerica2(botwindow);
                    break;
                default:
                    market = new MarketSing(botwindow);
                    break;
            }
            return market;
        }
    }
}
