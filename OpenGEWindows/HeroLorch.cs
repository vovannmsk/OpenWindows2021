
namespace OpenGEWindows
{
    public class HeroLorch : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroLorch(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 8947000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 7434000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 2252000, 3);    
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 6277000, 3);
            pointBuff11 = new PointColor(2 + 77 - 5 + xx, 587 - 5 + yy, 1400619, 0);
            pointBuff12 = new PointColor(3 + 77 - 5 + xx, 587 - 5 + yy, 1400619, 0);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "Lorch";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Infiltration" /Lorch/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 587 - 5 + yy, 1400619, 0).isColor() &&
        //            new PointColor(3 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 587 - 5 + yy, 1400619, 0).isColor();
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
