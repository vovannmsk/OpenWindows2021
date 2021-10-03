using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GEBot.Data;



namespace OpenGEWindows
{
    /// <summary>
    /// Класс описывает процесс перехода к торговле от фарма , начиная от проверки необходимости продажи и заканчивая закрытием окна с ботом (для сервера Сингапур (Avalon))
    /// </summary>
    public class ServerEuropa2 : Server
    {
        [DllImport("user32.dll")]
        private static extern UIntPtr FindWindow(String ClassName, String WindowName);  //ищет окно с заданным именем и классом


        //основной конструктор
        public ServerEuropa2(botWindow botwindow)
        {
            isLoadedGEBH = false;   
            isLoadedSteamBH = false;

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

            TownFactory townFactory = new Europa2TownFactory(botwindow);                                     // здесь выбирается конкретная реализация для фабрики Town
            this.town = townFactory.createTown();
            this.town_begin = new Europa2TownReboldo(botwindow);   //город взят по умолчанию, как Ребольдо. 
            dialog = new DialogEuropa2(botwindow);

            #endregion

            #region параметры, зависящие от сервера
            //            this.pathClient = path_Client();
            this.pathClient = serverParam.PathClient;
            this.isActiveServer = serverParam.IsActiveServer;

            #endregion

            #region No Window
            this.pointSafeIP1 = new PointColor(941, 579, 13600000, 5);
            this.pointSafeIP2 = new PointColor(942, 579, 13600000, 5);
            this.pointisSteam1 = new PointColor(1037, 553, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointisSteam2 = new PointColor(1038, 553, 14471368, 0);//для Стима используются абсолютные координаты
            this.pointSteamLogin1 = new Point(1150, 455);//для Стима используются абсолютные координаты
            this.pointSteamLogin2 = new Point(844, 455);//для Стима используются абсолютные координаты
            this.pointSteamPassword = new Point(862, 489);//для Стима используются абсолютные координаты
            this.pointSteamSavePassword = new Point(843, 518);//для Стима используются абсолютные координаты
            this.pointSteamOk = new Point(902, 551);//для Стима используются абсолютные координаты
            this.pointisNewSteam1 = new PointColor(998, 715, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointisNewSteam2 = new PointColor(999, 715, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointNewSteamOk = new Point(983, 712);   //кнопка "Соглашаюсь", когда входишь в новый акк
            
            //this.pointisContinueRunning1 = new PointColor(861, 551, 14400000, 5); //для Стима используются абсолютные координаты
            //this.pointisContinueRunning2 = new PointColor(862, 551, 14400000, 5); //для Стима используются абсолютные координаты
            this.pointisContinueRunning1 = new PointColor(1042, 551, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointisContinueRunning2 = new PointColor(1043, 551, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointCancelContinueRunning = new Point(1044, 554); //для Стима используются абсолютные координаты
            #endregion

            #region Logout

//            this.pointConnect = new PointColor(696 - 5 + xx, 148 - 5 + yy, 7800000, 5);
            this.pointConnect = new PointColor(547 - 30 + xx, 441 - 5 + yy, 7800000, 5);
            //this.pointisLogout1 = new PointColor(565 - 5 + xx, 532 - 5 + yy, 16000000, 6);       // проверено   слово Leave Game буква L
            //this.pointisLogout2 = new PointColor(565 - 5 + xx, 531 - 5 + yy, 16000000, 6);       // проверено
            this.pointisLogout1 = new PointColor(935 - 5 + xx, 707 - 5 + yy, 7925494, 0);       // проверено   слово Ver буква r
            this.pointisLogout2 = new PointColor(935 - 5 + xx, 708 - 5 + yy, 7925494, 0);       // проверено
            //this.pointIsServerSelection1 = new PointColor(430 - 5 + xx, 340 - 5 + yy, 5848111, 0);    // проверено
            //this.pointIsServerSelection2 = new PointColor(430 - 5 + xx, 341 - 5 + yy, 5848111, 0);    // проверено
            //pointserverSelection = new Point(480 - 5 + xx, 344 - 5 + yy); //синг. первая строка
            this.pointIsServerSelection1 = new PointColor(430 - 5 + xx, 390 - 5 + yy, 5848111, 0);    // проверено
            this.pointIsServerSelection2 = new PointColor(430 - 5 + xx, 391 - 5 + yy, 5848111, 0);    // проверено
            pointserverSelection = new Point(480 - 5 + xx, 394 - 5 + yy); //европа. третья строка

            #endregion

            #region Pet

            this.pointisSummonPet1 = new PointColor(494 - 5 + xx, 304 - 5 + yy, 13000000, 6);
            this.pointisSummonPet2 = new PointColor(494 - 5 + xx, 305 - 5 + yy, 13000000, 6);
            this.pointisActivePet1 = new PointColor(493 - 5 + xx, 310 - 5 + yy, 13000000, 6);
            this.pointisActivePet2 = new PointColor(494 - 5 + xx, 309 - 5 + yy, 13000000, 6);
            this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                      //не проверено
            this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointisOpenMenuPet1 = new PointColor(474 - 5 + xx, 219 - 5 + yy, 12000000, 6);      //834 - 5, 98 - 5, 12400000, 835 - 5, 98 - 5, 12400000, 5);             //проверено
            this.pointisOpenMenuPet2 = new PointColor(474 - 5 + xx, 220 - 5 + yy, 12000000, 6);
            this.pointCancelSummonPet = new Point(410 - 5 + xx, 390 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(540 - 5 + xx, 380 - 5 + yy);                   // 868, 258   //Click Pet
            this.pointSummonPet2 = new Point(410 - 5 + xx, 360 - 5 + yy);                   // 748, 238   //Click кнопку "Summon"
            this.pointActivePet = new Point(410 - 5 + xx, 410 - 5 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion

            #region Top Menu

            this.pointisOpenTopMenu21 = new PointColor(337 - 5 + xx, 76 - 5 + yy, 13421721, 0);
            this.pointisOpenTopMenu22 = new PointColor(337 - 5 + xx, 77 - 5 + yy, 13421721, 0);
            this.pointisOpenTopMenu61 = new PointColor(512 - 5 + xx, 125 - 5 + yy, 16711422, 0);      //буква "M" в слове Zone Map
            this.pointisOpenTopMenu62 = new PointColor(512 - 5 + xx, 126 - 5 + yy, 16711422, 0);
            this.pointisOpenTopMenu81 = new PointColor(562 - 5 + xx, 89 - 5 + yy, 13421721, 0);
            this.pointisOpenTopMenu82 = new PointColor(562 - 5 + xx, 90 - 5 + yy, 13421721, 0);
            this.pointisOpenTopMenu91 = new PointColor(597 - 5 + xx, 112 - 5 + yy, 16711422, 0);      //буква "Р" в слове Pet
            this.pointisOpenTopMenu92 = new PointColor(597 - 5 + xx, 113 - 5 + yy, 16711422, 0);
            this.pointisOpenTopMenu121 = new PointColor(600 - 5 + xx, 116 - 5 + yy, 8000000, 6);        //Warp List
            this.pointisOpenTopMenu122 = new PointColor(610 - 5 + xx, 116 - 5 + yy, 8000000, 6);
            this.pointisOpenTopMenu131 = new PointColor(404 - 5 + xx, 278 - 5 + yy, 16000000, 6);          //Quest Name                                                         //проверено
            this.pointisOpenTopMenu132 = new PointColor(404 - 5 + xx, 279 - 5 + yy, 16000000, 6);
            this.pointisOpenMenuChooseChannel1 = new PointColor(500 - 5 + xx, 248 - 5 + yy, 8036794, 0);   //Menu of Choose a channel
            this.pointisOpenMenuChooseChannel2 = new PointColor(501 - 5 + xx, 248 - 5 + yy, 8036794, 0);
            this.pointIsCurrentChannel1 = new PointColor(571 - 5 + xx, 286 - 5 + yy, 10000000, 6);          //Channel = 1
            this.pointIsCurrentChannel2 = new PointColor(571 - 5 + xx, 287 - 5 + yy, 10000000, 6);
            this.pointIsCurrentChannel3 = new PointColor(571 - 5 + xx, 286 - 5 + yy, 11000000, 6);          //Channel = 1
            this.pointIsCurrentChannel4 = new PointColor(571 - 5 + xx, 287 - 5 + yy, 11000000, 6);

            this.pointGotoEnd = new Point(685 - 5 + xx, 470 - 5 + yy);            //end
            this.pointLogout = new Point(685 - 5 + xx, 440 - 5 + yy);            //логаут
            this.pointGotoBarack = new Point(685 - 5 + xx, 380 - 5 + yy);            //в барак

            this.pointTeleportFirstLine = new Point(400 + xx, 178 + yy);    //              тыкаем в первую строчку телепорта                          //проверено
            //this.pointTeleportSecondLine = new Point(400 + xx, 193 + yy);   //              тыкаем во вторую строчку телепорта                          //проверено
            this.pointTeleportExecute = new Point(355 + xx, 580 + yy);   //355, 570               тыкаем в кнопку Execute                   //проверено

            #endregion

            #region Shop

            this.pointIsSale1 = new PointColor(907 + xx, 675 + yy, 7200000, 5);
            this.pointIsSale2 = new PointColor(907 + xx, 676 + yy, 7800000, 5);
            this.pointIsSale21 = new PointColor(841 - 5 + xx, 665 - 5 + yy, 7900000, 5);
            this.pointIsSale22 = new PointColor(841 - 5 + xx, 668 - 5 + yy, 7900000, 5);
            this.pointIsClickSale1 = new PointColor(731 - 5 + xx, 662 - 5 + yy, 7900000, 5);
            this.pointIsClickSale2 = new PointColor(731 - 5 + xx, 663 - 5 + yy, 7900000, 5);

            this.pointBookmarkSell = new Point(225 + xx, 163 + yy);
            this.pointSaleToTheRedBottle = new Point(335 + xx, 220 + yy);
            this.pointSaleOverTheRedBottle = new Point(335 + xx, 220 + yy);
            this.pointWheelDown = new Point(375 + xx, 220 + yy);           //345 + 30 + botwindow.getX(), 190 + 30 + botwindow.getY(), 3);        // колесо вниз
            this.pointButtonBUY = new Point(725 + xx, 663 + yy);   //725, 663);
            this.pointButtonSell = new Point(725 + xx, 663 + yy);   //725, 663);
            this.pointButtonClose = new Point(847 + xx, 663 + yy);   //847, 663);
            this.pointBuyingMitridat1 = new Point(360 + xx, 537 + yy);      //360, 537
            this.pointBuyingMitridat2 = new Point(517 + xx, 433 + yy);      //1392 - 875, 438 - 5
            this.pointBuyingMitridat3 = new Point(517 + xx, 423 + yy);      //1392 - 875, 428 - 5

            #endregion

            #region atWork

            this.pointisBoxOverflow1 = new PointColor(522 - 5 + xx, 434 - 5 + yy, 7700000, 5);        //проверка всплывающего окна
            this.pointisBoxOverflow2 = new PointColor(522 - 5 + xx, 435 - 5 + yy, 7700000, 5);
            this.pointisBoxOverflow3 = new PointColor(379 - 5 + xx, 497 - 5 + yy, 5600000, 5);          //проверка оранжевой надписи
            this.pointisBoxOverflow4 = new PointColor(379 - 5 + xx, 498 - 5 + yy, 5600000, 5);

            //this.arrayOfColorsIsWork1 = new uint[16] { 11051, 1721, 7644, 2764, 16777, 4278, 5138, 3693, 66, 5068, 15824, 8756, 3291, 5400, 2569, 3291 };
            //this.arrayOfColorsIsWork2 = new uint[16] { 10919, 2106, 16711, 7243, 3560, 5401, 9747, 10258, 0, 9350, 15767, 8162, 1910, 3624, 3616, 1910 };

            this.arrayOfColorsIsWork1 = new uint[16] { 12565, 4094, 4545, 16383, 9371, 8231, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            this.arrayOfColorsIsWork2 = new uint[16] { 12169, 2850, 2438, 16777, 3562,    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // ружье, флинт, outrange, эксп дробаш, джайна, Миса 

            this.pointisKillHero1 = new PointColor(80 - 5 + xx, 636 - 5 + yy, 1900000, 5);
            this.pointisKillHero2 = new PointColor(335 - 5 + xx, 636 - 5 + yy, 1900000, 5);
            this.pointisKillHero3 = new PointColor(590 - 5 + xx, 636 - 5 + yy, 1900000, 5);
            this.pointisLiveHero1 = new PointColor( 80 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero2 = new PointColor(335 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero3 = new PointColor(590 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointSkillCook = new Point(183 - 5 + xx, 700 - 5 + yy);
            //this.pointisBattleMode1 = new PointColor(173 - 5 + xx, 511 - 5 + yy, 8900000, 5);
            //this.pointisBattleMode2 = new PointColor(200 - 5 + xx, 511 - 5 + yy, 8900000, 5);
            this.pointisBattleMode1 = new PointColor(172 - 5 + xx, 511 - 5 + yy, 10000000, 6);
            this.pointisBattleMode2 = new PointColor(201 - 5 + xx, 511 - 5 + yy, 10000000, 6);
            this.pointisAssaultMode1 = new PointColor(76 - 5 + xx, 521 - 5 + yy, 6000000, 6);
            this.pointisAssaultMode2 = new PointColor(76 - 5 + xx, 522 - 5 + yy, 6000000, 6);

            //            this.pointisBulletHalf = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5500000, 5);
            this.pointisBulletHalf1 = new PointColor(229 - 5 + xx, 622 - 5 + yy, 8245488, 0);
            this.pointisBulletHalf2 = new PointColor(484 - 5 + xx, 622 - 5 + yy, 8245488, 0);
            this.pointisBulletHalf3 = new PointColor(739 - 5 + xx, 622 - 5 + yy, 8245488, 0);
            //            this.pointisBulletOff  = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5700000, 5);
            this.pointisBulletOff1 = new PointColor(229 - 5 + xx, 622 - 5 + yy, 401668, 0);
            this.pointisBulletOff2 = new PointColor(484 - 5 + xx, 622 - 5 + yy, 401668, 0);
            this.pointisBulletOff3 = new PointColor(739 - 5 + xx, 622 - 5 + yy, 401668, 0);



            #endregion

            #region inTown

            this.pointisToken1 = new PointColor(478 - 5 + xx, 92 - 5 + yy, 13000000, 5);  //проверяем открыто ли окно с токенами
            this.pointisToken2 = new PointColor(478 - 5 + xx, 93 - 5 + yy, 13000000, 5);
            this.pointToken = new Point(755 - 5 + xx, 94 - 5 + yy);                       //крестик в углу окошка с токенами
            this.pointCure1 = new Point(215 - 5 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой U
            this.pointCure2 = new Point(215 - 5 + 255 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой J
            this.pointCure3 = new Point(215 - 5 + 255 * 2 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой M
            this.pointMana1 = new Point(245 - 5 + xx, 705 - 5 + yy);                        //бутылка маны под буквой I
            this.pointMana2 = new Point(245 - 5 + 255 + xx, 705 - 5 + yy);                  //бутылка маны под буквой K
            this.pointMana3 = new Point(245 - 5 + 510 + xx, 705 - 5 + yy);                  //бутылка маны под буквой ,
            this.pointGM = new Point(439 - 5 + xx, 413 - 5 + yy);
            this.pointHeadGM = new Point(394 - 5 + xx, 394 - 5 + yy);

            //this.arrayOfColorsIsTown1 = new uint[16] { 11053, 1710, 7631, 2763, 16777, 4276, 5131, 3684, 65, 5066, 15856, 8750, 3291, 5395, 3291, 2565 };
            //this.arrayOfColorsIsTown2 = new uint[16] { 10921, 2105, 16711, 7237, 3552, 5395, 9737, 10263, 0, 9342, 15790, 8158, 1910, 3618, 1910, 3618 };
            // ружье, флинт, дробаш, вет дробаш, эксп дробаш, джайна, повар вет, C.Daria, outrange, Sight Shot, Unlimited Shot (эксп пистолет), Миса, и еще 4 непонятно кто

            this.arrayOfColorsIsTown1 = new uint[6] { 12566, 4079, 4539, 16382, 9342, 8224 };
            this.arrayOfColorsIsTown2 = new uint[6] { 12171, 2829, 2434, 16777, 3552, 0 };
            // ружье, флинт, outrange, эксп дробаш, джайна, Миса 



            //this.pointIsTown_RifleFirstDot1 = new PointColor(24 + xx, 692 + yy, 11053000, 3);        //точки для проверки обычной стойки с ружьем
            //this.pointIsTown_RifleFirstDot2 = new PointColor(25 + xx, 692 + yy, 10921000, 3);
            //this.pointIsTown_ExpRifleFirstDot1 = new PointColor(24 + xx, 692 + yy, 1710000, 4);       //точки для проверки эксп стойки с ружьем
            //this.pointIsTown_ExpRifleFirstDot2 = new PointColor(25 + xx, 692 + yy, 2100000, 4);
            //this.pointIsTown_DrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 7631000, 3);       //точки для проверки обычной стойки с дробашом в городе               
            //this.pointIsTown_DrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 16711000, 3);
            //this.pointIsTown_VetDrobFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 2763000, 3);       //точки для проверки вет стойки с дробашом в городе            
            //this.pointIsTown_VetDrobFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 7237000, 3);
            //this.pointIsTown_ExpDrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 16777000, 3);       //точки для проверки эксп стойки с дробашом
            //this.pointIsTown_ExpDrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 3552000, 3);
            //this.pointIsTown_JainaDrobFirstDot1 = new PointColor(24 + xx, 692 + yy, 4276000, 3);       //точки для проверки эксп стойки с дробашом Джейн
            //this.pointIsTown_JainaDrobFirstDot2 = new PointColor(25 + xx, 692 + yy, 5395000, 3);
            //this.pointIsTown_VetSabreFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 5131000, 3);       //точки для проверки вет стойки с саблей (повар)
            //this.pointIsTown_VetSabreFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 9737000, 3);
            //this.pointIsTown_ExpSwordFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 3684000, 3);       //точки для проверки эксп стойки с мечом (дарья)
            //this.pointIsTown_ExpSwordFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 10263000, 3);
            ////пистолеты
            //this.pointIsTown_VetPistolFirstDot1   = new PointColor(29 - 5 + xx, 697 - 5 + yy, 65000, 3);       //точки для проверки вет стойки с пистолетом Outrange
            //this.pointIsTown_VetPistolFirstDot2   = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 0);
            //this.pointIsTown_SightPistolFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 5066000, 3);       //точки для проверки вет стойки с пистолетом Sight Shot
            //this.pointIsTown_SightPistolFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 9342000, 3);
            //this.pointIsTown_UnlimPistolFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 15856000, 3);      //точки для проверки эксп стойки с пистолетами Unlimited Shot
            //this.pointIsTown_UnlimPistolFirstDot2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 15790000, 3);
            ////пушка Миса
            //this.pointIsTown_ExpCannonFirstDot1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 8750000, 3);         //точки для проверки пушки Миса
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

            this.sdvigY = 0;
            this.pointMoveNow = new Point(651 - 5 + xx, 591 - 5 + yy);                        //выбор канала в меню Alt+F2
            this.pointisBarack1 = new PointColor(53 - 5 + xx, 154 - 5 + yy, 2400000, 5);            //зеленый цвет в слове Barracks  
            this.pointisBarack2 = new PointColor(53 - 5 + xx, 155 - 5 + yy, 2400000, 5);            //проверено
            this.pointisBarack3 = new PointColor(42 - 5 + xx, 61 - 5 + yy, 15500000, 5);            //проверено   Barrack Mode буква "B"
            this.pointisBarack4 = new PointColor(42 - 5 + xx, 62 - 5 + yy, 15500000, 5);            //проверено
            this.pointisBarack5 = new PointColor(105 - 5 + xx, 41 - 5 + yy, 8036794, 0);            //страница создания нового персонажа
            this.pointisBarack6 = new PointColor(106 - 5 + xx, 41 - 5 + yy, 8036794, 0);            //
            this.pointisBarackTeamSelection1 = new PointColor(23 - 5 + xx, 68 - 5 + yy, 7600000, 5);            //Team Member
            this.pointisBarackTeamSelection2 = new PointColor(23 - 5 + xx, 69 - 5 + yy, 7600000, 5);            //
            this.pointTeamSelection1 = new Point(140 - 5 + xx, 533 - 5 + yy);                   //проверено
            this.pointTeamSelection2 = new Point(70 - 5 + xx, 355 - 5 + yy);                   //проверено
            this.pointTeamSelection3 = new Point(50 - 5 + xx, 620 - 5 + yy);                   //проверено
            this.pointButtonLogoutFromBarack = new Point(785 - 5 + xx, 700 - 5 + yy);               //кнопка логаут в казарме
            //this.pointChooseChannel = new Point(820 - 5 + xx, 382 - 5 + yy);                       //переход из меню Alt+Q в меню Alt+F2 (нажатие кнопки Choose a channel)
            this.pointEnterChannel = new Point(646 - 5 + xx, 409 - 5 + yy + (botwindow.getKanal() - 2) * 15);                        //выбор канала в меню Alt+F2
            this.pointNewPlace = new Point(85 + xx, 670 + yy);
            this.pointLastPoint = new Point(210 - 5 + xx, 670 - 5 + yy);
            this.pointisBHLastPoint1 = new PointColor(101 - 30 + xx, 527 - 30 + yy, 11000000, 6);            
            this.pointisBHLastPoint2 = new PointColor(101 - 30 + xx, 528 - 30 + yy, 11000000, 6);
            this.pointCreateButton = new Point(447 - 5 + xx, 700 - 5 + yy);

            #endregion

            #region  новые боты


            this.pointNewName = new Point(490 - 5 + xx, 280 - 5 + yy);                             //строчка, куда надо вводить имя семьи
            this.pointButtonCreateNewName = new Point(465 - 5 + xx, 510 - 5 + yy);                 //кнопка Create для создания новой семьи
            this.pointCreateHeroes = new Point(800 - 5 + xx, 655 - 5 + yy);                        //кнопка Create для создания нового героя (перса)
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
    //      this.pointPressNunez = new Point(830 - 5 + xx, 340 - 5 + yy);                          //нажимаем на Нуньеса
            this.pointPressNunez = new Point(848 - 5 + xx, 307 - 5 + yy);                          //нажимаем на Нуньеса
            this.ButtonOkDialog = new Point(910 - 5 + xx, 680 - 5 + yy);                           //нажимаем на Ок в диалоге
            this.PressMedal = new Point(300 - 5 + xx, 210 - 5 + yy);                               //нажимаем на медаль
            this.ButtonCloseMedal = new Point(740 - 5 + xx, 600 - 5 + yy);                         //нажимаем на кнопку Close и закрываем медали
            this.pointPressNunez2 = new Point(700 - 5 + xx, 360 - 5 + yy);                         //нажимаем на Нуньеса после надевания медали
                                               
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
//            this.pointDomingoOnMap = new Point(820 - 5 + xx, 115 - 5 + yy);                        //нажимаем на Доминго на карте Alt+Z
            this.pointDomingoOnMap = new Point(820 - 5 + xx, 130 - 5 + yy);                        //нажимаем на Доминго на карте Alt+Z
            this.pointPressDomingo = new Point(508 - 5 + xx, 404 - 5 + yy);                        //нажимаем на Доминго
            this.pointFirstStringDialog = new Point(520 - 5 + xx, 660 - 5 + yy);                   //нажимаем Yes в диалоге Доминго (нижняя строчка)
            this.pointSecondStringDialog = new Point(520 - 5 + xx, 640 - 5 + yy);                  //нажимаем Yes в диалоге Доминго второй раз (вторая строчка снизу)
            this.pointDomingoMiss = new Point(396 - 5 + xx, 206 - 5 + yy);                         //нажимаем правой кнопкой по карте миссии Доминго
            this.pointPressDomingo2 = new Point(572 - 5 + xx, 237 - 5 + yy);                       //нажимаем на Доминго после миссии
            this.pointLindonOnMap = new Point(820 - 5 + xx, 445 - 5 + yy);                         //нажимаем на Линдона на карте Alt+Z
            //this.pointLindonOnMap = new Point(514 - 5 + xx, 392 - 5 + yy);                         //нажимаем на Линдона на карте Alt+Z
            this.pointPressLindon2 = new Point(663 - 5 + xx, 268 - 5 + yy);                        //нажимаем на Линдона
            this.pointPetExpert = new Point(771 - 5 + xx, 333 - 5 + yy);                           //нажимаем на петэксперта
            this.pointPetExpert2 = new Point(759 - 5 + xx, 336 - 5 + yy);                          //нажимаем на петэксперта второй раз 
            this.pointThirdBookmark = new Point(842 - 5 + xx, 150 - 5 + yy);                       //тыкнули в третью закладку в кармане
            this.pointNamePet = new Point(440 - 5 + xx, 440 - 5 + yy);                             //нажимаем на строку, где вводить имя пета
            this.pointButtonNamePet = new Point(520 - 5 + xx, 495 - 5 + yy);                       //тыкнули в кнопку Raise Pet
            this.pointButtonClosePet = new Point(520 - 5 + xx, 535 - 5 + yy);                      //тыкнули в кнопку Close
            this.pointWayPointMap = new Point(820 - 5 + xx, 430 - 5 + yy);                         //тыкнули в строчку телепорт на карте Ребольдо
            this.pointWayPoint = new Point(665 - 5 + xx, 345 - 5 + yy);                            //тыкнули в телепорт
            this.pointBookmarkField = new Point(220 - 5 + xx, 200 - 5 + yy);                       //закладка Field в телепорте
            this.pointButtonLavaPlato = new Point(820 - 5 + xx, 320 - 5 + yy);                     //кнопка лавовое плато в телепорте
            this.pointPetBegin = new Point(920 - 5 + xx, 180 - 5 + yy);                           // коробка с петом лежит здесь
            this.pointPetEnd = new Point(520 - 5 + xx, 330 - 5 + yy);                            // коробка с петом перетаскиваем сюда

            #endregion

            #region кратер
            this.pointGateCrater = new Point(373 - 5 + xx, 605 - 5 + yy);                          //переход (ворота) из лавового плато в кратер
            this.pointMitridat = new Point(800 - 5 + xx, 180 - 5 + yy);                            //митридат в кармане
            this.pointMitridatTo2 = new Point(30 - 5 + xx, 140 - 5 + yy);                          //ячейка, где должен лежать митридат
            this.pointBookmark3 = new Point(155 - 5 + xx, 180 - 5 + yy);                           //третья закладка в спецкармане
            this.pointButtonYesPremium = new Point(470 - 5 + xx, 415 - 5 + yy);                    //третья закладка в спецкармане
            this.pointSecondBookmark = new Point(870 - 5 + xx, 150 - 5 + yy);                      //вторая закладка в кармане

            this.pointWorkCrater = new Point(botwindow.getTriangleX()[0] + xx, botwindow.getTriangleY()[0] + yy);     //бежим на место работы
            this.pointButtonSaveTeleport = new Point(440 - 5 + xx, 570 - 5 + yy);                   // нажимаем на кнопку сохранения телепорта в текущей позиции
            this.pointButtonOkSaveTeleport = new Point(660 - 5 + xx, 645 - 5 + yy);               // нажимаем на кнопку OK для подтверждения сохранения телепорта 

            #endregion

            #region заточка Ида 
            this.pointAcriveInventory = new Point(905 - 5 + xx, 425 - 5 + yy);
            this.pointIsActiveInventory = new PointColor(696 - 5 + xx, 146 - 5 + yy, 16000000, 6);
            this.pointisMoveEquipment1 = new PointColor(493 - 5 + xx, 281 - 5 + yy, 7400000, 5);
            this.pointisMoveEquipment2 = new PointColor(493 - 5 + xx, 282 - 5 + yy, 7400000, 5);
            this.pointButtonEnhance = new Point(525 - 5 + xx, 625 - 5 + yy);
            this.pointIsPlus41 = new PointColor(469 - 5 + xx, 461 - 5 + yy, 15700000, 5);
            this.pointIsPlus42 = new PointColor(470 - 5 + xx, 462 - 5 + yy, 16700000, 5);
            this.pointIsPlus43 = new PointColor(469 - 5 + xx, 489 - 5 + yy, 15700000, 5);
            this.pointIsPlus44 = new PointColor(470 - 5 + xx, 490 - 5 + yy, 16700000, 5);
            this.pointAddShinyCrystall = new Point(472 - 5 + xx, 487 - 5 + yy);                                   //max button
            this.pointIsAddShinyCrystall1 = new PointColor(653 - 5 + xx, 316 - 5 + yy, 15000000, 5);
            this.pointIsAddShinyCrystall2 = new PointColor(654 - 5 + xx, 316 - 5 + yy, 15000000, 5);
            this.pointIsIda1 = new PointColor(487 - 5 + xx, 143 - 5 + yy, 16700000, 5);
            this.pointIsIda2 = new PointColor(487 - 5 + xx, 144 - 5 + yy, 16700000, 5);
            #endregion

            #region чиповка

            this.pointIsEnchant1 = new PointColor(513 - 5 + xx, 189 - 5 + yy, 13000000, 5);
            this.pointIsEnchant2 = new PointColor(514 - 5 + xx, 189 - 5 + yy, 13000000, 5);
            this.pointisWeapon1 = new PointColor(584 - 5 + xx, 365 - 5 + yy, 10700000, 5);
            this.pointisWeapon2 = new PointColor(585 - 5 + xx, 366 - 5 + yy, 10700000, 5);
            this.pointisArmor1 = new PointColor(586 - 5 + xx, 367 - 5 + yy, 6100000, 5);
            this.pointisArmor2 = new PointColor(586 - 5 + xx, 373 - 5 + yy, 6100000, 5);
            this.pointMoveLeftPanelBegin = new Point(161 - 5 + xx, 130 - 5 + yy);
            this.pointMoveLeftPanelEnd = new Point(161 - 5 + xx, 592 - 5 + yy);
            this.pointButtonEnchance = new Point(630 - 5 + xx, 490 - 5 + yy);
            this.pointisDef15 = new PointColor(388 - 5 + xx, 247 - 5 + yy, 12400000, 5);  //проверено
            this.pointisHP1 = new PointColor(355 - 5 + xx, 277 - 5 + yy, 7100000, 5);     //проверено
            this.pointisHP2 = new PointColor(355 - 5 + xx, 292 - 5 + yy, 7100000, 5);     //проверено
            this.pointisHP3 = new PointColor(355 - 5 + xx, 307 - 5 + yy, 7100000, 5);     //проверено
            this.pointisHP4 = new PointColor(355 - 5 + xx, 322 - 5 + yy, 7100000, 5);     //проверено

            this.pointisAtk401 = new PointColor(373 - 5 + xx, 247 - 5 + yy, 13300000, 5);   //проверено
            this.pointisAtk402 = new PointColor(373 - 5 + xx, 256 - 5 + yy, 13700000, 5);   //проверено
            this.pointisSpeed30 = new PointColor(390 - 5 + xx, 269 - 5 + yy, 15500000, 5);   //проверено

            this.pointisAtk391 = new PointColor(378 - 5 + xx, 252 - 5 + yy, 14800000, 5);  //проверено
            this.pointisAtk392 = new PointColor(381 - 5 + xx, 252 - 5 + yy, 13300000, 5);  //проверено
            this.pointisSpeed291 = new PointColor(394 - 5 + xx, 267 - 5 + yy, 14800000, 5);   //проверено
            this.pointisSpeed292 = new PointColor(397 - 5 + xx, 267 - 5 + yy, 13300000, 5);   //проверено

            this.pointisAtk381 = new PointColor(378 - 5 + xx, 251 - 5 + yy, 14600000, 5);   //проверено
            this.pointisAtk382 = new PointColor(381 - 5 + xx, 251 - 5 + yy, 13500000, 5);   //проверено
            this.pointisSpeed281 = new PointColor(394 - 5 + xx, 266 - 5 + yy, 14600000, 5);  //проверено
            this.pointisSpeed282 = new PointColor(397 - 5 + xx, 266 - 5 + yy, 13500000, 5);  //проверено

            this.pointisAtk371 = new PointColor(377 - 5 + xx, 247 - 5 + yy, 14300000, 5);    //проверено
            this.pointisAtk372 = new PointColor(382 - 5 + xx, 247 - 5 + yy, 14600000, 5);    //проверено
            this.pointisSpeed271 = new PointColor(393 - 5 + xx, 262 - 5 + yy, 14300000, 5);  //проверено
            this.pointisSpeed272 = new PointColor(398 - 5 + xx, 262 - 5 + yy, 14600000, 5);  //проверено

            this.pointisWild41 = new PointColor(415 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild42 = new PointColor(415 - 5 + xx, 301 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild51 = new PointColor(415 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild52 = new PointColor(415 - 5 + xx, 316 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild61 = new PointColor(415 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild62 = new PointColor(415 - 5 + xx, 331 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild71 = new PointColor(415 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено
            this.pointisWild72 = new PointColor(415 - 5 + xx, 346 - 5 + yy, 7900000, 5);   //проверено

            this.pointisHuman41 = new PointColor(403 - 5 + xx, 292 - 5 + yy, 7600000, 5);   //проверено
            this.pointisHuman42 = new PointColor(403 - 5 + xx, 301 - 5 + yy, 7700000, 5);   //проверено
            this.pointisHuman51 = new PointColor(403 - 5 + xx, 307 - 5 + yy, 7600000, 5);   //проверено
            this.pointisHuman52 = new PointColor(403 - 5 + xx, 316 - 5 + yy, 7700000, 5);   //проверено
            this.pointisHuman61 = new PointColor(403 - 5 + xx, 322 - 5 + yy, 7600000, 5);   //проверено
            this.pointisHuman62 = new PointColor(403 - 5 + xx, 331 - 5 + yy, 7700000, 5);   //проверено
            this.pointisHuman71 = new PointColor(403 - 5 + xx, 337 - 5 + yy, 7600000, 5);   //проверено
            this.pointisHuman72 = new PointColor(403 - 5 + xx, 346 - 5 + yy, 7700000, 5);   //проверено

            this.pointisDemon41 = new PointColor(398 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon42 = new PointColor(399 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon51 = new PointColor(398 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon52 = new PointColor(399 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon61 = new PointColor(398 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon62 = new PointColor(399 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon71 = new PointColor(398 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено
            this.pointisDemon72 = new PointColor(399 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено

            this.pointisUndead41 = new PointColor(397 - 5 + xx, 293 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead42 = new PointColor(397 - 5 + xx, 294 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead51 = new PointColor(397 - 5 + xx, 308 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead52 = new PointColor(397 - 5 + xx, 309 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead61 = new PointColor(397 - 5 + xx, 323 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead62 = new PointColor(397 - 5 + xx, 324 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead71 = new PointColor(397 - 5 + xx, 338 - 5 + yy, 7400000, 5);   //проверено
            this.pointisUndead72 = new PointColor(397 - 5 + xx, 339 - 5 + yy, 7400000, 5);   //проверено

            this.pointisLifeless41 = new PointColor(398 - 5 + xx, 292 - 5 + yy, 7500000, 5);   //проверено
            this.pointisLifeless42 = new PointColor(398 - 5 + xx, 301 - 5 + yy, 7600000, 5);   //проверено
            this.pointisLifeless51 = new PointColor(398 - 5 + xx, 307 - 5 + yy, 7500000, 5);   //проверено
            this.pointisLifeless52 = new PointColor(398 - 5 + xx, 316 - 5 + yy, 7600000, 5);   //проверено
            this.pointisLifeless61 = new PointColor(398 - 5 + xx, 322 - 5 + yy, 7500000, 5);   //проверено
            this.pointisLifeless62 = new PointColor(398 - 5 + xx, 331 - 5 + yy, 7600000, 5);   //проверено
            this.pointisLifeless71 = new PointColor(398 - 5 + xx, 337 - 5 + yy, 7500000, 5);   //проверено
            this.pointisLifeless72 = new PointColor(398 - 5 + xx, 346 - 5 + yy, 7600000, 5);   //проверено

            #endregion

            #region передача песо торговцу
            this.pointPersonalTrade1 = new PointColor(472 - 5 + xx, 251 - 5 + yy, 12800000, 5);
            this.pointPersonalTrade2 = new PointColor(472 - 5 + xx, 252 - 5 + yy, 12800000, 5);

            this.pointTrader = new Point(472 - 5 + xx, 175 - 5 + yy);
            this.pointPersonalTrade = new Point(536 - 5 + xx, 203 - 5 + yy);
            this.pointMap = new Point(405 - 5 + xx, 220 - 5 + yy);

            this.pointVis1 = new Point(903 - 5 + xx, 151 - 5 + yy);
            this.pointVisMove1 = new Point(701 - 5 + xx, 186 - 5 + yy);
            this.pointVisMove2 = new Point(395 - 5 + xx, 361 - 5 + yy);

            this.pointFood = new Point(361 - 5 + xx, 331 - 5 + yy);     //шаг = 27 пикселей на одну строчку магазина (на случай если добавят новые строчки)
            this.pointButtonFesoBUY = new Point(730 - 5 + xx, 625 - 5 + yy);
            this.pointArrowUp2 = new Point(379 - 5 + xx, 250 - 5 + yy);
            this.pointButtonFesoSell = new Point(730 - 5 + xx, 625 - 5 + yy);
            this.pointBookmarkFesoSell = new Point(245 - 5 + xx, 201 - 5 + yy);
            this.pointDealer = new Point(405 - 5 + xx, 210 - 5 + yy);

            this.pointYesTrade = new Point(1161 - 700 + xx, 595 - 180 + yy);
            this.pointBookmark4 = new Point(903 - 5 + xx, 151 - 5 + yy);
            this.pointFesoBegin = new Point(740 - 5 + xx, 186 - 5 + yy);
            this.pointFesoEnd = new Point(388 - 5 + xx, 343 - 5 + yy);
            this.pointOkSum = new Point(611 - 5 + xx, 397 - 5 + yy);
            this.pointOk = new Point(441 - 5 + xx, 502 - 5 + yy);
            this.pointTrade = new Point(522 - 5 + xx, 502 - 5 + yy);

            #endregion

            #region Undressing in Barack

                this.pointShowEquipment = new Point(145 - 5 + xx, 442 - 5 + yy);
                //this.pointBarack1 = new Point( 80 - 5 + xx, 152 - 5 + yy);
                //this.pointBarack2 = new Point(190 - 5 + xx, 152 - 5 + yy);
                //this.pointBarack3 = new Point( 80 - 5 + xx, 177 - 5 + yy);
                //this.pointBarack4 = new Point(190 - 5 + xx, 177 - 5 + yy);

                this.pointBarack[1] = new Point(80 - 5 + xx, 152 - 5 + yy);
                this.pointBarack[2] = new Point(190 - 5 + xx, 152 - 5 + yy);
                this.pointBarack[3] = new Point( 80 - 5 + xx, 177 - 5 + yy);
                this.pointBarack[4] = new Point(190 - 5 + xx, 177 - 5 + yy);

                this.pointEquipment1 = new PointColor(300 - 5 + xx, 60 - 5 + yy, 12600000, 5);
                this.pointEquipment2 = new PointColor(302 - 5 + xx, 60 - 5 + yy, 12600000, 5);
            #endregion

            #region BH

                this.pointGateInfinityBH = new Point(410 - 5 + xx, 430 - 5 + yy);
                //this.pointGateInfinityBH = new Point(892 - 30 + xx, 573 - 30 + yy);
                this.pointisBH1 = new PointColor(985 - 30 + xx, 91 - 30 + yy, 10353000, 3);             // желтый ободок на миникарте (в BH миникарты нет)
                this.pointisBH3 = new PointColor(963 - 5 + xx, 47 - 5 + yy, 6600000, 5);                // верхняя желтая часть колонны
                this.pointisBH2 = new PointColor(1020 - 5 + xx, 216 - 5 + yy, 5744852, 0);              //

            //int[] aa = new int[16] {1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 2104613};
            //bool ff = aa.Contains(725272);
            //int tt = Array.IndexOf(aa, 725272);

            // this.arrayOfColors = new uint[19] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 1842730, 3955550, 1250584 };
            this.arrayOfColors = new uint[38] { 0, 1644, 725, 6123, 3088, 1715, 1452, 6608, 14190, 1319, 2302, 5275, 2830, 1577, 525, 2635, 1842, 3955, 1250, 5144, 460, 1584,7370, 7304, 2105, 6806, 1711, 15043, 1971, 15306, 2899, 1118, 1713, 5275, 921, 1447, 5074, 5663};
            //                                  0   1*    2*     3    4     5*   6*   7*      8     9     10*   11    12    13   14*   15    16*   17    18    19   20    21   22    23    24    25    26   27     28    29     30    31    32    33    34   35    36    37
            //this.pointIsAtak1 = new PointColor(101 - 30 + xx, 541 - 30 + yy, 4000000, 6);                // проверяем, атакует ли бот босса (есть ли зеленый ободок вокруг сабли)
            //this.pointIsAtak2 = new PointColor(101 - 30 + xx, 542 - 30 + yy, 3000000, 6);
            //this.pointIsAtak3 = new PointColor(101 - 30 + xx, 542 - 30 + yy, 5000000, 6);
            this.pointIsAtak1 = new PointColor(101 - 30 + xx, 541 - 30 + yy, 6000000, 6);                // проверяем, атакует ли бот босса (есть ли зеленый ободок вокруг сабли)
            this.pointIsAtak2 = new PointColor(101 - 30 + xx, 542 - 30 + yy, 6000000, 6);
            this.pointIsRoulette1 = new PointColor(507 - 5 + xx, 83 - 5 + yy, 15000000, 6);                
            this.pointIsRoulette2 = new PointColor(509 - 5 + xx, 83 - 5 + yy, 15000000, 6);

            #endregion

            #region Вход-выход
            this.pointisWhatNews1 = new PointColor(976, 712, 15131615, 0);
            this.pointisWhatNews2 = new PointColor(977, 712, 15131615, 0);
            this.pointisBeginOfMission1 = new PointColor(443 - 5 + xx, 547 - 5 + yy, 12238784, 0);
            this.pointisBeginOfMission2 = new PointColor(443 - 5 + xx, 548 - 5 + yy, 12238784, 0);

            #endregion

            #region Работа с инвентарем и CASH-инвентарем

            //this.pointisOpenInventory1 = new PointColor(731 - 5 + xx, 86 - 5 + yy, 8036794, 0);
            //this.pointisOpenInventory2 = new PointColor(745 - 5 + xx, 86 - 5 + yy, 8036794, 0);
            this.pointisOpenInventory1 = new PointColor(731 - 5 + xx, 87 - 5 + yy, 8549475, 0);
            this.pointisOpenInventory2 = new PointColor(745 - 5 + xx, 87 - 5 + yy, 8549475, 0);


            #endregion

        }

        ///// <summary>
        ///// коструктор с другим аргументом
        ///// </summary>
        ///// <param name="numberOfWindow">номер окна по порядку</param>
        //public ServerSing(int numberOfWindow) : this(new botWindow(numberOfWindow))
        //{
        //}

        //==================================== Методы ===================================================

        #region общие методы 2

        ///// <summary>
        ///// путь к исполняемому файлу игры (сервер сингапур)
        ///// </summary>
        ///// <returns></returns>
        //private String path_Client()
        //{ return File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Singapoore_path.txt"); }

        ///// <summary>
        ///// считываем параметр, отвечающий за то, надо ли загружать окна на сервере сингапур
        ///// </summary>
        ///// <returns></returns>
        //private int SingActive()
        //{ return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Singapoore_active.txt")); }


        #endregion

        #region No window

        ///// <summary>
        ///// Определяет, надо ли грузить данное окно с ботом
        ///// </summary>
        ///// <returns> true означает, что это окно (данный бот) должно быть активно и его надо грузить </returns>
        //public override bool isActive()
        //{
        //    bool result = false;
        //    if (SingActive() == 1) result = true;
        //    return result;
        //}

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
                //вариант 1. ишем окно, открытое в песочнице===========================================================
                HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada [#]");

                ////вариант 2. ищем чистое окно==========================================================================
                //HWND = FindWindow("Granado Espada", "Granado Espada");


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
        /// проверяю белый цвет в загружающемся окне
        /// </summary>
        /// <returns></returns>
        public bool isWhiteWindow()
        {
            iPointColor pointisWhiteWindow = new PointColor(1000, 500, 16700000, 5);            //проверяю белый цвет в загружающемся окне
            return pointisWhiteWindow.isColor();
        }

        /// <summary>
        /// загружен ли данный клиент на другом компе
        /// </summary>
        /// <returns></returns>
        public override bool isContinueRunning()
        {
            return pointisContinueRunning1.isColor() && pointisContinueRunning2.isColor();
        }

        /// <summary>
        /// запуск клиента игры
        /// </summary>
        public override void runClient()
        {
            #region для песочницы

            AccountBusy = false;

            //запускаем steam в песочнице (вариант 1)
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            //steam://rungameid/319730
            process.Start();
            Pause(15000);

            //if (isSteam())         //для старого варианта ввода логина-пароля
            //{
            //    InsertLoginPasswordOk();
            //}

            for (int i = 1; i <= 10; i++)
            {
                CloseWhatNews();

                Pause(1000);

                //new Point(963 - 5 + xx, 717 - 5 + yy).PressMouseL();

                if (isSystemError())  //если выскакивает системная ошибка, то нажимаем "Ок"     проверка не работает
                {
                    OkSystemError();
                }

                if (isNewSteam())
                {
                    pointNewSteamOk.PressMouseL();
                }

                if (isContinueRunning())    //если аккаунт запущен на другом компе
                {
                    NextAccount();
                    AccountBusy = true;
                    break;
                }
            }


            #endregion

            #region для чистого окна Steam

            //AccountBusy = false;

            //Process process = new Process();
            //process.StartInfo.FileName = this.pathClient;
            //process.StartInfo.Arguments = " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            //process.Start();
            //Pause(10000);

            //for (int i = 1; i <= 10; i++)
            //{
            //    Pause(1000);

            //    if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
            //    {
            //        pointNewSteamOk.PressMouseL();
            //    }

            //    if (isContinueRunning())    //если аккаунт запущен на другом компе
            //    {
            //        NextAccount();
            //        AccountBusy = true;
            //        break;
            //    }
            //}

            #endregion

            #region для чистого окна
            //Process.Start(getPathClient());                             //запускаем саму игру или бот Catzmods
            //botwindow.Pause(10000);
            #endregion

            #region если CatzMods
            //Process.Start(getPathClient());                                    //запускаем саму игру или бот Catzmods
            //Pause(10000);
            //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(1110, 705, 1);        //нажимаем кнопку "старт" в боте      
            //Pause(500);
            //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(1222, 705, 1);        //нажимаем кнопку "Close" в боте
            #endregion
        }

        /// <summary>
        /// действия для оранжевой кнопки
        /// </summary>
        public override void OrangeButton()
        {
            //runClientSteam();
            ReOpenWindow();
            Pause(100);
        }



        #endregion

        #region LogOut

        /// <summary>
        /// переключились ли на нужный сервер FERRUCCIO-ESPADA (в режиме логаута)
        /// </summary>
        /// <returns></returns>
        public override bool IsServerSelection()
        {
            return pointIsServerSelection1.isColor() && pointIsServerSelection2.isColor();
        }


        #endregion

        #region at Work

        /// <summary>
        /// пополняем патроны в кармане на 10 000 штук
        /// </summary>
        public override void AddBullet10000()
        {
            pointMana1.PressMouseL();
        }

        /// <summary>
        /// Открываем инвентарь (Alt+V)
        /// </summary>
        private void OpenInventory()
        {
            TopMenu(8, 1);
            Pause(1000);
        }

        /// <summary>
        /// проверяем, находится ли в инвентае 248 вещей 
        /// </summary>
        /// <returns></returns>
        public override bool is248Items()
        {
            bool result = false;
            iPointColor pointis248_1 = new PointColor(684 - 5 + xx, 561 - 5 + yy, 12000000, 6);
            iPointColor pointis248_2 = new PointColor(694 - 5 + xx, 561 - 5 + yy, 12000000, 6);
            iPointColor pointis248_3 = new PointColor(703 - 5 + xx, 559 - 5 + yy, 12000000, 6);

            OpenInventory();

            //проверяем, что в инвентаре 248 вещей
            result = (pointis248_1.isColor()) && (pointis248_2.isColor()) && (pointis248_3.isColor());

            botwindow.PressEscThreeTimes();

            return result;
        }

        #endregion

        #region Top Menu
        
        /// <summary>
        /// нажмает на выбранный раздел верхнего меню 
        /// </summary>
        /// <param name="numberOfThePartitionMenu"> номер раздела верхнего меню </param>
        public override void TopMenu(int numberOfThePartitionMenu)
        {
            //int[] MenukoordX = { 300, 333, 365, 398, 431, 470, 518, 565, 606, 637, 669, 700, 733 };
            //int[] MenukoordX = { 283, 316, 349, 382, 415, 453, 500, 547, 588, 620, 653, 683, 715, 748 };
            int[] MenukoordX = { 305, 339, 371, 402, 435, 475, 522, 570, 610, 642, 675, 705, 738 };
            int x = MenukoordX[numberOfThePartitionMenu - 1];
            int y = 55;
            iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy);

            int count = 0;
            do
            {
                pointMenu.PressMouse();
                botwindow.Pause(2000);
                count++; if (count > 3) break;
            } while (!isOpenTopMenu(numberOfThePartitionMenu));
        }

        /// <summary>
        /// нажать на выбранный раздел верхнего меню, а далее на пункт раскрывшегося списка
        /// </summary>
        /// <param name="numberOfThePartitionMenu">номер раздела верхнего меню п/п</param>
        /// <param name="punkt">пункт меню. номер п/п</param>
        public override void TopMenu(int numberOfThePartitionMenu, int punkt)
        {
            int[] numberOfPunkt = { 0, 8, 4, 2, 0, 3, 2, 6, 9, 0, 0, 0, 0 };  //количество пунктов меню в соответствующем разделе
            //int[] MenukoordX = { 305, 339, 371, 402, 435, 475, 522, 570, 610, 642, 675, 705, 738 };
            int[] PunktX = { 0, 336, 368, 401, 0, 462, 510, 561, 578, 0, 0, 0, 0 };    //координата X пунктов меню
            int[] FirstPunktOfMenuKoordY = { 0, 83, 83, 83, 0, 97, 97, 97, 83, 0, 0, 0, 0 }; //координата Y первого пункта меню

            if (punkt <= numberOfPunkt[numberOfThePartitionMenu - 1])
            {
                //int x = MenukoordX[numberOfThePartitionMenu - 1] + 25;
                int y = FirstPunktOfMenuKoordY[numberOfThePartitionMenu - 1] + 25 * (punkt - 1);
                //iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy);

                TopMenu(numberOfThePartitionMenu);   //сначала открываем раздел верхнего меню (1-14)
                Pause(500);

                int x = PunktX[numberOfThePartitionMenu - 1];
                iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy);
                pointMenu.PressMouseL();  //выбираем конкретный пункт подменю (в раскрывающемся списке)
            }
        }


        /// <summary>
        /// телепортируемся в город продажи по Alt+W (Америка)
        /// </summary>
        public override void TeleportToTownAltW(int nomerTeleport)
        {
            iPoint pointTeleportToTownAltW;
            if (botwindow.getNomerTeleport() < 14)
            {
                pointTeleportToTownAltW = new Point(800 + xx, 500 + yy + (nomerTeleport - 1) * 17);
            }
            else
            {
                pointTeleportToTownAltW = new Point(800 + xx, 500 + yy);   //ребольдо
            }

            // отбегаю в сторону. чтобы бот не стрелял 
            //runAway();                            


            TopMenu(6, 1);
            Pause(1000);
            pointTeleportToTownAltW.PressMouse();           //было два нажатия левой, решил попробовать RRL
            botwindow.Pause(2000);
        }

        /// <summary>
        /// вызываем телепорт через верхнее меню и телепортируемся по указанному номеру телепорта
        /// </summary>
        /// <param name="NumberOfLine"></param>
        public override void Teleport(int NumberOfLine)
        {
            Pause(400);
            TopMenu(12);                     // Click Teleport menu

            Point pointTeleportNumbertLine = new Point(405 - 5 + xx, 180 - 5 + (NumberOfLine - 1) * 15 + yy);    //              тыкаем в указанную строчку телепорта 

            pointTeleportNumbertLine.DoubleClickL();   // Указанная строка в списке телепортов
            Pause(500);

            pointTeleportExecute.PressMouseL();        // Click on button Execute in Teleport menu
            Pause(2000);
        }

        #endregion

        #region заточка

        /// <summary>
        /// переносим (DragAndDrop) одну из частей экипировки на место для заточки
        /// </summary>
        /// <param name="numberOfEquipment">номер экипировки п/п</param>
        public override void MoveToSharpening(int numberOfEquipment)
        {
            pointAcriveInventory.PressMouseR();
            Pause(500);
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
        /// запуск клиента Steam без запуска игры
        /// </summary>
        public override void runClientSteamBH()
        {
            //if (!isLoadedSteamBH && !isLoadedGEBH)   //если в данный момент не грузится другой стим и другие окна ГЭ

            if (!isLoadedSteamBH)   //если в данный момент не грузится другой стим и другие окна ГЭ
            {
                if (!FindWindowSteamBool())
                {
                    #region для песочницы

                    isLoadedSteamBH = true;

                    //запускаем steam в песочнице
                    Process process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
                    process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -silent";
                    process.Start();


                    //for (int i = 1; i <= 5; i++)
                    //{
                    //    Pause(1000);

                    //    //if (isSystemError())  //если выскакивает системная ошибка, то нажимаем "Ок"     проверка не работает
                    //    //{
                    //    //    OkSystemError();
                    //    //}

                    //    if (isNewSteam())
                    //    {
                    //        pointNewSteamOk.PressMouseL();
                    //    }

                    //    if (isContinueRunning())    //если аккаунт запущен на другом компе
                    //    {
                    //        NextAccount();
                    //        AccountBusy = true;
                    //        RemoveSandboxie();
                    //        break;
                    //    }
                    //}
                    #endregion
                }
            }
        }

        /// <summary>
        /// запуск клиента игры для Инфинити
        /// </summary>
        public override void runClientBH()
        {
            if (!isLoadedGEBH)   //если в данный момент не грузится другой стим и другие окна ГЭ
            {
                if (!FindWindowGEforBHBool())
                {
                    #region для песочницы

                    isLoadedGEBH = true;
                    AccountBusy = false;

                    //запускаем steam в песочнице (вариант 1)
                    Process process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
                    process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -applaunch 663090";
                    //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";

                    process.Start();


                    for (int i = 1; i <= 10; i++)
                    {
                        Pause(1000);

                        //if (isSystemError())  //если выскакивает системная ошибка, то нажимаем "Ок"     проверка не работает
                        //{
                        //    OkSystemError();
                        //}

                        if (isNewSteam())
                        {
                            pointNewSteamOk.PressMouseL();
                        }

                        if (isContinueRunning())    //если аккаунт запущен на другом компе
                        {
                            NextAccount();
                            AccountBusy = true;
                            RemoveSandboxie();
                            break;
                        }
                    }

                    #endregion
                }
            }
        }

        ///// <summary>
        ///// поиск новых окон с игрой для кнопки "Найти окна"
        ///// </summary>
        ///// <returns></returns>
        //public override UIntPtr FindWindowGEforBH()
        //{
        //    UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada [#]");

        //    if (HWND != (UIntPtr)0)
        //    {
        //        botParam.Hwnd = HWND;  //если окно найдено, то запись в файл HWND.txt
        //        isLoadedGEBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
        //    }

        //    return HWND;
        //}

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
                isLoadedGEBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
                result = true;  //нашли окно
            }
            return result;
        }

        ///// <summary>
        ///// поиск новых окон Steam
        ///// </summary>
        ///// <returns>номер hwnd найденного Steam</returns>
        //public override UIntPtr FindWindowSteam()
        //{
        //    UIntPtr HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", "Steam");
        //    if (HWND != (UIntPtr)0)
        //    {
        //        isLoadedSteamBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
        //    }
        //    return HWND;
        //}

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
                isLoadedSteamBH = false;     //если нашли загружаемое окно, значит уже можно грузить другие окна
            }
            return result;
        }


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

    }
}

