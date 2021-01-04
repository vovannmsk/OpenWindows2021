namespace OpenGEWindows
{
    public class KatoviaMarketFactory
    {
        private KatoviaMarket market;
        private botWindow botwindow;

        public KatoviaMarketFactory()
        { }

        public KatoviaMarketFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
        }

        public KatoviaMarket createMarket()
        {
            switch (botwindow.getParam())
            {
                case "C:\\America\\":
                    market = new KatoviaMarketAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    market = new KatoviaMarketEuropa(botwindow);
                    break;
                case "C:\\Europa2\\":
                    market = new KatoviaMarketEuropa2(botwindow);
                    break;
                case "C:\\SINGA\\":
                    market = new KatoviaMarketSing(botwindow);
                    break;
                case "C:\\America2\\":
                    market = new KatoviaMarketAmerica2(botwindow);
                    break;
                default:
                    market = new KatoviaMarketSing(botwindow);
                    break;
            }
            return market;
        }
    }
}
