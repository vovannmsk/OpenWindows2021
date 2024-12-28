
namespace OpenGEWindows
{
    public class HeroMarie : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroMarie(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 10132000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 7566000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 5603000, 3);    
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 5406000, 3);
            pointBuff11 = new PointColor(1 + 77 - 5 + xx, 0 + 587 - 5 + yy, 12409326, 0);
            pointBuff12 = new PointColor(1 + 77 - 5 + xx, 0 + 588 - 5 + yy, 12409326, 0);
            pointBuff21 = new PointColor(0 + 77 - 5 + xx, 0 + 585 - 5 + yy, 16758600, 0);
            pointBuff22 = new PointColor(1 + 77 - 5 + xx, 0 + 585 - 5 + yy, 16758600, 0);
            heroName = "Marie";
         }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "CleaningTime" / Marie / Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 587 - 5 + yy, 12409326, 0).isColor() &&
        //            new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 588 - 5 + yy, 12409326, 0).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф "OrderLady" / Marie / E
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff2(int j)
        //{
        //    return  new PointColor(0 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 16758600, 0).isColor() &&
        //            new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 16758600, 0).isColor();
        //}

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
            if (!FindBuff2()) BuffE();
        }

        ///// <summary>
        ///// скиллуем лучшим скиллом
        ///// </summary>
        //public override void SkillBoss()
        //{
        //    BuffT();
        //    BuffR();
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
            BuffR();
        }
    }
}
