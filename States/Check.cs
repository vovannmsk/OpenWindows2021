using System;
using System.Windows.Forms;
using OpenGEWindows;
using System.Threading;
using GEBot.Data;

namespace States
{
    public class Check
    {   
        /// <summary>
        /// если никакой Steam не грузится, то переменная = 0. Значит можно грузить новый Стим.
        /// </summary>
        private static int IsItAlreadyPossibleToUploadNewSteam = 0;

        /// <summary>
        /// если никакой бот не грузится, то переменная = 0. Если грузится, то переменная равна номеру окна (numberOfWindow)
        /// </summary>
        private static int IsItAlreadyPossibleToUploadNewWindow = 0;

        /// <summary>
        /// если равна 1, то надо идти в барак
        /// </summary>
        private int GoBarack;
        /// <summary>
        /// номер проблемы на предыдущем цикле
        /// </summary>
        private int prevProblem;
        private int prevPrevProblem;
        private int numberOfWindow;
        private botWindow botwindow;
        private Server server;
        private Market market;
        private KatoviaMarket kMarket;
        private Pet pet;
        private Dialog dialog;
        private Otit otit;
        private MM mm;
        private BHDialog BHdialog;
        private GlobalParam globalParam;
        private DriversOfState driver;
        private BotParam botParam;
        //private System.Windows.Forms.Timer NowTimer = new System.Windows.Forms.Timer();
        private bool SteamLoaded;
        /// <summary>
        /// текущее дата и время
        /// </summary>
        private DateTime dateNow;
        /// <summary>
        /// время запуска Стим для этого окна
        /// </summary>
        private DateTime dateSteam;
        /// <summary>
        /// номер состояния бота (место, где бот сейчас находится)
        /// </summary>
        private int numberOfState;
        /// <summary>
        /// выполнено ли задание OldMan для получения отита
        /// </summary>
        private bool taskCompleted;
        /// <summary>
        /// задание получено
        /// </summary>
        private bool gotTask;
        /// <summary>
        /// направление движения (1 - вправо, -1 - влево)
        /// </summary>
        private int DirectionOfMovement;
        /// <summary>
        /// информация о том, какие герои в нашей команде: Hero[1] - первый герой, Hero[2] - второй, Hero[3] - третий
        /// 0 - герой не определён, 1 - мушкетёр или Берка(Флинт), 2 - Бернелли (Superior Blaster), 3 - М.Лорч, 4 - Джайна
        /// 5 - молодой Барель, 6 - C.Daria, 7 - Том, 8 - Moon
        /// </summary>
        private int[] Hero;

        private bool isActiveServer;

        public bool IsActiveServer { get => isActiveServer; }
        /// <summary>
        /// номер состояния бота (место, где бот сейчас находится)
        /// </summary>
        public int NumberOfState { get => numberOfState; set => numberOfState = value; }
        /// <summary>
        /// выполнено ли задание OldMan для получения отита
        /// </summary>
        public bool TaskCompleted { get => taskCompleted; set => taskCompleted = value; }
        /// <summary>
        /// задание получено
        /// </summary>
        public bool GotTask { get => gotTask; set => gotTask = value; }

        public Check()
        { 
        }

        public Check(int numberOfWindow)
        {
            
            GoBarack = 0;
            numberOfState = 0;
            taskCompleted = false;
            gotTask = false;
            prevProblem = 0;
            prevPrevProblem = 0;
            this.numberOfWindow = numberOfWindow;
            botwindow = new botWindow(numberOfWindow);
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            MarketFactory marketFactory = new MarketFactory(botwindow);
            this.market = marketFactory.createMarket();
            KatoviaMarketFactory kMarketFactory = new KatoviaMarketFactory(botwindow);
            this.kMarket = kMarketFactory.createMarket();
            PetFactory petFactory = new PetFactory(botwindow);
            this.pet = petFactory.createPet();
            DialogFactory dialogFactory = new DialogFactory(botwindow);
            this.dialog = dialogFactory.createDialog();
            OtitFactory otitFactory = new OtitFactory(botwindow);
            this.otit = otitFactory.createOtit();
            MMFactory mmFactory = new MMFactory(botwindow);
            this.mm = mmFactory.create();
            BHDialogFactory bhDialogFactory = new BHDialogFactory(botwindow);
            this.BHdialog = bhDialogFactory.create();  

            this.driver = new DriversOfState(numberOfWindow);
            this.globalParam = new GlobalParam();
            this.botParam = new BotParam(numberOfWindow);
            this.isActiveServer = server.IsActiveServer;
            //botParam.HowManyCyclesToSkip = 0;
            DirectionOfMovement = 1;
            Hero = new int[4] {0,0,0,0};
            dateNow = DateTime.Now;
            dateSteam = DateTime.Now;
            SteamLoaded = false;
        }

        ///// <summary>
        ///// продаем одно окно с ботом (кнопка "Направить все окна на продажу" )
        ///// </summary>
        //public void NewWhiteButton()
        //{
        //    driver.StateGotoTrade();
        //}

        ///// <summary>
        ///// возвращает номер телепорта для продажи
        ///// </summary>
        ///// <returns></returns>
        //public int getNumberTeleport()
        //{
        //    return botwindow.getNomerTeleport();
        //}

        ///// <summary>
        ///// если находимся на алхимическом столе, то true
        ///// </summary>
        ///// <returns></returns>
        //public bool isAlchemy()
        //{
        //    return server.isAlchemy();
        //}

        ///// <summary>
        ///// выполняет действия по открытию окна с игрой
        ///// </summary>
        //public void OpenWindow ()
        //{
        //    server.ReOpenWindow();
        //}

        //public bool EndOfList()
        //{
        //    return botParam.EndOfList();
        //}

        /// <summary>
        /// закрываем песочницу и переходим к следующему аккаунту
        /// </summary>
        public void RemoveSandboxie()
        {
            server.RemoveSandboxieBH();
        }

        #region  =================================== Demonic Solo Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemStage1()
        {
            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                return 21;
                //if (!server.FindWindowSteamBool())  //если Стима тоже нет
                //{
                //    return 24;
                //}
                //else    //если Стим уже загружен
                //{
                //    if (!server.FindWindowGEforBHBool())
                //    {
                //        return 22;    //если нет окна с нужным HWND и, если не найдено окно с любым другим hwnd не равным нулю
                //    }
                //    else
                //    {
                //        return 23;  //нашли другое окно с заданными параметрами (открыли новое окно на предыдущем этапе программы)
                //    }
                //}
            }

            //ворота
            if (dialog.isDialog()) 
            {
                if (server.isMissionNotAvailable())
                    return 10;
                else
                    return 8;                        //если стоим в воротах Demonic и миссия доступна
            }
            

            //Mission Lobby
            if (server.isMissionLobby()) return 5;

            //Waiting Room
            if (server.isWaitingRoom()) return 3;

            //город или БХ
            if (server.isTown() && !server.isBattleMode() && !server.isAssaultMode())   //если в городе, но не в боевом режиме и не в режиме атаки
            {
                if (server.isBH())     //в БХ 
                {
                    //if (server.isBH2()) return 18;   //стоим в БХ в неправильном месте
                    //else
                    return 4;   // стоим в правильном месте (около ворот Demonic)
                }
                else   // в городе, но не в БХ
                {
                    return 6;
                }
            }

