
namespace OpenGEWindows
{
    public class HeroCecille : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroCecille(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 7566000, 3);    //
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 7237000, 3);    //
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 1052000, 3);    //
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 2829000, 3);    //
            pointBuff11 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff12 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "Cecille";

        }

        ///// <summary>
        ///// проверяем, есть ли на герое 
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return true;
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
            //if (!FindBuff1()) BuffY();
        }

        ///// <summary>
        ///// скиллуем лучшим скиллом
        ///// </summary>
        //public override void SkillBoss()
        //{
        //    BuffR();
        //}

        /// <summary>
        /// скиллуем лучшим скиллом
        /// </summary>
        public override void SkillBoss1()
        {
            BuffR();
        }

        /// <summary>
        /// скиллуем вторым скиллом 
        /// </summary>
        public override void SkillBoss2()
        {
            BuffE();
        }
    }
}
