
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
            if (new PointColor(33 - 5 + xx, 696 - 5 + yy, 806655, 0).isColor((i - 1) * 255, 0))    //узнали, что это флинтлок
            {
                //проверяем Y, чтобы понять мушкетер или Бернелли
 
                    return 1;     //мушкетер с флинтом
                //if (new PointColor(187 - 5 + xx + (i - 1) * 255, 705 - 5 + yy, 11251395, 0).isColor())
                //    return 10;   //Бернелли с флинтом
            }
            if (new PointColor(29 - 5 + xx, 695 - 5 + yy, 16777078, 0).isColor((i - 1) * 255,0)) return 2;     //Берка(супериор бластер)
            if (new PointColor(23 - 5 + xx, 697 - 5 + yy, 5041407, 0).isColor((i - 1) * 255, 0)) return 3;     //М.Лорч
            if (new PointColor(29 - 5 + xx, 697 - 5 + yy, 9371642, 0).isColor((i - 1) * 255, 0)) return 4;     //Джайна
            //if (new PointColor(28 - 5 + xx, 707 - 5 + yy, 5046271, 0).isColor((i - 1) * 255,0)) return 5;     //Баррель
            if (new PointColor(22 - 5 + xx, 704 - 5 + yy, 16121838, 0).isColor((i - 1) * 255, 0)) return 6;    //Сесиль  --------++++++++++++
            //if (new PointColor(28 - 5 + xx, 698 - 5 + yy, 5636130, 0).isColor((i - 1) * 255,0)) return 7;    //Tom
            //if (new PointColor(31 - 5 + xx, 701 - 5 + yy, 5081, 0).isColor((i - 1) * 255,0)) return 8;       //Moon
            //if (new PointColor(30 - 5 + xx, 706 - 5 + yy, 6116670, 0).isColor((i - 1) * 255,0)) return 9;    //Misa
            if (new PointColor(26 - 5 + xx, 699 - 5 + yy, 14438144, 0).isColor((i - 1) * 255, 0)) return 11;    //Rosie
            if (new PointColor(28 - 5 + xx, 700 - 5 + yy, 4944448, 0).isColor((i - 1) * 255, 0)) return 12;     //Marie
            //if (new PointColor(28 - 5 + xx, 700 - 5 + yy, 0, 0).isColor((i - 1) * 255,0)) return 13;           //C.Daria   -------------------------
            if (new PointColor(26 - 5 + xx, 705 - 5 + yy, 8716287, 0).isColor((i - 1) * 255, 0)) return 14;     // Aither   ---------++++++++++++++++
            if (new PointColor(22 - 5 + xx, 712 - 5 + yy, 65535, 0).isColor((i - 1) * 255, 0)) return 15;       //М.Калипсо   --------+++++++++++++++
            if (new PointColor(31 - 5 + xx, 703 - 5 + yy, 15856385, 0).isColor((i - 1) * 255, 0)) return 16;    //Банши   --------++++++++++++++
            if (new PointColor(34 - 5 + xx, 703 - 5 + yy, 16251007, 0).isColor((i - 1) * 255, 0)) return 17;    //СуперРомина   ------+++++++++++++++
            if (new PointColor(39 - 5 + xx, 697 - 5 + yy, 8323241, 0).isColor((i - 1) * 255, 0)) return 18;     //Miho   -------+++++++++++++++
            if (new PointColor(27 - 5 + xx, 700 - 5 + yy, 69370, 0).isColor((i - 1) * 255, 0)) return 19;       //R.JD   ----------++++++++++++++++
            if (new PointColor(23 - 5 + xx, 700 - 5 + yy, 11453688, 0).isColor((i - 1) * 255, 0)) return 20;    //Jane  --------++++++++++++++
            if (new PointColor(31 - 5 + xx, 698 - 5 + yy, 8711323, 0).isColor((i - 1) * 255, 0)) return 21;     //Лорч  -------++++++++++++++++++
            if (new PointColor(32 - 5 + xx, 702 - 5 + yy, 5925855, 0).isColor((i - 1) * 255, 0)) return 22;     //Rebecca ------+++++++++++
            if (new PointColor(32 - 5 + xx, 700 - 5 + yy, 4929704, 0).isColor((i - 1) * 255, 0)) return 23;     //DivineHammerBryan --------++++++++++++++
            if (new PointColor(29 - 5 + xx, 700 - 5 + yy, 2703856, 0).isColor((i - 1) * 255, 0)) return 24;     //LoraConstans -------------++++++++++++
            if (new PointColor(32 - 5 + xx, 703 - 5 + yy, 16777214, 0).isColor((i - 1) * 255, 0)) return 25;    //Calipso -------------++++++++++++

            return 0;
        }
        /// <summary>
        /// вычисляем, какой герой стоит в команде на i-м месте
        /// </summary>
        /// <param name="i">номер места в команде</param>
        /// <returns></returns>
        public int WhatsHeroInTown(int i)
        {
            PointColor point1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 0, 0);
            PointColor point2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 0, 0);
            uint color1 = point1.GetPixelColor((i - 1) * 255, 0) /1000;
            uint color2 = point2.GetPixelColor((i - 1) * 255, 0) /1000;

            if ((color1 == 5921) && (color2 == 1907)) return 1;   //мушкетер с флинтом ++
            if ((color1 == 16777) && (color2 == 6118)) return 2;   //Берка(супериор бластер) ++
            if ((color1 == 14869) && (color2 == 8882)) return 3;   //М.Лорч ++
            if ((color1 == 4737) && (color2 == 6710)) return 4;   //Джайна ++
            //if ((color1 == 1) && (color2 == 1)) return 5; //Баррель молодой
            if ((color1 == 1) && (color2 == 1)) return 6;   //Сесиль --
            //if ((color1 == 1) && (color2 == 1)) return 7; //Tom
            //if ((color1 == 1) && (color2 == 1)) return 8; //Moon
            //if ((color1 == 1) && (color2 == 1)) return 9; //Misa
            //if ((color1 == 1) && (color2 == 1)) return 10; //Бернелли с флинтом
            if ((color1 == 1) && (color2 == 1)) return 11;  //Rosie --
            if ((color1 == 10132) && (color2 == 7566)) return 12;  //Marie ++
            //if ((color1 == 1) && (color2 == 1)) return 13;  //C.Daria
            if ((color1 == 1) && (color2 == 1)) return 14;  // Aither --
            if ((color1 == 3815) && (color2 == 9803)) return 15;  //М.Калипсо ++
            if ((color1 == 592) && (color2 == 0)) return 16;  //Банши ++
            if ((color1 == 13092) && (color2 == 12566)) return 17;  //СуперРомина ++
            if ((color1 == 1) && (color2 == 1)) return 18;  //Miho --
            if ((color1 == 1) && (color2 == 1)) return 19;  //R.JD -- 
            if ((color1 == 1) && (color2 == 1)) return 20;  //Jane --
            if ((color1 == 8947) && (color2 == 7434)) return 21;  //Лорч ++
            if ((color1 == 4144) && (color2 == 8816)) return 22;  //Rebecca ++
            if ((color1 == 8618) && (color2 == 11711)) return 23;  //DivineHammerBryan ++
            if ((color1 == 5066) && (color2 == 6381)) return 24;  //LoraConstans ++
            if ((color1 == 12632) && (color2 == 4079)) return 25;  //Калипсо ++

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
                    hero = new HeroCalipso(i, this.xx, this.yy);
                    break;
                default:
                    hero = new HeroMusk(i, this.xx, this.yy);
                    break;
            }
            return hero;
        }


    }
}
