
namespace OpenGEWindows
{
    public class HeroBernelli : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroBernelli(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 16777000, 3);    //*
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 6118000, 3);     //*
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 16383000, 3);    //*
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 16777000, 3);    //*
        }

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Marksmanship"
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff1(int j)
        {
            return  new PointColor(89 - 5 + xx + j * 14 + (i - 1) * 255, 596 - 5 + yy, 13133369, 0).isColor() &&
                    new PointColor(89 - 5 + xx + j * 14 + (i - 1) * 255, 597 - 5 + yy, 13067318, 0).isColor();
        }

        /// <summary>
        /// проверяем, есть ли на i-м герое доп. бафф (Hound)
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff2(int j)
        {
            return  new PointColor(80 - 5 + xx + j * 14 + (i - 1) * 255, 589 - 5 + yy, 6319302, 0).isColor() &&
                    new PointColor(81 - 5 + xx + j * 14 + (i - 1) * 255, 589 - 5 + yy, 5858504, 0).isColor();
        }

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
        }

    }
}
