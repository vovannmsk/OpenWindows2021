
namespace OpenGEWindows
{
    public class HeroMCalipso : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroMCalipso(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 3815000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 9803000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 3);    //+++
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 3);    //+++
            pointBuff11 = new PointColor(7 + 77 - 5 + xx, 591 - 5 + yy, 16252927, 0);
            pointBuff12 = new PointColor(7 + 77 - 5 + xx, 592 - 5 + yy, 15073279, 0);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "M.Calipso";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Redacendo" /M.Calipso/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16252927, 0).isColor() &&
        //            new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 15073279, 0).isColor();
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
        }

    }
}
