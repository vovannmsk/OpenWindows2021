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
            return (pointisOpenMenuPet1.isColor() && pointisOpenMenuPet2.isColor());
        }

        /// <summary>
        /// проверяет, активирован ли пет (зависит от сервера)
        /// </summary>
        /// <returns></returns>
        public bool isActivePet()
        {
           // return ((pointisActivePet1.isColor() && pointisActivePet2.isColor()) || (pointisActivePet3.isColor() && pointisActivePet4.isColor())); //две точки от обычной еды и две точки от периодической еды на месяц
            return ((pointisActivePet1.isColor() && pointisActivePet2.isColor())); //две точки от обычной еды 
        }

        /// <summary>
        /// активируем уже призванного пета
        /// </summary>
        public void ActivePet()
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

        #endregion

    }
}
