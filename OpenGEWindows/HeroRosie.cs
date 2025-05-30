﻿
namespace OpenGEWindows
{
    public class HeroRosie : Hero
    {
        /// <summary>
        /// конструктор 
        /// </summary>
        /// <param name="i">место героя в команде</param>
        public HeroRosie(int i, int xx, int yy)
        {
            this.i = i;
            this.xx = xx;
            this.yy = yy;
            colorTown1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 6710000, 3);
            colorTown2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 7829000, 3);
            colorWork1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 3762000, 3);
            colorWork2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 4684000, 3);
            pointBuff11 = new PointColor(4 + 77 - 5 + xx, 4 + 585 - 5 + yy, 4800000, 5);
            pointBuff12 = new PointColor(6 + 77 - 5 + xx, 6 + 585 - 5 + yy, 5800000, 5);
            pointBuff21 = new PointColor(4 + 77 - 5 + xx, 5 + 585 - 5 + yy, 6400000, 5);
            pointBuff22 = new PointColor(6 + 77 - 5 + xx, 7 + 585 - 5 + yy, 6700000, 5);
            heroName = "Rosie";

        }

        ///// <summary>
        ///// проверяем, есть ли на герое бафф "Link" / Rosie / Y
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff1(int j)
        //{
        //    return new PointColor(4 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 4 + 585 - 5 + yy, 4800000, 5).isColor() &&
        //             new PointColor(6 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 6 + 585 - 5 + yy, 5800000, 5).isColor();
        //}

        ///// <summary>
        ///// проверяем, есть ли на герое доп. бафф "Soul Weapon" / Rosie /  Q
        ///// </summary>
        ///// <param name="j">проверяем бафф на j-м месте</param>
        ///// <returns>true, если есть</returns>
        //public override bool isBuff2(int j)
        //{
        //    return new PointColor(4 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 5 + 585 - 5 + yy, 6400000, 5).isColor() &&
        //             new PointColor(6 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 7 + 585 - 5 + yy, 6700000, 5).isColor();
        //}

        /// <summary>
        /// бафаем героя
        /// </summary>
        public override void Buff()
        {
            if (!FindBuff1()) BuffY();
            if (!FindBuff2()) BuffQ();
        }

        ///// <summary>
        ///// скиллуем лучшим скиллом
        ///// </summary>
        //public override void SkillBoss()
        //{
        //    BuffT();
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
            BuffT();
        }
    }
}
