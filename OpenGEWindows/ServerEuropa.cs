using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GEBot.Data;


namespace OpenGEWindows
{
    public class ServerEuropa : Server 
    {
        [DllImport("user32.dll")]
        private static extern UIntPtr FindWindow(String ClassName, String WindowName);  //ищет окно с заданным именем и классом

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="botwindow"></param>
        public ServerEuropa(botWindow botwindow)
        {
            //isLoadedGEBH = false;   //?????????????? может не ставить сюда ??????????????????
            isLoadedSteamBH = false;
            this.Hero = new int[4] { 0, 0, 0, 0 };
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();
            this.globalParam = new GlobalParam();
            ServerParamFactory serverParamFactory = new ServerParamFactory(botwindow.getNumberWindow());
            this.serverParam = serverParamFactory.create();
            this.botParam = new BotParam(botwindow.getNumberWindow());

            #endregion

            #region общие 2

            TownFactory townFactory = new EuropaTownFactory(botwindow);                      // здесь выбирается конкретная реализация для фабрики Town
            this.town = townFactory.createTown();                                            // выбирается город с помощью фабрики
            this.town_begin = new EuropaTownReboldo(botwindow);
            dialog = new DialogEuropa(botwindow);

            #endregion

            #region параметры, зависящие от сервера

            //            this.pathClient = path_Client();
            this.pathClient = serverParam.PathClient;
            this.isActiveServer = serverParam.IsActiveServer;

            #endregion

            #region No window



            #endregion

            #region Logout

            this.pointConnect   = new PointColor(522 - 5 + xx, 418 - 5 + yy, 6100000, 5); 
            this.pointisLogout1 = new PointColor(362 - 5 + xx, 315 - 5 + yy, 15000000, 6);    //проверено
            this.pointisLogout2 = new PointColor(362 - 5 + xx, 317 - 5 + yy, 15000000, 6);

            #endregion

            #region Pet

            this.pointisOpenMenuPet1 = new PointColor(829 + xx, 93 + yy, 12000000, 6);   
            this.pointisOpenMenuPet2 = new PointColor(830 + xx, 93 + yy, 12000000, 6);
            this.pointisSummonPet1 = new PointColor(740 - 5 + xx, 237 - 5 + yy, 7400000, 5);
            this.pointisSummonPet2 = new PointColor(741 - 5 + xx, 237 - 5 + yy, 7400000, 5);
            this.pointisActivePet1 = new PointColor(828 - 5 + xx, 186 - 5 + yy, 13000000, 6);
            this.pointisActivePet2 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 13000000, 6);
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                      //проверено
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointCancelSummonPet = new Point(750 + xx, 265 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(868 + xx, 258 + yy);                   // 868, 258   //Click Pet
            this.pointSummonPet2 = new Point(748 + xx, 238 + yy);                   // 748, 238   //Click кнопку "Summon"
            this.pointActivePet = new Point(748 + xx, 288 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion

            #region Top Menu

            this.pointGotoEnd = new Point(681 - 5 + xx, 436 - 5 + yy);                              //логаут
//            this.pointGotoEnd = new Point(681 - 5 + xx, 467 - 5 + yy);                              //end
            this.pointLogout = new Point(681 - 5 + xx, 436 - 5 + yy);            //логаут
            this.pointTeleportFirstLine = new Point(405 - 5 + xx, 180 - 5 + yy);   //400, 193               тыкаем в первую строчку телепорта                          //проверено
            //this.pointTeleportSecondLine = new Point(400 + xx, 208 + yy);   //              тыкаем во вторую строчку телепорта                          //проверено
            this.pointTeleportExecute = new Point(360 - 5 + xx, 590 - 5 + yy);               //        тыкаем в кнопку Execute                
            this.pointisOpenTopMenu21 = new PointColor(328 + xx, 74 + yy, 13420000, 4);                //не проверено
            this.pointisOpenTopMenu22 = new PointColor(329 + xx, 74 + yy, 13420000, 4);
            this.pointisOpenTopMenu61 = new PointColor(455 + xx, 87 + yy, 13420000, 4);                //не проверено
            this.pointisOpenTopMenu62 = new PointColor(456 + xx, 87 + yy, 13420000, 4);
            this.pointisOpenTopMenu81 = new PointColor(553 + xx, 87 + yy, 13420000, 4);                //не проверено
            this.pointisOpenTopMenu82 = new PointColor(554 + xx, 87 + yy, 13420000, 4);
            this.pointisOpenTopMenu91 = new PointColor(601 + xx, 74 + yy, 13420000, 4);                //проверено
            this.pointisOpenTopMenu92 = new PointColor(602 + xx, 74 + yy, 13420000, 4);
            this.pointisOpenTopMenu121 = new PointColor(507 - 5 + xx, 111 - 5 + yy, 8030000, 4);      
            this.pointisOpenTopMenu122 = new PointColor(508 - 5 + xx, 111 - 5 + yy, 8030000, 4);
            this.pointisOpenTopMenu131 = new PointColor(539 - 5 + xx, 374 - 5 + yy, 16100000, 5);      //проверено
            this.pointisOpenTopMenu132 = new PointColor(540 - 5 + xx, 374 - 5 + yy, 16500000, 5);

            #endregion

            #region Shop

            this.pointIsSale1 = new PointColor(903 + xx, 674 + yy, 7590000, 4);
            this.pointIsSale2 = new PointColor(904 + xx, 674 + yy, 7850000, 4);
            this.pointIsSale21 = new PointColor(840 - 5 + xx, 665 - 5 + yy, 7720000, 4);
            this.pointIsSale22 = new PointColor(840 - 5 + xx, 668 - 5 + yy, 7720000, 4);
            this.pointIsClickSale1 = new PointColor(728 + xx, 660 + yy, 7720000, 4);
            this.pointIsClickSale2 = new PointColor(728 + xx, 659 + yy, 7720000, 4);
            this.pointBookmarkSell = new Point(225 + xx, 163 + yy);
            this.pointSaleToTheRedBottle = new Point(335 + xx, 220 + yy);
            this.pointSaleOverTheRedBottle = new Point(335 + xx, 220 + yy);
            this.pointWheelDown = new Point(375 + xx, 220 + yy);           //345 + 30 + botwindow.getX(), 190 + 30 + botwindow.getY(), 3);        // колесо вниз
            this.pointButtonBUY = new Point(725 + xx, 663 + yy);   //725, 663);
            this.pointButtonSell = new Point(725 + xx, 663 + yy);   //725, 663);
            this.pointButtonClose = new Point(847 + xx, 663 + yy);   //847, 663);

            #endregion

            #region atWork

            this.pointBuyingMitridat1 = new Point(360 + xx, 537 + yy);      //360, 537
            this.pointBuyingMitridat2 = new Point(517 + xx, 433 + yy);      //1392 - 875, 438 - 5
            this.pointBuyingMitridat3 = new Point(517 + xx, 423 + yy);      //1392 - 875, 428 - 5
            this.pointisBoxOverflow1 = new PointColor(523 - 5 + xx, 438 - 5 + yy, 7100000, 5);            //     это правильные точки для определения, переполнился карман или нет
            this.pointisBoxOverflow2 = new PointColor(524 - 5 + xx, 438 - 5 + yy, 7600000, 5);
            this.pointisBoxOverflow3 = new PointColor(379 - 5 + xx, 497 - 5 + yy, 5600000, 5);          //проверка оранжевой надписи
            this.pointisBoxOverflow4 = new PointColor(379 - 5 + xx, 498 - 5 + yy, 5600000, 5);
            //this.pointisBoxOverflow1 = new PointColor(573 - 5 + xx, 488 - 5 + yy, 7500000, 5);          //это неправильные точки. сигнализация о наполненном кармане никогда не сработает
            //this.pointisBoxOverflow2 = new PointColor(574 - 5 + xx, 488 - 5 + yy, 7800000, 5);
            this.arrayOfColorsIsWork1 = new uint[13] { 11051, 1721, 7644, 2764, 16777, 4278, 5138, 3693, 66, 5068, 15824, 8756, 3291 };
            this.arrayOfColorsIsWork2 = new uint[13] { 10919, 2106, 16711, 7243, 3560, 5401, 9747, 10258, 0, 9350, 15767, 8162, 1910 };

            this.pointisKillHero1 = new PointColor(75 + xx, 631 + yy, 1900000, 4);
            this.pointisKillHero2 = new PointColor(330 + xx, 631 + yy, 1900000, 4);
            this.pointisKillHero3 = new PointColor(585 + xx, 631 + yy, 1900000, 4);
            this.pointisLiveHero1 = new PointColor(80 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero2 = new PointColor(335 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero3 = new PointColor(590 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointSkillCook = new Point(183 - 5 + xx, 700 - 5 + yy);
            this.pointisBattleMode1 = new PointColor(173 - 5 + xx, 511 - 5 + yy, 8900000, 5);
            this.pointisBattleMode2 = new PointColor(200 - 5 + xx, 511 - 5 + yy, 8900000, 5);
            this.pointisAssaultMode1 = new PointColor(76 - 5 + xx, 511 - 5 + yy, 10207400, 2);
            this.pointisAssaultMode2 = new PointColor(105 - 5 + xx, 511 - 5 + yy, 10207400, 2);

            //            this.pointisBulletHalf = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5500000, 5);
            this.pointisBulletHalf1 = new PointColor(229 - 5 + xx, 622 - 5 + yy, 5500000, 5);
            this.pointisBulletHalf2 = new PointColor(484 - 5 + xx, 622 - 5 + yy, 5500000, 5);
            this.pointisBulletHalf3 = new PointColor(739 - 5 + xx, 622 - 5 + yy, 5500000, 5);
            //            this.pointisBulletOff  = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5700000, 5);
            this.pointisBulletOff1 = new PointColor(229 - 5 + xx, 622 - 5 + yy, 5700000, 5);
            this.pointisBulletOff2 = new PointColor(484 - 5 + xx, 622 - 5 + yy, 5700000, 5);
            this.pointisBulletOff3 = new PointColor(739 - 5 + xx, 622 - 5 + yy, 5700000, 5);

            #endregion

            #region inTown

            this.pointCure1 = new Point(215 - 5 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой U
            this.pointCure2 = new Point(215 - 5 + 255 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой J
            this.pointCure3 = new Point(215 - 5 + 255 * 2 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой M
            this.pointMana1 = new Point(215 - 5 + 30 + xx, 705 - 5 + yy);                        //бутылка маны под буквой I
            this.pointMana2 = new Point(245 - 5 + 255 + xx, 705 - 5 + yy);                        //бутылка маны под буквой K
            this.pointMana3 = new Point(245 - 5 + 510 + xx, 705 - 5 + yy);                        //бутылка маны под буквой ,
            this.pointisToken1 = new PointColor(478 - 5 + xx, 92 - 5 + yy, 13000000, 5);  //проверяем открыто ли окно с токенами
            this.pointisToken2 = new PointColor(478 - 5 + xx, 93 - 5 + yy, 13000000, 5);
            this.pointToken = new Point(755 - 5 + xx, 94 - 5 + yy);                       //крестик в углу окошка с токенами
            this.pointGM = new Point(439 - 5 + xx, 413 - 5 + yy);
            this.pointHeadGM = new Point(369 - 5 + xx, 290 - 5 + yy);

            this.arrayOfColorsIsTown1 = new uint[13] { 11053, 1710, 7631, 2763, 16777, 4276, 5131, 3684, 65, 5066, 15856, 8750, 3291 };
            this.arrayOfColorsIsTown2 = new uint[13] { 10921, 2105, 16711, 7237, 3552, 5395, 9737, 10263, 0, 9342, 15790, 8158, 1910 };

            //this.pointIsTown_RifleFirstDot1 = new PointColor(24 + xx, 692 + yy, 11053000, 3);       //точки для проверки стойки с ружьем
            //this.pointIsTown_RifleFirstDot2 = new PointColor(25 + xx, 692 + yy, 10921000, 3);
            //this.pointIsTown_ExpRifleFirstDot1 = new PointColor(24 + xx, 692 + yy, 1710000, 3);       //точки для проверки эксп стойки с ружьем
            //this.pointIsTown_ExpRifleFirstDot2 = new PointColor(25 + xx, 692 + yy, 2105000, 3);
            //this.pointIsTown_DrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 7631000, 3);       //точки для проверки обычной стойки с дробашом в городе               
            //this.pointIsTown_DrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 16711000, 3);
            //this.pointIsTown_VetDrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 7631000, 3);       //точки для проверки вет стойки с дробашом в городе              не проверено             
            //this.pointIsTown_VetDrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 16711000, 3);
            //this.pointIsTown_ExpDrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 16777000, 3);       //точки для проверки эксп стойки с дробашом
            //this.pointIsTown_ExpDrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 3552000, 3);
            //this.pointIsTown_JainaDrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 4276000, 3);       //точки для проверки эксп стойки с дробашом Джейн
            //this.pointIsTown_JainaDrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 5395000, 3);
            //this.pointIsTown_VetSabreFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 5131000, 3);       //точки для проверки вет стойки с саблей (повар)
            //this.pointIsTown_VetSabreFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 9737000, 3);
            //this.pointIsTown_ExpSwordFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 3684000, 3);       //точки для проверки эксп стойки с мечом (дарья)
            //this.pointIsTown_ExpSwordFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 10263000, 3);
            //this.pointIsTown_VetPistolFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 65000, 3);       //точки для проверки вет стойки с пистолетом Outrange
            //this.pointIsTown_VetPistolFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 0);
            //this.pointIsTown_SightPistolFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 5066000, 3);       //точки для проверки вет стойки с пистолетом Sight Shot
            //this.pointIsTown_SightPistolFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 9342000, 3);
            //this.pointIsTown_UnlimPistolFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 15856000, 3);      //точки для проверки эксп стойки с пистолетами Unlimited Shot
            //this.pointIsTown_UnlimPistolFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 15790000, 3);
            //this.pointIsTown_ExpCannonFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 8750000, 3);       //точки для проверки пушки Миса
            //this.pointIsTown_ExpCannonFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 8158000, 3);

            #endregion

            #region алхимия

            this.pointisAlchemy1 = new PointColor(513 - 5 + xx, 565 - 5 + yy, 7925494, 0);
            this.pointisAlchemy2 = new PointColor(513 - 5 + xx, 566 - 5 + yy, 7925494, 0);
            this.pointAlchemy = new Point(522 - 5 + xx, 521 - 5 + yy);                                           //кнопка "Start Alchemy"
            this.pointisInventoryFull1 = new PointColor(647 - 130 + xx, 559 - 130 + yy, 7727344, 0);             //переполнение инвентаря при алхимии
            this.pointisInventoryFull2 = new PointColor(647 - 130 + xx, 560 - 130 + yy, 7727344, 0);
            this.pointisOutOfIngredient1_1 = new PointColor(570 - 130 + xx, 645 - 130 + yy, 1973790, 0);             //закончился ОДИН ИЗ ингредиентов
            this.pointisOutOfIngredient1_2 = new PointColor(570 - 130 + xx, 646 - 130 + yy, 1973790, 0);             //
            this.pointOutOfMoney1 = new PointColor(647 - 130 + xx, 540 - 130 + yy, 7700000, 5);
            this.pointOutOfMoney2 = new PointColor(647 - 130 + xx, 541 - 130 + yy, 7700000, 5);

            #endregion

            #region Barack

            this.pointisBarack1 = new PointColor(104 - 5 + xx, 152 - 5 + yy, 2350000, 4);       //проверено
            this.pointisBarack2 = new PointColor(104 - 5 + xx, 155 - 5 + yy, 2350000, 4);
            this.pointisBarack3 = new PointColor(81 - 5 + xx, 63 - 5 + yy, 15300000, 5);       //проверено
            this.pointisBarack4 = new PointColor(81 - 5 + xx, 64 - 5 + yy, 13700000, 5);
            this.pointisBarackTeamSelection1 = new PointColor(15 - 5 + xx, 60 - 5 + yy, 7900000, 5);            //Team Selection
            this.pointisBarackTeamSelection2 = new PointColor(16 - 5 + xx, 60 - 5 + yy, 7900000, 5);            //
            this.pointButtonLogoutFromBarack = new Point(955 - 5 + xx, 700 - 5 + yy);               //кнопка логаут в казарме
            this.pointTeamSelection1 = new Point(140 - 5 + xx, 500 - 5 + yy);                   //проверено
            this.pointTeamSelection2 = new Point(70 - 5 + xx, 355 - 5 + yy);                   //проверено
            this.pointTeamSelection3 = new Point(50 - 5 + xx, 620 - 5 + yy);                   //проверено
            this.sdvigY = 15;
            //this.pointChooseChannel = new Point(820 - 5 + xx, 382 - 5 + yy);                       //переход из меню Alt+Q в меню Alt+F2 (нажатие кнопки Choose a channel)
            this.pointEnterChannel = new Point(646 - 5 + xx, 409 - 5 + yy + (botwindow.getKanal() - 2) * 15);                        //выбор канала в меню Alt+F2
            this.pointMoveNow = new Point(651 - 5 + xx, 591 - 5 + yy);                        //выбор канала в меню Alt+F2
            this.pointNewPlace = new Point(85 + xx, 670 + yy);
            this.pointLastPoint = new Point(210 - 5 + xx, 670 - 5 + yy);
            this.pointisBHLastPoint1 = new PointColor(101 - 5 + xx, 527 - 5 + yy, 11000000, 6);
            this.pointisBHLastPoint2 = new PointColor(101 - 5 + xx, 528 - 5 + yy, 11000000, 6);            

            #endregion

            #region новые боты

            pointPetBegin = new Point(800 - 5 + xx, 220 - 5 + yy);    // 800-5, 220-5
            pointPetEnd = new Point(520 - 5 + xx, 330 - 5 + yy);    // 520-5, 330-5
            this.pointNewName = new Point(490 - 5 + xx, 280 - 5 + yy);                             //строчка, куда надо вводить имя семьи
            this.pointButtonCreateNewName = new Point(465 - 5 + xx, 510 - 5 + yy);                 //кнопка Create для создания новой семьи
            this.pointCreateHeroes = new Point(800 - 5 + xx, 635 - 5 + yy);                        //кнопка Create для создания нового героя (перса)
            this.pointButtonOkCreateHeroes = new Point(520 - 5 + xx, 420 - 5 + yy);                //кнопка Ok для подтверждения создания героя
            this.pointMenuSelectTypeHeroes = new Point(810 - 5 + xx, 260 - 5 + yy);                //меню выбора типа героя в казарме
            this.pointSelectTypeHeroes = new Point(800 - 5 + xx, 320 - 5 + yy);                    //выбор мушкетера в меню типо героев в казарме
            this.pointNameOfHeroes = new Point(800 - 5 + xx, 180 - 5 + yy);                        //нажимаем на строчку, где вводится имя героя (перса)
            this.pointButtonCreateChar = new Point(450 - 5 + xx, 700 - 5 + yy);                    //нажимаем на зеленую кнопку создания нового перса
            this.pointSelectMusk = new Point(320 - 5 + xx, 250 - 5 + yy);                          //нажимаем на строчку, где вводится имя героя (перса)
            this.pointUnselectMedik = new Point(450 - 5 + xx, 250 - 5 + yy);                       //нажимаем на медика и выкидываем из команды
            this.pointNameOfTeam = new Point(30 - 5 + xx, 660 - 5 + yy);                           //нажимаем на строчку, где вводится имя команды героев (в казарме)
            this.pointButtonSaveNewTeam = new Point(190 - 5 + xx, 660 - 5 + yy);                   //нажимаем на кнопку сохранения команды (в казарме)

            this.pointRunNunies = new Point(920 - 5 + xx, 170 - 5 + yy);                           //нажимаем на зеленую стрелку, чтобы бежать к Нуньесу в Стартонии
            this.pointPressNunez = new Point(830 - 5 + xx, 340 - 5 + yy);                          //нажимаем на Нуньеса
            this.ButtonOkDialog = new Point(910 - 5 + xx, 680 - 5 + yy);                           //нажимаем на Ок в диалоге
            this.PressMedal = new Point(300 - 5 + xx, 210 - 5 + yy);                               //нажимаем на медаль
            this.ButtonCloseMedal = new Point(740 - 5 + xx, 600 - 5 + yy);                         //нажимаем на кнопку Close и закрываем медали
            this.pointPressNunez2 = new Point(700 - 5 + xx, 360 - 5 + yy);                         //нажимаем на Нуньеса после надевания медали

            this.town_begin = new EuropaTownReboldo(botwindow);                                   //город взят по умолчанию, как Ребольдо. 
            this.pointPressLindon1 = new Point(590 - 5 + xx, 210 - 5 + yy);                        //нажимаем на Линдона
            this.pointPressGMonMap = new Point(840 - 5 + xx, 235 - 5 + yy);                        //нажимаем на строчку GM на карте Alt+Z
            this.pointPressGM_1 = new Point(555 - 5 + xx, 425 - 5 + yy);                           //нажимаем на голову GM 
            this.pointPressSoldier = new Point(570 - 5 + xx, 315 - 5 + yy);                        //нажимаем на голову солдата
            this.pointFirstStringSoldier = new Point(520 - 5 + xx, 545 - 5 + yy);                  //нажимаем на первую строчку в диалоге
            this.pointRifle = new Point(380 - 5 + xx, 320 - 5 + yy);                               //нажимаем на ружье
            this.pointCoat = new Point(380 - 5 + xx, 345 - 5 + yy);                                //нажимаем на плащ
            this.pointButtonPurchase = new Point(740 - 5 + xx, 590 - 5 + yy);                      //нажимаем на кнопку купить
            this.pointButtonCloseSoldier = new Point(860 - 5 + xx, 590 - 5 + yy);                  //нажимаем на кнопку Close
            this.pointButtonYesSoldier = new Point(470 - 5 + xx, 430 - 5 + yy);                    //нажимаем на кнопку Yes
            this.pointFirstItem = new Point(35 - 5 + xx, 210 - 5 + yy);                            //нажимаем дважды на первую вещь в спецкармане
            this.pointDomingoOnMap = new Point(810 - 5 + xx, 130 - 5 + yy);                        //нажимаем на Доминго на карте Alt+Z
            this.pointPressDomingo = new Point(510 - 5 + xx, 425 - 5 + yy);                        //нажимаем на Доминго
            this.pointFirstStringDialog = new Point(520 - 5 + xx, 660 - 5 + yy);                   //нажимаем Yes в диалоге Доминго (нижняя строчка)
            this.pointSecondStringDialog = new Point(520 - 5 + xx, 640 - 5 + yy);                  //нажимаем Yes в диалоге Доминго второй раз (вторая строчка снизу)
            this.pointDomingoMiss = new Point(396 - 5 + xx, 206 - 5 + yy);                         //нажимаем правой кнопкой по карте миссии Доминго
            this.pointPressDomingo2 = new Point(572 - 5 + xx, 237 - 5 + yy);                       //нажимаем на Доминго после миссии
            this.pointLindonOnMap = new Point(820 - 5 + xx, 385 - 5 + yy);                         //нажимаем на Линдона на карте Alt+Z
            this.pointPressLindon2 = new Point(655 - 5 + xx, 255 - 5 + yy);                        //нажимаем на Линдона
            this.pointPetExpert = new Point(910 - 5 + xx, 415 - 5 + yy);                           //нажимаем на петэксперта
            this.pointPetExpert2 = new Point(815 - 5 + xx, 425 - 5 + yy);                          //нажимаем на петэксперта второй раз 
            this.pointThirdBookmark = new Point(842 - 5 + xx, 150 - 5 + yy);                       //тыкнули в третью закладку в кармане
            this.pointNamePet = new Point(440 - 5 + xx, 440 - 5 + yy);                             //нажимаем на строку, где вводить имя пета
            this.pointButtonNamePet = new Point(520 - 5 + xx, 495 - 5 + yy);                       //тыкнули в кнопку Raise Pet
            this.pointButtonClosePet = new Point(520 - 5 + xx, 535 - 5 + yy);                      //тыкнули в кнопку Close
            this.pointWayPointMap = new Point(820 - 5 + xx, 430 - 5 + yy);                         //тыкнули в строчку телепорт на карте Ребольдо
            this.pointWayPoint = new Point(665 - 5 + xx, 345 - 5 + yy);                            //тыкнули в телепорт
            this.pointBookmarkField = new Point(220 - 5 + xx, 200 - 5 + yy);                       //закладка Field в телепорте
            this.pointButtonLavaPlato = new Point(820 - 5 + xx, 320 - 5 + yy);                     //кнопка лавовое плато в телепорте

            #endregion

            #region кратер
            #endregion

            #region заточка
            #endregion

            #region чиповка
            #endregion

            #region передача песо
            #endregion

            #region Undressing in Barack

            this.pointShowEquipment = new Point(145 - 5 + xx, 442 - 5 + yy);
            //this.pointBarack1 = new Point( 80 - 5 + xx, 152 - 5 + yy);
            //this.pointBarack2 = new Point(190 - 5 + xx, 152 - 5 + yy);
            //this.pointBarack3 = new Point( 80 - 5 + xx, 177 - 5 + yy);
            //this.pointBarack4 = new Point(190 - 5 + xx, 177 - 5 + yy);

            this.pointBarack[1] = new Point(80 - 5 + xx, 152 - 5 + yy);
            this.pointBarack[2] = new Point(190 - 5 + xx, 152 - 5 + yy);
            this.pointBarack[3] = new Point(80 - 5 + xx, 177 - 5 + yy);
            this.pointBarack[4] = new Point(190 - 5 + xx, 177 - 5 + yy);

            this.pointEquipment1 = new PointColor(300 - 5 + xx, 60 - 5 + yy, 12600000, 5);
            this.pointEquipment2 = new PointColor(302 - 5 + xx, 60 - 5 + yy, 12600000, 5);


            #endregion

            #region BH

            this.pointGateInfinityBH = new Point(410 - 5 + xx, 430 - 5 + yy);
            this.pointisBH1 = new PointColor(985 - 30 + xx, 91 - 30 + yy, 10353000, 3);                    // желтый ободок на миникарте (в BH миникарты нет)
            this.pointisBH2 = new PointColor(975 - 30 + xx, 95 - 30 + yy, 5700000, 5);                 //синий ободок на миникарте (в BH миникарты нет)
            // this.arrayOfColors = new uint[19] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 1842730, 3955550, 1250584 };
            this.arrayOfColors = new uint[20] { 0, 1644, 725, 6123, 3088, 1715, 1452, 6608, 14190, 1319, 2302, 5275, 2830, 1577, 525, 2635, 1842, 3955, 1250, 5144 };
            this.pointIsAtak1 = new PointColor(101 - 30 + xx, 541 - 30 + yy, 6000000, 6);                // проверяем, атакует ли бот босса (есть ли зеленый ободок вокруг сабли)
            this.pointIsAtak2 = new PointColor(101 - 30 + xx, 542 - 30 + yy, 6000000, 6);
            this.pointIsRoulette1 = new PointColor(507 - 5 + xx, 83 - 5 + yy, 15000000, 6);
            this.pointIsRoulette2 = new PointColor(509 - 5 + xx, 83 - 5 + yy, 15000000, 6);

            #endregion

            #region Вход-выход
            this.pointisWhatNews1 = new PointColor(976, 712, 15131615, 0);
            this.pointisWhatNews2 = new PointColor(977, 712, 15131615, 0);
            #endregion


        }

        //public ServerEuropa(int numberOfWindow) : this(new botWindow(numberOfWindow))
        //{
        //}



        // ===============================  Методы ==================================================

        #region Общие методы 2

        ///// <summary>
        ///// возвращает параметр, прочитанный из файла
        ///// </summary>
        ///// <returns></returns>
        //private int EuropaActive()
        //{ return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Europa_active.txt")); }

        ///// <summary>
        ///// возвращает параметр, прочитанный из файла
        ///// </summary>
        ///// <returns></returns>
        //private String path_Client()
        //{ return File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Europa_path.txt"); }

        #endregion

        #region No window

        /// <summary>
        /// поиск окна с ошибкой
        /// </summary>
        /// <returns>HWND найденного окна</returns>
        public override UIntPtr FindWindowError()
        {
            UIntPtr HWND = (UIntPtr)0;

            //HWND = FindWindow("Granado Espada", "Granado Espada");
            HWND = FindWindow("Ошибка", "[#] #32770 (Диалоговое окно) [#]");


            //SetWindowPos(HWND, 0, 5, 5, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);

            //Pause(500);

            return HWND;
        }


        public override bool isContinueRunning()
        {
            return false;
        }


        /// <summary>
        /// запуск клиента Steam /На Европейском сервере нет стима. Ничего не делаем/
        /// </summary>
        public override void runClientSteamBH()
        {
        }


        /// <summary>
        /// запуск клиента игры
        /// </summary>
        public override void runClient()
        {
            Process.Start(this.pathClient);
        }

        /// <summary>
        /// запуск клиента игры в чистом окне без песочницы
        /// </summary>
        public override void runClientCW()
        {
            AccountBusy = false;

            Process process = new Process();
            process.StartInfo.FileName = this.pathClient;
            process.StartInfo.Arguments = " -noreactlogin -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            process.Start();
            Pause(10000);

            if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
            {
                pointNewSteamOk.PressMouseL();
            }

            for (int i = 1; i <= 10; i++)
            {
                Pause(1000);

                if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
                {
                    pointNewSteamOk.PressMouseL();
                }

                //if (isContinueRunning())    //если аккаунт запущен на другом компе
                //{
                //    NextAccount();
                //    AccountBusy = true;
                //    break;
                //}
            }
        }


        /// <summary>
        /// действия для оранжевой кнопки
        /// </summary>
        public override void OrangeButton()
        {
            ReOpenWindow();
            Pause(100);
            if (isLogout())
            {
                EnterLoginAndPasword();
            }
        }

        ///// <summary>
        ///// Определяет, надо ли грузить данное окно с ботом
        ///// </summary>
        ///// <returns> true означает, что это окно (данный бот) должно быть активно и его надо грузить </returns>
        //public override bool isActive()
        //{
        //    bool result = false;
        //    if (EuropaActive() == 1) result = true;
        //    return result;
        //}

        /// <summary>
        /// поиск нового окна с игрой 
        /// чистое окно без песочницы
        /// </summary>
        /// <returns></returns>
        public override UIntPtr FindWindowGE_CW()
        {
            UIntPtr HWND = (UIntPtr)0;

            int count = 0;
            while (HWND == (UIntPtr)0)
            {
                Pause(500);

                HWND = FindWindow("Granado Espada", "Granado Espada");

                count++; if (count > 5) return (UIntPtr)0;
            }

            botParam.Hwnd = HWND;
            //botwindow.setHwnd(HWND);

            SetWindowPos(HWND, 0, xx, yy, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            //            ShowWindow(HWND, 2);   //скрыть окно в трей
            Pause(500);

            return HWND;
        }


        /// <summary>
        /// поиск новых окон с игрой для кнопки "Найти окна"
        /// </summary>
        /// <returns></returns>
        public override UIntPtr FindWindowGE()
        {
            UIntPtr HWND = (UIntPtr)0;

            int count = 0;
            while (HWND == (UIntPtr)0)
            {
                Pause(500);
//                HWND = FindWindow("Granado Espada", "Granado Espada Online");
                HWND = FindWindow("Granado Espada", "Granado Espada Europe");

                count++; if (count > 5) return (UIntPtr)0;
            }

            botParam.Hwnd = HWND;
            //botwindow.setHwnd(HWND);

            SetWindowPos(HWND, 1, xx, yy, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            //            ShowWindow(HWND, 2);   //скрыть окно в трей

            Pause(500);


            #region старый вариант метода
            //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(350, 700, 8);
            //Pause(200);
            //while (New_HWND_GE == (UIntPtr)0)                
            //{
            //    Pause(500);
            //    New_HWND_GE = FindWindow("Granado Espada", "Granado Espada Online");
            //}
            //setHwnd(New_HWND_GE);
            //hwnd_to_file();
            ////Перемещает вновь открывшиеся окно в заданные координаты, игнорирует размеры окна
            ////SetWindowPos(New_HWND_GE, 1, getX(), getY(), WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            //SetWindowPos(New_HWND_GE, 1, 825, 5, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            //Pause(500);
            #endregion

            return HWND;
        }

        #endregion

        #region LogOut

        /// <summary>
        /// переключились ли на нужный сервер FERRUCCIO-ESPADA (в режиме логаута)
        /// </summary>
        /// <returns></returns>
        public override bool IsServerSelection()
        {
            return true;    //не нужно выбирать сервер на Европе, поэтому сразу возвращаем true
        }

        #endregion

        #region Top Menu



        /// <summary>
        /// нажмает на выбранный раздел верхнего меню 
        /// </summary>
        /// <param name="numberOfThePartitionMenu"> ноиер раздела верхнего меню </param>
        public override void TopMenu(int numberOfThePartitionMenu)
        {
            int[] MenukoordX = { 300, 333, 365, 398, 431, 470, 518, 565, 606, 637, 669, 700, 733 };
            int x = MenukoordX[numberOfThePartitionMenu - 1];
            int y = 55;
            iPoint pointMenu = new Point(x + botwindow.getX(), y + botwindow.getY());

            //do
            //    {
            pointMenu.PressMouse();
            //PressMouse(x, y);
            Pause(1000);
            //if ((isLogout()) || (!botwindow.isHwnd())) break;    //если вылетели в логаут или закрылось окно с игрой, то выход из цикла.  (29.04.2017) 
            //} while (!isOpenTopMenu(numberOfThePartitionMenu));
        }

        /// <summary>
        /// нажать на выбранный раздел верхнего меню, а далее на пункт раскрывшегося списка
        /// </summary>
        /// <param name="numberOfThePartitionMenu"></param>
        /// <param name="punkt"></param>
        public override void TopMenu(int numberOfThePartitionMenu, int punkt)
        {
            int[] numberOfPunkt = { 0, 8, 4, 5, 0, 3, 2, 6, 9, 0, 0, 0, 0 };
            int[] MenukoordX = { 300, 333, 365, 398, 431, 470, 518, 565, 606, 637, 669, 700, 733 };
            int[] FirstPunktOfMenuKoordY = { 0, 80, 80, 80, 0, 92, 92, 92, 80, 0, 0, 0, 0 };

            if (punkt <= numberOfPunkt[numberOfThePartitionMenu - 1])
            {
                int x = MenukoordX[numberOfThePartitionMenu - 1];
                int y = FirstPunktOfMenuKoordY[numberOfThePartitionMenu - 1] + 25 * (punkt - 1);
                iPoint pointMenu = new Point(x + botwindow.getX(), y + botwindow.getY());

                TopMenu(numberOfThePartitionMenu);   //сначала открываем раздел верхнего меню (1-13)
                Pause(500);
                pointMenu.PressMouse();  //выбираем конкретный пункт подменю (раскрывающийся список)
                //PressMouse(x, y);  //выбираем конкретный пункт подменю (раскрывающийся список)
            }
        }

        /// <summary>
        /// телепортируемся в город продажи по Alt+W (Америка)
        /// </summary>
        public override void TeleportToTownAltW(int nomerTeleport)
        {
            iPoint pointTeleportToTownAltW = new Point(806 - 5 + xx, 517 - 5 + yy + (nomerTeleport - 1) * 17);

            TopMenu(6, 1);
            Pause(1000);
            pointTeleportToTownAltW.PressMouse();           //было два нажатия левой, решил попробовать RRL
            Pause(2000);
        }

        /// <summary>
        /// вызываем телепорт через верхнее меню и телепортируемся по указанному номеру телепорта
        /// </summary>
        /// <param name="NumberOfLine"></param>
        public override void Teleport(int NumberOfLine)
        {
            Pause(400);
            TopMenu(12);                     // Click Teleport menu

            Point pointTeleportNumbertLine = new Point(405 - 5 + xx, 180 - 5 + (NumberOfLine - 1) * 15 + yy);    //              тыкаем в указанную строчку телепорта  405 - 5 + xx, 198 - 5 + yy

            pointTeleportNumbertLine.DoubleClickL();   // Указанная строка в списке телепортов
            Pause(500);

            pointTeleportExecute.PressMouseL();        // Click on button Execute in Teleport menu
            Pause(2000);
        }

        #endregion

        #region at Work

        /// <summary>
        /// пополняем патроны в кармане на 10 000 штук
        /// </summary>
        public override void AddBullet10000()
        {
        }

        /// <summary>
        /// проверяем, находится ли в инвентае 248 вещей 
        /// </summary>
        /// <returns></returns>
        public override bool is248Items()
        {
            return true;
        }

        #endregion

        #region Заточка

        /// <summary>
        /// переносим (DragAndDrop) одну из частей экипировки на место для заточки
        /// </summary>
        /// <param name="numberOfEquipment">номер экипировки п/п</param>
        public override void MoveToSharpening(int numberOfEquipment)
        {
            iPoint pointEquipmentBegin = new Point(701 - 5 + xx + (numberOfEquipment - 1) * 39, 183 - 5 + yy);
            iPoint pointEquipmentEnd = new Point(521 - 5 + xx, 208 - 5 + yy);
            pointEquipmentBegin.Drag(pointEquipmentEnd);
        }

        #endregion

        #region чиповка

        /// <summary>
        /// переносим (DragAndDrop) одну из частей экипировки на место для чиповки
        /// </summary>
        /// <param name="numberOfEquipment">номер экипировки п/п</param>
        public override void MoveToNintendo(int numberOfEquipment)
        {
            iPoint pointEquipmentBegin = new Point(701 - 5 + xx + (numberOfEquipment - 1) * 39, 183 - 5 + yy);
            iPoint pointEquipmentEnd = new Point(631 - 5 + xx, 367 - 5 + yy);
            pointEquipmentBegin.Drag(pointEquipmentEnd);
        }

        #endregion

        #region BH

        /// <summary>
        /// найдены ли окна с ГЭ ??
        /// </summary>
        /// <returns></returns>
        public override bool FindWindowGEforBHBool()
        {
            bool result = false;
            UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada [#]");

            if (HWND != (UIntPtr)0)
            {
                botParam.Hwnd = HWND;  //если окно найдено, то запись в файл HWND.txt
                //isLoadedGEBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
                result = true;  //нашли окно
            }
            return result;
        }

        /// <summary>
        /// поиск новых окон Steam
        /// </summary>
        /// <returns>true, если стим найден</returns>
        public override bool FindWindowSteamBool()
        {
            bool result = false;
            UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", "Steam");
            if (HWND != (UIntPtr)0)
            {
                result = true;
                //isLoadedGEBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
            }
            return result;
        }

        /// <summary>
        /// запуск клиента игры для Инфинити
        /// </summary>
        public override void runClientBH()
        {
        }

        ///// <summary>
        ///// поиск нового окна с игрой для миссий Инфинити в БХ
        ///// </summary>
        ///// <returns></returns>
        //public override UIntPtr FindWindowGEforBH()
        //{
        //    UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada [#]");

        //    if (HWND != (UIntPtr)0)
        //    {
        //        botParam.Hwnd = HWND;  //если окно найдено, то запись в файл HWND.txt
        //        SetWindowPos(HWND, 0, xx, yy, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);  //перемещение окна в заданные координаты
        //    }

        //    return HWND;
        //}

        ///// <summary>
        ///// поиск новых окон Steam
        ///// </summary>
        ///// <returns>номер hwnd найденного Steam</returns>
        //public override UIntPtr FindWindowSteam()
        //{
        //    return (UIntPtr)0;
        //}


        /// <summary>
        /// проверка миссии по цвету контрольной точки
        /// </summary>
        /// <returns> номер цвета </returns>
        public override uint ColorOfMissionBH()
        {
            return new PointColor(700 - 30 + xx, 500 - 30 + yy, 0, 0).GetPixelColor();
        }

        ///// <summary>
        ///// нажать указанную строку в диалоге в воротах Infinity BH. Отсчет снизу вверх
        ///// </summary>
        ///// <param name="number"></param>
        //public override void PressStringInfinityGateBH(int number)
        //{
        //    iPoint pointString = new Point(839 - 30 + xx, 363 - 30 + yy - (number - 1) * 19);
        //    pointString.PressMouse();
        //    Pause(2000);
        //}

        #endregion

        #region inTown

        /// <summary>
        /// идем к высокой бабе в Ребольдо GM /карта города уже открыта/
        /// </summary>
        public override void GotoGM()
        {
            pointGM.PressMouseL();
        }

        /// <summary>
        /// тыкаем в голову GM
        /// </summary>
        public override void PressToHeadGM()
        {
            pointHeadGM.PressMouseL();
        }

        #endregion

        /// <summary>
        /// запуск клиента игры для Demonic
        /// </summary>
        public override void RunClientDem()
        {
            #region для песочницы

            //запускаем steam в песочнице (вариант 1)
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -applaunch 663090 -silent";
            //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            process.Start();

            #endregion
        }

        /// <summary>
        /// запуск клиента игры для GE Classic
        /// </summary>
        public override void RunClientClassic()
        {
            #region для песочницы

            //запускаем игру в песочнице 
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " C:\\Games\\ge_classique_client\\GE Classique.exe";
            process.Start();

            #endregion
        }


        /// <summary>
        /// найдено ли окно ГЭ в текущей песочнице? Если найдено, то в файл hwnd.txt пишем найденный hwnd 
        /// </summary>
        /// <returns>true, если найдено</returns>
        public override bool FindWindowGEClassic()
        {
            bool result = false;
            UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada Classique [#]");

            if (HWND != (UIntPtr)0)
            {
                botParam.Hwnd = HWND;  //если окно найдено, то запись в файл HWND.txt
                //isLoadedGEBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
                result = true;  //нашли окно
            }
            return result;
        }
    }
}
