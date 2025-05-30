﻿using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GEBot.Data;
using System.Runtime.InteropServices.ComTypes;


namespace OpenGEWindows
{
    /// <summary>
    /// Класс описывает процесс перехода к торговле от фарма , начиная от проверки необходимости продажи и заканчивая закрытием окна с ботом (для сервера Сингапур (Avalon))
    /// </summary>
    public class ServerSing : Server
    {
        [DllImport("user32.dll")]
        private static extern UIntPtr FindWindow(String ClassName, String WindowName);  //ищет окно с заданным именем и классом


        //основной конструктор
        public ServerSing(botWindow botwindow)
        {
            //isLoadedGEBH = false;   
            isLoadedSteamBH = false;

            #region общие

            this.botwindow = botwindow;
            numberOfWindow = botwindow.getNumberWindow();
            //this.xx = botwindow.getX();
            //this.yy = botwindow.getY();
            this.globalParam = new GlobalParam();
            ServerParamFactory serverParamFactory = new ServerParamFactory(numberOfWindow);
            this.serverParam = serverParamFactory.create();
            this.botParam = new BotParam(numberOfWindow);
            this.xx = botParam.X;
            this.yy = botParam.Y;
            this.xxx = 5;
            this.yyy = 5;
            this.heroFactory = new HeroFactory(xx, yy);

            #endregion

            #region общие 2

            TownFactory townFactory = new SingTownFactory(botwindow);                                     // здесь выбирается конкретная реализация для фабрики Town
            this.town = townFactory.createTown();
            this.town_begin = new SingTownReboldo(botwindow);   //город взят по умолчанию, как Ребольдо. 
            dialog = new DialogSing(botwindow);

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
            //this.pointisNewSteam1 = new PointColor(1165, 338, 11382191, 0); //для Стима используются абсолютные координаты
            //this.pointisNewSteam2 = new PointColor(1166, 338, 11382191, 0); //для Стима используются абсолютные координаты
            this.pointisNewSteam1 = new PointColor(1052, 832, 15131615, 0); //для Стима используются абсолютные координаты
            this.pointisNewSteam2 = new PointColor(1053, 832, 15131615, 0); //для Стима используются абсолютные координаты
            this.pointisNewSteam3 = new PointColor(1042, 822, 15131615, 0); //для Стима используются абсолютные координаты
            this.pointisNewSteam4 = new PointColor(1043, 822, 15131615, 0); //для Стима используются абсолютные координаты
            this.pointNewSteamOk = new Point(1034, 694);   //кнопка "Соглашаюсь", когда входишь в новый акк
            
            //this.pointisContinueRunning1 = new PointColor(861, 551, 14400000, 5); //для Стима используются абсолютные координаты
            //this.pointisContinueRunning2 = new PointColor(862, 551, 14400000, 5); //для Стима используются абсолютные координаты
            this.pointisContinueRunning1 = new PointColor(1042, 551, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointisContinueRunning2 = new PointColor(1043, 551, 14471368, 0); //для Стима используются абсолютные координаты
            this.pointCancelContinueRunning = new Point(1044, 554); //для Стима используются абсолютные координаты

            this.pointisWhatNews1 = new PointColor(976, 712, 15131615, 0);
            this.pointisWhatNews2 = new PointColor(977, 712, 15131615, 0);

            //this.pointLeaveGame = new Point(613 - 5 + xx, 540 - 5 + yy);    //если окно смещается в левый верхний угол
            this.pointLeaveGame = new Point(1040, 710);                       //если окно не смещается в левый верхний угол, а остаётся посредине экрана

            #endregion

            #region Logout

            //            this.pointConnect = new PointColor(696 - 5 + xx, 148 - 5 + yy, 7800000, 5);
            this.pointConnect = new PointColor(547 - 30 + xx, 441 - 5 + yy, 7800000, 5);
            //this.pointisLogout1 = new PointColor(565 - 5 + xx, 532 - 5 + yy, 16000000, 6);       // проверено   слово Leave Game буква L
            //this.pointisLogout2 = new PointColor(565 - 5 + xx, 531 - 5 + yy, 16000000, 6);       // проверено
            this.pointisLogout1 = new PointColor(935 - 5 + xx, 707 - 5 + yy, 12118521, 0);       // проверено   слово Ver буква r
            this.pointisLogout2 = new PointColor(935 - 5 + xx, 708 - 5 + yy, 12118521, 0);       // проверено
            //выбран ли сервер на экране логаута
            this.pointIsServerSelection1 = new PointColor(430 - 5 + xx, 340 - 5 + yy, 5986903, 0);    // проверено
            this.pointIsServerSelection2 = new PointColor(430 - 5 + xx, 341 - 5 + yy, 5986903, 0);    // проверено
            pointserverSelection = new Point(480 - 5 + xx, 344 - 5 + yy); //синг. первая строка
            //this.pointIsServerSelection1 = new PointColor(430 - 5 + xx, 390 - 5 + yy, 5848111, 0);    // европа. проверено
            //this.pointIsServerSelection2 = new PointColor(430 - 5 + xx, 391 - 5 + yy, 5848111, 0);    // европа. проверено
            //pointserverSelection = new Point(480 - 5 + xx, 394 - 5 + yy); //европа. третья строка

            #endregion

            #region Pet

            this.pointisSummonPet1 = new PointColor(380 - 5 + xx, 88 - 5 + yy, 9400000, 5);
            this.pointisSummonPet2 = new PointColor(390 - 5 + xx, 88 - 5 + yy, 9500000, 5);
            this.pointisActivePet1 = new PointColor(380 - 5 + xx, 88 - 5 + yy, 8000000, 5);
            this.pointisActivePet2 = new PointColor(390 - 5 + xx, 88 - 5 + yy, 8000000, 5);
            //this.pointisActivePet3 = new PointColor(829 - 5 + xx, 186 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц                                      //не проверено
            //this.pointisActivePet4 = new PointColor(829 - 5 + xx, 185 - 5 + yy, 12000000, 6); // для проверки периодической еды на месяц
            this.pointisOpenMenuPet1 = new PointColor(474 - 5 + xx, 219 - 5 + yy, 12000000, 6);      //834 - 5, 98 - 5, 12400000, 835 - 5, 98 - 5, 12400000, 5);             //проверено
            this.pointisOpenMenuPet2 = new PointColor(474 - 5 + xx, 220 - 5 + yy, 12000000, 6);
            this.pointCancelSummonPet = new Point(410 - 5 + xx, 390 - 5 + yy);   //750, 265                    //проверено
            this.pointSummonPet1 = new Point(540 - 5 + xx, 380 - 5 + yy);                   // 868, 258   //Click Pet
            this.pointSummonPet2 = new Point(410 - 5 + xx, 360 - 5 + yy);                   // 748, 238   //Click кнопку "Summon"
            this.pointActivePet = new Point(410 - 5 + xx, 410 - 5 + yy);                   // //Click Button Active Pet                            //проверено

            #endregion

            #region Top Menu

            this.pointisOpenTopMenu21 = new PointColor(337 - 5 + xx + xxx, 76 - 5 + yy + yyy, 13421721, 0);
            this.pointisOpenTopMenu22 = new PointColor(337 - 5 + xx + xxx, 77 - 5 + yy + yyy, 13421721, 0);
            //this.pointisOpenTopMenu51 = new PointColor(442 - 5 + xx, 83 - 5 + yy, 6713000, 3);      //верхняя коричневая полоса
            //this.pointisOpenTopMenu52 = new PointColor(443 - 5 + xx, 83 - 5 + yy, 6713000, 3);     //22-11
            this.pointisOpenTopMenu51 = new PointColor(175 - 5 + xx, 175 - 5 + yy, 12118521, 0);      //буква L в слове Warp List
            this.pointisOpenTopMenu52 = new PointColor(175 - 5 + xx, 176 - 5 + yy, 12118521, 0);     //23-11
            this.pointisOpenTopMenu61 = new PointColor(512 - 5 + xx, 125 - 5 + yy, 6713463, 0);      
            this.pointisOpenTopMenu62 = new PointColor(512 - 5 + xx, 126 - 5 + yy, 6713463, 0);     
            this.pointisOpenTopMenu81 = new PointColor(562 - 5 + xx + xxx, 89 - 5 + yy + yyy, 13421721, 0);
            this.pointisOpenTopMenu82 = new PointColor(562 - 5 + xx + xxx, 90 - 5 + yy + yyy, 13421721, 0);
            this.pointisOpenTopMenu91 = new PointColor(408 - 5 + xx, 308 - 5 + yy, 16316664, 0);      //N в слове Name  23-04-25
            this.pointisOpenTopMenu92 = new PointColor(408 - 5 + xx, 309 - 5 + yy, 16316664, 0);
            this.pointisOpenTopMenu121 = new PointColor(242 - 5 + xx, 166 - 5 + yy, 16700000, 5);       //WorldMap and Zone Map
            this.pointisOpenTopMenu122 = new PointColor(242 - 5 + xx, 167 - 5 + yy, 16700000, 5);        //буква М в слове Zone Map
            this.pointisOpenTopMenu121work = new PointColor(242 - 5 + xx, 145 - 5 + yy, 16700000, 5);       //WorldMap and Zone Map
            this.pointisOpenTopMenu122work = new PointColor(242 - 5 + xx, 146 - 5 + yy, 16700000, 5);        //буква М в слове Zone Map
            this.pointisOpenTopMenu131 = new PointColor(516 - 5 + xx, 269 - 5 + yy, 12000000, 6);          //Quest Name (system menu)                                                        //проверено
            this.pointisOpenTopMenu132 = new PointColor(517 - 5 + xx, 269 - 5 + yy, 12000000, 6);
            this.pointisOpenTopMenu141 = new PointColor(275 - 5 + xx, 291 - 5 + yy, 16700000, 5);        //буква E в слове Expedition Team   
            this.pointisOpenTopMenu142 = new PointColor(275 - 5 + xx, 292 - 5 + yy, 16700000, 5);        //буква E в слове Expedition Team
            //this.pointisOpenTopMenu161 = new PointColor(339 - 5 + xx, 116 - 5 + yy, 15000000, 6);        //буква I (в слове Inventory)                                                        //проверено
            //this.pointisOpenTopMenu162 = new PointColor(339 - 5 + xx, 117 - 5 + yy, 15000000, 6);
            this.pointisOpenTopMenu161 = new PointColor(339 - 5 + xx, 190 - 5 + yy, 16700000, 5);        //буква E (в слове Equip Lumin)                                                        //проверено
            this.pointisOpenTopMenu162 = new PointColor(339 - 5 + xx, 197 - 5 + yy, 16700000, 5);
            this.pointisOpenMenuChooseChannel1 = new PointColor(500 - 5 + xx, 253 - 5 + yy, 8036794, 0);   //Menu of Choose a channel
            this.pointisOpenMenuChooseChannel2 = new PointColor(501 - 5 + xx, 253 - 5 + yy, 8036794, 0);
            this.pointIsCurrentChannel1 = new PointColor(576 - 5 + xx, 288 - 5 + yy, 10000000, 6);          //Channel = 1
            this.pointIsCurrentChannel2 = new PointColor(576 - 5 + xx, 295 - 5 + yy, 11000000, 6);
            //this.pointIsCurrentChannel3 = new PointColor(571 - 5 + xx, 286 - 5 + yy, 11000000, 6);          //Channel = 1
            //this.pointIsCurrentChannel4 = new PointColor(571 - 5 + xx, 287 - 5 + yy, 11000000, 6);

            this.pointGotoEnd = new Point(685 - 5 + xx, 470 - 5 + yy);            //end
            this.pointLogout = new Point(685 - 5 + xx, 440 - 5 + yy);            //логаут
            this.pointGotoBarack = new Point(685 - 5 + xx, 380 - 5 + yy);            //в барак

            this.pointTeleportFirstLine = new Point(400 + xx, 178 + yy);    //              тыкаем в первую строчку телепорта                          //проверено
            //this.pointTeleportSecondLine = new Point(400 + xx, 193 + yy);   //              тыкаем во вторую строчку телепорта                          //проверено
            this.pointTeleportExecute = new Point(114 + xx, 571 + yy);              //22-11               тыкаем в кнопку Operate

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
            this.pointisBoxOverflow3 = new PointColor(385 - 5 + xx, 497 - 5 + yy, 5000000, 6);          //проверка оранжевой надписи
            this.pointisBoxOverflow4 = new PointColor(385 - 5 + xx, 508 - 5 + yy, 5000000, 6);

            //this.arrayOfColorsIsWork1 = new uint[16] { 11051, 1721, 7644, 2764, 16777, 4278, 5138, 3693, 66, 5068, 15824, 8756, 3291, 5400, 2569, 3291 };
            //this.arrayOfColorsIsWork2 = new uint[16] { 10919, 2106, 16711, 7243, 3560, 5401, 9747, 10258, 0, 9350, 15767, 8162, 1910, 3624, 3616, 1910 };
            // ружье, флинт, дробаш, вет дробаш, эксп дробаш, джайна, повар вет, C.Daria, outrange, Sight Shot, Unlimited Shot (эксп пистолет), Миса, и еще 4 непонятно кто

            //this.arrayOfColorsIsWork1 = new uint[7] { 12565, 4094, 4545, 16383, 9371, 8231, 995};
            //this.arrayOfColorsIsWork2 = new uint[7] { 12169, 2850, 2438, 16777, 3562,    0, 1522};   до 22-11-2023
            this.arrayOfColorsIsWork1 = new uint[] { 12565, 4094, 4545, 16383, 9371, 8231, 995, 5603, 7500 };
            this.arrayOfColorsIsWork2 = new uint[] { 12169, 2850, 2438, 16777, 3562, 0, 1522, 5406, 8878 };
                                                 // ружье, флинт, outrange, эксп дробаш, джайна, Миса, M.Lorch, Mary, Банши


            this.pointisKillHero1 = new PointColor( 81 - 5 + xx, 642 - 5 + yy, 2800000, 5);   //22-11
            this.pointisKillHero2 = new PointColor(336 - 5 + xx, 642 - 5 + yy, 2800000, 5);   //23-11
            this.pointisKillHero3 = new PointColor(591 - 5 + xx, 642 - 5 + yy, 2800000, 5);   //23-11
            this.pointisLiveHero1 = new PointColor( 80 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero2 = new PointColor(335 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointisLiveHero3 = new PointColor(590 - 5 + xx, 636 - 5 + yy, 4200000, 5);
            this.pointSkillCook = new Point(183 - 5 + xx, 700 - 5 + yy);
            this.pointisBattleMode1 = new PointColor(177 - 5 + xx, 516 - 5 + yy, 10000000, 7);
            this.pointisBattleMode2 = new PointColor(206 - 5 + xx, 516 - 5 + yy, 10000000, 7);
            //this.pointisAssaultMode1 = new PointColor(76 - 5 + xx, 521 - 5 + yy, 6000000, 6);
            //this.pointisAssaultMode2 = new PointColor(76 - 5 + xx, 522 - 5 + yy, 6000000, 6);
            this.pointisAssaultMode1 = new PointColor(81 - 5 + xx, 516 - 5 + yy, 10000000, 7);
            this.pointisAssaultMode2 = new PointColor(110 - 5 + xx, 516 - 5 + yy, 10000000, 7);


            //this.pointisBattleModeOff1 = new PointColor(173 - 5 + xx, 511 - 5 + yy, 8900000, 5);
            //this.pointisBattleModeOff2 = new PointColor(200 - 5 + xx, 511 - 5 + yy, 8900000, 5);

            //            this.pointisBulletHalf = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5500000, 5);
            this.pointisBulletHalf1 = new PointColor(229 - 5 + xx + xxx, 622 - 5 + yy + yyy, 8245488, 0);
            this.pointisBulletHalf2 = new PointColor(484 - 5 + xx + xxx, 622 - 5 + yy + yyy, 8245488, 0);
            this.pointisBulletHalf3 = new PointColor(739 - 5 + xx + xxx, 622 - 5 + yy + yyy, 8245488, 0);
//            this.pointisBulletOff  = new PointColor(227 - 5 + xx, 621 - 5 + yy, 5700000, 5);
            this.pointisBulletOff1 = new PointColor(229 - 5 + xx + xxx, 622 - 5 + yy + yyy, 401668, 0);
            this.pointisBulletOff2 = new PointColor(484 - 5 + xx + xxx, 622 - 5 + yy + yyy, 401668, 0);
            this.pointisBulletOff3 = new PointColor(739 - 5 + xx + xxx, 622 - 5 + yy + yyy, 401668, 0);

            this.pointProperFightingStance = new Point(115 - 5 + xx, 674 - 5 + yy);    //23-11 проверено
            this.pointisBadFightingStance1 = new PointColor(85 - 5 + xx, 673 - 5 + yy, 16777215, 0);
            this.pointisBadFightingStance2 = new PointColor(86 - 5 + xx, 674 - 5 + yy, 16777215, 0);

            #endregion

            #region inTown

            this.pointisToken1 = new PointColor(478 - 5 + xx, 92 - 5 + yy, 13000000, 5);  //проверяем открыто ли окно с токенами
            this.pointisToken2 = new PointColor(478 - 5 + xx, 93 - 5 + yy, 13000000, 5);
            this.pointToken = new Point(755 - 5 + xx, 94 - 5 + yy);                       //крестик в углу окошка с токенами
            this.pointCure1 = new Point(215 - 5 + xx, 705 - 5 + yy);                        //бутылка лечения под буквой U
            this.pointCure2 = new Point(215 - 5 + 255 + xx, 705 - 5 + yy);                  //бутылка лечения под буквой J
            this.pointCure3 = new Point(215 - 5 + 255 * 2 + xx, 705 - 5 + yy);              //бутылка лечения под буквой M
            this.pointMana1 = new Point(250 - 5 + xx, 705 - 5 + yy);                        //бутылка маны под буквой I  
            this.pointMana2 = new Point(250 - 5 + 255 + xx, 705 - 5 + yy);                  //бутылка маны под буквой K  
            this.pointMana3 = new Point(250 - 5 + 510 + xx, 705 - 5 + yy);                  //бутылка маны под буквой ,  
            this.pointGM = new Point(439 - 5 + xx, 413 - 5 + yy);
//            this.pointHeadGM = new Point(369 - 5 + xx, 290 - 5 + yy);
            this.pointHeadGM = new Point(394 - 5 + xx, 394 - 5 + yy);
            //this.arrayOfColorsIsTown1 = new uint[16] { 11053, 1710, 7631, 2763, 16777, 4276, 5131, 3684, 65, 5066, 15856, 8750,         3291, 5395, 3291, 2565 };
            //this.arrayOfColorsIsTown2 = new uint[16] { 10921, 2105, 16711, 7237, 3552, 5395, 9737, 10263, 0, 9342, 15790, 8158, 1910, 3618, 1910, 3618 };
            // ружье, флинт, дробаш, вет дробаш, эксп дробаш, джайна, повар вет, C.Daria, outrange, Sight Shot, Unlimited Shot (эксп пистолет), Миса, и еще 4 непонятно кто

            //this.arrayOfColorsIsTown1 = new uint[7] { 12566, 4079, 4539, 16382, 9342, 8224, 986 };
            //this.arrayOfColorsIsTown2 = new uint[7] { 12171, 2829, 2434, 16777, 3552,    0, 1513 };   //до 22-11-2023 было норм
            this.arrayOfColorsIsTown1 = new uint[9] { 8092, 5921, 0, 16777, 0, 0, 0, 10132, 592 };
            this.arrayOfColorsIsTown2 = new uint[9] { 7895, 1907, 0, 6118, 0, 0, 0, 7566, 0 };
            // ружье, флинт, outrange, эксп дробаш, джайна, Миса, M.Lorch, Mary, Банши

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

            this.sdvigY = 0;   //это значит, что в Ребольдо три канала. Если было бы два, то sdvigY = 13
            this.pointMoveNow = new Point(651 - 5 + xx, 591 - 5 + yy);                              //выбор канала 
            this.pointisBarack1 = new PointColor(53 - 5 + xx, 154 - 5 + yy, 2400000, 5);            //зеленый цвет в слове Barracks  
            this.pointisBarack2 = new PointColor(53 - 5 + xx, 155 - 5 + yy, 2400000, 5);            //проверено
            //this.pointisBarack3 = new PointColor(67 - 5 + xx, 62 - 5 + yy, 15500000, 5);            //проверено   Barrack Mode буква "B"
            //this.pointisBarack4 = new PointColor(67 - 5 + xx, 63 - 5 + yy, 15500000, 5);            //проверено
            this.pointisBarack3 = new PointColor(41 - 5 + xx, 62 - 5 + yy, 15500000, 5);            //проверено   Barrack Mode буква "B"
            this.pointisBarack4 = new PointColor(41 - 5 + xx, 63 - 5 + yy, 15500000, 5);            //проверено
            this.pointisBarack5 = new PointColor(907 - 5 + xx, 670 - 5 + yy, 11723248, 0);            //страница создания нового персонажа  22-11
            this.pointisBarack6 = new PointColor(907 - 5 + xx, 671 - 5 + yy, 11723248, 0);            //буква Т в слове "To Barack"
            this.pointToBarack = new Point(945 - 5 + xx, 675 - 5 + yy);                       // кнопка "To Barack" на странице создания перса 22-11

            this.pointisBarackTeamSelection1 = new PointColor(23 - 5 + xx, 68 - 5 + yy, 11000000, 6);            //Team Member буква T
            this.pointisBarackTeamSelection2 = new PointColor(23 - 5 + xx, 69 - 5 + yy, 11000000, 6);            // 22-11
            // кнопка вызова списка групп в бараке "View Character List"
            this.pointTeamSelection1 = new Point(140 - 5 + xx, 608 - 5 + yy);                   //23.04.2025
            this.pointTeamSelection2 = new Point(70 - 5 + xx, 355 - 5 + yy);                   //проверено
            this.pointTeamSelection3 = new Point(48 - 5 + xx, 623 - 5 + yy);                   //проверено
            this.pointButtonLogoutFromBarack = new Point(790 - 5 + xx + xxx, 705 - 5 + yy + yyy);               //кнопка логаут в казарме
            //this.pointChooseChannel = new Point(820 - 5 + xx, 382 - 5 + yy);                       // нажатие кнопки Choose a channel
            this.pointEnterChannel = new Point(646 - 5 + xx, 409 - 5 + yy + (botwindow.getKanal() - 2) * 15);    //выбор канала 
            this.pointNewPlace = new Point(85 + xx, 670 + yy);
            this.pointLastPoint = new Point(210 - 5 + xx, 670 - 5 + yy);
            this.pointisBHLastPoint1 = new PointColor(141 - 5 + xx, 503 - 5 + yy, 11000000, 6);
            this.pointisBHLastPoint2 = new PointColor(141 - 5 + xx, 504 - 5 + yy, 11000000, 6);
            this.pointCreateButton = new Point(447 - 5 + xx, 700 - 5 + yy);

            #endregion

            #region  новые боты

           

            this.pointNewName = new Point(490 - 5 + xx, 280 - 5 + yy);                             //строчка, куда надо вводить имя семьи
            this.pointButtonCreateNewName = new Point(465 - 5 + xx, 510 - 5 + yy);                 //кнопка Create для создания новой семьи

            this.pointCreateHeroes = new Point(800 - 5 + xx + xxx, 655 - 5 + yy + yyy);                        //кнопка Create для создания нового героя (перса)
            this.pointButtonOkCreateHeroes = new Point(520 - 5 + xx + xxx, 420 - 5 + yy + yyy);                //кнопка Ok для подтверждения создания героя
            this.pointMenuSelectTypeHeroes = new Point(810 - 5 + xx + xxx, 260 - 5 + yy + yyy);                //меню выбора типа героя в казарме
            this.pointSelectTypeHeroes = new Point(800 - 5 + xx + xxx, 320 - 5 + yy + yyy);                    //выбор мушкетера в меню типо героев в казарме
            this.pointNameOfHeroes = new Point(800 - 5 + xx + xxx, 180 - 5 + yy + yyy);                        //нажимаем на строчку, где вводится имя героя (перса)

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
            this.pointDomingoOnMap = new Point(820 - 5 + xx, 115 - 5 + yy);                        //нажимаем на Доминго на карте Alt+Z
            this.pointPressDomingo = new Point(508 - 5 + xx, 404 - 5 + yy);                        //нажимаем на Доминго
            this.pointFirstStringDialog = new Point(520 - 5 + xx, 660 - 5 + yy);                   //нажимаем Yes в диалоге Доминго (нижняя строчка)
            this.pointSecondStringDialog = new Point(520 - 5 + xx, 640 - 5 + yy);                  //нажимаем Yes в диалоге Доминго второй раз (вторая строчка снизу)
            this.pointDomingoMiss = new Point(396 - 5 + xx, 206 - 5 + yy);                         //нажимаем правой кнопкой по карте миссии Доминго
            this.pointPressDomingo2 = new Point(572 - 5 + xx, 237 - 5 + yy);                       //нажимаем на Доминго после миссии
//            this.pointLindonOnMap = new Point(820 - 5 + xx, 430 - 5 + yy);                         //нажимаем на Линдона на карте Alt+Z
            this.pointLindonOnMap = new Point(523 - 5 + xx, 405 - 5 + yy);                         //нажимаем на Линдона на карте Alt+Z
            this.pointPressLindon2 = new Point(627 - 5 + xx, 274 - 5 + yy);                        //нажимаем на Линдона
            this.pointPetExpert = new Point(818 - 5 + xx, 428 - 5 + yy);                           //нажимаем на петэксперта
            this.pointPetExpert2 = new Point(816 - 5 + xx, 415 - 5 + yy);                          //нажимаем на петэксперта второй раз 
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
            this.pointGateCrater = new Point(373 - 5 + xx, 605 - 5 + yy);                          //переход (ворота) из лавового плато в кратер
            this.pointMitridat = new Point(800 - 5 + xx, 180 - 5 + yy);                            //митридат в кармане
            this.pointMitridatTo2 = new Point(30 - 5 + xx, 140 - 5 + yy);                          //ячейка, где должен лежать митридат
            this.pointBookmark3 = new Point(155 - 5 + xx, 180 - 5 + yy);                           //третья закладка в спецкармане
            this.pointButtonYesPremium = new Point(470 - 5 + xx, 415 - 5 + yy);                    //третья закладка в спецкармане
            this.pointSecondBookmark = new Point(870 - 5 + xx, 150 - 5 + yy);                      //вторая закладка в кармане

            this.pointWorkCrater = new Point(botwindow.getTriangleX()[0] + xx, botwindow.getTriangleY()[0] + yy);     //бежим на место работы
            this.pointButtonSaveTeleport = new Point(440 - 5 + xx, 570 - 5 + yy);                   // нажимаем на кнопку сохранения телепорта в текущей позиции
            this.pointButtonOkSaveTeleport = new Point(660 - 5 + xx, 645 - 5 + yy);               // нажимаем на кнопку OK для подтверждения сохранения телепорта 
            this.pointPetBegin = new Point(895 - 5 + xx, 180 - 5 + yy);    // коробка с петом лежит здесь
            this.pointPetEnd = new Point(520 - 5 + xx, 330 - 5 + yy);    // коробка с петом перетаскиваем сюда

            #endregion

            #region заточка Ида 
//            this.pointAcriveInventory = new Point(905 - 5 + xx, 425 - 5 + yy);
            this.pointAcriveInventory = new Point(910 - 5 + xx, 430 - 5 + yy);
            //this.pointIsActiveInventory = new PointColor(696 - 5 + xx, 146 - 5 + yy, 16000000, 6);
            this.pointIsActiveInventory = new PointColor(701 - 5 + xx, 151 - 5 + yy, 16000000, 6);

            //this.pointisMoveEquipment1 = new PointColor(493 - 5 + xx, 281 - 5 + yy, 7400000, 5);
            //this.pointisMoveEquipment2 = new PointColor(493 - 5 + xx, 282 - 5 + yy, 7400000, 5);
            this.pointisMoveEquipment1 = new PointColor(354 - 5 + xx, 305 - 5 + yy, 14900000, 5);
            this.pointisMoveEquipment2 = new PointColor(355 - 5 + xx, 305 - 5 + yy, 14900000, 5);

//            this.pointButtonEnhance = new Point(525 - 5 + xx, 625 - 5 + yy);
            this.pointButtonEnhance = new Point(530 - 5 + xx, 630 - 5 + yy);

            //this.pointIsPlus41 = new PointColor(469 - 5 + xx, 461 - 5 + yy, 15700000, 5);
            //this.pointIsPlus42 = new PointColor(470 - 5 + xx, 462 - 5 + yy, 16700000, 5);
            //this.pointIsPlus43 = new PointColor(469 - 5 + xx, 489 - 5 + yy, 15700000, 5);
            //this.pointIsPlus44 = new PointColor(470 - 5 + xx, 490 - 5 + yy, 16700000, 5);
            this.pointIsPlus41 = new PointColor(474 - 5 + xx, 466 - 5 + yy, 15700000, 5);
            this.pointIsPlus42 = new PointColor(475 - 5 + xx, 467 - 5 + yy, 16700000, 5);
            this.pointIsPlus43 = new PointColor(474 - 5 + xx, 494 - 5 + yy, 15700000, 5);
            this.pointIsPlus44 = new PointColor(475 - 5 + xx, 495 - 5 + yy, 16700000, 5);

//            this.pointAddShinyCrystall = new Point(472 - 5 + xx, 487 - 5 + yy);                                   //max button
            this.pointAddShinyCrystall = new Point(477 - 5 + xx, 506 - 5 + yy);                                   //max button (когда точим оружие)
            this.pointAddShinyCrystall2 = new Point(477 - 5 + xx, 478 - 5 + yy);                                  //max button (когда точим броню)

            //this.pointIsAddShinyCrystall1 = new PointColor(653 - 5 + xx, 316 - 5 + yy, 15000000, 5);
            //this.pointIsAddShinyCrystall2 = new PointColor(654 - 5 + xx, 316 - 5 + yy, 15000000, 5);
            this.pointIsAddShinyCrystall1 = new PointColor(658 - 5 + xx, 321 - 5 + yy, 15000000, 5);
            this.pointIsAddShinyCrystall2 = new PointColor(659 - 5 + xx, 321 - 5 + yy, 15000000, 5);

            //this.pointIsIda1 = new PointColor(487 - 5 + xx, 143 - 5 + yy, 16700000, 5);
            //this.pointIsIda2 = new PointColor(487 - 5 + xx, 144 - 5 + yy, 16700000, 5);
            this.pointIsIda1 = new PointColor(492 - 5 + xx, 148 - 5 + yy, 16700000, 5);
            this.pointIsIda2 = new PointColor(492 - 5 + xx, 149 - 5 + yy, 16700000, 5);
            #endregion

            #region чиповка

            //this.pointIsEnchant1 = new PointColor(513 - 5 + xx, 189 - 5 + yy, 13000000, 5);
            //this.pointIsEnchant2 = new PointColor(514 - 5 + xx, 189 - 5 + yy, 13000000, 5);
            //this.pointisWeapon1 = new PointColor(584 - 5 + xx, 365 - 5 + yy, 10700000, 5);
            //this.pointisWeapon2 = new PointColor(585 - 5 + xx, 366 - 5 + yy, 10700000, 5);
            //this.pointisArmor1 = new PointColor(586 - 5 + xx, 367 - 5 + yy, 6100000, 5);
            //this.pointisArmor2 = new PointColor(586 - 5 + xx, 373 - 5 + yy, 6100000, 5);
            //this.pointMoveLeftPanelBegin = new Point(161 - 5 + xx, 130 - 5 + yy);
            //this.pointMoveLeftPanelEnd = new Point(161 - 5 + xx, 592 - 5 + yy);
            //this.pointButtonEnchance = new Point(630 - 5 + xx, 490 - 5 + yy);
            //this.pointisDef15 = new PointColor(388 - 5 + xx, 247 - 5 + yy, 12400000, 5);  //проверено
            //this.pointisHP1 = new PointColor(355 - 5 + xx, 277 - 5 + yy, 7100000, 5);     //проверено
            //this.pointisHP2 = new PointColor(355 - 5 + xx, 292 - 5 + yy, 7100000, 5);     //проверено
            //this.pointisHP3 = new PointColor(355 - 5 + xx, 307 - 5 + yy, 7100000, 5);     //проверено
            //this.pointisHP4 = new PointColor(355 - 5 + xx, 322 - 5 + yy, 7100000, 5);     //проверено

            //this.pointisAtk401 = new PointColor(373 - 5 + xx, 247 - 5 + yy, 13300000, 5);   //проверено
            //this.pointisAtk402 = new PointColor(373 - 5 + xx, 256 - 5 + yy, 13700000, 5);   //проверено
            //this.pointisSpeed30 = new PointColor(390 - 5 + xx, 269 - 5 + yy, 15500000, 5);   //проверено

            //this.pointisAtk391 = new PointColor(378 - 5 + xx, 252 - 5 + yy, 14800000, 5);  //проверено
            //this.pointisAtk392 = new PointColor(381 - 5 + xx, 252 - 5 + yy, 13300000, 5);  //проверено
            //this.pointisSpeed291 = new PointColor(394 - 5 + xx, 267 - 5 + yy, 14800000, 5);   //проверено
            //this.pointisSpeed292 = new PointColor(397 - 5 + xx, 267 - 5 + yy, 13300000, 5);   //проверено

            //this.pointisAtk381 = new PointColor(378 - 5 + xx, 251 - 5 + yy, 14600000, 5);   //проверено
            //this.pointisAtk382 = new PointColor(381 - 5 + xx, 251 - 5 + yy, 13500000, 5);   //проверено
            //this.pointisSpeed281 = new PointColor(394 - 5 + xx, 266 - 5 + yy, 14600000, 5);  //проверено
            //this.pointisSpeed282 = new PointColor(397 - 5 + xx, 266 - 5 + yy, 13500000, 5);  //проверено

            //this.pointisAtk371 = new PointColor(377 - 5 + xx, 247 - 5 + yy, 14300000, 5);    //проверено
            //this.pointisAtk372 = new PointColor(382 - 5 + xx, 247 - 5 + yy, 14600000, 5);    //проверено
            //this.pointisSpeed271 = new PointColor(393 - 5 + xx, 262 - 5 + yy, 14300000, 5);  //проверено
            //this.pointisSpeed272 = new PointColor(398 - 5 + xx, 262 - 5 + yy, 14600000, 5);  //проверено

            //this.pointisWild41 = new PointColor(415 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild42 = new PointColor(415 - 5 + xx, 301 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild51 = new PointColor(415 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild52 = new PointColor(415 - 5 + xx, 316 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild61 = new PointColor(415 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild62 = new PointColor(415 - 5 + xx, 331 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild71 = new PointColor(415 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisWild72 = new PointColor(415 - 5 + xx, 346 - 5 + yy, 7900000, 5);   //проверено

            //this.pointisHuman41 = new PointColor(403 - 5 + xx, 292 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisHuman42 = new PointColor(403 - 5 + xx, 301 - 5 + yy, 7700000, 5);   //проверено
            //this.pointisHuman51 = new PointColor(403 - 5 + xx, 307 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisHuman52 = new PointColor(403 - 5 + xx, 316 - 5 + yy, 7700000, 5);   //проверено
            //this.pointisHuman61 = new PointColor(403 - 5 + xx, 322 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisHuman62 = new PointColor(403 - 5 + xx, 331 - 5 + yy, 7700000, 5);   //проверено
            //this.pointisHuman71 = new PointColor(403 - 5 + xx, 337 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisHuman72 = new PointColor(403 - 5 + xx, 346 - 5 + yy, 7700000, 5);   //проверено

            //this.pointisDemon41 = new PointColor(398 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon42 = new PointColor(399 - 5 + xx, 292 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon51 = new PointColor(398 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon52 = new PointColor(399 - 5 + xx, 307 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon61 = new PointColor(398 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon62 = new PointColor(399 - 5 + xx, 322 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon71 = new PointColor(398 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено
            //this.pointisDemon72 = new PointColor(399 - 5 + xx, 337 - 5 + yy, 7900000, 5);   //проверено

            //this.pointisUndead41 = new PointColor(397 - 5 + xx, 293 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead42 = new PointColor(397 - 5 + xx, 294 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead51 = new PointColor(397 - 5 + xx, 308 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead52 = new PointColor(397 - 5 + xx, 309 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead61 = new PointColor(397 - 5 + xx, 323 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead62 = new PointColor(397 - 5 + xx, 324 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead71 = new PointColor(397 - 5 + xx, 338 - 5 + yy, 7400000, 5);   //проверено
            //this.pointisUndead72 = new PointColor(397 - 5 + xx, 339 - 5 + yy, 7400000, 5);   //проверено

            //this.pointisLifeless41 = new PointColor(398 - 5 + xx, 292 - 5 + yy, 7500000, 5);   //проверено
            //this.pointisLifeless42 = new PointColor(398 - 5 + xx, 301 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisLifeless51 = new PointColor(398 - 5 + xx, 307 - 5 + yy, 7500000, 5);   //проверено
            //this.pointisLifeless52 = new PointColor(398 - 5 + xx, 316 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisLifeless61 = new PointColor(398 - 5 + xx, 322 - 5 + yy, 7500000, 5);   //проверено
            //this.pointisLifeless62 = new PointColor(398 - 5 + xx, 331 - 5 + yy, 7600000, 5);   //проверено
            //this.pointisLifeless71 = new PointColor(398 - 5 + xx, 337 - 5 + yy, 7500000, 5);   //проверено
            //this.pointisLifeless72 = new PointColor(398 - 5 + xx, 346 - 5 + yy, 7600000, 5);   //проверено


            this.pointIsEnchant1 = new PointColor(513 - 5 + xx, 186 - 5 + yy, 8000000, 5);
            this.pointIsEnchant2 = new PointColor(514 - 5 + xx, 186 - 5 + yy, 8000000, 5);
            this.pointisWeapon1 = new PointColor(584 - 5 + xx+xxx, 365 - 5 + yy + yyy, 10700000, 5);
            this.pointisWeapon2 = new PointColor(585 - 5 + xx+xxx, 366 - 5 + yy + yyy, 10700000, 5);
            this.pointisArmor1 = new PointColor(586 - 5 + xx+xxx, 367 - 5 + yy + yyy, 6100000, 5);
            this.pointisArmor2 = new PointColor(586 - 5 + xx+xxx, 373 - 5 + yy + yyy, 6100000, 5);
            this.pointMoveLeftPanelBegin = new Point(161 - 5 + xx+xxx, 130 - 5 + yy + yyy);
            this.pointMoveLeftPanelEnd = new Point(161 - 5 + xx+xxx, 592 - 5 + yy + yyy);
            this.pointButtonEnchance = new Point(630 - 5 + xx+xxx, 490 - 5 + yy + yyy);
            this.pointisDef15 = new PointColor(388 - 5 + xx+xxx, 247 - 5 + yy + yyy, 5200000, 5);  //проверено
            this.pointisHP1 = new PointColor(355 - 5 + xx+xxx, 277 - 5 + yy + yyy, 7100000, 5);     //проверено
            this.pointisHP2 = new PointColor(355 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7100000, 5);     //проверено
            this.pointisHP3 = new PointColor(355 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7100000, 5);     //проверено
            this.pointisHP4 = new PointColor(355 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7100000, 5);     //проверено

            this.pointisAtk401 = new PointColor(373 - 5 + xx+xxx, 247 - 5 + yy + yyy, 13900000, 5);     //сделано
            this.pointisAtk402 = new PointColor(373 - 5 + xx+xxx, 256 - 5 + yy + yyy, 13700000, 5);     //сделано
            this.pointisAtk391 = new PointColor(378 - 5 + xx+xxx, 252 - 5 + yy + yyy, 14700000, 5);     //сделано
            this.pointisAtk392 = new PointColor(381 - 5 + xx+xxx, 252 - 5 + yy + yyy,  9100000, 5);     //сделано
            this.pointisAtk381 = new PointColor(378 - 5 + xx+xxx, 251 - 5 + yy + yyy, 13500000, 5);     //сделано
            this.pointisAtk382 = new PointColor(381 - 5 + xx+xxx, 251 - 5 + yy + yyy, 13700000, 5);     //сделано
            this.pointisAtk371 = new PointColor(377 - 5 + xx+xxx, 247 - 5 + yy + yyy, 6700000, 5);      //сделано
            this.pointisAtk372 = new PointColor(382 - 5 + xx+xxx, 247 - 5 + yy + yyy, 14300000, 5);     //сделано

            this.pointisSpeed30 = new PointColor(390 - 5 + xx + xxx, 269 - 5 + yy + yyy, 15300000, 5);  //сделано
            this.pointisSpeed291 = new PointColor(394 - 5 + xx + xxx, 267 - 5 + yy + yyy, 14700000, 5); //сделано
            this.pointisSpeed292 = new PointColor(397 - 5 + xx + xxx, 267 - 5 + yy + yyy,  9100000, 5); //сделано
            this.pointisSpeed281 = new PointColor(394 - 5 + xx + xxx, 266 - 5 + yy + yyy, 13500000, 5); //сделано
            this.pointisSpeed282 = new PointColor(397 - 5 + xx + xxx, 266 - 5 + yy + yyy, 13700000, 5); //сделано
            this.pointisSpeed271 = new PointColor(393 - 5 + xx+xxx, 262 - 5 + yy + yyy, 6700000, 5);    //сделано
            this.pointisSpeed272 = new PointColor(398 - 5 + xx+xxx, 262 - 5 + yy + yyy, 14300000, 5);   //сделано

            this.pointisWild41 = new PointColor(415 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild42 = new PointColor(415 - 5 + xx+xxx, 301 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild51 = new PointColor(415 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild52 = new PointColor(415 - 5 + xx+xxx, 316 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild61 = new PointColor(415 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild62 = new PointColor(415 - 5 + xx+xxx, 331 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild71 = new PointColor(415 - 5 + xx+xxx, 337 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisWild72 = new PointColor(415 - 5 + xx+xxx, 346 - 5 + yy + yyy, 7900000, 5);   //проверено

            this.pointisHuman41 = new PointColor(403 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisHuman42 = new PointColor(403 - 5 + xx+xxx, 301 - 5 + yy + yyy, 7700000, 5);   //проверено
            this.pointisHuman51 = new PointColor(403 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisHuman52 = new PointColor(403 - 5 + xx+xxx, 316 - 5 + yy + yyy, 7700000, 5);   //проверено
            this.pointisHuman61 = new PointColor(403 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisHuman62 = new PointColor(403 - 5 + xx+xxx, 331 - 5 + yy + yyy, 7700000, 5);   //проверено
            this.pointisHuman71 = new PointColor(403 - 5 + xx+xxx, 337 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisHuman72 = new PointColor(403 - 5 + xx+xxx, 346 - 5 + yy + yyy, 7700000, 5);   //проверено

            this.pointisDemon41 = new PointColor(398 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon42 = new PointColor(399 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon51 = new PointColor(398 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon52 = new PointColor(399 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon61 = new PointColor(398 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon62 = new PointColor(399 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon71 = new PointColor(398 - 5 + xx+xxx, 337 - 5 + yy + yyy, 7900000, 5);   //проверено
            this.pointisDemon72 = new PointColor(399 - 5 + xx+xxx, 337 - 5 + yy + yyy, 7900000, 5);   //проверено

            this.pointisUndead41 = new PointColor(397 - 5 + xx+xxx, 293 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead42 = new PointColor(397 - 5 + xx+xxx, 294 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead51 = new PointColor(397 - 5 + xx+xxx, 308 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead52 = new PointColor(397 - 5 + xx+xxx, 309 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead61 = new PointColor(397 - 5 + xx+xxx, 323 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead62 = new PointColor(397 - 5 + xx+xxx, 324 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead71 = new PointColor(397 - 5 + xx+xxx, 338 - 5 + yy + yyy, 7400000, 5);   //проверено
            this.pointisUndead72 = new PointColor(397 - 5 + xx+xxx, 339 - 5 + yy + yyy, 7400000, 5);   //проверено

            this.pointisLifeless41 = new PointColor(398 - 5 + xx+xxx, 292 - 5 + yy + yyy, 7500000, 5);   //проверено
            this.pointisLifeless42 = new PointColor(398 - 5 + xx+xxx, 301 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisLifeless51 = new PointColor(398 - 5 + xx+xxx, 307 - 5 + yy + yyy, 7500000, 5);   //проверено
            this.pointisLifeless52 = new PointColor(398 - 5 + xx+xxx, 316 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisLifeless61 = new PointColor(398 - 5 + xx+xxx, 322 - 5 + yy + yyy, 7500000, 5);   //проверено
            this.pointisLifeless62 = new PointColor(398 - 5 + xx+xxx, 331 - 5 + yy + yyy, 7600000, 5);   //проверено
            this.pointisLifeless71 = new PointColor(398 - 5 + xx+xxx, 337 - 5 + yy + yyy, 7500000, 5);   //проверено
            this.pointisLifeless72 = new PointColor(398 - 5 + xx+xxx, 346 - 5 + yy + yyy, 7600000, 5);   //проверено
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

            this.pointGateInfinityBH = new Point(410 - 5 + xx + xxx, 430 - 5 + yy + yyy);                       //тыкаем в ворота Инфинити
            //this.pointGateInfinityBH = new Point(892 - 30 + xx + xxx, 573 - 30 + yy + yyy);
            this.pointisBH1 = new PointColor(991 - 5 + xx, 199 - 5 + yy, 10420000, 4);             // желтый ободок на миникарте (в BH миникарты нет)  //*
            this.pointisBH3 = new PointColor(963 - 5 + xx + xxx, 47 - 5 + yy + yyy, 6600000, 5);                // верхняя желтая часть колонны
            this.pointisBH2 = new PointColor(1020 - 5 + xx + xxx, 216 - 5 + yy + yyy, 5744852, 0);              //

            // this.arrayOfColors = new uint[19] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 1842730, 3955550, 1250584 };
            this.arrayOfColors = new uint[38] { 0, 1644, 725, 6123, 3088, 1715, 1452, 6608, 14190, 1319, 2302, 5275, 2830, 1577, 525, 2635, 1842, 3955, 1250, 5144, 460, 1584,7370, 7304, 2105, 6806, 1711, 15043, 1971, 15306, 2899, 1118, 1713, 5275, 921, 1447, 5074, 5663};
            //                                  0   1*    2*     3    4     5*   6*   7*      8     9     10*   11    12    13   14*   15    16*   17    18    19   20    21   22    23    24    25    26   27     28    29     30    31    32    33    34   35    36    37
            this.pointIsAtak1 = new PointColor(101 - 30 + xx + xxx, 541 - 30 + yy + yyy, 6000000, 6);                // проверяем, атакует ли бот босса (есть ли зеленый ободок вокруг сабли)
            this.pointIsAtak2 = new PointColor(101 - 30 + xx + xxx, 542 - 30 + yy + yyy, 6000000, 6);
            this.pointIsRoulette1 = new PointColor(507 - 5 + xx + xxx, 83 - 5 + yy + yyy, 15000000, 6);                
            this.pointIsRoulette2 = new PointColor(509 - 5 + xx + xxx, 83 - 5 + yy + yyy, 15000000, 6);

            #endregion

            #region Вход-выход
                this.pointisBeginOfMission1 = new PointColor(443 - 5 + xx, 547 - 5 + yy, 12238784, 0);
                this.pointisBeginOfMission2 = new PointColor(443 - 5 + xx, 548 - 5 + yy, 12238784, 0);
            #endregion

            #region Работа с инвентарем и CASH-инвентарем

                //this.pointisOpenInventory1 = new PointColor(731 - 5 + xx, 86 - 5 + yy, 8036794, 0);
                //this.pointisOpenInventory2 = new PointColor(745 - 5 + xx, 86 - 5 + yy, 8036794, 0);
            this.pointisOpenInventory1 = new PointColor(731 - 5 + xx, 87 - 5 + yy, 8549475, 0);
            this.pointisOpenInventory2 = new PointColor(745 - 5 + xx, 87 - 5 + yy, 8549475, 0);

            #endregion

            #region All in One

            //this.Hero = new int[4] { 0, 0, 0, 0 };
            this.DirectionOfMovement = 1;
            this.NeedToPickUpFeso = false;
            this.NeedToPickUpRight = false;
            this.NeedToPickUpLeft = false;
            this.NextPointNumber = 0;
            this.Counter = 3;
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

        /// <summary>
        /// поиск окна с ошибкой
        /// </summary>
        /// <returns>HWND найденного окна</returns>
        public override UIntPtr FindWindowError()
        {
            UIntPtr HWND = (UIntPtr)0;

            //HWND = FindWindow("Granado Espada", "Granado Espada");
            HWND = FindWindow("Sandbox:1:Ошибка", "#32770 (Диалоговое окно)");


            //SetWindowPos(HWND, 0, 5, 5, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);

            //Pause(500);

            return HWND;
        }


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
        /// поиск нового окна с игрой 
        /// чистое окно без песочницы
        /// </summary>
        /// <returns>HWND найденного окна</returns>
        public override UIntPtr FindWindowGE_CW()
        {
            UIntPtr HWND = (UIntPtr)0;

            int count = 0;
            while (HWND == (UIntPtr)0)
            {
                Pause(500);

                HWND = FindWindow("Granado Espada", "Granado Espada");
                //HWND = FindWindow("Granado Espada", "[#] Granado Espada [#]");
                
                count++; if (count > 5) return (UIntPtr)0;
            }

            botParam.Hwnd = HWND;
            //botwindow.setHwnd(HWND);

            //SetWindowPos(HWND, 0, 5, 5, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            
            //Pause(500);

            return HWND;
        }


        /// <summary>
        /// поиск новых окон с игрой для кнопки "Найти окна"
        /// </summary>
        /// <returns>HWND найденного окна</returns>
        public override UIntPtr FindWindowGE()
        {
            UIntPtr HWND = (UIntPtr)0;

            int count = 0;
            while (HWND == (UIntPtr)0)
            {
                Pause(500);
                ////вариант 1. ишем окно, открытое в песочнице===========================================================
                HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":Granado Espada", "[#] Granado Espada [#]");

                //вариант 2. ищем чистое окно==========================================================================
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

            for (int i = 1; i <= 10; i++)
            {
                Pause(1000);

                if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
                {
                    //pointNewSteamOk.PressMouseL();
                    AcceptUserAgreement();
                    break;
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
        /// запуск клиента игры
        /// </summary>
        public override void runClient()
        {

            #region песочница. сначала Стим, потом игра

            AccountBusy = false;

            //запускаем steam в песочнице
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -noreactlogin -login " + GetLogin() + " " + GetPassword() + " -silent";
            //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -noreactlogin -login " + GetLogin() + " " + GetPassword();
            process.Start();

//            Pause(55000);
            while (!FindWindowSteamBool())
            {
                Pause(2000);
            }

            process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -applaunch 663090 -silent";
            process.Start();

            Pause(5000);
            for (int i = 1; i <= 10; i++)
            {
                Pause(1000);
                if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
                {
                    AcceptUserAgreement();
                    break;
                }
            }

            #endregion

            #region для песочницы

            //AccountBusy = false;

            ////запускаем steam в песочнице (вариант 1)
            //Process process = new Process();
            //process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -noreactlogin -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            ////steam://rungameid/319730
            //process.Start();


            //Pause(15000);
            //for (int i = 1; i <= 20; i++)
            //{
            //    new Point(1400, 710).PressMouseL();

            //    //если открыто окно Стим
            //    if (isOpenSteamWindow()) { CloseSteamWindow(); CloseSteam(); }
            //    //если открыто окно Стим
            //    if (isOpenSteamWindow2()) { CloseSteamWindow2(); CloseSteam(); }
            //    //Pause(1000);
            //    //if (isSteamService())
            //    //{
            //    //    CloseSteam();
            //    //    //Pause(10000);
            //    //    //break;
            //    //}
            //    Pause(2000);
            //}

            //for (int i = 1; i <= 10; i++)
            //{
            //    CloseWhatNews();

            //Pause(1000);

            //    if (isSystemError())  //если выскакивает системная ошибка, то нажимаем "Ок"     проверка не работает
            //    {
            //        OkSystemError();
            //    }

            //if (isNewSteam())
            //{
            //    //new Point(1040, 560).PressMouseL();   //нажимаем "Отмена" на предложение установить службу Стим
            //    //Pause(1000);
            //    //pointNewSteamOk.PressMouseL();  //нажимаем "Соглашаюсь"
            //    AcceptUserAgreement();
            //    break;
            //}

            //    if (isContinueRunning())    //если аккаунт запущен на другом компе
            //    {
            //        NextAccount();
            //        AccountBusy = true;
            //        break;
            //    }
            //}


            #endregion

            #region для чистого окна Steam

            //AccountBusy = false;

            //Process process = new Process();
            //process.StartInfo.FileName = this.pathClient;
            //process.StartInfo.Arguments = " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            //process.Start();
            //Pause(10000);

            //if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
            //{
            //    pointNewSteamOk.PressMouseL();
            //}

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
            //Pause(100);
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
            pointMana2.PressMouseL();
            pointMana3.PressMouseL();
            new Point(1500, 500).Move();
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
        /// нажмает на выбранный раздел верхнего меню /не работает с 22-11-23/
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
        /// нажать на выбранный раздел верхнего меню, а далее на пункт раскрывшегося списка  /не работает с 22-11-23/
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

            //// отбегаю в сторону. чтобы бот не стрелял 
            //runAway();                            

            TopMenu(6, 1);
            Pause(1000);
            pointTeleportToTownAltW.PressMouseL();           
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
            //iPoint pointEquipmentBegin = new Point(701 - 5 + xx + (numberOfEquipment - 1) * 39, 183 - 5 + yy);
            //iPoint pointEquipmentEnd = new Point(521 - 5 + xx, 208 - 5 + yy);
            iPoint pointEquipmentBegin = new Point(706 - 5 + xx + (numberOfEquipment - 1) * 39, 188 - 5 + yy);
            iPoint pointEquipmentEnd = new Point(526 - 5 + xx, 211 - 5 + yy);
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
            if (!isLoadedSteamBH)   //если в данный момент не грузится другой стим и другие окна ГЭ
            {
                if (!FindWindowSteamBool())
                {
                    #region для песочницы
                    if (isActiveServer)    //если надо грузить, то грузим 
                    {
                        isLoadedSteamBH = true;   

                        //запускаем steam в песочнице
                        Process process = new Process();
                        process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
                        process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -noreactlogin -login " + GetLogin() + " " + GetPassword() + " -silent";
                        //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -noreactlogin -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";   //15.06.23
                        process.Start();
                        
                        for (int i = 1; i <= 10; i++)
                        {
                            Pause(1000);

                            if (isNewSteam())           //если первый раз входим в игру, то соглашаемся с лиц. соглашением
                            {
                                pointNewSteamOk.PressMouseL();
                            }

                        }
                    }
                    else             //если надо пропустить этот аккаунт из-за "Параметр.txt"
                    {
                        RemoveSandboxieBH();
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Стим уже загружен. Запускаем игру в песочнице. для Инфинити и проч.
        /// </summary>
        public override void runClientBH()
        {
            //if (!isLoadedGEBH)   //если в данный момент не грузится другой стим и другие окна ГЭ
            //{
                //if (!FindWindowGEforBHBool())  //эта проверка уже стоит в check и получает код проблемы 23, а сюда приходим только с проблемой 22
                //{
                #region для песочницы

                if (isActiveServer)    //если надо грузить, то грузим 
                {
                    //isLoadedGEBH = true;
                    AccountBusy = false;


                    //Стим уже загружен. Запускаем игру в песочнице (вариант 1)
                    Process process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
                    process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -applaunch 663090";
                    //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";

                    process.Start();



                    for (int i = 1; i <= 10; i++)
                    {
                        Pause(1000);

                        //    //if (isSystemError())  //если выскакивает системная ошибка, то нажимаем "Ок"     проверка не работает
                        //    //{
                        //    //    OkSystemError();
                        //    //}

                        if (isNewSteam())
                        {
                            pointNewSteamOk.PressMouseL(); //нажимаем кнопку "соглашаюсь"
                        }

                        //    if (isContinueRunning())    //если аккаунт запущен на другом компе
                        //    {
                        //        NextAccount();
                        //        AccountBusy = true;
                        //        //                            RemoveSandboxieBH();
                        //        CloseSandboxieBH();
                        //        botParam.NumberOfInfinity = globalParam.Infinity;
                        //        globalParam.Infinity = botParam.NumberOfInfinity + 1;

                        //        break;
                        //    }
                    }
                }
                else             //если надо пропустить этот аккаунт из-за "Параметр.txt"
                {
                    RemoveSandboxieBH();
                }
                #endregion
                //}
            //}
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
        /// найдено ли окно ГЭ в текущей песочнице? Если найдено, то в файл hwnd.txt пишем найденный hwnd 
        /// </summary>
        /// <returns>true, если найдено</returns>
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
        /// поиск окна Steam в текущей песочнице (в том числе для миссии в Demonic)
        /// </summary>
        /// <returns>true, если найден стим для текущего окна</returns>
        public override bool FindWindowSteamBool()
        {
            bool result = false;
            UIntPtr HWND;
            if (globalParam.Windows10)
                //HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", "[#] Steam [#]");
                HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", ""); //15.06.23
            else
//                HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", "Steam");
                HWND = FindWindow("Sandbox:" + botwindow.getNumberWindow().ToString() + ":vguiPopupWindow", "");   //15.06.23

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
        /// запуск клиента игры для миссии Demonic в Гильдии Охотников
        /// </summary>
        public override void RunClientDem()
        {
            #region для песочницы

            //Стим загружен. запускаем игру в песочнице (вариант 1)
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -applaunch 663090 -silent";
            //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " " + this.pathClient + " -login " + GetLogin() + " " + GetPassword() + " -applaunch 663090 -silent";
            process.Start();

            //for (int i = 1; i <= 10; i++)
            //{
            //    Pause(1000);

            //    if (isNewSteam())
            //    {
            //        pointNewSteamOk.PressMouseL(); //нажимаем кнопку "соглашаюсь"
            //    }

            //}
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




    }
}

