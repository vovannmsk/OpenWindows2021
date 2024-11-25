
namespace OpenGEWindows
{
    public class HeroRJD : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroRJD(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 0, 3);    //доделать
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 0, 3);    //доделать
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 3);    //доделать
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 3);    //доделать
            pointBuff11 = new PointColor(6 + 77 - 5 + xx, 589 - 5 + yy, 1574057, 0);
            pointBuff12 = new PointColor(7 + 77 - 5 + xx, 589 - 5 + yy, 5505222, 0);
            pointBuff21 = new PointColor(2 + 77 - 5 + xx, 591 - 5 + yy, 16663289, 0);
            pointBuff22 = new PointColor(3 + 77 - 5 + xx, 591 - 5 + yy, 16666360, 0);
            heroName = "R.JD";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Weapon Master" /R.JD/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 1574057, 0).isColor() &&
        //            new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 5505222, 0).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф "Resolution" /R.JD/
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff2(int j)
        //{
        //    return  new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16663289, 0).isColor() &&
        //            new PointColor(3 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16666360, 0).isColor();
        //}

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
            if (!FindBuff2()) BuffR();
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
