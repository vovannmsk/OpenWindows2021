using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using GEBot.Data;
using System.Diagnostics;

namespace OpenGEWindows
{
    public abstract class Server

    {
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(UIntPtr myhWnd, int myhwndoptional, int xx, int yy, int cxx, int cyy, uint flagus); // Перемещает окно в заданные координаты с заданным размером

        [DllImport("user32.dll")]
        static extern bool PostMessage(UIntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern UIntPtr FindWindowEx(UIntPtr hwndParent, UIntPtr hwndChildAfter, string className, string windowName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(UIntPtr hWnd, int nCmdShow);  //раскрывает окно, если оно было скрыто в трей

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(UIntPtr hWnd); // Перемещает окно в верхний список Z порядка


        #region статические переменные

        /// <summary>
        /// true, если окно загружено на другом компе
        /// </summary>
        public static bool AccountBusy;

        /// <summary>
        /// true, если сейчас происходит загрузка нового окна ГЭ. Переменная нужна, чтобы не грузить одновременно два окна для BH
        /// </summary>
        public static bool isLoadedGEBH;

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
        protected iPointColor pointisOpenTopMenu61;
        protected iPointColor pointisOpenTopMenu62;
        protected iPointColor pointisOpenTopMenu81;
        protected iPointColor pointisOpenTopMenu82;
        protected iPointColor pointisOpenTopMenu91;
        protected iPointColor pointisOpenTopMenu92;
        protected iPointColor pointisOpenTopMenu121;
        protected iPointColor pointisOpenTopMenu122;
        protected iPointColor pointisOpenTopMenu131;
        protected iPointColor pointisOpenTopMenu132;
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

        //        protected uint[] arrayOfColors = new uint[17] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 2104613 };
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
        //Item Steroid = new Item(36, 211, 11690052);
        Item Steroid = new Item(36, 211, 11296065);
        Item Principle = new Item(37, 210, 3226091);
        Item Triumph = new Item(35, 209, 47612);
        Item SteroidLeftPanel = new Item(31, 241, 11690052);
        Item PrincipleLeftPanel = new Item(32, 272, 3226091);
        Item TriumphLeftPanel = new Item(31, 304, 47612);
        /// <summary>
        /// розовые крылья
        /// </summary>
        Item PinkWings = new Item(36, 211, 4870905);
        /// <summary>
        /// коробка с розовыми крыльями
        /// </summary>
        Item PinkWingsBox = new Item(36, 211, 5144242);

        protected iPointColor pointisOpenInventory1;
        protected iPointColor pointisOpenInventory2;



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

        #region No Window

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
        /// Перемещает окно с ботом в заданные координаты. Если окно есть, то result = true, а если вылетело окно, то result = false. не учитываются ширина и высота окна
        /// </summary>
        /// <returns></returns>
        public bool isHwnd()
        {
            return SetPosition();
        }

        /// <summary>
        /// активируем окно
        /// </summary>
        private void ActiveWindow()
        {
            ShowWindow(botParam.Hwnd, 9);                                       // Разворачивает окно если свернуто  было 9
            SetForegroundWindow(botParam.Hwnd);                                 // Перемещает окно в верхний список Z порядка     
            //BringWindowToTop(databot.hwnd);                                    // Делает окно активным и Перемещает окно в верхний список Z порядка     

            SetPosition();                                                      //перемещаем окно в заданные для него координаты
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
            bool result = isHwnd();   //Перемещает в заданные координаты. Если окно есть, то result=true, а если вылетело окно, то result=false.
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

        /// <summary>
        /// проверяем, выскочило ли сообщение о несовместимости версии SafeIPs.dll
        /// </summary>
        /// <returns></returns>
        public bool isSafeIP()
        {
            return (pointSafeIP1.isColor() && pointSafeIP2.isColor());
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
        /// если первый раз загружается стим на этом компе, то надо нажать "соглашаюсь"
        /// </summary>
        /// <returns>true, если первый раз загружается стим на этом компе</returns>
        protected bool isNewSteam()
        {
            return (pointisNewSteam1.isColor() && pointisNewSteam2.isColor());
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

        protected string GetLogin()
        {
            //string result = botParam.Login;
            //if (globalParam.Infinity)
            //{
            //    //если ходим в Инфинити вместо обычного ботоводства, 
            //    //то здесь надо написать выбор логина из списка
            //    result = botParam.Logins[botParam.NumberOfInfinity];   //получили логин
            //}
            //return result;
            return botParam.Logins[botParam.NumberOfInfinity];
        }

        protected string GetPassword()
        {
            //string result = botParam.Password;
            //if (globalParam.Infinity)
            //{
            //    //если ходим в Инфинити вместо обычного ботоводства, 
            //    //то здесь надо написать выбор пароля из списка
            //    result = botParam.Passwords[botParam.NumberOfInfinity];   //получили пароль
            //}
            //return result;
            return botParam.Passwords[botParam.NumberOfInfinity];
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
            if (botParam.NumberOfInfinity == 0)  pointSteamSavePassword.PressMouseL(); //если обычный режим бота
        }
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
            if (result >= botParam.LengthOfList) result = 0;
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
            int result = globalParam.Infinity;
            if (result >= 424) result = 0;  //если дошли до последнего подготовленного аккаунта, то идём в начала списка
            //в качестве альтернативы тут можно сделать присвоение FAlse какому-нибудь глоб параметру, чтобы остановить общий цикл
            botParam.NumberOfInfinity = result;
            globalParam.Infinity = result + 1;

            CloseSandboxieBH();
            MoveMouseDown();
        }

        /// <summary>
        /// закрываем программы в конкретной песочнице для БХ
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


        public abstract void runClientSteamBH();
        public abstract void runClient();
        public abstract UIntPtr FindWindowGE();
        //public abstract UIntPtr FindWindowSteam();
        public abstract bool FindWindowSteamBool();
        public abstract void OrangeButton();
        //public abstract bool isActive();



        #endregion

        #region Logout

        /// <summary>
        /// вводим логин и пароль в соответствующие поля
        /// </summary>
        protected void EnterLoginAndPasword()
        {
            iPoint pointPassword = new Point(510 - 5 + botParam.X, 355 - 5 + botParam.Y);    //  505, 350
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
            iPoint pointButtonConnect = new Point(595 - 5 + botParam.X, 485 - 5 + botParam.Y);    // кнопка "Connect" в логауте 
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

            iPointColor point5050 = new PointColor(50 - 5 + botParam.X, 50 - 5 + botParam.Y, 7800000, 5);  //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            iPoint pointButtonOk = new Point(525 - 5 + botParam.X, 410 - 5 + botParam.Y);    // кнопка Ok в логауте
            iPoint pointButtonOk2 = new Point(525 - 5 + botParam.X, 445 - 5 + botParam.Y);    // кнопка Ok в логауте

            uint Tek_Color1;
            uint Test_Color = 0;
            bool ColorBOOL = true;
            uint currentColor = 0;
            const int MAX_NUMBER_ITERATION = 4;  //максимальное количество итераций

            bool aa = true;

            Test_Color = point5050.GetPixelColor();       //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
            Tek_Color1 = Test_Color;

            ColorBOOL = (Test_Color == Tek_Color1);
            int counter = 0; //счетчик

            pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
            Pause(500);
            pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
            Pause(500);

            bool IsServerSelected = serverSelection();          //выбираем из списка свой сервер
            if (!IsServerSelected) return false; //если не смогли выбрать сервер, то выход из метода с результатом false

            WriteToLogFileBH("дошли до while");
            while ((aa | (ColorBOOL)) & (counter < MAX_NUMBER_ITERATION))
            {
                counter++;  //счетчик

                Tek_Color1 = point5050.GetPixelColor();
                ColorBOOL = (Test_Color == Tek_Color1);

                WriteToLogFileBH("нажимаем на кнопку connect");

                //pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
                //Pause(500);
                //pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
                //Pause(500);
                //server.serverSelection();          //выбираем из списка свой сервер
                if (PressConnectButton())
                {
                    Pause(1500);
                }
                else
                    return false;

                //pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
                //Pause(500);
                //pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
                //Pause(500);

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

                    EnterLoginAndPasword();
                }
                else
                {
                    aa = false;
                }

            }

            bool result = true;
            Pause(5000);
            currentColor = point5050.GetPixelColor();
            if (currentColor == Test_Color)      //проверка входа в казарму. 
            {
                //тыкнуть в Quit 
                //PressMouseL(600, 530);          //если не вошли в казарму, то значит зависли и жмем кнопку Quit
                //PressMouseL(600, 530);
                result = false;
            }
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
            //WriteToLogFileBH("выбираем сервер из списка серверов начало");
            int count = 0;
            while ((!IsServerSelection())&&(count<5))
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
            //нажимаем на кнопки, которые могут появиться из-за сбоев входа в игру
            new Point(525 - 5 + xx, 410 - 5 + yy).PressMouseL();    // кнопка Ok в логауте
            Pause(500);
            new Point(525 - 5 + xx, 445 - 5 + yy).PressMouseL();    // кнопка Ok в логауте
            Pause(500);

            serverSelection();
            PressConnectButton();
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

        #region Pet (перенесено в другой класс Pet)

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
            return ((pointisActivePet1.isColor() && pointisActivePet2.isColor()) || (pointisActivePet3.isColor() && pointisActivePet4.isColor()));
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
            return (pointisSummonPet1.isColor() && pointisSummonPet2.isColor());
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
            return ( pointIsCurrentChannel1.isColor() || pointIsCurrentChannel3.isColor() ) && ( pointIsCurrentChannel2.isColor() || pointIsCurrentChannel4.isColor() );
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
                    result = (pointisOpenTopMenu21.isColor() && pointisOpenTopMenu22.isColor());
                    break;
                case 6:
                    result = (pointisOpenTopMenu61.isColor() && pointisOpenTopMenu62.isColor());
                    break;
                case 8:
                    result = (pointisOpenTopMenu81.isColor() && pointisOpenTopMenu82.isColor());
                    break;
                case 9:
                    result = (pointisOpenTopMenu91.isColor() && pointisOpenTopMenu92.isColor());
                    break;
                case 12:
                    result = (pointisOpenTopMenu121.isColor() && pointisOpenTopMenu122.isColor());
                    break;
                case 13:
                    result = (pointisOpenTopMenu131.isColor() && pointisOpenTopMenu132.isColor());
                    break;
                default:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Открыть городской телепорт (Alt + F3) без проверок и while (для паттерна Состояние)  StateGT
        /// </summary>
        public void OpenTownTeleportForState()
        {
            TopMenu(6, 3);
            Pause(1000);
        }

        /// <summary>
        /// Открыть карту местности (Alt + Z) для группы классов StateGT (паттерн Состояние)
        /// </summary>
        public void OpenMapForState()
        {
            TopMenu(6, 2);
            Pause(1000);
        }

        /// <summary>
        /// Выгружаем окно через верхнее меню 
        /// </summary>
        public void GoToEnd()
        {
            systemMenu(7);
        }

        /// <summary>
        /// Выгружаем окно через верхнее меню 
        /// </summary>
        public void Logout()
        {
            systemMenu(6);
        }

        /// <summary>
        /// Идем в казармы через верхнее меню 
        /// </summary>
        public void GotoBarack()
        {
            systemMenu(4);
        }

        /// <summary>
        /// Идем в казармы через верхнее меню 
        /// </summary>
        /// <param name="status"> false, если без проверки открытия системного меню </param>
        public void GotoBarack(bool status)
        {
            systemMenu(4, status);
        }

        /// <summary>
        /// переход по системному меню 
        /// </summary>
        /// <param name="number"> номер пункта меню </param>
        public void systemMenu(int number)
        {
            //if (!isOpenTopMenu(13))
                TopMenu(13);
            Pause(1000);
            iPoint pointCurrentMenu = new Point(685 - 5 + xx, 288 - 5 + (number - 1) * 30 + yy);
            pointCurrentMenu.PressMouse();
        }

        /// <summary>
        /// переход по системному меню (для БХ)
        /// </summary>
        /// <param name="number"> номер пункта меню </param>
        public void systemMenu(int number, bool status)
        {
            if(!isOpenTopMenu(13))  TopMenu(13, status);       //если не открыто системное меню, то открыть
            //Pause(500);
            iPoint pointCurrentMenu = new Point(685 - 5 + xx, 288 - 5 + (number - 1) * 30 + yy);  //строчка в меню
            if (!isRouletteBH() && (isOpenTopMenu(13))) pointCurrentMenu.PressMouse();   //нажимаем на указанный пункт меню
        }

        /// <summary>
        /// телепортируемся в город продажи по Alt+W (улетаем из БХ или другого города)
        /// </summary>
        public void TeleportAltW_BH()
        {
            int teleport = botwindow.getNomerTeleport();

            if (teleport >= 14)
            {
                teleport = 1;              //если номер телепорта больше 14 (катовия и отит), то летим в Ребольдо
            }

            iPoint pointTeleportToTownAltW = new Point(800 + xx, 517 + yy + (teleport - 1) * 17);

            TopMenu(6, 1);
            Pause(1000);
            pointTeleportToTownAltW.PressMouse();           //было два нажатия левой, решил попробовать RRL
            //            botwindow.Pause(2000);
        }

        /// <summary>
        /// нажмает на выбранный раздел верхнего меню (используется для БХ)
        /// </summary>
        /// <param name="numberOfThePartitionMenu"></param>
        /// <param name="status">если false, то не проверяется сработало ли нажатие</param>
        public void TopMenu(int numberOfThePartitionMenu, bool status)
        {
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
            } while ((!isOpenTopMenu(numberOfThePartitionMenu)) && (status));
        }

        /// <summary>
        /// нажать на выбранный раздел верхнего меню, а далее на пункт раскрывшегося списка
        /// </summary>
        /// <param name="numberOfThePartitionMenu">номер раздела верхнего меню п/п</param>
        /// <param name="punkt">пункт меню. номер п/п</param>
        public void TopMenu(int numberOfThePartitionMenu, int punkt, bool status)
        {
            int[] numberOfPunkt = { 0, 8, 4, 2, 0, 3, 2, 6, 9, 0, 0, 0, 0 };  //количество пунктов меню в соответствующем разделе
            int[] PunktX = { 0, 336, 368, 401, 0, 462, 510, 561, 578, 0, 0, 0, 0 };    //координата X пунктов меню
            int[] FirstPunktOfMenuKoordY = { 0, 83, 83, 83, 0, 97, 97, 97, 83, 0, 0, 0, 0 }; //координата Y первого пункта меню

            if (punkt <= numberOfPunkt[numberOfThePartitionMenu - 1])
            {
                int y = FirstPunktOfMenuKoordY[numberOfThePartitionMenu - 1] + 25 * (punkt - 1);

                TopMenu(numberOfThePartitionMenu, status);   //сначала открываем раздел верхнего меню (1-14)
                Pause(500);

                int x = PunktX[numberOfThePartitionMenu - 1];
                iPoint pointMenu = new Point(x - 5 + xx, y - 5 + yy);
                pointMenu.PressMouseL();  //выбираем конкретный пункт подменю (в раскрывающемся списке)
            }
        }


        /// <summary>
        /// открываем список телепоров, проверяем, есть ли телепорт в Катовию и летим туда
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
        /// проверяем, является ли третий телепорт телепортом в Катовию
        /// </summary>
        /// <returns>true, если является</returns>
        public bool isKatoviaTeleport()
        {
            return new PointColor(348 - 5 + xx, 205 - 5 + yy, 16711422, 0).isColor() 
                && new PointColor(348 - 5 + xx, 209 - 5 + yy, 16711422, 0).isColor() 
                && new PointColor(348 - 5 + xx, 213 - 5 + yy, 16711422, 0).isColor();
        }

        /// <summary>
        /// вызываем телепорт через верхнее меню и телепортируемся по указанному номеру телепорта (используется для БХ)
        /// </summary>
        /// <param name="NumberOfLine"></param>
        /// <param name="status">если false, то не проверяется сработало ли нажатие</param>
        public void Teleport(int NumberOfLine, bool status)
        {
            TopMenu(12, status);                     // Click Teleport menu

            Point pointTeleportNumbertLine = new Point(405 - 5 + xx, 180 - 5 + (NumberOfLine - 1) * 15 + yy);    //    тыкаем в указанную строчку телепорта 

            pointTeleportNumbertLine.DoubleClickL();   // Указанная строка в списке телепортов
            Pause(500);

            pointTeleportExecute.PressMouseL();        // Click on button Execute in Teleport menu
        }

        public abstract void Teleport(int numberOfLine);
        public abstract void TopMenu(int numberOfThePartitionMenu);
        public abstract void TopMenu(int numberOfThePartitionMenu, int punkt);
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
        /// функция проверяет, убит ли хоть один герой из пати (проверка проходит на карте)
        /// </summary>
        /// <returns></returns>
        public bool isKillHero()
        {
            return (pointisKillHero1.isColor() || pointisKillHero2.isColor() || pointisKillHero3.isColor());
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
        /// проверяем, включён ли боевой режим
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
                pointBattleMode.PressMouse();  // Кликаю на кнопку "боевой режим"
                Pause(500);
            }
        }

        #endregion

        #region inTown

        /// <summary>
        /// нажимаем на голову Lucia 
        /// </summary>
        public void PressHeadOfLucia()
        {
            //new Point(471 - 5 + xx, 404 - 5 + yy).PressMouseL();
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
            return new PointColor(361 - 5 + xx, 82 - 5 + yy, 8549475, 0).isColor() && new PointColor(362 - 5 + xx, 82 - 5 + yy, 8549475, 0).isColor();
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

            uint color1 = new PointColor(29 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем
            uint color2 = new PointColor(30 - 5 + xx, 697 - 5 + yy, 0, 0).GetPixelColor() / 1000;                 // проверяем номер цвета в контрольной точке и округляем

            return ((this.arrayOfColorsIsTown1.Contains(color1)) && (this.arrayOfColorsIsTown2.Contains(color2)));                // проверяем, есть ли цвет контрольной точки в массивах цветов
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
            return (pointisAlchemy1.isColor() && pointisAlchemy2.isColor());
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
            pointTeamSelection1.PressMouse();   // Нажимаем кнопку вызова списка групп
            new Point(70 - 5 + xx, 355 - 5 + (numberTeam - 1) * 15 + yy).PressMouseL();  // выбираем нужную группу персов 
            pointTeamSelection3.PressMouseL();  // Нажимаем кнопку выбора группы (Select Team) 
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
            return (pointisBarack1.isColor() && pointisBarack2.isColor()) || (pointisBarack3.isColor() && pointisBarack4.isColor());
        }

        /// <summary>
        /// проверяем, в бараках ли бот (на стадии выбора группы)
        /// </summary>
        /// <returns> true, если бот в бараках на стадии выбора группы  </returns>
        public bool isBarackTeamSelection()
        {
            return (pointisBarackTeamSelection1.isColor() && pointisBarackTeamSelection2.isColor());
        }

        /// <summary>
        /// начать с выхода в город (нажать на кнопку "начать с нового места")
        /// </summary>
        public void NewPlace()
        {
            iPoint pointNewPlace = new Point(54 - 5 + xx, 685 - 5 + yy);
            pointNewPlace.DoubleClickL();
        }

        /// <summary>
        /// начинаем со старого места (из барака)
        /// </summary>
        public void barackLastPoint()
        {
            pointLastPoint.PressMouseL();
        }

        /// <summary>
        /// проверяем, попадём ли мы в BH, если в бараке начинаем со старого места
        /// </summary>
        public bool isBarackLastPoint()
        {
            pointLastPoint.PressMouseR();    // наводим мышку на кнопку Last Point в бараке
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
            return new PointColor(522 - 5 + xx, 427 - 5 + yy, 7727344, 0).isColor() 
                && new PointColor(522 - 5 + xx, 428 - 5 + yy, 7727344, 0).isColor();
        }

        /// <summary>
        /// создание мушкетера в казарме с заданным порядковым номером
        /// </summary>
        /// <param name="Number">номер мушкетера</param>
        public bool CreateMuskInBarack(int Number)
        {
            PressCreateButton();

            if (isBarackFull()) return false;

            pointMenuSelectTypeHeroes.PressMouse();
            Pause(1500);
            pointSelectTypeHeroes.PressMouseL();
            Pause(1500);
            pointNameOfHeroes.PressMouseL();
            Pause(1500);

            string NameMusk = "Musk" + Number;
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
            return ((pointIsPlus41.isColor() && pointIsPlus42.isColor()) || (pointIsPlus43.isColor() && pointIsPlus44.isColor()));  //либо одни две точки либо другие две
        }

        /// <summary>
        /// нажимаем на кнопку Max (добавляем Shiny Crystal для заточки на +5 или +6)
        /// </summary>
        public void AddShinyCrystall()
        {
            pointAddShinyCrystall.PressMouseL();
        }

        /// <summary>
        /// проверяем, прибавились ли шайники к заточке на +6 (проверка по голубой полоске)
        /// </summary>
        /// <returns></returns>
        public bool isAddShinyCrystall()
        {
            return (pointIsAddShinyCrystall1.isColor() && pointIsAddShinyCrystall2.isColor());
        }

        /// <summary>
        /// проверяем, находимся ли в магазине у Иды (заточка)
        /// </summary>
        /// <returns></returns>
        public bool isIda()
        {
            //bool ff = pointIsIda1.isColor();
            //bool gg = pointIsIda2.isColor();
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
            return (pointisWeapon1.isColor() && pointisWeapon2.isColor());
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
            return (pointisHP1.isColor() || pointisHP2.isColor() || pointisHP3.isColor() || pointisHP4.isColor());
        }

        /// <summary>
        /// метод возвращает параметр, отвечающий за тип чиповки оружия
        /// </summary>
        /// <returns></returns>
        public int TypeOfNintendo()
        { return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Чиповка.txt")); }

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
            return (
                    (pointisWild41.isColor() && pointisWild42.isColor()) ||
                    (pointisWild51.isColor() && pointisWild52.isColor()) || 
                    (pointisWild61.isColor() && pointisWild62.isColor()) ||
                    (pointisWild71.isColor() && pointisWild72.isColor())
                   );
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
            return (
                    (pointisUndead41.isColor() && pointisUndead42.isColor()) ||
                    (pointisUndead51.isColor() && pointisUndead52.isColor()) ||
                    (pointisUndead61.isColor() && pointisUndead62.isColor()) ||
                    (pointisUndead71.isColor() && pointisUndead72.isColor())
                    );
        }

        /// <summary>
        /// проверяем, зачиповалось ли оружие на атаку по Lifeless
        /// </summary>
        /// <returns></returns>
        public bool isLifeless()
        {
            return (
                    (pointisLifeless41.isColor() && pointisLifeless42.isColor()) ||
                    (pointisLifeless51.isColor() && pointisLifeless52.isColor()) ||
                    (pointisLifeless61.isColor() && pointisLifeless62.isColor()) ||
                    (pointisLifeless71.isColor() && pointisLifeless72.isColor())
                    );
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
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isWild())) result = true;
                    //if ((isAtk40() || isAtk39()) && (isAtkSpeed30() || isAtkSpeed29())) result = true;
                    break;
                case 3:
                    if ((isAtk40() || isAtk39() || isAtk38() || isAtk37()) && (isAtkSpeed30() || isAtkSpeed29() || isAtkSpeed28() || isAtkSpeed27()) && (isLifeless())) result = true;
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
                        (isHuman() || isWild() || isLifeless() || isUndead() || isDemon()))  result = true;
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
        public  void OpenBookmarkSell()
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
                ff = ! ff;
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
        public abstract bool FindWindowGEforBHBool();

        /// <summary>
        /// подбор дропа в миссии Инфинити
        /// </summary>
        public void GetDrop()
        {
            new Point(188 - 5 + xx, 526 - 5 + yy).PressMouseL();    //боевой режим, чтобы боты остановились
            //Pause(1000);
            new Point(123 - 5 + xx, 526 - 5 + yy).PressMouseL();    //нажимаем на сундук (иконка подбора)
            Pause(200);
            new Point(760 - 5 + xx, 330 - 5 + yy).PressMouseL();    //нажимаем в случайную точку в стороне, чтобы начать подбор
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
            StreamWriter writer = new StreamWriter(globalParam.DirectoryOfMyProgram + "\\BH.log", true);
            string timeNow = DateTime.Now.ToString("dd MMMM yyyy | HH:mm:ss | ");

            writer.WriteLine(timeNow + botwindow.getNumberWindow().ToString() + " " + strLog);
            writer.Close();
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
             Point pointSabreBottonBH = new Point(92 - 5 + xx, 525 - 5 + yy);         //нажимаем на кнопку с саблей на боевой панели (соответствует нажатию Ctrl)
             Point pointFightBH = new Point(x - 30 + xx, y - 30 + yy);                //нажимаем конкретную точку, куда надо бежать и бить всех по пути

             pointSabreBottonBH.PressMouseL();
             pointFightBH.PressMouseL();
             Pause(t * 1000);
        }

        /// <summary>
        /// проверка миссии по цвету контрольной точки
        /// </summary>
        /// <returns> номер цвета </returns>
        public abstract uint ColorOfMissionBH();                                                                    //может быть перенести из синга сюда

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

            uint color = new PointColor(700 - 30 + xx, 500 - 30 + yy, 0, 0).GetPixelColor();                 // проверяем номер цвета в контрольной точке
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
            
            uint color = new PointColor(700 - 30 + xx, 500 - 30 + yy, 0, 0).GetPixelColor();                 // проверяем номер цвета в контрольной точке
            //color = color / 1000;
            WriteToLogFileBH("неизвестная миссия!!!, цвет " + color);

            //FightToPoint(1000, 100, 0);    //бежим с атакой в сторону (вдруг зацепим Босса)

            //сохраняем скриншот
            string timeNow = DateTime.Now.ToString(" ddMMMMyyyy HH-mm-ss");
            Size screenSz = Screen.PrimaryScreen.Bounds.Size;
            Bitmap screenshot = new Bitmap(screenSz.Width, screenSz.Height);
            Graphics gr = Graphics.FromImage(screenshot);
            gr.CopyFromScreen(0, 0, 0, 0, screenSz);
            string filepath = globalParam.DirectoryOfMyProgram + "\\ScreenShot"+ timeNow + ".bmp" ;
            screenshot.Save(filepath);    
        }

