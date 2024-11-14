
namespace OpenGEWindows
{
    public class HeroMusk : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroMusk(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 8092000, 3);
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 7895000, 3);
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 12565000, 3);
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 12169000, 3);
        }

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Concentration"
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff1(int j)
        {
            return  new PointColor(83 - 5 + xx + j * 14 + (i - 1) * 255, 587 - 5 + yy, 5390673, 0).isColor() &&
                    new PointColor(83 - 5 + xx + j * 14 + (i - 1) * 255, 588 - 5 + yy, 5521228, 0).isColor();
        }

        /// <summary>
        /// проверяем, есть ли на герое доп. бафф 
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff2(int j)
        {
            return true;
        }

        /// <summary>
        /// бафаем мушкетера
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
