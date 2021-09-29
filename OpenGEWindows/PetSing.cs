namespace OpenGEWindows
{
    public class PetSing : Pet
    {
        public PetSing ()
        {}

        public PetSing(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            #region Pet

            //this.pointisSummonPet1 = new PointColor(494 - 5 + xx, 304 - 5 + yy, 13000000, 6);   //старый вариант
            //this.pointisSummonPet2 = new PointColor(494 - 5 + xx, 305 - 5 + yy, 13000000, 6);
            this.pointisSummonPet1 = new PointColor(409 - 5 + xx, 368 - 5 + yy, 7700000, 5);    //Буква "m" в слове Summon (серая, если уже призван пет)
            this.pointisSummonPet2 = new PointColor(409 - 5 + xx, 369 - 5 + yy, 7700000, 5);
            this.pointisActivePet1 = new PointColor(549 - 5 + xx, 310 - 5 + yy, 13890558, 0);
            this.pointisActivePet2 = new PointColor(549 - 5 + xx, 311 - 5 + yy, 13890558, 0);
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                     
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointisOpenMenuPet1 = new PointColor(481 - 5 + xx, 216 - 5 + yy, 8030000, 4);
            this.pointisOpenMenuPet2 = new PointColor(482 - 5 + xx, 216 - 5 + yy, 8030000, 4);

            this.pointCancelSummonPet = new Point(465 - 5 + xx, 390 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(540 - 5 + xx, 385 - 5 + yy);                   // Click First Pet
            this.pointSummonPet2 = new Point(465 - 5 + xx, 367 - 5 + yy);                   // Click кнопку "Summon"
            this.pointActivePet = new Point(465 - 5 + xx, 417 - 5 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion
        }

        // ===============================  Методы ==================================================
    }
}
