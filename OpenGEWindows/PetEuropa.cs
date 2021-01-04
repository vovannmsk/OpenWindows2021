namespace OpenGEWindows
{
    public class PetEuropa : Pet
    {
        public PetEuropa()
        { }

        public PetEuropa(botWindow botwindow)
        {

            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            #region Pet

            this.pointisOpenMenuPet1 = new PointColor(500 - 5 + xx, 211 - 5 + yy, 8000000, 6);               //проверено
            this.pointisOpenMenuPet2 = new PointColor(505 - 5 + xx, 211 - 5 + yy, 8000000, 6);               //проверено

            //this.pointisSummonPet1 = new PointColor(408 - 5 + xx, 361 - 5 + yy, 7000000, 5);                 //старый вариант
            //this.pointisSummonPet2 = new PointColor(409 - 5 + xx, 361 - 5 + yy, 7000000, 5);                 //проверено
            
            this.pointisSummonPet1 = new PointColor(433 - 5 + xx, 385 - 5 + yy, 7300000, 5);                
            this.pointisSummonPet2 = new PointColor(433 - 5 + xx, 386 - 5 + yy, 7300000, 5);                

            this.pointisActivePet1 = new PointColor(517 - 5 + xx, 302 - 5 + yy, 10000000, 7);                //проверено
            this.pointisActivePet2 = new PointColor(495 - 5 + xx, 310 - 5 + yy, 10000000, 7);                //проверено
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                      //проверено
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            //this.pointisActivePet5 = new PointColor(517 - 5 + xx, 302 - 5 + yy, 12500000, 5);                //проверено
            //this.pointisActivePet6 = new PointColor(495 - 5 + xx, 310 - 5 + yy, 11700000, 5);                //проверено
            this.pointCancelSummonPet = new Point(465 - 5 + xx, 387 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(542 - 5 + xx, 381 - 5 + yy);                   // 868, 258   //Click Pet                                       //проверено
            this.pointSummonPet2 = new Point(465 - 5 + xx, 363 - 5 + yy);                   // 748, 238   //Click кнопку "Summon"                           //проверено
            this.pointActivePet = new Point(465 - 5 + xx, 412 - 5 + yy);                   // //Click Button "Activation"                                    //проверено

            #endregion

        }

        // ===============================  Методы ==================================================



    }
}
