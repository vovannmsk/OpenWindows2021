
namespace OpenGEWindows
{
    public class HeroLoraConstans : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroLoraConstans(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 0, 3);    //доделать
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 0, 3);    //доделать
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 3);    //доделать
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 3);    //доделать
        }

        /// <summary>
        /// проверяем, есть ли на герое бафф "Justice" /LoraConstans/ Y
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff1(int j)
        {
            return  new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 13080000, 4).isColor() &&
                    new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 12880000, 4).isColor();
        }

        /// <summary>
        /// проверяем, есть ли на герое доп. бафф "Promise" /LoraConstans/
        /// </summary>
        /// <param name="j">проверяем бафф на j-м месте</param>
        /// <returns>true, если есть</returns>
        public override bool isBuff2(int j)
        {
            return  new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 1068760, 0).isColor() &&
                    new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 17110, 0).isColor();
        }

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
            if (!FindBuff2()) BuffW();
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
