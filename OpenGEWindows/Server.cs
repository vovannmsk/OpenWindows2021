using System;
using System.Linq;
//using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
//using System.Drawing;
using GEBot.Data;
using System.Diagnostics;
using System.Data.Entity.Core.Metadata.Edm;
//using System.Runtime.InteropServices.ComTypes;
//using System.Windows.Media;
//using OpenGEWindows;

namespace OpenGEWindows
{
    public abstract class Server

    {
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(UIntPtr myhWnd, int myhwndoptional, int xx, int yy, int cxx, int cyy, uint flagus); // Перемещает окно в заданные координаты с заданным размером

        [DllImport("user32.dll")]
        static extern bool PostMessage(UIntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam);

        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //public static extern UIntPtr FindWindowEx(UIntPtr hwndParent, UIntPtr hwndChildAfter, string className, string windowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(UIntPtr hWnd, int nCmdShow);  //раскрывает окно, если оно было скрыто в трей

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(UIntPtr hWnd); // Перемещает окно в верхний список Z порядка


        #region статические переменные

        /// <summary>
        /// Если никакой Steam не грузится, то переменная = 0. Значит можно грузить новый Стим.
        /// Если Стим грузится, то переменная равна номеру окна (numberOfWindow)
        /// </summary>
        private static int IsItAlreadyPossibleToUploadNewSteam = 0;

        /// <summary>
        /// Если никакое окно с игрой не грузится, то переменная = 0. 
        /// Если окно грузится, то переменная равна номеру окна (numberOfWindow)
        /// </summary>
        private static int IsItAlreadyPossibleToUploadNewWindow = 0;

        /// <summary>
        /// true, если окно загружено на другом компе
        /// </summary>
        public static bool AccountBusy;

        /// <summary>
        /// true, если сейчас происходит загрузка нового окна ГЭ. Переменная нужна, чтобы не грузить одновременно два окна для BH
        /// </summary>
        //public static bool isLoadedGEBH;         //21-12-23

        /// <summary>
        /// true, если сейчас происходит загрузка нового стима. Переменная нужна, чтобы не грузить одновременно два окна для BH
        /// </summary>
        public static bool isLoadedSteamBH;

        #endregion

        #region общие

        protected botWindow botwindow;
        protected GlobalParam globalParam;
        protected ServerParam serverParam;
        protected BotParam botParam;
        protected int xx;
        protected int yy;
        protected int xxx;
        protected int yyy;
        protected int numberOfWindow;

        #endregion

        #region общие 2

        protected Town town;
        protected Town town_begin;
        protected Dialog dialog;

        #endregion

        #region параметры, зависящие от сервера

        protected string pathClient;

        /// <summary>
        /// это свойство можно использовать везде вместо метода isActive()
        /// </summary>
        protected bool isActiveServer;

        #endregion

        #region No Window

        protected iPointColor pointSafeIP1;
        protected iPointColor pointSafeIP2;
        protected iPointColor pointisSteam1;
        protected iPointColor pointisSteam2;
        protected iPointColor pointisNewSteam1;
        protected iPointColor pointisNewSteam2;
        protected iPointColor pointisNewSteam3;
        protected iPointColor pointisNewSteam4;
        //protected iPointColor pointisError1;
        //protected iPointColor pointisError2;
        protected iPointColor pointisContinueRunning1;
        protected iPointColor pointisContinueRunning2;
        protected iPoint pointSteamLogin1;
        protected iPoint pointSteamLogin2;
        protected iPoint pointSteamPassword;
        protected iPoint pointSteamSavePassword;
        protected iPoint pointSteamOk;
        protected iPoint pointNewSteamOk;
        protected iPoint pointCancelContinueRunning;

        protected const int WIDHT_WINDOW = 1024;
        protected const int HIGHT_WINDOW = 700;

        protected iPointColor pointisWhatNews1;
        protected iPointColor pointisWhatNews2;

        protected iPoint pointLeaveGame;


        #endregion

        #region Logout

        protected iPointColor pointConnect;
        protected iPointColor pointisLogout1;
        protected iPointColor pointisLogout2;
        protected iPointColor pointIsServerSelection1;
        protected iPointColor pointIsServerSelection2;
        protected iPoint pointserverSelection;

        #endregion

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

        #region Top Menu
        protected iPointColor pointisOpenTopMenu21;
        protected iPointColor pointisOpenTopMenu22;
        protected iPointColor pointisOpenTopMenu51;
        protected iPointColor pointisOpenTopMenu52;
        protected iPointColor pointisOpenTopMenu61;
        protected iPointColor pointisOpenTopMenu62;
        protected iPointColor pointisOpenTopMenu81;
        protected iPointColor pointisOpenTopMenu82;
        protected iPointColor pointisOpenTopMenu91;
        protected iPointColor pointisOpenTopMenu92;
        protected iPointColor pointisOpenTopMenu121;
        protected iPointColor pointisOpenTopMenu122;
        protected iPointColor pointisOpenTopMenu121work;
        protected iPointColor pointisOpenTopMenu122work;
        protected iPointColor pointisOpenTopMenu131;
        protected iPointColor pointisOpenTopMenu132;
        protected iPointColor pointisOpenTopMenu141;
        protected iPointColor pointisOpenTopMenu142;
        protected iPointColor pointisOpenTopMenu161;
        protected iPointColor pointisOpenTopMenu162;
        protected iPointColor pointisOpenMenuChooseChannel1;
        protected iPointColor pointisOpenMenuChooseChannel2;
        protected iPointColor pointIsCurrentChannel1;
        protected iPointColor pointIsCurrentChannel2;
        protected iPointColor pointIsCurrentChannel3;
        protected iPointColor pointIsCurrentChannel4;

        protected iPoint pointLogout;
        protected iPoint pointGotoEnd;
        protected iPoint pointGotoBarack;
        protected iPoint pointTeleportFirstLine;
        //        protected iPoint pointTeleportSecondLine;
        protected iPoint pointTeleportExecute;

        #endregion

        #region Shop

        protected iPointColor pointIsSale1;
        protected iPointColor pointIsSale2;
        protected iPointColor pointIsSale21;
        protected iPointColor pointIsSale22;
        protected iPointColor pointIsClickSale1;
        protected iPointColor pointIsClickSale2;
        protected iPoint pointBookmarkSell;
        protected iPoint pointSaleToTheRedBottle;
        protected iPoint pointSaleOverTheRedBottle;
        protected iPoint pointWheelDown;
        protected iPoint pointButtonBUY;
        protected iPoint pointButtonSell;
        protected iPoint pointButtonClose;
        protected iPoint pointBuyingMitridat1;
        protected iPoint pointBuyingMitridat2;
        protected iPoint pointBuyingMitridat3;

        /// <summary>
        /// структура для сравнения товаров в магазине
        /// </summary>
        protected struct Product
        {
            public uint color1;
            public uint color2;
            public uint color3;

            /// <summary>
            /// создаем структуру объекта, состоящую из трех точек
            /// </summary>
            /// <param name="xx">сдвиг окна по оси X</param>
            /// <param name="yy">сдвиг окна по оси Y</param>
            /// <param name="numderOfString">номер строки в магазине, где берется товар</param>
            public Product(int xx, int yy, int numderOfString)
            {
                color1 = new PointColor(149 - 5 + xx, 219 - 5 + yy + (numderOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color2 = new PointColor(146 - 5 + xx, 219 - 5 + yy + (numderOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color3 = new PointColor(165 - 5 + xx, 214 - 5 + yy + (numderOfString - 1) * 27, 3360337, 0).GetPixelColor();
            }

            /// <summary>
            /// сравнение двух товаров по трем точкам
            /// </summary>
            /// <param name="product">товар для сравнения</param>
            /// <returns>true, если два товара одинаковы</returns>
            public bool EqualProduct(Product product)
            {
                return ((product.color1 == color1) &&
                        (product.color2 == color2) &&
                        (product.color3 == color3));
            }
        };

        #endregion

        #region atWork

        protected iPointColor pointisBadFightingStance1;   //неправильная стойка
        protected iPointColor pointisBadFightingStance2;
        protected iPoint pointProperFightingStance;  //правильная стойка
        protected iPointColor pointisKillHero1;      //если перс убит
        protected iPointColor pointisKillHero2;
        protected iPointColor pointisKillHero3;
        protected iPointColor pointisLiveHero1;      //если перс жив
        protected iPointColor pointisLiveHero2;
        protected iPointColor pointisLiveHero3;
        protected iPointColor pointisBoxOverflow1;
        protected iPointColor pointisBoxOverflow2;
        protected iPointColor pointisBoxOverflow3;
        protected iPointColor pointisBoxOverflow4;
        protected iPoint pointSkillCook;
        protected iPointColor pointisBattleMode1;
        protected iPointColor pointisBattleMode2;
        //protected iPointColor pointisBattleModeOff1;
        //protected iPointColor pointisBattleModeOff2;
        protected uint[] arrayOfColorsIsWork1;
        protected uint[] arrayOfColorsIsWork2;
        //protected iPointColor pointisBulletHalf;      //если патронов половина (желтый значок)
        protected iPointColor pointisBulletHalf1;      //если патронов половина (желтый значок) первый перс
        protected iPointColor pointisBulletHalf2;      //если патронов половина (желтый значок) второй перс
        protected iPointColor pointisBulletHalf3;      //если патронов половина (желтый значок) третий перс
        //protected iPointColor pointisBulletOff;       //если патронов почти нет (красный значок)
        protected iPointColor pointisBulletOff1;       //если патронов почти нет (красный значок) первый перс
        protected iPointColor pointisBulletOff2;       //если патронов почти нет (красный значок) второй перс
        protected iPointColor pointisBulletOff3;       //если патронов почти нет (красный значок) третий перс
        protected iPointColor pointisAssaultMode1;
        protected iPointColor pointisAssaultMode2;
        #endregion

        #region inTown

        protected iPointColor pointisToken1;      //при входе в город проверяем, открыто ли окно с подарочными токенами.
        protected iPointColor pointisToken2;
        protected iPoint pointToken;            //если окно с подарочными токенами открыто, то закрываем его нажатием на эту точку
        protected iPoint pointCure1;
        protected iPoint pointCure2;
        protected iPoint pointCure3;
        protected Point pointMana1;
        protected Point pointMana2;
        protected Point pointMana3;
        protected iPoint pointGM;
        protected iPoint pointHeadGM;
        //protected iPointColor pointIsTown_RifleFirstDot1;   //проверка по обычному ружью
        //protected iPointColor pointIsTown_RifleFirstDot2;
        //protected iPointColor pointIsTown_RifleSecondDot1;
        //protected iPointColor pointIsTown_RifleSecondDot2;
        //protected iPointColor pointIsTown_RifleThirdDot1;
        //protected iPointColor pointIsTown_RifleThirdDot2;
        //protected iPointColor pointIsTown_ExpRifleFirstDot1;   //проверка по эксп. ружью (флинт)
        //protected iPointColor pointIsTown_ExpRifleFirstDot2;
        //protected iPointColor pointIsTown_ExpRifleSecondDot1;
        //protected iPointColor pointIsTown_ExpRifleSecondDot2;
        //protected iPointColor pointIsTown_ExpRifleThirdDot1;
        //protected iPointColor pointIsTown_ExpRifleThirdDot2;
        //protected iPointColor pointIsTown_DrobFirstDot1;      //проверка по обычному дробовику
        //protected iPointColor pointIsTown_DrobFirstDot2;
        //protected iPointColor pointIsTown_DrobSecondDot1;
        //protected iPointColor pointIsTown_DrobSecondDot2;
        //protected iPointColor pointIsTown_DrobThirdDot1;
        //protected iPointColor pointIsTown_DrobThirdDot2;
        //protected iPointColor pointIsTown_VetDrobFirstDot1;    //проверка по вет. дробовику
        //protected iPointColor pointIsTown_VetDrobFirstDot2;
        //protected iPointColor pointIsTown_VetDrobSecondDot1;
        //protected iPointColor pointIsTown_VetDrobSecondDot2;
        //protected iPointColor pointIsTown_VetDrobThirdDot1;
        //protected iPointColor pointIsTown_VetDrobThirdDot2;
        //protected iPointColor pointIsTown_ExpDrobFirstDot1;    //проверка по эксп. дробовику
        //protected iPointColor pointIsTown_ExpDrobFirstDot2;
        //protected iPointColor pointIsTown_ExpDrobSecondDot1;
        //protected iPointColor pointIsTown_ExpDrobSecondDot2;
        //protected iPointColor pointIsTown_ExpDrobThirdDot1;
        //protected iPointColor pointIsTown_ExpDrobThirdDot2;
        //protected iPointColor pointIsTown_JainaDrobFirstDot1;    //проверка по эксп. дробовику Джаина
        //protected iPointColor pointIsTown_JainaDrobFirstDot2;
        //protected iPointColor pointIsTown_VetSabreFirstDot1;    //проверка по вет сабле
        //protected iPointColor pointIsTown_VetSabreFirstDot2;
        //protected iPointColor pointIsTown_ExpSwordFirstDot1;    //проверка по мечу Дарья
        //protected iPointColor pointIsTown_ExpSwordFirstDot2;   
        //protected iPointColor pointIsTown_VetPistolFirstDot1;    //проверка по двум пистолетам outrange
        //protected iPointColor pointIsTown_VetPistolFirstDot2;
        //protected iPointColor pointIsTown_SightPistolFirstDot1;  //проверка по одному пистолету Sight Shot
        //protected iPointColor pointIsTown_SightPistolFirstDot2;
        //protected iPointColor pointIsTown_UnlimPistolFirstDot1;  //проверка по двум пистолетам эксп стойка Unlimited Shot
        //protected iPointColor pointIsTown_UnlimPistolFirstDot2;
        //protected iPointColor pointIsTown_ExpCannonFirstDot1;   // проверка по эксп пушке Мисы
        //protected iPointColor pointIsTown_ExpCannonFirstDot2;
        protected uint[] arrayOfColorsIsTown1;
        protected uint[] arrayOfColorsIsTown2;

        #endregion

        #region Алхимия

        protected iPointColor pointisAlchemy1;
        protected iPointColor pointisAlchemy2;
        protected iPoint pointAlchemy;
        protected iPointColor pointisInventoryFull1;
        protected iPointColor pointisInventoryFull2;
        protected iPointColor pointisOutOfIngredient1_1;
        protected iPointColor pointisOutOfIngredient1_2;
        protected iPointColor pointisOutOfIngredient2_1;
        protected iPointColor pointisOutOfIngredient2_2;
        protected iPointColor pointisOutOfIngredient3_1;
        protected iPointColor pointisOutOfIngredient3_2;
        protected iPointColor pointOutOfMoney1;
        protected iPointColor pointOutOfMoney2;

        #endregion

        #region Barack

        protected iPoint pointTeamSelection1;
        protected iPoint pointTeamSelection2;
        protected iPoint pointTeamSelection3;
        protected iPoint pointButtonLogoutFromBarack;
        //protected iPoint pointChooseChannel;
        protected iPoint pointEnterChannel;
        protected iPoint pointMoveNow;
        protected int sdvigY;
        protected iPointColor pointisBarack1;
        protected iPointColor pointisBarack2;
        protected iPointColor pointisBarack3;
        protected iPointColor pointisBarack4;
        protected iPointColor pointisBarack5;
        protected iPointColor pointisBarack6;
        protected iPointColor pointisBarackTeamSelection1;
        protected iPointColor pointisBarackTeamSelection2;
        protected iPoint pointNewPlace;
        protected iPoint pointLastPoint;
        protected iPoint pointCreateButton;
        /// <summary>
        /// кнопка "To Barack" на странице создания персонажа
        /// </summary>
        protected iPoint pointToBarack;
        protected iPointColor pointisBHLastPoint1;
        protected iPointColor pointisBHLastPoint2;

        #endregion

        #region создание нового бота
        protected iPoint pointNewName;
        protected iPoint pointButtonCreateNewName;
        protected iPoint pointCreateHeroes;
        protected iPoint pointButtonOkCreateHeroes;
        protected iPoint pointMenuSelectTypeHeroes;
        protected iPoint pointSelectTypeHeroes;
        protected iPoint pointNameOfHeroes;
        protected iPoint pointButtonCreateChar;
        protected iPoint pointSelectMusk;
        protected iPoint pointUnselectMedik;
        protected iPoint pointNameOfTeam;
        protected iPoint pointButtonSaveNewTeam;

        protected iPoint pointRunNunies;
        protected iPoint pointPressNunez;
        protected iPoint ButtonOkDialog;
        protected iPoint PressMedal;
        protected iPoint ButtonCloseMedal;
        protected iPoint pointPressNunez2;

        protected iPoint pointPressLindon1;
        protected iPoint pointPressGMonMap;
        protected iPoint pointPressGM_1;
        protected iPoint pointPressSoldier;
        protected iPoint pointFirstStringSoldier;
        protected iPoint pointRifle;
        protected iPoint pointCoat;
        protected iPoint pointButtonPurchase;
        protected iPoint pointButtonCloseSoldier;
        protected iPoint pointButtonYesSoldier;
        protected iPoint pointFirstItem;
        protected iPoint pointDomingoOnMap;
        protected iPoint pointPressDomingo;
        protected iPoint pointFirstStringDialog;
        protected iPoint pointSecondStringDialog;
        protected iPoint pointDomingoMiss;
        protected iPoint pointPressDomingo2;
        protected iPoint pointLindonOnMap;
        protected iPoint pointPressLindon2;
        protected iPoint pointPetExpert;
        protected iPoint pointPetExpert2;
        protected iPoint pointThirdBookmark;
        protected iPoint pointNamePet;
        protected iPoint pointButtonNamePet;
        protected iPoint pointButtonClosePet;
        protected iPoint pointWayPointMap;
        protected iPoint pointWayPoint;
        protected iPoint pointBookmarkField;
        protected iPoint pointButtonLavaPlato;
        protected Point pointPetBegin;  //для перетаскивания пета
        protected Point pointPetEnd;

        #endregion

        #region кратер

        protected iPoint pointGateCrater;
        protected iPoint pointSecondBookmark;
        protected iPoint pointMitridat;
        protected iPoint pointMitridatTo2;
        protected iPoint pointBookmark3;
        protected iPoint pointButtonYesPremium;

        protected iPoint pointWorkCrater;
        protected iPoint pointButtonSaveTeleport;
        protected iPoint pointButtonOkSaveTeleport;
        #endregion

        #region Ида заточка
        protected iPoint pointAcriveInventory;
        protected iPointColor pointIsActiveInventory;

        protected iPoint pointEquipmentBegin;
        protected iPoint pointEquipmentEnd;
        protected iPointColor pointisMoveEquipment1;
        protected iPointColor pointisMoveEquipment2;

        protected iPoint pointButtonEnhance;
        protected iPointColor pointIsPlus41;
        protected iPointColor pointIsPlus42;
        protected iPointColor pointIsPlus43;
        protected iPointColor pointIsPlus44;

        protected iPoint pointAddShinyCrystall;
        protected iPoint pointAddShinyCrystall2;
        protected iPointColor pointIsAddShinyCrystall1;
        protected iPointColor pointIsAddShinyCrystall2;

        protected iPointColor pointIsIda1;
        protected iPointColor pointIsIda2;
        #endregion

        #region чиповка
        protected iPointColor pointIsEnchant1;
        protected iPointColor pointIsEnchant2;
        protected iPointColor pointisWeapon1;
        protected iPointColor pointisWeapon2;
        protected iPointColor pointisArmor1;
        protected iPointColor pointisArmor2;
        protected iPoint pointMoveLeftPanelBegin;
        protected iPoint pointMoveLeftPanelEnd;
        protected iPoint pointButtonEnchance;
        protected iPointColor pointisDef15;
        protected iPointColor pointisHP1;
        protected iPointColor pointisHP2;
        protected iPointColor pointisHP3;
        protected iPointColor pointisHP4;

        protected iPointColor pointisAtk401;
        protected iPointColor pointisAtk402;
        protected iPointColor pointisSpeed30;

        protected iPointColor pointisAtk391;
        protected iPointColor pointisAtk392;
        protected iPointColor pointisSpeed291;
        protected iPointColor pointisSpeed292;

        protected iPointColor pointisAtk381;
        protected iPointColor pointisAtk382;
        protected iPointColor pointisSpeed281;
        protected iPointColor pointisSpeed282;

        protected iPointColor pointisAtk371;
        protected iPointColor pointisAtk372;
        protected iPointColor pointisSpeed271;
        protected iPointColor pointisSpeed272;

        protected iPointColor pointisWild41;  //строка 4
        protected iPointColor pointisWild42;
        protected iPointColor pointisWild51;  //строка 5
        protected iPointColor pointisWild52;
        protected iPointColor pointisWild61;  //строка 6
        protected iPointColor pointisWild62;
        protected iPointColor pointisWild71;  //строка 7
        protected iPointColor pointisWild72;

        protected iPointColor pointisHuman41;  //строка 4
        protected iPointColor pointisHuman42;
        protected iPointColor pointisHuman51;  //строка 5
        protected iPointColor pointisHuman52;
        protected iPointColor pointisHuman61;  //строка 6
        protected iPointColor pointisHuman62;
        protected iPointColor pointisHuman71;  //строка 7
        protected iPointColor pointisHuman72;

        protected iPointColor pointisDemon41;  //строка 4
        protected iPointColor pointisDemon42;
        protected iPointColor pointisDemon51;  //строка 5
        protected iPointColor pointisDemon52;
        protected iPointColor pointisDemon61;  //строка 6
        protected iPointColor pointisDemon62;
        protected iPointColor pointisDemon71;  //строка 7
        protected iPointColor pointisDemon72;

        protected iPointColor pointisUndead41;  //строка 4
        protected iPointColor pointisUndead42;
        protected iPointColor pointisUndead51;  //строка 5
        protected iPointColor pointisUndead52;
        protected iPointColor pointisUndead61;  //строка 6
        protected iPointColor pointisUndead62;
        protected iPointColor pointisUndead71;  //строка 7
        protected iPointColor pointisUndead72;

        protected iPointColor pointisLifeless41;  //строка 4
        protected iPointColor pointisLifeless42;
        protected iPointColor pointisLifeless51;  //строка 5
        protected iPointColor pointisLifeless52;
        protected iPointColor pointisLifeless61;  //строка 6
        protected iPointColor pointisLifeless62;
        protected iPointColor pointisLifeless71;  //строка 7
        protected iPointColor pointisLifeless72;


        #endregion

        #region для перекладывания песо в торговца

        protected iPointColor pointPersonalTrade1;
        protected iPointColor pointPersonalTrade2;
        protected iPoint pointTrader;
        protected iPoint pointPersonalTrade;
        protected iPoint pointMap;
        protected iPoint pointVis1;
        protected iPoint pointVisMove1;
        protected iPoint pointVisMove2;
        protected iPoint pointVisOk2;
        protected iPoint pointVisTrade;
        protected iPoint pointFood;
        protected iPoint pointButtonFesoBUY;
        protected iPoint pointArrowUp2;
        protected iPoint pointButtonFesoSell;
        protected iPoint pointBookmarkFesoSell;
        protected iPoint pointDealer;
        protected iPoint pointYesTrade;
        protected iPoint pointBookmark4;
        protected iPoint pointFesoBegin;
        protected iPoint pointFesoEnd;
        protected iPoint pointOkSum;
        protected iPoint pointOk;
        protected iPoint pointTrade;

        #endregion

        #region Undressing in Barack

        protected iPoint pointShowEquipment;
        //protected iPoint pointBarack1;
        //protected iPoint pointBarack2;
        //protected iPoint pointBarack3;
        //protected iPoint pointBarack4;

        protected iPoint[] pointBarack = new Point[5];
        protected iPointColor pointEquipment1;
        protected iPointColor pointEquipment2;


        #endregion

        #region BH

        protected iPoint pointGateInfinityBH;
        protected iPointColor pointisBH1;           //правильное место в бх
        protected iPointColor pointisBH2;           //неправильное место в бх №1
        protected iPointColor pointisBH3;           //неправильное место в бх №2
        protected iPointColor pointIsAtak1;
        protected iPointColor pointIsAtak2;
        protected iPointColor pointIsRoulette1;
        protected iPointColor pointIsRoulette2;

        /// <summary>
        /// массив номеров цветов. по нему определяем, в какую миссию вошли
        /// </summary>
        protected uint[] arrayOfColors;



        #endregion

        #region Вход-выход
        protected iPointColor pointisBeginOfMission1;
        protected iPointColor pointisBeginOfMission2;
        #endregion

        #region Работа с инвентарем и CASH-инвентарем

        /// <summary>
        /// структура для поиска вещей в инвентаре по двум точкам типа PointColor
        /// </summary>
        public struct Thing   //вещь 
        {
            public PointColor point1;
            public PointColor point2;

            public Thing(PointColor point1, PointColor point2)
            {
                this.point1 = point1;
                this.point2 = point2;
            }
        }

        Thing masterScroll = new Thing(new PointColor(703, 187, 461065, 0), new PointColor(704, 174, 1317005, 0));
        Thing expCard = new Thing(new PointColor(704, 174, 1317005, 0), new PointColor(705, 174, 1317005, 0));
        Thing Honey = new Thing(new PointColor(697, 180, 4289893, 0), new PointColor(703, 180, 4487011, 0));
        Thing Apple = new Thing(new PointColor(697, 180, 7465471, 0), new PointColor(704, 180, 42495, 0));
        Thing DesapioNecklace = new Thing(new PointColor(704, 183, 16777112, 0), new PointColor(704, 182, 16711143, 0));
        Thing DesapioGloves = new Thing(new PointColor(700, 181, 14130236, 0), new PointColor(700, 182, 16777177, 0));
        Thing DesapioBoots = new Thing(new PointColor(697, 183, 8154148, 0), new PointColor(697, 184, 9074735, 0));
        Thing DesapioBelt = new Thing(new PointColor(700, 182, 12426579, 0), new PointColor(700, 183, 8360599, 0));
        Thing DesapioEarrings = new Thing(new PointColor(700, 183, 16777215, 0), new PointColor(701, 184, 657680, 0));


        /// <summary>
        /// структура для поиска вещей в CASH-инвентаре по одной точке
        /// </summary>
        public struct Item    //предмет 
        {
            public int x;
            public int y;
            public uint color;

            /// <summary>
            /// конструктор. Создание структуры по координатам (x, y) и цвету точки (color)
            /// </summary>
            /// <param name="x">X</param>
            /// <param name="y">Y</param>
            /// <param name="color">цвет</param>
            public Item(int x, int y, uint color)
            {
                this.x = x;
                this.y = y;
                this.color = color;
            }
        }

        Item Coat = new Item(35, 209, 16645630);
        Item Necklace = new Item(36, 219, 12179686);
        Item Glove = new Item(26, 205, 2906307);
        Item Shoes = new Item(33, 202, 16777215);
        Item Belt = new Item(35, 209, 11372402);
        Item Earring = new Item(37, 205, 11263973);
        Item Rifle = new Item(31, 214, 12567238);
        /// <summary>
        /// журнал с ежедневными наградами
        /// </summary>
        Item Journal = new Item(38, 209, 4390911);
        Item Steroid10Hours2 = new Item(36, 211, 11690052);
        Item Steroid10Hours = new Item(36, 211, 11296065);
        Item Principle10Hours = new Item(37, 210, 3226091);
        Item SteroidOneHour = new Item(36, 216, 15082944);
        Item Steroid10HoursHP = new Item(43, 213, 16732560);
        Item PrincipleOneHour = new Item(43, 214, 16756209);
        //Item Principle10HoursHP = new Item(0, 0, 0);     // не сделано
        Item Triumph = new Item(35, 209, 47612);

        Item Steroid10HoursLeftPanel = new Item(31 + 5, 241 + 5, 11690052);
        Item Principle10HoursLeftPanel = new Item(32 + 5, 272 + 5, 3226091);
        //Item TriumphLeftPanel = new Item(31 + 5, 304 + 5, 47612);          //AR+DR
        Item SteroidOneHourLeftPanel = new Item(38, 249, 13836720);
        Item PrincipleOneHourLeftPanel = new Item(29, 282, 11943268);
        Item Steroid10HoursHPLeftPanel = new Item(37, 249, 16724347);
        Item Principle10HoursHPLeftPanel = new Item(29, 282, 11943268);     // не сделано
        /// <summary>
        /// розовые крылья
        /// </summary>
        Item PinkWings = new Item(36 + 5, 211 + 5, 4870905);
        /// <summary>
        /// коробка с розовыми крыльями
        /// </summary>
        Item PinkWingsBox = new Item(42, 213, 5522397);  //проверено
        //Item PinkWingsBox = new Item(36, 211, 5144242);   //старое значение


        protected iPointColor pointisOpenInventory1;
        protected iPointColor pointisOpenInventory2;



        #endregion

        #region All in One

        /// <summary>
        /// номер проблемы на предыдущем цикле
        /// </summary>
        protected int prevProblem;
        /// <summary>
        /// номер проблемы на предпредыдущем цикле
        /// </summary>
        protected int prevPrevProblem;
        /// <summary>
        /// информация о том, какие герои в нашей команде: Hero[1] - первый герой, Hero[2] - второй, Hero[3] - третий
        /// 0 - герой не определён, 1 - мушкетёр, 2 - Бернелли (Superior Blaster), 3 - М.Лорч, 4 - Джайна
        /// 5 - молодой Барель, 6 - C.Daria, 7 - Том, 8 - Moon
        /// </summary>
        //protected int[] Hero;
        protected HeroFactory heroFactory;
        /// <summary>
        /// информация о том, какие герои в нашей команде: Hero[1] - первый герой, Hero[2] - второй, Hero[3] - третий
        /// 0 - герой не определён, 1 - мушкетёр, 2 - Бернелли (Superior Blaster), 3 - М.Лорч, 4 - Джайна
        /// 5 - молодой Барель, 6 - C.Daria, 7 - Том, 8 - Moon
        /// </summary>
        protected Hero[] hero;
        protected Hero hero1;
        protected Hero hero2;
        protected Hero hero3;

        /// <summary>
        /// направление движения (1 - вправо, -1 - влево)
        /// </summary>
        protected int DirectionOfMovement;
        /// <summary>
        /// если равна true, тогда атака на мобов закончена и надо подбирать дроп (фесо)
        /// </summary>
        protected bool NeedToPickUpFeso;
        /// <summary>
        /// нужно собирать дроп, двигаясь вправо (Кастилия)
        /// </summary>
        protected bool NeedToPickUpRight;
        /// <summary>
        /// нужно собирать дроп, двигаясь влево (Кастилия)
        /// </summary>
        protected bool NeedToPickUpLeft;
        /// <summary>
        /// для миссии Кастилия. номер следующей точки маршрута
        /// </summary>
        protected int NextPointNumber;
        /// <summary>
        /// счётчик для миссии Демоник. Показывает, пора ли уже бафаться
        /// </summary>
        protected int Counter;
        #endregion

        // ===========================================  Методы ==========================================

        #region общие методы

        /// <summary>
        /// Останавливает поток на некоторый период (пауза)
        /// </summary>
        /// <param name="ms"> ms - период в милисекундах </param>
        protected void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        #endregion

        #region Getters

        public bool IsActiveServer { get { return isActiveServer; } }

        /// <summary>
        /// геттер
        /// </summary>
        /// <returns></returns>
        public Town getTown()
        { return this.town; }

        /// <summary>
        /// возвращает параметр, прочитанный из файла
        /// </summary>
        /// <returns></returns>
        public Town getTownBegin()
        {
            return town_begin;
        }

        #endregion

        #region No window  (работа с чистым окном)

        public abstract void runClientSteamCW();
        public abstract void runClientCW();
        public abstract UIntPtr FindWindowGE_CW();
        public abstract bool FindWindowSteamBoolCW();
        public abstract bool FindWindowCWBool();

       /// <summary>
       /// восстановливает окно (т.е. переводит из состояния "нет окна" в состояние "логаут", плюс из состояния свернутого окна в состояние развернутого и на нужном месте)  
       /// чистое окно (без песочницы)
       /// </summary>
       public void ReOpenWindowCW()
        {
            bool result = isHwnd();   //Перемещает в заданные координаты. Если окно есть, то result=true, а если вылетело окно, то result=false.
            if (!result)  //нет окна с нужным HWND
            {
                if (FindWindowGE_CW() == (UIntPtr)0)   //если поиск окна тоже не дал результатов
                    OpenWindowCW();                 //то загружаем новое окно
                else
                    ActiveWindow(botParam.Hwnd);                      //сдвигаем окно на своё место и активируем его
            }
            else
                ActiveWindow(botParam.Hwnd);                      //сдвигаем окно на своё место и активируем его
        }

        /// <summary>
        /// закрываем текущий клиент игры вместе со Steam (чистое окно)
        /// </summary>
        public void RemoveSandboxieCW()
        {
            int result = globalParam.Infinity;
            if (result >= globalParam.DemonicTo + 1) result = globalParam.DemonicFrom;  //если дошли до последнего подготовленного аккаунта,
                                                                                        //то идём в начало отведённого списка
            // общий кусок кода
            botParam.NumberOfInfinity = result;
            globalParam.Infinity = result + 1;

            CloseSandboxieCW();
            //MoveMouseDown();

        }

        /// <summary>
        /// закрываем чистое окно без перехода к следующему аккаунту (для БХ)
        /// </summary>
        public void CloseSandboxieCW()
        {
            Process process = new Process();
            process.StartInfo.FileName = this.pathClient;
            process.StartInfo.Arguments = " -shutdown";
            process.Start();
        }



        /// <summary>
        /// открывает новое окно бота (т.е. переводит из состояния "нет окна" в состояние "логаут")
        /// чистое окно
        /// </summary>
        /// <returns> hwnd окна </returns>
        public void OpenWindowCW()
        {
            runClientCW();    ///запускаем чистый клиент игры (не в песочнице)
            //Pause(15000);

            while (true)
            {
                UIntPtr hwnd = FindWindowGE_CW();      //ищем окно ГЭ с нужными параметрами(сразу запись в файл HWND.txt)
                if (hwnd != (UIntPtr)0)
                {
                    ActiveWindow(hwnd);
                    //MessageBox.Show("номер нового окна = " + hwnd + " HWND=" + botParam.Hwnd);
                    break;             //если найденное hwnd не равно нулю (то есть открыли ГЭ), то выходим из бесконечного цикла
                }
                Pause(3000);
            }
        }

        /// <summary>
        /// активируем окно с заданным hwnd
        /// </summary>
        public void ActiveWindow(UIntPtr Hwnd)
        {   
            ShowWindow(Hwnd, 9);                                          // Разворачивает окно если свернуто  было 9
            SetForegroundWindow(Hwnd);                                   // Перемещает окно в верхний список Z порядка
            bool sss = SetPosition(Hwnd);                                   // перемещаем окно в заданные для него координаты
        }

        /// <summary>
        /// Перемещает окно с ботом в заданные координаты.  не учитываются ширина и высота окна
        /// </summary>
        /// <returns>Если окно есть, то result = true, а если вылетело окно, то result = false.</returns>
        private bool SetPosition(UIntPtr Hwnd)
        {
            if (globalParam.Windows10)
            {
                //return SetWindowPos(Hwnd, 0, botParam.X, botParam.Y - 1, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);

                return SetWindowPos(Hwnd, -1, botParam.X, botParam.Y - 1, 1040, 739, 1);
            }
            else
                return SetWindowPos(Hwnd, 0, botParam.X, botParam.Y, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001 | 0x0040); ;
        }


        #endregion

        #region No Window

        /// <summary>
        /// закрыть сообщение Steam о том, что окно игры открыто на другом компе
        /// </summary>
        public void CloseSteamMessage()
        {
            new Point(1223, 613).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// открыто ли окно с игрой на другом компе?
        /// </summary>
        /// <returns></returns>
        public bool isOpenGEWindow()
        {
            return (new PointColor(1223, 613, 15131615, 0).isColor() &&
                   new PointColor(1224, 613, 15131615, 0).isColor())
                   ||
                   (new PointColor(1213, 603, 15131615, 0).isColor() &&
                   new PointColor(1214, 603, 15131615, 0).isColor())
                   ;
        }

        /// <summary>
        /// Перемещает окно с ботом в заданные координаты.  не учитываются ширина и высота окна
        /// </summary>
        /// <returns>Если окно есть, то result = true, а если вылетело окно, то result = false.</returns>
        private bool SetPosition()
        {
            if (globalParam.Windows10)
                return SetWindowPos(botParam.Hwnd, 0, botParam.X, botParam.Y - 1, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
            else
                return SetWindowPos(botParam.Hwnd, 0, botParam.X, botParam.Y, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001); ;
        }





        /// <summary>
        /// Существует ли окно с указанным Hwnd? (Попутно перемещает окно с ботом в заданные координаты).
        /// Изначально HWND читаем из текстового файла в модуле BotParam
        /// </summary>
        /// <returns>true, если окно с заданным Hwnd существует</returns>
        public bool isHwnd()
        {
            return SetPosition();
        }

        /// <summary>
        /// активируем окно
        /// </summary>
        public void ActiveWindow()
        {
            //MessageBox.Show("ActiveWindow HWND=" + botParam.Hwnd);
            ShowWindow(botParam.Hwnd, 9);                                       // Разворачивает окно если свернуто  было 9
            SetForegroundWindow(botParam.Hwnd);                                 // Перемещает окно в верхний список Z порядка     
            //BringWindowToTop(botParam.Hwnd);                                  // Делает окно активным и Перемещает окно в верхний список Z порядка     

            bool sss = SetPosition();                                   // перемещаем окно в заданные для него координаты
        }



        /// <summary>
        /// открывает новое окно бота (т.е. переводит из состояния "нет окна" в состояние "логаут")
        /// </summary>
        /// <returns> hwnd окна </returns>
        private void OpenWindow()
        {
            runClient();    ///запускаем клиент игры и ждем 30 сек

            if (!AccountBusy)           //если аккаунт не занят на другом компе
            {
                while (true)
                {
                    Pause(3000);
                    UIntPtr hwnd = FindWindowGE();      //ищем окно ГЭ с нужными параметрами(сразу запись в файл HWND.txt)
                    if (hwnd != (UIntPtr)0) break;             //если найденное hwnd не равно нулю (то есть открыли ГЭ), то выходим из цикла
                }
            }
        }

        /// <summary>
        /// восстановливает окно (т.е. переводит из состояния "нет окна" в состояние "логаут", плюс из состояния свернутого окна в состояние развернутого и на нужном месте)
        /// </summary>
        public void ReOpenWindow()
        {
            bool result = isHwnd();   //Перемещает в заданные координаты. Если окно с нужным HWND есть, то result=true, а если вылетело окно, то result=false.
            if (!result)  //нет окна с нужным HWND
            {
                if (FindWindowGE() == (UIntPtr)0)   //если поиск окна тоже не дал результатов
                {
                    OpenWindow();                 //то загружаем новое окно

                    if (!Server.AccountBusy)
                    {
                        ActiveWindow();

                        while (!isLogout()) Pause(1000);    //ожидание логаута        бесконечный цикл

                        ActiveWindow();
                    }
                }
                else
                {
                    ActiveWindow();                      //сдвигаем окно на своё место и активируем его
                }
            }
            else
            {
                ActiveWindow();                      //сдвигаем окно на своё место и активируем его
            }
        }

        ///// <summary>
        ///// восстановливает "чистое" окно без Sandboxie   /пока не работает/
        ///// (т.е. переводит из состояния "нет окна" в состояние "логаут", плюс из состояния свернутого окна в состояние развернутого и на нужном месте)
        ///// </summary>
        //public void ReOpenClearWindow()
        //{
        //    bool result = isHwnd();   //Перемещает в заданные координаты. Если окно с нужным HWND есть, то result=true, а если вылетело окно, то result=false.
        //    if (!result)  //нет окна с нужным HWND
        //    {
        //        if (FindWindowGE_CW() == (UIntPtr)0)   //если поиск окна тоже не дал результатов
        //        {
        //            OpenWindow();                 //то загружаем новое окно

        //            if (!Server.AccountBusy)
        //            {
        //                ActiveWindow();

        //                while (!isLogout()) Pause(1000);    //ожидание логаута        бесконечный цикл

        //                ActiveWindow();
        //            }
        //        }
        //        else
        //        {
        //            ActiveWindow();                      //сдвигаем окно на своё место и активируем его
        //        }
        //    }
        //    else
        //    {
        //        ActiveWindow();                      //сдвигаем окно на своё место и активируем его
        //    }
        //}


        /// <summary>
        /// проверяем, выскочило ли сообщение о несовместимости версии SafeIPs.dll
        /// </summary>
        /// <returns></returns>
        public bool isSafeIP()
        {
            return pointSafeIP1.isColor() && pointSafeIP2.isColor();
        }

        /// <summary>
        /// открыто ли окно Стим по центру экрана?
        /// </summary>
        /// <returns></returns>
        public bool isOpenSteamWindow()
        {
            return (new PointColor(1559, 161, 9603704, 0).isColor() &&
                    new PointColor(1560, 161, 9603704, 0).isColor());
        }

        /// <summary>
        /// скрыть окно Steam
        /// </summary>
        public void CloseSteamWindow()
        {
            new Point(1593, 166).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// открыто ли окно Стим? другое место в центре
        /// </summary>
        /// <returns></returns>
        public bool isOpenSteamWindow2()
        {
            return new PointColor(1549, 151, 9603704, 0).isColor() &&
                    new PointColor(1550, 151, 9603704, 0).isColor();
        }

        /// <summary>
        /// скрыть окно Steam
        /// </summary>
        public void CloseSteamWindow2()
        {
            new Point(1583, 156).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// открыто ли окно Стим в правом нижнем углу?
        /// </summary>
        /// <returns></returns>
        public bool isOpenSteamWindow3()
        {
            return new PointColor(1869, 451, 9603704, 0).isColor() &&
                    new PointColor(1870, 451, 9603704, 0).isColor();
        }

        /// <summary>
        /// скрыть окно Steam
        /// </summary>
        public void CloseSteamWindow3()
        {
            new Point(1903, 456).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// открыто ли окно Стим? другое место в центре
        /// </summary>
        /// <returns></returns>
        public bool isOpenSteamWindow4()
        {
            return new PointColor(1579, 181, 9603704, 0).isColor() &&
                    new PointColor(1580, 181, 9603704, 0).isColor();
        }

        /// <summary>
        /// скрыть окно Steam
        /// </summary>
        public void CloseSteamWindow4()
        {
            new Point(1613, 186).PressMouseL();
            Pause(500);
        }


        /// <summary>
        /// выскочила ошибка Sandboxie?  /проверяем букву р в слове Закрыть/
        /// </summary>
        /// <returns></returns>
        public bool isErrorSandboxie()
        {
            return new PointColor(1060, 615, 5898291, 0).isColor() &&
                    new PointColor(1060, 616, 51, 0).isColor()
                    ;
        }

        /// <summary>
        /// закрываем окно Sandboxie с ошибкой 
        /// </summary>
        /// <returns></returns>
        public void CloseErrorSandboxie()
        {
            new Point(1060, 615).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// выскочила ошибка 820?  /проверяем белый цвет фона сообщения об ошибке/
        /// </summary>
        /// <returns></returns>
        public bool isError820()
        {
            return new PointColor(1152, 601, 6908265, 0).isColor() &&
                    new PointColor(1152, 602, 6908265, 0).isColor()
                    ||
                    new PointColor(1151, 601, 6908265, 0).isColor() &&
                    new PointColor(1151, 602, 6908265, 0).isColor()
                    ||
                    new PointColor(1150, 601, 6908265, 0).isColor() &&
                    new PointColor(1150, 602, 6908265, 0).isColor()
                    ||
                    new PointColor(1106, 594, 3539040, 0).isColor() &&
                    new PointColor(1106, 595, 3539040, 0).isColor()
                    ;
        }

        /// <summary>
        /// закрываем сообщения об ошибке 820 нажатием на кнопку Ок
        /// </summary>
        public void CloseError820()
        {
            new Point(1106, 595).PressMouseL();
            Pause(500);
            new Point(1106, 605).PressMouseL();
        }

        /// <summary>
        /// выскочила ошибка с предложением отправить отчёт к разработчикам /проверяем букву Н в слове Нет/
        /// </summary>
        /// <returns></returns>
        public bool isUnexpectedError()
        {
            return new PointColor(1100, 609, 3473504, 0).isColor() &&
                    new PointColor(1100, 610, 96, 0).isColor()
                    ||
                    new PointColor(1127, 614, 3539040, 0).isColor() &&
                    new PointColor(1127, 615, 3539040, 0).isColor()
                    ;
        }

        /// <summary>
        /// закрываем сообщение об UnexpectedError нажатием на кнопку Нет
        /// </summary>
        public void CloseUnexpectedError()
        {
            new Point(1100, 609).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// проверяем, открыто ли окно Стим для ввода логина и пароля
        /// </summary>
        /// <returns></returns>
        protected bool isSteam()
        {
            return (pointisSteam1.isColor() && pointisSteam2.isColor());
        }

        /// <summary>
        /// если первый раз загружается стим на этом компе, то надо нажать "Принять",
        /// соглашаясь с пользовательским соглашение
        /// </summary>
        /// <returns>true, если первый раз загружается стим на этом компе</returns>
        public bool isNewSteam()
        {
            return (pointisNewSteam1.isColor() && pointisNewSteam2.isColor())
                    ||
                   (pointisNewSteam3.isColor() && pointisNewSteam4.isColor());
        }

        /// <summary>
        /// Нажимаем кнопку "Принять" и соглашаемся с пользовательским соглашением
        /// </summary>
        public void AcceptUserAgreement()
        {
            new Point(1052, 832).PressMouseL();
            Pause(2000);
        }

        protected bool isSystemError()
        {
            iPointColor pointisError1 = new PointColor(1142, 610, 3539040, 0);
            iPointColor pointisError2 = new PointColor(1142, 608, 3539040, 0);
            return pointisError1.isColor() && pointisError2.isColor();
        }

        protected void OkSystemError()
        {
            WriteToLogFile("системная ошибка");
            new Point(1142, 610).PressMouseL();
        }

        ///// <summary>
        ///// проверяем, выскочило ли сообщение, что аккаунт загружен на другом компе и кнопка "Продолжить запуск"
        ///// </summary>
        ///// <returns>true = выскочило сообщение</returns>
        //protected bool isContinueRunning()
        //{
        //    bool ff1 = pointisContinueRunning1.isColor();
        //    bool ff2 = pointisContinueRunning2.isColor();
        //    return pointisContinueRunning1.isColor() && pointisContinueRunning2.isColor();
        //}

        public abstract bool isContinueRunning();

        /// <summary>
        /// жмем кнопку "Отмена" (аккаунт загружен на другом компе) и убираем мышку в сторону 
        /// </summary>
        protected void NextAccount()
        {
            pointCancelContinueRunning.PressMouseLL();  // жмём кнопку отмена

            new Point(1300, 500).Move();  //убираем мышку в сторону

            //if (botParam.NumberOfInfinity > 0)     //если не обычный режим бота
            //{
            //    RemoveSandboxie();    // удаляем текущую песочницу и к счетчику NumberOfInfinity прибавляем единицу
            //    //botwindow.ReOpenWindow(); //перезапускаем окно

            //}
        }

        /// <summary>
        /// возвращает действующий логин
        /// </summary>
        /// <returns></returns>
        protected string GetLogin()
        {
            //return botParam.Logins[botParam.NumberOfInfinity];
            return botParam.Login;
        }
        /// <summary>
        /// возвращает действующий пароль
        /// </summary>
        /// <returns></returns>
        protected string GetPassword()
        {
            //return botParam.Passwords[botParam.NumberOfInfinity];
            return botParam.Password;
        }
        protected void EnterSteamLogin()
        {
            pointSteamLogin2.Drag(pointSteamLogin1); //выделяем старый пароль, если он есть
            Pause(1000);
            SendKeys.SendWait(GetLogin());
        }
        protected void EnterSteamPassword()
        {
            pointSteamPassword.PressMouseL();
            SendKeys.SendWait(GetPassword());
        }
        protected void CheckSteamSavePassword()
        {
            //if (globalParam.Infinity)
            //{
            //    //если ходим в Инфинити вместо обычного ботоводства, 

            //}
            //else   //обычное ботоводство
            //{
            //    pointSteamSavePassword.PressMouseL();
            //}
            if (botParam.NumberOfInfinity == 0) pointSteamSavePassword.PressMouseL(); //если обычный режим бота
        }

        /// <summary>
        /// ???
        /// </summary>
        protected void PressSteamOk()
        {
            pointSteamOk.PressMouseL();
            //botParam.NumberOfInfinity++;   //прибавили индекс, когда нажали Ок (загрузка аккаунта Стим)
        }


        /// <summary>
        /// Вводим в стим логин-пароль и нажимаем Ок
        /// </summary>
        protected void InsertLoginPasswordOk()
        {
            EnterSteamLogin();
            EnterSteamPassword();
            CheckSteamSavePassword();
            PressSteamOk();
            //WriteToLogFile(botParam.NumberOfInfinity + " "+ botParam.Logins[botParam.NumberOfInfinity] + 
            //" " + botParam.Passwords[botParam.NumberOfInfinity] + " " + botParam.Parametrs[botParam.NumberOfInfinity]);
        }




        /// <summary>
        /// удаляем текущую песочницу
        /// </summary>
        public void RemoveSandboxie()
        {
            int result = botParam.NumberOfInfinity + 1; //прибавили индекс, когда удаляем песочницу 
            if (result >= botParam.LengthOfList) result = 0;    //если номер аккаунта больше длины списка логинов, то присваиваем нулевой номер
            botParam.NumberOfInfinity = result;
            //new Point(1597, 1060).Move();   //перемещаем мышь вниз 
            MoveMouseDown();
            Pause(400);

            //вариант с песочницей
            Process process = new Process();
            //закрываем программы
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " /terminate";
            process.Start();
            Pause(5000);

            //удаляем содержимое песочницы
            //process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            //process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " delete_sandbox";
            //process.Start();

            ////вариант с чистым клиентом
            //Process process = new Process();
            //process.StartInfo.FileName = this.pathClient;
            //process.StartInfo.Arguments = " -shutdown";
            //process.Start();


            Pause(4000);

        }

        /// <summary>
        /// плавно перемещаем мышь вдоль нижней панели Виндовс
        /// </summary>
        public void MoveMouseDown()
        {
            for (int i = 1490; i < 1620; i += 50)
            {
                new Point(i, 1060).Move();   //перемещаем мышь по низу, чтобы скрыть лишние стимы с нижней панели Виндовс
                //Pause(500);
            }
        }

        /// <summary>
        /// удаляем текущую песочницу (для кнопки "BH")
        /// </summary>
        public void RemoveSandboxieBH()
        {
            //старый рабочий вариант
            //int result = globalParam.Infinity;
            //if (result >= 424) result = 0;  //если дошли до последнего подготовленного аккаунта, то идём в начало списка
            ////в качестве альтернативы тут можно сделать присвоение FAlse какому-нибудь глоб параметру, чтобы остановить общий цикл

            //новый вариант
            int result = globalParam.Infinity;
            if (result >= globalParam.DemonicTo + 1) result = globalParam.DemonicFrom;  //если дошли до последнего подготовленного аккаунта,
                                                                                        //то идём в начало отведённого списка

            // общий кусок кода
            botParam.NumberOfInfinity = result;
            globalParam.Infinity = result + 1;
            CloseSandboxieBH();
            MoveMouseDown();
        }

        /// <summary>
        /// закрываем программы в конкретной песочнице без перехода к следующему аккаунту (для БХ)
        /// </summary>
        public void CloseSandboxieBH()
        {
            //вариант с песочницей
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " /terminate";
            process.Start();
        }

        /// <summary>
        /// закрываем программы в конкретной песочнице
        /// </summary>
        public void CloseSandboxie()
        {
            new Point(1597, 1060).Move();   //перемещаем мышь вниз 
            Pause(400);

            //вариант с песочницей
            Process process = new Process();
            //закрываем программы
            process.StartInfo.FileName = @"C:\Program Files\Sandboxie\Start.exe";
            process.StartInfo.Arguments = @"/box:" + botwindow.getNumberWindow() + " /terminate";
            process.Start();
            Pause(5000);

            ////вариант с чистым клиентом
            //Process process = new Process();
            //process.StartInfo.FileName = this.pathClient;
            //process.StartInfo.Arguments = " -shutdown";
            //process.Start();

            //Pause(4000);
        }

        public abstract void RunClientClassic();

        public abstract void runClientSteamBH();
        public abstract void runClient();
        public abstract UIntPtr FindWindowGE();

        //public abstract UIntPtr FindWindowSteam();

        /// <summary>
        /// поиск окна Steam в текущей песочнице (в том числе для миссии в Demonic)
        /// </summary>
        /// <returns>true, если Steam найден</returns>
        public abstract bool FindWindowSteamBool();

        public abstract void OrangeButton();
        //public abstract bool isActive();

        /// <summary>
        /// поиск окна с ошибкой
        /// </summary>
        /// <returns>HWND найденного окна</returns>
        public abstract UIntPtr FindWindowError();



        #endregion

        #region Logout

        /// <summary>
        /// вводим логин и пароль в соответствующие поля
        /// </summary>
        protected void EnterLoginAndPasword()
        {
            iPoint pointPassword = new Point(510 - 5 + xx, 355 - 5 + yy);    //  505, 350
            // окно открылось, надо вставить логин и пароль
            pointPassword.PressMouseL();   //Кликаю в строчку с паролем
            //PressMouseL(505, 350);       //Кликаю в строчку с паролем
            Pause(500);
            pointPassword.PressMouseL();   //Кликаю в строчку с паролем
            //PressMouseL(505, 350);       //Кликаю в строчку с паролем
            Pause(500);
            SendKeys.SendWait("{TAB}");
            Pause(500);
            SendKeys.SendWait(botParam.Login);
            Pause(500);
            SendKeys.SendWait("{TAB}");
            Pause(500);
            SendKeys.SendWait(botParam.Password);
            Pause(500);
        }

        /// <summary>
        /// нажимаем на кнопку Connect (окно в логауте)
        /// </summary>
        /// <returns>true=если удалось нажать,  false=если окно вылетело</returns>
        private bool PressConnectButton()
        {
            iPoint pointButtonConnect = new Point(595 - 5 + xx, 485 - 5 + yy);    // кнопка "Connect" в логауте 
            if (isHwnd())
            {
                pointButtonConnect.PressMouseLL();   // Кликаю в Connect
                Pause(500);
                WriteToLogFileBH("Нажали на Коннект");
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Нажимаем Коннект (переводим бота из состояния логаут в состояние казарма)
        /// </summary>
        /// <returns></returns>
        public bool Connect()    // возвращает true, если успешно вошли в казарму
        {

            #region новый вариант
            //bool result = true;
            //const int MAX_NUMBER_ITERATION = 4;    //максимальное количество итераций
            //uint count = 0;

            //iPointColor testColor = new PointColor(65 - 5 + databot.X, 55 - 5 + databot.Y, 7800000, 5);  //запоминаем цвет в координатах 55, 55 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            //Pause(500);

            //do
            //{
            //    PressConnectButton();
            //    Pause(10000);
            //    if (isCheckBugs()) BugFixes();

            //    count++;
            //    if (count > MAX_NUMBER_ITERATION)
            //    {
            //        result = false;
            //        break;
            //    }
            //    //if (server.isBarack())
            //    //{
            //    //    result = true;
            //    //    break;
            //    //}
            //} while (!isChangeDisplay(testColor.GetPixelColor()));

            //return result;

            #endregion

            #region старый вариант (но рабочий)

            //server.WriteToLogFileBH("вошли в процедуру коннект");
            //server.WriteToLogFile(botParam.NumberOfInfinity + " " + botParam.Logins[botParam.NumberOfInfinity] + " " + botParam.Passwords[botParam.NumberOfInfinity] + " " + botParam.Parametrs[botParam.NumberOfInfinity]);
            //server.serverSelection();          //выбираем из списка свой сервер

            //iPointColor point5050 = new PointColor(50 - 5 + botParam.X, 50 - 5 + botParam.Y, 7800000, 5);  //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            //iPoint pointButtonOk = new Point(525 - 5 + botParam.X, 415 - 5 + botParam.Y);    // кнопка Ok. всплывающее сообщение в логауте
            //iPoint pointButtonOk2 = new Point(525 - 5 + botParam.X, 450 - 5 + botParam.Y);    // кнопка Ok. всплывающее сообщение  в логауте
            iPointColor point5050 = new PointColor(50 - 5 + xx, 50 - 5 + yy, 7800000, 5);  //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            iPoint pointButtonOk = new Point(525 - 5 + xx, 415 - 5 + yy);    // кнопка Ok. всплывающее сообщение в логауте
            iPoint pointButtonOk2 = new Point(525 - 5 + xx, 450 - 5 + yy);    // кнопка Ok. всплывающее сообщение  в логауте
            iPoint pointButtonOk3 = new Point(525 - 5 + xx, 432 - 5 + yy);    // кнопка Ok. всплывающее сообщение в логауте

            uint Tek_Color1;
            //bool ColorBOOL = true;
            uint currentColor;
            const int MAX_NUMBER_ITERATION = 4;  //максимальное количество итераций

            bool aa = true;

            uint Test_Color = point5050.GetPixelColor();       //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            Tek_Color1 = Test_Color;

            bool ColorBOOL = (Test_Color == Tek_Color1);
            int counter = 0; //счетчик

            pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК" (сообщение об ошибке 1)
            Pause(500);
            pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" (сообщение об ошибке 1)
            Pause(500);
            pointButtonOk3.PressMouseL();  //кликаю в кнопку  "ОК" (сообщение об ошибке 1)
            Pause(500);

            bool IsServerSelected = serverSelection();          //выбираем из списка свой сервер
            if (!IsServerSelected) return false; //если не смогли выбрать сервер, то выход из метода с результатом false

            WriteToLogFileBH("Функция Connect. дошли до while");
            while ((aa | (ColorBOOL)) & (counter < MAX_NUMBER_ITERATION))
            {
                counter++;  //счетчик

                Tek_Color1 = point5050.GetPixelColor();
                ColorBOOL = (Test_Color == Tek_Color1);

                WriteToLogFileBH("нажимаем на кнопку connect");

                if (PressConnectButton())
                {
                    Pause(1500);
                }
                else
                    return false;

                //если есть ошибки в логине-пароле, то возникает сообщение с кнопкой "OK". 

                if (isPointConnect())                                         // Обработка Ошибок.
                {
                    pointButtonOk.PressMouse();  //кликаю в кнопку  "ОК"
                    Pause(500);

                    if (isPointConnect())   //проверяем, выскочила ли форма с кнопкой ОК
                    {
                        pointButtonOk.PressMouse();  //кликаю в кнопку  "ОК"
                        Pause(500);
                    }
                    pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"

                    pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min

                    //EnterLoginAndPasword();
                }
                else
                {
                    aa = false;
                }

            }

            bool result = true;
            Pause(500);
            currentColor = point5050.GetPixelColor();
            if (currentColor == Test_Color)      //проверка входа в казарму, т.е. поменялся пиксель на экране логаута. 
                result = false;

            return result;

            #endregion

        }

        #region методы для нового варианта Connect

        /// <summary>
        /// исправление ошибок при нажатии кнопки Connect (бот в логауте)
        /// </summary>
        private void BugFixes()
        {
            iPoint pointButtonOk = new Point(525 - 5 + botParam.X, 410 - 5 + botParam.Y);    // кнопка Ok в логауте
            iPoint pointButtonOk2 = new Point(525 - 5 + botParam.X, 445 - 5 + botParam.Y);    // кнопка Ok в логауте

            pointButtonOk.PressMouse();   //кликаю в кнопку  "ОК"
            Pause(500);

            pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"  второй раз (их может быть две)
            Pause(500);

            pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" другой формы (где написано про 3 min)

            EnterLoginAndPasword();        //вводим логин и пароль заново
        }

        /// <summary>
        /// проверяем, есть ли проблемы после нажатия кнопки Connect (выскачила форма с кнопкой ОК)
        /// </summary>
        /// <returns></returns>
        private bool isCheckBugs()
        { return isPointConnect(); }

        /// <summary>
        /// проверяем, сменилось ли изображение на экране
        /// </summary>
        /// <param name="testColor">тестовая точка</param>
        /// <returns>true, если сменился экран</returns>
        private bool isChangeDisplay(uint testColor)
        {
            iPointColor currentColor = new PointColor(65 - 5 + botParam.X, 55 - 5 + botParam.Y, 7800000, 5);
            uint color = currentColor.GetPixelColor();
            bool result = (color == testColor);
            return !result;
        }

        #endregion

        /// <summary>
        /// Нажимаем Коннект (переводим бота из состояния логаут в состояние казарма)
        /// </summary>
        /// <returns></returns>
        public bool ConnectBH()    // возвращает true, если успешно вошли в казарму
        {

            bool result = true;
            const int MAX_NUMBER_ITERATION = 4;    //максимальное количество итераций
            uint count = 0;

            iPointColor testColor = new PointColor(65 - 5 + botParam.X, 55 - 5 + botParam.Y, 7800000, 5);  //запоминаем цвет в координатах 55, 55 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            Pause(500);

            do
            {
                PressConnectButton();
                Pause(10000);
                if (isCheckBugs()) BugFixes();

                count++;
                if (count > MAX_NUMBER_ITERATION)
                {
                    result = false;
                    break;
                }
                //if (server.isBarack())
                //{
                //    result = true;
                //    break;
                //}
            } while (!isChangeDisplay(testColor.GetPixelColor()));

            return result;

        }

        /// <summary>
        /// нажимаем кнопку Leave Game в Логауте
        /// </summary>
        public void LeaveGame()
        {
            new Point(605 - 5 + botParam.X, 535 - 5 + botParam.Y).PressMouseL();    // кнопка LeaveGame в логауте
        }

        /// <summary>
        /// метод проверяет, находится ли данное окно в режиме логаута, т.е. на стадии ввода логина-пароля
        /// </summary>
        /// <returns></returns>
        public bool isLogout()
        {
            return pointisLogout1.isColor() && pointisLogout2.isColor();
        }

        /// <summary>
        /// проверяем, есть ли ошибки при нажатии кнопки Connect
        /// </summary>
        /// <returns></returns>
        public bool isPointConnect()
        {
            return pointConnect.isColor();
        }

        /// <summary>
        /// переключились ли на нужный сервер FERRUCCIO-ESPADA (в режиме логаута)
        /// </summary>
        /// <returns></returns>
        public abstract bool IsServerSelection();

        /// <summary>
        /// выбираем сервер путем нажатия на соответствующую строчку в меню
        /// </summary>
        public bool serverSelection()
        {
            WriteToLogFileBH("выбираем сервер из списка серверов начало");
            int count = 0;
            while ((!IsServerSelection()) && (count < 5))
            {
                pointserverSelection.PressMouseLL();
                Pause(500);
                count++;
            }
            if (count < 5)
                return true;
            else
                return false;

            //WriteToLogFileBH("выбираем сервер из списка серверов конец метода");
        }

        /// <summary>
        /// нажимаем коннект без проверки результатов нажатия
        /// </summary>
        public void QuickConnect()
        {
            if (isLogout())
            {
                //нажимаем на кнопки, которые могут появиться из-за сбоев входа в игру
                new Point(520 - 5 + xx, 420 - 5 + yy).PressMouseL();    // кнопка Ok в логауте при ошибке
                Pause(500);
                new Point(525 - 5 + xx, 450 - 5 + yy).PressMouseL();    // кнопка Ok в логауте при ошибке
                Pause(500);
                new Point(526 - 5 + xx, 436 - 5 + yy).PressMouseL();    // кнопка Ok в логауте при ошибке
                Pause(500);

                serverSelection();
                PressConnectButton();
            }
        }

        ///// <summary>
        ///// нажимаем на кнопку Connect (окно в логауте)
        ///// </summary>
        //protected void PressConnectButton()
        //{
        //    iPoint pointButtonConnect = new Point(595 - 5 + xx, 485 - 5 + yy);    // кнопка коннект в логауте 
        //    pointButtonConnect.PressMouseLL();   // Кликаю в Connect
        //}

        #endregion

        #region Pet

        /// <summary>
        /// открыто ли меню с коллекцией доступных петов
        /// </summary>
        /// <returns></returns>
        public bool isOpenFamilyCollection()
        {
            return new PointColor(224 - 5 + xx, 156 - 5 + yy, 13200000, 5).isColor()
                && new PointColor(224 - 5 + xx, 157 - 5 + yy, 13200000, 5).isColor();
        }

        /// <summary>
        /// в открытом окне выбора пета (Family Collection) ищем пета Koko (цыплёнок)
        /// возвращает 0, если пет не найден в первых пяти строках
        /// </summary>
        /// <returns>номер строки, в которой стоит пет</returns>
        public int FindKoko()
        {
            int NumberString = -1;
            for (int i = 0; i <= 4; i++)
            {
                if (new PointColor(310 - 5 + xx, 278 - 5 + yy + i * 50, 13300000, 5).isColor() && 
                    new PointColor(310 - 5 + xx, 285 - 5 + yy + i * 50, 13300000, 5).isColor())
                {
                    NumberString = i;
                    break;
                }
            }
            return NumberString + 1;
        }

        /// <summary>
        /// в открытом окне выбора пета (Family Collection) ищем пета PigLing (поросёнок).
        /// возвращает 0, если пет не найден в первых пяти строках
        /// </summary>
        /// <returns>номер строки, в которой стоит пет</returns>
        private int FindPiggy()
        {
            int NumberString = -1;
            for (int i = 0; i <= 4; i++)
            {
                //if (new PointColor(305 - 5 + xx, 278 - 5 + yy + i * 50, 13300000, 5).isColor() &&
                //    new PointColor(305 - 5 + xx, 285 - 5 + yy + i * 50, 13300000, 5).isColor())
                if (new PointColor(264 - 5 + xx, 271 - 5 + yy + i * 50, 1200000, 5).isColor()) 
                {
                    NumberString = i;
                    break;
                }
            }
            return NumberString + 1;
        }

        /// <summary>
        /// в открытом окне "Family Collection" нажимаем кнопку "Cancel"
        /// </summary>
        private void PressCancelButton()
        {
            Pause(500);
            new Point(180 - 5 + xx, 555 - 5 + yy).PressMouseLL();       
            Pause(500);
        }

        /// <summary>
        /// в открытом окне "Family Collection" нажимаем кнопку "Summon"
        /// </summary>
        private void PressSummonButton()
        {
            new Point(180 - 5 + xx, 525 - 5 + yy).PressMouseLL();       //кнопка Summon
            Pause(500);
        }

        /// <summary>
        /// в открытом окне "Family Collection" выбираем пета в выбранной строке
        /// </summary>
        /// <param name="NumberString">номер строки</param>
        private void SelectPetString(int NumberString)
        {
            if (NumberString <= 0 || NumberString >= 5) NumberString = 1;   //если номер строки не в пределах от 1 до 5,
                                                                            //то берём первую строку (там поросёнок)
            new Point(270 - 5 + xx, 281 - 5 + yy + (NumberString - 1) * 50).PressMouseLL();     
            Pause(500);
        }

        /// <summary>
        /// призываем пета "поросёнок" через верхнее меню
        /// </summary>
        public void SummonPetPiggy()
        {
            if (!isSummonPetPiggy() && !isActivePetPiggy())
            {
                TopMenu(14, 7, true);

                if (isOpenFamilyCollection())
                {
                    PressCancelButton();

                    SelectPetString(FindPiggy());

                    PressSummonButton();
                }
                botwindow.PressEscThreeTimes();
            }
        }

        /// <summary>
        /// призываем пета через верхнее меню
        /// </summary>
        public void SummonPetKoko()
        {
            if (!isSummonPetKoko() && !isActivePetKoko())
            {
                TopMenu(14, 7, true);

                if (isOpenFamilyCollection())
                {
                    PressCancelButton();

                    SelectPetString(FindKoko());

                    PressSummonButton();
                }
                botwindow.PressEscThreeTimes();
            }
        }

        /// <summary>
        /// призываем пета через верхнее меню
        /// </summary>
        public void SummonPet()
        {
            SummonPetPiggy();
            //SummonPetKoko();
        }

        /// <summary>
        /// проверяет, активирован ли пет поросёнок (т.е. пет призван и активирован = иконка поросёнка розовая)  10/02/24
        /// </summary>
        /// <returns></returns>
        public bool isActivePetPiggy()
        {
            return pointisActivePet1.isColor() && pointisActivePet2.isColor();
        }

        /// <summary>
        /// проверяет, активирован ли пет цыплёнок (т.е. пет призван и активирован = иконка цыплёнка жёлтая)  10/02/24
        /// </summary>
        /// <returns></returns>
        public bool isActivePetKoko()
        {
            return new PointColor(384 - 5 + xx, 92 - 5 + yy, 8642801, 0).isColor() &&
                   new PointColor(385 - 5 + xx, 92 - 5 + yy, 8052981, 0).isColor();
        }

        /// <summary>
        /// проверяет, активирован ли пет (т.е. пет призван и активирован = иконка поросёнка розовая)  10/02/24
        /// </summary>
        /// <returns></returns>
        public bool isActivePet()
        {
            return isActivePetPiggy() || isActivePetKoko();
        }

        /// <summary>
        /// активируем уже призванного пета нажатием на пиктограмму пета          10/02/24
        /// </summary>
        public void ActivatePet()
        {
            new Point(385 - 5 + xx, 88 - 5 + yy).PressMouseL();  //тыкаем в пиктограмму пета вверху слева экрана
        }

        /// <summary>
        /// проверяет, призван ли поросёнок (т.е. пет призван, но не активирован = иконка поросёнка серая)  10/02/24
        /// </summary>
        /// <returns> true, если призван </returns>
        public bool isSummonPetPiggy()
        {
            return pointisSummonPet1.isColor() && pointisSummonPet2.isColor();
        }

        /// <summary>
        /// проверяет, призван ли цыплёнок (т.е. пет призван, но не активирован = иконка цыплёнка серая)  12-03-24
        /// </summary>
        /// <returns> true, если призван </returns>
        public bool isSummonPetKoko()
        {
            return new PointColor(384 - 5 + xx, 92 - 5 + yy, 14408667, 0).isColor() &&
                   new PointColor(385 - 5 + xx, 92 - 5 + yy, 14408667, 0).isColor();
        }

        /// <summary>
        /// проверяет, призван ли пет (т.е. пет призван, но не активирован = иконка поросёнка серая)  10/02/24
        /// </summary>
        /// <returns> true, если призван </returns>
        public bool isSummonPet()
        {
            return isSummonPetPiggy() || isSummonPetKoko();
        }

        /// <summary>
        /// активируем пета через пиктограмму 
        /// считается, что изначально пет не активирован
        /// </summary>
        public void ActivatePetDem()
        {
            //новый вариант
            if (!isActivePet())
            {
                if (!isSummonPet()) SummonPet();
                Pause(500);
                ActivatePet();
            }
        }


        #endregion

        #region Top Menu 

        /// <summary>
        /// открыто ли меню с выбором канала
        /// </summary>
        /// <returns>true, если открыто</returns>
        public bool isOpenMenuChooseChannel()
        {
            return pointisOpenMenuChooseChannel1.isColor() && pointisOpenMenuChooseChannel2.isColor();
        }

        /// <summary>
        /// является ли текущий канал первым каналом?
        /// </summary>
        /// <returns>true, если первый</returns>
        public bool CurrentChannel_is_1()
        {
            //return pointIsCurrentChannel1.isColor() && pointIsCurrentChannel2.isColor();
            //return pointIsCurrentChannel1.GetPixelColor() == pointIsCurrentChannel2.GetPixelColor();  //если цвета в обоих точках совпадают, значит это первый канал
            //return  ( pointIsCurrentChannel1.isColor() || pointIsCurrentChannel3.isColor() ) && 
            //        ( pointIsCurrentChannel2.isColor() || pointIsCurrentChannel4.isColor() );
            return pointIsCurrentChannel1.isColor() && pointIsCurrentChannel2.isColor();

        }

        /// <summary>
        /// В меню выбора канала выбрать строку с указанным номером и кнопка "Move Now"
        /// </summary>
        /// <param name="NumberOfString">номер строки в списке номеров каналов</param>
        public void MenuChooseChannel(int NumberOfString)
        {
            iPoint pointChooseChannel = new Point(571 - 5 + xx, 286 - 5 + yy + (NumberOfString - 1) * 15);
            iPoint pointMoveBotton = new Point(444 - 5 + xx, 491 - 5 + yy);

            pointChooseChannel.PressMouseLL();
            pointMoveBotton.PressMouseLL();

        }

        /// <summary>
        /// метод проверяет, открылось ли верхнее меню 
        /// </summary>
        /// <param name="numberOfThePartitionMenu"></param>
        /// <returns> true, если меню открылось </returns>
        public bool isOpenTopMenu(int numberOfThePartitionMenu)
        {
            bool result = false;
            switch (numberOfThePartitionMenu)
            {
                case 2:
                    //result = (pointisOpenTopMenu21.isColor() && pointisOpenTopMenu22.isColor());
                    break;
                case 5:
                    result = pointisOpenTopMenu51.isColor() && pointisOpenTopMenu52.isColor(); //телепорт 22-11
                    break;
                case 6:
                    //result = pointisOpenTopMenu61.isColor() && pointisOpenTopMenu62.isColor();
                    break;
                case 8:
                    //result = (pointisOpenTopMenu81.isColor() && pointisOpenTopMenu82.isColor());
                    break;
                case 9:
                    result = pointisOpenTopMenu91.isColor() && pointisOpenTopMenu92.isColor(); //системное меню 22-11
                    break;
                case 12:
                    result = (pointisOpenTopMenu121.isColor() && pointisOpenTopMenu122.isColor()) ||
                            (pointisOpenTopMenu121work.isColor() && pointisOpenTopMenu122work.isColor());  //карты   22/11/23
                    break;
                case 13:
                    result = pointisOpenTopMenu131.isColor2() && pointisOpenTopMenu132.isColor2();
                    break;
                case 14:
                    result = pointisOpenTopMenu141.isColor() && pointisOpenTopMenu142.isColor();   //пет   10/02/24
                    break;
                case 16:
                    result = pointisOpenTopMenu161.isColor2() && pointisOpenTopMenu162.isColor2();
                    break;
                default:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// открыт ли список телепортов
        /// </summary>
        /// <returns></returns>
        public bool isOpenWarpList()
        {
            return isOpenTopMenu(5);
        }

        /// <summary>
        /// Открыть городской телепорт (Alt + F3) без проверок и while (для паттерна Состояние)  StateGT  //=== точно не работает ===
        /// </summary>
        public void OpenTownTeleportForState()
        {
            TopMenu(6, 3);
            Pause(1000);
        }

        /// <summary>
        /// Открыть карту местности (Alt + Z) на работе
        /// </summary>
        public void OpenMapForState()
        {
            TopMenu(12, 2, true);
            Pause(500);
            //new Point(197 - 5 + xx, 144 - 5 + yy).PressMouseL();
            //Pause(500);
        }

        /// <summary>
        /// Выгружаем окно через верхнее меню 
        /// </summary>
        public void GoToEnd()
        {
            systemMenu(7, true);
        }

        /// <summary>
        /// Выгружаем окно через верхнее меню 
        /// </summary>
        public void Logout()
        {
            systemMenu(6, true);
        }

        /// <summary>
        /// Идем в казармы через верхнее меню 
        /// </summary>
        public void GotoBarack()
        {
            systemMenu(4, true);
        }

        /// <summary>
        /// Идем в начальный город через верхнее меню 
        /// </summary>
        public void GotoSavePoint()
        {
            systemMenu(3, true);
        }

        /// <summary>
        /// переход по системному меню (для БХ)
        /// </summary>
        /// <param name="number"> номер пункта меню </param>
        public void systemMenu(int number, bool status)
        {
            if (!isOpenTopMenu(9) && status)
            {
                TopMenu(9, status);       //если не открыто системное меню, то открыть
                Pause(500);
            }
            if (isOpenTopMenu(9))
                systemMenuPressCurrentLine(number);
        }

        /// <summary>
        /// нажать на выбранную строку системного меню
        /// </summary>
        /// <param name="number"> номер пункта меню </param>
        public void systemMenuPressCurrentLine(int number)
        {
            new Point(685 - 5 + xx, 291 - 5 + (number - 1) * 30 + yy).PressMouseL();   //нажимаем на указанный пункт меню
            Pause(500);
        }


        /// <summary>
        /// телепортируемся в город продажи по Alt+W (улетаем из БХ или другого города)     /21-12-23/
        /// </summary>
        public void TeleportAltW_BH()
        {
            TeleportAltW(botwindow.getNomerTeleport());
        }

        /// <summary>
        /// нажмает на выбранный раздел верхнего меню     ===== 22-11-2023 =======
        /// </summary>
        /// <param name="numberOfThePartitionMenu"></param>
        /// <param name="status">если false, то не проверяется сработало ли нажатие</param>
        public void TopMenu(int numberOfThePartitionMenu, bool status)
        {
            //int[] MenukoordX = { 305, 339, 371, 402, 435, 475, 522, 570, 610, 642, 675, 705, 738 };
            int[] MenukoordX = { 105, 137, 169, 201, 233, 265, 297, 329, 105, 137, 169, 201, 233, 265, 297, 329 };
            int[] MenukoordY = { 55, 55, 55, 55, 55, 55, 55, 55, 87, 87, 87, 87, 87, 87, 87, 87 };
            int x = MenukoordX[numberOfThePartitionMenu - 1];
            int y = MenukoordY[numberOfThePartitionMenu - 1];

            iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy); //кнопка с нужными координатами

            int count = 0;
            do
            {
                pointMenu.PressMouseL();
                botwindow.Pause(500);           //было 1000
                count++; if (count > 3) break;
            } while ((!isOpenTopMenu(numberOfThePartitionMenu)) && status);
        }

        /// <summary>
        /// нажать на выбранный раздел верхнего меню, а далее на пункт раскрывшегося списка
        /// </summary>
        /// <param name="numberOfThePartitionMenu">номер раздела верхнего меню п/п</param>
        /// <param name="punkt">пункт меню. номер п/п</param>
        public void TopMenu(int numberOfThePartitionMenu, int punkt, bool status)
        {
            int[] numberOfPunkt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 9, 0, 5 };  //количество пунктов меню в соответствующем разделе
            int[] PunktX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 197, 0, 262, 0, 325 };    //координата X первого подпункта меню
            int[] FirstPunktOfMenuKoordY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 118, 0, 118, 0, 118 }; //координата Y первого пункта меню

            if (punkt <= numberOfPunkt[numberOfThePartitionMenu - 1])
            {
                int x = PunktX[numberOfThePartitionMenu - 1];
                int y = FirstPunktOfMenuKoordY[numberOfThePartitionMenu - 1] + 25 * (punkt - 1);

                TopMenu(numberOfThePartitionMenu, status);   //сначала открываем раздел верхнего меню (1-14)
                Pause(500);

                iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy);
                pointMenu.PressMouseL();  //выбираем конкретный пункт подменю (в раскрывающемся списке)
            }
        }

        /// <summary>
        /// проверяем, открыт ли мировой телепорт (Alt+W)  (буква M в слове Managed)
        /// </summary>
        /// <returns>true, если открыт</returns>
        public bool isOpenWorldTeleport()
        {
            return new PointColor(747 - 5 + xx, 252 - 5 + yy, 13000000, 6).isColor() &&
                   new PointColor(747 - 5 + xx, 260 - 5 + yy, 13000000, 6).isColor();
        }

        /// <summary>
        /// телепортируемся в город по Alt+W. Актуальный             /21-12-23/
        /// </summary>
        /// <param name="NumberOfTown">номер телепорта, начиная сверху</param>
        public void TeleportAltW(int NumberOfTown)
        {
            if (NumberOfTown > 15) NumberOfTown = 1;              //если номер телепорта больше 15, то летим в Ребольдо

            while (!isOpenWorldTeleport())
            {
                if (isTown() || isWork())               //защита от бесконечного цикла
                {
                    TopMenu(12, 1, true);
                    Pause(1000);
                }
                else
                    break;
            }
            if (isOpenWorldTeleport())
                new Point(700 - 5 + xx, 282 - 5 + yy + (NumberOfTown - 1) * 31).DoubleClickL();
        }

        /// <summary>
        /// открываем список телепоров, проверяем, есть ли телепорт в Катовию и летим туда             //не работает
        /// </summary>
        /// <returns>true, если улетели. false, если не улетели, т.к. нет нужного телепорта</returns>
        public bool TeleportKatovia()
        {
            bool result = false;

            TopMenu(12, true);                     // Click Teleport menu
            Pause(500);

            if (isKatoviaTeleport())
            {
                Point pointTeleportNumbertLine = new Point(405 - 5 + xx, 180 - 5 + (3 - 1) * 15 + yy);    //    

                pointTeleportNumbertLine.DoubleClickL();   // Третья строка в списке телепортов
                Pause(500);

                pointTeleportExecute.PressMouseL();        // Click on button Execute in Teleport menu

                result = true;
            }
            else
            {
                Pause(500);
            }

            return result;
        }

        /// <summary>
        /// проверяем, является ли третий телепорт телепортом в Катовию             //не работает
        /// </summary>
        /// <returns>true, если является</returns>
        public bool isKatoviaTeleport()
        {
            return new PointColor(348 - 5 + xx, 205 - 5 + yy, 16711422, 0).isColor()
                && new PointColor(348 - 5 + xx, 209 - 5 + yy, 16711422, 0).isColor()
                && new PointColor(348 - 5 + xx, 213 - 5 + yy, 16711422, 0).isColor();
        }

        /// <summary>
        /// в открытом меню телепортов выбираем нужную строчку и летим по выбранному телепорту  ===== 22-11-2023 =======
        /// </summary>
        /// <param name="NumberOfLine">номер строки в списке сохраненных телепортов</param>
        private void FlyByTeleport(int NumberOfLine)
        {
            Point pointTeleportNumbertLine = new Point(132 - 5 + xx, 200 - 5 + (NumberOfLine - 1) * 15 + yy);    //    вычисляем точку на экране, куда тыкать

            pointTeleportNumbertLine.DoubleClickL();   // Указанная строка в списке телепортов
            Pause(500);

            pointTeleportExecute.PressMouseL();        // Click on button Execute in Teleport menu (нажимаем на кнопку, чтобы перейти по телепорту)
        }

        /// <summary>
        /// вызываем телепорт через верхнее меню и телепортируемся по указанному номеру телепорта 
        /// </summary>
        /// <param name="NumberOfLine"></param>
        /// <param name="status">если false, то не проверяется сработало ли нажатие</param>
        public void Teleport(int NumberOfLine, bool status)
        {
            TopMenu(5, status);                     // Click Teleport menu. открываем меню со списком сохранённых телепортов
            Pause(500);

            FlyByTeleport(NumberOfLine);
        }

        /// <summary>
        /// тыкаем в первую строчку списка телепортов (меню телепортов уже открыто)
        /// </summary>
        public void Teleport1()
        {
            FlyByTeleport(1);
        }

        public abstract void Teleport(int numberOfLine);
        public abstract void TopMenu(int numberOfThePartitionMenu);                 //не работает
        public abstract void TopMenu(int numberOfThePartitionMenu, int punkt);      //не работает
        public abstract void TeleportToTownAltW(int i);

        #endregion

        #region Shop

        ///// <summary>
        ///// проверяет, находится ли данное окно в магазине (а точнее на странице входа в магазин) 
        ///// </summary>
        ///// <returns> true, если находится в магазине </returns>
        //public bool isSale()
        //{
        //    //uint bb = pointIsSale1.GetPixelColor();
        //    //uint dd = pointIsSale2.GetPixelColor();
        //    return ((pointIsSale1.isColor()) && (pointIsSale2.isColor()));
        //}

        ///// <summary>
        ///// проверяет, находится ли данное окно в самом магазине (на закладке BUY или SELL)                                       
        ///// </summary>
        ///// <returns> true, если находится в магазине </returns>
        //public bool isSale2()
        //{
        //    //uint bb = pointIsSale21.GetPixelColor();
        //    //uint dd = pointIsSale22.GetPixelColor();
        //    return ((pointIsSale21.isColor()) && (pointIsSale22.isColor()));
        //    //return botwindow.isColor2(841 - 5, 665 - 5, 7390000, 841 - 5, 668 - 5, 7390000, 4);
        //}

        ///// <summary>
        ///// проверяет, открыта ли закладка Sell в магазине 
        ///// </summary>
        ///// <returns> true, если закладка Sell в магазине открыта </returns>
        //public bool isClickSell()
        //{
        //    //uint bb = pointIsClickSale1.GetPixelColor();
        //    //uint dd = pointIsClickSale2.GetPixelColor();
        //    return ((pointIsClickSale1.isColor()) && (pointIsClickSale2.isColor()));
        //}

        ///// <summary>
        ///// Кликаем в закладку Sell  в магазине 
        ///// </summary>
        //public void Bookmark_Sell()
        //{
        //    pointBookmarkSell.DoubleClickL();
        //    Pause(1500);
        //}

        ///// <summary>
        ///// проверяем, является ли товар в первой строке магазина маленькой красной бутылкой
        ///// </summary>
        ///// <param name="numberOfString">номер строки, в которой проверяем товар</param>
        ///// <returns> true, если в первой строке маленькая красная бутылка </returns>
        //public bool isRedBottle(int numberOfString)
        //{
        //    PointColor pointFirstString = new PointColor(147 - 5 + xx, 224 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0);
        //    return pointFirstString.isColor();
        //}

        ///// <summary>
        ///// добавляем товар из указанной строки в корзину 
        ///// </summary>
        ///// <param name="numberOfString">номер строки</param>
        //public void AddToCart(int numberOfString)
        //{
        //    Point pointAddProduct = new Point(380 - 5 + botwindow.getX(), 220 - 5 + (numberOfString - 1) * 27 + botwindow.getY());
        //    pointAddProduct.PressMouseL();
        //    pointAddProduct.PressMouseWheelDown();
        //}

        ///// <summary>
        ///// определяет, анализируется ли нужный товар либо данный товар можно продавать
        ///// </summary>
        ///// <param name="color"> цвет полностью определяет товар, который поступает на анализ </param>
        ///// <returns> true, если анализируемый товар нужный и его нельзя продавать </returns>
        //public bool NeedToSellProduct(uint color)
        //{
        //    bool result = true;

        //    switch (color)                                             // Хорошая вещь или нет, сверяем по картотеке
        //    {
        //        case 394901:      // soul crystal                **
        //        case 3947742:     // красная бутылка 1200HP      **
        //        case 2634708:     // красная бутылка 2500HP      **
        //        case 7171437:     // devil whisper               **
        //        case 5933520:     // маленькая красная бутылка   **
        //        case 1714255:     // митридат                    **
        //        case 7303023:     // чугун                       **
        //        case 4487528:     // honey                       **
        //        case 1522446:     // green leaf                  **
        //        case 2112641:     // red leaf                    **
        //        case 1533304:     // yelow leaf                  **
        //        case 13408291:    // shiny                       **
        //        case 3303827:     // карта перса                 **
        //        case 6569293:     // warp                        **
        //        case 662558:      // head of Mantis              **
        //        case 4497887:     // Mana Stone                  **
        //        case 7305078:     // Ящики для джеков            **
        //        case 15420103:    // Бутылка хрина               **
        //        case 9868940:     // композитная сталь           **
        //        case 5334831:     // магическая сфера            **
        //        case 13164006:    // свекла                      **
        //        case 16777215:    // Wheat flour                 **
        //        case 13565951:    // playtoken                   **
        //        case 10986144:    // Hinge                       **
        //        case 3481651:     // Tube                        **
        //        case 6593716:     // Clock                       **
        //        case 13288135:    // Spring                      **
        //        case 7233629:     // Cogwheel                    **
        //        case 13820159:    // Family Support Token        **
        //        case 4222442:     // Wolf meat                   **
        //        case 4435935:     // Yellow ore                  **
        //        case 5072004:     // Bone Stick                   **
        //        case 3559777:     // Dragon Lether                   **
        //        case 1712711:     // Dragon Horn                   **
        //        case 6719975:     // Wild Boar Meat                   **
        //        case 4448154:     // Green ore                   **
        //        case 13865807:    // blue ore                   **
        //        case 4670431:     // Red ore                 **
        //        case 13291199:    // Diamond Ore                   **
        //        case 1063140:     // Stone of Philos                   **
        //        case 8486756:     // Ice Crystal                  **
        //        case 4143156:     // bulk of Coal                   **
        //        case 9472397:     // Steel piece                 **
        //        case 7187897:     // Mustang ore
        //        case 1381654:     // стрелы эксп
        //        case 11258069:     // пули эксп
        //        case 2569782:     // дробь эксп
        //        case 5137276:     // сундук деревянный как у сфер древней звезды

        //            //              case 7645105:     // Quartz                 **
        //            result = false;
        //            break;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Посылаем нажатие числа 333 в окно с ботом с помощью команды PostMessage
        ///// </summary>
        //public void Press333()
        //{
        //    const int WM_KEYDOWN = 0x0100;
        //    const int WM_KEYUP = 0x0101;
        //    UIntPtr lParam = new UIntPtr();
        //    UIntPtr HWND = FindWindowEx(botwindow.getHwnd(), UIntPtr.Zero, "Edit", "");   // это handle дочернего окна ге, т.е. области, где можно писать циферки

        //    for (int i = 1; i <= 3; i++)
        //    {
        //        uint dd = 0x00400001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYDOWN, (UIntPtr)Keys.D3, lParam);
        //        Pause(150);

        //        dd = 0xC0400001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYUP, (UIntPtr)Keys.D3, lParam);
        //        Pause(150);
        //    }
        //}

        ///// <summary>
        ///// Посылаем нажатие числа 44444 в окно с ботом с помощью команды PostMessage
        ///// </summary>
        //public void Press44444()
        //{
        //    const int WM_KEYDOWN = 0x0100;
        //    const int WM_KEYUP = 0x0101;
        //    UIntPtr lParam = new UIntPtr();
        //    UIntPtr HWND = FindWindowEx(botwindow.getHwnd(), UIntPtr.Zero, "Edit", "");   // это handle дочернего окна ге, т.е. области, где можно писать циферки

        //    for (int i = 1; i <= 5; i++)
        //    {
        //        uint dd = 0x00500001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYDOWN, (UIntPtr)Keys.D4, lParam);
        //        Pause(150);

        //        dd = 0xC0500001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYUP, (UIntPtr)Keys.D4, lParam);
        //        Pause(150);
        //    }
        //}

        ///// <summary>
        ///// добавляем товар из указанной строки в корзину 
        ///// </summary>
        ///// <param name="numberOfString">номер строки</param>
        //public void GoToNextproduct(int numberOfString)
        //{
        //    Point pointAddProduct = new Point(380 - 5 + botwindow.getX(), 225 - 5 + (numberOfString - 1) * 27 + botwindow.getY());
        //    pointAddProduct.PressMouseWheelDown();   //прокручиваем список

        //}

        ///// <summary>
        ///// добавляем товар из указанной строки в корзину 
        ///// </summary>
        ///// <param name="numberOfString">номер строки</param>
        //public void AddToCartLotProduct(int numberOfString)
        //{
        //    Point pointAddProduct = new Point(360 - 5 + botwindow.getX(), 220 - 5 + (numberOfString - 1) * 27 + botwindow.getY());  //305 + 30, 190 + 30)
        //    pointAddProduct.PressMouseL();  //тыкаем в строчку с товаром
        //    Pause(150);
        //    SendKeys.SendWait("33000");
        //    Pause(100);
        //    //Press44444();                   // пишем 444444 , чтобы максимальное количество данного товара попало в корзину 
        //    pointAddProduct.PressMouseWheelDown();   //прокручиваем список
        //}

        ///// <summary>
        ///// Продажа товаров в магазине вплоть до маленькой красной бутылки 
        ///// </summary>
        //public void SaleToTheRedBottle()
        //{
        //    uint count = 0;
        //    while (!isRedBottle(1))
        //    {
        //        AddToCart(1);
        //        count++;
        //        if (count > 220) break;   // защита от бесконечного цикла
        //    }
        //}

        ///// <summary>
        ///// Продажа товара после маленькой красной бутылки, до момента пока прокручивается список продажи
        ///// </summary>
        //public void SaleOverTheRedBottle()
        //{
        //    Product previousProduct;
        //    Product currentProduct;

        //    currentProduct = new Product(xx, yy, 1);  //создаем структуру "текущий товар" из трёх точек, которые мы берем у товара в первой строке магазина

        //    do
        //    {
        //        previousProduct = currentProduct;
        //        if (NeedToSellProduct(currentProduct.color1))
        //            AddToCartLotProduct(1);
        //        else
        //            GoToNextproduct(1);

        //        Pause(200);  //пауза, чтобы ГЕ успела выполнить нажатие. Можно и увеличить     
        //        currentProduct = new Product(xx, yy, 1);
        //    } while (!currentProduct.EqualProduct(previousProduct));          //идет проверка по трем точкам
        //}

        ///// <summary>
        ///// Продажа товара, когда список уже не прокручивается 
        ///// </summary>
        //public void SaleToEnd()
        //{
        //    iPointColor pointCurrentProduct;
        //    for (int j = 2; j <= 12; j++)
        //    {
        //        pointCurrentProduct = new PointColor(149 - 5 + xx, 219 - 5 + yy + (j - 1) * 27, 3360337, 0);   //проверяем цвет текущего продукта
        //        Pause(50);
        //        if (NeedToSellProduct(pointCurrentProduct.GetPixelColor()))       //если нужно продать товар
        //            AddToCartLotProduct(j);                                       //добавляем в корзину весь товар в строке j
        //    }
        //}

        ///// <summary>
        ///// Кликаем в кнопку BUY  в магазине 
        ///// </summary>
        //public void Botton_BUY()
        //{
        //    pointButtonBUY.PressMouseL();
        //    pointButtonBUY.PressMouseL();
        //    Pause(2000);
        //}

        ///// <summary>
        ///// Кликаем в кнопку Sell  в магазине 
        ///// </summary>
        //public void Botton_Sell()
        //{
        //    pointButtonSell.PressMouseL();
        //    pointButtonSell.PressMouseL();
        //    Pause(2000);
        //}

        ///// <summary>
        ///// Кликаем в кнопку Close в магазине
        ///// </summary>
        //public void Button_Close()
        //{
        //    pointButtonClose.PressMouse();
        //    Pause(2000);
        //    //PressMouse(847, 663);
        //}

        ///// <summary>
        ///// Покупка митридата в количестве 333 штук
        ///// </summary>
        //public void BuyingMitridat()
        //{
        //    //botwindow.PressMouseL(360, 537);          //кликаем левой кнопкой в ячейку, где надо написать количество продаваемого товара
        //    pointBuyingMitridat1.PressMouseL();             //кликаем левой кнопкой в ячейку, где надо написать количество продаваемого товара
        //    Pause(150);

        //    //Press333();
        //    SendKeys.SendWait("333");

        //    Botton_BUY();                             // Нажимаем на кнопку BUY 


        //    pointBuyingMitridat2.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если переполнение митридата
        //    //botwindow.PressMouseL(1392 - 875, 438 - 5);         
        //    Pause(500);

        //    pointBuyingMitridat3.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если нет денег на покупку митридата
        //    //botwindow.PressMouseL(1392 - 875, 428 - 5);          
        //    Pause(500);
        //}

        #endregion

        #region atWork

        public abstract bool is248Items();

        public abstract void AddBullet10000();

        /// <summary>
        /// неправильная стойка у мушкетёра или Бернелли (не флинтлок)?
        /// </summary>
        /// <returns> true, если неправильная </returns>
        public bool isBadFightingStance()
        {
            //return pointisBadFightingStance1.isColor() && pointisBadFightingStance2.isColor();
            return false;
        }

        /// <summary>
        /// Выбрать правильную стойку (Флинтлок)
        /// </summary>
        public void ProperFightingStanceOn()
        {
            pointProperFightingStance.PressMouseLL();  //включить правильную стойку (Флинтлок)
        }

        /// <summary>
        /// метод проверяет, закончились ли экспертные патроны (красный значок индикатора)
        /// </summary>
        /// <returns> true, если красный значок </returns>
        public bool isBulletOff()
        {
            return pointisBulletOff1.isColor() || pointisBulletOff2.isColor() || pointisBulletOff3.isColor();
        }

        /// <summary>
        /// метод проверяет, ополовинились ли экспертные патроны (желтый значок индикатора)
        /// </summary>
        /// <returns> true, если желтый значок </returns>
        public bool isBulletHalf()
        {
            return pointisBulletHalf1.isColor() || pointisBulletHalf2.isColor() || pointisBulletHalf3.isColor();
        }

        /// <summary>
        /// метод проверяет, переполнился ли карман (выскочило ли уже сообщение о переполнении)
        /// </summary>
        /// <returns> true, еслм карман переполнен </returns>
        public bool isBoxOverflow()
        {
            return
                 //(pointisBoxOverflow1.isColor() && pointisBoxOverflow2.isColor()) ||     //всплывающее окно на экране
                 pointisBoxOverflow3.isColor() && pointisBoxOverflow4.isColor();          //оранжевая надпись (эта строка)
        }

        /// <summary>
        /// убит ли первый герой?
        /// </summary>
        /// <returns>true, если убит</returns>
        public bool isKillFirstHero()
        {
            return !(new PointColor(81 - 5 + xx, 641 - 5 + yy, 4930000, 4).isColor() &&
                   new PointColor(82 - 5 + xx, 641 - 5 + yy, 4930000, 4).isColor());
            //pointisKillHero1.isColor();
        }
        /// <summary>
        /// убит ли первый герой?
        /// </summary>
        /// <returns>true, если убит</returns>
        public bool isKillFirstHero2()
        {
            return pointisKillHero1.isColor();
        }


        /// <summary>
        /// функция проверяет, убит ли i-й герой из пати (проверка проходит на карте)
        /// </summary>
        /// <returns></returns>
        public bool isKillHero(int i)
        {
            bool result = false;
            switch (i)
            {
                case 1:
                    result = pointisKillHero1.isColor();
                    break;
                case 2:
                    result = pointisKillHero2.isColor();
                    break;
                case 3:
                    result = pointisKillHero3.isColor();
                    break;
            }
            return result;
        }

        /// <summary>
        /// функция проверяет, убит ли хоть один герой из пати (проверка проходит на карте)
        /// </summary>
        /// <returns></returns>
        public bool isKillHero()
        {
            return pointisKillHero1.isColor() || pointisKillHero2.isColor() || pointisKillHero3.isColor();
        }

        /// <summary>
        /// функция проверяет, убиты ли все герои
        /// </summary>
        /// <returns></returns>
        public bool isKillAllHero()
        {
            return (pointisKillHero1.isColor() && pointisKillHero2.isColor() && pointisKillHero3.isColor());
        }

        ///// <summary>
        ///// метод проверяет, находится ли данное окно на работе (проверка по стойке)  две стойки
        ///// </summary>
        ///// <returns> true, если сейчас на рабочей карте </returns>
        //public bool isWork()
        //{
        //    bool resultRifle = (pointisWork_RifleDot1.isColor() && pointisWork_RifleDot2.isColor());
        //    bool resultExpRifle = (pointisWork_ExpRifleDot1.isColor() && pointisWork_ExpRifleDot2.isColor());
        //    bool resultDrob = (pointisWork_DrobDot1.isColor() && pointisWork_DrobDot2.isColor());
        //    bool resultVetDrob = (pointisWork_VetDrobDot1.isColor() && pointisWork_VetDrobDot2.isColor());
        //    bool resultExpDrob = (pointisWork_ExpDrobDot1.isColor() && pointisWork_ExpDrobDot2.isColor());
        //    bool resultJainaDrob = (pointisWork_JainaDrobDot1.isColor() && pointisWork_JainaDrobDot2.isColor());
        //    bool resultVetSabre = (pointisWork_VetSabreDot1.isColor() && pointisWork_VetSabreDot2.isColor());
        //    bool resultExpSword = (pointisWork_ExpSwordDot1.isColor() && pointisWork_ExpSwordDot2.isColor());
        //    bool resultVetPistol2 = (pointisWork_VetPistolDot1.isColor() && pointisWork_VetPistolDot2.isColor());
        //    bool resultVetPistol1 = (pointisWork_SightPistolDot1.isColor() && pointisWork_SightPistolDot2.isColor());
        //    bool resultExpPistol = (pointisWork_UnlimPistolDot1.isColor() && pointisWork_UnlimPistolDot2.isColor());
        //    bool resultExpCannon = (pointisWork_ExpCannonDot1.isColor() && pointisWork_ExpCannonDot2.isColor());

        //    return (resultRifle || resultExpRifle || resultDrob || resultVetDrob || resultExpDrob || resultVetSabre || resultExpSword || resultJainaDrob || resultVetPistol2 || resultVetPistol1 || resultExpPistol || resultExpCannon);  //проверка только по первому персу
        //}

        /// <summary>
        /// проверяем, находимся ли на работе
        /// </summary>
        /// <returns></returns>
        public bool isWork()
        {
            //проверяем одну и ту же точку на экране в миссии
            //каждой стойке соответствует свой цвет этой точки
            //проверяем две точки для надежности

            // в массиве arrayOfColorsIsWork1 хранятся все цвета первой контрольной точки, которые есть у проверяемых стоек 
            // в массиве arrayOfColorsIsWork2 хранятся все цвета второй контрольной точки, которые есть у проверяемых стоек 

            uint color1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем

            return this.arrayOfColorsIsWork1.Contains(color1) && this.arrayOfColorsIsWork2.Contains(color2);                // проверяем, есть ли цвет контрольной точки в массивах цветов
        }


        ///// <summary>
        ///// метод проверяет является ли первый перс поваром
        ///// </summary>
        ///// <returns> true, если сейчас на рабочей карте </returns>
        //public bool isCook()
        //{
        //    return (pointisWork_VetSabreDot1.isColor() && pointisWork_VetSabreDot2.isColor());
        //}

        /// <summary>
        /// нажимаем на спец. умение повару
        /// </summary>
        public void ClickSkillCook()
        {
            pointSkillCook.PressMouseL();
        }

        /// <summary>
        /// проверяем, включён ли боевой режим (пробел)
        /// </summary>
        /// <returns>true, если включён</returns>
        public bool isBattleMode()
        {
            return pointisBattleMode1.isColor() && pointisBattleMode2.isColor();
        }

        /// <summary>
        /// проверяем, включён ли штурмовой режим (Ctrl+Click)
        /// </summary>
        /// <returns>true, если включён</returns>
        public bool isAssaultMode()
        {
            return pointisAssaultMode1.isColor() && pointisAssaultMode2.isColor();
        }

        /// <summary>
        /// проверяем, включён ли режим подбора лута
        /// </summary>
        /// <returns>true, если включён</returns>
        public bool isHarvestMode()
        {
            return new PointColor(114 - 5 + xx, 516 - 5 + yy, 9366000, 3).isColor() &&
                   new PointColor(142 - 5 + xx, 544 - 5 + yy, 9760000, 3).isColor();
        }


        ///// <summary>
        ///// проверяем, вЫключен ли боевой режим
        ///// </summary>
        ///// <returns></returns>
        //public bool isBattleModeOff()
        //{
        //    return (pointisBattleModeOff1.isColor() && pointisBattleModeOff2.isColor());
        //}

        /// <summary>
        /// Переход в боевой режим
        /// </summary>
        public void BattleModeOn()
        {
            iPoint pointBattleMode = new Point(190 - 5 + xx, 530 - 5 + yy);
            if (!isBattleMode())
            {
                if (!globalParam.Windows10)            //сделал для Наташи, чтобы у нее не нажимался пробел (Боевой режим)
                {
                    pointBattleMode.PressMouseL();  // Кликаю на кнопку "боевой режим"
                    Pause(500);
                }
            }
        }

        /// <summary>
        /// Переход в боевой режим для Демоника (пробел)
        /// </summary>
        public void BattleModeOnDem()
        {
            new Point(190 - 5 + xx, 530 - 5 + yy).PressMouseL();  // Кликаю на кнопку "боевой режим"   22-11
        }

        #endregion

        #region inTown

        public bool isOkButton()
        {
            return new PointColor(939 - 5 + xx, 650 - 5 + yy, 11800000, 5).isColor() &&
                   new PointColor(939 - 5 + xx, 651 - 5 + yy, 11800000, 5).isColor();
            //pointisKillHero1.isColor();
        }

        /// <summary>
        /// нажимаем Ок после вызова мушкетёра
        /// </summary>
        public void PressOkButton()
        {
            new Point(940 - 5 + xx, 652 - 5 + yy).PressMouseL();
        }


        /// <summary>
        /// вызов нового мушкетёра
        /// </summary>
        public void CallMusk()
        {
            new Point(694 - 5 + xx, 381 - 5 + yy).PressMouseL();
            Pause(1000);
            PressOkButton();
        }

        /// <summary>
        /// запускаем следующий раунд
        /// </summary>
        public void NextRound()
        {
            new Point(694 - 5 + xx, 436 - 5 + yy).PressMouseL();
            if (isOkButton()) PressOkButton();
        }

        /// <summary>
        /// нажимаем на машинку для вызова капибар
        /// </summary>
        public void PressMashina()
        {
            new Point(347 - 5 + xx, 419 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// комплекс действий в миссии с капибарами
        /// </summary>
        private void MissionCapibara()
        {
            Pause(1000);
            MinHeight(7);
            Pause(1000);
            botwindow.ThirdHero();
            Pause(1000);

            for (int i = 1; i <= 2; i++)
            {
                PressMashina();
                Pause(1000);

                CallMusk();
                Pause(1000);
            }

            //перебор раундов
            for (int i = 1; i <= 10; i++)
            {
                if (isReboldo() || isLogout()) break;
                PressMashina();
                Pause(1000);
                NextRound();
                Pause(20000);

                for (int j = 1; j <= 5; j++)
                {
                    if (isReboldo() || isLogout()) break;

                    PressMashina();
                    Pause(1000);

                    CallMusk();
                    Pause(6000);
                }
            }
            Pause(20000);
        }


        /// <summary>
        /// в уже открытой карте Ребольдо идём к персонажу в строке numberString
        /// </summary>
        public void GotoPersonOnMapReboldo(int numberString)
        {
            new Point(812 - 5 + xx, 94 + (numberString - 1) * 15 - 5 + yy).DoubleClickL();   //тыкаем в нужную строку (в списке справа от карты)
            Pause(500);
            new Point(940 - 5 + xx, 734 - 5 + yy).DoubleClickL();   //тыкаем в кнопку Move Now
            Pause(500);
            botwindow.PressEscThreeTimes();
            Pause(10000);
        }

        /// <summary>
        /// подходим к Event Game Console
        /// </summary>
        private void GoToEventGameConsole()
        {
            botwindow.PressEscThreeTimes();
            Pause(1000);
            TopMenu(12, 2, true);                       //открываем карту города (LocalMap)
            Pause(1000);
            GotoPersonOnMapReboldo(8);
        }

        /// <summary>
        /// нажимаем на Event Game Console
        /// </summary>
        private void PressEventGameConsole()
        {
            //new Point(670 - 5 + xx, 349 - 5 + yy).PressMouseL();
            new Point(670 - 5 + xx, 384 - 5 + yy).PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// диалог в Event Game Console, чтобы войти в миссию
        /// </summary>
        private void DialogInEventGameConsole()
        {
            dialog.PressStringDialog(4);
            Pause(1000);
            dialog.PressStringDialog(1);
            if (dialog.isDialog()) dialog.PressStringDialog(1);
            Pause(33000);
        }

        //private void GotoBarackAndReboldo() 
        //{
        //    Pause(2000);
        //    GotoBarack();
        //    Pause(10000);
        //    FromBarackToTown(3);                        // barack --> town
        //    Pause(30000);
        //}
    

        public void CapibaraComplex()
        {
            Pause(2000);
            while (true)
            {
                if (isLogout())
                {
                    QuickConnect();
                    Pause(10000);
                }

                if (isBarack())
                {
                    FromBarackToTown(3);                        // barack --> town  
                    Pause(20000);
                }

                GoToEventGameConsole();
                PressEventGameConsole();
                DialogInEventGameConsole();
                MissionCapibara();
                Pause(2000);

                GotoBarack();
                Pause(10000);
            }
        }


        /// <summary>
        /// проверяем, закончились ли награды в окне Achievement
        /// </summary>
        /// <returns>true, если закончились</returns>
        public bool isEmptyReward()
        {
            return new PointColor(939 - 5 + xx + 5, 177 - 5 + yy + 5, 5848111, 0).isColor();
        }

        /// <summary>
        /// получаем одну награду в окне Achievement (Alt+L)
        /// </summary>
        public void ReceiveTheReward()
        {
            new Point(939 - 5 + xx + 5, 177 - 5 + yy + 5).PressMouseL();
            MoveCursorOfMouse();
            Pause(1000);
        }

        /// <summary>
        /// проверяем цвет пиктограммы "Receive The Reward". Если серый, то мы на странице получения наград
        /// </summary>
        /// <returns>true, если серый</returns>
        public bool isReceiveReward()

        {
            return new PointColor(408 - 5 + xx + 5, 559 - 5 + yy + 5, 10197915, 0).isColor();
        }

        /// <summary>
        /// открываем страницу получения награды в Achievement (Alt+L)
        /// (нажимаем на пиктограмму "Receive The Reward")
        /// </summary>
        public void ToBookmarkReceiveTheReward()
        {
            new Point(406 - 5 + xx + 5, 559 - 5 + yy + 5).PressMouseLL();
            MoveCursorOfMouse();
        }

        /// <summary>
        /// проверяем, открыто ли окно Achievement (Alt+L)
        /// </summary>
        /// <returns>true, если открыто</returns>
        public bool isOpenAchievement()
        {
            //return new PointColor(380 - 5 + xx, 182 - 5 + yy, 8000000, 6).isColor() &&
            //        new PointColor(380 - 5 + xx, 183 - 5 + yy, 8000000, 6).isColor();
            return new PointColor(380 - 5 + xx + 5, 182 - 5 + yy + 5, 7000000, 6).isColor() ||
                    new PointColor(380 - 5 + xx + 5, 182 - 5 + yy + 5, 8000000, 6).isColor();
        }

        /// <summary>
        /// нажимаем на голову Lucia 
        /// </summary>
        public void PressHeadOfLucia()
        {
            new Point(349 - 5 + xx, 445 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// нажимаем на место немного в стороне от Lucia 
        /// </summary>
        public void PressLucia1()
        {
            new Point(471 - 5 + xx, 404 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// нажимаем на Lucia на карте города (чувак с какашками)
        /// </summary>
        public void PressLuciaOnMap()
        {
            //вариант 1. тыкаем непосредственно в карту
            //new Point(457 - 5 + xx, 409 - 5 + yy).PressMouseL();

            //вариант 2. тыкаем в список NPC справа от карты
            new Point(830 - 5 + xx, 295 - 5 + yy).DoubleClickL();

        }

        /// <summary>
        /// проверяем, открыто ли хотя бы одно задание (список справа экрана)
        /// </summary>
        /// <returns></returns>
        public bool isTask()
        {
            return new PointColor(1008 - 5 + xx, 509 - 5 + yy, 15335423, 0).isColor();
        }

        /// <summary>
        /// закрываем задания крестиком
        /// </summary>
        public void TaskOff()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (isTask())
                {
                    //закрываем задание
                    new Point(1011 - 5 + xx, 512 - 5 + yy).PressMouseL();
                    new Point(500 - 5 + xx, 300 - 5 + yy).Move();
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// проверяем, открыт ли журнал с подарками
        /// </summary>
        /// <returns></returns>
        public bool isPioneerJournal()
        {
            //return new PointColor(361 - 5 + xx, 82 - 5 + yy, 8549475, 0).isColor() && new PointColor(362 - 5 + xx, 82 - 5 + yy, 8549475, 0).isColor();
            return new PointColor(285 - 5 + xx, 134 - 5 + yy, 13890000, 4).isColor() 
                && new PointColor(285 - 5 + xx, 135 - 5 + yy, 13890000, 4).isColor();    //буква l в слове Select
        }

        /// <summary>
        /// получение подарков из журнала
        /// </summary>
        public void GetGiftsNew()
        {
            for (int i = 0; i <= 4; i++)
            {
                new Point(735 - 5 + xx, 254 - 5 + yy + i * 97).PressMouseL();
                Pause(1000);
            }
            //botwindow.PressEsc(4);
        }


        /// <summary>
        /// получение подарков из журнала
        /// </summary>
        public void GetGifts()
        {
            bool result = false;  //изначально считаем, что нет готовых подарков для получения
            for (int i = 1; i <= 5; i++)
            {
                iPointColor pointgifts = new PointColor(709 - 5 + xx, 229 - 5 + yy + (i - 1) * 97, 16000000, 5);
                result = result || pointgifts.isColor();
            }
            if (!result) botwindow.PressEsc();      //если журнал с подарками не открыт, то жмём Esc, чтобы убрать рекламу

            result = true;
            while (result)
            {
                result = false;
                for (int i = 1; i <= 5; i++)
                {
                    iPointColor pointgifts = new PointColor(709 - 5 + xx, 229 - 5 + yy + (i - 1) * 97, 16000000, 5);
                    result = result || pointgifts.isColor();

                    if (pointgifts.isColor())
                    {
                        new Point(709 - 5 + xx, 229 - 5 + yy + (i - 1) * 97).PressMouseL();
                        Pause(1000);
                    }
                }
            }
        }

        /// <summary>
        /// проверяем, открыто ли окно с подарочными окнами
        /// </summary>
        /// <returns></returns>
        public bool isToken()
        {
            //uint bb = pointisOpenMenuPet1.GetPixelColor();
            //uint dd = pointisOpenMenuPet2.GetPixelColor();
            return (pointisToken1.isColor() && pointisToken2.isColor());
        }

        /// <summary>
        /// закрываем окно с подарочными токенами
        /// </summary>
        public void TokenClose()
        {
            pointToken.PressMouse();
        }

        ///// <summary>
        ///// метод проверяет, находится ли данное окно в городе (проверка по стойкам - серые в городе, цветные - на рабочих картах) 
        ///// </summary>
        ///// <returns> true, если бот находится в городе </returns>
        //public bool isTown()
        //{
        //    //ружье
        //    bool resultRifle = (pointIsTown_RifleFirstDot1.isColor() && pointIsTown_RifleFirstDot2.isColor());
        //    bool resultRifleExp = (pointIsTown_ExpRifleFirstDot1.isColor() && pointIsTown_ExpRifleFirstDot2.isColor());
        //    //дробовик
        //    bool resultShotgun = (pointIsTown_DrobFirstDot1.isColor() && pointIsTown_DrobFirstDot2.isColor());           //проверка по первому персу обычная стойка
        //    bool resultShotgunVet = (pointIsTown_VetDrobFirstDot1.isColor() && pointIsTown_VetDrobFirstDot2.isColor());  //проверка по первому персу вет стойка
        //    bool resultShotgunExp = (pointIsTown_ExpDrobFirstDot1.isColor() && pointIsTown_ExpDrobFirstDot2.isColor());  //проверка по первому персу эксп стойка
        //    bool resultShotgunJaina = (pointIsTown_JainaDrobFirstDot1.isColor() && pointIsTown_JainaDrobFirstDot2.isColor());  //проверка по первому персу Джаина
        //    //сабля
        //    bool resultVetSabre = (pointIsTown_VetSabreFirstDot1.isColor() && pointIsTown_VetSabreFirstDot2.isColor());  //проверка по первому персу вет сабля
        //    //меч
        //    bool resultExpSword = (pointIsTown_ExpSwordFirstDot1.isColor() && pointIsTown_ExpSwordFirstDot2.isColor());  //проверка по первому персу эксп меч 
        //    //пистолет
        //    bool resultVetPistol2 = (pointIsTown_VetPistolFirstDot1.isColor() && pointIsTown_VetPistolFirstDot2.isColor());   //два пистолета
        //    bool resultVetPistol1 = (pointIsTown_SightPistolFirstDot1.isColor() && pointIsTown_SightPistolFirstDot2.isColor());   //один пистолет
        //    bool resultExpPistol = (pointIsTown_UnlimPistolFirstDot1.isColor() && pointIsTown_UnlimPistolFirstDot2.isColor());   //один пистолет
        //    //пушка Миса
        //    bool resultExpCannon = (pointIsTown_ExpCannonFirstDot1.isColor() && pointIsTown_ExpCannonFirstDot2.isColor());   //пушка Миса

        //    return (resultRifle || resultRifleExp || resultShotgun || resultShotgunVet || resultShotgunExp || resultVetSabre || resultExpSword || resultShotgunJaina || resultVetPistol2 || resultVetPistol1 || resultExpPistol || resultExpCannon);
        //}   !!!!!!!!!!!!!!!!!!!!!! СТАРЫЙ СПОСОБ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        /// <summary>
        /// проверяем, находимся ли в городе
        /// </summary>
        /// <returns></returns>
        public bool isTown()
        {
            //проверяем одну и ту же точку на экране в миссии
            //каждой стойке соответствует свой цвет этой точки
            //проверяем две точки для надежности

            // в массиве arrayOfColorsIsTown1 хранятся все цвета первой контрольной точки, которые есть у проверяемых стоек 
            // в массиве arrayOfColorsIsTown2 хранятся все цвета второй контрольной точки, которые есть у проверяемых стоек 

            //uint color1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            //uint color2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color1 = new PointColor(34 - 5 + xx, 702 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color2 = new PointColor(35 - 5 + xx, 702 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем

            return this.arrayOfColorsIsTown1.Contains(color1) && this.arrayOfColorsIsTown2.Contains(color2);                // проверяем, есть ли цвет контрольной точки в массивах цветов
        }

        /// <summary>
        /// лечение персов нажатием на красную бутылку и выпивание бутылок маны
        /// </summary>
        public void Cure(int n)
        {
            for (int j = 1; j <= n; j++)
            {
                pointCure1.PressMouseL();
                pointMana1.PressMouseL();

                pointCure2.PressMouseL();
                pointMana2.PressMouseL();

                pointCure3.PressMouseL();
                pointMana3.PressMouseL();
            }
            Pause(500);

            iPoint pointFourthBox = new Point(31 - 5 + xx, 200 - 5 + yy);
            pointFourthBox.PressMouseL();                // тыкаю в  (четвертая ячейка)

        }

        /// <summary>
        /// выпиваем бутылки с маной, которые лежат в ячейках для лечения (U,J,M)
        /// </summary>
        public void ManaForDemonic()
        {
            pointCure1.PressMouseL();
            pointCure2.PressMouseL();
            pointCure3.PressMouseL();
        }

        /// <summary>
        /// применение коробок с шайниками, лежащими в ячейках U, J, M
        /// </summary>
        public void AddShinyCrystal(int n)
        {
            for (int j = 1; j <= n; j++)
            {
                pointCure1.PressMouseL();
                pointCure2.PressMouseL();
                pointCure3.PressMouseL();
            }
            Pause(500);
        }

        /// <summary>
        /// "пьём" патроны. Применение коробок патронов в трёх ячейках с маной
        /// </summary>
        public void AddBullets()
        {
            pointMana1.PressMouseL();
            pointMana2.PressMouseL();
            pointMana3.PressMouseL();
        }


        /// <summary>
        /// "быстрое лечение". Применение коробок патронов в ячейках с маной
        /// </summary>
        public void QuickCure()
        {
            pointMana1.PressMouseL();
            //pointMana2.PressMouseL();
            //pointMana3.PressMouseL();
        }

        /// <summary>
        /// идём к высокой бабе GM (уже не актуально. ивент закончился)
        /// </summary>
        public abstract void GotoGM();
        public abstract void PressToHeadGM();

        #endregion

        #region Алхимия

        /// <summary>
        /// проверяем, закончились ли ингредиенты для алхимии (проверяем по две точки на каждую из трех ячеек алхимии)
        /// </summary>
        /// <returns></returns>
        public bool isOutOfIngredients()
        {
            return (pointisOutOfIngredient1_1.isColor() && pointisOutOfIngredient1_2.isColor());
        }

        /// <summary>
        /// проверяем, закончились ли ингредиенты для алхимии (проверяем по две точки на каждую из трех ячеек алхимии)
        /// </summary>
        /// <returns></returns>
        public bool isOutOfMoney()
        {
            return (pointOutOfMoney1.isColor() && pointOutOfMoney2.isColor());
        }


        /// <summary>
        /// проверяет, заполнился ли инвентарь при производстве на алхимическом столе
        /// </summary>
        /// <returns></returns>
        public bool isInventoryFull()
        {
            return (pointisInventoryFull1.isColor() && pointisInventoryFull2.isColor());
        }

        /// <summary>
        /// нажимаем кнопку "Start Alchemy" на алхимическом столе
        /// </summary>
        /// <returns></returns>
        public void PressButtonAlchemy()
        {
            pointAlchemy.PressMouseL();
        }

        /// <summary>
        /// проверяем, открыт ли алхимический стол
        /// </summary>
        /// <returns></returns>
        public bool isAlchemy()
        {
            return pointisAlchemy1.isColor() && pointisAlchemy2.isColor();
        }

        #endregion

        #region Barack

        /// <summary>
        /// создание новой команды в бараке
        /// </summary>
        public void CreateNewTeam()
        {
            TeamSelection(1);   //выбираем первую группу персов

            pointNameOfTeam.PressMouseL();      //тыкаем в строку, где надо вводить имя группы героев
            Pause(1500);

            SendKeys.SendWait("HighMasters");
            Pause(1500);
            pointButtonSaveNewTeam.PressMouseL();
            Pause(2500);

        }

        /// <summary>
        /// находимся ли на странице создания нового персонажа
        /// </summary>
        /// <returns></returns>
        public bool isBarackCreateNewHero()
        {
            return pointisBarack5.isColor() && pointisBarack6.isColor();
        }

        /// <summary>
        /// нажать кнопку "To Barack" на странице создания нового персонажа
        /// </summary>
        public void ButtonToBarack()
        {
            pointToBarack.PressMouseL();
        }

        /// <summary>
        /// нажимаем на кнопку логаут в казарме, тем самым покидаем казарму
        /// </summary>
        public void buttonExitFromBarack()
        {
            pointButtonLogoutFromBarack.DoubleClickL();
            Pause(500);

        }

        /// <summary>
        /// выбор команды персов из списка в казарме
        /// </summary>
        public void TeamSelection()
        {
            pointTeamSelection1.PressMouse();   // Нажимаем кнопку вызова списка групп
            pointTeamSelection2.PressMouseL();  // выбираем нужную группу персов (первую в списке)
            pointTeamSelection3.PressMouseL();  // Нажимаем кнопку выбора группы (Select Team) 
        }

        /// <summary>
        /// выбор команды персов из списка в казарме
        /// </summary>
        public void TeamSelection(int numberTeam)
        {
            pointTeamSelection1.PressMouseL();   // Нажимаем кнопку вызова списка групп
            Pause(500);
            new Point(110 - 5 + xx, 360 - 5 + (numberTeam - 1) * 20 + yy).DoubleClickL();  // выбираем нужную группу персов 
            Pause(500);
            pointTeamSelection3.PressMouseLL();  // Нажимаем кнопку выбора группы (Select) 
        }

        /// <summary>
        /// сдвиг для правильного выбора канала
        /// </summary>
        /// <returns></returns>
        public int sdvig()
        {
            return sdvigY;
        }

        /// <summary>
        /// проверяем, в бараках ли бот
        /// </summary>
        /// <returns> true, если бот в бараках </returns>
        public bool isBarack()
        {
            return
                    //(pointisBarack1.isColor() && pointisBarack2.isColor()) || 
                    (pointisBarack3.isColor() && pointisBarack4.isColor());
        }

        /// <summary>
        /// проверяем, выскочило ли сообщение, где надо нажать кнопку "Yes"
        /// для подтверждения того, что начинаем с города
        /// (сообщение возникает после выхода из миссий на мосту или БХ)
        /// </summary>
        /// <returns> true, если выскочило </returns>
        public bool isBarackWarningYes()
        {
            return new PointColor(564 - 5 + xx, 435 - 5 + yy, 11900000, 5).isColor() &&
                    new PointColor(564 - 5 + xx, 436 - 5 + yy, 11900000, 5).isColor();       //буква N в слове No
            //return new PointColor(482 - 5 + xx, 314 - 5 + yy, 6700000, 5).isColor() &&
            //        new PointColor(483 - 5 + xx, 314 - 5 + yy, 6700000, 5).isColor();
        }

        /// <summary>
        /// Нажимаем кнопку Yes для подтверждения того, что начинаем с города 
        /// (сообщение возникает после выхода из миссий на мосту или БХ)
        /// </summary>
        public void PressYesBarack()
        {
            new Point(471 - 5 + xx, 437 - 5 + yy).PressMouseL();   // Нажимаем кнопку Yes
        }


        /// <summary>
        /// проверяем, в бараках ли бот (на стадии выбора группы)
        /// </summary>
        /// <returns> true, если бот в бараках на стадии выбора группы  </returns>
        public bool isBarackTeamSelection()
        {
            return pointisBarackTeamSelection1.isColor() && pointisBarackTeamSelection2.isColor();
        }

        /// <summary>
        /// начать с выхода в город (нажать на кнопку "начать с нового места")
        /// </summary>
        public void NewPlace()
        {
            iPoint pointNewPlace = new Point(59 - 5 + xx, 690 - 5 + yy);
            pointNewPlace.DoubleClickL();
        }

        /// <summary>
        /// начинаем со старого места (из барака)
        /// </summary>
        public void barackLastPoint()
        {
            pointLastPoint.PressMouseL();
            botwindow.Pause(1000);
            botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку 

            if (isBarackWarningYes()) PressYesBarack();
            botwindow.Pause(500);
        }

        /// <summary>
        /// проверяем, попадём ли мы в BH, если в бараке начинаем со старого места
        /// </summary>
        public bool isBarackLastPoint()
        {
            //            pointLastPoint.PressMouseR();    // наводим мышку на кнопку Last Point в бараке
            pointLastPoint.Move();    // наводим мышку на кнопку Last Point в бараке
            return (pointisBHLastPoint1.isColor() && pointisBHLastPoint2.isColor());
        }

        /// <summary>
        /// нажать на кнопку "Create" в казарме
        /// </summary>
        public void PressCreateButton()
        {
            pointCreateButton.PressMouseLL();
            Pause(1000);
        }

        #endregion

        #region создание новых ботов

        /// <summary>
        /// бежим к петэксперту
        /// </summary>
        public void RunToPetExpert()
        {
            botwindow.ThirdHero();
            Pause(500);
            pointPetExpert.PressMouseL();            //тыкнули в эксперта по петам
            Pause(3000);
            pointPetExpert2.PressMouseL();            //тыкнули в эксперта по петам второй раз
            Pause(3000);

            //pointPetExpert2.PressMouseL();            //тыкнули в эксперта по петам второй раз
            //Pause(3000);

        }

        /// <summary>
        /// диалог с петэкспертом
        /// </summary>
        public void DialogPetExpert()
        {
            //pointFirstStringDialog.PressMouseL();       //нижняя строчка в диалоге
            //Pause(1500);
            //ButtonOkDialog.PressMouseL();               // Нажимаем на Ok в диалоге
            //Pause(1500);
            dialog.PressStringDialog(2);
            dialog.PressOkButton(1);

            //pointFirstStringDialog.PressMouseL();       //нижняя строчка в диалоге
            //Pause(1500);

            //ButtonOkDialog.PressMouseL();               // Нажимаем на Ok в диалоге
            //Pause(1500);
            dialog.PressStringDialog(1);
            dialog.PressOkButton(1);

            pointThirdBookmark.PressMouseL();           //тыкнули в третью закладку в кармане
            Pause(1500);

            pointPetBegin.Drag(pointPetEnd);            // берем коробку с кокошкой и переносим в куб для вылупления у петэксперта
            //botwindow.MouseMoveAndDrop(800-5, 220-5, 520-5, 330-5);            // берем коробку с кокошкой и переносим в куб для вылупления у петэксперта
            botwindow.Pause(2500);

            pointNamePet.PressMouseL();                // Нажимаем на строчку, где надо написать имя пета
            Pause(1500);

            SendKeys.SendWait("Koko");                 //вводим имя пета
            Pause(1500);

            pointButtonNamePet.PressMouseL();          // Нажимаем на строчку, где надо написать имя пета
            Pause(1500);

            pointButtonClosePet.PressMouseL();         // Нажимаем на строчку, где надо написать имя пета
            Pause(1500);

            ////жмем Ок 3 раза
            //for (int j = 1; j <= 3; j++)
            //{
            //    ButtonOkDialog.PressMouse();           // Нажимаем на Ok в диалоге
            //    Pause(1500);
            //}
            dialog.PressOkButton(3);

            Pause(2500);
        }


        /// <summary>
        /// диалог с Линдоном для получения лицензии
        /// </summary>
        public void LindonDialog2()
        {
            //жмем Ок 30 раз
            //for (int j = 1; j <= 30; j++)
            //{
            //    ButtonOkDialog.PressMouse();           // Нажимаем на Ok в диалоге
            //    Pause(1500);
            //}
            dialog.PressOkButton(30);
        }


        /// <summary>
        /// бежим к линдону после Доминго
        /// </summary>
        public void RunToLindon2()
        {
            botwindow.FirstHero();
            Pause(500);

            MaxHeight(7);
            Pause(500);

            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1500);

            pointLindonOnMap.PressMouseL();            //выбрали Линдона
            Pause(1000);

            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(30000);

            botwindow.PressEscThreeTimes();
            Pause(1000);

            pointPressLindon2.PressMouseL();           //нажимаем на голову Линдона
            Pause(3000);


        }

        /// <summary>
        /// бежим к Линдону
        /// </summary>
        public void RunToDomingo2()
        {
            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1500);

            pointDomingoOnMap.PressMouseL();           //выбрали Доминго
            Pause(1000);

            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(10000);

            botwindow.PressEscThreeTimes();
            Pause(1000);

            pointPressDomingo2.PressMouseL();           //нажимаем на голову Доминго
            Pause(3000);


        }

        /// <summary>
        /// диалог с Доминго после миссии
        /// </summary>
        public void DomingoDialog2()
        {
            //жмем Ок 9 раз
            //for (int j = 1; j <= 9; j++)
            //{
            //    ButtonOkDialog.PressMouse();           // Нажимаем на Ok в диалоге
            //    Pause(1500);
            //}
            dialog.PressOkButton(9);
        }

        /// <summary>
        /// Миссия у Доминго
        /// </summary>
        public void MissionDomingo()
        {
            TopMenu(6, 2);
            Pause(1000);

            pointDomingoMiss.PressMouseR();
            Pause(5000);


            botwindow.PressEscThreeTimes();
            Pause(180000);

        }

        /// <summary>
        /// диалог с Доминго
        /// </summary>
        public void DomingoDialog()
        {
            //жмем Ок 3 раза
            //for (int j = 1; j <= 3; j++)
            //{
            //    ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
            //    Pause(1500);
            //}
            dialog.PressOkButton(3);

            //pointFirstStringDialog.PressMouse();   //YES
            //Pause(1000);

            dialog.PressStringDialog(1);


            dialog.PressOkButton(1);
            //            ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
            //Pause(1000);

            dialog.PressStringDialog(2);
            //pointSecondStringDialog.PressMouse();   //YES во второй раз
            //Pause(1000);

            dialog.PressOkButton(1);

            //ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
            Pause(10000);


        }

        /// <summary>
        /// бежим к доминго
        /// </summary>
        public void RunToDomingo()
        {
            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1000);

            pointDomingoOnMap.PressMouseL();           //выбрали Доминго
            Pause(1000);

            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(33000);

            botwindow.PressEscThreeTimes();
            Pause(1000);

            pointPressDomingo.PressMouseL();           //нажимаем на голову Доминго
            Pause(2000);

        }

        /// <summary>
        /// надеваем оружие и броню
        /// </summary>
        public void Arm()
        {
            //открываем карман со спецвещами
            TopMenu(8, 2);
            Pause(1000);

            //надеваем обмундирование на первого перса, тыкая в первую вещь 25 раз (хватило бы 21, но с запасом)
            for (int j = 1; j <= 23; j++)
            {
                pointFirstItem.DoubleClickL();      //нажимаем дважды на первую вещь в спецкармане
                Pause(1000);
            }

            //надеваем обмундирование на второго перса, тыкая в первую вещь 17 раз (хватило бы 14, но с запасом)
            botwindow.SecondHero();
            Pause(1000);
            for (int j = 1; j <= 15; j++)
            {
                pointFirstItem.DoubleClickL();
                Pause(1000);
            }

            //надеваем обмундирование на третьего перса, тыкая в первую вещь 10 раз (хватило бы 7, но с запасом)
            botwindow.ThirdHero();
            Pause(1000);
            for (int j = 1; j <= 8; j++)
            {
                pointFirstItem.DoubleClickL();
                Pause(1000);
            }

            Pause(2000);
            botwindow.PressEscThreeTimes();
            Pause(1000);

        }

        /// <summary>
        /// говорим с солдатом для получения оружия
        /// </summary>
        public void TalkToSoldier()
        {
            pointPressSoldier.PressMouseL();  //нажимаем на голову солдата
            Pause(2000);

            pointFirstStringSoldier.PressMouseL();   //нажимаем на первую строчку в диалоге
            Pause(1500);

            ButtonOkDialog.PressMouseL();    // Нажимаем на Ok в диалоге
            Pause(1500);

            pointRifle.PressMouseL();    // Нажимаем на ружье
            Pause(500);
            pointRifle.PressMouseL();    // Нажимаем на ружье
            Pause(500);
            pointRifle.PressMouseL();    // Нажимаем на ружье
            Pause(500);

            //крутим колесо мыши вниз 35 раз
            for (int j = 1; j <= 35; j++)
            {
                pointRifle.PressMouseWheelDown();
                Pause(500);
            }

            pointCoat.PressMouseL();    // Нажимаем на плащ
            Pause(500);
            pointCoat.PressMouseL();    // Нажимаем на плащ
            Pause(500);
            pointCoat.PressMouseL();    // Нажимаем на плащ
            Pause(1000);

            pointButtonPurchase.PressMouseL();
            Pause(2000);
            pointButtonYesSoldier.PressMouseL();
            Pause(2000);
            pointButtonCloseSoldier.PressMouseL();
            Pause(3000);

        }

        /// <summary>
        /// говорим с GM для получения бижутерии и купонов на оружие-броню
        /// </summary>
        public void TalkToGM()
        {
            Pause(1000);
            //нажимаем на голову GM 
            pointPressGM_1.PressMouseL();
            Pause(3000);

            //жмем Ок 10 раз
            for (int j = 1; j <= 10; j++)
            {
                ButtonOkDialog.PressMouseL();    // Нажимаем на Ok в диалоге
                Pause(1500);
            }

            Pause(2500);
        }
        /// <summary>
        /// бежим к персу для получения оружия и брони 35 лвл
        /// </summary>
        public void RunToGetWeapons()
        {
            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1000);
            pointPressGMonMap.PressMouseL();           //нажимаем на строчку GM на карте Alt+Z
            Pause(1000);
            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(10000);
            botwindow.PressEscThreeTimes();
            Pause(1000);
        }

        /// <summary>
        /// первый разговор с Линдоном после Стартонии
        /// </summary>
        public void TalkToLindon1()
        {
            //Pause(4000);
            pointPressLindon1.PressMouseL(); //нажимаем на Линдона
            Pause(4000);

            for (int j = 1; j <= 4; j++)
            {
                ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
                Pause(2000);
            }
            Pause(2000);
        }

        /// <summary>
        /// разговор с Нуьесом
        /// </summary>
        public void TalkRunToNunez()
        {
            pointPressNunez.PressMouseL();   // Нажимаем на Нуньеса
            Pause(3000);
            pointPressNunez.PressMouseL();   // Нажимаем на Нуньеса
            Pause(3000);

            //for (int j = 1; j <= 7; j++)
            //{
            //    ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
            //    Pause(2000);
            //}

            dialog.PressOkButton(7);

            PressMedal.DoubleClickL();        //нажимаем на медаль 1 (медаль новичка)  двойной щелчок
            Pause(1500);

            botwindow.PressEscThreeTimes();    //закрываем лишние окна
            Pause(3000);
            //ButtonCloseMedal.PressMouseL();    // Нажимаем на Close и закрываем медали
            //Pause(2500);

            pointPressNunez2.PressMouseL();   // Нажимаем на Нуньеса
            Pause(5000);

            //for (int j = 1; j <= 5; j++)
            //{
            //    ButtonOkDialog.PressMouse();    // Нажимаем на Ok в диалоге
            //    Pause(2500);
            //}

            dialog.PressOkButton(5);

            Pause(1500);

            //ожидаем ребольдо

            //PressMedal_1.PressMouseL();          //нажимаем на медаль 1 (медаль новичка)  двойной щелчок
            //Pause(50);
            //PressMedal_1.PressMouseL();
            //Pause(500);


        }

        /// <summary>
        /// бежим в Стартонии до Нуньеса
        /// </summary>
        public void RunToNunez()
        {

            pointRunNunies.DoubleClickL();   // Нажимаем кнопку вызова списка групп
            Pause(25000);
        }

        /// <summary>
        /// создаем Team в казарме
        /// </summary>
        public void CreateOfTeam()
        {
            pointTeamSelection1.PressMouse();   // Нажимаем кнопку вызова списка групп
            Pause(1500);
            pointUnselectMedik.PressMouseL();   // выкидывание медика из команды
            Pause(1500);
            pointSelectMusk.PressMouseL();       // выбор мушкетера в команду
            Pause(1500);
            pointNameOfTeam.PressMouseL();      //тыкаем в строку, где надо вводить имя группы героев
            Pause(1500);

            Random random = new Random();
            int temp;
            temp = random.Next(99999);        //случайное число от 0 до 9999
            string randomNumber = temp.ToString();     //число в строку
            SendKeys.SendWait(randomNumber);
            Pause(1500);
            pointButtonSaveNewTeam.PressMouseL();
            Pause(2500);

        }

        /// <summary>
        /// Ввод имени новой семьи
        /// </summary>
        public void NewName()
        {
            pointNewName.PressMouse();   //тыкнули в строчку, где нужно вводить имя семьи
            Pause(1500);

            Random random = new Random();
            int temp;
            temp = random.Next(9999);        //случайное число от 0 до 9999
            string randomNumber = temp.ToString();    //число в строку

            string newFamily = botwindow.getNameOfFamily();
            SendKeys.SendWait(newFamily + randomNumber);

            Pause(1500);
            pointButtonCreateNewName.DoubleClickL();    //тыкнули в кнопку создания новой семьи
            Pause(3000);
        }

        /// <summary>
        /// вводим имена героев (участников семьи) 
        /// </summary>
        public void NamesOfHeroes()
        {
            //medik
            pointCreateHeroes.PressMouse();    //создали медика
            Pause(1500);
            pointButtonOkCreateHeroes.PressMouse(); //нажали Ок
            Pause(1500);

            //musketeer #1
            pointMenuSelectTypeHeroes.PressMouse();
            Pause(1500);
            pointSelectTypeHeroes.PressMouseL();
            Pause(1500);
            pointNameOfHeroes.PressMouseL();
            Pause(1500);
            SendKeys.SendWait("Musk1");
            Pause(1500);
            pointCreateHeroes.PressMouse();    //создали мушкетера
            Pause(1500);
            pointButtonOkCreateHeroes.PressMouse(); //нажали Ок
            Pause(1500);

            //musketeer #2
            pointMenuSelectTypeHeroes.PressMouse();
            Pause(1500);
            pointSelectTypeHeroes.PressMouseL();
            Pause(1500);
            pointNameOfHeroes.PressMouseL();
            Pause(1500);
            SendKeys.SendWait("Musk2");
            Pause(1500);
            pointCreateHeroes.PressMouse();    //создали мушкетера
            Pause(1500);
            pointButtonOkCreateHeroes.PressMouse(); //нажали Ок
            Pause(1500);

            //musketeer #3

            pointButtonCreateChar.PressMouseL();
            Pause(1500);
            pointMenuSelectTypeHeroes.PressMouse();
            Pause(1500);
            pointSelectTypeHeroes.PressMouseL();
            Pause(1500);
            pointNameOfHeroes.PressMouseL();
            Pause(1500);
            SendKeys.SendWait("Musk3");
            Pause(1500);
            pointCreateHeroes.PressMouse();    //создали мушкетера
            Pause(1500);
            pointButtonOkCreateHeroes.PressMouse(); //нажали Ок
            Pause(1500);

        }

        /// <summary>
        /// проверяем, выскочило ли сообщение, что казарма переполнена
        /// </summary>
        /// <returns>true, если казарма переполнена</returns>
        public bool isBarackFull()
        {
            return new PointColor(522 - 5 + xx + 5, 427 - 5 + yy + 5, 7727344, 0).isColor()
                && new PointColor(522 - 5 + xx + 5, 428 - 5 + yy + 5, 7727344, 0).isColor();
        }

        /// <summary>
        /// создание мушкетера в казарме с заданным порядковым номером
        /// </summary>
        /// <returns>true, если мушкетёр создан.  False - если казарма уже заполнена</returns>
        public bool CreateMuskInBarack()
        {
            PressCreateButton();

            if (isBarackFull())
                return false;

            pointMenuSelectTypeHeroes.PressMouse();
            Pause(1500);
            pointSelectTypeHeroes.PressMouseL();
            Pause(1500);
            pointNameOfHeroes.PressMouseL();
            Pause(1500);

            Random rnd = new Random();
            int number = rnd.Next(1, 10000);
            string NameMusk = "Musk" + number;
            SendKeys.SendWait(NameMusk);
            Pause(1500);

            pointCreateHeroes.PressMouse();    //создали мушкетера
            Pause(1500);
            pointButtonOkCreateHeroes.PressMouse(); //нажали Ок
            Pause(1500);

            return true;
        }


        #endregion

        #region кратер
        /// <summary>
        /// перекладываем митридат в ячейку под цифрой 2
        /// </summary>
        public void PutMitridat()
        {
            TopMenu(8, 1);
            Pause(2000);

            pointSecondBookmark.PressMouseL();
            Pause(2000);

            pointMitridat.Drag(pointMitridatTo2);
            Pause(2000);

            botwindow.PressEscThreeTimes();
            Pause(1000);
        }

        /// <summary>
        /// включаем родные стены
        /// </summary>
        public void OnPremium()
        {
            TopMenu(8, 2);
            Pause(2000);

            pointBookmark3.PressMouseL();
            Pause(1000);

            pointFirstItem.DoubleClickL();      //нажимаем дважды на первую вещь в спецкармане
            Pause(1000);

            pointButtonYesPremium.PressMouseL();  //подтверждаем
            Pause(2000);

            botwindow.PressEscThreeTimes();
            Pause(1000);
        }

        /// <summary>
        /// сохраняем телепорт в месте работы
        /// </summary>
        public void SaveTeleport()
        {
            TopMenu(12);
            Pause(1000);

            pointButtonSaveTeleport.PressMouseL();
            Pause(1000);

            pointButtonOkSaveTeleport.PressMouseL();
            Pause(1000);

            botwindow.PressEscThreeTimes();
            Pause(1000);
        }

        /// <summary>
        /// бежим к месту работы в Кратере
        /// </summary>
        public void RunToWork()
        {
            botwindow.PressMitridat();
            Pause(1000);

            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1000);

            pointWorkCrater.PressMouseR();             //бежим к месту работы в кратере
            Pause(1000);

            botwindow.PressEscThreeTimes();
            Pause(botwindow.getNumberWindow() * 10000);     // пауза зависит от номера окна, так как первые боты будут ближе ко входу стоять
        }

        /// <summary>
        /// бежим к телепорту WP
        /// </summary>
        public void RunToCrater()
        {
            botwindow.PressMitridat();
            Pause(1000);

            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1000);

            pointGateCrater.PressMouseL();             //переход (ворота) из лавового плато в кратер
            Pause(1000);

            botwindow.PressEscThreeTimes();
            Pause(65000);


        }


        /// <summary>
        /// выбираем лавовое плато на телепорте
        /// </summary>
        public void DialogWaypoint()
        {
            pointBookmarkField.PressMouseL();            //выбрали телепорт (последняя строчка)
            Pause(1000);

            pointButtonLavaPlato.PressMouseL();          //выбрали лавовое плато
            Pause(10000);

        }

        /// <summary>
        /// бежим к телепорту WP
        /// </summary>
        public void RunToWaypoint()
        {
            Pause(5000);
            botwindow.PressEscThreeTimes();
            Pause(1000);

            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1000);

            pointWayPointMap.PressMouseL();            //выбрали телепорт (последняя строчка)
            Pause(1000);

            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(5000);

            botwindow.PressEscThreeTimes();
            Pause(1000);

            pointWayPoint.PressMouseL();           //нажимаем на телепорт
            Pause(1000);
        }

        #endregion

        #region "Заточка у Иды" 

        /// <summary>
        /// делаем активным инвентарь
        /// </summary>
        public void InventoryActive()
        {
            pointAcriveInventory.PressMouseR();
        }

        /// <summary>
        /// проверяем, стал ли активным инвентарь (по слову Equip)
        /// </summary>
        /// <returns></returns>
        public bool isActiveInventory()
        {
            return pointIsActiveInventory.isColor();
        }

        /// <summary>
        /// проверяем, был ли переложен предмет экипировки на место для заточки
        /// </summary>
        /// <returns></returns>
        public bool isMoveEquipment()
        {
            return (pointisMoveEquipment1.isColor() && pointisMoveEquipment2.isColor());
        }

        /// <summary>
        /// нажимаем на кнопку Enhance
        /// </summary>
        public void PressButtonEnhance()
        {
            pointButtonEnhance.PressMouseL();
        }

        /// <summary>
        /// проверяем, заточилась ли вещь на +4
        /// </summary>
        /// <returns></returns>
        public bool isPlus4()
        {
            return (pointIsPlus41.isColor() && pointIsPlus42.isColor()) ||
                    (pointIsPlus43.isColor() && pointIsPlus44.isColor());  //либо одни две точки либо другие две
        }

        /// <summary>
        /// нажимаем на кнопку Max (добавляем Shiny Crystal для заточки на +5 или +6)
        /// </summary>
        public void AddShinyCrystall()
        {
            pointAddShinyCrystall.PressMouseL();
            pointAddShinyCrystall2.PressMouseL();
        }

        /// <summary>
        /// проверяем, прибавились ли шайники к заточке на +6 (проверка по голубой полоске)
        /// </summary>
        /// <returns></returns>
        public bool isAddShinyCrystall()
        {
            return pointIsAddShinyCrystall1.isColor() && pointIsAddShinyCrystall2.isColor();
        }

        /// <summary>
        /// проверяем, находимся ли в магазине у Иды (заточка)
        /// </summary>
        /// <returns></returns>
        public bool isIda()
        {
            return (pointIsIda1.isColor() && pointIsIda2.isColor());
        }

        public abstract void MoveToSharpening(int numberOfEquipment);

        #endregion

        #region чиповка

        /// <summary>
        /// проверяем, находимся ли в магазине у Чиповщицы
        /// </summary>
        /// <returns></returns>
        public bool isEnchant()
        {
            return (pointIsEnchant1.isColor() && pointIsEnchant2.isColor());
        }

        /// <summary>
        /// проверяем, является ли предмет для чиповки оружием
        /// </summary>
        /// <returns></returns>
        public bool isWeapon()
        {
            return pointisWeapon1.isColor() && pointisWeapon2.isColor();
        }

        /// <summary>
        /// проверяем, является ли предмет для чиповки брони
        /// </summary>
        /// <returns></returns>
        public bool isArmor()
        {
            return (pointisArmor1.isColor() && pointisArmor2.isColor());
        }

        /// <summary>
        /// переносим (DragAndDrop) левую панель, чтобы она не мешала
        /// </summary>
        /// <param name="numberOfEquipment">номер экипировки п/п</param>
        public void MoveLeftPanel()
        {
            pointMoveLeftPanelBegin.Drag(pointMoveLeftPanelEnd);
        }

        /// <summary>
        /// нажимаем на кнопку Enchance для чипования
        /// </summary>
        public void PressButtonEnchance()
        {
            pointButtonEnchance.PressMouseL();
        }

        /// <summary>
        /// проверяем, является ли предмет для чиповки брони
        /// </summary>
        /// <returns></returns>
        public bool isGoodChipArmor()
        {
            bool result = false;

            if (isDef15() && isHP()) result = true;
            //            if (isDef15()) result = true;
            //            if (isHP()) result = true;


            return result;
        }

        /// <summary>
        /// проверяем, зачиповалась ли броня на +15 def
        /// </summary>
        /// <returns></returns>
        public bool isDef15()
        {
            return pointisDef15.isColor();
        }

        /// <summary>
        /// проверяем, зачиповалась ли броня на +15 def
        /// </summary>
        /// <returns></returns>
        public bool isHP()
        {
            bool result = false;
            for (int i = 1; i <= 5; i++)
            {
                if (new PointColor(355 - 5 + 5 + 5, 277 - 5 + 5 + 5 + (i - 1) * 15, 7400000, 5).isColor())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        ///// <summary>
        ///// метод возвращает параметр, отвечающий за тип чиповки оружия
        ///// </summary>
        ///// <returns></returns>
        //public int TypeOfNintendo()
        //{ return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Чиповка.txt")); }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на АТК + 40%
        /// </summary>
        /// <returns></returns>
        public bool isAtk40()
        {
            return (pointisAtk401.isColor() && pointisAtk402.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на АТК + 39%
        /// </summary>
        /// <returns></returns>
        public bool isAtk39()
        {
            return (pointisAtk391.isColor() && pointisAtk392.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на АТК + 38%
        /// </summary>
        /// <returns></returns>
        public bool isAtk38()
        {
            return (pointisAtk381.isColor() && pointisAtk382.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на АТК + 37%
        /// </summary>
        /// <returns></returns>
        public bool isAtk37()
        {
            return (pointisAtk371.isColor() && pointisAtk372.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на скорость + 30%
        /// </summary>
        /// <returns></returns>
        public bool isAtkSpeed30()
        {
            return pointisSpeed30.isColor();
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на скорость + 29%
        /// </summary>
        /// <returns></returns>
        public bool isAtkSpeed29()
        {
            return (pointisSpeed291.isColor() && pointisSpeed292.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на скорость + 28%
        /// </summary>
        /// <returns></returns>
        public bool isAtkSpeed28()
        {
            return (pointisSpeed281.isColor() && pointisSpeed282.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на скорость + 28%
        /// </summary>
        /// <returns></returns>
        public bool isAtkSpeed27()
        {
            return (pointisSpeed271.isColor() && pointisSpeed272.isColor());
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по животным
        /// </summary>
        /// <returns></returns>
        public bool isWild()
        {
            bool result = false;
            for (int i = 1; i <= 5; i++)
            {
                if (
                    new PointColor(415 - 5 + 5 + 5, 292 - 5 + 5 + 5 + (i - 1) * 15, 7900000, 5).isColor() ||
                    new PointColor(415 - 5 + 5 + 5, 301 - 5 + 5 + 5 + (i - 1) * 15, 7900000, 5).isColor()
                    )
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по людям
        /// </summary>
        /// <returns></returns>
        public bool isHuman()
        {
            return (
                    (pointisHuman41.isColor() && pointisHuman42.isColor()) ||
                    (pointisHuman51.isColor() && pointisHuman52.isColor()) ||
                    (pointisHuman61.isColor() && pointisHuman62.isColor()) ||
                    (pointisHuman71.isColor() && pointisHuman72.isColor())
                    );
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по демонам
        /// </summary>
        /// <returns></returns>
        public bool isDemon()
        {
            return (
                    (pointisDemon41.isColor() && pointisDemon42.isColor()) ||
                    (pointisDemon51.isColor() && pointisDemon52.isColor()) ||
                    (pointisDemon61.isColor() && pointisDemon62.isColor()) ||
                    (pointisDemon71.isColor() && pointisDemon72.isColor())
                    );
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по Undead
        /// </summary>
        /// <returns></returns>
        public bool isUndead()
        {
            bool result = false;
            for (int i = 1; i <= 5; i++)
            {
                if (
                    new PointColor(397 - 5 + 5 + 5, 293 - 5 + 5 + 5 + (i - 1) * 15, 7400000, 5).isColor() ||
                    new PointColor(397 - 5 + 5 + 5, 294 - 5 + 5 + 5 + (i - 1) * 15, 7400000, 5).isColor()
                    )
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по Lifeless
        /// </summary>
        /// <returns></returns>
        public bool isLifeless()
        {
            bool result = false;
            for (int i = 1; i <= 5; i++)
            {
                if (
                    new PointColor(398 - 5 + 5 + 5, 292 - 5 + 5 + 5 + (i - 1) * 15, 7500000, 5).isColor() ||
                    new PointColor(398 - 5 + 5 + 5, 301 - 5 + 5 + 5 + (i - 1) * 15, 7600000, 5).isColor()
                    )
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// проверяем, является ли предмет для чиповки оружия
        /// 1 - без рассы
        /// 2 - wild
        /// 3 - LifeLess
        /// 4 - wild or Human
        /// 5 - Undeed
        /// 6 - Demon
        /// 7 - Human
        /// </summary>
        /// <returns></returns>
        public bool isGoodChipWeapon()
        {
            bool result = false;
            int parametr = globalParam.Nintendo;
            switch (parametr)
            {
                case 1:
                    if ((isAtk40() || isAtk39()) && (isAtkSpeed30() || isAtkSpeed29())) result = true;
                    break;
                case 2:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && isWild()) result = true;
                    //if ((isAtk40() || isAtk39()) && (isAtkSpeed30() || isAtkSpeed29())) result = true;
                    break;
                case 3:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && isLifeless()) result = true;
                    break;
                case 4:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isWild() || isHuman())) result = true;
                    if ((isAtk40() || isAtk39()) && (isAtkSpeed30() || isAtkSpeed29())) result = true;
                    break;
                case 5:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isUndead())) result = true;
                    if ((isAtk40() || isAtk39()) && (isAtkSpeed30() || isAtkSpeed29())) result = true;
                    break;
                case 6:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isDemon())) result = true;
                    break;
                case 7:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isHuman())) result = true;
                    break;
                case 10:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) &&
                        (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) &&
                        (isHuman() || isWild() || isLifeless() || isUndead() || isDemon())) result = true;
                    break;
            }
            return result;
        }

        public abstract void MoveToNintendo(int numberOfEquipment);

        #endregion

        #region Personal Trade
        /// <summary>
        /// определяет открыто ли окно для персональной торговли
        /// </summary>
        /// <returns></returns>
        public bool isPersonalTrade()
        {
            return (pointPersonalTrade1.isColor() && pointPersonalTrade2.isColor());
        }


        #endregion

        #region методы для перекладывания песо в торговца

        /// <summary>
        /// для передачи песо торговцу. Идем на место и предложение персональной торговли                        
        /// </summary>
        public void GotoPlaceTradeBot()
        {
            //идем на место передачи песо
            botwindow.PressEscThreeTimes();
            Pause(1000);

            town.MaxHeight();                    // с учетом города и сервера
            Pause(500);

            OpenMapForState();                   // открываем карту города
            Pause(500);

            pointMap.DoubleClickL();             // тыкаем в карту, чтобы добежать до нужного места

            botwindow.PressEscThreeTimes();      // закрываем карту
            Pause(25000);                        // ждем пока добежим

            iPointColor pointMenuTrade = new PointColor(588 - 5 + xx, 230 - 5 + yy, 1710000, 4);
            while (!pointMenuTrade.isColor())
            {
                //жмем правой на торговце
                pointTrader.PressMouseR();
                Pause(1000);
            }

            //жмем левой  на пункт "Personal Trade"
            pointPersonalTrade.PressMouseL();
            Pause(500);
        }        //проверено

        /// <summary>
        /// обмен песо на фесо (часть 1 со стороны торговца) 
        /// </summary>
        public void ChangeVisDealer()
        {
            // наживаем Yes, подтверждая открытие торговли
            PressButtonYesTrade();

            // открываем сундук (карман)
            TopMenu(8, 1);

            // открываем закладку кармана, там где фесо
            OpenFourthBookmark();

            // перетаскиваем фесо на стол торговли
            MoveFesoToTrade();

            // нажимаем ок и обмен
            PressOkTrade();
        }                //проверено

        /// <summary>
        /// обмен песо. закрываем сделку со стороны бота
        /// </summary>
        public void ChangeVisBot()
        {
            // открываем инвентарь
            TopMenu(8, 1);

            // открываем закладку кармана, там где песо
            OpenFourthBookmark();
            //pointVis1.DoubleClickL();
            //Pause(500);


            //// перетаскиваем песо на стол торговли
            MoveVisToTrade();

            // нажимаем ок и обмен
            PressOkTrade();
        }                  // проверено

        /// <summary>
        /// подтверждает согласие на персональную торговлю
        /// </summary>
        public void PressButtonYesTrade()
        {
            pointYesTrade.DoubleClickL();
            Pause(500);
        }

        /// <summary>
        /// нажимаем Ок и Обмен в персональной торговле
        /// </summary>
        public void PressOkTrade()
        {
            // нажимаем ок
            pointOk.DoubleClickL();
            Pause(500);

            // нажимаем обмен
            pointTrade.DoubleClickL();
            Pause(500);
        }

        /// <summary>
        /// открываем четвертую закладку в инвентаре
        /// </summary>
        public void OpenFourthBookmark()
        {
            pointBookmark4.DoubleClickL();
            Pause(500);
        }

        /// <summary>
        /// перемещаем фесо 125 000 из инвентаря на стол торговли
        /// </summary>
        public void MoveFesoToTrade()
        {
            // перетаскиваем фесо на стол торговли
            pointFesoBegin.Drag(pointFesoEnd);
            Pause(500);

            SendKeys.SendWait("300000");
            Pause(500);

            // нажимаем Ок для подтверждения передаваемой суммы фесо
            pointOkSum.DoubleClickL();

        }

        /// <summary>
        /// перемещаем всё песо из инвентаря на стол торговли
        /// </summary>
        public void MoveVisToTrade()
        {
            // перетаскиваем песо
            pointVisMove1.Drag(pointVisMove2);                                             // песо берется из первой ячейки на 4-й закладке  
            Pause(500);

            // нажимаем Ок для подтверждения передаваемой суммы песо
            pointOkSum.DoubleClickL();

        }

        /// <summary>
        /// купить 125 еды в фесо шопе                    
        /// </summary>
        public void Buy125PetFood()
        {
            // тыкаем два раза в стрелочку вверх
            pointFood.DoubleClickL();
            Pause(500);

            //нажимаем 125
            SendKeys.SendWait("125");

            // жмем кнопку купить
            pointButtonFesoBUY.DoubleClickL();
            Pause(1500);

            //нажимаем кнопку Close
            botwindow.PressEscThreeTimes();
            //pointButtonClose.DoubleClickL();
            Pause(1500);
        }

        /// <summary>
        /// продать 3 ВК (GS) в фесо шопе 
        /// </summary>
        public void SellGrowthStone3pcs()
        {
            // 3 раза нажимаем на стрелку вверх, чтобы отсчитать 3 ВК
            for (int i = 1; i <= 3; i++)
            {
                pointArrowUp2.PressMouseL();
                Pause(700);
            }

            //нажимаем кнопку Sell
            pointButtonFesoSell.PressMouseL();
            Pause(1000);

            //нажимаем кнопку Close
            pointButtonClose.PressMouseL();
            Pause(2500);
        }

        /// <summary>
        /// открыть вкладку Sell в фесо шопе
        /// </summary>
        public void OpenBookmarkSell()
        {
            pointBookmarkFesoSell.DoubleClickL();
            Pause(1500);
        }

        /// <summary>
        /// переход торговца к месту передачи песо (внутри города)
        /// </summary>
        public void GoToChangePlace()
        {
            pointDealer.DoubleClickL();
        }

        /// <summary>
        /// открыть фесо шоп
        /// </summary>
        public void OpenFesoShop()
        {
            TopMenu(2, 2);
            Pause(1000);
        }


        #endregion

        #region общие2

        /// <summary>
        /// определяем, есть ли в команде второй перс (герой)
        /// </summary>
        public bool isSecondHero()
        {
            return (pointisKillHero2.isColor() || pointisLiveHero2.isColor());
        }

        /// <summary>
        /// определяем, есть ли в команде третий перс (герой)
        /// </summary>
        public bool isThirdHero()
        {
            return (pointisKillHero3.isColor() || pointisLiveHero3.isColor());
        }

        #endregion

        #region Undressing in Barack

        /// <summary>
        /// раздевание персонажей в выбранной казарме
        /// </summary>
        private void UnDressingInCurrentBarack()
        {
            int[] x = { 0, 0, 130, 260, 390, -70, 60, 190, 320, 450 };
            int[] y = { 0, 0, 0, 0, 0, 340, 340, 340, 340, 340 };

            for (int i = 1; i <= 9; i++)            //перебор героев в текущей казарме
            {
                iPointColor pointHatC = new PointColor(285 - 5 + xx + x[i], 119 - 5 + yy + y[i], 2434089, 0);
                iPointColor pointGlassesC = new PointColor(369 - 5 + xx + x[i], 119 - 5 + yy + y[i], 131588, 0);
                iPointColor pointMedalC = new PointColor(345 - 5 + xx + x[i], 159 - 5 + yy + y[i], 5398113, 0);
                iPointColor pointWingsC = new PointColor(285 - 5 + xx + x[i], 199 - 5 + yy + y[i], 197640, 0);
                iPointColor pointArmorC = new PointColor(325 - 5 + xx + x[i], 199 - 5 + yy + y[i], 2960436, 0);
                iPointColor pointCostumeC = new PointColor(365 - 5 + xx + x[i], 199 - 5 + yy + y[i], 2960436, 0);
                iPointColor pointWeaponC = new PointColor(290 - 5 + xx + x[i], 241 - 5 + yy + y[i], 855313, 0);
                iPointColor pointGlovesC = new PointColor(325 - 5 + xx + x[i], 279 - 5 + yy + y[i], 1644571, 0);
                iPointColor pointBootsC = new PointColor(328 - 5 + xx + x[i], 325 - 5 + yy + y[i], 6513252, 0);

                iPoint pointHat = new Point(285 - 5 + xx + x[i], 119 - 5 + yy + y[i]);
                iPoint pointGlasses = new Point(365 - 5 + xx + x[i], 119 - 5 + yy + y[i]);
                iPoint pointMedal = new Point(345 - 5 + xx + x[i], 159 - 5 + yy + y[i]);
                iPoint pointWings = new Point(285 - 5 + xx + x[i], 199 - 5 + yy + y[i]);
                iPoint pointArmor = new Point(325 - 5 + xx + x[i], 199 - 5 + yy + y[i]);
                iPoint pointCostume = new Point(365 - 5 + xx + x[i], 199 - 5 + yy + y[i]);
                iPoint pointWeapon = new Point(285 - 5 + xx + x[i], 239 - 5 + yy + y[i]);
                iPoint pointGloves = new Point(325 - 5 + xx + x[i], 279 - 5 + yy + y[i]);
                iPoint pointBoots = new Point(325 - 5 + xx + x[i], 319 - 5 + yy + y[i]);

                if (!pointGlassesC.isColor()) { pointGlasses.DoubleClickL(); Pause(200); }
                if (!pointHatC.isColor()) { pointHat.DoubleClickL(); Pause(200); }
                if (!pointMedalC.isColor()) { pointMedal.DoubleClickL(); Pause(200); }
                if (!pointWingsC.isColor()) { pointWings.DoubleClickL(); Pause(200); }
                if (!pointArmorC.isColor()) { pointArmor.DoubleClickL(); Pause(200); }
                if (!pointCostumeC.isColor()) { pointCostume.DoubleClickL(); Pause(200); }
                if (!pointWeaponC.isColor()) { pointWeapon.DoubleClickL(); Pause(200); }
                if (!pointGlovesC.isColor()) { pointGloves.DoubleClickL(); Pause(200); }
                if (!pointBootsC.isColor()) { pointBoots.DoubleClickL(); Pause(200); }
            }

        }

        /// <summary>
        /// раздевание в первых четырёх казармах (36 персонажей)
        /// </summary>
        public void UnDressing()
        {
            bool ff = true;
            while (ff)
            {
                pointShowEquipment.PressMouseL();      //нажимаем на кнопку "Show Equipment"
                Pause(1000);
                ff = (pointEquipment1.isColor()) && (pointEquipment2.isColor());
                ff = !ff;
            }

            for (int i = 1; i <= 4; i++)
            {
                pointBarack[i].PressMouseL();            //выбираем i-ю казарму
                Pause(1000);
                UnDressingInCurrentBarack();
            }

            //pointBarack1.PressMouseL();            //выбираем первую казарму
            //Pause(1000);
            //UnDressingInCurrentBarack();

            //pointBarack2.PressMouseL();            //выбираем вторую казарму
            //Pause(1000);
            //UnDressingInCurrentBarack();


            //pointBarack3.PressMouseL();            //выбираем третью казарму
            //Pause(1000);
            //UnDressingInCurrentBarack();


            //pointBarack4.PressMouseL();            //выбираем четвертую казарму
            //Pause(1000);
            //UnDressingInCurrentBarack();

        }

        #endregion

        #region Гильдия Охотников BH

        public abstract void runClientBH();
        //public abstract UIntPtr FindWindowGEforBH();

        /// <summary>
        /// найдено ли окно ГЭ в текущей песочнице?
        /// </summary>
        /// <returns>true, если найдено</returns>
        public abstract bool FindWindowGEforBHBool();

        /// <summary>
        /// найдено ли окно ГЭ в текущей песочнице? GE Classic
        /// </summary>
        /// <returns>true, если найдено</returns>
        public abstract bool FindWindowGEClassic();

        /// <summary>
        /// подбор дропа в миссии Инфинити
        /// </summary>
        public void GetDrop()
        {
            //старый способ
            //new Point(188 - 5 + xx, 526 - 5 + yy).PressMouseL();    //боевой режим, чтобы боты остановились
            //new Point(123 - 5 + xx, 526 - 5 + yy).PressMouseL();    //нажимаем на сундук (иконка подбора)
            //Pause(200);
            //new Point(760 - 5 + xx, 330 - 5 + yy).PressMouseL();    //нажимаем в случайную точку в стороне, чтобы начать подбор

            BattleModeOn();   //боевой режим, чтобы боты остановились
            new Point(123 - 5 + xx + 5, 526 - 5 + yy + 5).PressMouseLL();    //нажимаем на сундук (иконка подбора)
            Pause(200);
            new Point(230 - 5 + xx + 5, 390 - 5 + yy + 5).PressMouseL();    //нажимаем в случайную точку в стороне, чтобы начать подбор

        }

        /// <summary>
        /// проверяем, крутится ли рулетка после убийства босса
        /// </summary>
        /// <returns></returns>
        public bool isRouletteBH()
        {
            return pointIsRoulette1.isColor() && pointIsRoulette2.isColor();
        }

        ///// <summary>
        ///// метод возвращает значение статуса, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        ///// </summary>
        ///// <returns></returns>
        //private int GetStatusOfSale()
        //{ return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\StatusOfSale.txt")); }

        ///// <summary>
        ///// метод возвращает значение статуса, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        ///// </summary>
        ///// <returns></returns>
        //private void SetStatusOfSale(int status)
        //{
        //    File.WriteAllText(globalParam.DirectoryOfMyProgram + "\\StatusOfSale.txt", status.ToString());
        //}

        /// <summary>
        /// проверяем, атакуем ли сейчас босса
        /// если точки серые т.е. pointIsAtak1.isColor() = true или pointIsAtak1.isColor() = true, то значит не атакуем. Поэтому стоит отрицание
        /// </summary>
        /// <returns></returns>
        public bool isAtakBH()
        {
            return (pointIsAtak1.isColor() && pointIsAtak2.isColor());
        }

        /// <summary>
        /// ожидание прекращения атаки
        /// </summary>
        public void waitToCancelAtak()
        {
            //while (isAtakBH())
            //{
            //    Pause(500);
            //}
        }

        ///// <summary>
        ///// опускаем камеру (опускаем максимально вниз)                           
        ///// </summary>
        //public void MinHeight()
        //{
        //    Point pointMinHeight = new Point(514 - 30 + xx, 352 - 30 + yy);
        //    for (int j = 1; j <= 5; j++)
        //    {
        //        pointMinHeight.PressMouseWheelDown();
        //        Pause(300);
        //    }
        //}

        /// <summary>
        /// записываем в лог-файл инфу по прохождению программы
        /// </summary>
        /// <param name="strLog"></param>
        public void WriteToLogFileBH(string strLog)
        {
            //StreamWriter writer = new StreamWriter(globalParam.DirectoryOfMyProgram + "\\BH.log", true);
            //string timeNow = DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss | ");

            //writer.WriteLine(timeNow + botwindow.getNumberWindow().ToString() + " " + strLog);
            //writer.Close();
        }

        /// <summary>
        /// записываем в лог-файл инфу по прохождению программы
        /// </summary>
        /// <param name="strLog"></param>
        public void WriteToLogFile(string strLog)
        {
            //StreamWriter writer = new StreamWriter(globalParam.DirectoryOfMyProgram + "\\Error.log", true);
            //string timeNow = DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss | ");

            //writer.WriteLine(timeNow + botwindow.getNumberWindow().ToString() + " " + strLog);
            //writer.Close();
        }

        /// <summary>
        /// нажимаем левой кнопкой мыши на точку с указанными координатами
        /// </summary>
        /// <param name="x">коорд X</param>
        /// <param name="y">коорд Y</param>
        public void FightToPoint(int x, int y, int t)
        {
            //старый вариант
            //Point pointSabreBottonBH = new Point(92 - 5 + xx, 525 - 5 + yy);         //нажимаем на кнопку с саблей на боевой панели (соответствует нажатию Ctrl+Click)
            //Point pointFightBH = new Point(x - 30 + xx, y - 30 + yy);                //нажимаем конкретную точку, куда надо бежать и бить всех по пути

            //pointSabreBottonBH.PressMouseL();
            //pointFightBH.PressMouseL();
            //Pause(t * 1000);

            //новый вариант
            //            Point pointSabreBottonBH = new Point(92 - 5 + xx, 525 - 5 + yy);         //нажимаем на кнопку с саблей на боевой панели (соответствует нажатию Ctrl+Click)
            Point pointFightBH = new Point(x - 30 + xx + 5, y - 30 + yy + 5);                //нажимаем конкретную точку, куда надо бежать и бить всех по пути

            AssaultMode();
            pointFightBH.PressMouseL();
            Pause(t * 1000);

        }

        /// <summary>
        /// проверка миссии по цвету контрольной точки
        /// </summary>
        /// <returns> номер цвета </returns>
        public abstract uint ColorOfMissionBH();                                        //может быть перенести из синга сюда

        /// <summary>
        /// отбегаю в сторону, чтобы бот не стрелял
        /// </summary>
        public void runAway()
        {
            iPoint pointNotToShoot = new Point(300 - 5 + xx, 500 - 5 + yy);
            // отбегаю в сторону. чтобы бот не стрелял  
            pointNotToShoot.DoubleClickL();
            botwindow.Pause(4000);
        }

        /// <summary>
        /// проверяем, находимся ли в миссии Гильдии Охотников (проверяется 16 миссий по контрольной точке)
        /// </summary>
        /// <returns></returns>
        public bool isMissionBH()
        {
            //проверяем одну и ту же точку на экране в миссии
            //каждой карте в теории соответствует свой цвет этой точки

            // в массиве arrayOfColors хранятся все цвета контрольной точки, которые могут быть миссиях. Один цвет - одна миссия

            uint color = new PointColor(700 - 30 + xx + 5, 500 - 30 + yy + 5, 0, 0).GetPixelColor();                 // проверяем номер цвета в контрольной точке
            color = color / 1000;
            //int tt = Array.IndexOf(arrayOfColors, color);

            bool result = this.arrayOfColors.Contains(color);                                                         // проверяем, есть ли цвет контрольной точки в массиве цветов
            //if (!result) WriteToLogFileBH("неизвестная миссия, цвет " + color);

            return result;                                                   // проверяем, есть ли цвет контрольной точки в массиве цветов

        }

        /// <summary>
        /// если миссия в БХ не найдена. Нет в списке
        /// </summary>
        public void MissionNotFoundBH()
        {
            //botwindow.setStatusOfAtk(1);

            uint color = new PointColor(700 - 30 + xx + 5, 500 - 30 + yy + 5, 0, 0).GetPixelColor();                 // проверяем номер цвета в контрольной точке
            WriteToLogFileBH("неизвестная миссия!!!, цвет " + color);


            //сохраняем скриншот
            //string timeNow = DateTime.Now.ToString(" ddMMMMyyyy HH-mm-ss");
            //Size screenSz = Screen.PrimaryScreen.Bounds.Size;
            //Bitmap screenshot = new Bitmap(screenSz.Width, screenSz.Height);
            //Graphics gr = Graphics.FromImage(screenshot);
            //gr.CopyFromScreen(0, 0, 0, 0, screenSz);
            //string filepath = globalParam.DirectoryOfMyProgram + "\\ScreenShot"+ timeNow + ".bmp" ;
            //screenshot.Save(filepath);    
        }

        /// <summary>
        /// проверяем контрольную точку в миссии БХ и находим, в какой миссии мы находимся 
        /// </summary>
        /// <returns>номер миссии по порядку</returns>
        public int NumberOfMissionBH()
        {
            uint color = new PointColor(700 - 30 + xx + 5, 500 - 30 + yy + 5, 0, 0).GetPixelColor();                // проверяем номер цвета в контрольной точке
            color = color / 1000;
            int result = Array.IndexOf(this.arrayOfColors, color); // номер миссии соответствует порядковому номеру цвета в массиве arrayOfColors
            //if (result == 0) WriteToLogFileBH("неизвестная миссия, цвет " + color);
            return result;
        }

        /// <summary>
        /// проверяем, находимся ли в Гильдии Охотников
        /// </summary>
        /// <returns></returns>
        public bool isBH()
        {
            //return (isTown() && !pointisBH1.isColor());
            return !pointisBH1.isColor();   //нет желтого ободка на миникарте
        }

        /// <summary>
        /// проверяем, находимся ли в Гильдии Охотников на неправильном месте (вылет из миссии после убийства босса через минуту бездействия)
        /// </summary>
        /// <returns></returns>
        public bool isBH2()
        {
            return pointisBH2.isColor() || pointisBH3.isColor();    //именно оператор "или"
        }

        /// <summary>
        /// тыкаем в ворота Infinity (Гильдии Охотников)
        /// </summary>
        /// <returns></returns>
        public void GoToInfinityGateBH()
        {
            //MinHeight();
            pointGateInfinityBH.PressMouseL();
        }

        /// <summary>
        /// поворот на 180 градусов через левое плечо
        /// </summary>
        /// <returns></returns>
        public void Turn180()
        {
            //если begin больше на 24, то вправо на 90
            //если begin меньше на 24, то влево на 90
            //если begin больше на 16, то на 180 градусов

            //Point pointBegin = new Point(560 + 16 + - 30 + xx, 430 - 30 + yy);
            //Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);

            TurnL(16);


            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);

            ////ровно 135 градусов за 4 поворота (33,75)
            //Point pointBegin = new Point(560 + 1 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd   = new Point(560     - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);

            //ровно 270 градусов за 4 поворота (67,5)
            //Point pointBegin = new Point(560 + 2 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd   = new Point(560     - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);

            ////ровно 360+45 градусов за 4 поворота (101,25 )
            //Point pointBegin = new Point(560 + 3 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd =   new Point(560     - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);

            ////ровно 360+180=540 градусов за 4 поворота (135)
            //Point pointBegin = new Point(560 + 4 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd   = new Point(560     - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);
            //pointBegin.Turn(pointEnd);

        }

        /// <summary>
        /// поворот на 90 градусов вправо (по часовой стрелке)
        /// </summary>
        /// <returns></returns>
        public void Turn90L()
        {
            //если begin больше на 24, то влево на 90
            //если begin меньше на 24, то вправо на 90
            //если begin больше на 16, то на 180 градусов

            //Point pointBegin = new Point(560 + 24 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd   = new Point(560 - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);

            TurnL(24);
        }

        /// <summary>
        /// поворот на 90 градусов влево (против часовой стрелки)
        /// </summary>
        /// <returns></returns>
        public void Turn90R()
        {
            //если begin больше на 24, то влево на 90
            //если begin меньше на 24, то вправо на 90
            //если begin больше на 16, то на 180 градусов

            //Point pointBegin = new Point(560 - 24 - 30 + xx, 430 - 30 + yy);
            //Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            //pointBegin.Turn(pointEnd);

            TurnR(24);
        }

        /// <summary>
        /// поворот, чтобы смотреть более горизонтально (снизу вверх)
        /// </summary>
        public void TurnUp()
        {
            Point pointBegin = new Point(560 - 30 + xx, 430 - 30 + 1 + yy);
            Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }

        /// <summary>
        /// поворот, чтобы смотреть более вертикально (сверху вниз)
        /// </summary>
        public void TurnDown()
        {
            Point pointBegin = new Point(560 - 30 + xx, 430 - 30 - 1 + yy);
            Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }


        /// <summary>
        /// поворот влево (против часовой стрелки)
        /// </summary>
        /// <returns></returns>
        public void TurnL(int gradus)
        {
            Point pointBegin = new Point(560 + gradus - 30 + xx, 430 - 30 + yy);
            Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }

        /// <summary>
        /// поворот вправо (по часовой стрелке)
        /// </summary>
        /// <returns></returns>
        public void TurnR(int gradus)
        {
            Point pointBegin = new Point(560 - gradus - 30 + xx, 430 - 30 + yy);
            Point pointEnd = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }

        #endregion

        #region   ========== Кастилия (миссия) ==================

        /// <summary>
        /// проверяем, находимся ли в Castilia.Mine (в миссии)
        /// </summary>
        /// <returns></returns>
        public bool isCastiliaMine()
        {
            return new PointColor(946 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor() 
                && new PointColor(946 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor()
                && new PointColor(949 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor()
                && new PointColor(949 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor();        //слово Castilla под миникартой (две буквы l)
        }

        /// <summary>
        /// проверяем, находимся ли в Кастилии   
        /// </summary>
        /// <returns></returns>
        public bool isCastilia()
        {
            return new PointColor(927 - 5 + xx, 254 - 5 + yy, 15000000, 6).isColor() &&
                   new PointColor(927 - 5 + xx, 255 - 5 + yy, 15000000, 6).isColor();        //слово Castilla под миникартой (первая буква l)

        }

        /// <summary>
        /// проверяем, доступна ли миссия (не доступна, потому что уже ходили сегодня)  // не готово
        /// /// </summary>
        /// <returns>true, если не доступна</returns>
        public bool isMissionCastiliaNotAvailable()
        {
            //проверяем букву Т в слове Today  
            return new PointColor(373 - 5 + xx, 537 - 5 + yy, 13752023, 0).isColor() &&
                    new PointColor(374 - 5 + xx, 537 - 5 + yy, 13752023, 0).isColor();
        }

        /// <summary>
        /// тыкаем в зелёную стрелку для начала миссии в  Кастилии
        /// </summary>
        public void GoToMissionCastilia()
        {
            MaxHeight(14);
            botwindow.FirstHero();
            new Point(585 - 5 + xx, 50 - 5 + yy).PressMouseL();
            Pause(1000);
            new Point(685 - 5 + xx, 50 - 5 + yy).PressMouseL();
            Pause(1000);
            new Point(785 - 5 + xx, 50 - 5 + yy).PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// получаем следующую точку маршрута для миссии Кастилия
        /// </summary>
        /// <param name="counter">номер точки маршрута</param>
        /// <returns>точка iPoint</returns>
        public iPoint RouteNextPointMulti(int counter)
        {
            iPoint[] routeMulti = {  
                                     //new Point(372 - 5 + xx, 585 - 5 + yy),
                                     new Point(353 - 5 + xx, 572 - 5 + yy), //1+ да
                                     //new Point(347 - 5 + xx, 550 - 5 + yy),
                                     new Point(343 - 5 + xx, 524 - 5 + yy), //2+ да 
                                     //new Point(368 - 5 + xx, 495 - 5 + yy),
                                     //new Point(395 - 5 + xx, 489 - 5 + yy), //3+ да           //13-06-24
                                     //new Point(417 - 5 + xx, 508 - 5 + yy),
                                     //new Point(436 - 5 + xx, 516 - 5 + yy),
                                     //new Point(448 - 5 + xx, 519 - 5 + yy), //4+
                                     //new Point(474 - 5 + xx, 511 - 5 + yy),
                                     //new Point(478 - 5 + xx, 498 - 5 + yy),
                                     new Point(474 - 5 + xx, 483 - 5 + yy), //5+ да    473, 481   Кризалис
                                     //new Point(443 - 5 + xx, 432 - 5 + yy), //6+ да           //13-06-24  пауки
                                     //new Point(407 - 5 + xx, 444 - 5 + yy),
                                     new Point(384 - 5 + xx, 416 - 5 + yy), //7+ да
                                     //new Point(359 - 5 + xx, 388 - 5 + yy),
                                     //new Point(355 - 5 + xx, 359 - 5 + yy),
                                     //new Point(391 - 5 + xx, 352 - 5 + yy),
                                     new Point(398 - 5 + xx, 335 - 5 + yy), //8+ да
                                     //new Point(398 - 5 + xx, 334 - 5 + yy),  //да
                                     //new Point(438 - 5 + xx, 297 - 5 + yy),
                                     //new Point(437 - 5 + xx, 285 - 5 + yy), //9
                                     new Point(437 - 5 + xx, 280 - 5 + yy),     //9 да
                                     new Point(437 - 5 + xx, 225 - 5 + yy), //10 нет
                                     new Point(439 - 5 + xx, 212 - 5 + yy), //11 нет
                                  };
            iPoint result = routeMulti[counter];

            return result;
        }

        /// <summary>
        /// бежим с Ctrl к указанной точке маршрута
        /// </summary>
        /// <param name="NextPointNumber">точка маршрута</param>
        public void AssaultToNextPoint(int NextPointNumber)
        {
            botwindow.PressEsc();
            TopMenu(12, 2, true);                       //открываем карту в миссии (LocalMap)
            Pause(500);
            RouteNextPointMulti(NextPointNumber).PressMouseR();  //тыкаем правой кнопкой в карту, чтобы бежал вперёд с Ctrl
            botwindow.PressEsc();
        }

        /// <summary>
        /// бежим к указанной точке маршрута
        /// </summary>
        /// <param name="NextPointNumber">точка маршрута</param>
        public void MoveToNextPoint(int NextPointNumber)
        {
            botwindow.PressEsc();
            TopMenu(12, 2, true);                       //открываем карту в миссии (LocalMap)
            Pause(500);
            RouteNextPointMulti(NextPointNumber).PressMouseL();              //тыкаем левой кнопкой в карту, чтобы бежал к точке
            botwindow.PressEsc();                       //закрываем карту
        }


        /// <summary>
        /// возвращает количество циклов для ожилдания, пока герои добегут до следующей контрольной точки
        /// </summary>
        /// <param name="counter">номер контрольной точки маршрута</param>
        /// <returns>кол-во циклов</returns>
        public int GetWaitingTurnForMoveToPoint(int counter)
        {
            int[] WaitingTurn = {   
                                    //0, 
                                    6,      //1
                                    5,      //2
                                    //6,     //3           //13-06-24
                                    //6,     //4
                                    4,      //5  Кризалис
                                    //6,    //6  Пауки           //13-06-24
                                    6,      //7
                                    6,      //8
                                    6,      //9
                                    6,      //10
                                    6,      //11
                                    6
            };

            return WaitingTurn[counter];
        }

        /// <summary>
        /// возвращает время ожидания подбора дропа для выбранной точки маршрута
        /// </summary>
        /// <param name="counter">номер точки маршрута</param>
        /// <returns>время в милисекундах</returns>
        public int GetWaitingTimeForDropPicking(int counter)
        {
            int[] WaitingTime = {   
                                    //0, 
                                    5000,       //1
                                    10000,      //2
                                    //6000,     //3           //13-06-24
                                    //5000,     //4
                                    1000,       //5  Кризалис
                                    //1000,     //6  Пауки           //13-06-24
                                    5000,       //7
                                    1000,       //8
                                    6000,       //9
                                    5000,       //10
                                    5000,       //11
                                    5000
            };

            return WaitingTime[counter];
        }


        /// <summary>
        /// начинаем собирать лут справа от героев
        /// </summary>
        /// <param name="NumberOfPoint">номер точки сбора лута</param>
        private void HarvestToRight(int NumberOfPoint)
        {
            Point[] PointsToRight = {
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(765 - 5 + xx + 5, 408 - 5 + yy + 5)
                                    }; 
            HarvestMode();
            //точка, куда тыкаем, может различаться в зависимости от номера комнаты, где остановилсь для сбора лута
            PointsToRight[NumberOfPoint].PressMouseL();    //нажимаем в точку справа от героев, чтобы начать подбор
            Pause(500);
        }

        /// <summary>
        /// начинаем собирать лут слева от героев
        /// </summary>
        /// <param name="NumberOfPoint">номер точки сбора лута</param>
        private void HarvestToLeft(int NumberOfPoint)
        {
            Point[] PointsToLeft = {
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5),
                                        new Point(217 - 5 + xx + 5, 408 - 5 + yy + 5)
                                    };
            HarvestMode();
            //точка, куда тыкаем, может различаться в зависимости от номера комнаты, где остановилсь для сбора лута
            PointsToLeft[NumberOfPoint].PressMouseL();    //нажимаем в точку справа от героев, чтобы начать подбор
            Pause(500);
        }


        /// <summary>
        /// собираем дроп справа от героев
        /// </summary>
        /// <param name="Period">время сбора</param>
        public void GetDropCastiliaRight(int NumberOfPoint)
        {
            for (int i = 1; i <= 2; i++)
                if (!isHarvestMode()) HarvestToRight(NumberOfPoint);

            //новый вариант. ожидаем до тех пор, пока идёт сбор лута (пока горит соответствующая кнопка)
            while (isHarvestMode())
                Pause(500);
            
            // старый вариант. ожидаем конца сбора лута конкретное количество секунд
            //Pause(Period);

        }

        /// <summary>
        /// собираем дроп слева от героев
        /// </summary>
        /// <param name="Period">время сбора</param>
        public void GetDropCastiliaLeft(int NumberOfPoint)
        {
            for (int i = 1; i <= 2; i++)                //два раза пытаемся начать собирать лут. С первого раза может не получиться, если тыкнуть точно в дроп на земле
                if (!isHarvestMode()) HarvestToLeft(NumberOfPoint);

            //новый вариант. ожидаем до тех пор, пока идёт сбор лута (пока горит соответствующая кнопка)
            while (isHarvestMode())
                Pause(500);

            // старый вариант. ожидаем конца сбора лута конкретное количество секунд
            //Pause(Period);
        }




        /// <summary>
        /// подбор дропа в миссии Кастилия 
        /// </summary>
        /// <param name="Period">кол-во милисекунд ожидания подбора</param>
        //public void GetDropCastilia(int Period)
        //{
        //    GetDropCastiliaRight(Period);

        //    BattleModeOnDem();
        //    Pause(1500);

        //    GetDropCastiliaLeft(Period);
        //}

        /// <summary>
        /// нажимаем кнопку Сундук (Harvest Mode) для начала подбора дропа
        /// </summary>
        public void HarvestMode()
        {
            new Point(123 - 5 + xx + 5, 526 - 5 + yy + 5).PressMouseL();    //нажимаем на сундук (иконка подбора)
            Pause(500);
        }

        #endregion

        #region подготовка новых ботов для БХ

        /// <summary>
        /// бежим к линдону
        /// </summary>
        public void RunToLindon()
        {
            botwindow.FirstHero();

            OpenMapForState();                         //открыли карту Alt+Z
            Pause(1500);

            pointLindonOnMap.PressMouseL();            //выбрали Линдона
            Pause(1000);

            town_begin.ClickMoveMap();                 //нажимаем на кнопку Move на карте
            Pause(10000);

            botwindow.PressEscThreeTimes();
            Pause(1000);

            new Point(454 - 5 + xx, 161 - 5 + yy).PressMouseL();           //нажимаем на голову Master Guardian
            Pause(3000);
        }

        /// <summary>
        /// находимся на странице распределения очков семьи (Family Attribute)
        /// </summary>
        /// <returns></returns>
        public bool isFamilyAttribute()
        {
            return new PointColor(253 - 5 + xx, 98 - 5 + yy, 15132648, 0).isColor() &&
                   new PointColor(253 - 5 + xx, 99 - 5 + yy, 15132648, 0).isColor();
        }

        /// <summary>
        /// потратить очки семьи на странице распределения очков семьи (Family Attribute)
        /// </summary>
        public void FamilyAttributeAdded()
        {
            for (int i = 1; i <= 5; i++) new Point(280 - 5 + xx, 142 - 5 + yy).PressMouseL();  //атака

            FamilyAttributeBookmark(2);
            new Point(280 - 5 + xx, 142 - 5 + yy).PressMouseLL();  //атака
            new Point(500 - 5 + xx, 214 - 5 + yy).PressMouseLL();  //норм атака

            FamilyAttributeBookmark(1);
            for (int i = 1; i <= 5; i++) new Point(351 - 5 + xx, 142 - 5 + yy).PressMouseL();  //скорость

            new Point(709 - 5 + xx, 625 - 5 + yy).PressMouseL();   //Save
            new Point(798 - 5 + xx, 69 - 5 + yy).PressMouseL();    //закрыть крестиком
        }

        /// <summary>
        /// распределяем оставшиеся очки семьи на странице распределения очков семьи (Family Attribute)
        /// </summary>
        public void FamilyAttributeAdded2()
        {
            TopMenu(9, 8);  //открываем Ctrl+T
            FamilyAttributeBookmark(3);
            for (int i = 1; i <= 5; i++) new Point(572 - 5 + xx, 142 - 5 + yy).PressMouseL();  //атака по мобам

            FamilyAttributeBookmark(1);
            for (int i = 1; i <= 5; i++) new Point(280 - 5 + xx, 142 - 5 + yy).PressMouseL();  //атака
            for (int i = 1; i <= 5; i++) new Point(351 - 5 + xx, 142 - 5 + yy).PressMouseL();  //скорость

            new Point(709 - 5 + xx, 625 - 5 + yy).PressMouseL();   //Save
            new Point(798 - 5 + xx, 69 - 5 + yy).PressMouseL();    //закрыть крестиком
        }

        /// <summary>
        /// потратить очки семьи на странице распределения очков семьи (Family Attribute)
        /// </summary>
        public void FamilyAttributeBookmark(int bookmark)
        {
            switch (bookmark)
            {
                case 1:
                    new Point(286 - 5 + xx, 100 - 5 + yy).PressMouseL();
                    break;
                case 2:
                    new Point(378 - 5 + xx, 100 - 5 + yy).PressMouseL();
                    break;
                case 3:
                    new Point(458 - 5 + xx, 100 - 5 + yy).PressMouseL();
                    break;
            }
        }

        /// <summary>
        /// голова Master Guardian
        /// </summary>
        public void HeadMasterGuardian()
        {
            botwindow.PressEscThreeTimes();
            Pause(500);

            new Point(446 - 5 + xx, 194 - 5 + yy).PressMouseL();           //нажимаем на голову Master Guardian
            Pause(3000);
        }

        /// <summary>
        /// голова Master Guardian
        /// </summary>
        public void HeadMasterGuardian2()
        {
            botwindow.PressEscThreeTimes();
            Pause(500);

            botwindow.SecondHero();
            Pause(1000);

            new Point(413 - 5 + xx, 188 - 5 + yy).PressMouseL();           //нажимаем на голову Master Guardian
            Pause(3000);
        }

        /// <summary>
        /// голова Master Guardian
        /// </summary>
        public void HeadMasterGuardian3()
        {
            botwindow.PressEscThreeTimes();
            Pause(1000);

            botwindow.ThirdHero();
            Pause(1000);

            new Point(413 - 5 + xx, 188 - 5 + yy).PressMouseL();           //нажимаем на голову Master Guardian
            Pause(3000);
        }

        /// <summary>
        /// получить подарки за уровень семьи (Alt+L)
        /// </summary>
        public void GiveGiftsAltL()
        {
            TopMenu(9, 9);

            while (new PointColor(937 - 5 + xx, 175 - 5 + yy, 15986174, 0).isColor())
            {
                new Point(937 - 5 + xx, 175 - 5 + yy).PressMouseL();
                new Point(500 - 5 + xx, 500 - 5 + yy).FastMove();
                Pause(1500);
            }

            while (new PointColor(937 - 5 + xx, 211 - 5 + yy, 15986174, 0).isColor())
            {
                new Point(937 - 5 + xx, 211 - 5 + yy).PressMouseL();
                new Point(500 - 5 + xx, 500 - 5 + yy).FastMove();
                Pause(1500);
            }

            botwindow.PressEscThreeTimes();
            //new Point(1007 - 5 + xx, 99 - 5 + yy).PressMouseL();  //закрыли крестиком

        }

        /// <summary>
        /// использовать все levelUpGiftBox в инвентаре
        /// </summary>
        /// <returns></returns>
        public bool UseGiftBoxes()
        {
            Thing levelUpGiftBox = new Thing(new PointColor(698, 176, 13238257, 0), new PointColor(701, 176, 13500159, 0));
            bool result = UseItem(levelUpGiftBox);
            return result;
        }

        /// <summary>
        /// проверяем флинтлок в магазине стоек
        /// </summary>
        /// <returns></returns>
        public bool isFlintlock()
        {
            return new PointColor(155 - 5 + xx, 538 - 5 + yy, 12048866, 0).isColor() &&
                   new PointColor(155 - 5 + xx, 530 - 5 + yy, 12048866, 0).isColor();
        }

        /// <summary>
        /// нажимаем на стрелку прокрутки списка в магазине стоек
        /// </summary>
        public void ArrowDown()
        {
            new Point(662 - 5 + xx, 542 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// купить флинтлок 2 штуки в магазине стоек
        /// </summary>
        public void BuyingFlintlocks()
        {
            for (int i = 1; i <= 2; i++) new Point(378 - 5 + xx, 535 - 5 + yy).PressMouseL();  //кол-во

            new Point(742 - 5 + xx, 589 - 5 + yy).PressMouseL();   //кнопка купить
            Pause(2000);
            new Point(863 - 5 + xx, 589 - 5 + yy).DoubleClickL();   //кнопка закрыть
            Pause(2000);
        }

        /// <summary>
        /// использовать флинтлоки
        /// </summary>
        public bool UseStanceBooks()
        {
            Thing StanceBook = new Thing(new PointColor(707, 184, 11335674, 0), new PointColor(703, 192, 12904947, 0));

            //int ii;
            //int jj;
            bool result = FindItem(StanceBook, out int ii, out int jj);

            botwindow.FirstHero();
            UseThingInCell(ii, jj);

            botwindow.SecondHero();
            UseThingInCell(ii, jj);

            return result;
        }

        /// <summary>
        /// включить стойку Флинтлок у первых двух персов
        /// </summary>
        public void FlintlockIsOn()
        {
            new Point(110 - 5 + xx, 670 - 5 + yy).PressMouseLL();
            new Point(365 - 5 + xx, 670 - 5 + yy).PressMouseLL();
        }

        /// <summary>
        /// увеличиваем всем персам характеристику DEX
        /// </summary>
        public void AddDex()
        {
            new Point(219 - 5 + xx, 672 - 5 + yy).PressMouseL();
            Pause(1000);
            new Point(247 - 5 + xx, 186 - 5 + yy).PressMouseL();    //прибавляем Декс у первого перса
            Pause(1000);
            AnswerYesOrNo(true);

            new Point(474 - 5 + xx, 672 - 5 + yy).PressMouseL();
            Pause(1000);
            new Point(502 - 5 + xx, 186 - 5 + yy).PressMouseL();    //прибавляем Декс у 2-го перса
            Pause(1000);
            AnswerYesOrNo(true);

            new Point(730 - 5 + xx, 672 - 5 + yy).PressMouseL();
            Pause(1000);
            new Point(757 - 5 + xx, 186 - 5 + yy).PressMouseL();    //прибавляем Декс у 3-го перса
            Pause(1000);
            AnswerYesOrNo(true);

        }

        #endregion

        #region Работа с инвентарем и CASH-инвентарем


        #region Обычный инвентарь

        /// <summary>
        /// выбор i-го героя
        /// </summary>
        /// <param name="i"></param>
        protected void SelectHero(int i)
        {
            switch (i)
            {
                case 1:
                    botwindow.FirstHero();
                    break;
                case 2:
                    botwindow.SecondHero();
                    break;
                case 3:
                    botwindow.ThirdHero();
                    break;
            }
        }

        /// <summary>
        /// надеть всю бижутерию на i-го героя
        /// </summary>
        /// <param name="i">номер героя (1,2 или 3)</param>
        public void WearJewerly(int i)
        {
            OpenDetailInfo(i);
            SelectHero(i);

            iPoint Inventory = new Point(840 - 5 + xx, 100 - 5 + yy);
            iPoint DetailInfo = new Point(63 - 5 + xx + (i - 1) * 255, 346 - 5 + yy);
            if (isEmptyBelt(i))
            {
                Inventory.PressMouseLL();
                UseItem(DesapioBelt);
                DetailInfo.PressMouseLL();
            }
            if (isEmptyBoots(i))
            {
                Inventory.PressMouseLL();
                UseItem(DesapioBoots);
                DetailInfo.PressMouseLL();
            }
            if (isEmptyEarrings(i))
            {
                Inventory.PressMouseLL();
                UseItem(DesapioEarrings);
                DetailInfo.PressMouseLL();
            }
            if (isEmptyGloves(i))
            {
                Inventory.PressMouseLL();
                UseItem(DesapioGloves);
                DetailInfo.PressMouseLL();
            }
            if (isEmptyNecklace(i))
            {
                Inventory.PressMouseLL();
                UseItem(DesapioNecklace);
                DetailInfo.PressMouseLL();
            }
        }

        /// <summary>
        /// ищем указанную вещь в инвентаре (инвентарь должен быть открыт на нужной закладке)
        /// </summary>
        /// <param name="thing">искомая вещь</param>
        /// <param name="ii">строка с найденной вещью</param>
        /// <param name="jj">столбец с найденной вещью</param>
        /// <returns></returns>
        protected bool FindItem(Thing thing, out int ii, out int jj)
        {
            ii = 1;
            jj = 1;
            bool result = false;   //объект в кармане пока не найден
            for (int i = 1; i <= 5; i++)         //строки в инвентаре
            {
                for (int j = 1; j <= 8; j++)        // столбцы в инвентаре
                {
                    if (!result)         //если вещь уже найдена, то дальше не ищем
                    {
                        if (CheckThingInCell(i, j, thing))
                        {
                            ii = i;
                            jj = j;
                            result = true;
                            Pause(500);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// используем указанную вещь в инвентаре (инвентарь должен быть открыт на нужной закладке)
        /// </summary>
        protected bool UseItem(Thing thing)
        {
            bool result = false;   //объект в инвентаре пока не найден

            if (FindItem(thing, out int i, out int j))
            {
                UseThingInCell(i, j);
                Pause(500);
                result = true;
            }
            MoveCursorOfMouse();
            Pause(500);
            return result;
        }

        /// <summary>
        /// открыт ли инвентарь?
        /// </summary>
        /// <returns></returns>
        public bool isOpenInventory()
        {
            return pointisOpenInventory1.isColor() &&
                   pointisOpenInventory2.isColor();
        }

        /// <summary>
        /// открываем инвентарь на нужной закладке
        /// </summary>
        /// <param name="numberOfbookmark">номер закладки</param>
        public void OpenInventory(int numberOfbookmark)
        {
            if (!isOpenInventory()) TopMenu(8, 1, false);
            Pause(1000);

            new Point(713 - 5 + xx + 61 * (numberOfbookmark - 1), 152 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// проверяем, лежит ли вещь thing в инвентаре в ячейке i,j
        /// </summary>
        /// <param name="i">строка</param>
        /// <param name="j">столбец</param>
        /// <param name="thing">вещь</param>
        /// <returns></returns>
        protected bool CheckThingInCell(int i, int j, Thing thing)
        {
            return new PointColor(thing.point1.X - 5 + xx + (j - 1) * 39, thing.point1.Y - 5 + yy + (i - 1) * 38, thing.point1.Color, 0).isColor() &&
                   new PointColor(thing.point2.X - 5 + xx + (j - 1) * 39, thing.point2.Y - 5 + yy + (i - 1) * 38, thing.point2.Color, 0).isColor();
        }

        /// <summary>
        /// использовать предмет в указанной ячейве инвентаря
        /// </summary>
        /// <param name="i">строка</param>
        /// <param name="j">столбец</param>
        protected void UseThingInCell(int i, int j)
        {
            new Point(702 - 5 + xx + (j - 1) * 39, 183 - 5 + yy + (i - 1) * 38).DoubleClickL();
        }

        /// <summary>
        /// перекладываем указанную вещь во все слоты (инвентарь должене быть открыт на нужной закладке)
        /// </summary>
        protected bool DragItemToManaSlots(Thing thing)
        {
            bool result = false;   //объект в кармане пока не найден

            //for (int i = 1; i <= 5; i++)         //строки в инвентаре
            //{
            //    for (int j = 1; j <= 8; j++)        // столбцы в инвентаре
            //    {
            //        if (!result)
            //        {
            //            if (CheckThingInCell(i, j, thing))
            //            {
            //                DragItemToSlot(i, j, 1);
            //                DragItemToSlot(i, j, 2);
            //                DragItemToSlot(i, j, 3);
            //                new Point(1500, 800).Move();   //убираем мышь в сторону
            //                result = true;
            //                Pause(500);
            //            }
            //        }
            //    }
            //}

            //int i, j;

            if (FindItem(thing, out int i, out int j))
            {
                result = true;
                DragItemToSlot(i, j, 1);
                DragItemToSlot(i, j, 2);
                DragItemToSlot(i, j, 3);
                MoveCursorOfMouse();
                Pause(500);
            }

            return result;
        }

        /// <summary>
        /// перетащить вещь из ячейки инвентаря (строка i столбец j) в слот маны под номером Slot
        /// </summary>
        /// <param name="i">строка</param>
        /// <param name="j">столбец</param>
        /// <param name="Slot">номер слота маны</param>
        protected void DragItemToSlot(int i, int j, int Slot)
        {
            iPoint pointBegin = new Point(700 - 5 + xx + (j - 1) * 39, 182 - 5 + yy + (i - 1) * 38);
            iPoint pointSlot = new Point(244 - 5 + xx + (Slot - 1) * 255, 700 - 5 + yy);   //конечная точка перемещения

            pointBegin.Drag(pointSlot);
            Pause(1000);
        }

        /// <summary>
        /// перетащить мастер свиток в слоты маны (в процессе открываем инвентарь)
        /// </summary>
        public bool DragMasterCardToManaSlots()
        {
            OpenInventory(2);
            botwindow.Pause(1000);

            return DragItemToManaSlots(masterScroll);
        }

        /// <summary>
        /// перетащить любую карту опыта в слоты маны
        /// </summary>
        public bool DragExpCardToManaSlots()
        {
            OpenInventory(2);
            botwindow.Pause(1000);

            return DragItemToManaSlots(expCard);
        }

        /// <summary>
        /// перетащить вещь из ячейки инвентаря (строка i столбец j) в координаты x,y
        /// </summary>
        /// <param name="i">строка</param>
        /// <param name="j">столбец</param>
        /// <param name="Slot">номер слота маны</param>
        protected void DragItemToXY(int i, int j, int x, int y)
        {
            iPoint pointEnd = new Point(x - 5 + xx, y - 5 + yy);
            iPoint pointBegin = new Point(700 - 5 + xx + (j - 1) * 39, 182 - 5 + yy + (i - 1) * 38);

            pointBegin.Drag(pointEnd);
            Pause(1000);
        }

        /// <summary>
        /// перекладываем указанную вещь по указанным координатам (инвентарь должене быть открыт на нужной закладке)
        /// </summary>
        protected bool DragItemToXY(Thing thing, int x, int y)
        {
            bool result = false;   //объект в кармане пока не найден
            //int i, j;

            if (FindItem(thing, out int i, out int j))
            {
                DragItemToXY(i, j, x, y);
                Pause(500);
                MoveCursorOfMouse();
                result = true;
                Pause(500);
            }

            return result;
        }

        ///// <summary>
        ///// перетаскивание коробки с подарком в ячейку Mana1
        ///// </summary>
        //public void MoveGiftBox()
        //{
        //    iPointColor pointmovegift = new PointColor(237 - 5 + xx, 701 - 5 + yy, 16700000, 5);
        //    if (!pointmovegift.isColor())    //если нет на месте коробки с подарком
        //    {
        //        //перекладываем коробку с подарком в ячейку Mana1
        //        TopMenu(8, 1);
        //        new Point(778 - 5 + xx, 152 - 5 + yy).PressMouseLL();   //вторая закладка инвентаря

        //        for (int i = 1; i <= 5; i++)         //строки в инвентаре
        //        {
        //            for (int j = 1; j <= 8; j++)        // столбцы в инвентаре
        //            {
        //                if (new PointColor(694 - 5 + xx + (j - 1) * 39, 182 - 5 + yy + (i - 1) * 38, 16700000, 5).isColor())
        //                {
        //                    new Point(694 - 5 + xx + (j - 1) * 39, 182 - 5 + yy + (i - 1) * 38).Drag(new Point(237 - 5 + xx, 701 - 5 + yy));
        //                    Pause(1000);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}     // нигде не применяется !!!!!!!!!!!!!!!!!!!!

        #endregion

        #region CASH-инвентарь


        /// <summary>
        /// если в середине экрана возникает запрос на подтверждение, то ответить
        /// </summary>
        /// <param name="answer">если true, то ответить положительно</param>
        protected void AnswerYesOrNo(bool answer)
        {
            if (new PointColor(559 - 5 + xx + xxx, 420 - 5 + yy + yyy, 7727344, 0).isColor() ||
                new PointColor(559 - 5 + xx + xxx, 426 - 5 + yy + yyy, 7727344, 0).isColor())  //проверка, есть ли запрос на экране
                if (answer)
                    new Point(465 - 5 + xx + xxx, 422 - 5 + yy + yyy).PressMouseL();   //да
                else
                    new Point(565 - 5 + xx + xxx, 422 - 5 + yy + yyy).PressMouseL();   //нет
        }

        /// <summary>
        /// открыт ли специнвентарь (Cash Item)
        /// буква I в слове Item и буква l в слове Ctrl+F    //19-02-24
        /// </summary>
        /// <returns></returns>
        public bool isOpenSpecInventory()
        {
            return new PointColor(107 - 5 + xx, 124 - 5 + yy, 16700000, 5).isColor() &&
                   new PointColor(107 - 5 + xx, 136 - 5 + yy, 16700000, 5).isColor() &&
                   new PointColor(170 - 5 + xx, 129 - 5 + yy, 11000000, 6).isColor() &&
                   new PointColor(170 - 5 + xx, 136 - 5 + yy, 11000000, 6).isColor();
        }

        /// <summary>
        /// открыть специнвентарь на указанной закладке //19-02-24
        /// </summary>
        public void OpenSpecInventory(int bookmark)
        {
            if (!isOpenSpecInventory()) OpenSpecInventory();
            Pause(1000);
            SpecInventoryBookmark(bookmark);
        }

        /// <summary>
        /// открыть коробку с розовыми крыльями (специнвентарь уже открыт)  
        /// </summary>
        public void OpenWingsBox()
        {
            PuttingItem(PinkWingsBox);
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);
        }

        /// <summary>
        /// надеть крылья (сначала открываем специнвентарь, а после надевания крыльев закрываем его)  
        /// </summary>
        public void PutPinkWings()
        {
            OpenSpecInventory(4);

            botwindow.FirstHero();
            PuttingItem(PinkWings);
            //            new Point(36 - 5 + xx, 211 - 5 + yy).DoubleClickL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);

            botwindow.SecondHero();
            PuttingItem(PinkWings);
            //            new Point(36 - 5 + xx, 211 - 5 + yy).DoubleClickL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);

            botwindow.ThirdHero();
            PuttingItem(PinkWings);
            //            new Point(36 - 5 + xx, 211 - 5 + yy).DoubleClickL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);

            botwindow.PressEsc();
        }

        /// <summary>
        /// надеваем оружие и броню
        /// </summary>
        public void PuttingOnWeaponsAndArmors()
        {
            //Item Coat = new Item(35, 209, 16645630);
            //Item Necklace = new Item(36, 219, 12179686);
            //Item Glove = new Item(26, 205, 2906307);
            //Item Shoes = new Item(33, 202, 16777215);
            //Item Belt = new Item(35, 209, 11372402);
            //Item Earring = new Item(37, 205, 11263973);
            //Item Rifle = new Item(31, 214, 12567238);

            Item[] items = new Item[7] { Coat, Necklace, Glove, Shoes, Belt, Earring, Rifle };

            SpecInventoryBookmark(1);

            botwindow.FirstHero();
            for (int i = 0; i <= 6; i++) PuttingItem(items[i]);     //надеваем амуницию

            botwindow.SecondHero();
            for (int i = 0; i <= 6; i++) PuttingItem(items[i]);     //надеваем амуницию

            botwindow.ThirdHero();
            for (int i = 0; i <= 6; i++) PuttingItem(items[i]);     //надеваем амуницию
        }

        /// <summary>
        /// открываем закладку с указанным номером в уже открытом спец инвентаре (Cash Item):       //19-02-24
        /// первая - Equip; вторая - Expend; третья - Misk; четвертая - Costume.
        /// </summary>
        /// <param name="n">номер закладки</param>
        public void SpecInventoryBookmark(int n)
        {
            new Point(70 - 5 + xx + (n - 1) * 60, 204 - 5 + yy).PressMouseLL();     
        }

        /// <summary>
        /// применяем журнал в специнвентаре     //19-02-24
        /// </summary>
        public void ApplyingJournal()
        {
            OpenSpecInventory(3);

            PuttingItem(Journal);

            AnswerYesOrNo(true);
        }

        /// <summary>
        /// открыть специнвентарь       //19-02-24
        /// </summary>
        public void OpenSpecInventory()
        {
            TopMenu(16, 2,true);
        }

        /// <summary>
        /// применяем вещь в специнвентаре
        /// </summary>
        protected void PuttingItemOld(Item item)
        {
            bool result = false;                    //объект в специнвентаре пока не найден
            for (int i = 1; i <= 6; i++)            //строки в инвентаре
            {
                for (int j = 1; j <= 5; j++)        // столбцы в инвентаре
                {
                    if (!result)
                    {
                        if (new PointColor(item.x - 5 + xx + (j - 1) * 54, item.y - 5 + yy + (i - 1) * 54, item.color, 0).isColor())
                        {
                            new Point(35 - 5 + xx + (j - 1) * 54, 209 - 5 + yy + (i - 1) * 54).DoubleClickL();
                            new Point(1500, 800).Move();   //убираем мышь в сторону
                            Pause(500);

                            result = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// применяем вещь в специнвентаре (новый метод)
        /// </summary>
        protected bool PuttingItem(Item item)
        {
            //Point point;

            if (FindItemFromSpecInventory(item, 0, out Point point))
            {
                point.DoubleClickL();
                new Point(1500, 800).Move();   //убираем мышь в сторону
                Pause(500);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Находим конкретную вещь (Item) в сдвинутом спец инвентаре в текущей закладке  //19-02-24
        /// </summary>
        /// <param name="item">искомая вещь</param>
        /// <param name="sdvig">величина сдвига специнвентаря по оси Х. Если инвентарь не сдвинут, то сдвиг = 0</param>
        /// <param name="point">если вещь найдена, то возвращаем точку с координатами вещи</param>
        /// <returns>если вещь найдена, то возвращаем true</returns>
        protected bool FindItemFromSpecInventory(Item item, int sdvig, out Point point)
        {
            MoveCursorOfMouse();
            point = new Point(0, 0);                //возвращаемый объект. не убирать
            bool result = false;                    //объект в кармане пока не найден

            for (int i = 1; i <= 6; i++)            //строки в инвентаре
            {
                for (int j = 1; j <= 5; j++)        // столбцы в инвентаре
                {
                    int xxx = item.x - 5 + xx + (j - 1) * 54 + sdvig;  //координаты исследуемой точки
                    int yyy = item.y - 5 + yy + (i - 1) * 54;
                    if (!result)
                    {
                        if (new PointColor(xxx, yyy, item.color, 0).isColor())
                        {
                            result = true;
                            point = new Point(xxx, yyy);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// перемещаем вещь из открытого специнвентаря на левую панель в конкрентую ячейку
        /// </summary>
        /// <param name="item">вещь</param>
        /// <param name="CellNumber">номер ячейки, куда перемещаем вещь</param>
        protected void MoveItemOnLeftPanelFromSpecInventory(Item item, int sdvig, int CellNumber)
        {
            bool result;

            //Point pointBegin;
            result = FindItemFromSpecInventory(item, sdvig, out Point pointBegin);
            if (result)
            {
                Point pointEnd = new Point(31 - 5 + xx, 114 - 5 + (CellNumber - 1) * 32 + yy);
                pointBegin.Drag(pointEnd);
            }
            Pause(500);
        }

        /// <summary>
        /// перемещаем окно специнвентаря по оси X на sdvig точек
        /// </summary>
        /// <param name="sdvig">смещение по оси Х</param>
        public void MoveSpecInventory(int sdvig)
        {
            new Point(165 - 5 + xx, 130 - 5 + yy).Drag(new Point(165 + sdvig - 5 + xx, 130 - 5 + yy));
        }

        /// <summary>
        /// перемещаем курсор мыши прочь от специнвентаря и инвентаря
        /// </summary>
        public void MoveCursorOfMouse()
        {
            //new Point(754 - 5 + xx, 491 - 5 + yy).Move();
            new Point(670 - 5 + xx, 640 - 5 + yy).Move();
        }

        /// <summary>
        /// перемещаем три бутылки (хрин + принципал + элик славы) на левую панель
        /// </summary>
        public void MoveBottlesToTheLeftPanel()
        {
            int sdvig = 40;
            OpenSpecInventory();
            Pause(500);
            SpecInventoryBookmark(2);
            Pause(500);
            MoveSpecInventory(sdvig);
            Pause(500);

            MoveItemOnLeftPanelFromSpecInventory(SteroidOneHour, sdvig, 5);  //5 - номер ячейки
            MoveItemOnLeftPanelFromSpecInventory(Steroid10Hours, sdvig, 5);
            MoveItemOnLeftPanelFromSpecInventory(Steroid10Hours2, sdvig, 5);
            MoveItemOnLeftPanelFromSpecInventory(Steroid10HoursHP, sdvig, 5);

            MoveItemOnLeftPanelFromSpecInventory(PrincipleOneHour, sdvig, 6);
            MoveItemOnLeftPanelFromSpecInventory(Principle10Hours, sdvig, 6);

            MoveItemOnLeftPanelFromSpecInventory(Triumph, sdvig, 7);
        }

        /// <summary>
        /// проверяем, есть ли указанная вещь item на левой панели
        /// </summary>
        /// <param name="item">искомая вещь</param>
        /// <returns></returns>
        public bool isItemOnLeftPanel(Item item)
        {
            PointColor Bottle = new PointColor(item.x - 5 + xx, item.y - 5 + yy, item.color, 0);
            return Bottle.isColor();
        }

        /// <summary>
        /// находятся ли бутылки (хрин+принципал+элик славы) на левой панели 
        /// </summary>
        /// <returns></returns>
        public bool isBottlesOnLeftPanel()
        {
            return (isItemOnLeftPanel(Steroid10HoursLeftPanel) || isItemOnLeftPanel(SteroidOneHourLeftPanel) || isItemOnLeftPanel(Steroid10HoursHPLeftPanel)) &&
                    (isItemOnLeftPanel(Principle10HoursLeftPanel) || isItemOnLeftPanel(PrincipleOneHourLeftPanel) || isItemOnLeftPanel(Principle10HoursHPLeftPanel));
            //можно добавить проверку и для эликов славы
        }

        #endregion

        #region Detail Info (сведения о персонаже и  просмотр экипировки)

        /// <summary>
        /// открыт ли Detail Info для первого персонажа?
        /// </summary>
        /// <returns>true, если открыт</returns>
        public bool isOpenDetailInfo(int i)
        {
            return new PointColor(62 - 5 + xx + (i - 1) * 255 + 5, 345 - 5 + yy + 5, 14000000, 6).isColor() &&
                   new PointColor(62 - 5 + xx + (i - 1) * 255 + 5, 346 - 5 + yy + 5, 14000000, 6).isColor();
        }

        /// <summary>
        /// открыть Detail Info у первого героя
        /// </summary>
        public void OpenDetailInfo(int i)
        {
            if (!isOpenDetailInfo(i))
                new Point(220 - 5 + xx + (i - 1) * 255, 671 - 5 + yy).PressMouseL();
            MoveCursorOfMouse();
        }

        /// <summary>
        /// экипированы розовые крылья у первого героя
        /// </summary>
        /// <returns>true, если экипирпованы</returns>
        public bool isWearingPinkWings()
        {
            return new PointColor(163 - 5 + xx + 5, 363 - 5 + yy + 5, 1579516, 0).isColor();
        }

        /// <summary>
        /// У i-го героя проверяем, пуст ли слот с ожерельем (Detail info уже открыт)
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если слот пуст</returns>
        public bool isEmptyNecklace(int i)
        {
            return new PointColor(158 + (i - 1) * 255 - 5 + xx, 328 - 5 + yy, 6576231, 0).isColor();
        }

        /// <summary>
        /// У i-го героя проверяем, пуст ли слот с серьгой (Detail info уже открыт)
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если слот пуст</returns>
        public bool isEmptyEarrings(int i)
        {
            return new PointColor(196 + (i - 1) * 255 - 5 + xx, 287 - 5 + yy, 6844014, 0).isColor();
        }

        /// <summary>
        /// У i-го героя проверяем, пуст ли слот с поясом (Detail info уже открыт)
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если слот пуст</returns>
        public bool isEmptyBelt(int i)
        {
            return new PointColor(196 + (i - 1) * 255 - 5 + xx, 411 - 5 + yy, 9935004, 0).isColor();
        }

        /// <summary>
        /// У i-го героя проверяем, пуст ли слот с перчатками (Detail info уже открыт)
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если слот пуст</returns>
        public bool isEmptyGloves(int i)
        {
            //            return new PointColor(198 + (i - 1) * 255 - 5 + xx, 438 - 5 + yy, 11711155, 0).isColor();
            return new PointColor(192 + (i - 1) * 255 - 5 + xx, 450 - 5 + yy, 3223604, 0).isColor();
        }

        /// <summary>
        /// У i-го героя проверяем, пуст ли слот с ботинками (Detail info уже открыт)
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если слот пуст</returns>
        public bool isEmptyBoots(int i)
        {
            return new PointColor(195 + (i - 1) * 255 - 5 + xx, 485 - 5 + yy, 4079167, 0).isColor();
        }

        #endregion

        #endregion

        #region Работа со слотами маны

        /// <summary>
        /// есть ли свиток во всех слотах манны?
        /// </summary>
        /// <returns>true, если свиток во всех слотах </returns>
        public bool isScrollinAllManaSlots()
        {
            return new PointColor(236 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor() &&
                   new PointColor(491 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor() &&
                   new PointColor(746 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor();
        }

        /// <summary>
        /// есть ли свитки хоть в каком-нибудь слоте маны?
        /// </summary>
        /// <returns>true, если свитков в слотах нет </returns>
        public bool isOutScrollinAllManaSlots()
        {
            return new PointColor(236 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor() ||
                   new PointColor(491 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor() ||
                   new PointColor(746 - 5 + xx, 709 - 5 + yy, 11335674, 0).isColor();
        }

        /// <summary>
        /// есть ли свиток в указанном слоте манны?
        /// </summary>
        /// <param name="Slot"></param>
        /// <returns></returns>
        public bool isScrollinManaslot(int Slot)
        {
            bool result;

            iPointColor pointSlot;

            switch (Slot)
            {
                case 1:
                    pointSlot = new PointColor(236 - 5 + xx, 709 - 5 + yy, 11335674, 0);
                    break;
                case 2:
                    pointSlot = new PointColor(491 - 5 + xx, 709 - 5 + yy, 11335674, 0);
                    break;
                case 3:
                    pointSlot = new PointColor(746 - 5 + xx, 709 - 5 + yy, 11335674, 0);
                    break;
                default:
                    pointSlot = new PointColor(236 - 5 + xx, 709 - 5 + yy, 11335674, 0);
                    break;
            }

            result = pointSlot.isColor();
            return result;
        }

        /// <summary>
        /// применить вещи, лежащие в слотах маны 
        /// </summary>
        public void ApplyItems()
        {
            pointMana1.PressMouseL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);
            pointMana2.PressMouseL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);
            pointMana3.PressMouseL();
            Pause(500);
            AnswerYesOrNo(true);
            Pause(500);
        }

        /// <summary>
        /// применить вещи, лежащие в слотах маны 
        /// </summary>
        public void ApplyItems(bool confirmation)
        {
            pointMana1.FastPressMouseL();
            pointMana2.FastPressMouseL();
            pointMana3.FastPressMouseL();
            new Point(500 - 5 + xx, 500 - 5 + yy).FastMove();
        }

        #endregion

        #region Вход-выход

        ///// <summary>
        ///// проверяем правильный ли диалог для начала миссии у фонтана
        ///// </summary>
        ///// <returns></returns>
        //public bool isBeginOfMission()
        //{
        //    return (pointisBeginOfMission1.isColor() || pointisBeginOfMission2.isColor());
        //}

        ///// <summary>
        ///// нажимаем на голову перса на фонтане
        ///// </summary>
        //public void PressHeadSharon()
        //{
        //    new Point(238 - 5 + xx, 320 - 5 + yy).PressMouseL();
        //}

        ///// <summary>
        ///// нажимаем на голову перса на фонтане после прохождения миссии
        ///// </summary>
        //public void PressHeadSharon2()
        //{
        //    new Point(408 - 5 + xx, 271 - 5 + yy).PressMouseL();
        //    Pause(3000);
        //}

        /// <summary>
        /// проверяем, открыто ли окно от Стима "Что нового?"
        /// </summary>
        /// <returns></returns>
        public bool isWhatNews()
        {
            return (pointisWhatNews1.isColor() || pointisWhatNews2.isColor());
        }

        /// <summary>
        /// нажимаем на кнопку "Закрыть", если открыто окно "Что нового?" от Steam
        /// </summary>
        public void CloseWhatNews()
        {
            if (isWhatNews()) new Point(960, 715).PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// проверяем, докачались ли все три персонажа до Мастера 10лвл 100%  (можно ли уже продвигать до Хаймастера)
        /// </summary>
        /// <returns></returns>
        public bool isHighMaster()
        {
            return new PointColor(126 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(150 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(381 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(405 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(636 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(660 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor();
        }

        /// <summary>
        /// проверяем, докачались ли все три персонажа до ХайМастера 10лвл 100%, т.е. 140лвл
        /// </summary>
        /// <returns></returns>
        public bool isHighMaster140Lvl()
        {
            return new PointColor(137 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(161 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(392 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(416 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(647 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor() &&
                    new PointColor(671 - 5 + xx, 722 - 5 + yy, 15000000, 6).isColor();
        }

        #endregion

        #region  Demonic

        /// <summary>
        /// проверяем, находимся ли на работе
        /// </summary>
        /// <returns></returns>
        public bool isWorkInDemonic()
        {
            return hero1.isWork() || hero2.isWork() || hero3.isWork();
        }

        /// <summary>
        /// проверяем, находимся ли в городе (все герои из команды уже определены)
        /// </summary>
        /// <returns></returns>
        public bool isTownDemonic()
        {
            return hero1.isTown() && hero2.isTown() && hero3.isTown();
        }
        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Насление древних" /даётся на мосту/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        public bool isAncientBlessing(int i)
        {
            bool result = false;    //бафа нет
            for (int j = 0; j < 15; j++)
                if (
                        new PointColor(78 - 5 + xx + j * 14 + (i - 1) * 255, 588 - 5 + yy, 15577230, 0).isColor() &&
                        new PointColor(86 - 5 + xx + j * 14 + (i - 1) * 255, 593 - 5 + yy, 16028031, 0).isColor()
                   ) result = true;
            if (isKillHero(i)) result = true;   //если убит i-й герой, то считаем, что у него есть бафф 

            return result;
        }

        /// <summary>
        /// тыкнуть в ворота Demonic
        /// </summary>
        public void PressToGateDemonic()
        {
            //new Point(628 - 5 + xx, 326 - 5 + yy).PressMouseL();
            new Point(528 - 5 + xx, 320 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// активируем пета (быстрый способ)
        /// </summary>
        public void ActivatePetDemonic()
        {
            new Point(372 - 5 + xx, 60 - 5 + yy).PressMouseL();
            Pause(100);
        }


        /// <summary>
        /// увеличиваем окно чата до максимума, чтобы гарантированно увидеть сообщение об окончании боя
        /// </summary>
        /// <param name="N">номер героя</param>
        public void messageWindowExtension()
        {
            for (int i = 1; i <= 5; i++)
            {
                new Point(800 - 5 + xx, 725 - 5 + yy).PressMouseL();
                Pause(100);
            }
        }

        /// <summary>
        /// делаем главным (активным) в команде указанного героя
        /// </summary>
        /// <param name="N">номер героя</param>
        public void ActiveHeroDem(int N)
        {
            new Point(213 + (N - 1) * 250 - 5 + xx, 636 - 5 + yy).DoubleClickL();
            MoveCursorOfMouse();
        }

        /// <summary>
        /// находимся в Expedition Merchant ?
        /// </summary>
        /// <returns></returns>
        public bool isExpedMerch()
        {
            return new PointColor(420 - 5 + xx, 539 - 5 + yy, 7400000, 5).isColor() &&
                    new PointColor(420 - 5 + xx, 540 - 5 + yy, 7400000, 5).isColor();
        }

        /// <summary>
        /// находимся в Faction Merchant ?
        /// </summary>
        /// <returns></returns>
        public bool isFactionMerch()
        {
            return new PointColor(378 - 5 + xx, 539 - 5 + yy, 7400000, 5).isColor() &&
                    new PointColor(378 - 5 + xx, 540 - 5 + yy, 7400000, 5).isColor();
        }

        /// <summary>
        /// находимся в Expedition Merchant (на странице с товарами)?
        /// </summary>
        /// <returns></returns>
        public bool isExpedMerch2()
        {
            return new PointColor(722 - 5 + xx, 613 - 5 + yy, 12000000, 6).isColor() 
                && new PointColor(722 - 5 + xx, 623 - 5 + yy, 12000000, 6).isColor()
                && new PointColor(843 - 5 + xx, 613 - 5 + yy, 12000000, 6).isColor()
                && new PointColor(843 - 5 + xx, 623 - 5 + yy, 12000000, 6).isColor();
        }

        /// <summary>
        /// находимся в Faction Merchant (на странице с товарами)?
        /// </summary>
        /// <returns></returns>
        public bool isFactionMerch2()
        {
            return isExpedMerch2();
        }

        /// <summary>
        /// закрываем магазин Exped Merch, нажав кнопку "Close"
        /// </summary>
        public void CloseMerchReboldo2()
        {
            new Point(843 - 5 + xx, 618 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// закрываем магазин Exped Merch
        /// </summary>
        public void CloseMerchReboldo()
        {
            //new Point(843 - 5 + xx, 616 - 5 + yy).PressMouseL();
            dialog.PressStringDialog(3);
            //dialog.PressOkButton(1);
            Pause(500);
            if (dialog.isDialog()) dialog.PressOkButton(1);
        }

        /// <summary>
        /// появилось ли напоминание об установке службы Steam
        /// </summary>
        /// <returns>true, если появилось</returns>
        public bool isSteamService()
        {
            //return new PointColor(1380, 451, 2000000, 6).isColor() &&
            //        new PointColor(1380, 452, 2000000, 6).isColor();
            return new PointColor(1380, 453, 2000000, 6).isColor()
                    && new PointColor(1380, 454, 2000000, 6).isColor()
                    && new PointColor(1380, 455, 2000000, 6).isColor();
        }

        /// <summary>
        /// закрываем уведомление об установке службы Steam
        /// </summary>
        public void CloseSteam()
        {
            new Point(1380, 451).PressMouseLL();
            new Point(1043, 560).PressMouseL();
        }

        /// <summary>
        /// Стим загружен. грузим игру для Demonic и проч.
        /// </summary>
        public abstract void RunClientDem();

        /// <summary>
        /// тыкаем в ворота Demonic (Гильдии Охотников)
        /// </summary>
        public void GoToInfinityGateDem()
        {
            //MinHeight();
            botwindow.FirstHero();
            new Point(528 - 105 + xx, 296 - 105 + yy).PressMouseL();       //316-105
        }

        /// <summary>
        /// проверяем, находимся ли в Mission Lobby
        /// </summary>
        /// <returns>true, если находимся</returns>
        public bool isMissionLobby()
        {
            return new PointColor(242 - 5 + xx, 163 - 5 + yy, 13800000, 5).isColor() &&
                    new PointColor(242 - 5 + xx, 164 - 5 + yy, 13800000, 5).isColor();  //22-11   буква М
        }

        /// <summary>
        /// создаем миссию. цель - дойти из Mission Lobby в Mission Room
        /// </summary>
        public void CreatingMission()
        {
            new Point(547 - 5 + xx, 486 - 5 + yy).PressMouseL();    // тыкаем в окно Mission Lobby, чтобы сделать его поверх остального,
                                                                    // тогда становится видимой кнопка Create Mission Room
            Pause(500);
            new Point(607 - 5 + xx, 640 - 5 + yy).PressMouseL();    //кнопка Create Mission Room
            Pause(1000);
            new Point(440 - 5 + xx, 433 - 5 + yy).PressMouseL();    //кнопка Create a Room
        }


        /// <summary>
        /// создаем миссию. цель - дойти из Mission Lobby в Mission Room
        /// </summary>
        /// <param name="weekday">день недели</param>
        public void CreatingMission(int weekday)
        {
            new Point(547 - 5 + xx, 486 - 5 + yy).PressMouseL();    // тыкаем в окно Mission Lobby, чтобы сделать его поверх остального,
                                                                    // тогда становится видимой кнопка Create Mission Room
            Pause(500);
            new Point(607 - 5 + xx, 640 - 5 + yy).PressMouseL();    //кнопка Create Mission Room
            Pause(1000);
            // на этом месте сделать выбор миссии для выходных
            // if (weekday ==6 || weekday == 7)
            //
            //
            new Point(440 - 5 + xx, 433 - 5 + yy).PressMouseL();    //кнопка Create a Room
        }

        /// <summary>
        /// проверяем, находимся ли в Mission Room
        /// </summary>
        /// <returns>true, если находимся</returns>
        public bool isWaitingRoom()
        {
            return new PointColor(177 - 5 + xx, 155 - 5 + yy, 12829635, 0).isColor() &&
                    new PointColor(177 - 5 + xx, 156 - 5 + yy, 12829635, 0).isColor();      //22-11   буква М
        }

        /// <summary>
        /// появился ли сундук в миссии? /проверка по сообщению в чате/
        /// </summary>
        /// <returns>true, если появился</returns>
        public bool isTreasureChest()
        {
            // проверяем букву Т в слове Treasure в пяти нижних строчках

            bool result = false;

            for (int i = 1; i <= 17; i++)
            {
                if (
                    //new PointColor(837 - 5 + xx, 664 - 5 + yy - (i - 1) * 19, 15500000, 5).isColor() &&       //левая верхняя точка буквы Т
                    //new PointColor(843 - 5 + xx, 664 - 5 + yy - (i - 1) * 19, 15500000, 5).isColor() &&       //правая верхняя точка буквы Т
                    //new PointColor(840 - 5 + xx, 674 - 5 + yy - (i - 1) * 19, 14900000, 5).isColor()          //нижняя средняя точка буквы Т
                    new PointColor(837 - 5 + xx, 674 - 5 + yy - (i - 1) * 19, 15500000, 5).isColor() &&         //левая верхняя точка буквы Т
                    new PointColor(843 - 5 + xx, 674 - 5 + yy - (i - 1) * 19, 15500000, 5).isColor() &&         //правая верхняя точка буквы Т
                    new PointColor(840 - 5 + xx, 684 - 5 + yy - (i - 1) * 19, 15000000, 5).isColor()            //нижняя средняя точка буквы Т 23-11
                    )
                {
                    result = true;
                    break;
                }

            }

            return result;


        }

        /// <summary>
        /// стартуем миссию Demonic
        /// </summary>
        public void MissionStart()
        {
            //new Point(703 - 5 + xx, 538 - 5 + yy).PressMouseLL();
            new Point(707 - 5 + xx, 565 - 5 + yy).PressMouseL();    //кнопка Mission Start  22-11
        }

        /// <summary>
        /// бафаемся умением Q (i-й герой)
        /// </summary>
        public void BuffQ(int i)
        {
            new Point(34 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();   //проверено 23-11
            //MoveCursorOfMouse();
            //Pause(500);
        }

        /// <summary>
        /// бафаемся умением W (i-й герой)
        /// </summary>
        public void BuffW(int i)
        {
            new Point(65 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();
            //MoveCursorOfMouse();            
            //Pause(500);
        }

        /// <summary>
        /// бафаемся умением E (i-й герой)
        /// </summary>
        public void BuffE(int i)
        {
            //            new Point(89 - 5 + xx + (i - 1) * 255, 701 - 5 + yy).PressMouseL();
            new Point(96 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).DoubleClickL();
            //MoveCursorOfMouse();
        }

        /// <summary>
        /// бафаемся умением R (i-й герой)
        /// </summary>
        public void BuffR(int i)
        {
            new Point(127 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();
            //MoveCursorOfMouse();
        }

        /// <summary>
        /// бафаемся умением T (i-й герой)
        /// </summary>
        public void BuffT(int i)
        {
            new Point(158 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).DoubleClickL();
            //MoveCursorOfMouse();
        }

        /// <summary>
        /// бафаемся рабочим умением Y (i-й герой)
        /// </summary>
        public void BuffY(int i)
        {
            new Point(189 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();              //проверено 23-11
            MoveCursorOfMouse();
        }

        /// <summary>
        /// бафаемся рабочим умением Y (i-й герой) и выбираем того, кого бафать (также i-го героя)
        /// </summary>
        public void BuffYY(int i)
        {
            new Point(189 - 5 + xx + (i - 1) * 255, 706 - 5 + yy).PressMouseL();              //проверено 23-11
            botwindow.Hero(i);
            MoveCursorOfMouse();
        }

        /// <summary>
        /// переключаем чат на пятую закладку "System"
        /// </summary>
        public void ChatFifthBookmark()
        {
            new Point(1010 - 5 + xx, 323 - 5 + yy).PressMouseL();
            Pause(500);
            new Point(805 - 5 + xx, 327 - 5 + yy).PressMouseL();
            Pause(500);
            //MoveCursorOfMouse();
        }

        /// <summary>
        /// нажимаем в бараках на кнопку "Mission Again"
        /// </summary>
        public void ReturnToMissionFromBarack()
        {
            //new Point(330 - 5 + xx, 670 - 5 + yy).PressMouseLL();
            new Point(330 - 5 + xx, 670 - 5 + yy).PressMouseL();     //23-11
            Pause(500);
        }

        /// <summary>
        /// проверяем, появилась ли в миссии Demonic белая надпись сверху, означаюшая окончание миссии
        /// </summary>
        /// <returns>true, если появилась</returns>
        public bool isWhiteLabel()
        {
            return
                    new PointColor(300 - 5 + xx, 145 - 5 + yy, 16711422, 0).isColor() &&
                    new PointColor(300 - 5 + xx, 146 - 5 + yy, 16711422, 0).isColor();
            //белая надпись (миссия закончится через 2 минуты )
        }

        ///// <summary>
        ///// атакуем монстров в миссии  (с CTRL) старый вариант
        ///// </summary>
        //public void AttackTheMonsters(int Direction)
        //{
        //    //MaxHeight(1); //если убили и мы прошли через бараки, то камера низко. А так мы ее немного поднимаем.
        //    AssaultMode();
        //    //if (new PointColor(525 + Direction * 500 - 5 + xx, 400 - 5 + yy, 0, 0).GetPixelColor() > 400000)
        //    //    new Point(525 + Direction * 500 - 5 + xx, 400 - 5 + yy).PressMouseL();   //*350 -5 , 392-5
        //    //else
        //    //    new Point(525 - Direction * 500 - 5 + xx, 400 - 5 + yy).PressMouseL();
        //    if (new PointColor(525 + Direction * 200 - 5 + xx, 392 - 5 + yy, 0, 0).GetPixelColor() > 400000)
        //        new Point(525 + Direction * 200 - 5 + xx, 392 - 5 + yy).PressMouseL();   //*350 -5 , 392-5
        //    else
        //        new Point(525 - Direction * 200 - 5 + xx, 392 - 5 + yy).PressMouseL();
        //}

        /// <summary>
        /// подбираем дроп, двигаясь влево (Demonic.FesoMap)
        /// </summary>
        public void PickUpToLeft()
        {
            for (int i = 1; i <= 2; i++)
            {
                if (!isHarvestMode())
                {
                    HarvestMode();
                    new Point(250 - 5 + xx, 432 - 5 + yy).PressMouseL();
                    Pause(1000);
                }
            }
        }

        /// <summary>
        /// подбираем дроп, двигаясь влево (Demonic.FesoMap)
        /// </summary>
        public void PickUpToRight()
        {
            for (int i = 1; i <= 2; i++)
            {
                if (!isHarvestMode())
                {
                    HarvestMode();
                    new Point(766 - 5 + xx, 432 - 5 + yy).PressMouseL();
                    Pause(1000);
                }
            }
        }

        /// <summary>
        /// атакуем с CTRL максимально вправо (Demonic.FesoMap)
        /// </summary>
        public void AttackCtrlToRight()
        {
            for (int i = 1; i <= 3; i++)
            {
                AssaultMode();
                new Point(766 - 5 + xx, 432 - 5 + yy).PressMouseL();
            }
        }

        /// <summary>
        /// атакуем с CTRL максимально влево (Demonic.FesoMap)
        /// </summary>
        public void AttackCtrlToLeft()
        {
            for (int i = 1; i <= 3; i++)
            {
                AssaultMode();
                new Point(72 - 5 + xx, 432 - 5 + yy).PressMouseL();
            }
        }


        /// <summary>
        /// пытаемся тыкнуть с CTRL в заданные координаты. Если не удалось, то тыкаем чуть ниже.  И так делаем 5 раз
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AttackCtrl(int x, int y)
        {
            int count = 0;
            do
            {
                AssaultMode();
                new Point(x, y).PressMouseL();
                //старый проверенный вариант
                //y += 10;

                //новый вариант=====
                if (x < 525) x -= 20;
                else x += 20;
                Pause(500);
                //===================
                count++;
            }
            while (!isAssaultMode() && (count <= 5));
            //выходим из цикла, если получилось перейти в боевой режим (AssaultMode), т.е. атака с CTRL, либо тыкали уже больше пяти раз
        }

        /// <summary>
        /// атакуем монстров в миссии  (с CTRL)
        /// </summary>
        /// <param name="Direction"> направление движения (влево или вправо)</param>
        public void AttackTheMonsters(int Direction)
        {
            //int DeltaX = 200; //амплитуда движения героев по оси Х         //старый проверенный вариант
            int DeltaX = 150; //амплитуда движения героев по оси Х  
            int DeltaY = 30;  //амплитуда движения героев по оси Y
            int x;
            int y;

            #region вариант 1. (Старый и сложный)
            //int x = 525 - 5 + xx + Direction * DeltaX;    //за счет Direction будет ходить влево-вправо
            //int y = 382 - 5 + yy + Direction * DeltaY;    // за счет Direction при ходьбе влево-вправо будет немного смещаться вверх-вниз
            //bool IsBoss;
            //do
            //{
            //    y += 10; if (y > 432 - 5 + yy + Direction * DeltaY) break;

            //    AssaultMode();
            //    if (new PointColor(x, y, 0, 0).GetPixelColor() < 400000)        //если хотим тыкнуть в темное место (т.е. за пределы боевой арены),
            //        Direction = -Direction;                                     //то меняем направление тыка                     
            //    new Point(x, y).PressMouseL();
            //    IsBoss = isBossOrMob() && (!isMob());    //определяем босса так: в прицеле кто-то есть, но это не моб
            //}
            //while (!isAssaultMode() && !IsBoss); //выходим из цикла, если получилось перейти в боевой режим (AssaultMode), т.е. атака с CTRL
            //проверка на "босса в прицеле" стоит для того, что не всегда удаётся перейти в Assault Mode, так как Кризалис загораживает весь экран и некуда тыкнуть, кроме как не в него
            #endregion

            //вариант 2. новый и простой
            // сначала атакуем в выбранном направлении
            x = 525 - 5 + xx + Direction * DeltaX;    //за счет Direction будет ходить влево-вправо
            y = 412 - 5 + yy + Direction * DeltaY;    // за счет Direction при ходьбе влево-вправо будет немного смещаться вверх-вниз   //382

            if (new PointColor(x, y - 150, 0, 0).GetPixelColor() < 400000)        //если сдвинулись слишком сильно вверх (т.е. близко к верхнему краю боевой арены)
            {
                y = y + 60;
            }
            else if (new PointColor(x, y + 150, 0, 0).GetPixelColor() < 400000)
            {
                y = y - 60;
            }
            //AssaultMode();
            //new Point(x, y).PressMouseL();
            AttackCtrl(x, y);


            if (this.Counter >= 5)          //бафаемся один раз в 5 ходов
            {
                BattleModeOnDem();
                botwindow.ActiveAllBuffBH();
                botwindow.PressEsc(4);

                //бафаемся героями первый раз
                BuffHeroes();
                ManaForDemonic();
                MoveCursorOfMouse();
                //бафаемся героями второй раз (на случай, если у героя два бафа или первый раз не получилось пробафаться)
                BuffHeroes();
                ManaForDemonic();
                MoveCursorOfMouse();
                this.Counter = 0;
            }
            else
            { 
                if (isBoss())
                {
                    SkillHeroes();
                    ManaForDemonic();
                    MoveCursorOfMouse();
                }
            }
            this.Counter++;


            //потом атакуем в обратном направлении и переходим в исходную точку
            x = 525 - 5 + xx - Direction * DeltaX;    //за счет Direction будет ходить влево-вправо
            y = 382 - 5 + yy - Direction * DeltaY;    // за счет Direction при ходьбе влево-вправо будет
            if (new PointColor(x, y - 150, 0, 0).GetPixelColor() < 400000)        //если сдвинулись слишком сильно вверх (т.е. близко к верхнему краю боевой арены)
            {
                y = y + 60;
            }
            else if (new PointColor(x, y + 150, 0, 0).GetPixelColor() < 400000)
            {
                y = y - 60;
            }

            //AssaultMode();
            //new Point(x, y).PressMouseL();
            BattleModeOnDem();
            AttackCtrl(x, y);

        }

        /// <summary>
        /// атакуем монстров в миссии  (с CTRL)
        /// </summary>
        /// <param name="Direction"> направление движения (влево или вправо)</param>
        public void AttackTheMonsters()
        {
            //смещение движения героев по оси Х
            int DeltaX;

            //смещение движения героев по оси Y
            int DeltaY;

            //проверка цвета реперных точек на экране

            PointColor LeftUP = new PointColor(85 - 5 + xx, 85 - 5 + yy, 0, 0);
            PointColor RightUP = new PointColor(980 - 5 + xx, 85 - 5 + yy, 0, 0);
            PointColor LeftDown = new PointColor(85 - 5 + xx, 480 - 5 + yy, 0, 0);
            PointColor RightDown = new PointColor(980 - 5 + xx, 360 - 5 + yy, 0, 0);

            uint LeftUpColor = LeftUP.GetPixelColor();
            uint RightUpColor = RightUP.GetPixelColor();
            uint LeftDownColor = LeftDown.GetPixelColor();
            uint RightDownColor = RightDown.GetPixelColor();

            if (LeftUpColor < 400000 && RightUpColor < 400000)
                DeltaY = 150;
            else
                if (LeftDownColor < 400000 && RightDownColor < 400000)
                DeltaY = -150;
            else
                DeltaY = 0;

            if (LeftUpColor < 400000 && LeftDownColor < 400000)
                DeltaX = 250;
            else
                if (RightUpColor < 400000 && RightDownColor < 400000)
                DeltaX = -250;
            else
                DeltaX = 250;

            //вычисляем координаты точки, куда будем тыкать мышкой
            int x = 525 - 5 + xx + DeltaX;
            int y = 382 - 5 + yy + DeltaY;

            AssaultMode();
            new Point(x, y).PressMouseL();

            //бафаемся
            //тут надо бы сделать проверку, наложены ли уже баффы (хрин+принципал+элики славы) или нет. И всё это вместе с баффами запихнуть в процедурку
            botwindow.ActiveAllBuffBH();
            botwindow.PressEscThreeTimes();

        }

        /// <summary>
        /// переходим в режим атаки с CTRL
        /// </summary>
        public void AssaultMode()
        {
            //            new Point(91 - 5 + xx, 526 - 5 + yy).PressMouseLL();
            new Point(91 - 5 + xx, 526 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// нажимаем на сундук, чтобы запустить рулетку
        /// </summary>
        public void OpeningTheChest()
        {
            new Point(523 - 5 + xx, 350 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// проверяем, доступна ли миссия (не доступна, потому что уже ходили сегодня)
        /// /// </summary>
        /// <returns>true, если не доступна</returns>
        public bool isMissionNotAvailable()
        {
            //return  new PointColor(349 - 5 + xx, 652 - 5 + yy, 13752023, 0).isColor() &&
            //        new PointColor(349 - 5 + xx, 653 - 5 + yy, 13752023, 0).isColor();


            //проверяем букву Т в слове Today  22-11-23
            return new PointColor(373 - 5 + xx, 561 - 5 + yy, 13752023, 0).isColor() &&
                    new PointColor(374 - 5 + xx, 561 - 5 + yy, 13752023, 0).isColor();
        }

        /// <summary>
        /// вычисляем, какой герой стоит в команде на i-м месте
        /// </summary>
        /// <param name="i">номер места в команде</param>
        /// <returns></returns>
//        public int WhatsHero(int i)
//        {
//            if (new PointColor(33 - 5 + xx + (i - 1) * 255, 696 - 5 + yy, 806655, 0).isColor())    //узнали, что это флинтлок
//            {
//                //проверяем Y, чтобы понять мушкетер или Бернелли
//                if (new PointColor(187 - 5 + xx + (i - 1) * 255, 704 - 5 + yy, 13951211, 0).isColor())
//                    return 1;     //мушкетер с флинтом
//                if (new PointColor(187 - 5 + xx + (i - 1) * 255, 705 - 5 + yy, 11251395, 0).isColor())
//                    return 10;   //Бернелли с флинтом
//            }
//            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 695 - 5 + yy, 16777078, 0).isColor()) return 2;     //Берка(супериор бластер)
//            if (new PointColor(23 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 5041407, 0).isColor())  return 3;     //М.Лорч
//            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 9371642, 0).isColor())  return 4;     //Джайна
////          if (new PointColor(28 - 5 + xx + (i - 1) * 255, 707 - 5 + yy, 5046271, 0).isColor()) result = 5;    //Баррель
//            if (new PointColor(22 - 5 + xx + (i - 1) * 255, 704 - 5 + yy, 16121838, 0).isColor()) return 6;      //Сесиль  --------++++++++++++
////          if (new PointColor(28 - 5 + xx + (i - 1) * 255, 698 - 5 + yy, 5636130, 0).isColor()) result = 7;    //Tom
////          if (new PointColor(31 - 5 + xx + (i - 1) * 255, 701 - 5 + yy, 5081, 0).isColor()) result = 8;       //Moon
////          if (new PointColor(30 - 5 + xx + (i - 1) * 255, 706 - 5 + yy, 6116670, 0).isColor()) result = 9;    //Misa
//            if (new PointColor(26 - 5 + xx + (i - 1) * 255, 699 - 5 + yy, 14438144, 0).isColor()) return 11;    //Rosie
//            if (new PointColor(28 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 4944448, 0).isColor()) return 12;     //Marie
//            if (new PointColor(28 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 0, 0).isColor()) return 13;           //C.Daria   -------------------------
//            if (new PointColor(26 - 5 + xx + (i - 1) * 255, 705 - 5 + yy, 8716287, 0).isColor()) return 14;     // Aither   ---------++++++++++++++++
//            if (new PointColor(22 - 5 + xx + (i - 1) * 255, 712 - 5 + yy, 65535, 0).isColor()) return 15;       //М.Калипсо   --------+++++++++++++++
//            if (new PointColor(31 - 5 + xx + (i - 1) * 255, 703 - 5 + yy, 15856385, 0).isColor()) return 16;    //Банши   --------++++++++++++++
//            if (new PointColor(34 - 5 + xx + (i - 1) * 255, 703 - 5 + yy, 16251007, 0).isColor()) return 17;    //СуперРомина   ------+++++++++++++++
//            if (new PointColor(39 - 5 + xx + (i - 1) * 255, 697 - 5 + yy, 8323241, 0).isColor()) return 18;     //Miho   -------+++++++++++++++
//            if (new PointColor(27 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 69370, 0).isColor()) return 19;       //R.JD   ----------++++++++++++++++
//            if (new PointColor(23 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 11453688, 0).isColor()) return 20;    //Jane  --------++++++++++++++
//            if (new PointColor(31 - 5 + xx + (i - 1) * 255, 698 - 5 + yy, 8711323, 0).isColor()) return 21;     //Лорч  -------++++++++++++++++++
//            if (new PointColor(32 - 5 + xx + (i - 1) * 255, 702 - 5 + yy, 5925855, 0).isColor()) return 22;     //Rebecca ------+++++++++++
//            if (new PointColor(32 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 4929704, 0).isColor()) return 23;     //DivineHammerBryan --------++++++++++++++
//            if (new PointColor(29 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 2703856, 0).isColor()) return 24;     //LoraConstans -----------------------
//            if (new PointColor(32 - 5 + xx + (i - 1) * 255, 703 - 5 + yy, 16777214, 0).isColor()) return 25;     //Calipso -----------------------

//            return 0;
//        }

        ///// <summary>
        ///// бафаем i-го героя (если бафали муху, то возвращаем true)
        ///// </summary>
        ///// <param name="typeOfHero">тип героя (1-муха, 2-Берка, 3-Лорч и т.д.)</param>
        ///// <param name="i">номер героя. на каком месте стоит</param>
        //public bool Buff(int typeOfHero, int i)
        //{
        //    bool result = false;
        //    if (!isTreasureChest())      //бафаемся, только в том случае, если не появился сундук
        //                                 //чтобы лишние записи не появились в чате
        //    {
        //        MoveCursorOfMouse();
        //        switch (typeOfHero)
        //        {
        //            case 1:
        //                result = BuffMusk(i);  //*
        //                break;
        //            case 2:
        //                BuffBernelliBlaster(i);   //*
        //                break;
        //            case 3:
        //                BuffMLorch(i); //*
        //                break;
        //            case 4:
        //                BuffJaina(i);  //*
        //                break;
        //            //case 5:
        //            //    BuffBarell(i);  //*
        //            //    break;
        //            case 6:
        //                BuffCecile(i);        //без бафа
        //                break;
        //            //case 7:
        //            //    BuffTom(i);
        //            //    break;
        //            //case 8:
        //            //    BuffMoon(i);  //*
        //            //    break;
        //            //case 9:
        //            //    BuffMisa(i);
        //            //    break;
        //            case 10:
        //                BuffBernelliFlint(i);   //*
        //                break;
        //            case 11:
        //                BuffRosie(i);   //*
        //                break;
        //            case 12:
        //                BuffMary(i);   //*
        //                break;
        //            case 13:
        //                BuffCDaria(i);   //надо сделать
        //                break;
        //            case 14:
        //                BuffAither(i); //*
        //                break;
        //            case 15:
        //                BuffMCalipso(i);  //надо сделать
        //                break;
        //            case 16:
        //                BuffVanshee(i);   //надо сделать
        //                break;
        //            case 17:
        //                BuffSuperRomina(i);   //надо сделать
        //                break;
        //            case 18:
        //                BuffMiho(i);  //надо сделать
        //                break;
        //            case 19:
        //                BuffRJD(i);   //
        //                break;
        //            case 20:
        //                BuffJane(i);  //надо сделать
        //                break;
        //            case 21:
        //                BuffLorch(i);  
        //                break;
        //            case 22:
        //                BuffRebecca(i);  
        //                break;
        //            case 23:
        //                BuffDivine(i);  
        //                break;
        //            case 24:
        //                BuffLoraConstans(i);
        //                break;
        //            case 25:
        //                BuffCalipso(i);
        //                break;

        //            case 0:
        //                break;
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// есть ли кто в прицеле?
        /// </summary>
        /// <returns>true, если кто-то есть в прицеле</returns>
        public bool isBossOrMob()
        {
            //проверяем букву D в слове Демоник
            return new PointColor(456 - 5 + xx, 103 - 5 + yy, 12434870, 1).isColor() ||
                        new PointColor(456 - 5 + xx, 103 - 5 + yy, 4000000, 6).isColor() ||
                        new PointColor(456 - 5 + xx, 103 - 5 + yy, 7314875, 0).isColor() ||
                        new PointColor(456 - 5 + xx, 103 - 5 + yy, 7462629, 0).isColor();

        }

        /// <summary>
        /// в прицеле босс? (Для Демоника)
        /// </summary>
        /// <returns>true, если босс в прицеле</returns>
        public bool isBoss()
        {
            //Определяем по наличию плюсика перед названием босса вверху
            return  new PointColor(415 - 5 + xx, 93 - 5 + yy, 13430000, 4).isColor() &&
                    new PointColor(416 - 5 + xx, 93 - 5 + yy, 13430000, 4).isColor();

        }

        /// <summary>
        /// есть ли моб Demonic Enhance в прицеле?
        /// </summary>
        /// <returns>true, если в прицеле моб</returns>
        public bool isMob()
        {
            //проверяем букву E и h в слове Enhance
            return (new PointColor(521 - 5 + xx, 100 - 5 + yy, 13355979, 0).isColor() &&
                        new PointColor(521 - 5 + xx, 109 - 5 + yy, 13355979, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 100 - 5 + yy, 13355979, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 109 - 5 + yy, 13355979, 0).isColor())
                        ||
                       (new PointColor(521 - 5 + xx, 100 - 5 + yy, 7776457, 0).isColor() &&
                        new PointColor(521 - 5 + xx, 109 - 5 + yy, 7776457, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 100 - 5 + yy, 7776457, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 109 - 5 + yy, 7776457, 0).isColor())
                        ||
                       (new PointColor(521 - 5 + xx, 100 - 5 + yy, 7925494, 0).isColor() &&
                        new PointColor(521 - 5 + xx, 109 - 5 + yy, 7925494, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 100 - 5 + yy, 7925494, 0).isColor() &&
                        new PointColor(532 - 5 + xx, 109 - 5 + yy, 7925494, 0).isColor())
                       ;

        }

        /// <summary>
        /// если скилл i-го мушкетёра T готов к использованию
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool isSkillMuskT(int i)
        {
            return new PointColor(152 - 5 + xx + (i - 1) * 255, 704 - 5 + yy, 12491137, 0).isColor();
        }

        /// <summary>
        /// если скилл i-го мушкетёра W готов к использованию
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool isSkillMuskW(int i)
        {
            return new PointColor(63 - 5 + xx + (i - 1) * 255, 700 - 5 + yy, 3119530, 0).isColor();
        }

        /// <summary>
        /// если скилл i-го мушкетёра E готов к использованию
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool isSkillMuskE(int i)
        {
            return new PointColor(84 - 5 + xx + (i - 1) * 255, 701 - 5 + yy, 4094905, 0).isColor();
        }

        /// <summary>
        /// скилуем i-м героем. Скилл в зависимости от того, босс в прицеле или нет /не используется/
        /// </summary>
        /// <param name="typeOfHero">тип героя (1-муха, 2-Берка, 3-Лорч и т.д.)</param>
        /// <param name="i">номер героя</param>
        /// <param name="isMobs"true, если в прицеле мобы</param>
        //public void Skill(int typeOfHero, int i, bool isMobs)
        //{
        //    switch (typeOfHero)
        //    {
        //        case 1:
        //            //Муха
        //            if (isMobs)
        //                BuffE(i);
        //            else
        //                BuffT(i);

        //            //if (isSkillMuskE(i))   //если готов скилл
        //            //BuffE(i);
        //            //if (isSkillMuskT(i))   //если готов скилл
        //            //    BuffT(i);

        //            //if (isSkillMuskW(i))   //если готов скилл
        //            //    BuffW(i);
        //            break;
        //        case 2:
        //            //BuffBernelliBlaster(i);   //*
        //            if (!isMobs)
        //                BuffT(i);
        //            break;
        //        case 3:
        //            //BuffLorch(i); //*
        //            if (!isMobs)
        //                BuffT(i);
        //            break;
        //        case 4:
        //            //BuffJaina(i);  //*
        //            break;
        //        case 5:
        //            //BuffBarell(i);  //*
        //            BuffT(i);
        //            break;
        //        case 6:
        //            //BuffCDaria(i);   //47
        //            BuffT(i);
        //            break;
        //        case 7:
        //            //BuffTom(i);
        //            break;
        //        case 8:
        //            //BuffMoon(i);  //*
        //            break;
        //        case 9:
        //            //BuffMisa(i);
        //            BuffT(i);  //скилуем самым крутым скиллом
        //            break;
        //        case 10:
        //            //BuffBernelliFlint(i);   //*
        //            if (isMobs)
        //                BuffE(i);
        //            else
        //                BuffT(i);
        //            break;
        //        case 0:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// скилуем i-м героем
        /// </summary>
        /// <param name="typeOfHero">тип героя (1-муха, 2-Берка, 3-Лорч и т.д.)</param>
        /// <param name="i">номер героя</param>
        //public void SkillBoss(int typeOfHero, int i)
        //{
        //    switch (typeOfHero)
        //    {
        //        case 1:
        //            //Муха
        //            BuffT(i);
        //            break;
        //        case 2:
        //            //Bernelli Blaster
        //            BuffT(i);
        //            break;
        //        case 3:
        //            //MLorch
        //            BuffT(i);
        //            break;
        //        case 4:
        //            //Jaina
        //            BuffT(i);
        //            break;
        //        case 5:
        //            //Barell
                    
        //            break;
        //        case 6:
        //            //Cecille
        //            BuffR(i);
        //            break;
        //        case 7:
        //            //Tom
        //            break;
        //        case 8:
        //            //Moon
        //            break;
        //        case 9:
        //            //Misa
        //            BuffT(i);  //скилуем самым крутым скиллом
        //            break;
        //        case 10:
        //            //BernelliFlint
        //            BuffT(i);
        //            break;
        //        case 11:
        //            //Rosie
        //            BuffT(i);
        //            break;
        //        case 12:
        //            //Mary
        //            BuffT(i);
        //            break;
        //        case 13:
        //            //C.Daria
        //            BuffT(i);
        //            break;
        //        case 14:
        //            // Aither 
        //            BuffT(i);
        //            break;
        //        case 15:
        //            //М.Калипсо
        //            BuffT(i);
        //            break;
        //        case 16:
        //            //Банши
        //            BuffT(i);
        //            break;
        //        case 17:
        //            //СуперРомина
        //            BuffT(i);
        //            break;
        //        case 18:
        //            //Miho
        //            BuffT(i);
        //            break;
        //        case 19:
        //            //R.JD
        //            BuffT(i);
        //            break;
        //        case 20:
        //            //Jane
        //            BuffT(i);
        //            break;
        //        case 21:
        //            //Лорч
        //            BuffT(i);
        //            break;
        //        case 22:
        //            //Rebecca
        //            BuffT(i);
        //            break;
        //        case 23:
        //            //DivineHammer
        //            BuffT(i);
        //            break;
        //        case 24:
        //            //
        //            BuffT(i);
        //            break;
        //        case 25:
        //            //
        //            BuffT(i);
        //            break;
        //        default:
        //            break;
        //    }
        //}


        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Marchen" /Moon/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindMarchen(int i)
        //{
        //    //MoveCursorOfMouse();
        //    bool result = false;    //бафа нет
        //    for (int j = 0; j < 15; j++)
        //        if (new PointColor(85 - 5 + xx + j * 15 + (i - 1) * 255, 588 - 5 + yy, 787682, 0).isColor() &&
        //                new PointColor(86 - 5 + xx + j * 15 + (i - 1) * 255, 588 - 5 + yy, 1906639, 0).isColor()
        //           ) result = true;
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Reload Bullet" /Moon/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindReloadBullet(int i)
        //{
        //    //MoveCursorOfMouse();
        //    bool result = false;    //бафа нет
        //    for (int j = 0; j < 15; j++)
        //        if (new PointColor(91 - 5 + xx + j * 15 + (i - 1) * 255, 592 - 5 + yy, 464572, 0).isColor() &&
        //                new PointColor(91 - 5 + xx + j * 15 + (i - 1) * 255, 593 - 5 + yy, 397755, 0).isColor()
        //            ) result = true;
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Mysophoia" /Jaina/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindMysophoia(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                //new PointColor(78 - 5 + xx + j * 15 + (i - 1) * 255, 595 - 5 + yy, 16767324, 0).isColor() &&
        //                //    new PointColor(78 - 5 + xx + j * 15 + (i - 1) * 255, 596 - 5 + yy, 16767324, 0).isColor()
        //                new PointColor(77 - 5 + xx + j * 14 + (i - 1) * 255, 595 - 5 + yy, 16767324, 0).isColor() &&
        //                new PointColor(77 - 5 + xx + j * 14 + (i - 1) * 255, 596 - 5 + yy, 16767324, 0).isColor()           //23-11
        //            ) result = true;
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Bullet Apilicion" /М Лорч/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindBulletApilicon(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //            //new PointColor(79 - 5 + xx + j * 15 + (i - 1) * 255, 587 - 5 + yy, 7967538, 0).isColor() &&
        //            //    new PointColor(79 - 5 + xx + j * 15 + (i - 1) * 255, 588 - 5 + yy, 7572528, 0).isColor()
        //            new PointColor(78 - 5 + xx + j * 14 + (i - 1) * 255, 587 - 5 + yy, 7967538, 0).isColor() &&
        //                new PointColor(78 - 5 + xx + j * 14 + (i - 1) * 255, 588 - 5 + yy, 7572528, 0).isColor()   //23-11
        //            ) result = true;

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Share Flint" /М Лорч/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindShareFlint(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //            //new PointColor(80 - 5 + xx + j * 15 + (i - 1) * 255, 591 - 5 + yy, 8257280, 0).isColor() &&
        //            //    new PointColor(81 - 5 + xx + j * 15 + (i - 1) * 255, 590 - 5 + yy, 7995136, 0).isColor()
        //            new PointColor(79 - 5 + xx + j * 14 + (i - 1) * 255, 591 - 5 + yy, 8257280, 0).isColor() &&
        //                new PointColor(80 - 5 + xx + j * 14 + (i - 1) * 255, 590 - 5 + yy, 7995136, 0).isColor()  //23-11
        //            ) result = true;

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Concentration"
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindConcentracion(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                //new PointColor(84 - 5 + xx + j * 15 + (i - 1) * 255, 587 - 5 + yy, 5390673, 0).isColor() &&
        //                //new PointColor(84 - 5 + xx + j * 15 + (i - 1) * 255, 588 - 5 + yy, 5521228, 0).isColor()
        //                new PointColor(83 - 5 + xx + j * 14 + (i - 1) * 255, 587 - 5 + yy, 5390673, 0).isColor() &&
        //                new PointColor(83 - 5 + xx + j * 14 + (i - 1) * 255, 588 - 5 + yy, 5521228, 0).isColor()          //23-11
        //           ) result = true;
        //    if (isKillHero(i)) result = true;   //если убит i-й герой, то считаем, что у него есть бафф концентрации 

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Drunken" / Barrell /
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //private bool FindDrunken(int i)
        //{
        //    //MoveCursorOfMouse();
        //    bool result = false;    //бафа нет
        //    for (int j = 0; j < 8; j++)
        //        if (new PointColor(78 - 5 + xx + j * 15 + (i - 1) * 255, 585 - 5 + yy, 10861754, 0).isColor() &&
        //                new PointColor(79 - 5 + xx + j * 15 + (i - 1) * 255, 585 - 5 + yy, 11124411, 0).isColor()
        //           ) result = true;

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Shooting Promoted" / Barrell /
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //private bool FindShootingPromoted(int i)
        //{
        //    //MoveCursorOfMouse();
        //    bool result = false;    //бафа нет
        //    for (int j = 0; j < 8; j++)
        //        if (new PointColor(89 - 5 + xx + j * 15 + (i - 1) * 255, 586 - 5 + yy, 2503088, 0).isColor() &&
        //                new PointColor(90 - 5 + xx + j * 15 + (i - 1) * 255, 586 - 5 + yy, 1976470, 0).isColor()
        //           ) result = true;

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Marksmanship" / Bernelli /
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindMarksmanship(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (new PointColor(89 - 5 + xx + j * 14 + (i - 1) * 255, 596 - 5 + yy, 13133369, 0).isColor() &&
        //                new PointColor(89 - 5 + xx + j * 14 + (i - 1) * 255, 597 - 5 + yy, 13067318, 0).isColor()   //23-11
        //           ) result = true;

        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Hound" / Bernelli Blaster /
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindHound(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //            //new PointColor(81 - 5 + xx + j * 15 + (i - 1) * 255, 589 - 5 + yy, 6319302, 0).isColor() &&
        //            //    new PointColor(82 - 5 + xx + j * 15 + (i - 1) * 255, 589 - 5 + yy, 5858504, 0).isColor()
        //            new PointColor(80 - 5 + xx + j * 14 + (i - 1) * 255, 589 - 5 + yy, 6319302, 0).isColor() &&
        //                new PointColor(81 - 5 + xx + j * 14 + (i - 1) * 255, 589 - 5 + yy, 5858504, 0).isColor()   //23-11
        //           ) result = true;
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Soul Weapon" / Rosie /  Q
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindSoulWeapon(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 8; j++)
        //        if (new PointColor(4 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 5 + 585 - 5 + yy, 6400000, 5).isColor() &&
        //             new PointColor(6 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 7 + 585 - 5 + yy, 6700000, 5).isColor()
        //           )
        //        { 
        //            result = true; 
        //            break; 
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Link" / Rosie / Y
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindLink(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 8; j++)
        //        if (new PointColor(4 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 4 + 585 - 5 + yy, 4800000, 5).isColor() &&
        //             new PointColor(6 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 6 + 585 - 5 + yy, 5800000, 5).isColor()
        //           )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "CleaningTime" / Mary / Y
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        //public bool FindCleaningTime(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //            //new PointColor(0 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 4800000, 5).isColor() &&
        //            // new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 5800000, 5).isColor()
        //             new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 587 - 5 + yy, 12409326, 0).isColor() &&
        //             new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 588 - 5 + yy, 12409326, 0).isColor()
        //           )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "OrderLady" / Mary / E
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        //public bool FindOrderLady(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (new PointColor(0 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 4800000, 5).isColor() &&
        //             new PointColor(1 + 77 - 5 + xx + j * 14 + (i - 1) * 255, 0 + 585 - 5 + yy, 5800000, 5).isColor()
        //           )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Reasoning" /Jane/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindReasoning(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(8 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 588 - 5 + yy, 16777215, 0).isColor() &&
        //                new PointColor(8 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 16777215, 0).isColor()           //23-11
        //            )
        //        { 
        //            result = true; 
        //            break; 
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Infiltration" /Lorch/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindInfiltration(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 587 - 5 + yy, 1400619, 0).isColor() &&
        //                new PointColor(3 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 587 - 5 + yy, 1400619, 0).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}


        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "ChangerDeMode" /Rebecca/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindChangerDeMode(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 593 - 5 + yy, 15920000, 4).isColor() &&
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 594 - 5 + yy, 15730000, 4).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Metallurgy" /DivineHammerBryan/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindMetallurgy(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(1 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 13090000, 4).isColor() &&
        //                new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 590 - 5 + yy, 13290000, 4).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}


        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Justice" /LoraConstans/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindJustice(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 13080000, 4).isColor() &&
        //                new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 12880000, 4).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "CatsEye" /Calipso/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindCatsEye(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 16380000, 4).isColor() &&
        //                new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 16380000, 4).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Eagle Eye" /Calipso/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindEagleEye(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(1 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16050000, 4).isColor() &&
        //                new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 590 - 5 + yy, 16250000, 4).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}


        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Promise" /LoraConstans/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindPromise(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 1068760, 0).isColor() &&
        //                new PointColor(4 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 17110, 0).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Aergia" /Aither/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindAergia(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 588 - 5 + yy, 16252671, 0).isColor() &&
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 16777215, 0).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Redacendo" /M.Calipso/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindRedacendo(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16252927, 0).isColor() &&
        //                new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 592 - 5 + yy, 15073279, 0).isColor()           //23-11
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}


        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "ShadowTrigger" /Банши/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindShadowTrigger(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 2; j < 11; j++)
        //        if (
        //                new PointColor(5 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 5788827, 0).isColor() &&
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 588 - 5 + yy, 5788827, 0).isColor()
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}




        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "SupportOrder" /СуперРомина/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindSupportOrder(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 2; j < 11; j++)
        //        if (
        //                new PointColor(5 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 13672448, 0).isColor() &&
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 13672448, 0).isColor()           
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}


        

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Fox Spirit" /Miho/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindFoxSpirit(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(5 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 6749695, 0).isColor() &&
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 6749695, 0).isColor()
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}




        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Weapon Master" /R.JD/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindWeaponMaster(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(6 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 1574057, 0).isColor() &&
        //                new PointColor(7 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 589 - 5 + yy, 5505222, 0).isColor()
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}

        /// <summary>
        /// проверяем, есть ли на i-м герое бафф "Resolution" /R.JD/
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns>true, если есть</returns>
        //public bool FindResolution(int i)
        //{
        //    bool result = false;    //бафа нет
        //    for (int j = 3; j < 11; j++)
        //        if (
        //                new PointColor(2 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16663289, 0).isColor() &&
        //                new PointColor(3 + 77 - 5 + xx + (j - 1) * 14 + (i - 1) * 255, 591 - 5 + yy, 16666360, 0).isColor()
        //            )
        //        {
        //            result = true;
        //            break;
        //        }
        //    return result;
        //}



        /// <summary>
        /// бафаем Rosie на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffRosie(int i)
        //{
        //    if (!FindLink(i)) BuffY(i);
        //    //            Pause(2000);
        //    if (!FindSoulWeapon(i)) BuffQ(i);
        //}

        /// <summary>
        /// бафаем Mary на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMary(int i)
        //{
        //    if (!FindCleaningTime(i)) BuffY(i);
        //    //            Pause(2000);
        //    if (!FindOrderLady(i)) BuffE(i);
        //}

        /// <summary>
        /// бафаем мушкетера (с флинтом) на i-м месте 
        /// </summary>
        /// <param name="i"></param>
        //private bool BuffMusk(int i)
        //{
        //    bool result = false;
        //    if (!FindConcentracion(i))
        //    {
        //        BuffY(i);
        //        result = true;
        //    }
        //    return result;
        //}

        /// <summary>
        /// бафаем Бернелли с супериор бластер на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffBernelliBlaster(int i)
        //{
        //    if (!FindMarksmanship(i)) BuffY(i);
        //    //if (!FindHound(i)) BuffQ(i);
        //}

        /// <summary>
        /// бафаем Бернелли с Flintlock на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffBernelliFlint(int i)
        //{
        //    if (!FindMarksmanship(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем М Лорча на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMLorch(int i)
        //{
        //    if (!FindBulletApilicon(i)) BuffY(i);
        //    //            Pause(2000);
        //    if (!FindShareFlint(i)) BuffW(i);
        //}

        /// <summary>
        /// бафаем Джайну на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffJaina(int i)
        //{
        //    if (!FindMysophoia(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем молодого Барреля на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffBarell(int i)
        //{
        //    if (!FindDrunken(i)) BuffY(i);
        //    if (!FindShootingPromoted(i)) BuffW(i);
        //}

        
        /// <summary>
        /// бафаем Сесиль на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffCecile(int i)
        //{ 
        //}

        /// <summary>
        /// бафаем коммандера Дарью на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffCDaria(int i)
        //{ }

        /// <summary>
        /// бафаем Тома на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffTom(int i)
        //{ }

        /// <summary>
        /// бафаем Муна на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMoon(int i)
        //{
        //    if (!FindReloadBullet(i)) BuffY(i);
        //    if (!FindMarchen(i)) BuffQ(i);
        //}

        /// <summary>
        /// бафаем Мису на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMisa(int i)
        //{
        //    //if (!FindWindUp(i)) 
        //    //BuffY(i);
        //    // не бафаем Мису, бафф снижает меткость

        //}

        /// <summary>
        /// бафаем Aither на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffAither(int i)
        //{
        //    if (!FindAergia(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем МКалипсо на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMCalipso(int i)
        //{
        //    if (!FindRedacendo(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем Банши на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffVanshee(int i)
        //{
        //    if (!FindShadowTrigger(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем СуперРомина на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffSuperRomina(int i)
        //{
        //    if (!FindSupportOrder(i)) BuffYY(i);
        //}

        /// <summary>
        /// бафаем Михо на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffMiho(int i)
        //{
        //    if (!FindFoxSpirit(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем R.JD на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffRJD(int i)
        //{
        //    if (!FindWeaponMaster(i)) BuffY(i);
            
        //    if (!FindResolution(i)) BuffR(i);
        //}

        /// <summary>
        /// бафаем Jane на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffJane(int i)
        //{
        //    if (!FindReasoning(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем Lorcha на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffLorch (int i)
        //{
        //    if (!FindInfiltration(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем Rebecca на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffRebecca(int i)
        //{
        //    if (!FindChangerDeMode(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем DivineHammerBryan на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffDivine(int i)
        //{
        //    if (!FindMetallurgy(i)) BuffY(i);
        //}

        /// <summary>
        /// бафаем LoraConstans на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffLoraConstans(int i)
        //{
        //    if (!FindJustice(i)) BuffY(i);
        //    if (!FindPromise(i)) BuffW(i);
        //}

        /// <summary>
        /// бафаем Calipso на i-м месте
        /// </summary>
        /// <param name="i"></param>
        //private void BuffCalipso(int i)
        //{
        //    if (!FindCatsEye(i)) BuffY(i);
        //    if (!FindEagleEye(i)) BuffQ(i);
        //}

        /// <summary>
        /// появились ворота вместо сундука?
        /// </summary>
        /// <returns>true, если появились</returns>
        public bool isGate()
        {
            return new PointColor(599 - 5 + xx, 397 - 5 + yy, 5700000, 5).isColor() &&
                    new PointColor(599 - 5 + xx, 398 - 5 + yy, 5700000, 5).isColor();
        }

        /// <summary>
        /// появились вторые ворота (с фесом)? /проверка по надписи "Generous Room"/
        /// </summary>
        /// <returns>true, если появились</returns>
        public bool isSecondGate()
        {
            return new PointColor(552 - 5 + xx, 435 - 5 + yy, 0, 0).isColor() &&
                    new PointColor(552 - 5 + xx, 436 - 5 + yy, 0, 0).isColor();

            // ЦВЕТ ТОЧЕК НАДО УКАЗАТЬ и проверить координаты. сейчас этот метод не работает
        }

        /// <summary>
        /// тыкаем в ворота с фесом
        /// </summary>
        public void PressOnFesoGate()
        {
            //new Point(539 - 5 + xx, 399 - 5 + yy).PressMouseL();
            new Point(530 - 5 + xx + 13, 397 - 5 + yy + 35).PressMouseL();    //  y можно пробовать =405
        }

        /// <summary>
        /// атакуем монстров в миссии (с CTRL) или подбираем фесо
        /// </summary>
        public void AttackTheMonsters2(int Direction)
        {
            if (Direction == -1)  //если идем влево, то мочим всех
            {
                AssaultMode();
                new Point(525 + Direction * 200 - 5 + xx, 392 - 5 + yy).PressMouseL();
            }
            else  //если идем направо, то собираем лут
            {
                DropSelectionMode();
                new Point(525 + Direction * 200 - 5 + xx, 392 - 5 + yy).PressMouseL();
            }
        }

        /// <summary>
        /// режим подбора дропа (сундук)
        /// </summary>
        public void DropSelectionMode()
        {
            new Point(123 - 5 + xx, 526 - 5 + yy).PressMouseL();    //нажимаем на сундук (иконка подбора)
            Pause(200);

        }

        #endregion

        #region Delivery Event

        /// <summary>
        /// нажать на кнопку "Trade"
        /// </summary>
        public void PressButtonRelic()
        {
            iPoint p1 = new Point(470 - 5 + xx, 520 - 5 + yy);
            for (int i = 1; i <= 500; i++)
            {
                botwindow.ActiveWindowBH();
                p1.PressMouseL();
                Pause(500);
            }
        }

        /// <summary>
        /// открываем ящики в открытом инвентаре
        /// </summary>
        public void OpenBoxes()
        {
            int dxBox = 0;
            int dyBox = 0;

            for (int dx = 0; dx <= 4; dx++)
                for (int dy = 0; dy <= 5; dy++)
                {
                    if (new PointColor(724 - 5 + xx + dx * 58 + 40, 223 - 5 + yy + dy * 58 + 31, 5837047, 0).isColor() &&
                        new PointColor(724 - 5 + xx + dx * 58 + 40, 223 - 5 + yy + dy * 58 + 32, 6426876, 0).isColor())
                    {
                        dxBox = dx;
                        dyBox = dy;
                        break;
                    }

                }

            if ((dxBox == 0) && (dyBox == 0))
            {
                botwindow.PressEscThreeTimes();
            }
            else
            {
                while (new PointColor(724 - 5 + xx + dxBox * 58 + 40, 223 - 5 + yy + dyBox * 58 + 31, 5837047, 0).isColor() &&
                        new PointColor(724 - 5 + xx + dxBox * 58 + 40, 223 - 5 + yy + dyBox * 58 + 32, 6426876, 0).isColor())
                {

                    new Point(724 - 5 + xx + dxBox * 58 + 20, 223 - 5 + yy + dyBox * 58 + 20).DoubleClickL();
                    Pause(500);
                    new Point(440 - 5 + xx, 440 - 5 + yy).Move();  //убираем мышь в сторону
                    Pause(8500);

                }
            }

            botwindow.PressEscThreeTimes();

            //new Point(750 - 5 + xx, 206 - 5 + yy).PressMouseLL();  //тыкаем в выбранную закладку
            //Pause(500);
            //new Point(440 - 5 + xx, 440 - 5 + yy).Move();  //убираем мышь
            //Pause(500);
        }

        /// <summary>
        /// открываем закладку в уже открытом инвентаре
        /// </summary>
        /// <param name="number">номер закладки</param>
        public void OpenInventoryBookmark(int number)
        {
            new Point(750 - 5 + (number - 1) * 65 + xx, 206 - 5 + yy).PressMouseLL();  //тыкаем в выбранную закладку
            Pause(500);
            new Point(440 - 5 + xx, 440 - 5 + yy).Move();  //убираем мышь
            Pause(500);
        }

        /// <summary>
        /// идём к Рудольфу по карте (карта уже открыта)
        /// </summary>
        public void GoToRudolph()
        {
            //botwindow.FirstHero();

            new Point(535 - 5 + xx, 435 - 5 + yy).PressMouseLL();   //тыкаем в Рудольфа на карте
            botwindow.PressEscThreeTimes();                         //убираем карту
            Pause(12000);                                           //ждём, пока добежим до Рудольфа

            new Point(621 - 5 + xx, 424 - 5 + yy).PressMouseL();    //тыкаем в Рудольфа, чтобы перейти к диалогу с ним
        }

        /// <summary>
        /// проверяем, можно ли взять задание у Рудольфа (синий кружок на карте на месте Рудольфа)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool GotTask()
        {
            return new PointColor(533 - 5 + xx, 413 - 5 + yy, 5784856, 0).isColor() &&
                    new PointColor(534 - 5 + xx, 413 - 5 + yy, 5456151, 0).isColor();


        }

        /// <summary>
        /// проверяем, есть ли задание у первого клиента (синий кружок на карте)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool GotTask2()
        {
            return new PointColor(288 - 5 + xx, 319 - 5 + yy, 16777215, 0).isColor() &&
                    new PointColor(289 - 5 + xx, 318 - 5 + yy, 16777215, 0).isColor();
        }

        /// <summary>
        /// проверяем, получено ли задание у Рудольфа (синий кружок на карте на месте первого клиента)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool GotTaskRudolph()
        {
            botwindow.PressEscThreeTimes();

            //надёжно открываем карту города
            if (isReboldo())
            {
                while (!isOpenMapReboldo()) { TopMenu(12, 2, true); Pause(500); }
            }

            bool result = GotTask2();  //проверяем, получено ли задание у Рудольфа

            botwindow.PressEscThreeTimes(); //закрываем карту

            return result;
        }

        /// <summary>
        /// открываем карту Ребольдо с гарантией
        /// </summary>
        /// <returns>true, если есть</returns>
        public void OpenMapReboldo()
        {
            botwindow.PressEscThreeTimes();

            //надёжно открываем карту города
            if (isReboldo())
            {
                while (!isOpenMapReboldo())
                    if (isReboldo())                //защита от бесконечного цикла. А то бывает, что мы в магазине, но пытаемся открыть карту города
                        TopMenu(12, 2, true);
                    else
                        break;
            }
        }

        /// <summary>
        /// проверяем, открыта ли карта Ребольдо (буква H  в надписи Repeption Hall)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool isOpenMapReboldo()
        {
            return new PointColor(199 - 5 + xx, 255 - 5 + yy, 6525666, 0).isColor() &&
                    new PointColor(199 - 5 + xx, 256 - 5 + yy, 6525666, 0).isColor();


        }

        /// <summary>
        /// идём к Рудольфу за наградой (карта уже открыта)
        /// </summary>
        public void GoToRudolph2()
        {
            new Point(535 - 5 + xx, 435 - 5 + yy).PressMouseLL();   //тыкаем, куда бежать по карте
            Pause(1000);
            botwindow.PressEscThreeTimes();                         //убираем карту
            Pause(11000);

            new Point(290 - 5 + xx, 513 + yy).PressMouseL();        // тыкаем в Рудольфа
            Pause(1000);
            if (!dialog.isDialog())                                 // если не попали в Рудольфа
                new Point(434 - 5 + xx, 540 - 5 + yy).PressMouseL();    // тыкаем ещё раз немного в другое место
        }

        /// <summary>
        /// идём к первому клиенту по карте
        /// </summary>
        public void GoToDeliveryNumber1()
        {
            botwindow.PressEscThreeTimes();

            if (!dialog.isDialog())
                while (!isOpenMapReboldo())
                {
                    TopMenu(12, 2, true);               //если мы сейчас не в диалоге, то пытаемся открыть карту города
                    if (dialog.isDialog()) break;
                }
            //Pause(500);

            Random rand = new Random();
            int randemNumber = rand.Next(1, 10);
            if (randemNumber <= 7)
            { if (!dialog.isDialog()) new Point(295 - 5 + xx, 324 - 5 + yy).PressMouseLL(); }
            else
            { if (!dialog.isDialog()) new Point(314 - 5 + xx, 325 - 5 + yy).PressMouseLL(); }

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём к клиенту 2 по карте
        /// </summary>
        public void GoToDeliveryNumber2()
        {
            botwindow.PressEscThreeTimes();
            if (!dialog.isDialog())
                //while (!isOpenMapReboldo())
                TopMenu(12, 2, true);               //если мы сейчас не в диалоге, то пытаемся открыть карту города

            Random rand = new Random();
            int randemNumber = rand.Next(1, 10);
            if (randemNumber <= 7)
            { if (!dialog.isDialog()) new Point(220 - 5 + xx, 434 - 5 + yy).PressMouseLL(); }
            else
            { if (!dialog.isDialog()) new Point(235 - 5 + xx, 443 - 5 + yy).PressMouseLL(); }

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём к клиенту 3 по карте
        /// </summary>
        public void GoToDeliveryNumber3()
        {
            botwindow.PressEscThreeTimes();
            if (!dialog.isDialog())
                //while (!isOpenMapReboldo())
                TopMenu(12, 2, true);               //если мы сейчас не в диалоге, то пытаемся открыть карту города

            Random rand = new Random();
            int randemNumber = rand.Next(1, 10);
            if (randemNumber <= 7)
            { if (!dialog.isDialog()) new Point(547 - 5 + xx, 606 - 5 + yy).PressMouseLL(); }
            else
            { if (!dialog.isDialog()) new Point(648 - 5 + xx, 532 - 5 + yy).PressMouseLL(); }

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём к клиенту 4 по карте
        /// </summary>
        public void GoToDeliveryNumber4()
        {
            botwindow.PressEscThreeTimes();

            if (!dialog.isDialog())
                //while (!isOpenMapReboldo())
                TopMenu(12, 2, true);               //если мы сейчас не в диалоге, то пытаемся открыть карту города

            Random rand = new Random();
            int randemNumber = rand.Next(1, 10);
            if (randemNumber <= 7)
            { if (!dialog.isDialog()) new Point(462 - 5 + xx, 479 - 5 + yy).PressMouseLL(); }
            else
            { if (!dialog.isDialog()) new Point(422 - 5 + xx, 462 - 5 + yy).PressMouseLL(); }

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём к клиенту 5 по карте
        /// </summary>
        public void GoToDeliveryNumber5()
        {
            botwindow.PressEscThreeTimes();

            if (!dialog.isDialog())
                //while (!isOpenMapReboldo())
                TopMenu(12, 2, true);               //если мы сейчас не в диалоге, то пытаемся открыть карту города

            Random rand = new Random();
            int randemNumber = rand.Next(1, 10);
            if (randemNumber <= 7)
            { if (!dialog.isDialog()) new Point(675 - 5 + xx, 410 - 5 + yy).PressMouseLL(); }
            else
            { if (!dialog.isDialog()) new Point(645 - 5 + xx, 393 - 5 + yy).PressMouseLL(); }

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём в Ребольдо
        /// </summary>
        public void GoToReboldo()
        {
            botwindow.PressEscThreeTimes();
            TeleportAltW(1);
            botwindow.PressEscThreeTimes();
        }
        /// <summary>
        /// идём в Коимбру
        /// </summary>
        public void GoToCoimbra()
        {
            botwindow.PressEscThreeTimes();
            TeleportAltW(2);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём в Ош
        /// </summary>
        public void GoToAuch()
        {
            botwindow.PressEscThreeTimes();
            TeleportAltW(3);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// проверяем, находимся ли в Ребольдо
        /// </summary>
        /// <returns></returns>
        public bool isReboldo()
        {
            return     new PointColor(927 - 5 + xx, 252 - 5 + yy, 16000000, 6).isColor()
                    && new PointColor(927 - 5 + xx, 259 - 5 + yy, 16000000, 6).isColor()        //слово Rebo под миникартой (буква R)
                    && new PointColor(957 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor()
                    && new PointColor(957 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor();       //слово Rebo под миникартой (буква l)

        }

        /// <summary>
        /// проверяем, находимся ли в Коимбре
        /// </summary>
        /// <returns></returns>
        public bool isCoimbra()
        {
            return new PointColor(976 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor() &&
                   new PointColor(976 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor();        //слово Coimbra под миникартой (буква i)

        }

        /// <summary>
        /// проверяем, находимся ли в Оше
        /// </summary>
        /// <returns></returns>
        public bool isAuch()
        {
            return new PointColor(921 - 5 + xx, 252 - 5 + yy, 14000000, 6).isColor() &&
                   new PointColor(921 - 5 + xx, 259 - 5 + yy, 14000000, 6).isColor();        //слово of под миникартой (буква f)

        }

        #endregion

        #region ================ Bridge =========================

        /// <summary>
        /// мы в диалоге в воротах после миссии на мосту?
        /// </summary>
        /// <returns>true, если диалог именно в воротах</returns>
        public bool isGateBridgeMission()
        {
            return      new PointColor(514 - 5 + xx, 536 - 5 + yy, 7450000, 4).isColor()
                     && new PointColor(514 - 5 + xx, 549 - 5 + yy, 7450000, 4).isColor()
                     && new PointColor(569 - 5 + xx, 536 - 5 + yy, 7450000, 4).isColor()
                     && new PointColor(569 - 5 + xx, 549 - 5 + yy, 7450000, 4).isColor();
        }

        /// <summary>
        /// мы в диалоге с Терезией на мосту?
        /// </summary>
        /// <returns>true, если диалог именно с Терезией</returns>
        public bool isTeresia()
        {
            return      new PointColor(566 - 5 + xx, 536 - 5 + yy, 7450000, 4).isColor()
                     && new PointColor(574 - 5 + xx, 536 - 5 + yy, 7450000, 4).isColor()
                     && new PointColor(570 - 5 + xx, 549 - 5 + yy, 7450000, 4).isColor();
        }

        /// <summary>
        /// мы в диалоге со статуей (наследие древних) на мосту?
        /// </summary>
        /// <returns>true, если диалог именно со статуей</returns>
        public bool isAncientBlessing()
        {
            return new PointColor(497 - 5 + xx, 536 - 5 + yy, 7450000, 4).isColor()
                && new PointColor(497 - 5 + xx, 549 - 5 + yy, 7450000, 4).isColor()
                && new PointColor(509 - 5 + xx, 536 - 5 + yy, 6660000, 4).isColor()
                && new PointColor(509 - 5 + xx, 549 - 5 + yy, 6790000, 4).isColor();
        }

        /// <summary>
        /// вычисляем день недели по Сингапурскому времени
        /// </summary>
        /// <returns></returns>
        public int WeekDayNow()
        {
            int wd = (int)DateTime.Now.AddHours(5).DayOfWeek;
            if (wd == 0) wd = 7;
            return wd;
        }

        /// <summary>
        /// открываем карту Моста с гарантией
        /// </summary>
        /// <returns>true, если есть</returns>
        public void OpenMapBridge()
        {
            //botwindow.PressEscThreeTimes();

            //надёжно открываем карту Alt+Z
            while (!isOpenMapBridge())
                if (isBridge())
                {
                    TopMenu(12, 2, true);
                    Pause(500);
                }
                else break;
        }

        /// <summary>
        /// проверяем, открыта ли карта Мост (буква M в надписи Zone Map)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool isOpenMapBridge()
        {
            return new PointColor(253 - 5 + xx, 125 - 5 + yy, 16382457, 0).isColor() &&
                    new PointColor(253 - 5 + xx, 126 - 5 + yy, 16382457, 0).isColor();
        }

        /// <summary>
        /// в уже открытой карте моста идём к выбранному персонажу (в строке numberString )
        /// </summary>
        public void GotoPersonOnMapBridge(int numberString)
        {
            new Point(740 - 5 + xx, 192 + (numberString - 1) * 15 - 5 + yy).DoubleClickL();   //тыкаем в статую (в списке справа от карты)
            Pause(500);
            new Point(840 - 5 + xx, 646 - 5 + yy).DoubleClickL();   //тыкаем в кнопку Move Now
            Pause(500);
            botwindow.PressEscThreeTimes();
            Pause(2000);
        }

        /// <summary>
        /// в уже открытой карте моста тыкаем в Терезию (шестая строчка в списке справа от карты Alt+Z)
        /// </summary>
        public void GotoTeresia()
        {
            GotoPersonOnMapBridge(6);
            new Point(522 - 5 + xx, 231 - 5 + yy).PressMouseL();    //тыкаем в голову Терезии, чтобы войти в диалог
        }

        /// <summary>
        /// в уже открытой карте моста тыкаем в AncientBlessingStatue (первая строчка в списке справа от карты Alt+Z)
        /// </summary>
        public void GotoAncientBlessingStatue()
        {
            GotoPersonOnMapBridge(1);
            new Point(522 - 5 + xx, 231 - 5 + yy).PressMouseL();    //тыкаем в статую, чтобы войти в диалог
        }

        /// <summary>
        /// в уже открытой карте моста тыкаем в солдата (третья строчка в списке справа от карты Alt+Z)
        /// </summary>
        public void GotoElementMissionEntrance()
        {
            GotoPersonOnMapBridge(3);
            new Point(511 - 5 + xx, 164 - 5 + yy).PressMouseL();    //тыкаем в голову солдата, чтобы войти в диалог
        }

        /// <summary>
        /// в уже открытой карте моста тыкаем в солдата (четвёртая строчка в списке справа от карты Alt+Z)
        /// </summary>
        public void GotoIndividualRaid()
        {
            GotoPersonOnMapBridge(4);
            new Point(525 - 5 + xx, 185 - 5 + yy).PressMouseL();    //тыкаем в голову солдата, чтобы войти в диалог
        }

        ///// <summary>
        ///// в диалоге с солдатом на мосту выбираем миссию для выполнения
        ///// </summary>
        ///// <param name="rank">ранг миссии</param>
        ///// <param name="typeOfMission"> 1 - миссия с плюсом. 3 - обычная </param>
        //public void GotoIndividualMission(int rank, int typeOfMission, int weekDay)
        //{
        //    dialog.PressStringDialog(8 - rank);             //выбираем ранг
        //    dialog.PressStringDialog(6 - weekDay);          //выбираем миссию (животные, андиды, лайфлесы или проч)  //только в выходные//
        //    dialog.PressStringDialog(typeOfMission);        // выбираем тип миссии (плюсовая или обычная)
        //    dialog.PressStringDialog(1);                    // в миссию (join) 
        //    //if (dialog.isDialog()) dialog.PressOkButton(1);
        //}

        /// <summary>
        /// в диалоге с солдатом на мосту выбираем миссию для выполнения //28-02-2024
        /// </summary>
        /// <param name="rank">ранг миссии</param>
        /// <param name="typeOfMission"> 1 - миссия с плюсом. 3 - обычная </param>
        public void GotoIndividualMission(int weekDay)
        {
            int[]          rank = { 0, 1, 1, 3, 1, 1, 1, 1 };
            int[] typeOfMission = { 0, 1, 1, 3, 3, 3, 3, 3 };

            dialog.PressStringDialog(9 - rank[weekDay]);            //выбираем ранг миссии. Он зависит от дня недели
            if (weekDay == 6 || weekDay == 7)
                dialog.PressStringDialog(1);                        //выбираем миссию (последнюю строчку - Undead)  //только в выходные//
                                                                    //dialog.PressStringDialog(6 - weekDay);              //выбираем миссию (животные, андиды, лайфлесы или проч)  //только в выходные//
            Pause(1000);
            dialog.PressStringDialog(typeOfMission[weekDay]);       // выбираем тип миссии (плюсовая или обычная)
            dialog.PressStringDialog(1);                            // в миссию (join) 
            //if (dialog.isDialog()) dialog.PressOkButton(1);
        }

        /// <summary>
        /// в диалоге с солдатом на мосту переходим к созданию миссии (к Mission Lobby)
        /// </summary>
        public void GotoElementMission()
        {
            dialog.PressStringDialog(2);                             //                            
        }


        /// <summary>
        /// на мосту ли мы?
        /// </summary>
        /// <returns></returns>
        public bool isBridge()
        {
            return new PointColor(940 - 5 + xx, 252 - 5 + yy, 16711422, 0).isColor()
                    && new PointColor(940 - 5 + xx, 259 - 5 + yy, 16711422, 0).isColor()
                    && new PointColor(983 - 5 + xx, 252 - 5 + yy, 16711422, 0).isColor()
                    && new PointColor(983 - 5 + xx, 259 - 5 + yy, 16711422, 0).isColor();
        }

        /// <summary>
        /// по карте миссии тыкаем в точку, где враг
        /// </summary>
        /// <param name="weekDay"> день недели по сингапурскому времени </param>
        public void AttackTheEnemy(int weekDay)
        {
            iPoint[] PixelOfAttack = {  //new Point(423 - 5 + xx, 303 - 5 + yy),        //1+ понедельник, тигр
                                        new Point(415 - 5 + xx, 303 - 5 + yy),          // 1+ понедельник, животные
                                        //new Point(595 - 5 + xx, 296 - 5 + yy),        //2+ понедельник, паук
                                        //new Point(390 - 5 + xx, 550 - 5 + yy),          //1+ вторник, human
                                        new Point(423 - 5 + xx, 611 - 5 + yy),          // 1+ вторник, human
                                        //new Point(430 - 5 + xx, 340 - 5 + yy),        // 2+ вторник, human
                                        //new Point(636 - 5 + xx, 495 - 5 + yy),        // 1+ среда, demon
                                        //new Point(424 - 5 + xx, 240 - 5 + yy),        // 2+ среда, demon
                                        new Point(553 - 5 + xx, 245 - 5 + yy),          // 3 среда, demon
                                        new Point(422 - 5 + xx, 322 - 5 + yy),          // 1 четверг
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 пятница (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 суббота (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 воскресенье (undead череп)
                                        //new Point(567 - 5 + xx, 479 - 5 + yy),        // 1+ пятница
                                        //new Point(459 - 5 + xx, 408 - 5 + yy),        // 2 пятница не надёжно
                                        //new Point(461 - 5 + xx, 501 - 5 + yy),        // 2+ пятница
                                        //new Point(335 - 5 + xx, 399 - 5 + yy),        // 3 пятница
            };
            //открываем карту Alt+Z
            TopMenu(12, 2, true);
            Pause(500);

            PixelOfAttack[weekDay - 1].PressMouseRR();

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// по карте миссии тыкаем в точку, где враг
        /// </summary>
        /// <param name="weekDay"> день недели по сингапурскому времени </param>
        public void AttackTheEnemyElement(int weekDay)
        {
            iPoint[] PixelOfAttack = {  new Point(413 - 5 + xx, 263 - 5 + yy),          // понедельник, животные
                                        new Point(390 - 5 + xx, 550 - 5 + yy),          // 1+ вторник, human
                                        new Point(553 - 5 + xx, 245 - 5 + yy),          // 3 среда, demon
                                        new Point(422 - 5 + xx, 322 - 5 + yy),          // 1 четверг
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 пятница (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 суббота (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 воскресенье (undead череп)
            };
            //открываем карту Alt+Z
            TopMenu(12, 2, true);
            Pause(500);

            PixelOfAttack[weekDay - 1].PressMouseRR();

            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// вычисляем продолжительность паузы после нажатия пробела в миссии на мосту
        /// </summary>
        /// <param name="weekDay">день недели по Сингапурскому времени</param>
        /// <returns>время ожидания в пропущенных циклах</returns>
        public int WaitForBattle(int weekDay)
        {
            int[] TimeOfWait = {   0,          // 1+ понедельник, животные
                                    //0,        // 2+ понедельник, животные
                                   1,          // 1+ вторник, human    надо проверить
                                    //0,        // 2+ вторник, human
                                    //0,        // 1+ среда, demon
                                    //0,        // 2+ среда, demon
                                    0,          // 3  среда, demon
                                    0,          // 1  четверг
                                    7,          // 1 пятница (undead череп)
                                    //0,        // 1+ пятница
                                    //0,        // 2  пятница                        
                                    //0,        // 2+ пятница
                                    //0,        // 3  пятница
                                    7,          // 1 суббота (undead череп)
                                    7          // 1 воскресенье (undead череп)
            };
            return TimeOfWait[weekDay - 1];
        }

        /// <summary>
        /// идём к сундуку и открываем его
        /// </summary>
        /// <param name="weekDay"> день недели по сингапурскому времени </param>
        public void GoToChest(int weekDay)
        {
            iPoint[] PixelOfChest = {   new Point(415 - 5 + xx, 303 - 5 + yy),          // 1+ понедельник, животные
                                        //new Point(595 - 5 + xx, 296 - 5 + yy),        // 2+ понедельник, животные
                                        new Point(423 - 5 + xx, 611 - 5 + yy),          // 1+ вторник, human    надо проверить
                                        //new Point(430 - 5 + xx, 340 - 5 + yy),        // 2+ вторник, human
                                        //new Point(636 - 5 + xx, 495 - 5 + yy),        // 1+ среда, demon
                                        //new Point(424 - 5 + xx, 240 - 5 + yy),        // 2+ среда, demon
                                        new Point(553 - 5 + xx, 245 - 5 + yy),          // 3  среда, demon
                                        new Point(422 - 5 + xx, 322 - 5 + yy),          // 1  четверг
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 пятница (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 суббота (undead череп)
                                        new Point(537 - 5 + xx, 402 - 5 + yy),          // 1 воскресенье (undead череп)
                                        //new Point(567 - 5 + xx, 479 - 5 + yy),        // 1+ пятница
                                        //new Point(457 - 5 + xx, 405 - 5 + yy),        // 2  пятница                        
                                        //new Point(461 - 5 + xx, 501 - 5 + yy),          // 2+ пятница
                                        //new Point(335 - 5 + xx, 399 - 5 + yy),        // 3  пятница
            };
            //iPoint[] PressChest = { new Point(396 - 5 + xx, 482 - 5 + yy),
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),      //* 1 четверг
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),      //* 1+, 2, 2+,3 пятница
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),
            //                        new Point(396 - 5 + xx, 482 - 5 + yy),
            //};


            //открываем карту Alt+Z
            if ((weekDay != 1) && (weekDay != 2))               //в понедельник и вторник не идём к сундуку
            {
                TopMenu(12, 2, true);
                Pause(500);
                PixelOfChest[weekDay - 1].PressMouseLL();   //идём к сундуку
                Pause(500);
                botwindow.PressEscThreeTimes();
                Pause(2000);
            }
            new Point(356 - 5 + xx, 462 - 5 + yy).PressMouseL();  //нажимаем на сундук      //было 396 482
            //PressChest[weekDay - 1].PressMouseL();      //нажимаем на сундук
            Pause(4000);                               //ждём рулетку

        }

        /// <summary>
        /// Закончилась Activity?
        /// </summary>
        /// <returns></returns>
        public bool isActivityOut()
        {
            return  new PointColor(704 - 5 + xx, 565 - 5 + yy, 4530000, 4).isColor()
                 && new PointColor(704 - 5 + xx, 566 - 5 + yy, 4530000, 4).isColor()
                 ||
                    new PointColor(484 - 5 + xx, 561 - 5 + yy, 4730000, 4).isColor()
                 && new PointColor(485 - 5 + xx, 561 - 5 + yy, 4730000, 4).isColor();
        }

        /// <summary>
        /// определяем, что за герои стоят в команде
        /// </summary>
        public void WhatsHeroes()
        {
            //this.Hero[1] = WhatsHero(1);
            //this.Hero[2] = WhatsHero(2);
            //this.Hero[3] = WhatsHero(3);
            this.hero1 = heroFactory.Create(1);
            this.hero2 = heroFactory.Create(2);
            this.hero3 = heroFactory.Create(3);
        }

        /// <summary>
        /// бафаем героев всеми бафами, какие есть у героев  (Q,W,E,R,T,Y)
        /// </summary>
        public void BuffHeroes()
        {
            //Buff(this.Hero[1], 1);
            //Buff(this.Hero[2], 2);
            //Buff(this.Hero[3], 3);

            this.hero1.Buff();
            this.hero2.Buff();
            this.hero3.Buff();
        }

        /// <summary>
        /// скиллуем лучшими ударами, какие есть у героев  (T)
        /// </summary>
        public void SkillHeroes()
        {
            //SkillBoss(this.Hero[1], 1);
            //SkillBoss(this.Hero[2], 2);
            //SkillBoss(this.Hero[3], 3);

            //this.hero1.SkillBoss();
            //this.hero2.SkillBoss();
            //this.hero3.SkillBoss();

            //бьём первым скиллом
            this.hero1.SkillBoss1();
            this.hero2.SkillBoss1();
            this.hero3.SkillBoss1();

            //бьём вторым скиллом
            this.hero1.SkillBoss2();
            this.hero2.SkillBoss2();
            this.hero3.SkillBoss2();

        }


        /// <summary>
        /// комплекс действий при начале атаки в миссии на мосту
        /// </summary>
        /// <param name="weekDay">день недели по сингапуру</param>
        public void BeginAttack(int weekDay)
        {
            //бафаемся
            BattleModeOnDem();

            botwindow.ActiveAllBuffBH();
            botwindow.PressEsc(4);

            MoveCursorOfMouse();

            WhatsHeroes();
            BuffHeroes();
            BuffHeroes();

            //идём в атаку с Ctrl
            AttackTheEnemy(weekDay);
            Pause(1000);

            //активируем пета
            ActivatePetDem();
        }

        /// <summary>
        /// комплекс действий при начале атаки в миссии на мосту
        /// </summary>
        /// <param name="weekDay">день недели по сингапуру</param>
        public void BeginAttackElement(int weekDay)
        {
            //бафаемся
            MoveCursorOfMouse();
            botwindow.ActiveAllBuffBH();
            botwindow.PressEsc(4);

            WhatsHeroes();
            BuffHeroes();
            BuffHeroes();

            //идём в атаку с Ctrl
            AttackTheEnemyElement(weekDay);    
            Pause(1000);

            //активируем пета
            ActivatePetDem();
        }


        /// <summary>
        /// скиллуем всеми героями (скиллы зависят от того, босс в прицеле или нет) /не используется/
        /// </summary>
        /// <param name="isBoss">если в прицеле босс, то true</param>
        public void SkillAll(bool isBoss)
        {
            MoveCursorOfMouse();
            WhatsHeroes();
            //Skill(Hero[1], 1, isBoss);
            //Skill(Hero[2], 2, isBoss);
            //Skill(Hero[3], 3, isBoss);

            hero[1].Skill(isBoss);
            hero[2].Skill(isBoss);
            hero[3].Skill(isBoss);

            MoveCursorOfMouse();
        }

        /// <summary>
        /// есть ли кто в прицеле? только для миссий на мосту
        /// </summary>
        /// <returns>true, если кто-то есть в прицеле</returns>
        public bool isBossOrMobBridge()
        {
            //проверяем букву D в слове Ancient

            //return new PointColor(456 - 5 + xx, 103 - 5 + yy, 12434870, 1).isColor() ||
            //            new PointColor(456 - 5 + xx, 103 - 5 + yy, 4000000, 6).isColor() ||
            //            new PointColor(456 - 5 + xx, 103 - 5 + yy, 7314875, 0).isColor() ||
            //            new PointColor(456 - 5 + xx, 103 - 5 + yy, 7462629, 0).isColor();

            return false;

        }

        #endregion

        #region Pure Otite New

        /// <summary>
        /// на карте, где стоит Мамут (Desert Quay)?
        /// </summary>
        /// <returns></returns>
        public bool isDesertedQuay()
        {
            return new PointColor(891 - 5 + xx, 252 - 5 + yy, 16000000, 6).isColor()
                && new PointColor(891 - 5 + xx, 259 - 5 + yy, 16000000, 6).isColor()
                && new PointColor(938 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor()
                && new PointColor(938 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor();
        }

        #endregion

        #region  ========================= Ферма ===============================

        /// <summary>
        /// поднимаем камеру в верхнюю точку
        /// </summary>
        public void MaxHeight(int n)
        {
            new Point(555 - 5 + xx, 430 - 5 + yy).Move();
            for (int i = 1; i <= n; i++)
            {
                new Point(555 - 5 + xx, 430 - 5 + yy).PressMouseWheelUp();
                //Pause(500); 
            }
        }

        /// <summary>
        /// опускаем камеру ниже к земле
        /// </summary>
        /// <param name="n"> количество оборотов мышки </param>
        public void MinHeight(int n)
        {
            new Point(555 - 5 + xx, 430 - 5 + yy).Move();
            for (int i = 1; i <= n; i++)
            {
                new Point(555 - 5 + xx, 430 - 5 + yy).PressMouseWheelDown();
                Pause(500);
            }
        }

        /// <summary>
        /// тыкаем в аппарат на ферме
        /// </summary>
        public void PressApparate()
        {
            new Point(100 - 5 + xx, 100 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// положить яблоки в первый аппарат
        /// </summary>
        public void PutAppleToFirstStorage()
        {
            //Thing Apple = new Thing(new PointColor(697 - 5 + xx, 180 - 5 + yy, 7465471, 0), 
            //                        new PointColor(704 - 5 + xx, 180 - 5 + yy, 42495, 0));

            DragItemToXY(Apple, 529 - 5 + xx, 181 - 5 + yy);  //перекладываем яблоки на аппарат
            Pause(2000);

            new Point(612 - 5 + xx, 398 - 5 + yy).PressMouseL();  //ок. подтверждаем количество
            Pause(3000);

            new Point(812 - 5 + xx, 339 - 5 + yy).PressMouseL();    //нажимаем "Insert"
            Pause(1500);
            new Point(950 - 5 + xx, 369 - 5 + yy).PressMouseL();    //нажимаем  "Ok"
            Pause(1500);

            botwindow.PressEscThreeTimes();
            Pause(500);

        }

        /// <summary>
        /// положить мед во второй аппарат
        /// </summary>
        public void PutHoneyToSecondStorage()
        {
            //Thing Honey = new Thing(new PointColor(697 - 5 + xx, 180 - 5 + yy, 4289893, 0), new PointColor(703 - 5 + xx, 180 - 5 + yy, 4487011, 0));

            DragItemToXY(Honey, 492 - 5 + xx, 112 - 5 + yy);
            Pause(1000);

            new Point(612 - 5 + xx, 398 - 5 + yy).PressMouseL();
            Pause(2000);

            new Point(814 - 5 + xx, 339 - 5 + yy).PressMouseL();
            Pause(500);
            new Point(954 - 5 + xx, 369 - 5 + yy).PressMouseL();
            Pause(500);

            botwindow.PressEscThreeTimes();
            Pause(500);

        }

        /// <summary>
        /// нажать OK в форме о ежедневной награде
        /// </summary>
        public void PressOkDailyReward()
        {
            new Point(520 - 5 + xx, 444 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// выскочила ли форма о ежедневной награде на ферме
        /// </summary>
        /// <returns></returns>
        public bool IsDailyReward()
        {
            return new PointColor(522 - 5 + xx, 440 - 5 + yy, 7727344, 0).isColor() &&
                   new PointColor(522 - 5 + xx, 441 - 5 + yy, 7727344, 0).isColor();
        }

        /// <summary>
        /// нажать на карте на точку #1, близкую к аппарату
        /// </summary>
        public void PressPointNearlyApparatus1()
        {
            new Point(437 - 5 + xx, 470 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// нажать на карте на точку #2, близкую к аппарату
        /// </summary>
        public void PressPointNearlyApparatus2()
        {
            new Point(437 - 5 + xx, 501 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// мы в Юстиаре?
        /// </summary>
        public bool isUstiar()
        {
            return  (new PointColor(926 - 5 + xx, 252 - 5 + yy, 16000000, 6).isColor()    //буква B при 20% налоге
                    && new PointColor(926 - 5 + xx, 259 - 5 + yy, 16000000, 6).isColor()
                    && new PointColor(961 - 5 + xx, 252 - 5 + yy, 15000000, 6).isColor()    //буква C при 20% налоге
                    && new PointColor(961 - 5 + xx, 259 - 5 + yy, 15000000, 6).isColor()) 
                    ||
                    (new PointColor(930 - 5 + xx, 252 - 5 + yy, 16000000, 6).isColor()    //буква B при 5% налоге
                    && new PointColor(930 - 5 + xx, 259 - 5 + yy, 16000000, 6).isColor()
                    && new PointColor(963 - 5 + xx, 252 - 5 + yy, 16000000, 6).isColor()    //буква C при 5% налоге
                    && new PointColor(963 - 5 + xx, 259 - 5 + yy, 16000000, 6).isColor());

        }

        /// <summary>
        /// проверяем, открыта ли карта Мост (буква M в надписи Zone Map)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool isOpenMapUstiar()
        {
            return new PointColor(253 - 5 + xx, 125 - 5 + yy, 16382457, 0).isColor() &&
                    new PointColor(253 - 5 + xx, 126 - 5 + yy, 16382457, 0).isColor();
        }

        /// <summary>
        /// открываем карту Юстиара с гарантией
        /// </summary>
        /// <returns>true, если есть</returns>
        public void OpenMapUstiar()
        {

            //надёжно открываем карту Alt+Z
            while (!isOpenMapUstiar())
                if (isUstiar())
                    TopMenu(12, 2, true);
                else break;
        }

        /// <summary>
        /// в уже открытой карте моста идём к выбранному персонажу (в строке numberString )
        /// </summary>
        public void GotoPersonOnMapUstiar(int numberString)
        {
            new Point(740 - 5 + xx, 192 + (numberString - 1) * 15 - 5 + yy).DoubleClickL();   //тыкаем в указанную строку
            Pause(500);
            new Point(840 - 5 + xx, 646 - 5 + yy).DoubleClickL();   //тыкаем в кнопку Move Now
            Pause(500);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// идём к Farm Manager
        /// </summary>
        public void GotoFarmManager()
        {
            WARP(3);
            MinHeight(10);
            OpenMapUstiar();
            Pause(500);
            GotoPersonOnMapUstiar(4);
            Pause(2000);
            PressOnFarmManager();
        }

        /// <summary>
        /// проверяем, открыт ли городской телепорт (Alt+F3)  (буква l в слове Alt)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool isOpenTownTeleport()
        {
            return new PointColor(114 - 5 + xx, 318 - 5 + yy, 11000000, 6).isColor() &&
                    new PointColor(114 - 5 + xx, 325 - 5 + yy, 11000000, 6).isColor();
        }

        /// <summary>
        /// Открыть городской телепорт (Alt + F3) 
        /// </summary>
        public void OpenTownTeleport()
        {
            //надёжно открываем  городской телепорт (Alt + F3)
            while (!isOpenTownTeleport())
                if (isTown())
                    TopMenu(12, 3, true);
                else
                    break;      //выход из while
            Pause(500);
        }

        /// <summary>
        /// переход по городскому телепорту (Alt + F3)
        /// </summary>
        /// <param name="NumberOfTeleport">номер телепорта по порядку (вверху - первый)</param>
        public void WARP(int NumberOfTeleport)
        {
            OpenTownTeleport();
            new Point(80 - 5 + xx, 363 - 5 + yy + (NumberOfTeleport - 1) * 30).DoubleClickL();
        }

        /// <summary>
        /// идём в Ребольдо
        /// </summary>
        public void GoToUstiar()
        {
            botwindow.PressEscThreeTimes();
            TeleportAltW(4);
            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// переход из казарм в стартовый город
        /// </summary>
        public void FromBarackToTown(int NumberOfTeam)
        {
            //============ выбор персонажей  ===========
            TeamSelection(NumberOfTeam);
            Pause(500);

            //============ выбор канала ===========
            botwindow.SelectChannel(1);                 //12.03.2023

            //============ выход в город  ===========
            NewPlace();                //начинаем в ребольдо  

            botwindow.Pause(1000);
            botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку 

            if (isBarackWarningYes()) PressYesBarack();    //сделано
            botwindow.Pause(500);

            botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown
        }

        /// <summary>
        /// тыкаем в голову Farm Manager
        /// </summary>
        public void PressOnFarmManager()
        {
            new Point(536 - 5 + xx, 125 - 5 + yy).DoubleClickL();
        }

        /// <summary>
        /// проверяем, доступна ли награда на ферме (буква К в слове Ок на кнопке)
        /// </summary>
        /// <returns>true, если есть</returns>
        public bool isRewardAvailable()
        {
            return new PointColor(827 - 5 + xx, 456 - 5 + yy, 11000000, 6).isColor() &&
                    new PointColor(827 - 5 + xx, 457 - 5 + yy, 11000000, 6).isColor();
        }

        /// <summary>
        /// получить награду на ферме
        /// </summary>
        public void GetReward()
        {
            new Point(827 - 5 + xx, 485 - 5 + yy).DoubleClickL();
        }

        /// <summary>
        /// делаем в бараке новую команду для фермы
        /// </summary>
        public void NewTeamCreating()
        {
            pointTeamSelection1.DoubleClickL();                         // Нажимаем кнопку вызова списка групп
            Pause(500);

            new Point(680 - 5 + xx, 120 - 5 + yy).PressMouseL();        //выбираем третьего героя
            new Point(680 - 5 + xx, 120 - 5 + yy).DoubleClickL();        //убираем из команды третьего героя
            Pause(500);
            new Point(530 - 5 + xx, 120 - 5 + yy).PressMouseL();        //выбираем второго героя
            new Point(530 - 5 + xx, 120 - 5 + yy).DoubleClickL();        //убираем из команды второго героя
            Pause(500);

            new Point(97 - 5 + xx, 590 - 5 + yy).PressMouseL();         //тыкаем 
            new Point(97 - 5 + xx, 661 - 5 + yy).PressMouseL();         //тыкаем в поле с названием команды
            Pause(500);
            SendKeys.SendWait("FERMA");
            Pause(500);
            new Point(189 - 5 + xx, 661 - 5 + yy).PressMouseL();        //save
            Pause(500);
        }


        #endregion

        #region =================================== All in One ==================================================
        /// <summary>
        /// выбор дальнейшего пути после выполнения миссии Демоник
        /// [ 1 = переход к след. аккаунту; 2 = Кастилия; 3 = ферма ]
        /// </summary>
        /// <param name="way">1 = переход к след. аккаунту; 2 = Кастилия; 3 = ферма</param>
        private void WayToGoDemonic(int way)
        {
            switch (way)
            {
                case 0:
                    //вариант 0.  Идём в Демоник
                    Teleport(3, true);                          // телепорт в Гильдию Охотников (третий телепорт в списке)        
                    botParam.HowManyCyclesToSkip = 2;           // даём время, чтобы подгрузились ворота Демоник.
                    break;
                case 1:
                    //вариант 1. закрываем аккаунт и переходим к следующему 

                    //RemoveSandboxieBH();
                    GoToEnd();
                    Pause(7000);
                    RemoveSandboxieCW();
                    botParam.Stage = 1;
                    botParam.HowManyCyclesToSkip = 2;
                    break;
                case 2:
                    //вариант 2. идём в Кастилию (стадия 6) 
                    Teleport(4, true);                          //телепорт в Кастилию
                    botParam.Stage = 6;                         //переходим на стадию Кастилия
                    botParam.HowManyCyclesToSkip = 4;
                    break;
                case 3:
                    //вариант 3. идём на ферму (стадия 9)
                    GotoBarack();
                    botParam.Stage = 9;                          //ферма
                    botParam.HowManyCyclesToSkip = 4;
                    break;
            }
        }

        #region ======================== Поиск стандартных проблем ===============================

        /// <summary>
        /// проверяем, если ли проблемы и возвращаем номер проблемы.  
        /// Проблемы общие для всех миссий 1,2,11,12,16,17,19,20,22,23,24,33,34,35,36,37,38
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemCommonForAll()
        {
            //если открыто окно Стим
            //if (isOpenSteamWindow()) CloseSteamWindow();
            //if (isOpenSteamWindow2()) CloseSteamWindow2();
            //if (isOpenSteamWindow3()) CloseSteamWindow3();
            //if (isOpenSteamWindow4()) CloseSteamWindow4();
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            //if (isError820()) return 33;
            //если выскочило сообщение о пользовательском соглашении
            //if (isNewSteam()) return 34;
            //если ошибка Sandboxie 
            //if (isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            //если окно игры открыто на другом компе
            //if (isOpenGEWindow()) return 37;
            //служба Steam
            //if (isSteamService()) return 11;
            //если нет окна
            if (!isHwnd())        //если нет окна ГЭ с hwnd таким как в файле HWND.txt
            {
                //if (!FindWindowSteamBool())  //если Стима тоже нет
                if (!FindWindowSteamBoolCW()) //если Стима тоже нет          -------------------------------------------------
                    return 24;
                else    //если Стим уже загружен
                    //if (FindWindowGEforBHBool())  return 23;      //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool) --------------
                    if (FindWindowCWBool()) return 23;              //нашли чистое окно ГЭ (и перезаписали Hwnd в функции FindWindowCWBool)
                else
                    return 22;                  //если нет окна ГЭ в текущей песочнице
            }
            else            //если окно с нужным HWND нашлось
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            //в логауте
            if (isLogout()) return 1;
            //если в магазине на странице с товарами
            if (isExpedMerch2()) return 13;
            //диалог
            if (dialog.isDialog())
                if (isExpedMerch() || isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;
            //в бараке
            if (isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (isBarack()) return 2;                    //если стоят в бараке 
            if (isBarackWarningYes()) return 16;
            if (isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы

            //if (isBadFightingStance()) return 19;       // если неправильная стойка
            if (isOpenWorldTeleport()) return 38;
            //если стандартных проблем не найдено
            return 0;
        }

        #endregion ===============================================================================

        #region ======================== Поиск проблем в Демонике ================================
        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic (стадия 1) и возвращаем номер проблемы
        /// 3, 4, 5, 6, 7, 8, 9, 10, 40, 41, 42, 43
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage1()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //диалог
            if (dialog.isDialog())
                if (isMissionNotAvailable())
                    return 10;                          //если стоим в воротах Demonic и миссия не доступна
                else
                    return 8;                           //если стоим в воротах Demonic и миссия доступна

            //Mission Lobby
            if (isMissionLobby()) return 5;      //22-11

            //Waiting Room //Mission Room 22-11
            if (isWaitingRoom()) return 3;      //22-11

            //город или БХ
            if (isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                // здесь проверка нужна, чтобы разделить "город" и "работу с убитым первым персонажем".  23-11
                if (!isKillFirstHero2())
                {
                    if (isBH())     //в БХ     
                        return 4;   // стоим в правильном месте (около ворот Demonic)
                    else   // в городе, но не в БХ
                    {
                        //if (!isSummonPet() && !isActivePet())
                        //    return 41;
                        if (isUstiar())
                            return 42;
                        if (isCastilia())
                            return 43;
                        if (isAncientBlessing(1))
                            return 6;                       //скорее всего в стартовом городе (ребольдо)
                        else
                            return 9;                       //нет наследия древних
                    }
                }
            }
            //в миссии (если убит первый персонаж, то это точно миссия
            if (isWork() || isKillFirstHero2())
                if (isCastiliaMine())
                    return 40;
                else
                    return 7;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic (стадия 2) и возвращаем номер проблемы
        /// 3, 4, 6, 10 
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage2()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //в миссии
            //if (isWork() || isKillFirstHero2())
            if (isWorkInDemonic())
            {
                //если розовая надпись в чате "Treasure chest...", значит появился сундук и надо идти в барак и далее стадия 3
                if (isTreasureChest())
                    return 10;                        //надо собрать дроп, идти в барак и далее - стадия 3
                else
                    if (!isActivePet())                 //пет не активирован
                        return 4;
                    else
                        return 3;                         //продолжаем атаковать
            }

            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            //if (isTownDemonic() && isBH() && !isKillHero())
                //return 6;                                                 

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic  (стадия 3) и возвращаем номер проблемы
        /// 3,6,8
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage3()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //диалог 
            //после открытия сундука тыкаем в место, где должны открыться второта фесо, а здесь по диалогу проверяем, удалось или нет
            if (dialog.isDialog())
                return 8;                       //диалог в воротах фесо

            //в миссии
            if (isWork())
                return 3;                       //надо тыкать в сундук
            //в городе или в БХ
            if (isTown())                       //если в городе или в БХ, то значит миссия закончилась и нас выкинуло
                return 6;                       //закрываем песочницу и аккаунт

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic (стадия 4) и возвращаем номер проблемы
        /// 3,5,6,7,8
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage4()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //в миссии фесо
            if (isWork())
            {
                if (isHarvestMode())
                    return 7;
                if (isBattleMode())
                {
                    if (NeedToPickUpFeso == false)
                        return 3;
                    else
                        return 8;
                }
                else
                    return 5;
            }

            //в городе или в БХ
            if (isTown())                           //если в городе или в БХ, то значит миссия закончилась и нас выкинуло
                return 6;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic (стадия 5) и возвращаем номер проблемы
        /// 3, 4, 5, 7, 8
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage5()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //если диалог 
            if (dialog.isDialog())
                return 7;                    //получаем бафф "наследие"
            //в миссии
            if (isWork())
            {
                if (isBridge())
                    if (isAncientBlessing(1))
                        return 4;           //на мосту и получили бафф наследие
                    else
                        if (isOpenMapBridge())
                            return 5;           //на мосту, но пока не получили бафф наследие. карта Alt+Z открыта
                    else
                            return 3;           //на мосту, но пока не получили бафф наследие. карта Alt+Z не открыта
            }
            //в городе
            if (isTown())
                return 8;                   //в городе. летим на мост

            //если проблем не найдено
            return 0;
        }

        #endregion ================================================================================

        #region ======================== Поиск проблем в Кастилии =================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Кастилии (стадия 1.Вход в миссию) и возвращаем номер проблемы
        /// 3,4,5,6,7,8,9,10
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage6()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            // диалог
            if (dialog.isDialog())
            {
                if (isMissionCastiliaNotAvailable())
                    return 10;                              //если стоим в воротах Castilia и миссия не доступна
                else
                    return 8;                               //если стоим в воротах Castilia и миссия доступна
            }
            //Mission Lobby
            if (isMissionLobby()) return 5;      //22-11
            //Waiting Room //Mission Room 22-11
            if (isWaitingRoom()) return 3;      //22-11
            //город или БХ
            if (isTown())
            {
                botwindow.PressEscThreeTimes();                 //27-10-2021
                if (!isKillFirstHero2()) // эта проверка нужна, чтобы разделить "город" и "работу с убитым первым персонажем".  23-11
                {
                    if (isCastilia())                           //в Кастилии     
                        if (isAncientBlessing(1))
                            return 4;                           // стоим в правильном месте (около зеленой стрелки в миссию)
                        else
                            return 9;                           //нет наследия. идём на мост
                    else                                        // в городе, но не в Кастилии (значит в Ребольдо)
                        return 6;                               // из Ребольдо в Кастилию
                }
            }

            //в миссии (если убит первый персонаж, то это точно миссия
            if (isWork() || isKillFirstHero2()) return 7;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Кастилии (стадия 2. Миссия) и возвращаем номер проблемы
        /// 3,4,5,6,7,8
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage7()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //в миссии
            if (isWork() ||
                isKillFirstHero2())       //проверить
            {
                if (isAssaultMode())     //значит ещё бегут с атакой к выбранной точке
                    return 4;                   //тыкаем туда же ещё раз
                if (isBattleMode())
                {
                    if (NeedToPickUpRight)
                        return 7;
                    if (NeedToPickUpLeft)
                        return 8;
                    else
                        if (NextPointNumber > 5)    //дошли до конца миссии         //13-06-24
                            return 6;               //летим на ферму
                        else
                            return 5;               //значит бежим к очередному боссу без Ctrl
                }
                else
                {
                    //if (NextPointNumber > 7)    //дошли до конца миссии
                        if (NextPointNumber > 5)    //дошли до конца миссии         //13-06-24
                            return 6;               //летим на ферму
                    else
                        return 3;                   //ни пробела, ни Ctrl на нажато. Значит далее бежим с Ctrl.
                                                    //но проверяем через 1-2 сек, не пропал ли Ctrl.
                                                    //Это бы означало, что добежали до места, перебиты все монстры
                                                    //надо собирать лут
                }
            }
            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            if (isTown() && isCastilia())
                return 6;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// проверяем, если ли проблемы при работе в Кастилии (стадия 3. Мост) и возвращаем номер проблемы
        /// 4,5,7,8,9
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage8()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //если диалог 
            if (dialog.isDialog())
                return 7;                       //получаем бафф "наследие"
            //в миссии
            if (isWork())
            {
                if (isBridge())
                    if (isAncientBlessing(1))
                        return 4;               //на мосту и получили бафф наследие
                    else
                        if (isOpenMapBridge())
                        return 5;               //на мосту, но пока не получили бафф наследие. карта открыта
                    else
                        return 9;               //на мосту, но пока не получили бафф наследие. карта не открыта
            }
            //в городе
            if (isTown())
                return 8;                       //в городе. летим на мост

            //если проблем не найдено
            return 0;
        }

        #endregion 

        #region ======================== Поиск проблем на ФЕРМЕ =================================

        /// <summary>
        /// проверяем, если ли проблемы при работе на ферме (стадия 1.Вход в миссию) и возвращаем номер проблемы
        /// 5,6,7,8,9
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemAllinOneStage9()
        {
            int result = NumberOfProblemCommonForAll();
            if (result != 0) return result;

            //диалог
            if (dialog.isDialog())
                return 8;        //Farm Manager               

            //город
            if (isTown())
            {
                //if (isReboldo())
                //    return 6;
                if (isUstiar())
                    return 5;
                else
                    return 6;       //если не Юстиар, то летим в Юстиар
            }

            //на ферме
            if (isWork())
                if (isRewardAvailable())
                    return 7;
                else
                    return 9;

            //если проблем не найдено
            return 0;
        }

        #endregion

        #region ======================== Решение стандартных проблем ================================

        /// <summary>
        /// разрешение выявленных проблем. Стандартные проблемы (для стадии 1)
        /// 1,11,12,16,17,19,20,22,23,24,33,34,35,36,37,38
        /// </summary>
        public void problemResolutionCommonForStage1(int numberOfProblem)
        {
            switch (numberOfProblem)
            {
                case 1:
                    QuickConnect();                             // Logout-->Barack  
                    botParam.HowManyCyclesToSkip = 2;  //1
                    break;
                case 2:
                    FromBarackToTown(2);                        // barack --> town
                    botParam.HowManyCyclesToSkip = 4;  //2
                    break;
                case 11:                                        // закрыть службу Стим
                    CloseSteam();
                    break;
                case 12:                                        // закрыть магазин 
                    CloseMerchReboldo();
                    break;
                case 13:                                        // закрыть магазин 
                    CloseMerchReboldo2();
                    break;
                case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    PressYesBarack();
                    break;
                case 17:                                        // в бараках на стадии выбора группы
                    botwindow.PressEsc();                       // нажимаем Esc
                    break;
                case 19:                                        // включить правильную стойку
                    ProperFightingStanceOn();
                    MoveCursorOfMouse();
                    break;
                case 20:
                    ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                            //то нажимаем кнопку, чтобы войти обратно в барак
                    break;
                case 22:
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                    //if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                    //{
                        //RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                        runClientCW();                       // если нет чистого окна ГЭ, но загружен Steam, то запускаем окно ГЭ -----------------------
                        botParam.HowManyCyclesToSkip = 7;   //30.10.2023    //пропускаем следующие 6-8 циклов
                        IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        IsItAlreadyPossibleToUploadNewSteam = 0;
                    //}
                    break;
                case 23:                                    //стим есть. только что нашли новое окно с игрой
                                                            //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                    IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewSteam = 0;
                    break;
                case 24:                //если нет стима, значит удалили песочницу
                                        //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                    //if (IsItAlreadyPossibleToUploadNewSteam == 0)
                    //{
                        //************************ запускаем стим ************************************************************
                        //runClientSteamBH();              // если Steam еще не загружен, то грузим его
                        runClientSteamCW();              // если чистый Steam еще не загружен, то грузим его --------------------------------
                        botParam.HowManyCyclesToSkip = 3;        //пропускаем следующие циклы (от 2 до 4)
                        //IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                    //}
                    break;
                //case 33:
                //    CloseError820();
                //    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                //    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                //                                                // а значит смело можно грузить окно еще раз
                //    break;
                //case 34:
                //    AcceptUserAgreement();
                //    break;
                //case 35:
                //    CloseErrorSandboxie();
                    //break;
                //case 36:
                //    CloseUnexpectedError();
                //    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                //    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                //                                                // а значит смело можно грузить окно еще раз
                //    break;
                //case 37:
                //    CloseSteamMessage();
                //    IsItAlreadyPossibleToUploadNewWindow = 0;
                //    break;
                case 38:
                    botwindow.PressEsc();
                    break;
            }
        }

        /// <summary>
        /// разрешение выявленных проблем. Стандартные проблемы для Демоник (для стадий 2+)
        /// 1,12,22,23,24 - на стадию 1.         6,11,16,17,19,20,33,34,35,36,37
        /// </summary>
        public void problemResolutionCommonDemonicForStageFrom2To(int numberOfProblem)
        {
            switch (numberOfProblem)
            {
                case 1:                                         //логаут
                case 12:                                        //закрыть магазин 
                case 22:                                        //если нет окна ГЭ в текущей песочнице
                case 23:                                        //есть окно стим
                case 24:                                        //если нет стима, значит удалили песочницу
                    botParam.Stage = 1;
                    break;
                case 6:                                         // Миссия окончена 
                    Pause(5000);
                    WayToGoDemonic(1);
                    break;
                case 11:                                         // закрыть службу Стим
                    CloseSteam();
                    break;
                case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    PressYesBarack();
                    break;
                case 17:                                        // в бараках на стадии выбора группы
                    botwindow.PressEsc();                       // нажимаем Esc
                    break;
                case 19:                                         // включить правильную стойку
                    ProperFightingStanceOn();
                    MoveCursorOfMouse();
                    break;
                case 20:
                    ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                         //то нажимаем кнопку, чтобы войти обратно в барак
                    break;
                case 33:
                    CloseError820();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 34:
                    AcceptUserAgreement();
                    break;
                case 35:
                    CloseErrorSandboxie();
                    break;
                case 36:
                    CloseUnexpectedError();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 37:
                    CloseSteamMessage();
                    IsItAlreadyPossibleToUploadNewWindow = 0;
                    break;
            }
        }

        /// <summary>
        /// разрешение выявленных проблем. Стандартные проблемы для Кастилии (для стадий 2+)
        /// 1,12,22,23,24 - на стадию 6.         6,11,16,17,19,20,33,34,35,36,37
        /// </summary>
        public void problemResolutionCommonCastiliaForStageFrom2To(int numberOfProblem)
        {
            switch (numberOfProblem)
            {
                case 1:                                         //логаут
                case 12:                                        //закрыть магазин 
                case 22:                                        //если нет окна ГЭ в текущей песочнице
                case 23:                                        //есть окно стим
                case 24:                                        //если нет стима, значит удалили песочницу
                case 2:                                         // в бараках  
                case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                case 17:                                        // в бараках на стадии выбора группы
                case 20:                                        // если стоят на странице создания нового персонажа
                    botParam.Stage = 6;
                    break;
                case 6:                                         // Миссия окончена. Летим на ферму

                    WayToGoDemonic(1);

                    //GotoBarack();                               // идём в барак, так как для фермы надо выбрать другую команду героев
                    //botParam.Stage = 9;                         //ферма
                    //botParam.HowManyCyclesToSkip = 4;
                    break;
                case 11:                                         // закрыть службу Стим
                    CloseSteam();
                    break;
                case 19:                                         // включить правильную стойку
                    ProperFightingStanceOn();
                    MoveCursorOfMouse();
                    break;
                case 33:
                    CloseError820();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 34:
                    AcceptUserAgreement();
                    break;
                case 35:
                    CloseErrorSandboxie();
                    break;
                case 36:
                    CloseUnexpectedError();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 37:
                    CloseSteamMessage();
                    IsItAlreadyPossibleToUploadNewWindow = 0;
                    break;
            }
        }


        #endregion ==================================================================================

        #region ======================== Решение проблем Демоник ====================================

        /// <summary>
        /// разрешение выявленных проблем в Демонике. стадия 1. Вход в миссию
        /// </summary>
        public void problemResolutionAllinOneStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage1();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 4:     //зависли в БХ
                            numberOfProblem = 18;
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                switch (numberOfProblem)
                {
                    case 2:
                        FromBarackToTown(2);                        // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    case 3:                                         // старт миссии      //ок
                        MissionStart();
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 4:                                         // BH --> Gate
                        Pause(1000);
                        //if (isPioneerJournal()) GetGiftsNew();
                        botwindow.PressEscThreeTimes();
                        AddBullets();
                        GoToInfinityGateDem();
                        break;
                    case 5:
                        CreatingMission();
                        break;
                    case 6:                                         // town --> BH
                        //if (isPioneerJournal())
                        //    GetGiftsNew();

                        botwindow.PressEscThreeTimes();
                        Pause(500);
                        SummonPet();

                        //вариант 1. не идём в Демоник, а сразу идём в Кастилию
                        WayToGoDemonic(2);

                        //вариант 2. Идём в Демоник
                        //WayToGoDemonic(0);
                        break;
                    case 7:                                         // поднимаем камеру максимально вверх, активируем пета и переходим к стадии 2
                        botwindow.CommandMode();
                        BattleModeOnDem();                          //пробел

                        botwindow.ThirdHero();                      //эксперимент от 29-01-2024

                        ChatFifthBookmark();

                        //MoveCursorOfMouse();
                        WhatsHeroes();
                        //this.Hero[1] = WhatsHero(1);
                        //this.Hero[2] = WhatsHero(2);
                        //this.Hero[3] = WhatsHero(3);

                        ActivatePetDem();                             //новая функция  22-11
                        MaxHeight(12);

                        //бафаемся, пока не вылезли мобы
                        botwindow.ActiveAllBuffBH();
                        botwindow.PressEsc(4);
                        //бафаемся героями первый раз
                        BuffHeroes();
                        MoveCursorOfMouse();
                        //Buff(this.Hero[1], 1);
                        //Buff(this.Hero[2], 2);
                        //Buff(this.Hero[3], 3);
                        //бафаемся героями второй раз
                        BuffHeroes();
                        MoveCursorOfMouse();
                        //Buff(this.Hero[1], 1);
                        //Buff(this.Hero[2], 2);
                        //Buff(this.Hero[3], 3);
                        ManaForDemonic();
                        MoveCursorOfMouse();

                        botParam.Stage = 2;
                        //botParam.HowManyCyclesToSkip = 1;           // даём время, чтобы вылезли мобы
                        break;
                    case 8:                                         //Gate --> Mission Lobby
                        dialog.PressStringDialog(1);                //нажимаем нижнюю строчку (I want to play)
                        break;
                    case 9:                                         //нет наследия. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(1, true);                          // телепорт на мост (первый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        botParam.Stage = 5;
                        break;
                    case 10:                                        //миссия не доступна на сегодня (уже прошли)
                        dialog.PressOkButton(1);                    //выходим из диалога
                        Pause(1000);
                        //======================================================================================================================
                        WayToGoDemonic(1);
                        //======================================================================================================================
                        break;
                    case 18:
                        //новейший вариант
                        GotoBarack();
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 40:
                        botParam.Stage = 7;
                        break;
                    //case 41:                                  // перенёс в п.9
                    //    SummonPet();
                    //    break;
                    case 42:
                        botParam.Stage = 9;     //Юстиар
                        break;
                    case 43:
                        botParam.Stage = 6;     //Кастилия (около шахты)
                        break;
                    default:
                        problemResolutionCommonForStage1(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        /// <summary>
        /// разрешение выявленных проблем в Демонике. стадия 2. Миссия
        /// 2, 3, 4, 10
        /// </summary>
        public void problemResolutionAllinOneStage2()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage2();

                switch (numberOfProblem)
                {
                    case 2:                                                 // в бараках
                        ReturnToMissionFromBarack();                        // идем из барака обратно в миссию     
                        MoveCursorOfMouse();
                        botParam.Stage = 3;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 3:                                                 // собираемся атаковать
                        DirectionOfMovement = -1 * DirectionOfMovement;     // меняем направление движения
                        AttackTheMonsters(DirectionOfMovement);             // атакуем с CTRL

                        MoveCursorOfMouse();
                        BattleModeOnDem();
                        break;
                    //case 6:                                     // Миссия окончена //перенёс в стандартные проблемы
                    //    //RemoveSandboxieBH();                  //закрываем песочницу
                    //    Teleport(4, true);                      //телепорт в Кастилию
                    //    botParam.Stage = 6;                     //переходим на стадию Кастилия (Stage 1)    
                    //    botParam.HowManyCyclesToSkip = 4;
                    //    break;
                    case 4:
                        ActivatePet();
                        break;
                    case 10:
                        //если появился сундук
                        BattleModeOn();                      //нажимаем пробел, чтобы не убежать от дропа
                        GotoBarack();                        // идем в барак, чтобы перейти к стадии 3 (открытие сундука и проч.)
                        botwindow.PressEscThreeTimes();
                        botParam.HowManyCyclesToSkip = 1;   //21-09-2024
                        //botParam.HowManyCyclesToSkip = 3;
                        break;

                    default:
                        problemResolutionCommonDemonicForStageFrom2To(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(3000);
            }
        }

        /// <summary>
        /// разрешение выявленных проблем в Демонике. стадия 3. открытие сундука
        /// 2, 3, 8
        /// </summary>
        public void problemResolutionAllinOneStage3()
        {
            WriteToLogFileBH("перешли к выполнению стадии 3");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);
                }
                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage3();

                switch (numberOfProblem)
                {
                    case 2:                         //в бараках   ======== этот пункт нельзя комментить ==========
                                                    // т.к. при возврате в миссию для открытия сундука сработает
                                                    // условие,  (если барак, то RemoveSandboxie)  // тогда мы не сможем открыть сундук
                        FromBarackToTown(2);        //здесь по идее можно писать, что угодно. лишь бы не удаление песочницы
                        botParam.HowManyCyclesToSkip = 4;    //было 2  //29-07-24
                        break;
                    case 3:                                         //в миссии, но сундук ещё не открыт
                        ActivatePetDem();                           //активируем пета (может он успеет собрать не подобранные вещи)
                        OpeningTheChest();                          //тыкаем в сундук и запускаем рулетку
                        MaxHeight(10);                              //чтобы точно было видно вторые ворота
                        PressOnFesoGate();
                        botwindow.Pause(2000);
                        if (!dialog.isDialog())
                        {
                            botwindow.Pause(6000);                  //ждём рулетку и собираем лут
                            WayToGoDemonic(1);
                        }
                        else
                        {
                            dialog.PressStringDialog(1);
                            dialog.PressStringDialog(1);
                            dialog.PressOkButton(1);
                            if (dialog.isDialog()) dialog.PressOkButton(1);
                            botwindow.Pause(5000);
                            //AttackCtrlToLeft();        //старый вариант
                            BattleModeOnDem();           //новый вариант
                            BattleModeOnDem();           //новый вариант
                            ActivatePetDem();
                            botParam.HowManyCyclesToSkip = 5;
                            botParam.Stage = 4;
                        }
                        break;
                    //case 6:
                    //    RemoveSandboxieBH();                 //закрываем песочницу
                    //    botParam.Stage = 1;
                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    case 8:                                         // появились ворота фесо
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        if (dialog.isDialog()) dialog.PressOkButton(1);
                        botwindow.Pause(3000);
                        AttackCtrlToLeft();
                        ActivatePetDem();
                        botParam.Stage = 4;
                        break;

                    default:
                        problemResolutionCommonDemonicForStageFrom2To(numberOfProblem);
                        break;

                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }

        }

        /// <summary>
        /// разрешение выявленных проблем в Демонике. стадия 4. Фесо
        /// 3,5,7,8
        /// </summary>
        public void problemResolutionAllinOneStage4()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage4();

                switch (numberOfProblem)
                {
                    //case 6:
                    //    RemoveSandboxieBH();                 //закрываем песочницу
                    //    botParam.Stage = 1;
                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    case 2:                                         //в бараках. если из карты с фесо попали в барак, то на стадию 1 (???)
                        botParam.Stage = 1;
                        break;
                    case 3:                                         //в комнате с фесо
                        NeedToPickUpFeso = true;
                        break;
                    case 5:                                         // в миссии. уже не бьём мобов. пора собирать фесо
                        PickUpToLeft();
                        break;
                    case 7:                                         // в миссии. в процессе сбора фесо
                        break;
                    case 8:
                        PickUpToRight();
                        break;
                    default:
                        problemResolutionCommonDemonicForStageFrom2To(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        /// <summary>
        /// разрешение выявленных проблем в Демонике. стадия 5. Мост. Бафф Наследие
        /// 2, 3, 4, 5, 7, 8
        /// </summary>
        public void problemResolutionAllinOneStage5()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage5();

                switch (numberOfProblem)
                {

                    case 2:                                         //в бараках  
                        botParam.Stage = 1;
                        break;
                    case 3:                                         //на мосту, но пока не получили бафф наследие. карта не открыта
                        MinHeight(10);
                        OpenMapBridge();
                        break;
                    case 4:                                         //на мосту и получили бафф наследие
                        //GotoBarack();
                        Logout();
                        botParam.HowManyCyclesToSkip = 2;
                        botParam.Stage = 1;
                        break;
                    case 5:                                         //на мосту, но пока не получили бафф наследие. карта открыта
                        GotoAncientBlessingStatue();
                        break;
                    case 7:                                         //получаем бафф "наследие"
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        break;
                    case 8:                                         //в городе. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(1, true);                          // телепорт на мост (первый телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(2500);
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        break;
                    default:
                        problemResolutionCommonDemonicForStageFrom2To(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        #endregion ==================================================================================

        #region ======================== Решение проблем Кастилия ===================================

        /// <summary>
        /// разрешение выявленных проблем в Кастилии (стадия 1)
        /// 2-10, 18
        /// </summary>
        public void problemResolutionAllinOneStage6()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);
                }


                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage6();
                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 4:  //зависли в Кастилии
                            numberOfProblem = 18; //переходим по телепорту снова к зелёной стрелке
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                switch (numberOfProblem)
                {
                    case 2:
                        FromBarackToTown(2);                        // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    case 3:                                         // старт миссии      //ок
                        MissionStart();
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 4:                                         // Castilia --> Green Arrow
                        AddBullets();
                        GoToMissionCastilia();
                        break;
                    case 5:                                         //Mission Lobby --> Mission Room
                        CreatingMission();
                        break;
                    case 6:                                         // town --> Castilia
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(4, true);                          // телепорт в Кастилию
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 7:                                         // начало миссии
                                                                    // активируем пета и переходим к стадии 2
                        botwindow.CommandMode();
                        BattleModeOnDem();                          //пробел

                        WhatsHeroes();
                        //Hero[1] = WhatsHero(1);
                        //Hero[2] = WhatsHero(2);
                        //Hero[3] = WhatsHero(3);

                        ActivatePetDem();                             //новая функция  22-11
                        botwindow.ThirdHero();
                        botParam.Stage = 7;
                        break;
                    case 8:                                         //Green Arrow --> Mission Lobby
                        dialog.PressStringDialog(1);                //I want to play
                        dialog.PressStringDialog(4);                //Normal Mode
                        break;
                    case 9:                                         //town --> bridge
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(1, true);                          // телепорт на мост (первый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        botParam.Stage = 8;                         //мост. наследие
                        break;
                    case 10:                                        //миссия не доступна на сегодня (уже прошли)
                        dialog.PressOkButton(1);
                        //TeleportAltW(4);

                        WayToGoDemonic(1);

                        //GotoBarack();
                        //botParam.Stage = 9;                          //ферма
                        //botParam.HowManyCyclesToSkip = 4;
                        
                        //старый вариант
                        //RemoveSandboxieBH();                        //закрываем песочницу и берём следующий бот для работы
                        //botParam.Stage = 1;
                        //botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 18:
                        //новый вариант                             //переход по телепорту в Кастилии к воротам
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(4, true);                          // телепорт в Гильдию Охотников (третий телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        //PressToGateDemonic();
                        prevProblem = 6;                            // делаем предыдущее состояние = город        
                                                                    // а иначе программа считает, что мы всё еще застряли в Кастилии,
                                                                    // стоим не там и опять попадаем сюда же (бесконечный цикл)
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    default:
                        problemResolutionCommonForStage1(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        /// <summary>
        /// разрешение выявленных проблем в Кастилии (стадия 2)
        /// 3,4,5,7,8
        /// </summary>
        public void problemResolutionAllinOneStage7()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage7();

                switch (numberOfProblem)
                {
                    case 3:                                                 // ни пробела, ни Ctrl не нажато (значит бежали до этого)
                        AssaultToNextPoint(NextPointNumber);
                        Pause(2000);
                        if (!isAssaultMode())    //если боевой режим пропал, значит пора собирать дроп
                        {
                            //новый вариант. сразу начинаем собирать дроп справа
                            NeedToPickUpRight = false;
                            NeedToPickUpLeft = true;
                            //GetDropCastiliaRight(GetWaitingTimeForDropPicking(NextPointNumber));
                            GetDropCastiliaRight(NextPointNumber);
                            BattleModeOnDem();
                        }
                        break;
                    case 4:                                                 // бежим с Ctrl. на всякий случай даём указание бить в ту же точку,
                                                                            // так как бывают случаи, что не все герои бьются, а только 1-2
                        AssaultToNextPoint(NextPointNumber);
                        break;
                    case 5:                                                 //стоим на пробеле, NeedToPickUpRight=false, NeedToPickUpLeft=false
                        // бафаемся перед перемещением дальше
                        if ((NextPointNumber == 0) || (NextPointNumber == 2) || (NextPointNumber == 4))
                        {
                            MoveCursorOfMouse();
                            BuffHeroes();
                            //Buff(Hero[1], 1);
                            //Buff(Hero[2], 2);
                            //Buff(Hero[3], 3);
                            botwindow.ActiveAllBuffBH();
                            botwindow.PressEscThreeTimes();
                        }
                        MoveToNextPoint(NextPointNumber);
                        botParam.HowManyCyclesToSkip = GetWaitingTurnForMoveToPoint(NextPointNumber);
                        break;
                    case 7:                                                 //на пробеле, NeedToPickUpRight=true, NeedToPickUpLeft = false,
                        NeedToPickUpRight = false;
                        NeedToPickUpLeft = true;
                        //GetDropCastiliaRight(GetWaitingTimeForDropPicking(NextPointNumber));
                        GetDropCastiliaRight(NextPointNumber);
                        BattleModeOnDem();
                        break;
                    case 8:                                                 //на пробеле, NeedToPickUpLeft=true
                        NeedToPickUpLeft = false;
                        NeedToPickUpRight = false;
                        //GetDropCastiliaLeft(GetWaitingTimeForDropPicking(NextPointNumber));
                        GetDropCastiliaLeft(NextPointNumber);
                        BattleModeOnDem();
                        NextPointNumber++;
                        //if (NextPointNumber > 7)
                            
                        break;
                    default:
                        problemResolutionCommonCastiliaForStageFrom2To(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        /// <summary>
        /// разрешение выявленных проблем в Кастилии (стадия 3)
        /// 4,5,7,8,9
        /// </summary>
        public void problemResolutionAllinOneStage8()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage8();

                switch (numberOfProblem)
                {
                    case 4:                                         //на мосту и получили бафф наследие
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        MaxHeight(12);
                        Teleport(4, true);                          // телепорт в Кастилию (четвёртый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        botParam.Stage = 6;
                        break;
                    case 5:                                         //на мосту, но пока не получили бафф наследие. карта открыта
                        GotoAncientBlessingStatue();
                        break;
                    case 9:                                         //на мосту, но пока не получили бафф наследие. карта не открыта
                        MinHeight(10);
                        OpenMapBridge();
                        break;
                    case 7:                                         //получаем бафф "наследие"
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        break;
                    case 8:                                         //в городе. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        Teleport(1, true);                          // телепорт на мост (первый телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(2500);
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        break;
                    //===========================================================================================
                    default:
                        problemResolutionCommonCastiliaForStageFrom2To(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        #endregion

        #region ======================== Решение проблем ФЕРМА ======================================

        /// <summary>
        /// разрешение выявленных проблем на ферме (стадия 1)
        /// 2,5,6,7,8,9
        /// </summary>
        public void problemResolutionAllinOneStage9()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemAllinOneStage9();


                switch (numberOfProblem)
                {
                    case 2:
                        FromBarackToTown(3);
                        botParam.HowManyCyclesToSkip = 3;           //из барака в город
                        break;
                    case 5:                                         // в Юстиаре
                        botwindow.PressEscThreeTimes();
                        GotoFarmManager();
                        break;
                    case 6:                                         // в Ребольдо
                        if (isPioneerJournal())  GetGiftsNew();
                        GoToUstiar();
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 7:                                         // на ферме. доступна награда
                        GetReward();
                        Pause(1000);
                        GoToEnd();
                        Pause(7000);
                        RemoveSandboxieCW();                        //закрываем чистый Стим и берём следующего бота в работу
                        //RemoveSandboxieBH();                        //закрываем песочницу и берём следующего бота в работу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 8:                                         //диалог (Farm Manager --> Farm)
                        dialog.PressStringDialog(1);
                        if (dialog.isDialog())
                        {
                            dialog.PressStringDialog(1);
                            dialog.PressOkButton(1);
                        }
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 9:                                         //на ферме. пока не доступна награда
                        Pause(2000);
                        GetReward();
                        Pause(1000);
                        GoToEnd();
                        Pause(7000);
                        RemoveSandboxieCW();                        //закрываем чистый Стим и берём следующего бота в работу
                        //RemoveSandboxieBH();                        //закрываем песочницу и берём следующего бота в работу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    default:
                        problemResolutionCommonForStage1(numberOfProblem);
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts <= 1) Pause(5000);
            }
        }

        #endregion


        #endregion ================================= All in One (end) =========================================== 

        #region ===========================  CapibaraEvent ===================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в CapibaraEvent (стадия 1) и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemCapibaraStage1()
        {
            //в логауте
            if (isLogout()) return 1;   //*

            //в бараке
            if (isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (isBarack()) return 2;                    //если стоят в бараке          //*
            if (isBarackWarningYes()) return 16;        //*
            if (isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы     //*

            //диалог
            if (dialog.isDialog())
                return 8;                           //если стоим в устройстве по запуску миссий


            //в миссии
            if (isWork())
                return 10;                      //в миссии      //*


            //город или миссия
            if (isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                if (isReboldo())
                    return 9;                       //ребольдо      //*
                else 
                    return 10;                      //в миссии      //*
            }

            
            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в CapibaraEvent. стадия 1.
        /// </summary>
        public void problemResolutionCapibaraStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemCapibaraStage1();

                switch (numberOfProblem)
                {
                    case 1:
                        QuickConnect();                             // Logout-->Barack  
                        botParam.HowManyCyclesToSkip = 2;  
                        break;
                    case 2:
                        FromBarackToTown(3);                        // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  
                        break;
                    case 8:                                         //диалог
                        DialogInEventGameConsole();
                        break;
                    case 9:                                         //в городе
                        GoToEventGameConsole();
                        PressEventGameConsole();
                        if (!dialog.isDialog())
                        {
                            GotoBarack();
                            botParam.HowManyCyclesToSkip = 3;  
                        }
                        break;
                    case 10:                                        //пришли в миссию
                        MissionCapibara();
                        //GotoBarack();
                        botParam.HowManyCyclesToSkip = 1;  
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 20:
                        ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                             //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(5000);
            }
        }


        #endregion ===============================================================================

        #region  =========================== методы для GE Classic =======================================

        /// <summary>
        /// проверяем, сколько заданий на мемориале
        /// </summary>
        /// <returns>true, если одно</returns>
        private bool isMemorialOne()
        {
            return new PointColor(731 - 5 + xx, 241 - 5 + yy, 4600000, 5).isColor() &&
                   new PointColor(731 - 5 + xx, 242 - 5 + yy, 4600000, 5).isColor();
        }

        /// <summary>
        /// взять задание в диалоге на мемориале
        /// </summary>
        private void GetTask()
        {
            if (isMemorialOne())
            {
                dialog.PressStringDialogClassic(2);
                dialog.PressOkButtonClassic(1);
                dialog.PressStringDialogClassic(1);
                dialog.PressOkButtonClassic(1);
            }
            else
            {
                dialog.PressStringDialogClassic(2);
                dialog.PressOkButtonClassic(2);
            }
        }

        /// <summary>
        /// используем i-й скил у j-го героя
        /// </summary>
        private void BuffSkillClassic(int i, int j)
        {
            new Point(26 - 5 + xx + (j - 1) * 255 + (i - 1) * 31, 715 - 5 + yy).PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// бафаемся. баффер на i-м месте
        /// </summary>
        private void BuffClassic(int i)
        {
            BuffSkillClassic(1, i);
            Pause(3000);
            BuffSkillClassic(4, i);
            Pause(3000);
            BuffSkillClassic(5, i);
        }

        /// <summary>
        /// проверяем, призван ли пет
        /// </summary>
        /// <returns>true, если призван</returns>
        private bool isSummonPetClassic()
        {
            return !(new PointColor(216 - 5 + xx, 334 - 5 + yy, 7700000, 5).isColor() &&
                   new PointColor(216 - 5 + xx, 335 - 5 + yy, 7700000, 5).isColor());
        }

        /// <summary>
        /// призываем пета
        /// </summary>
        private void SummonPetClassic()
        {
            if (!isSummonPetClassic())
            {
                new Point(360 - 5 + xx, 354 - 5 + yy).PressMouseL();                     //тыкаем в верхнюю строчку списка с петами
                Pause(500);
                new Point(238 - 5 + xx, 335 - 5 + yy).PressMouseL();                     //тыкаем в кнопку Summon
                Pause(500);
            }
        }



        /// <summary>
        /// проверяем, включён ли пет
        /// </summary>
        /// <returns>true, если включён</returns>
        public bool isActivePetClassic()
        {
            return new PointColor(269 - 5 + xx, 292 - 5 + yy, 13000000, 6).isColor() &&
                   new PointColor(269 - 5 + xx, 293 - 5 + yy, 13000000, 6).isColor();
        }

        /// <summary>
        /// активируем пета
        /// </summary>
        private void ActivationPetClassic()
        {
            new Point(382 - 5 + xx, 52 - 5 + yy).PressMouseL();                     //тыкаем в верхнем меню кнопку "Pet Information"
            Pause(2000);
            SummonPetClassic();
            if (!isActivePetClassic())  
                new Point(238 - 5 + xx, 395 - 5 + yy).PressMouseL();                     //тыкаем в кнопку Activation
            botwindow.PressEsc(3);
            Pause(500);
        }

        /// <summary>
        /// идём в логаут
        /// </summary>
        private void GotoBarackClassic()
        {
            new Point(41 - 5 + xx, 49 - 5 + yy).PressMouseL();                     //тыкаем в кнопку Menu
            Pause(1000);
            new Point(35 - 5 + xx, 434 - 5 + yy).PressMouseL();                     //тыкаем в кнопку Menu
            botwindow.PressEsc(2);
        }

        /// <summary>
        /// проверяем, включён ли боевой режим (пробел)
        /// </summary>
        /// <returns>true, если включён</returns>
        private bool isBattleModeClassic()
        {
            return new PointColor(173 - 5 + xx, 511 - 5 + yy, 9300000, 5).isColor() &&
                   new PointColor(172 - 5 + xx, 512 - 5 + yy, 9300000, 5).isColor();
        }


        /// <summary>
        /// . GE Classic
        /// </summary>
        private bool isBarackClassic()
        {
            return new PointColor(105 - 5 + xx, 55 - 5 + yy, 15500000, 5).isColor() &&
                   new PointColor(105 - 5 + xx, 56 - 5 + yy, 15500000, 5).isColor();
        }

        /// <summary>
        /// проверяем в бараке, все ли герои убиты. GE Classic
        /// </summary>
        private bool isKillHeroesBarackClassic()
        {
            return !(new PointColor(506 - 5 + xx, 545 - 5 + yy, 3000000, 5).isColor() &&              //Coimbra
                   new PointColor(506 - 5 + xx, 546 - 5 + yy, 5100000, 5).isColor()
                   ||
                   new PointColor(506 - 5 + xx, 545 - 5 + yy, 6300000, 5).isColor() &&               //Reboldo
                   new PointColor(506 - 5 + xx, 546 - 5 + yy, 5000000, 5).isColor())
                   ;
            //return new PointColor(506 - 5 + xx, 545 - 5 + yy, 700000, 5).isColor() &&
            //       new PointColor(506 - 5 + xx, 546 - 5 + yy, 700000, 5).isColor()
            //       ||
            //       new PointColor(506 - 5 + xx, 545 - 5 + yy, 500000, 5).isColor() &&
            //       new PointColor(506 - 5 + xx, 546 - 5 + yy, 500000, 5).isColor()
            //       ;
        }

        /// <summary>
        /// на работе проверяем, убит ли i-й герой
        /// </summary>
        /// <param name="i">номер героя</param>
        /// <returns></returns>
        private bool isKillHeroClassic(int i)
        {
            return new PointColor(77 - 5 + xx + (i - 1) * 255, 637 - 5 + yy, 980000, 4).isColor() &&
                   new PointColor(78 - 5 + xx + (i - 1) * 255, 637 - 5 + yy, 980000, 4).isColor();
        }

        /// <summary>
        /// на работе проверяем, убиты ли все герои
        /// </summary>
        /// <returns></returns>
        private bool isKillHeroesClassic()
        {
            return isKillHeroClassic(1) && isKillHeroClassic(2) && isKillHeroClassic(3);
        }

        /// <summary>
        /// выбор персонажей 
        /// </summary>
        private void TeamSelectionClassic()
        { 
            if (isKillHeroesBarackClassic())
            {
                new Point(349 - 5 + xx, 384 - 5 + yy).DoubleClickL();
                new Point(581 - 5 + xx, 405 - 5 + yy).DoubleClickL();
                new Point(693 - 5 + xx, 378 - 5 + yy).DoubleClickL();
            }
        }

        /// <summary>
        /// летим по i-му телепорту 
        /// </summary>
        private void TeleportClassic(int i)
        {
            new Point(347 - 5 + xx, 50 - 5 + yy).PressMouseL();                     //тыкаем в телепорт в верхнем меню
            Pause(1000);
            new Point(262 - 5 + xx, 257 - 5 + (i - 1) * 15 + yy).DoubleClickL();    //тыкаем в выбранную строку телепорта
            Pause(500);
            new Point(277 - 5 + xx, 476 - 5 + yy).PressMouseL();                    //тыкаем в кнопку "Go"
            botwindow.PressEsc(3);
        }

        /// <summary>
        /// начинаем со стартового города
        /// </summary>
        private void NewPlaceClassic()
        {
            new Point(548 - 5 + xx, 676 - 5 + yy).PressMouseLL();
        }

        /// <summary>
        /// переход из казарм в стартовый город
        /// </summary>
        public void FromBarackToTownClassic()
        {

            TeamSelectionClassic();
            Pause(500);

            NewPlaceClassic();                //начинаем в стартовом городе

            //botwindow.ToMoveMouse();             //убираем мышку в сторону, чтобы она не загораживала нужную точку для isTown
        }

        /// <summary>
        /// вызываем Zone Map. GE Classic
        /// </summary>
        public void OpenZoneMapClassic()
        {
            new Point(284 - 5 + xx, 49 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// проверяем, открыта ли карта (ZoneMap). GE Classic
        /// </summary>
        public bool isOpenZoneMapClassic()
        {
            return new PointColor(101 - 5 + xx, 56 - 5 + yy, 13100000, 5).isColor() &&
                   new PointColor(102 - 5 + xx, 56 - 5 + yy, 13100000, 5).isColor();
        }

        /// <summary>
        /// бежим до места работы в открытой карте
        /// </summary>
        private void GoToWorkOnMap(int i)
        {
            int[] X = {
            175,            //TetraCatacomb (проверить)
            404,            //PortoBello 
            436,            //LagoDeTres (2)
            329,            //GaviGivi (3)
            274,            //Torsche (4)
            258,            //Rion  (5)
            413,            //Crater (6)
            381,            //Lava (7)

            };
            int[] Y = {
            416,            //TetraCatacomb (проверить)
            377,            //PortoBello 
            279,            //LagoDeTres (2)
            446,            //GaviGivi (3)
            312,            //Torsche (4) 
            205,            //Rion  (5)
            438,            //Crater (6)
            195,            //Lava (7)

            };
            new Point(X[i] - 5 + xx, Y[i] - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// возвращает время бега до места работы
        /// </summary>
        private int TimeForRun(int i)
        {
            int[] Time = {
            3000,            //TetraCatacomb (проверить)
            3000,            //PortoBello
            10000,           //LagoDeTres (2)
            10000,           //GaviGivi (3)
            12000,           //Torsche (4)
            5000,            //rion  (5)
            4000,            //Crater (6)
            5000,            //Lava (7)
            };

            return Time[i];
        }


        /// <summary>
        /// идём к следующей точке кача
        /// </summary>
        public void GotoNextClassic(int i)
        {
            botwindow.PressEsc(2);
            OpenZoneMapClassic();
            Pause(1000);
            GoToWorkOnMap(i);           //==========        бежим        ================
            botwindow.PressEsc(2);
            Pause(TimeForRun(i));       //=========== ждём пока прибежим ================
            BattleModeOnDem();
            ActivationPetClassic();
        }


        /// <summary>
        /// нажимаем на мемориал в открытой карте
        /// </summary>
        private void PressMeMorialOnMap(int i)
        {
            int [] X = {
            175,            //TetraCatacomb (проверить)
            419,            //PortoBello 
            432,            //LagoDeTres (2)
            278,            //GaviGivi (3)
            256,            //Torsche (4)
            273,            //Rion  (5)
            398,            //Crater (6)
            359,            //Lava (7)

            };
            int [] Y = {
            416,            //TetraCatacomb (проверить)
            357,            //PortoBello 
            229,            //LagoDeTres (2)
            507,            //GaviGivi (3)
            344,            //Torsche (4)
            183,            //Rion  (5)
            455,            //Crater (6)
            148,            //Lava (7)

            };
            new Point(X[i] - 5 + xx, Y[i]  - 5 + yy).PressMouseL();                    
        }

        /// <summary>
        /// идём к мемориалу
        /// </summary>
        public void GotoMemorialClassic(int i)
        {
            botwindow.PressEsc(2);
            OpenZoneMapClassic();
            Pause(1000);
            PressMeMorialOnMap(i);   //==========        бежим к мемориалу      ================
            botwindow.PressEsc(2);
            Pause(5000);
            BattleModeOnDem();
        }

        /// <summary>
        /// идём к мемориалу
        /// </summary>
        public void PressMeMorial()
        {
            MinHeight(5);
            new Point(522 - 5 + xx, 322 - 5 + yy).PressMouseL();
            Pause(1000);
        }

        /// <summary>
        /// проверяем, выполнено задание номер i. GE Classic
        /// </summary>
        public bool isTaskDoneClassic(int i)
        {
            return 
                   new PointColor(798 - 5 + xx, 412 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor() &&       //PortoBello
                   new PointColor(798 - 5 + xx, 413 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor()
                   //||
                   //new PointColor(805 - 5 + xx, 410 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor() &&       //ОзероТрехСестер
                   //new PointColor(805 - 5 + xx, 411 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor()
                   ||
                   new PointColor(798 - 5 + xx, 412 - 5 + yy + (i - 1) * 47, 7000000, 6).isColor() &&       //Торше
                   new PointColor(798 - 5 + xx, 413 - 5 + yy + (i - 1) * 47, 7000000, 6).isColor()
                   ||
                   new PointColor(817 - 5 + xx, 410 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor() &&       //Торше
                   new PointColor(817 - 5 + xx, 411 - 5 + yy + (i - 1) * 47, 8000000, 6).isColor()
                   ;
        }

        /// <summary>
        /// проверяем, получено ли задание номер i. GE Classic
        /// </summary>
        public bool isTaskReceivedClassic(int i)
        {
            return new PointColor(798 - 5 + xx, 412 - 5 + yy + (i - 1) * 47, 15000000, 6).isColor() &&
                   new PointColor(798 - 5 + xx, 413 - 5 + yy + (i - 1) * 47, 15000000, 6).isColor()
                   ||
                   new PointColor(798 - 5 + xx, 412 - 5 + yy + (i - 1) * 47, 16000000, 6).isColor() &&
                   new PointColor(798 - 5 + xx, 413 - 5 + yy + (i - 1) * 47, 16000000, 6).isColor();
        }


        /// <summary>
        /// проверяем, находимся ли в городе. GE Classic
        /// </summary>
        /// <returns></returns>
        public bool isTownClassic()
        {
            //проверяем одну и ту же точку на экране в миссии
            //каждой стойке соответствует свой цвет этой точки
            //проверяем две точки для надежности

            // в массиве arrayOfColorsIsTownClassic1 хранятся все цвета первой контрольной точки, которые есть у проверяемых стоек 
            // в массиве arrayOfColorsIsTownClassic2 хранятся все цвета второй контрольной точки, которые есть у проверяемых стоек 
            uint[] arrayOfColorsIsTownClassic1 = new uint[] { 11908, 7829 };
            uint[] arrayOfColorsIsTownClassic2 = new uint[] { 11842, 9342 };
            //муха+ружьё,скаут+стойка1
            uint color1 = new PointColor(29 - 5 + xx, 704 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color2 = new PointColor(30 - 5 + xx, 704 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем

            return arrayOfColorsIsTownClassic1.Contains(color1) && arrayOfColorsIsTownClassic2.Contains(color2);    // проверяем, есть ли цвет контрольной точки в массивах цветов
        }



        /// <summary>
        /// проверяем, находимся ли на работе. GE Classic
        /// </summary>
        /// <returns></returns>
        public bool isWorkClassic()
        {
            //проверяем одну и ту же точку на экране в миссии
            //каждой стойке соответствует свой цвет этой точки
            //проверяем две точки для надежности

            // в массиве arrayOfColorsIsWork1 хранятся все цвета первой контрольной точки, которые есть у проверяемых стоек 
            // в массиве arrayOfColorsIsWork2 хранятся все цвета второй контрольной точки, которые есть у проверяемых стоек 
            uint[] arrayOfColorsIsWorkClassic1 = new uint[] { 11903, 13415, };
            uint[] arrayOfColorsIsWorkClassic2 = new uint[] { 11836, 14925, };
            //муха+ружьё, скаут(баффстойка),
            uint color1 = new PointColor(29 - 5 + xx, 704 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color2 = new PointColor(30 - 5 + xx, 704 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем

            return arrayOfColorsIsWorkClassic1.Contains(color1) && arrayOfColorsIsWorkClassic2.Contains(color2);                // проверяем, есть ли цвет контрольной точки в массивах цветов
        }

        /// <summary>
        /// метод проверяет, находится ли данное окно в режиме логаута, т.е. на стадии ввода логина-пароля. GE Classic
        /// </summary>
        /// <returns></returns>
        public bool isLogoutClassic()
        {
            return new PointColor(834 - 5 + xx, 716 - 5 + yy, 7720000, 4).isColor() &&
                    new PointColor(834 - 5 + xx, 717 - 5 + yy, 7720000, 4).isColor();
        }

        /// <summary>
        /// вводим логин и пароль в соответствующие поля. GE Classic
        /// </summary>
        protected void EnterLoginAndPaswordClassic()
        {
            iPoint pointPassword = new Point(580 - 5 + xx, 355 - 5 + yy);    //  505, 350
            // окно открылось, надо вставить логин и пароль
            pointPassword.PressMouseLL();   //Кликаю в строчку с паролем
            Pause(500);
            SendKeys.SendWait("{TAB}");
            Pause(500);
            SendKeys.SendWait(botParam.Login);
            Pause(500);
            SendKeys.SendWait("{TAB}");
            Pause(500);
            SendKeys.SendWait(botParam.Password);
            Pause(500);
        }

        private void ConnectClassic()
        {
            new Point(408 - 5 + xx, 471 - 5 + yy).PressMouseL();
            Pause(2000);
            new Point(520 - 5 + xx, 429 - 5 + yy).PressMouseL();

        }


        #endregion =========================================================================================

        #region ===========================  Восстановление окон Classic ===================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в CapibaraEvent (стадия 1) и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemRestartStage1()
        {
            //если нет окна
            if (!isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (FindWindowGEClassic())
                    return 3;    //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в файле Hwnd.txt при выполнении функции FindWindowGEClassic)
                else
                    return 2;                  //если нет окна ГЭ в текущей песочнице
            }
            else            //если окно с нужным HWND нашлось
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;

            //в логауте
            if (isLogoutClassic()) return 1;


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в CapibaraEvent. стадия 1.
        /// </summary>
        public int problemResolutionRestartStage1()
        {
            int result = 0;
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemRestartStage1();
                result = numberOfProblem;

                switch (numberOfProblem)
                {
                    case 1:
                        //QuickConnect();                             // Logout
                        //botParam.HowManyCyclesToSkip = 2;
                        EnterLoginAndPaswordClassic();                  // Logout
                        break;
                    case 2:
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            RunClientClassic();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = 1;   //30.10.2023    //пропускаем следующие 7 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 3:                                    //только что нашли новое окно с игрой
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        isHwnd();   //поставили окно на место
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(5000);
            }
            return result;
        }


        #endregion ===============================================================================

        #region ===========================  кач на мемориале Classic ===================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в CapibaraEvent (стадия 1) и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemMemorialStage1()
        {
            //если нет окна
            if (!isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (FindWindowGEClassic())
                    return 5;    //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в файле Hwnd.txt при выполнении функции FindWindowGEClassic)
                else
                    return 4;                  //если нет окна ГЭ в текущей песочнице
            }
            else            //если окно с нужным HWND нашлось
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;


            //в логауте
            if (isLogoutClassic()) return 1;

            //в бараке
            if (isBarackClassic()) return 2;                    //если стоят в бараке          

            //диалог
            if (dialog.isDialogClassic())
                return 8;                           //если стоим в устройстве по запуску миссий


            //убили всех
            if (isKillHeroesClassic())
            {
                return 6;
            }
            //в миссии
            if (isWorkClassic())
                if (isTaskDoneClassic(1) || isTaskDoneClassic(2) || isTaskDoneClassic(3))
                    return 9;   //одно из заданий выполнено
                else
                    return 10;  //задания не выполнены


            //город или миссия
            if (isTownClassic())
            {
                    return 7;                       
            }


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в CapibaraEvent. стадия 1.
        /// </summary>
        public void problemResolutionMemorialStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemMemorialStage1();
                
                 switch (numberOfProblem)
                {
                    case 1:
                        EnterLoginAndPaswordClassic();                      // Logout-->Barack  
                        ConnectClassic();
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:
                        FromBarackToTownClassic();                        // barack --> town              
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 4:                                                 //нет окна
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     
                        {
                            RunClientClassic();                             // запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = 1;               //пропускаем следующий 1 цикл
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 5:                                             //только что нашли новое окно с игрой
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        isHwnd();   //поставили окно на место
                        break;

                    case 6:
                        GotoBarackClassic();
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 7:                                                 //town
                        BattleModeOnDem();                                  //чтобы полечиться
                        Pause(1000);
                        TeleportClassic(10);
                        botParam.HowManyCyclesToSkip = 1;
                        break;

                    case 8:                                         //диалог
                        GetTask();
                        BattleModeOnDem();
                        BuffClassic(botParam.Scout);                    //================================================================
                        GotoNextClassic(botParam.Map);                 //================================================================
                        break;
                    case 9:                                         //одно из заданий выполнено
                        botwindow.Hero(botParam.Scout);
                        GotoMemorialClassic(botParam.Map);             //================================================================
                        PressMeMorial();
                        if (!dialog.isDialogClassic()) PressMeMorial();
                        break;
                    case 10:                                        //задания не выполнены
                        if (!isBattleModeClassic())
                        { 
                            BattleModeOnDem(); 
                            ActivationPetClassic();
                            botwindow.Hero(botParam.Scout);
                        }
                        //botParam.HowManyCyclesToSkip = 1;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(5000);
            }
        }


        #endregion ===============================================================================



    }
}