        /// <summary>
        /// проверяем контрольную точку в миссии БХ и находим, в какой миссии мы находимся 
        /// </summary>
        /// <returns>номер миссии по порядку</returns>
        public int NumberOfMissionBH()
        {
            uint color = new PointColor(700 - 30 + xx, 500 - 30 + yy, 0, 0).GetPixelColor();                // проверяем номер цвета в контрольной точке
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
//            return (isTown() && !pointisBH1.isColor());
            return (!pointisBH1.isColor());
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
            Point pointEnd   = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }

        /// <summary>
        /// поворот, чтобы смотреть более вертикально (сверху вниз)
        /// </summary>
        public void TurnDown()
        {
            Point pointBegin = new Point(560 - 30 + xx, 430 - 30 - 1 + yy);
            Point pointEnd   = new Point(560 - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }


        /// <summary>
        /// поворот влево (против часовой стрелки)
        /// </summary>
        /// <returns></returns>
        public void TurnL(int gradus)
        {
            Point pointBegin = new Point(560 + gradus - 30 + xx, 430 - 30 + yy);
            Point pointEnd   = new Point(560          - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
        }

        /// <summary>
        /// поворот вправо (по часовой стрелке)
        /// </summary>
        /// <returns></returns>
        public void TurnR(int gradus)
        {
            Point pointBegin = new Point(560 - gradus - 30 + xx, 430 - 30 + yy);
            Point pointEnd   = new Point(560          - 30 + xx, 430 - 30 + yy);
            pointBegin.Turn(pointEnd);
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

            while (new PointColor (937 - 5 + xx, 175 - 5 + yy, 15986174, 0).isColor() )
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

            int ii = 0;
            int jj = 0;
            bool result = FindItem(StanceBook, out ii, out jj);

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

        #region Ферма

        /// <summary>
        /// поднимаем камеру в верхнюю точку
        /// </summary>
        public void MaxHeight(int n)
        {
            new Point(555 - 5 + xx, 430 - 5 + yy).Move();
            for (int i = 1; i <= n; i++)
            {
                new Point(555 - 5 + xx, 430 - 5 + yy).PressMouseWheelUp();
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


        #endregion

        #region Работа с инвентарем и CASH-инвентарем


        #region Обычный инвентарь

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

            //for (int i = 1; i <= 5; i++)         //строки в инвентаре
            //{
            //    for (int j = 1; j <= 8; j++)        // столбцы в инвентаре
            //    {
            //        if (!result)         //если вещь уже найдена, то дальше не ищем
            //        {
            //            if (CheckThingInCell(i, j, thing))
            //            {
            //                UseThingInCell(i, j);
            //                new Point(1000 - 5 + xx, 500 - 5 + yy).Move();   //убираем мышь в сторону
            //                result = true;
            //                Pause(500);
            //            }
            //        }
            //    }
            //}

            int i, j;
            if (FindItem(thing, out i, out j))
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
        protected bool isOpenInventory()
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
            if (!isOpenInventory()) TopMenu(8, 1);
            Pause(1000);

            new Point(713 - 5 + xx + 61 * (numberOfbookmark - 1), 152 - 5 + yy).PressMouseL();

            //switch (numberOfbookmark)
            //{
            //    case 1:
            //        new Point(713 - 5 + xx, 152 - 5 + yy).PressMouseL();     //первая - Equip
            //        break;
            //    case 2:
            //        new Point(782 - 5 + xx, 152 - 5 + yy).PressMouseL();     //вторая - Expand
            //        break;
            //    case 3:
            //        new Point(843 - 5 + xx, 152 - 5 + yy).PressMouseL();     //третья - Misk
            //        break;
            //    case 4:
            //        new Point(902 - 5 + xx, 152 - 5 + yy).PressMouseL();     //четвертая - Quest
            //        break;
            //    case 5:
            //        new Point(871 - 5 + xx, 133 - 5 + yy).PressMouseL();     //пятая - костюмы
            //        break;
            //}
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

            int i, j;

            if (FindItem(thing, out i, out j))
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
            iPoint pointSlot  = new Point(244 - 5 + xx + (Slot - 1) * 255, 700 - 5 + yy);   //конечная точка перемещения

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
            int i, j;
            
            if (FindItem(thing, out i, out j))
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
            if (new PointColor(559 - 5 + xx, 420 - 5 + yy, 7727344, 0).isColor() ||
                new PointColor(559 - 5 + xx, 426 - 5 + yy, 7727344, 0).isColor())  //проверка, есть ли запрос на экране
                if (answer)
                    new Point(465 - 5 + xx, 422 - 5 + yy).PressMouseL();   //да
                else
                    new Point(565 - 5 + xx, 422 - 5 + yy).PressMouseL();   //нет
        }

        /// <summary>
        /// открыт ли специнвентарь
        /// </summary>
        /// <returns></returns>
        public bool isOpenSpecInventory()
        {
            return new PointColor(109 - 5 + xx, 116 - 5 + yy, 8036794, 0).isColor() &&
                   new PointColor(118 - 5 + xx, 116 - 5 + yy, 8036794, 0).isColor();
        }

        /// <summary>
        /// открыть специнвентарь на указанной закладке
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
        /// открываем закладку с указанным номером в уже открытом спец инвентаре:
        /// первая - Equip;
        /// вторая - Expand;
        /// третья - Misk;
        /// четвертая - Costume.
        /// </summary>
        /// <param name="n">номер закладки</param>
        public void SpecInventoryBookmark(int n)
        {
            new Point(48 - 5 + xx + (n - 1) * 60, 181 - 5 + yy).PressMouseL();     //закладка инвентаря с номером n
        }

        /// <summary>
        /// применяем журнал в уже открытом специнвентаре 
        /// </summary>
        public void ApplyingJournal()
        {
            SpecInventoryBookmark(3);

            PuttingItem(Journal);     

            AnswerYesOrNo(true);
        }

        /// <summary>
        /// открыть специнвентарь (без проверок)
        /// </summary>
        public void OpenSpecInventory()
        {
            TopMenu(8, 2);
        }

        /// <summary>
        /// применяем вещь в специнвентаре
        /// </summary>
        protected void PuttingItemOld(Item item)
        {
            bool result = false;   //объект в кармане пока не найден
            for (int i = 1; i <= 7; i++)         //строки в инвентаре
            {
                for (int j = 1; j <= 7; j++)        // столбцы в инвентаре
                {
                    if (!result)
                    {
                        if (new PointColor(item.x - 5 + xx + (j - 1) * 39, item.y - 5 + yy + (i - 1) * 38, item.color, 0).isColor())
                        {
                            new Point(35 - 5 + xx + (j - 1) * 39, 209 - 5 + yy + (i - 1) * 38).DoubleClickL();
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
            Point point;
            
            if (FindItemFromSpecInventory(item, 0, out point))
            {
                point.DoubleClickL();
                new Point(1500, 800).Move();   //убираем мышь в сторону
                Pause(500);
                return true;
            }
            else
            {
                return false;
            }

              
        } // НЕ проверено

        /// <summary>
        /// Находим конкретную вещь (Item) в сдвинутом спец инвентаре в текущей закладке
        /// </summary>
        /// <param name="item">искомая вещь</param>
        /// <param name="sdvig">величина сдвига специнвентаря по оси Х</param>
        /// <param name="point">если вещь найдена, то возвращаем точку с координатами вещи</param>
        /// <returns>если вещь найдена, то возвращаем true</returns>
        protected bool FindItemFromSpecInventory(Item item, int sdvig, out Point point)
        {
            MoveCursorOfMouse();
            point = new Point(item.x - 5 + xx, item.y - 5 + yy);
            bool result = false;   //объект в кармане пока не найден

            for (int i = 1; i <= 7; i++)         //строки в инвентаре
            {
                for (int j = 1; j <= 7; j++)        // столбцы в инвентаре
                {
                    int xxx = item.x - 5 + xx + (j - 1) * 39 + sdvig;
                    int yyy = item.y - 5 + yy + (i - 1) * 38;
                    if (!result)
                    {
                        if (new PointColor(xxx, yyy, item.color, 0).isColor())
                        {
                            result = true;
                            point = new Point(xxx, yyy);
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

            Point pointBegin = new Point(0, 0);
            result = FindItemFromSpecInventory(item, sdvig, out pointBegin);
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
        protected void MoveSpecInventory(int sdvig)
        {
            new Point(165 - 5 + xx, 130 - 5 + yy).Drag(new Point(165 + sdvig - 5 + xx, 130 - 5 + yy));
        }

        /// <summary>
        /// перемещаем курсор мыши прочь от специнвентаря и инвентаря
        /// </summary>
        protected void MoveCursorOfMouse()
        {
            new Point(400 - 5 + xx, 400 - 5 + yy).Move();
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

            MoveItemOnLeftPanelFromSpecInventory(Steroid, sdvig, 5);
            
            MoveItemOnLeftPanelFromSpecInventory(Principle, sdvig, 6);
            
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
            return isItemOnLeftPanel(SteroidLeftPanel) &&
                    isItemOnLeftPanel(PrincipleLeftPanel) &&
                    isItemOnLeftPanel(TriumphLeftPanel);

            //return  new PointColor(31 - 5 + xx, 241 - 5 + yy, 11690052, 0).isColor() &&
            //        new PointColor(32 - 5 + xx, 272 - 5 + yy, 3226091, 0).isColor() &&
            //        new PointColor(31 - 5 + xx, 304 - 5 + yy, 47612, 0).isColor();
        }

        #endregion

        #region Detail Info (сведения о персонаже и  просмотр экипировки)

        /// <summary>
        /// открыт ли Detail Info для первого персонажа?
        /// </summary>
        /// <returns>true, если открыт</returns>
        public bool isOpenDetailInfo()
        {
            return new PointColor(62 - 5 + xx, 345 - 5 + yy, 14534000, 3).isColor() &&
                   new PointColor(62 - 5 + xx, 346 - 5 + yy, 14534000, 3).isColor();
        }

        /// <summary>
        /// открыть Detail Info у первого героя
        /// </summary>
        public void OpenDetailInfo()
        {
            if (!isOpenDetailInfo())
                new Point(220 - 5 + xx, 671 - 5 + yy).PressMouseL();
        }

        /// <summary>
        /// экипированы розовые крылья у первого героя
        /// </summary>
        /// <returns>true, если экипирпованы</returns>
        public bool isWearingPinkWings()
        {
            return new PointColor(163 - 5 + xx, 363 - 5 + yy, 1579516, 0).isColor();
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
    }
}
