﻿
namespace OpenGEWindows
{
    public class HeroJane : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroJane(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 7829000, 3);
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 2039000, 3);
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 11046000, 3);
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 5986000, 3);
            pointBuff11 = new PointColor(2 + 77 - 5 + xx, 3 + 588 - 5 + yy, 12100000, 5);
            pointBuff12 = new PointColor(3 + 77 - 5 + xx, 3 + 588 - 5 + yy, 12200000, 5);
            pointBuff21 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            pointBuff22 = new PointColor(-5 + xx, -5 + yy, 0, 0);    //пустышка
            heroName = "Jane";
        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Reasoning" /Jane/ Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return  new PointColor(8 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 588 - 5 + yy, 16777215, 0).isColor() &&
        //            new PointColor(8 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 16777215, 0).isColor();
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