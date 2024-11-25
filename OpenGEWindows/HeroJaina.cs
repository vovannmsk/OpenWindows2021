
namespace OpenGEWindows
{
    public class HeroJaina : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroJaina(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 4737000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 6710000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 9371000, 3);    
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 3562000, 3);
            pointBuff11 = new PointColor(77 - 5 + xx, 595 - 5 + yy, 16767324, 0);
            pointBuff12 = new PointColor(77 - 5 + xx, 596 - 5 + yy, 16767324, 0);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка

            heroName = "Jaina";

        }

        ///// <summary>
        ///// проверяем, есть ли на герое "Mysophoia" /Jaina/
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(77 - 5 + xx + j * 14 + (i - 1) * 255, 595 - 5 + yy, 16767324, 0).isColor() &&
        //            new PointColor(77 - 5 + xx + j * 14 + (i - 1) * 255, 596 - 5 + yy, 16767324, 0).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф (Share Flint)
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
