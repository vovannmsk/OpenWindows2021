
namespace OpenGEWindows
{
    public class HeroRebecca : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroRebecca(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 4144000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 8816000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 10583000, 3);
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 7758000, 3);
            pointBuff11 = new PointColor(6 + 77 - 5 + xx, 593 - 5 + yy, 15920000, 4);
            pointBuff12 = new PointColor(6 + 77 - 5 + xx, 594 - 5 + yy, 15730000, 4);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "Rebecca";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "ChangerDeMode" /Rebecca/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 593 - 5 + yy, 15920000, 4).isColor() &&
        //            new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 594 - 5 + yy, 15730000, 4).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф 
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff2(int j)
        //{
        //    return  true;
        //}

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
        }

        ///// <summary>
        ///// скиллуем лучшим скиллом
        ///// </summary>
        //public override void SkillBoss()
        //{
        //    BuffT();
        //}

        /// <summary>
        /// скиллуем лучшим скиллом
        /// </summary>
        public override void SkillBoss1()
        {
            BuffT();
        }

        /// <summary>
        /// скиллуем вторым скиллом 
        /// </summary>
        public override void SkillBoss2()
        {
            BuffT();
        }
    }
}
