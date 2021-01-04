namespace OpenGEWindows
{
    /// <summary>
    /// реализация паттерна "Фабрика" (семейство классов EuropaTown)
    /// </summary>
    public class EuropaTownFactory : TownFactory
    {
        private botWindow botwindow;

        public EuropaTownFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
        }

        /// <summary>
        /// создаёт экземпляр класса для EuropaTown
        /// </summary>
        /// <param name="nomerOfTown"> номер города </param>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        /// <returns></returns>
        public override Town createTown()
        {
            Town town = null;
            switch (botwindow.getNomerTeleport())
            {
                case 1:
                    //=================== ребольдо =======================================
                    town = new EuropaTownReboldo(botwindow);
                    break;
                case 2:
                    //=================== Коимбра =======================================
                    town = new EuropaTownCoimbra(botwindow);
                    break;
                case 3:
                    //=================== Ош ============================================
                    town = new EuropaTownAuch(botwindow);
                    break;
                case 4:
                    //=================== Юстиар =======================================
                    town = new EuropaTownUstiar(botwindow);
                    break;
                case 5:
                    //=================== багама =======================================
                    town = new EuropaTownBagama(botwindow);
                    break;
                case 10:
                    //=================== Кастилия=======================================
                    town = new EuropaTownCastilia(botwindow);
                    break;
                case 100:
                    //=================== катовия (снежка) ======================================
                    town = new EuropaTownKatovia(botwindow);
                    break;
                default:
                    //=================== такого быть не должно, но пусть будет Ребольдо =======================================
                    town = new EuropaTownReboldo(botwindow);
                    break;
            }
            return town;
        }
    }

}
