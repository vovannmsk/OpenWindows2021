
namespace OpenGEWindows
{
    public class HeroFactory
    {
        private readonly int xx;
        private readonly int yy;

        public HeroFactory(int xx, int yy)
        {
            this.xx = xx;
            this.yy = yy;
        }

        /// <summary>
        /// вычисляем, какой герой стоит в команде на i-м месте
        /// </summary>
        /// <param name="i">номер места в команде</param>
        /// <returns></returns>
        private int WhatsHero(int i)
        {
            if (new PointColor(33 - 5 + xx + (i - 1) * 255, 696 - 5 + yy, 806655, 0).isColor())    //узнали, что это флинтлок
            {
                //проверяем Y, чтобы понять мушкетер или Бернелли
                if (new PointColor(187 - 5 + xx + (i - 1) * 255, 704 - 5 + yy, 13951211, 0).isColor())
                    return 1;     //мушкетер с флинтом
                //if (new PointColor(187 - 5 + xx + (i - 1) * 255, 705 - 5 + yy, 11251395, 0).isColor())
                //    return 10;   //Бернелли с флинтом
            }
            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 695 - 5 + yy, 16777078, 0).isColor()) return 2;     //Берка(супериор бластер)
            if (new PointColor(23 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 5041407, 0).isColor()) return 3;     //М.Лорч
            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 9371642, 0).isColor()) return 4;     //Джайна
            //if (new PointColor(28 - 5 + xx + (i - 1) * 255, 707 - 5 + yy, 5046271, 0).isColor()) return 5;     //Баррель
            if (new PointColor(22 - 5 + xx + (i - 1) * 255, 704 - 5 + yy, 16121838, 0).isColor()) return 6;    //Сесиль  --------++++++++++++
            //if (new PointColor(28 - 5 + xx + (i - 1) * 255, 698 - 5 + yy, 5636130, 0).isColor()) return 7;    //Tom
            //if (new PointColor(31 - 5 + xx + (i - 1) * 255, 701 - 5 + yy, 5081, 0).isColor()) return 8;       //Moon
            //if (new PointColor(30 - 5 + xx + (i - 1) * 255, 706 - 5 + yy, 6116670, 0).isColor()) return 9;    //Misa
            if (new PointColor(26 - 5 + xx + (i - 1) * 255, 699 - 5 + yy, 14438144, 0).isColor()) return 11;    //Rosie
            if (new PointColor(28 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 4944448, 0).isColor()) return 12;     //Marie
            //if (new PointColor(28 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 0, 0).isColor()) return 13;           //C.Daria   -------------------------
            if (new PointColor(26 - 5 + xx + (i - 1) * 255, 705 - 5 + yy, 8716287, 0).isColor()) return 14;     // Aither   ---------++++++++++++++++
            if (new PointColor(22 - 5 + xx + (i - 1) * 255, 712 - 5 + yy, 65535, 0).isColor()) return 15;       //М.Калипсо   --------+++++++++++++++
            if (new PointColor(31 - 5 + xx + (i - 1) * 255, 703 - 5 + yy, 15856385, 0).isColor()) return 16;    //Банши   --------++++++++++++++
            if (new PointColor(34 - 5 + xx + (i - 1) * 255, 703 - 5 + yy, 16251007, 0).isColor()) return 17;    //СуперРомина   ------+++++++++++++++
            if (new PointColor(39 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 8323241, 0).isColor()) return 18;     //Miho   -------+++++++++++++++
            if (new PointColor(27 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 69370, 0).isColor()) return 19;       //R.JD   ----------++++++++++++++++
            if (new PointColor(23 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 11453688, 0).isColor()) return 20;    //Jane  --------++++++++++++++
            if (new PointColor(31 - 5 + xx + (i - 1) * 255, 698 - 5 + yy, 8711323, 0).isColor()) return 21;     //Лорч  -------++++++++++++++++++
            if (new PointColor(32 - 5 + xx + (i - 1) * 255, 702 - 5 + yy, 5925855, 0).isColor()) return 22;     //Rebecca ------+++++++++++
            if (new PointColor(32 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 4929704, 0).isColor()) return 23;     //DivineHammerBryan --------++++++++++++++
            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 2703856, 0).isColor()) return 24;     //LoraConstans -------------++++++++++++

            return 0;
        }

        /// <summary>
        /// создание нового класса Hero (герой стоит в команде на i-м месте)
        /// </summary>
        /// <param name="i">место героя в команде</param>
        /// <returns></returns>
        public Hero Create(int i)
        {
            Hero hero;
            int param = WhatsHero(i);
            switch (param)
            {
                case 1:
                    hero = new HeroMusk(i, this.xx, this.yy);
                    break;
                case 2:
                    hero = new HeroBernelli(i, this.xx, this.yy);
                    break;
                case 3:
                    hero = new HeroMLorch(i, this.xx, this.yy);
                    break;
                case 4:
                    hero = new HeroJaina(i, this.xx, this.yy);
                    break;
                //case 5:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                case 6:
                    hero = new HeroCecille(i, this.xx, this.yy);
                    break;
                //case 7:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                //case 8:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                //case 9:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                //case 10:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                case 11:
                    hero = new HeroRosie(i, this.xx, this.yy);
                    break;
                case 12:
                    hero = new HeroMarie(i, this.xx, this.yy);
                    break;
                //case 13:
                //    hero = new HeroBernelli(i, this.xx, this.yy);
                //    break;
                case 14:
                    hero = new HeroAither(i, this.xx, this.yy);
                    break;
                case 15:
                    hero = new HeroMCalipso(i, this.xx, this.yy);
                    break;
                case 16:
                    hero = new HeroVanshee(i, this.xx, this.yy);
                    break;
                case 17:
                    hero = new HeroSuperRomina(i, this.xx, this.yy);
                    break;
                case 18:
                    hero = new HeroMiho(i, this.xx, this.yy);
                    break;
                case 19:
                    hero = new HeroRJD(i, this.xx, this.yy);
                    break;
                case 20:
                    hero = new HeroJane(i, this.xx, this.yy);
                    break;
                case 21:
                    hero = new HeroLorch(i, this.xx, this.yy);
                    break;
                case 22:
                    hero = new HeroRebecca(i, this.xx, this.yy);
                    break;
                case 23:
                    hero = new HeroDivineHammerBryan(i, this.xx, this.yy);
                    break;
                case 24:
                    hero = new HeroLoraConstans(i, this.xx, this.yy);
                    break;
                case 25:
                    hero = new HeroBernelli(i, this.xx, this.yy);
                    break;
                default:
                    hero = new HeroMusk(i, this.xx, this.yy);
                    break;
            }
            return hero;
        }


    }
}
