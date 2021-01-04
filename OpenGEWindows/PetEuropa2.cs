namespace OpenGEWindows
{
    public class PetEuropa2 : Pet
    {
        public PetEuropa2()
        {}

        public PetEuropa2(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            #region Pet

            //this.pointisSummonPet1 = new PointColor(494 - 5 + xx, 304 - 5 + yy, 13000000, 6);   //старый вариант
            //this.pointisSummonPet2 = new PointColor(494 - 5 + xx, 305 - 5 + yy, 13000000, 6);
            this.pointisSummonPet1 = new PointColor(435 - 5 + xx, 384 - 5 + yy, 7900000, 5);
            this.pointisSummonPet2 = new PointColor(435 - 5 + xx, 385 - 5 + yy, 7900000, 5);
            this.pointisActivePet1 = new PointColor(493 - 5 + xx, 310 - 5 + yy, 10000000, 7);
            this.pointisActivePet2 = new PointColor(494 - 5 + xx, 309 - 5 + yy, 10000000, 7);
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                     
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointisOpenMenuPet1 = new PointColor(474 - 5 + xx, 211 - 5 + yy, 8030000, 4);
            this.pointisOpenMenuPet2 = new PointColor(484 - 5 + xx, 212 - 5 + yy, 8540000, 4);

            this.pointCancelSummonPet = new Point(465 - 5 + xx, 390 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(540 - 5 + xx, 380 - 5 + yy);                   // 868, 258   //Click Pet
            this.pointSummonPet2 = new Point(465 - 5 + xx, 360 - 5 + yy);                   // 748, 238   //Click кнопку "Summon"
            this.pointActivePet = new Point(465 - 5 + xx, 410 - 5 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion
        }

        // ===============================  Методы ==================================================
    }
}
