namespace OpenGEWindows
{
    public class PetAmerica : Pet
    {
        public PetAmerica()
        { }

        public PetAmerica(botWindow botwindow) 
        {

            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            #region Pet

            //this.pointisOpenMenuPet1 = new PointColor(466 + xx, 214 + yy, 12500000, 5);      //471 - 5, 219 - 5, 12500000, 472 - 5, 219 - 5, 12500000, 5);
            //this.pointisOpenMenuPet2 = new PointColor(467 + xx, 214 + yy, 12500000, 5);
            //this.pointisSummonPet1 = new PointColor(401 - 5 + xx, 362 - 5 + yy, 7630000, 4);      //401 - 5, 362 - 5, 7630000, 401 - 5, 364 - 5, 7560000, 4);
            //this.pointisSummonPet2 = new PointColor(401 - 5 + xx, 364 - 5 + yy, 7560000, 4);
            //this.pointisActivePet1 = new PointColor(495 - 5 + xx, 310 - 5 + yy, 13200000, 5);      //495 - 5, 310 - 5, 13200000, 496 - 5, 308 - 5, 13600000, 5);
            //this.pointisActivePet2 = new PointColor(496 - 5 + xx, 308 - 5 + yy, 13600000, 5);
            //this.pointisActivePet3 = new PointColor(828 - 5 + xx, 186 - 5 + yy, 13000000, 5);     //для америки пока не нужно
            //this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 13100000, 5);     //для америки пока не нужно. еда на месяц
            //this.pointCancelSummonPet = new Point(408 + xx, 390 + yy);  //400, 190 
            //this.pointSummonPet1 = new Point(569 + xx, 375 + yy);                   // 569, 375   //Click Pet
            //this.pointSummonPet2 = new Point(408 + xx, 360 + yy);                   // 408, 360   //Click кнопку "Summon"
            //this.pointActivePet = new Point(408 + xx, 405 + yy);                   // 408, 405);  //Click Button Active Pet

           
            this.pointisSummonPet1 = new PointColor(511 - 5 + xx, 282 - 5 + yy, 13000000, 6);
            this.pointisSummonPet2 = new PointColor(511 - 5 + xx, 283 - 5 + yy, 13000000, 6);
            this.pointisActivePet1 = new PointColor(496 - 5 + xx, 307 - 5 + yy, 13000000, 6);
            this.pointisActivePet2 = new PointColor(496 - 5 + xx, 308 - 5 + yy, 13000000, 6);
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                      //не проверено
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointisOpenMenuPet1 = new PointColor(474 - 5 + xx, 211 - 5 + yy, 8000000, 6);  
            this.pointisOpenMenuPet2 = new PointColor(475 - 5 + xx, 211 - 5 + yy, 8000000, 6);
            this.pointCancelSummonPet = new Point(410 - 5 + xx, 390 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(540 - 5 + xx, 380 - 5 + yy);                   // 868, 258   //Click Pet
            this.pointSummonPet2 = new Point(467 - 5 + xx, 360 - 5 + yy);                   // 748, 238   //Click кнопку "Summon"
            this.pointActivePet = new Point(410 - 5 + xx, 410 - 5 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion

        }

        // ======================  методы  ========================




    }
}
