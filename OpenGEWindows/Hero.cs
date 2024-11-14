
namespace OpenGEWindows
{
    public abstract class Hero
    {
        /// <summary>
        /// номер героя в команде
        /// </summary>
        protected int i;
        /// <summary>
        /// смещение данного окна с игрой от края экрана по оси X
        /// </summary>
        protected int xx;
        /// <summary>
        /// смещение данного окна с игрой от края экрана по оси Y
        /// </summary>
        protected int yy;
        /// <summary>
        /// точка 1 для проверки нахождения в городе
        /// </summary>
        protected PointColor colorTown1;
        /// <summary>
        /// точка 2 для проверки нахождения в городе
        /// </summary>
        protected PointColor colorTown2;
        /// <summary>
        /// точка 1 для проверки нахождения в городе
        /// </summary>
        protected PointColor colorWork1;
        /// <summary>
        /// точка 2 для проверки нахождения в городе
        /// </summary>
        protected PointColor colorWork2;

        //======= методы =========

        public abstract void Buff();
        public abstract void SkillBoss();
        public abstract bool isBuff1(int j);
        public abstract bool isBuff2(int j);

        /// <summary>
        /// скиллуем боссов и мобов
        /// </summary>
        public void Skill(bool isBoss)
        {
            if (isBoss)
                SkillBoss();
            else
                BuffE();
        }
        /// <summary>
        /// функция проверяет, убит ли i-й герой из пати (проверка проходит на карте)
        /// </summary>
        /// <returns></returns>
        protected bool isKillHero()
        {
            bool result = false;
            result = new PointColor(81 - 5 + xx + (i - 1) * 255, 642 - 5 + yy, 2800000, 5).isColor();
            return result;
        }

        /// <summary>
        /// бафаемся умением Q
        /// </summary>
        protected void BuffQ()
        {
            new Point(34 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();   //проверено 23-11
        }

        /// <summary>
        /// бафаемся умением W
        /// </summary>
        protected void BuffW()
        {
            new Point(65 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// бафаемся умением E
        /// </summary>
        protected void BuffE()
        {
            new Point(96 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).DoubleClickL();
        }

        /// <summary>
        /// бафаемся умением R
        /// </summary>
        protected void BuffR()
        {
            new Point(127 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// бафаемся умением T
        /// </summary>
        protected void BuffT()
        {
            new Point(158 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).DoubleClickL();
        }

        /// <summary>
        /// бафаемся рабочим умением Y
        /// </summary>
        protected void BuffY()
        {
            new Point(189 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();              //проверено 23-11
            MoveCursorOfMouse();
        }

        /// <summary>
        /// перемещаем курсор мыши прочь от всего
        /// </summary>
        private void MoveCursorOfMouse()
        {
            new Point(670 - 5 + xx, 640 - 5 + yy).Move();
        }

        /// <summary>
        /// проверяем, есть ли на герое бафф 1 (Y)
        /// </summary>
        /// <returns>true, если есть</returns>
        protected bool FindBuff1()
        {
            bool result = false;    //бафа нет
            for (int j = 3; j < 11; j++)
                if ( isBuff1(j) )
                {
                    result = true;
                    break;
                }
            if (isKillHero()) result = true;   //если герой убит, то считаем, что у него есть бафф

            return result;
        }

        /// <summary>
        /// проверяем, есть ли на герое бафф 2 (дополнительный)
        /// </summary>
        /// <returns>true, если есть</returns>
        protected bool FindBuff2()
        {
            bool result = false;    //бафа нет
            for (int j = 3; j < 11; j++)
                if (isBuff2(j))
                {
                    result = true;
                    break;
                }
            if (isKillHero()) result = true;   //если герой убит, то считаем, что у него есть бафф 

            return result;
        }


        /// <summary>
        /// проверяем, находимся ли в городе
        /// </summary>
        /// <returns></returns>
        public bool isTown()
        {
            return this.colorTown1.isColor() && this.colorTown2.isColor(); 
        }

        /// <summary>
        /// проверяем, находимся ли на работе (на поле сражения)
        /// </summary>
        /// <returns></returns>
        public bool isWork()
        {
            return this.colorWork1.isColor() && this.colorWork2.isColor();
        }
    }
}
