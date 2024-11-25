
namespace OpenGEWindows
{
    public class HeroCalipso : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroCalipso(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 12632000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 4079000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 13811000, 3);    
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 12368000, 3);
            pointBuff11 = new PointColor(6 + 77 - 5 + xx, 592 - 5 + yy, 16380000, 4);
            pointBuff12 = new PointColor(7 + 77 - 5 + xx, 592 - 5 + yy, 16380000, 4);
            pointBuff21 = new PointColor(1 + 77 - 5 + xx, 591 - 5 + yy, 16050000, 4);
            pointBuff22 = new PointColor(2 + 77 - 5 + xx, 590 - 5 + yy, 16250000, 4);
            heroName = "Calipso";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "CatsEye" /Calipso/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 16380000, 4).isColor() &&
        //            new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 16380000, 4).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф "Eagle Eye" /Calipso/ Q
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff2(int j)
        //{
        //    return  new PointColor(1 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16050000, 4).isColor() &&
        //            new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 590 - 5 + yy, 16250000, 4).isColor();
        //}

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
            if (!FindBuff2()) BuffQ();
        }

        /// <summary>
        /// скиллуем лучшим скиллом
        /// </summary>
        public override void SkillBoss()
        {
            BuffT();
            BuffE();
        }

    }
}
