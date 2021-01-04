namespace OpenGEWindows
{
    /// <summary>
    /// реализация паттерна "Фабрика" (семейство классов EuropaTown)
    /// </summary>
    public class Europa2TownFactory : TownFactory
    {
        private botWindow botwindow;

        public Europa2TownFactory(botWindow botwindow)
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
                    town = new Europa2TownReboldo(botwindow);
                    break;
                //case 2:
                //    //=================== Коимбра =======================================
                //    town = new EuropaTownCoimbra(botwindow);
                //    break;
                case 3:
                    //=================== Ош ============================================
                    town = new Europa2TownAuch(botwindow);
                    break;
                //case 4:
                //    //=================== Юстиар =======================================
                //    town = new EuropaTownUstiar(botwindow);
                //    break;
                //case 5:
                //    //=================== багама =======================================
                //    town = new EuropaTownBagama(botwindow);
                //    break;
                case 8:
                    //=================== байрон =======================================
                    town = new Europa2TownViron(botwindow);
                    break;
                case 9:
                    //=================== Челси =======================================
                    town = new Europa2TownKielce(botwindow);
                    break;
                case 10:
                    //=================== байрон =======================================
                    town = new Europa2TownArmonia(botwindow);
                    break;
                //case 10:
                //    //=================== Кастилия=======================================
                //    town = new EuropaTownCastilia(botwindow);
                //    break;
                case 100:
                    //=================== катовия (снежка) ======================================
                    town = new Europa2TownKatovia(botwindow);
                    break;
                default:
                    //=================== такого быть не должно, но пусть будет Ребольдо =======================================
                    town = new Europa2TownReboldo(botwindow);
                    break;
            }
            return town;
        }
    }

}
