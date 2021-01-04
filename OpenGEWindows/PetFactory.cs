using GEBot.Data;

namespace OpenGEWindows
{
    public class PetFactory
    {
        private Pet pet;
        private botWindow botwindow;
        private string param;
        private int numberOfWindow;

        public PetFactory()
        { }

        public PetFactory(botWindow botwindow)
        {
            this.botwindow = botwindow;
            this.numberOfWindow = botwindow.getNumberWindow();
            BotParam botParam = new BotParam(numberOfWindow);
            param = botParam.Parametrs[botParam.NumberOfInfinity];
        }

        public Pet createPet()
        {
            //switch (botwindow.getParam())
            switch (param)
            {
                case "C:\\America\\":
                    pet = new PetAmerica(botwindow);
                    break;
                case "C:\\Europa\\":
                    pet = new PetEuropa(botwindow);
                    break;
                case "C:\\Europa2\\":
                    pet = new PetEuropa2(botwindow);
                    break;
                case "C:\\SINGA\\":
                    pet = new PetSing(botwindow);
                    break;
                case "C:\\America2\\":
                    pet = new PetAmerica2(botwindow);
                    break;
                default:
                    pet = new PetSing(botwindow);
                    break;
            }
            return pet;
        }
    }
}
