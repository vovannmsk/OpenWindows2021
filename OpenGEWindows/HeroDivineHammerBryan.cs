
namespace OpenGEWindows
{
    public class HeroDivineHammerBryan : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroDivineHammerBryan(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy,  8618000, 3);    
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 11711000, 3);    
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 63780000, 3);    
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 6512000, 3);
            pointBuff11 = new PointColor(1 + 77 - 5 + xx, 589 - 5 + yy, 13090000, 4);
            pointBuff12 = new PointColor(2 + 77 - 5 + xx, 590 - 5 + yy, 13290000, 4);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "DivineHammerBryan";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Metallurgy" /DivineHammerBryan/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(1 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 13090000, 4).isColor() &&
        //            new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 590 - 5 + yy, 13290000, 4).isColor();
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