            //в миссии
            if (server.isWork())    return 7;

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDemStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    //botwindow.ActiveWindowBH();   //перед проверкой проблем, активируем окно с ботом. Это вместо ReOpenWindow()
                    server.ReOpenWindow();
                    Pause(500);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemStage1();       

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)  
                {
                    switch (numberOfProblem)
                    {
                        //case 1:  //зависли в логауте
                        //case 23:  //загруженное окно зависло и не смещается на нужное место
                        //    numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                        //    break;
                        case 4:  //зависли в БХ
                            numberOfProblem = 18; //переходим в стартовый город через системное меню
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:
                        driver.StateFromBarackToTownBH();           // идем в город     //ок
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 3:                                         // старт миссии      //ок
                        server.MissionStart();
                        break;
                    case 4:
                        driver.StateFromBHToGateDem();              // BH --> Gate Demonic  (бафы + патроны)
                        break;
                    case 5:
                        server.CreatingMission();
                        break;
                    case 6:                                         // town --> BH
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(3, true);                   // телепорт в Гильдию Охотников (третий телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 7:
                        //бафаемся, поднимаем камеру максимально вверх, активируем пета и переходим к стадии 2
                        if (!botwindow.isCommandMode()) botwindow.CommandMode();
                        server.BattleModeOn();                      //пробел
                        server.ChatFifthBookmark();
                        Hero[1] = server.WhatsHero(1);
                        Hero[2] = server.WhatsHero(2);
                        Hero[3] = server.WhatsHero(3);
                        server.Buff(Hero[1], 1);
                        server.Buff(Hero[2], 2);
                        server.Buff(Hero[3], 3);
                        driver.StateActivePetDem();                 //сделать свой вариант вызова пета специально для Демоник!!!!!!
                        //MessageBox.Show(Hero[1] + " " + Hero[2] + " " + Hero[3]);
                        server.MaxHeight(7);                      //
                        botParam.Stage = 2;
                        break;
                    case 8:                                         //Gate --> List of missions
                        dialog.PressStringDialog(1);                //нажимаем нижнюю строчку (join)
                        dialog.PressOkButton(1);                    //нажимаем Ок в диалоге ворот
                        break;
                    case 10:                                         //миссия не доступна на сегодня (уже прошли)
                        dialog.PressOkButton(1);
                        botwindow.Pause(2000);
                        server.GotoBarack();
                        botwindow.Pause(6000);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 3;
                        //server.RemoveSandboxieBH();
                        break;
                    case 14:
                        driver.StateFromMissionToBarackBH();        // в барак 
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 18:
                        server.systemMenu(3, true);                 // переход в стартовый город
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 21:                                        // нет окна
                        server.ReOpenWindow();                      // 
                        //botParam.HowManyCyclesToSkip = 3;
                        break;
                    //case 22:                                    //Ок
                    //    server.runClientBH();                   // если нет окна ГЭ, то запускаем его   //Ок
                    //    botParam.HowManyCyclesToSkip = rand.Next(5, 8);       //пропускаем следующие 5-8 циклов
                    //    break;
                    //case 23:                                    //Ок
                    //    botwindow.ActiveWindowBH();             // если новое окно открыто, но еще не поставлено на своё место, то ставим
                    //    botParam.HowManyCyclesToSkip = 1;       //пропускаем следующий цикл (на всякий случай)
                    //    break;
                    //case 24:                                    //Ок
                    //    //поменялся номер инфинити в файле Инфинити.txt в папке, поэтому надо заново создать botwindow, server и проч *******
                    //    botwindow = new botWindow(numberOfWindow);
                    //    ServerFactory serverFactory = new ServerFactory(botwindow);
                    //    this.server = serverFactory.create();
                    //    this.globalParam = new GlobalParam();
                    //    this.botParam = new BotParam(numberOfWindow);
                    //    //********************************************************************************************************************
                    //    server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                    //    botParam.HowManyCyclesToSkip = rand.Next(1, 6);        //пропускаем следующие циклы (от одного до шести)
                    //    break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion

        #region  =================================== Demonic Solo Stage 2 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemStage2()
        {
            //в миссии
            if (server.isWork())
            {
                //если белая надпись сверху или розовая надпись в чате, значит появился сундук и надо идти в барак и далее стадия 3
                if (server.isWhiteLabel() || 
                    server.isTreasureChest()) return 10;

                //if (!server.isAssaultMode())
                //{
                    return 3;   //если нет режима атаки
                //}
                //else
                //{
                //    //if (server.isBoss()) 
                //    //    return 5;
                //    //else 
                //    return 4;   //если атакуем с Ctrl, то обновляем бафы
                //}
                
            }

            //в БХ вылетели, значит миссия закончена
            //if (server.isTown() && server.isBH()) return 5;


             //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа

            //в миссии, но убиты
            if (server.isKillAllHero()) return 29;              // если убиты все
            //if (server.isKillHero()) return 30;               // если убиты не все 

            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            if (server.isTown() && server.isBH() && !server.isKillHero()) return 29;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в Demonic
        /// </summary>
        public void problemResolutionDemStage2()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ReOpenWindow();
                    Pause(500);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemStage2();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                             //в логауте (при переходе в барак можем попасть в логаут)
                        driver.StateFromLogoutToBarackBH();             // Logout-->Barack   
                        botParam.HowManyCyclesToSkip = 1;
                        //server.RemoveSandboxieBH();
                        break;
                    case 2:                                             //в бараках
                        server.ReturnToMissionFromBarack();                 // идем из барака обратно в миссию     
                        botParam.HowManyCyclesToSkip = 2;
                        server.MoveCursorOfMouse();
                        botParam.Stage = 3;
                         //server.RemoveSandboxieBH();
                        break;
                    case 3:                                                 //уже не атакуем
                        DirectionOfMovement = -1 * DirectionOfMovement;     //меняем направление движения
                        server.AttackTheMonsters(DirectionOfMovement);      //атакуем
                        Pause(1500);
                        server.Buff(Hero[1], 1);
                        server.Buff(Hero[2], 2);
                        server.Buff(Hero[3], 3);
                        server.BattleModeOn();
                        break;
                    case 4:                                                 //пробуем пробафаться
                        
                        server.Buff(Hero[1], 1);
                        server.Buff(Hero[2], 2);
                        server.Buff(Hero[3], 3);
                        server.BattleModeOn();
                        //if (Hero[1] == 1) server.BuffE(1);
                        //if (Hero[2] == 1) server.BuffE(2);
                        //if (Hero[3] == 1) server.BuffE(3);
                        break;
                    case 5:                                         //если в миссии и в прицеле босс, то скилляем 
                        //обновляем баффы, если надо
                        server.Buff(Hero[1], 1);
                        server.Buff(Hero[2], 2);
                        server.Buff(Hero[3], 3);

                        //int number = rand.Next(1, 3);
                        //сделать выбор персонажа через rnd и им скиловать
                        //server.Skill(Hero[number], number);
                        //server.Skill(Hero[1], 1);
                        //server.Skill(Hero[2], 2);
                        //server.Skill(Hero[3], 3);
                        server.BattleModeOn();
                        break;
                    case 10:
                        //если белая надпись вверху
                        server.BattleModeOn();                      //нажимаем пробел, чтобы не убежать от дропа
                        botwindow.Pause(7000);                      //пауза, чтобы успеть собрать добычу
                        server.GotoBarack();                        // идем в барак, чтобы перейти к стадии 3 (открытие сундука и проч.)
                        botwindow.Pause(6000);                      //пауза, чтобы успеть войти в барак
                        //botParam.HowManyCyclesToSkip = 5;
                        //botParam.Stage = 3;
                        //server.RemoveSandboxieBH();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 20:
                        server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 29:                                        //если все убиты
                        server.GotoBarack();                        // идем в барак, чтобы перейти к стадии 3 (открытие сундука и проч.)
                        botwindow.Pause(5000);                      //пауза, чтобы успеть войти в барак
//                        botParam.HowManyCyclesToSkip = 5;
                        //botParam.Stage = 3;
                        //server.RemoveSandboxieBH();
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion

        #region  =================================== Demonic Solo Stage 3 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemStage3()
        {
            //если диалог (он получается, если тыкнуть в ворота)
            if (dialog.isDialog()) return 7;

            //в миссии
            if (server.isWork())
            {
                if (server.isRouletteBH()) 
                    return 4;
                else
                    return 3;
            }

            //в городе или в БХ
            if (server.isTown())
            {
                if (server.isBH()) return 5;        //в БХ
                else return 6;                      //в стартовом городе
            }

            //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 

            //if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ Демоник стадия 3
        /// </summary>
        public void problemResolutionDemStage3()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ReOpenWindow();
                    Pause(500);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemStage3();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:                                         //в бараках
                        driver.StateFromBarackToTownBH();           // идем в город (это нужно для Инфинити, а то они начнут со старого места в БХ)
                        botParam.HowManyCyclesToSkip = 2;
                        //server.RemoveSandboxieBH();               //закрываем песочницу
                        //botParam.Stage = 1;
                        break;
                    case 3:                                         //в миссии, но рулетка не крутится
                        server.OpeningTheChest();                   //тыкаем в сундук и запускаем рулетку
                        //server.MaxHeight(7);
                        botwindow.Pause(5000);
                        break;
                    case 4:                                         //крутится рулетка
                        botwindow.Pause(5000);
                        server.GotoBarack();
                        botParam.HowManyCyclesToSkip = 5;
                        //server.RemoveSandboxieBH();                 //закрываем песочницу
                        //botParam.Stage = 1;
                        //сюда можно будет запихнуть прохождение ворот с фесо
                        break;
                    case 5:                                         // в БХ
                        server.systemMenu(3, true);                 // переход в стартовый город
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 6:                                         // в городе
                        server.RemoveSandboxieBH();                 //закрываем песочницу
                        botParam.Stage = 1;
                        break;
                    case 7:                                         // в диалоге
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        break;
                    case 20:
                        server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                        break;

                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion


        #region  =================================== Demonic Multi Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage1()
        {
            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                //return 21;

                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (!server.FindWindowGEforBHBool())
                    {
                        return 22;                  //если нет окна ГЭ в текущей песочнице
                    }
                    else
                    {
                        return 23;                  //нашли окно ГЭ в текущей песочнице и перезаписали Hwnd
                    }
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow)
                    IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //если открыто окно Стим в правом нижнем углу
            //if (server.isOpenSteamWindow()) { server.CloseSteamWindow(); server.CloseSteam(); }

            //служба Steam
            //if (server.isSteamService())    return 11;

            //случайно зашли в Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;

            //ворота
            if (dialog.isDialog())
            {
                if (server.isMissionNotAvailable())
                    return 10;
                else
                    return 8;                       //если стоим в воротах Demonic и миссия доступна
            }


            //Mission Lobby
            if (server.isMissionLobby()) return 5;      //сделано

            //Waiting Room
            if (server.isWaitingRoom()) return 3;

            //город или БХ
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                if (
                !server.isBattleMode() &&
                !server.isAssaultMode())   //если в городе, но не в боевом режиме и не в режиме атаки
                {
                    if (server.isBH())     //в БХ     //проверка сделана
                    {
                        //if (server.isBH2()) return 18;   //стоим в БХ в неправильном месте
                        //else
                        return 4;   // стоим в правильном месте (около ворот Demonic)
                    }
                    else   // в городе, но не в БХ
                    {
                        return 6;
                    }
                }
            }

            //в миссии
            if (server.isWork()) return 7;

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDemMultiStage1()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 1  HowManyCyclesToSkip " + botParam.HowManyCyclesToSkip);
            GoBarack = 0;
            
            
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    //botwindow.ActiveWindowBH();   //перед проверкой проблем, активируем окно с ботом. Это вместо ReOpenWindow()

                    //server.ReOpenWindow();
                    server.ActiveWindow();
                    //Pause(500);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage1();

                if (SteamLoaded) dateSteam = DateTime.Now;
                dateNow = DateTime.Now;
                if ((dateNow - dateSteam).TotalMinutes > 5) 
                    numberOfProblem = 31;


                server.WriteToLogFileBH("номер проблемы " + numberOfProblem);

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        //case 24:
                        //    numberOfProblem = 32;  //закрываем песочницу без перехода к следующему аккаунту
                        //    break;

                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                        case 4:  //зависли в БХ
                            server.WriteToLogFileBH("зависли в БХ");
                            numberOfProblem = 18; //переходим в стартовый город через системное меню
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    case 3:                                         // старт миссии      //ок
                        server.WriteToLogFileBH("case 3");
                        server.MissionStart();
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 4:
                        server.WriteToLogFileBH("case 4");
                        driver.StateFromBHToGateDem();              // BH --> Gate Demonic  (бафы + патроны)
                        break;
                    case 5:
                        server.CreatingMission();
                        break;
                    case 6:                                         // town --> BH
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(3, true);                   // телепорт в Гильдию Охотников (третий телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 7:                                     // поднимаем камеру максимально вверх, активируем пета и переходим к стадии 2
                        server.WriteToLogFileBH("начинаем обработку case 4");
                        server.MoveMouseDown();
                        botwindow.CommandMode();
                        server.BattleModeOnDem();                   //пробел
                        server.ChatFifthBookmark();
                        Hero[1] = server.WhatsHero(1);
                        Hero[2] = server.WhatsHero(2);
                        Hero[3] = server.WhatsHero(3);

                        server.messageWindowExtension();

                        driver.StateActivePetDem();                 
                        server.MaxHeight(7);                      
                        botParam.Stage = 2;
                        break;
                    case 8:                                         //Gate --> List of missions
                        dialog.PressStringDialog(1);                //нажимаем нижнюю строчку (join)
                        dialog.PressOkButton(1);                    //нажимаем Ок в диалоге ворот
                        break;
                    case 10:                                        //миссия не доступна на сегодня (уже прошли)
                        dialog.PressOkButton(1);
                        botwindow.Pause(1000);  //2000
                        //server.GotoBarack();
                        server.GotoSavePoint();
                        botParam.Stage = 3;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин
                        server.CloseExpMerch();
                        break;

                    //case 14:
                    //    //driver.StateFromMissionToBarackBH();      // в барак 
                    //    botwindow.ClickSpaceBH();                   //переходим в боевой режим. Если есть в кого стрелять, то стреляем. 
                    //    server.GotoBarack(true);                    //если не в кого стрелять, уходим в барак

                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 18:
                        //server.systemMenu(3, true);                 // переход в стартовый город
                        server.GotoSavePoint();
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    //case 21:                                        // нет окна
                    //    server.ReOpenWindow();                      // 
                    //    //botParam.HowManyCyclesToSkip = 3;
                    //    break;
                    case 22:
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            SteamLoaded = true;
                            botParam.HowManyCyclesToSkip = rand.Next(3, 5);       //пропускаем следующие 5-8 циклов
                            IsItAlreadyPossibleToUploadNewSteam = 0;
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                        //server.ReOpenWindow();                      // 
                        //botwindow.ActiveWindowBH();             // если новое окно открыто, но еще не поставлено на своё место, то ставим
                        //botParam.HowManyCyclesToSkip = 1;       //пропускаем следующий цикл (на всякий случай)
                        IsItAlreadyPossibleToUploadNewWindow = 0; //не факт, что надо
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            dateSteam = DateTime.Now;
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            //botParam.HowManyCyclesToSkip = 1;
                            IsItAlreadyPossibleToUploadNewSteam = 1;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        //IsItAlreadyPossibleToUploadNewSteam = 0;
                        //IsItAlreadyPossibleToUploadNewWindow = 0; 
                        break;
                    case 32:
                        IsItAlreadyPossibleToUploadNewSteam = 0;
                        break;

                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем "+ botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Demonic Multi Stage 2 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage2()
        {
            //если открыто окно Стим в правом нижнем углу
            if (server.isOpenSteamWindow()) { server.CloseSteamWindow(); server.CloseSteam(); }

            //служба Steam
            if (server.isSteamService()) return 11;

            //в миссии
            if (server.isWork() || server.isKillFirstHero())
            {
                //если белая надпись сверху или розовая надпись в чате, значит появился сундук и надо идти в барак и далее стадия 3
                if (server.isWhiteLabel() ||
                    server.isTreasureChest()) return 10;
                else
                //    if (server.isBossOrMob() && !server.isMob())
                //    return 4;   //если босс в прицеле, то скилляем
                //else
                    return 3;
            }

            //служба Steam
            if (server.isSteamService()) return 11;


            //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа

            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            if (server.isTown() && server.isBH() && !server.isKillHero()) return 29;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в Demonic
        /// </summary>
        public void problemResolutionDemMultiStage2()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 2");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    //server.ReOpenWindow();
                    server.ActiveWindow();
                    //Pause(500);
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage2();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                                 // в логауте (при переходе в барак можем попасть в логаут)
                        driver.StateFromLogoutToBarackBH();                 // Logout-->Barack   
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:                                                 // в бараках
                        server.ReturnToMissionFromBarack();                 // идем из барака обратно в миссию     
                        botParam.HowManyCyclesToSkip = 2;
                        server.MoveCursorOfMouse();
                        botParam.Stage = 3;
                        DirectionOfMovement = 1;        //необходимо для стадии 4
                        break;
                    case 3:                                                 // собираемся атаковать
                        //botwindow.CommandMode();
                        DirectionOfMovement = -1 * DirectionOfMovement;     // меняем направление движения
                        server.AttackTheMonsters(DirectionOfMovement);      // атакуем с CTRL

                        //бафаемся. Если бафались мушкетеры, то result = true
                        server.MoveCursorOfMouse();
                        bool result =   server.Buff(Hero[1], 1) || 
                                        server.Buff(Hero[2], 2) || 
                                        server.Buff(Hero[3], 3);
                        //

                        //если не бафались и в прицеле моб или босс, то скиллуем мушкетерами скиллом E (массуха)
                        //а потом пробел
                        if (server.isBossOrMob() && !result)
                        //if (server.isBossOrMob())         //пробуем обойтись без проверки "баффались или нет"
                        {
                            bool isMobs = server.isMob();   
                            //server.MoveCursorOfMouse();
                            server.Skill(Hero[1], 1, isMobs);
                            server.Skill(Hero[2], 2, isMobs);
                            server.Skill(Hero[3], 3, isMobs);
                            //server.MoveCursorOfMouse();
                        }
                        //Pause(500);
                        server.BattleModeOnDem();

                        //вариант 2 (основной)
                        //server.Buff(Hero[1], 1);
                        //server.Buff(Hero[2], 2);
                        //server.Buff(Hero[3], 3);
                        ////server.BattleModeOn();

                        ////выбор главного героя через rnd и им скилуем
                        //int nn = rand.Next(1, 4);
                        //server.ActiveHeroDem(nn);
                        //if (server.isBossOrMob() && !server.isMob())
                        //{
                        //    server.Skill(Hero[nn], nn);
                        //}


                        break;
                    case 4:                                                 // скилуем

                        //int number = rand.Next(1, 3);
                        //сделать выбор персонажа через rnd и им скиловать
                        //server.Skill(Hero[number], number);

                        //server.Skill(Hero[1], 1);
                        //server.Skill(Hero[2], 2);
                        //server.Skill(Hero[3], 3);
                        
                        break;
                    case 5:                                                 // если в миссии и в прицеле босс, то скилляем 
                        //обновляем баффы, если надо
                        //server.Buff(Hero[1], 1);
                        //server.Buff(Hero[2], 2);
                        //server.Buff(Hero[3], 3);

                        //int number = rand.Next(1, 3);
                        //сделать выбор персонажа через rnd и им скиловать
                        //server.Skill(Hero[number], number);
                        //server.Skill(Hero[1], 1);
                        //server.Skill(Hero[2], 2);
                        //server.Skill(Hero[3], 3);
                        //server.BattleModeOn();
                        break;
                    case 10:
                        //если белая надпись вверху
                        server.BattleModeOn();                      //нажимаем пробел, чтобы не убежать от дропа
                        Pause(5000);
                        server.GotoBarack();                        // идем в барак, чтобы перейти к стадии 3 (открытие сундука и проч.)
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 20:
                        server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 29:                                        //если все убиты
                        server.GotoBarack();                        // идем в барак, чтобы потом перейти к стадии 3 (открытие сундука и проч.)
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion ======================================================================================================

        #region  =================================== Demonic Multi Stage 3 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage3()
        {
            //если открыто окно Стим в правом нижнем углу
            if (server.isOpenSteamWindow()) { server.CloseSteamWindow(); server.CloseSteam(); }

            //служба Steam
            if (server.isSteamService()) return 11;

            //если диалог (он получается, если тыкнуть в ворота)
            if (dialog.isDialog()) return 7;

            //в миссии
            if (server.isWork())
            {
                if (server.isGate() || GoBarack == 1 || server.isRouletteBH())     //если сундука уже нет, а появились ворота
                                                                                   //(либо уже тыкали в сундук)
                                                                                   //либо крутится рулетка
                {
                    if (server.isSecondGate())      //появились вторые ворота (с фесом)?
                        return 9;
                    else
                        return 4;                   //ворота с фесом не появились и надо идти в барак
                }
                //if (server.isRouletteBH() || GoBarack == 1)
                //    return 4;                       //надо идти в барак
                else
                    return 3;                       //надо тыкать в сундук
            }

            //в городе или в БХ
            if (server.isTown())
            {
                if (server.isBH()) return 5;        //в БХ
                else return 6;                      //в стартовом городе
            }

            //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 

            //if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ Демоник стадия 3
        /// </summary>
        public void problemResolutionDemMultiStage3()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 3");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow(); 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage3();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:                                         //в бараках
                        driver.StateFromBarackToTownBH();           // идем в город (это нужно для Инфинити, а то они начнут со старого места в БХ)
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 3:                                         //в миссии, но рулетка не крутится
                        server.OpeningTheChest();                   //тыкаем в сундук и запускаем рулетку
                        botParam.HowManyCyclesToSkip = 1;
                        GoBarack = 1;
                        server.MaxHeight(3);                        //чтобы было видно вторые ворота
                        break;
                    case 4:                                         //крутится рулетка или уже тыкали в сундук
                        server.GotoBarack();
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 5:                                         // в БХ
                        server.systemMenu(3, true);                 // переход в стартовый город
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 6:                                         // в городе
                        server.RemoveSandboxieBH();                 //закрываем песочницу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 7:                                         // в диалоге ворот (выйти в БХ или остаться на месте)
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    //case 8:                                         // появились ворота вместо сундука
                    //    server.GotoBarack();
                    //    botParam.HowManyCyclesToSkip = 2;
                    //    break;
                    case 9:                                         // появились ворота с фесо
                        //тыкаем в ворота с фесо и далее стадия 4
                        server.PressOnFesoGate();
                        botParam.Stage = 4;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;

                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion =======================================================================================================

        #region  =================================== Demonic Multi Stage 4 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic на стадии 4 и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage4()
        {
            //если диалог, то выбрать вход в комнату с фесо
            if (dialog.isDialog()) return 7;

            //в миссии
            if (server.isWork())
            {
                return 3;        
            }

            //в городе или в БХ
            if (server.isTown())
            {
                if (server.isBH()) return 5;        //в БХ
                else return 6;                      //в стартовом городе
            }

            //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ Демоник стадии 4
        /// </summary>
        public void problemResolutionDemMultiStage4()
        {
            //if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            //{
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage4();


                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //в бараках
                    case 5:                                         // в БХ
                    case 6:                                         // в городе
                        botParam.Stage = 3;
                        break;
                    case 3:                                         //в комнате с фесо
                        DirectionOfMovement = -1 * DirectionOfMovement;     // меняем направление движения
                        server.AttackTheMonsters2(DirectionOfMovement);     // атакуем с CTRL и подбираем лут
                        break;
                    case 7:                                         // в диалоге ворот (зайти в комнату с фесо)
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        //botParam.HowManyCyclesToSkip = 1;
                        break;
                }
            //}
            //else
            //{
            //    botParam.HowManyCyclesToSkip--;
            //}
        }

        #endregion =======================================================================================================



        #region Гильдия охотников BH

        /// <summary>
        /// проверяем, если ли проблемы при работе в БХ и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBH()
        {
            int statusOfAtk = botParam.StatusOfAtk;

            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (!server.FindWindowGEforBHBool())
                    {
                        return 22;    //если нет окна с нужным HWND и, если не найдено окно с любым другим hwnd не равным нулю
                    }
                    else
                    {
                        return 23;  //нашли другое окно с заданными параметрами (открыли новое окно на предыдущем этапе программы)
                    }
                }
            }

            //ворота
            if (BHdialog.isGateBH()) return 7;                    //если стоим в воротах, начальное состояние
            if (BHdialog.isGateBH1()) return 8;                   //ворота. дневной лимит миссий еще не исчерпан
            if (BHdialog.isGateBH3()) return 9;                   //ворота. дневной лимит миссий уже исчерпан
            if (BHdialog.isGateLevelLessThan11()) return 10;      //ворота. уровень миссии меньше 11
            if (BHdialog.isGateLevelFrom11to19()) return 19;      //ворота. уровень миссии от 11 до 19
            if (BHdialog.isGateLevelAbove20()) return 25;         //ворота. уровень миссии больше 20
            if (BHdialog.isInitialize()) return 26;               //ворота. форма, где надо вводить слово Initialize

            
            //город или БХ
            if (server.isTown() && !server.isBattleMode() && !server.isAssaultMode())   //если в городе, но не в боевом режиме и не в режиме атаки
            {
                if (server.isBH())     //в БХ 
                {
                    if (server.isBH2()) return 18;   //стоим в БХ в неправильном месте
                    else
                        return 4;   // стоим в правильном месте (около ворот Инфинити)
                }
                else   // в городе, но не в БХ
                {
                    return 6;
                }
            }

            ////магазин
            //if (market.isSale())
            //{
            //    if (!server.isTown() || server.isWork()) return 11;        //если стоим в магазине на экране входа, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            //}
            //if (market.isClickPurchase())
            //{
            //    if (!server.isTown() || server.isWork()) return 12;         //если стоим в магазине на закладке Purchase, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            //}
            //if (market.isClickSell())
            //{
            //    if (!server.isTown() || server.isWork()) return 15;         //если стоим в магазине на закладке Purchase, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            //}

            //в миссии
            if (server.isWork())
            {
                if (statusOfAtk == 0)                                                 //еще не атаковали босса в миссии
                {
                    if (server.isMissionBH())                                         //если миссия определилась
                    {
                        return 13;
                    }
                    else
                    {
                        return 21;                                                    //миссия не определилась
                    }
                }
                else                                                                  //после начала атаки босса
                {
                    if (server.isRouletteBH())                                        //если крутится рулетка
                    {
                        return 20;                                                    // подбор дропа
                    }
                    else
                    {
                        if (!server.isAtakBH()) return 14;                            //идем в барак (а можем и в БХ)
                                                                                        //если находимся в миссии, но уже не в начале и не атакуем босса и не крутится рулетка 
                                                                                        //(значит бой окончен, либо заблудились и надо выходить из миссии) 
                    }
                }
            }

            //в логауте
            if (server.isLogout()) return 1;               // если окно в логауте

            //в бараке
            if (server.isBarack())                         //если стоят в бараке 
            {
                if (server.isBarackLastPoint())            //если начиная со старого места попадаем в БХ
                { return 16; }
                else
                { return 2; }
            }
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы

            //в миссии, но убиты
            if (server.isKillAllHero()) return 29;            // если убиты все
            if (server.isKillHero()) return 30;               // если убиты не все 

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBH()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    //botwindow.ActiveWindowBH();   //перед проверкой проблем, активируем окно с ботом. Это вместо ReOpenWindow()
                    server.ReOpenWindow();
                    Pause(500);
                }

                int numberOfProblem = NumberOfProblemBH();          //проверили, какие есть проблемы (на какой стадии находится бот)

                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)  //если зависли в каком-либо состоянии, то особые действия
                {
                    switch (numberOfProblem)
                    {
                        case 1:  //зависли в логауте
                        case 23:  //загруженное окно зависло и не смещается на нужное место
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                        case 4:  //зависли в БХ
                            numberOfProblem = 18; //переходим в стартовый город через системное меню
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();
                

                switch (numberOfProblem)
                {
                    case 1:
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:
                        driver.StateFromBarackToTownBH();           // идем в город
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 3:
                        //временная заплатка. вместо продажи аккаунта закрываем песочницу и переходим к следующему аккаунту
                        botParam.StatusOfSale = 0;
                        server.RemoveSandboxieBH();

                        //проверено. работает
                        //int result = globalParam.Infinity;
                        //if (result >= 200) result = 52;
                        //botParam.NumberOfInfinity = result;
                        //globalParam.Infinity = result + 1;
                        //server.CloseSandboxieBH();
                        //server.MoveMouseDown();



                        //driver.StateGotoTradeStep1BH();             // BH-->Town (первый этап продажи)
                        //botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 4:
                        driver.StateFromBHToGateBH();               // BH --> Gate
                        break;
                    case 5:
                        driver.StateGotoTradeStep2BH();             // если стоят в городе и надо продаваться, то второй этап продажи
                        break;
                    case 6:
                        driver.StateFromTownToBH();                 // town --> BH
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 7:
                        driver.StateFromGateToMissionBH();          // Gate --> Mission
                        break;
                    case 8:
                        BHdialog.PressOkButton(1);                  //нажимаем Ок в диалоге ворот
                        break;
                    case 9:
                        //driver.StateLimitOff();                     // Ок в диалоге и удаляем песочницу (миссии закончились)
                        //server.RemoveSandboxieBH();

                        driver.StateCloseSandboxieBH();              //миссии закончились в воротах
                        break;
                    case 10:
                        driver.StateLevelLessThan11();              // диалог в воротах
                        break;
                    case 11:
                        driver.StateGotoTradeStep3BH();             // третий этап продажи
                        break;
                    case 12:
                        driver.StateGotoTradeStep4BH();             // четвертый этап продажи
                        break;
                    case 13:
                        driver.StateFromMissionToFightBH();         // Mission--> Fight!!!
                        break;
                    case 14:
                        driver.StateFromMissionToBarackBH();        // в барак 
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 15:
                        driver.StateGotoTradeStep5BH();             // пятый этап продажи
                        break;
                    case 16:
                        server.barackLastPoint();                   // начинаем со старого места в БХ
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 17:
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 18:
                        server.systemMenu(3, true);                 // переход в стартовый город
                        botParam.HowManyCyclesToSkip = 3;
                        break;
                    case 19:
                        driver.StateLevelFrom11to19();              // диалог в воротах
                        break;
                    case 20:
                        server.GetDrop();                         //подбор дропа
                        break;
                    case 21:
                        server.MissionNotFoundBH();              // миссия не найдена. записываем в файл данные и отбегаем в сторону
                        driver.StateFromMissionToBarackBH();        // в барак 
                        break;
                    case 22:
                        server.runClientBH();                   // если нет окна ГЭ, то запускаем его
                        botParam.HowManyCyclesToSkip = rand.Next(5, 8);       //пропускаем следующие 5-10 циклов
                        break;
                    case 23:
                        botwindow.ActiveWindowBH();             // если новое окно открыто, но еще не поставлено на своё место, то ставим
                        botParam.HowManyCyclesToSkip = 1;       //пропускаем следующий цикл (на всякий случай)
                        break;
                    case 24:
                        //поменялся номер инфинити в файле Инфинити.txt в папке, поэтому надо заново создать botwindow, server и проч *******
                        botwindow = new botWindow(numberOfWindow);
                        ServerFactory serverFactory = new ServerFactory(botwindow);
                        this.server = serverFactory.create();
                        this.globalParam = new GlobalParam();
                        this.botParam = new BotParam(numberOfWindow);
                        //********************************************************************************************************************
                        server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                        botParam.HowManyCyclesToSkip =  rand.Next(1, 6);        //пропускаем следующие циклы (от одного до шести)
                        break;
                    case 25:
                        driver.StateLevelAbove20();             // уровень ворот больше 20. идем тратить шайники
                        break;
                    case 26:
                        driver.StateInitialize();              // вводим слово Initialize и ок
                        break;
                    case 29:
                        botwindow.CureOneWindow();              // идем в logout
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 30:
                        botwindow.CureOneWindow2();             // отбегаем в сторону и логаут
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        break;

                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion

        #region Pure Otite Multi

        /// <summary>
        /// проверяем, есть ли проблемы при добыче Чистого Отита и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemOtite()
        {
            int result = 0;     // пока проблем не найдено

            if (!server.isHwnd()) result = 25;                  // если нет окна с hwnd таким как в файле HWND.txt
            if (server.isLogout()) result = 1;                  // если окно в логауте
            //if (server.isBarackTeamSelection()) result = 17;    // если в бараках на стадии выбора группы
            if (result != 0) return result;

            //            if (dialog.isDialog() && !server.isTown())
            //если стоим в диалоге с кем-то
            if (dialog.isDialog())
            {
                switch (NumberOfState)
                {
                    case 1:
                    case 2:
                        result = 8;             //диалог с Mamons
                        break;
                    case 3:
                        if (!TaskCompleted)
                            if (GotTask)
                                result = 11;        //Oldman(Dialog) --> Mission
                            else
                                result = 9;         //OldMan (задание не выполнено, и не взято). получаем задание
                        else
                            result = 10;        //OldMan (получение награды)
                        break;
                    case 4:
                        if (GotTask) result = 11;            //OldMan (задание взято, переходим в миссию)
                        break;
                    case 5:
                        result = 7;            //OldMan (задание взято, переходим в миссию)
                        break;
                    default:
                        if (!TaskCompleted)
                            if (GotTask)
                                result = 11;        //Oldman(Dialog) --> Mission
                            else
                                result = 9;         //OldMan (задание не выполнено, и не взято). получаем задание
                        else
                            result = 10;        //OldMan (получение награды)
                        break;
                }
                if (result != 0) return result;
            }

            // Город (Ребольдо или ЛосТолдос)
            if (server.isTown())   
            {
                if (otit.isGetTask()) GotTask = true;            // сделано
                if (otit.isTaskDone()) TaskCompleted = true;     // сделано

                botwindow.PressEscThreeTimes();  //чтобы убрать рекламу с экрана

                switch (NumberOfState)
                {
                    case 1:
                        result = 3;         // мы в Ребольдо
                        break;
                    case 2:
                        result = 4;         // мы в ЛосТолдосе (только что зашли)
                        break;
                    case 3:             //стоим около OldMan задание не взято
                    case 4:             //стоим около OldMan задание взято
                        if (otit.isNearOldMan())     //на всякий случай проверяем, стоим ли мы около ОлдМана   //сделано
                            result = 5;
                        else
                            result = 4;
                        break;
                    case 5:
                    case 6:
                        result = 29;         // все персы убиты, но выглядит, как будто мы в городе
                        break;
                    default:
                        //стоим в ЛосТолдосе
                        result = 4;         // мы в ЛосТолдосе (только что зашли)
                        break;
                }
                if (result != 0) return result;
            }

            //в миссии или около Мамута
            if (server.isWork())
            {
                switch (NumberOfState)
                {
                    case 1:
                        result = 6;         //стоим около мамута (работает хорошо)
                        break;
                    case 5:
                        result = 12;        //стоим в миссии. только что зашли
                        break;
                    case 6:                 // в бою!
                        if (!otit.isOpenMap())   //сделано
                            if (otit.isTaskDone())
                                result = 15;   //если карта закрыта и задание уже выполнено
                            else
                                result = 16;   //если карта закрыта, но задание еще не выполнено
                        else  // если карта уже открыта, то смотрим далее
                        {
                            if (!otit.isTaskDone())
                                if (!server.isAssaultMode()) 
                                    result = 13;    //бежим к следующей точке маршрута с атакой
                                else 
                                    return 0;       //ничего не делаем, так как уже бежим и атакуем
                            else
                                result = 14;        //бежим к следующей точке маршрута без атаки
                        }
                        break;
                    case 7:
                        result = 15;        //улетаем из миссии к Мамуну
                        break;
                    default:                //по умолчанию считаем, что если в бою, то в миссии 
                        if (!otit.isOpenMap())
                        {
                            if (otit.isTaskDone())
                                result = 15;   //если карта закрыта и задание уже выполнено
                            else
                                result = 16;   //если карта закрыта, но задание еще не выполнено
                        }
                        else  // если карта уже открыта, то смотрим далее
                        {
                            if (!otit.isTaskDone())
                                if (!server.isAssaultMode())
                                    result = 13;    //бежим к следующей точке маршрута с атакой
                                else
                                    return 0;       //ничего не делаем, так как уже бежим и атакуем
                            else
                                result = 14;    //бежим к следующей точке маршрута без атаки
                        }
                        break;
                }
                if (result != 0) return result;
            }


            if (result == 0 && server.isBarack()) result = 2;                  // если стоят в бараке 
            if (server.isKillAllHero()) result = 29;                            // если убиты все
            //if (result == 0 && server.isKillHero()) result = 30;               // если убиты не все 


            return result;
        }

        /// <summary>
        /// разрешение выявленных проблем в добыче Pure Otite
        /// </summary>
        public void problemResolutionOtite()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ReOpenWindow();
                    Pause(500);
                }

                int numberOfProblem = NumberOfProblemOtite();
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)  //если зависли в каком-либо состоянии, то особые действия
                {
                    if (dialog.isDialog())  numberOfProblem = 28;
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }


                //Random rand = new Random();


                switch (numberOfProblem)
                {
                    case 1:
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack
                        botParam.HowManyCyclesToSkip = 1;
                    break;
                    case 2:
                        driver.StateFromBarackToTownBH();           // Barack --> Town
                        NumberOfState = 1;
                        botParam.HowManyCyclesToSkip = 2;
                    break;
                    case 3:
                        driver.FromTownToMamut();                   // Town --> Mamut
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 4:
                        botwindow.FirstHero();
                        Pause(500);
                        otit.GoToOldManMulti();                         // Lostoldos --> OldMan
                        //driver.FromLostoldosToOldman();               // Lostoldos --> OldMan
                        if (otit.isTaskDone()) TaskCompleted = true; 
                            else TaskCompleted = false;  
                        if (otit.isGetTask()) GotTask = true; 
                            else GotTask = false;         
                        NumberOfState = 3;                              
                    break;
                    case 5:
                        //перед диалогом с Олдмэном проверяет задание
                        if (otit.isTaskDone()) TaskCompleted = true;
                        else TaskCompleted = false;
                        if (otit.isGetTask()) GotTask = true;
                        else GotTask = false;
                        //============================================
                        otit.PressOldMan();                             // OldMan --> OldMan (dialog)
                        //driver.PressOldMan();                       // OldMan --> OldMan (dialog)
                        break;
                    case 6:
                        driver.FromMamonsToMamonsDialog();          //Mamons --> MaMons(Dialog)
                    break;
                    case 7:
                        dialog.PressOkButton(1);                      //OldMan (dialog) нажатие по умолчанию (зависли)
                        break;
                    case 8:
                        otit.TalkMamons();
                        //driver.FromMamonsDialogToLostolods();       //Mamon Dialog --> LosToldos
                        NumberOfState = 2;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 9:
                        otit.GetTask();                             //Oldman(Dialog) --> Get Task
                      //driver.OldManDialogGetTask();               //Oldman(Dialog) --> Get Task
                        NumberOfState = 4;
                    break;
                    case 10:
                        otit.TakePureOtite();                       //Oldman(Dialog) --> Get Reward
                        //driver.OldManDialogGetReward();             //Oldman(Dialog) --> Get Reward
                        NumberOfState = 3;
                        TaskCompleted = false;
                        GotTask = false;
                        break;
                    case 11:
                        if (GotTask)
                        {
                            otit.EnterToTierraDeLosMuertus();           //Oldman(Dialog) --> Mission
                            //driver.FromOldManDialogToMission();         //Oldman(Dialog) --> Mission
                            botParam.HowManyCyclesToSkip = 2;
                            NumberOfState = 5;
                        }
                        else
                        {
                            NumberOfState = 3;              // возвращаем состояние к получению задания или получению награды
                        }
                    break;
                    case 12:
                        driver.FromMissionToFight();                //Mission-- > Mission (Fight begin)
                        botParam.HowManyCyclesToSkip = 1;
                        NumberOfState = 6;
                    break;
                    case 13:
                        otit.GotoNextPointRouteMulti();             //Mission(Fight)-- > Fight To Next Point
                        //driver.FightNextPoint();                    //Mission(Fight)-- > Fight To Next Point
                        //botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 14:
                        driver.FightIsFinished();                   // Mission (FightIsFinished) 
                        TaskCompleted = true;
                        NumberOfState = 7;
                    break;
                    case 15:
                        //botwindow.PressEscThreeTimes();             //убираем всё лишнее с экрана
                        //server.runAway();                           
                        //server.Teleport(1,true);                    // телепорт к Мамуну   //раньше true не было (01-07-2021)
                        //botwindow.PressEscThreeTimes();             //убираем меню с телепортами

                        driver.TeleportToMamut();
                        botParam.HowManyCyclesToSkip = 1;
                        NumberOfState = 1;
                    break;
                    case 16:
                        botwindow.PressEscThreeTimes();             //открываем карту в миссии
                        server.TopMenu(6, 2);
                        NumberOfState = 6;
                        break;
                    //case 17:
                    //    botwindow.PressEsc();                       // нажимаем Esc
                    //    break;
                    case 25:
                        server.ReOpenWindow();
                        break;
                    case 28:
                        server.CloseSandboxieBH();
                        break;
                    case 29:
                        botwindow.CureOneWindow();              // идем в logout
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    //case 30:
                    //    botwindow.CureOneWindow2();             // отбегаем в сторону и логаут
                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion

        #region Обычные боты (решение проблем)

        /// <summary>
        /// проверяем, есть ли проблемы с ботом (убили, застряли, нужно продать)
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblem()
        {
            if (server.isLogout()) { server.WriteToLogFile("Логаут");  return 1; }                         // если окно в логауте
            if (server.isKillAllHero()) { server.WriteToLogFile("Все убиты"); return 2; }                  // если убиты все
            if (server.isKillHero())    { server.WriteToLogFile("Убиты не все"); return 3; }               // если убиты не все 
            int numberTeleport = this.botwindow.getNomerTeleport();
            if (server.isBoxOverflow())                             // выскочило сообщение о переполнении кармана (либо оранжевая надпись на экране) 
            {
                if (numberTeleport > 0)                            // (телепорт = 0, тогда не нужно продавать)
                {
                    //if (server.is248Items())                       //проверяем реально ли карман переполнился
                    //{
                        //if (numberTeleport >= 100)                 // продажа в снежке
                        //    {  server.WriteToLogFile("Продажа в снежке"); return 5; }
                        //else                                 
                        //    { 
                            server.WriteToLogFile("Продажа не в снежке"); return 6; // продажа в городах
                        //}

                    //}
                    //else  
                    //{
                    //    //если выскочило сообщение о переполнении кармана, но количество вещей меньше 248, значит надо обменивать какашки на монеты
                    //    return 19;
                    //}
                }
            }
            if (pet.isOpenMenuPet()) { server.WriteToLogFile("открыто меню пет"); return 4; } //если открыто меню с петом, значит пет не выпущен
            if (market.isSale()) { server.WriteToLogFile("В магазине"); return 7; }           // если бот стоит в магазине на странице входа
            if (market.isSale2()) return 8;                         //если зависли в магазине на любой закладке
            if (kMarket.isSale()) return 12;                        //если бот стоит в магазине на странице входа
            if (kMarket.isClickSell()) return 13;                   //если зависли в катовичевском магазине на закладке Sell
            if (kMarket.isSaleIn()) return 14;                      //если зависли в катовичевском магазине на закладке BUY 
            if (server.isBarack()) return 9;                        //если стоят в бараке     
            if (server.isBarackTeamSelection()) return 9;           //если стоят в бараке  на странице выбора группы
            if (server.isBarackCreateNewHero()) return 20;          //если стоят на странице создания нового персонажа
            if (server.isTown()) return 10;                         //если стоят в городе
            if (mm.isMMSell() || (mm.isMMBuy())) return 11;         //если бот стоит на рынке
            //if (server.isBulletHalf()|| server.isBulletOff()) return 15;      // если заканчиваются экспертные патроны
            if (server.isWork() && !server.isBattleMode()) return 18;       //если стоим на работе, но не в боевом режиме
            return 0;
        }

        /// <summary>
        /// решение проблем с ботами
        /// </summary>
        public void problemResolution()
        { 
            ReOpenWindow();
            Pause(500);

            int numberOfProblem = NumberOfProblem();             //проверили, какие есть проблемы (на какой стадии находится бот)

            if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)  //если зависли в каком-либо состоянии, то особые действия
            {
                switch (numberOfProblem)
                {
                    case 1:  //зависли в логауте
                    case 9:  //в бараках
                        numberOfProblem = 17;
                        break;
                }
            }
            else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

            switch (numberOfProblem)
            {
                case 1: driver.StateRecovery();                 //логаут --> работа
                    break;
                case 2: botwindow.CureOneWindow();              //закрываем окно
                    break;
                case 3: botwindow.CureOneWindow2();             //отбегаем в сторону и закрываем окно
                    break;
                case 4: driver.StateActivePet();
                    break;
                case 5: driver.StateGotoTradeKatovia();         
                    break;
                case 6: driver.StateGotoTrade();                //работа -> продажа -> выгрузка окна
                    break;
                case 7: driver.StateExitFromShop2();            //продаемся и закрытие песочницы   09-14
                    break;
                case 8: driver.StateExitFromShop();             //продаемся и закрытие песочницы   10-14                                                  
                    break;
                case 9: server.buttonExitFromBarack();          //StateExitFromBarack();
                    break;
                case 10: //driver.StateExitFromTown();          
                    server.Logout();
                    //server.GoToEnd();
                    //server.CloseSandboxie();              //закрываем все проги в песочнице
                    break;
                case 11: SellProduct();                     // выставление товаров на рынок
                    break;
                case 12: driver.StateSelling2();          //продажа в катовичевском магазине      
                    break;
                case 13: driver.StateSelling4();          //продажа в катовичевском магазине
                    break;
                case 14: driver.StateSelling3();          //продажа в катовичевском магазине   
                    break;
                //case 15:
                //    server.AddBullet10000();              //открываем коробку с патронами 10 000 штук
                //    break;
                case 16:
                    server.LeaveGame();                  //если окно три прохода подряд в логауте, значит зависло. поэтому нажимаем кнопку "Leave Game"
                    //server.CloseSandboxie();              //закрываем все проги в песочнице
                    break;
                case 17:
                    server.CloseSandboxie();              //закрываем все проги в песочнице
                    break;
                case 18:
                    server.BattleModeOn();         //включаем боевой режим
                    server.TopMenu(9, 2);          //открываем окно с петом (для включения будущей расстановки)
                    break;
                //case 19:
                //    driver.StateSerendibyteToTrade();
                //    break;
                case 20:
                    server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                    break;
            }
        }
        
        #endregion

        /// <summary>
        /// выставляем на рынок продукт, если у нас на рынке не лучшая цена
        /// </summary>
        public void SellProduct()
        {
            mm.SellProduct();
        }

        ///// <summary>
        ///// поиск новых окон с игрой для кнопки "Найти окна"
        ///// </summary>
        ///// <returns></returns>
        //public UIntPtr FindWindow()
        //{
        //    return server.FindWindowGE();
        //}

        /// <summary>
        /// пауза в милисекундах
        /// </summary>
        /// <param name="ms">милисекунды</param>
        public void Pause(int ms)
        {
            botwindow.Pause(ms);
        }

        /// <summary>
        /// проверка открыто ли окно и перемещение его в заданные координаты 
        /// </summary>
        /// <returns></returns>
        public void ReOpenWindow()
        {
            server.ReOpenWindow();
        }

        ///// <summary>
        ///// проверка, находится ли окно в состоянии Logout
        ///// </summary>
        ///// <returns></returns>
        //public bool isLogout()
        //{
        //    return server.isLogout();
        //}

        /// <summary>
        /// оранжевая кнопка. разные действия по серверам . не удалять
        /// </summary>
        public void OrangeButton()
        {
            server.OrangeButton();
        }

        /// <summary>
        /// Желтая кнопка. Загрузка Стимов без загркзки окон ГЭ. Разные действия по серверам . не удалять
        /// </summary>
        public void YellowButton()
        {
            server.runClientSteamBH();
        }

        /// <summary>
        /// боты заходят в город, получают подарок у ГМ и окно выгружается
        /// </summary>
        public void ChangingAccounts()
        {
            //for (int j = botParam.NumberOfInfinity; j < botParam.Logins.Length; j++)  //цикл по j от Инфинити и до конца файла Логины
            //{
            //driver.StateInputOutput5(); //вход и выход из игры (а в промежутке выполнение задания)
            botwindow = new botWindow(numberOfWindow);
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();
            this.globalParam = new GlobalParam();
            this.botParam = new BotParam(numberOfWindow);
            this.isActiveServer = server.IsActiveServer;

            if (isActiveServer) driver.StateInputOutput6();
            else 
            { 
                server.RemoveSandboxie();
                
            }
            //}
        }

        /// <summary>
        /// боты заходят в город, 
        /// </summary>
        public void ChangingAccounts2()
        {
            for (int j = botParam.NumberOfInfinity; j < botParam.Logins.Length; j++)
            {
                //server.WriteToLogFile("номер окна = " + j);
                driver.ChangingAccounts2(); //вход и выход из игры
            }
        }

        /// <summary>
        /// боты заходят в город, 
        /// </summary>
        public void ChangingAccounts3()
        {
            for (int j = botParam.NumberOfInfinity; j < botParam.Logins.Length; j++)
            {
                driver.ChangingAccounts3(); //вход и выход из игры
            }
        }

        /// <summary>
        /// боты заходят в город, 
        /// </summary>
        public void ChangingAccounts4()
        {
            for (int j = botParam.NumberOfInfinity; j < botParam.Logins.Length; j++)
            {
                driver.ChangingAccounts4(); //вход и выход из игры
            }
        }

        /// <summary>
        /// создать новые аккаунты в одном окне бота
        /// </summary>
        public void NewAccountsTwo()
        {
            for (int j = botParam.NumberOfInfinity; j < botParam.Logins.Length; j++)
                driver.StateNewAcc2(); //новые акки
        }

        ///// <summary>
        ///// определяет, нужно ли работать с этим окном (может быть отключено из-за профилактики на сервере)
        ///// </summary>
        ///// <returns></returns>
        //public bool isActive()
        //{
        //    return IsActiveServer;
        //}

        /// <summary>
        /// проверяем, находимся ли в магазине у Иды (заточка)
        /// </summary>
        /// <returns></returns>
        public bool isIda()
        {
            return server.isIda();
        }

        /// <summary>
        /// проверяем, находимся ли в магазине у Чиповщицы
        /// </summary>
        /// <returns></returns>
        public bool isEnchant()
        {
            return server.isEnchant();
        }

        ///// <summary>
        ///// проверяем, находимся ли мы в диалоге со старым мужиком в Лос Толдосе
        ///// </summary>
        //public bool isOldMan()
        //{
        //    return otit.isOldMan();
        //}
        
        /// <summary>
        /// раздевание в казарме
        /// </summary>
        public void UnDressing()
        {
            server.UnDressing();
        }

        /// <summary>
        /// выбор сервера (синг или америка2)
        /// </summary>
        public void serverSelection()
        {
            server.serverSelection();
        }

        ///// <summary>
        ///// номер аккаунта, номер логина по порядку
        ///// </summary>
        ///// <returns></returns>
        //public string NumberOfInfinity()
        //{
        //    return botParam.NumberOfInfinity.ToString();
        //}


        /// <summary>
        /// тестовая кнопка
        /// </summary>
        public void TestButton()
        {
            int i = 1;   //номер проверяемого окна

            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 5, 30, 55, 80, 105, 130, 5 };

            botWindow botwindow = new botWindow(i);
            Server server = new ServerSing(botwindow);
            //server.Buff(server.WhatsHero(1), 1);
            //server.Buff(server.WhatsHero(2), 2);
            //server.Buff(server.WhatsHero(3), 3);
            //Server server = new ServerEuropa2(botwindow);
            Otit otit = new OtitSing(botwindow);
            Dialog dialog = new DialogSing(botwindow);
            Town town = new SingTownReboldo(botwindow);
            //BHDialog BHdialog = new BHDialogSing(botwindow);
            //KatoviaMarket kMarket = new KatoviaMarketSing (botwindow);
            //Market market = new MarketSing(botwindow);
            //Pet pet = new PetSing(botwindow);

            server.ReOpenWindow();
            //server.Logout();
            //if (server.isLogout()) server.CloseExpMerch();
            //если открыто окно Стим в правом нижнем углу
            //if (server.isOpenSteamWindow()) server.CloseSteamWindow();

            //MessageBox.Show("есть первые ворота? " + server.isGate());
            //MessageBox.Show("моб? " + server.isMob());
            //MessageBox.Show("меню открыто? " + server.isOpenWarpList());
            //MessageBox.Show("первые ворота? " + server.isGate());
            //MessageBox.Show("баф2? " + server.FindHound(2));
            //MessageBox.Show("баф1? " + server.FindHound(1));
            //MessageBox.Show("баф3? " + server.FindMarksmanship(3));
            //MessageBox.Show("баф2? " + server.FindMarksmanship(2));
            //MessageBox.Show("баф1? " + server.FindMarksmanship(1));
            //MessageBox.Show("баф3? " + server.FindReloadBullet(3));
            //MessageBox.Show("баф1? " + server.FindConcentracion(1));
            //MessageBox.Show("баф2? " + server.FindConcentracion(2));
            //MessageBox.Show(" " + botwindow.isCommandMode());
            //MessageBox.Show("боевой режим?" + server.isBattleMode());
            //MessageBox.Show(" " + town.isOpenTownTeleport());
            //MessageBox.Show(" " + pet.isOpenMenuPet());
            //MessageBox.Show(" " + pet.isSummonPet());
            //MessageBox.Show(" " + pet.isActivePet());
            //MessageBox.Show(" " + otit.isTaskDone());

            //botwindow.Pause(1000);

            //MessageBox.Show("в бараке? " + server.isBarackCreateNewHero());
            //MessageBox.Show("Пояса нет? " + server.isEmptyBelt(1));
            //MessageBox.Show("Ботинок нет? " + server.isEmptyBoots(1));
            //MessageBox.Show("Сережки нет? " + server.isEmptyEarrings(1));
            //MessageBox.Show("Перчаток нет? " + server.isEmptyGloves(1));
            //MessageBox.Show("Ожерелья нет? " + server.isEmptyNecklace(1));
            //MessageBox.Show("Открыто окно Inventory? " + server.isOpenInventory());
            //MessageBox.Show("Открыто окно Achievement? " + server.isOpenAchievement());
            //MessageBox.Show("На странице получения наград? " + server.isReceiveReward());
            //MessageBox.Show("Открыта карта??? " + otit.isOpenMap());
            //MessageBox.Show("Выполнено задание??? " + otit.isTaskDone());
            //MessageBox.Show("около ОлдМана??? " + otit.isNearOldMan());
            //MessageBox.Show("красное слово? " + dialog.isRedSerendbite());
            //MessageBox.Show("есть бутылки?" + server.isBottlesOnLeftPanel());
            //MessageBox.Show("Открыта карта города ??? " + town.isOpenMap());
            //server.OpenDetailInfo();
            //MessageBox.Show("Открыт Detail Info? " + server.isOpenDetailInfo());
            //MessageBox.Show("Штурмовой режим ? " + server.isAssaultMode());
            //MessageBox.Show("Undead " + server.isUndead());
            //MessageBox.Show(" " + server.isBarackTeamSelection());
            //MessageBox.Show("Demon " + server.isDemon());
            //MessageBox.Show("Human " + server.isHuman());
            //MessageBox.Show("isLogout " + server.isLogout());
            MessageBox.Show("system menu? " + server.isOpenTopMenu(13));
            //MessageBox.Show("Переполнение??? " + server.isBoxOverflow());
            //MessageBox.Show("Первый канал??? " + server.CurrentChannel_is_1());
            //MessageBox.Show("есть стим??? " + server.FindWindowSteamBool());
            //MessageBox.Show("мы в диалоге? " + dialog.isDialog());
            //MessageBox.Show("Призван пет? " + pet.isSummonPet());
            //MessageBox.Show("248 вещей в инвентаре? " + server.is248Items());
            //MessageBox.Show("одиночный режим? " + botwindow.isSingleMode());
            //MessageBox.Show(" Wild? " + server.isWild());
            //MessageBox.Show(" Lifeless? " + server.isLifeless());
            //MessageBox.Show(" Undead? " + server.isUndead());
            //MessageBox.Show(" SuperAtk? " + (server.isAtk40()|| server.isAtk39() || server.isAtk38() || server.isAtk37()));
            //MessageBox.Show(" SuperSpeed? " + (server.isAtkSpeed27() || server.isAtkSpeed28() || server.isAtkSpeed29() || server.isAtkSpeed30()) );
            //MessageBox.Show(" HP? " + server.isHP());
            //MessageBox.Show(" Def15? " + server.isDef15());
            //MessageBox.Show(" " + pet.isActivePet());
            //MessageBox.Show(" " + kMarket.isSaleIn());
            //MessageBox.Show(" " + market.isClickPurchase());
            //MessageBox.Show(" " + market.isClickSell());
            //MessageBox.Show(" " + botwindow.isCommandMode());
            //MessageBox.Show(" " + market.isRedBottle());

            //int[] x = { 0, 0, 130, 260, 390, -70, 60, 190, 320, 450 };
            //int[] y = { 0, 0, 0, 0, 0, 340, 340, 340, 340, 340 };

            //int[] aa = new int[17] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 2104613 };
            //bool ff = aa.Contains(725272);
            //int tt = Array.IndexOf(aa, 7272);
            //MessageBox.Show(" " + ff + " " + tt);

            //server.FightToPoint(997 + 25, 160 + 25, 3);
            //server.Turn180();
            //server.TurnUp();
            //server.Turn90R();
            //server.TurnL(1);
            //server.FightToPoint(595, 125, 3);
            //server.TurnDown();
            //server.TurnR(1);
            //server.FightToPoint(545, 110, 3);

            //server.TurnL(1); 
            //server.TurnUp();

            //Hero[1] = server.WhatsHero(1);
            //Hero[2] = server.WhatsHero(2);
            //Hero[3] = server.WhatsHero(3);
            //MessageBox.Show(Hero[1] + " " + Hero[2] + " " + Hero[3]);

            //server.Buff(Hero[1], 1);

            int xx, yy;
            xx = koordX[i - 1];
            yy = koordY[i - 1];
            uint color1;
            uint color2;
            uint color3;
            //int x = 483;
            //int y = 292;
            //int i = 4;

            //int j = 1;
            //PointColor point1 = new PointColor(154 - 5 + xx, 224 - 5 + yy + (j - 1) * 27, 1, 1);       // новый товар в магазине в городе
            //PointColor point2 = new PointColor(151 - 5 + xx, 209 - 5 + yy + (j - 1) * 27, 1, 1);       // новый товар в магазине в городе
            // PointColor point1 = new PointColor(152 - 5 + xx, 250 - 5 + yy + (j - 1) * 27, 1, 1);       // новый товар в магазине в Катовии


            //int xxx = 5;
            //int yyy = 5;
            //PointColor point1 = new PointColor(1042, 551, 1, 1);
            //PointColor point2 = new PointColor(1043, 551, 1, 1);
            PointColor point1 = new PointColor(516 - 5 + xx, 269 - 5 + yy, 0, 0);
            PointColor point2 = new PointColor(517 - 5 + xx, 269 - 5 + yy, 0, 0);
            PointColor point3 = new PointColor(33 - 5 + xx, 695 - 5 + yy, 0, 0);


            color1 = point1.GetPixelColor();
            color2 = point2.GetPixelColor();
            color3 = point3.GetPixelColor();

            //server.WriteToLogFile("цвет " + color1);
            //server.WriteToLogFile("цвет " + color2);

            MessageBox.Show(" " + color1);
            MessageBox.Show(" " + color2);
            //MessageBox.Show(" " + color3);


            //if ((color1 > 2000000) && (color2 > 2000000)) MessageBox.Show(" больше ");


            //string str = "";
            //if (server.isHuman()) str += "Human ";
            //if (server.isWild()) str += "Wild ";
            //if (server.isLifeless()) str += "Life ";
            //if (server.isUndead()) str += "Undead ";
            //if (server.isDemon()) str += "Demon ";

            //MessageBox.Show(str);
        }
    }
}
