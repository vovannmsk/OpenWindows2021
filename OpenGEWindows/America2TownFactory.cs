namespace OpenGEWindows
{
    /// <summary>
    /// реализация паттерна "Фабрика" (семейство классов SingTown)
    /// </summary>
    public class America2TownFactory : TownFactory
    {
        private botWindow botwindow;

        /// <summary>
        /// создаёт экземпляр класса для America2Town
        /// </summary>
        /// <param name="nomerOfTown"> номер города </param>
        /// <param name="nomerOfWindow"> номер окна по порядку </param>
        /// <returns></returns>

        public America2TownFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
        }

        public override Town createTown()
        {
            Town town = null;
            switch (botwindow.getNomerTeleport())
            {
                case 1:
                    //=================== ребольдо =======================================
                    town = new America2TownReboldo(botwindow);
                    break;
                //case 2:
                //    //=================== Коимбра =======================================
                //    town = new America2TownCoimbra(nomerOfWindow);
                //    break;
                case 3:
                    //=================== Ош ============================================
                    town = new America2TownAuch(botwindow);
                    break;
                case 4:
                    //=================== Юстиар =======================================
                    town = new America2TownUstiar(botwindow);
                    break;
                case 5:
                    //=================== багама =======================================
                    town = new America2TownBagama(botwindow);
                    break;
                case 6:
                    //=================== Эррак =======================================
                    town = new America2TownErrac(botwindow);
                    break;
                case 8:
                    //=================== байрон =======================================
                    town = new America2TownViron(botwindow);
                    break;
                case 9:
                    //=================== Челси =======================================
                    town = new America2TownKielce(botwindow);
                    break;
                case 10:
                    //=================== Армония =======================================
                    town = new America2TownArmonia(botwindow);
                    break;
                case 13:
                    //=================== Кастилия=======================================
                    town = new America2TownCastilia(botwindow);
                    break;
                case 14:
                    //=================== лос толдос ======================================
                    town = new America2TownLosToldos(botwindow);
                    break;
                case 100:
                    //=================== катовия (снежка) ======================================
                    town = new America2TownKatovia(botwindow);
                    break;
                default:
                    //=================== такого быть не должно, но пусть будет Ребольдо =======================================
                    town = new America2TownReboldo(botwindow);
                    break;
            }
            return town;
        }
    }
}
