using System.Drawing;

namespace OpenGEWindows
{
    public abstract class Pet : Server2
    {

        #region Pet

        protected iPointColor pointisSummonPet1;
        protected iPointColor pointisSummonPet2;
        protected iPointColor pointisActivePet1;
        protected iPointColor pointisActivePet2;
        protected iPointColor pointisActivePet3;  //3 и 4 сделаны для европы для проверки корма на месяц
        protected iPointColor pointisActivePet4;
        protected iPointColor pointisOpenMenuPet1;
        protected iPointColor pointisOpenMenuPet2;
        protected iPoint pointCancelSummonPet;
        protected iPoint pointSummonPet1;
        protected iPoint pointSummonPet2;
        protected iPoint pointActivePet;

        #endregion

        // ============  методы  ========================

        #region Pet

        /// <summary>
        /// нет еды у пета
        /// </summary>
        /// <returns> true, если нету </returns>
        public bool isNoFoodPet()
        {
            return  new PointColor(494 - 5 + xx, 301 - 5 + yy, 13000000, 6).isColor() &&
                    new PointColor(494 - 5 + xx, 310 - 5 + yy, 13000000, 6).isColor() &&
                    new PointColor(499 - 5 + xx, 310 - 5 + yy, 13000000, 6).isColor();
        }

        /// <summary>
        /// выбираем первого пета и нажимаем кнопку Summon в меню пет
        /// </summary>
        public void buttonSummonPet()
        {
            pointSummonPet1.PressMouseL();      //Click Pet
            pointSummonPet1.PressMouseL();
            Pause(500);
            pointSummonPet2.PressMouseL();      //Click кнопку "Summon"
            pointSummonPet2.PressMouseL();
            new Point(500 - 5 + xx, 500 - 5 + yy).Move();
            Pause(1000);
        }

        /// <summary>
        /// нажимаем кнопку Cancel Summon в меню пет
        /// </summary>
        public void buttonCancelSummonPet()
        {
            pointCancelSummonPet.PressMouseL();   //Click Cancel Summon
            pointCancelSummonPet.PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// метод проверяет, открылось ли меню с петом Alt + P
        /// </summary>
        /// <returns> true, если открыто </returns>
        public bool isOpenMenuPet()
        {
            return pointisOpenMenuPet1.isColor() && pointisOpenMenuPet2.isColor();
        }

        /// <summary>
        /// активирован ли пет?
        /// </summary>
        /// <returns>true, если активирован</returns>
        public bool isActivePet()
        {
           // return ((pointisActivePet1.isColor() && pointisActivePet2.isColor()) || (pointisActivePet3.isColor() && pointisActivePet4.isColor())); //две точки от обычной еды и две точки от периодической еды на месяц
            return pointisActivePet1.isColor() && pointisActivePet2.isColor(); //две точки от обычной еды 
        }

        /// <summary>
        /// активируем уже призванного пета
        /// </summary>
        public void ActivatePet()
        {
            pointActivePet.PressMouse(); //Click Button Active Pet
            Pause(2500);
        }

        /// <summary>
        /// проверяет, призван ли пет
        /// </summary>
        /// <returns> true, если призван </returns>
        public bool isSummonPet()
        {
            return pointisSummonPet1.isColor() && pointisSummonPet2.isColor();
        }

        /// <summary>
        /// активируем пета через пиктограмму (быстрый способ). Не используется пока
        /// считается, что изначально пет не активирован
        /// </summary>
        public void ActivatePetDem()
        {
            uint colorBegin  = new PointColor(385 - 5 + xx, 88 - 5 + yy, 0, 0).GetPixelColor();   //сохраняем изначальный цвет пета на пиктограмме
            uint colorCurrent = colorBegin;
            if ((colorCurrent != 11381225) && (colorCurrent != 0))          //если текущий цвет не розовый 
                                                                            // добавить цвет оленя вместо нуля
            {                                                                 
                while (colorBegin == colorCurrent)  //сверяем текущий цвет пета и изначальный
                                                //если текущий цвет и изначальный различны, то значит мы активировали пета
                {
                    new Point(385 - 5 + xx, 88 - 5 + yy).PressMouseL();  //тыкаем в пиктограмму пете вверху слева экрана
                    Pause(500);
                    new Point(600 - 5 + xx, 385 - 5 + yy).Move();   //убираем курсор в сторонку
                    Pause(500);
                    colorCurrent = new PointColor(385 - 5 + xx, 88 - 5 + yy, 0, 0).GetPixelColor();
                }
            }
        }



        #endregion

    }
}
