using GEBot.Data;

namespace OpenGEWindows
{
    /// <summary>
    /// реализация паттерна "Фабрика" (семейство классов AmericaTown)
    /// </summary>
    public class AmericaTownFactory : TownFactory
    {
  
        private botWindow botwindow;
        private int param;
        private int numberOfWindow;

        public AmericaTownFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.numberOfWindow = botwindow.getNumberWindow();
            BotParam botParam = new BotParam(numberOfWindow);
            param = botParam.NomerTeleport;
        }

        /// <summary>
        /// создаёт экземпляр класса для AmericaTown
        /// </summary>
        /// <returns> город со всеми методами, с учетом особенностей данного города и сервера </returns>
        public override Town createTown()
        {
            Town town = null;
            //switch (botwindow.getNomerTeleport())
            switch (param)
            {
                case 1:
                    //=================== ребольдо =======================================
                    town = new AmericaTownReboldo(botwindow);
                    break;
                case 2:
                    //=================== Коимбра =======================================
                    town = new AmericaTownCoimbra(botwindow);
                    break;
                case 3:
                    //=================== Ош ============================================
                    town = new AmericaTownAuch(botwindow);
                    break;
                case 4:
                    //=================== Юстиар =======================================
                    town = new AmericaTownUstiar(botwindow);
                    break;
                case 5:
                    //=================== багама =======================================
                    town = new AmericaTownBagama(botwindow);
                    break;
                case 10:
                    //=================== Армония=======================================
                    town = new AmericaTownArmonia(botwindow);
                    break;
                case 13:
                    //=================== Кастилия=======================================
                    town = new AmericaTownCastilia(botwindow);
                    break;
                case 100:
                    //=================== катовия (снежка) ======================================
                    town = new AmericaTownKatovia(botwindow);
                    break;
                default:
                    //=================== такого быть не должно, но пусть будет Ребольдо =======================================
                    town = new AmericaTownReboldo(botwindow);
                    break;
            }
            return town;
        }
    }
}
