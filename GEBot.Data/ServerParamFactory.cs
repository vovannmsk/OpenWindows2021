namespace GEBot.Data
{
    /// <summary>
    /// класс для реализация паттерна "Фабрика" (семейство классов ServerParam)
    /// </summary>
    public class ServerParamFactory
    {
        //private GlobalParam globalParam;
        private ServerParam serverParam;
        //private BotParam botParam;
        private string param;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="param">синг, америка или европа</param>
        public ServerParamFactory(int numberOfWindow)
        {
            BotParam botParam = new BotParam(numberOfWindow);
            param = botParam.Parametrs[botParam.NumberOfInfinity];
        }
        public ServerParam create()
        {
            switch (param)
            {
                case "C:\\America\\":
                    serverParam = new ServerParamAmerica();
                    break;
                case "C:\\Europa\\":
                    serverParam = new ServerParamEuropa();
                    break;
                case "C:\\Europa2\\":
                    serverParam = new ServerParamEuropa2();
                    break;
                case "C:\\SINGA\\":
                    serverParam = new ServerParamSing();
                    break;
                case "C:\\America2\\":
                    serverParam = new ServerParamAmerica2();
                    break;
                default:
                    serverParam = new ServerParamSing();
                    break;
            }
            return serverParam;
        }


    }
}
