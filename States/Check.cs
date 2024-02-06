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
        /// нужно собирать дроп, двигаясь вправо (Кастилия)
        /// </summary>
        private bool NeedToPickUpRight;
        /// <summary>
        /// нужно собирать дроп, двигаясь влево (Кастилия)
        /// </summary>
        private bool NeedToPickUpLeft;
        /// <summary>
        /// если равна true, тогда атака на мобов закончена и надо подбирать дроп (фесо)
        /// </summary>
        private bool NeedToPickUpFeso;
        /// <summary>
        /// день недели по сингапурскому времени
        /// </summary>
        private int WeekDay = 5;
        /// <summary>
        /// для миссии Кастилия. номер следующей точки маршрута
        /// </summary>
        private int NextPointNumber;
        /// <summary>
        /// номер проблемы на предыдущем цикле
        /// </summary>
        private int prevProblem;
        /// <summary>
        /// номер проблемы на предпредыдущем цикле
        /// </summary>
        private int prevPrevProblem;
        /// <summary>
        /// номер окна по порядку (в том порядке, на каком оно находится на экране. Самое левое верхнее левое окно имеет #1)
        /// </summary>
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

        /// <summary>
        /// Конструктор 1
        /// </summary>
        public Check()
        { 
        }

        /// <summary>
        /// ============================= конструктор основной =============================================
        /// </summary>
        /// <param name="numberOfWindow"></param>
        public Check(int numberOfWindow)
        {
            NeedToPickUpRight=false;
            NeedToPickUpLeft=false;
            NeedToPickUpFeso = false;
            NextPointNumber = 0;
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
        }


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
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow(); 
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2(); 
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3(); 
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;
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
                    if (server.FindWindowGEforBHBool()) 
                        return 23;    //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else  
                        return 22;                  //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }
            //если открыто окно Стим в правом нижнем углу
            //if (server.isOpenSteamWindow()) { server.CloseSteamWindow(); server.CloseSteam(); }
            //служба Steam
            //if (server.isSteamService())    return 11;
            
            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //===========================================================================================
            //ворота
            if (dialog.isDialog())
            {
                if (server.isExpedMerch() || server.isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;                         
                if (server.isMissionNotAvailable())
                    return 10;                          //если стоим в воротах Demonic и миссия не доступна
                else
                    return 8;                           //если стоим в воротах Demonic и миссия доступна
            }

            //Mission Lobby
            if (server.isMissionLobby()) return 5;      //22-11

            //Waiting Room //Mission Room 22-11
            if (server.isWaitingRoom()) return 3;      //22-11

            //город или БХ
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                // здесь проверка нужна, чтобы разделить "город" и "работу с убитым первым персонажем".  23-11
                if (!server.isKillFirstHero())
                //!server.isBattleMode() &&
                //!server.isAssaultMode())   //если в городе, но не в боевом режиме и не в режиме атаки
                {
                    if (server.isBH())     //в БХ     
                    {
                        return 4;   // стоим в правильном месте (около ворот Demonic)
                    }
                    else   // в городе, но не в БХ
                    {
                        if (server.isAncientBlessing(1))
                            return 6;
                        else
                            return 9;
                    }
                }
            }
            //в миссии (если убит первый персонаж, то это точно миссия
            if (server.isWork() || server.isKillFirstHero()) return 7;
            //===========================================================================================

            //в логауте
            if (server.isLogout()) return 1;
            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
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
            
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    //botwindow.ActiveWindowBH();   //перед проверкой проблем, активируем окно с ботом. Это вместо ReOpenWindow()

                    //server.ReOpenWindow();
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage1();

                //if (SteamLoaded) dateSteam = DateTime.Now;
                //dateNow = DateTime.Now;
                //if ((dateNow - dateSteam).TotalMinutes > 5) 
                //    numberOfProblem = 31;


                server.WriteToLogFileBH("номер проблемы " + numberOfProblem);

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                        case 4:  //зависли в БХ
                            server.WriteToLogFileBH("зависли в БХ");
                            numberOfProblem = 18; //переходим в стартовый город через системное меню
                            break;
                        //case 17:  //зависли в бараке на стадии выбора группы
                        //    server.WriteToLogFileBH("зависли в бараке на стадии выбора группы");
                        //    numberOfProblem = 18; //переходим в стартовый город через системное меню
                        //    break;

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
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузились ворота Демоник.
                        break;
                    case 7:                                     // поднимаем камеру максимально вверх, активируем пета и переходим к стадии 2
                        server.WriteToLogFileBH("начинаем обработку case 7");
                        //server.MoveMouseDown();
                        botwindow.CommandMode();
                        server.BattleModeOnDem();                   //пробел

                        //botwindow.SingleMode();    //эксперимент 08-02-2022
                        //botwindow.FirstHero();     //эксперимент 08-02-2022
                        botwindow.ThirdHero();          //эксперимент 29-01-2024

                        server.ChatFifthBookmark();
                        Hero[1] = server.WhatsHero(1);
                        Hero[2] = server.WhatsHero(2);
                        Hero[3] = server.WhatsHero(3);

                        //server.messageWindowExtension();     //не актуально 22-11-23

                        //driver.StateActivePetDem();
                        pet.ActivePetDem();                     //ноая функция  22-11

                        
                        server.MaxHeight(12);                      
                        botParam.Stage = 2;
                        break;
                    case 8:                                         //Gate --> Mission Lobby
                        dialog.PressStringDialog(1);                //нажимаем нижнюю строчку (I want to play)
                        break;
                    case 9:                                         //нет наследия. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта

                        botParam.Stage = 5;
                        break;
                    case 10:                                        //миссия не доступна на сегодня (уже прошли)
                        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующий бот для работы
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                        //старый вариант  (сначала идём в город)
                        //dialog.PressOkButton(1);
                        //botwindow.Pause(1000);  //2000
                        //server.GotoSavePoint();
                        //botParam.Stage = 3;
                        //break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;

                    //case 14:
                    //    //driver.StateFromMissionToBarackBH();      // в барак 
                    //    botwindow.ClickSpaceBH();                   //переходим в боевой режим. Если есть в кого стрелять, то стреляем. 
                    //    server.GotoBarack(true);                    //если не в кого стрелять, уходим в барак

                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        //botwindow.PressEsc();                       // нажимаем Esc
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 18:

                        //старый вариант
                        //server.GotoSavePoint();                   // переход в стартовый город (вряд ли работает)
                        //botParam.HowManyCyclesToSkip = 3;

                        //новый вариант                             //переход по телепорту в БХ к воротам
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(3, true);                   // телепорт в Гильдию Охотников (третий телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        //botwindow.Pause(500);
                        server.PressToGateDemonic();
                        prevProblem = 6;                    // делаем предыдущее состояние = город        
                                                            // а иначе программа считает, что мы всё еще застряли в БХ и стоим не там 
                                                            // и опять попадаем сюда же (бесконечный цикл)
                        botParam.HowManyCyclesToSkip = 1; 
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
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
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //стим есть. только что нашли новое окно с игрой
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                            IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) 
                            IsItAlreadyPossibleToUploadNewSteam = 0;
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                            IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
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

        #region  =================================== Demonic Multi Stage 2 (миссия Демоник) =============================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage2()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            //if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;

            //=============================================================================================================
            //в миссии
            if (server.isWork() || server.isKillFirstHero())
            {
                //если розовая надпись в чате "Treasure chest...", значит появился сундук и надо идти в барак и далее стадия 3
                if (server.isTreasureChest()) 
                    return 10;                        //надо собрать дроп, идти в барак и далее - стадия 3
                else
                //    if (server.isBossOrMob() && !server.isMob())
                //    return 4;   //если босс в прицеле, то скилляем
                //else
                    return 3;
            }

            //в логауте
            if (server.isLogout()) return 6;                    // если окно в логауте
            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;
            //в бараке
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            //=============================================================================================================
            if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа
            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            if (server.isTown() && server.isBH() && !server.isKillHero())
                //return 29;
                return 6;

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

                //Random rand = new Random();

                switch (numberOfProblem)
                {
                    //case 1:                                                 // в логауте (при переходе в барак можем попасть в логаут)
                    //    driver.StateFromLogoutToBarackBH();                 // Logout-->Barack   
                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    case 2:                                                 // в бараках
                        server.ReturnToMissionFromBarack();                 // идем из барака обратно в миссию     
                        //botParam.HowManyCyclesToSkip = 2;
                        server.MoveCursorOfMouse();
                        botParam.Stage = 3;
                        //DirectionOfMovement = 1;        //необходимо для стадии 4
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

                        server.BattleModeOnDem();

                        ////если не бафались и в прицеле моб или босс, то скиллуем мушкетерами скиллом E (массуха)
                        ////а потом пробел
                        //if (server.isBossOrMob() && !result)
                        ////if (server.isBossOrMob())         //пробуем обойтись без проверки "баффались или нет"
                        //{
                        //    bool isMobs = server.isMob();   
                        //    //server.MoveCursorOfMouse();
                        //    server.Skill(Hero[1], 1, isMobs);
                        //    server.Skill(Hero[2], 2, isMobs);
                        //    server.Skill(Hero[3], 3, isMobs);
                        //    //server.MoveCursorOfMouse();
                        //}
                        ////Pause(500);


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
                    case 6:                                         
                        server.RemoveSandboxieBH();                 //закрываем песочницу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 10:
                        //если появился сундук
                        server.BattleModeOn();                      //нажимаем пробел, чтобы не убежать от дропа
                        //Pause(5000);
                        server.GotoBarack();                        // идем в барак, чтобы перейти к стадии 3 (открытие сундука и проч.)
                        botwindow.PressEscThreeTimes();
                        //botParam.HowManyCyclesToSkip = 1;
                        break;
                    //===================================================================================
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        //botwindow.PressEsc();                       // нажимаем Esc
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 20:
                        server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    //case 29:                                        //если все убиты
                    //    server.GotoBarack();                        // идем в барак, чтобы потом перейти к стадии 3 (открытие сундука и проч.)
                    //    botParam.HowManyCyclesToSkip = 2;
                    //    break;
                    case 33:
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion ======================================================================================================

        #region  =================================== Demonic Multi Stage 3 (открытие сундука и переход в ворота фесо) ===

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage3()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;

            //==========================================================================================================

            //если диалог (он получается, если тыкнуть в ворота). 
            //а вообще это идеальное место для перехода к стадии 4 (добыча фесо)
            // т.е. после открытия сундука тыкаем в место, где должны открыться второта фесо, а здесь по диалогу проверяем,
            // удалось или нет
            if (dialog.isDialog())
                //return 6;                     //закрываем песочницу и аккаунт
                return 8;                       //диалог в воротах фесо
            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;

            //в миссии
            if (server.isWork())
            {
                //if (GoBarack == 1)              // уже тыкали в сундук 
                //    return 4;                   //можно удалять песочницу и переходить к следующему аккаунту
                //                                //сюда надо вставить тыкание в ворота фесо
                //else
                    return 3;                   //надо тыкать в сундук
            }
            //==========================================================================================================
            //в городе или в БХ
            if (server.isTown())                //если в городе или в БХ, то значит миссия закончилась и нас выкинуло
                return 6;                   //закрываем песочницу и аккаунт
            //в логауте
            if (server.isLogout())          // если окно в логауте
                return 6;                    //закрываем песочницу и аккаунт
            //в бараке
            if (server.isBarackCreateNewHero())         //если стоят на странице создания нового персонажа
                return 6;                    //закрываем песочницу и аккаунт
            if (server.isBarack())                    //если стоят в бараке 
                return 2;                    //закрываем песочницу и аккаунт
            if (server.isBarackWarningYes())
                return 6;                    //закрываем песочницу и аккаунт
            if (server.isBarackTeamSelection())    //если в бараках на стадии выбора группы
                return 6;                    //закрываем песочницу и аккаунт

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
                    //case 1:                                         //в логауте
                    //    driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок
                    //    botParam.HowManyCyclesToSkip = 1;
                    //    break;
                    //====================================================================================================
                    case 2:                         //в бараках   ======== этот пункт нельзя комментить ==========
                                                    // т.к. при возврате в миссию для открытия сундука сработает
                                                    // условие,  (если барак, то RemoveSandboxie)  // тогда мы не сможем открыть сундук
                        driver.StateFromBarackToTownBH();           // идем в город 
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 3:                                             //в миссии, но сундук ещё не открыт
                        //server.ActivePetDem();                          //активируем пета (может он успеет собрать не подобранные вещи)
                        server.OpeningTheChest();                       //тыкаем в сундук и запускаем рулетку
                        //driver.StateActivePetDem();                   //активируем пета (старый вариант)
                        server.MaxHeight(10);                        //чтобы точно было видно вторые ворота
                        //server.TopMenu(3,true);                       //скриншот

                        server.PressOnFesoGate();
                        botwindow.Pause(2000);
                        if (!dialog.isDialog())
                        {
                            botwindow.Pause(3000);
                            server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующего бота в работу
                            botParam.Stage = 1;
                            botParam.HowManyCyclesToSkip = 1;
                        }
                        else
                        {
                            dialog.PressStringDialog(1);
                            dialog.PressStringDialog(1);
                            dialog.PressOkButton(1);
                            if (dialog.isDialog()) dialog.PressOkButton(1);
                            botwindow.Pause(3000);
                            //server.AttackCtrlToLeft();        //старый вариант
                            server.BattleModeOnDem();           //новый вариант
                            server.ActivePetDem();
                            botParam.Stage = 4;
                        }

                        break;
                    //case 4:                                         //уже тыкали в сундук. Тыкаем в ворота с фесо
                    //    server.PressOnFesoGate();
                    //    botwindow.Pause(2000);
                    //    if (!dialog.isDialog())
                    //    {
                    //        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующего бота в работу
                    //        botParam.Stage = 1;
                    //        botParam.HowManyCyclesToSkip = 1;
                    //    }
                    //    else
                    //    {
                    //        dialog.PressStringDialog(1);
                    //        dialog.PressStringDialog(1);
                    //        dialog.PressOkButton(1);
                    //        if (dialog.isDialog()) dialog.PressOkButton(1);
                    //        botwindow.Pause(3000);
                    //        server.AttackCtrlToLeft();
                    //        server.ActivePetDem();
                    //        botParam.Stage = 4;
                    //    }
                    //    break;
                    //case 5:                                         // из БХ
                    //    server.systemMenu(3, true);                 // переход в стартовый город
                    //    botParam.HowManyCyclesToSkip = 3;
                    //    break;
                    case 6:                                         
                        server.RemoveSandboxieBH();                 //закрываем песочницу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    //====================================================================================================
                    //case 7:                                         // в диалоге ворот (выйти в БХ или остаться на месте)
                    //    dialog.PressStringDialog(1);
                    //    //dialog.PressOkButton(1);
                    //    botParam.HowManyCyclesToSkip = 2;
                    //    break;
                    case 8:                                         // появились ворота фесо
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        if (dialog.isDialog()) dialog.PressOkButton(1);
                        botwindow.Pause(3000);
                        server.AttackCtrlToLeft();
                        server.ActivePetDem();
                        botParam.Stage = 4;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    //case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    //    //botwindow.PressEsc();                       // нажимаем Esc
                    //    server.PressYesBarack();
                    //    break;
                    //case 17:                                        // в бараках на стадии выбора группы
                    //    botwindow.PressEsc();                       // нажимаем Esc
                    //    break;
                    //case 20:
                    //    server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                    //                                                //то нажимаем кнопку, чтобы войти обратно в барак
                    //    break;
                    case 33:
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion =======================================================================================================

        #region  =================================== Demonic Multi Stage 4 (фесо) =======================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic на стадии 4 и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage4()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            //if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;
            //==========================================================================================================

            //в миссии фесо
            if (server.isWork())
                if (server.isBattleMode())
                {
                    if (NeedToPickUpFeso == false)
                        return 3;
                    else
                        return 8;
                }
                else
                    //if (NeedToPickUpFeso)         
                    return 5;
                //else
                //    return 4;


            //==========================================================================================================
            //в городе или в БХ
            if (server.isTown())                //если в городе или в БХ, то значит миссия закончилась и нас выкинуло
                return 6;                   //закрываем песочницу и аккаунт
            //в логауте
            if (server.isLogout())          // если окно в логауте
                return 6;                    //закрываем песочницу и аккаунт
            //в бараке
            if (server.isBarackCreateNewHero())         //если стоят на странице создания нового персонажа
                return 6;                    //закрываем песочницу и аккаунт
            if (server.isBarack())                    //если стоят в бараке 
                return 6;                    //закрываем песочницу и аккаунт
            if (server.isBarackWarningYes())
                return 6;                    //закрываем песочницу и аккаунт
            if (server.isBarackTeamSelection())    //если в бараках на стадии выбора группы
                return 6;                    //закрываем песочницу и аккаунт
            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;

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
                case 6:                                         
                    server.RemoveSandboxieBH();                 //закрываем песочницу
                    botParam.Stage = 1;
                    botParam.HowManyCyclesToSkip = 1;
                    break;
                //=============================================================================================================
                case 3:                                         //в комнате с фесо
                    NeedToPickUpFeso = true;
                    break;
                //case 4:                                         // в миссии. уже не бьём мобов
                //    DirectionOfMovement = 1;
                //    server.AttackCtrlToRight();                 //атакуем мобов по направлению вправо, 
                //    NeedToPickUpFeso = true;                    //а когда закончим, можно будет начинать собирать фесо
                //    break;
                case 5:                                         // в миссии. уже не бьём мобов. пора собирать фесо
                    server.PickUpToLeft();
                    break;
                case 8:
                    server.PickUpToRight();
                    break;
                //=============================================================================================================
                case 11:                                         // закрыть службу Стим
                    server.CloseSteam();
                    break;
                case 12:                                         // включить правильную стойку
                    server.ProperFightingStanceOn();
                    server.MoveCursorOfMouse();
                    break;
                case 33:
                    server.CloseError820();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 34:
                    server.AcceptUserAgreement();
                    break;
                case 35:
                    server.CloseErrorSandboxie();
                    break;
                case 36:
                    server.CloseUnexpectedError();
                    //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                    IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                              // а значит смело можно грузить окно еще раз
                    break;
                case 37:
                    server.CloseSteamMessage();
                    IsItAlreadyPossibleToUploadNewWindow = 0;
                    break;

            }
            //}
            //else
            //{
            //    botParam.HowManyCyclesToSkip--;
            //}
        }

        #endregion =======================================================================================================

        #region  =================================== Demonic Multi Stage 5 (берём бафф Наследие древних на мосту ) ======

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDemMultiStage5()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;
            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;

            //==============================================================================
            //если диалог 
            if (dialog.isDialog())
                return 7;                    //получаем бафф "наследие"
            //в миссии
            if (server.isWork())
            {
                if (server.isBridge())
                    if (server.isAncientBlessing(1))
                        return 4;           //на мосту и получили бафф наследие
                    else
                        if (server.isOpenMapBridge())
                            return 5;           //на мосту, но пока не получили бафф наследие. карта открыта
                        else 
                            return 6;           //на мосту, но пока не получили бафф наследие. карта не открыта
            }
            //в городе
            if (server.isTown())
            {
                return 8;                   //в городе. летим на мост
            }
            //==============================================================================
            //в логауте
            if (server.isLogout())
                return 1;                    // если окно в логауте
            //в бараке
            if (server.isBarackCreateNewHero())
                return 20;      //если стоят на странице создания нового персонажа
            if (server.isBarack())
                return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes())
                return 16;        //нажимаем Yes
            if (server.isBarackTeamSelection())
                return 17;      //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ Демоник стадия 3
        /// </summary>
        public void problemResolutionDemMultiStage5()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 3");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDemMultiStage5();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //в бараках  
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа
                        botParam.Stage = 1;
                        break;
                    //===========================================================================================
                    case 4:                                         //на мосту и получили бафф наследие
                        //botwindow.PressEscThreeTimes();
                        //botwindow.Pause(500);
                        //server.MinHeight(3);
                        //server.Teleport(3, true);                   // телепорт в Гильдию Охотников (третий телепорт в списке)
                        server.Logout();
                        botParam.HowManyCyclesToSkip = 2;           
                        botParam.Stage = 1;
                        break;
                    case 5:                                         //на мосту, но пока не получили бафф наследие. карта открыта
                        server.GotoAncientBlessingStatue();
                        break;
                    case 6:                                         //на мосту, но пока не получили бафф наследие. карта не открыта
                        server.MinHeight(10);
                        server.OpenMapBridge();
                        break;
                    case 7:                                         //получаем бафф "наследие"
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        break;
                    case 8:                                         //в городе. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(2500);
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        break;
                    //===========================================================================================
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion =======================================================================================================


        #region  =================================== Castilia Multi Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemCastiliaMultiStage1()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;
            //если нет окна ГЭ
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                    return 24;
                else    //если Стим уже загружен
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else 
                        return 22;          //если нет окна ГЭ в текущей песочнице. Надо грузить окно
            }
            else            //если окно с нужным HWND нашлось
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            //в логауте
            if (server.isLogout()) return 1;
            ////случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23
            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //========================================================================================
            // диалог
            if (dialog.isDialog())
            {
                if (server.isExpedMerch() || server.isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;
                if (server.isMissionCastiliaNotAvailable())
                    return 10;                      //если стоим в воротах Castilia и миссия не доступна
                else
                    return 8;                       //если стоим в воротах Castilia и миссия доступна
            }

            //Mission Lobby
            if (server.isMissionLobby()) return 5;      //22-11

            //Waiting Room //Mission Room 22-11
            if (server.isWaitingRoom()) return 3;      //22-11

            //город или БХ
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                // здесь проверка нужна, чтобы разделить "город" и "работу с убитым первым персонажем".  23-11
                if (!server.isKillFirstHero())
                {
                    if (server.isCastilia())     //в Кастилии     
                        return 4;   // стоим в правильном месте (около зеленой стрелки в миссию)
                    else   // в городе, но не в Кастилии
                        if (server.isAncientBlessing(1))
                            return 6;
                        else
                            return 9;
                }
            }

            //в миссии (если убит первый персонаж, то это точно миссия
            if (server.isWork() || server.isKillFirstHero()) return 7;
            //========================================================================================


            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionCastiliaMultiStage1()
        {
            //server.WriteToLogFileBH("перешли к выполнению стадии 1,  HowManyCyclesToSkip " + botParam.HowManyCyclesToSkip);

            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemCastiliaMultiStage1();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            //server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                        case 4:  //зависли в Кастилии
                            numberOfProblem = 18; //переходим по телепорту снова к зелёной стрелке
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    case 3:                                         // старт миссии      //ок
                        server.MissionStart();                          
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 4:                                         // Castilia --> Green Arrow
                        server.AddBullets();                
                        server.GoToMissionCastilia();
                        break;
                    case 5:                                         //Mission Lobby --> Mission Room
                        server.CreatingMission();
                        break;
                    case 6:                                         // town --> Castilia
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(4, true);                   // телепорт в Кастилию
                        botParam.HowManyCyclesToSkip = 2;             
                        break;
                    case 7:                                     // начало миссии
                                                                // активируем пета и переходим к стадии 2
                        botwindow.CommandMode();
                        server.BattleModeOnDem();                   //пробел

                        Hero[1] = server.WhatsHero(1);
                        Hero[2] = server.WhatsHero(2);
                        Hero[3] = server.WhatsHero(3);

                        server.ActivePetDem();                     //новая функция  22-11

                        botParam.Stage = 2;
                        break;
                    case 8:                                         //Green Arrow --> Mission Lobby
                        dialog.PressStringDialog(1);                //I want to play
                        dialog.PressStringDialog(2);                //Normal Mode
                        break;
                    case 9:                                         //town --> bridge
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        botParam.Stage = 3;
                        break;
                    case 10:                                        //миссия не доступна на сегодня (уже прошли)
                        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующий бот для работы
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 18:
                        //новый вариант                             //переход по телепорту в Кастилии к воротам
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(4, true);                   // телепорт в Гильдию Охотников (третий телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        //server.PressToGateDemonic();
                        prevProblem = 6;                    // делаем предыдущее состояние = город        
                                                            // а иначе программа считает, что мы всё еще застряли в БХ и стоим не там 
                                                            // и опять попадаем сюда же (бесконечный цикл)
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                            IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;   
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;    
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0; 
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Castilia Multi Stage 2 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Castilia и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemCastiliaMultiStage2()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;

            //служба Steam
            if (server.isSteamService()) return 11;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;

            //======================================================================================================
            //в миссии
            if (server.isWork() || 
                server.isKillFirstHero())       //проверить
            {
                if (server.isAssaultMode())     //значит ещё бегут к выбранной точке
                    return 4;                   //тыкаем туда же ещё раз
                if (server.isBattleMode())
                {
                    if (NeedToPickUpRight)
                        return 7;
                    if (NeedToPickUpLeft)
                        return 8;
                    else
                        return 5;               //значит бежим к очередному боссу без Ctrl. пропустить 2-4 цикла
                }
                else
                    return 3;                   //ни пробела, ни Ctrl на нажато. Значит далее бежим с Ctrl.
                                                    //но проверяем через 1-2 сек, не пропал ли Ctrl.
                                                    //Это бы означало, что добежали до места, перебиты все монстры
                                                    //надо собирать лут
            }
            //======================================================================================================
            //служба Steam
            if (server.isSteamService()) return 11;

            //в логауте
            if (server.isLogout()) return 1;                    // если окно в логауте

            //в бараке
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackTeamSelection()) return 17;      //если в бараках на стадии выбора группы
            if (server.isBarackWarningYes()) return 16;         //если нужно нажать кнопку Yes в бараках
            if (server.isBarackCreateNewHero()) return 20;      //если стоят на странице создания нового персонажа

            //в БХ вылетели, значит миссия закончена (находимся в БХ, но никто не убит)
            if (server.isTown() && server.isCastilia()
                //&& !server.isKillHero()) 
                )
                return 29;

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в Castilia
        /// </summary>
        public void problemResolutionCastiliaMultiStage2()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 2");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                    server.ActiveWindow();

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemCastiliaMultiStage2();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                    case 2:
                    case 16:
                    case 17:
                    case 20:
                        botParam.Stage = 1;
                        break;
                    //====================================================================================
                    case 3:                                                 // ни пробела, ни Ctrl на нажато (значит бежали до этого)
                        server.AssaultToNextPoint(NextPointNumber);
                        Pause(2000);
                        if (!server.isAssaultMode())    //если боевой режим пропал, значит пора собирать дроп
                        {
                            NeedToPickUpRight = true;
                            //старый вариант
                            //server.GetDropCastilia(server.GetWaitingTimeForDropPicking(NextPointNumber));
                            //NextPointNumber++;
                            server.BattleModeOnDem();
                        }
                        break;
                    case 4:                                                 // бежим с Ctrl. на всякий случай даём указание бить в ту же точку,
                                                                            // так как бывают случаи, что не все герои бьются, а только 1-2
                        server.AssaultToNextPoint(NextPointNumber);
                        break;
                    case 5:                                                 //стоим на пробеле, NeedToPickUpRight=false, NeedToPickUpLeft=false
                        // бафаемся перед перемещением дальше
                        if ((NextPointNumber == 0) || (NextPointNumber == 3) || (NextPointNumber == 6))
                        {
                            server.MoveCursorOfMouse();
                            server.Buff(Hero[1], 1);
                            server.Buff(Hero[2], 2);
                            server.Buff(Hero[3], 3);
                            botwindow.ActiveAllBuffBH();
                            botwindow.PressEscThreeTimes();
                        }

                        if (NextPointNumber <= 7)        //если еще не дошли до конца миссии 
                        {
                            server.MoveToNextPoint(NextPointNumber);
                            botParam.HowManyCyclesToSkip = 6;
                        }
                        else
                        {
                            server.RemoveSandboxieBH();                     //закрываем песочницу
                            botParam.Stage = 1;
                            botParam.HowManyCyclesToSkip = 1;
                        }
                        break;
                    case 7:                                                 //на пробеле, NeedToPickUpRight=true, NeedToPickUpLeft = false,
                        NeedToPickUpRight = false;  
                        NeedToPickUpLeft = true;
                        server.GetDropCastiliaRight(server.GetWaitingTimeForDropPicking(NextPointNumber));
                        server.BattleModeOnDem();
                        break;
                    case 8:                                                 //на пробеле, NeedToPickUpLeft=true
                        NeedToPickUpLeft = false;
                        NeedToPickUpRight = false;
                        server.GetDropCastiliaLeft(server.GetWaitingTimeForDropPicking(NextPointNumber));
                        server.BattleModeOnDem();
                        NextPointNumber++;
                        break;
                    //====================================================================================
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 29:                                        //если все убиты
                        server.GotoBarack();                        // идем в барак, чтобы потом перейти к стадии 3 (открытие сундука и проч.)
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 33:
                        server.CloseError820();
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                //if (globalParam.TotalNumberOfAccounts == 1)
                //    Pause(6000);
            }
        }

        #endregion ======================================================================================================

        #region  =================================== Castilia Multi Stage 3 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemCastiliaMultiStage3()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //если окно игры открыто на другом компе
            if (server.isOpenGEWindow()) return 37;
            //служба Steam
            if (server.isSteamService()) return 11;
            // если неправильная стойка
            if (server.isBadFightingStance()) return 12;

            //==============================================================================
            //если диалог 
            if (dialog.isDialog())
                return 7;                       //получаем бафф "наследие"
            //в миссии
            if (server.isWork())
            {
                if (server.isBridge())
                    if (server.isAncientBlessing(1))
                        return 4;               //на мосту и получили бафф наследие
                    else
                        if (server.isOpenMapBridge())
                        return 5;               //на мосту, но пока не получили бафф наследие. карта открыта
                    else
                        return 6;               //на мосту, но пока не получили бафф наследие. карта не открыта
            }
            //в городе
            if (server.isTown())
            {
                return 8;                       //в городе. летим на мост
            }
            //==============================================================================
            //в логауте
            if (server.isLogout())
                return 1;                    // если окно в логауте
            //в бараке
            if (server.isBarackCreateNewHero())
                return 20;      //если стоят на странице создания нового персонажа
            if (server.isBarack())
                return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes())
                return 16;        //нажимаем Yes
            if (server.isBarackTeamSelection())
                return 17;      //если в бараках на стадии выбора группы

            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ Демоник стадия 3
        /// </summary>
        public void problemResolutionCastiliaMultiStage3()
        {
            server.WriteToLogFileBH("перешли к выполнению стадии 3");
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemCastiliaMultiStage3();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //в бараках  
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа
                        botParam.Stage = 1;
                        break;
                    //===========================================================================================
                    case 4:                                         //на мосту и получили бафф наследие
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.MaxHeight(10);
                        server.Teleport(4, true);                   // телепорт в Кастилию (четвёртый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        botParam.Stage = 1;
                        break;
                    case 5:                                         //на мосту, но пока не получили бафф наследие. карта открыта
                        server.GotoAncientBlessingStatue();
                        break;
                    case 6:                                         //на мосту, но пока не получили бафф наследие. карта не открыта
                        server.MinHeight(10);
                        server.OpenMapBridge();
                        break;
                    case 7:                                         //получаем бафф "наследие"
                        dialog.PressStringDialog(1);
                        dialog.PressStringDialog(1);
                        break;
                    case 8:                                         //в городе. летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(2500);
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы подгрузилась карта
                        break;
                    //===========================================================================================
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 37:
                        server.CloseSteamMessage();
                        IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
            }
        }

        #endregion =======================================================================================================


        #region  =================================== Delivery Multi Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage1()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с оленем
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город Ребольдо
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage1();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        //server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        //server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;

                    case 6:                                         // town --> олень (диалог)
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 3;
                            break;
                        }
                        if (server.isAuch())
                        {
                            botParam.Stage = 5;
                            break;
                        }
                        if (server.isReboldo())
                        {
                            server.OpenMapReboldo();
                            if ((!server.GotTask()) && (!server.GotTask2()))    //нет задания ни у рудольфа, ни у первого клиента.
                                                                                //значит сегодня уже сходили. переходим к след. аккаунту
                            {
                                server.RemoveSandboxieBH();                 //закрываем песочницу
                                botParam.Stage = 1;
                                botParam.HowManyCyclesToSkip = 1;
                                break;
                            }
                            if (server.GotTask2())
                            {
                                botParam.Stage = 2;
                                break;
                            }
                            server.GoToRudolph();
                        }
                        break;
                    case 8:                                         //Rudolph (dialog --> getting a job)
                        dialog.PressOkButton(10);                   //получили задание у оленя Рудольфа

                        if (!server.GotTaskRudolph())               //такая ситуация возникает, когда остается непогашенное задание с прошлого дня
                        {
                            server.RemoveSandboxieBH();                 //закрываем песочницу
                            botParam.Stage = 1;
                            botParam.HowManyCyclesToSkip = 1;
                        }
                        else
                            botParam.Stage = 2; 

                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                            IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 2 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage2()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с первым клиентом
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город Ребольдо
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage2()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage2();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // town --> первая доставка около фонтана (диалог)
                        botwindow.Pause(500);
                        server.GoToDeliveryNumber1();
                        break;
                    case 8:                                         //доставка 1
                        dialog.PressOkButton(1);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 3;                         //доставили груз 1
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 3 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage3()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с первым клиентом
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город 
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                if (server.isReboldo())
                    return 6;
                else
                    return 7;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage3()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage3();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // Ребольдо --> Коимбра
                        botwindow.Pause(500);
                        server.GoToCoimbra();
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 7:                                         // коимбра --> вторая доставка (диалог)
                        botwindow.Pause(500);
                        server.GoToDeliveryNumber2();
                        break;
                    case 8:                                         // доставка 2 завершена
                        dialog.PressOkButton(1);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 4;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 4 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage4()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            ////случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с клиентом
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город 
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage4()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage4();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // коимбра --> третья доставка (диалог)
                        botwindow.Pause(500);
                        server.GoToDeliveryNumber3();
                        break;
                    case 8:                                         // доставка 3 завершена
                        dialog.PressOkButton(1);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 5;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 5 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage5()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с первым клиентом
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город 
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                if (server.isCoimbra())
                    return 6;
                else
                    return 7;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage5()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage5();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // Коимбра --> Ош
                        botwindow.Pause(500);
                        server.GoToAuch();
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 7:                                         // Auch --> доставка 4 (диалог)
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 5;         //остаемся на этой эе стадии, чтобы еще раз попытаться перейти в Ош
                            break;
                        }
                        if (server.isAuch())
                        {
                            botwindow.Pause(500);
                            server.GoToDeliveryNumber4();
                            break;
                        }
                        break;
                    case 8:                                         // доставка 4 завершена
                        dialog.PressOkButton(1);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 6;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 6 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage6()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            ////случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с клиентом
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город 
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage6()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage6();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // Ош --> доставка 5 (диалог)
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 4;
                            break;
                        }
                        botwindow.Pause(500);
                        server.GoToDeliveryNumber5();
                        break;
                    case 8:                                         // доставка 5 завершена
                        dialog.PressOkButton(2);
                        botwindow.PressEscThreeTimes();
                        botParam.Stage = 7;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Delivery Multi Stage 7 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemDeliveryMultiStage7()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог 
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город 
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                if (server.isAuch())
                    return 6;
                else
                    return 7;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionDeliveryMultiStage7()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemDeliveryMultiStage7();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:     //логаут
                    case 2:     //в бараке
                    case 16:     //в бараке
                    case 17:     //в бараке
                    case 20:     //в бараке
                    case 22:     // нет окна
                    case 23:     // нет окна
                    case 24:     // нет окна
                        botParam.Stage = 1;
                        break;
                    case 6:                                         // Ош --> Reboldo
                        botwindow.Pause(500);
                        server.GoToReboldo();
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 7:                                         // Reboldo --> Rudolph (диалог)
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 3;
                            break;
                        }
                        if (server.isAuch())
                        {
                            botParam.Stage = 5;
                            break;
                        }
                        if (server.isReboldo())
                        {
                            server.OpenMapReboldo();
                            if ((!server.GotTask()) && (!server.GotTask2()))    //нет задания ни у рудольфа, ни у первого клиента.
                                                                                //значит сегодня уже сходили. переходим к след. аккаунту
                            {
                                server.RemoveSandboxieBH();                 //закрываем песочницу
                                botParam.Stage = 1;
                                botParam.HowManyCyclesToSkip = 1;
                                break;
                            }
                            if (server.GotTask2())
                            {
                                botParam.Stage = 2;
                                break;
                            }
                            server.GoToRudolph2();
                        }
                        break;
                    case 8:                                         // получаем награду у Рудольфа
                        dialog.PressOkButton(5);
                        botwindow.PressEscThreeTimes();

                        server.RemoveSandboxieBH();                 //закрываем песочницу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                    // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion


        #region  =================================== Bridge Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage1()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //служба Steam
            if (server.isSteamService()) return 11;
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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }
            //случайно зашли в магазин Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;                         //  22-11-23
            //в логауте
            if (server.isLogout()) return 1;
            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;
            //=========================================================================================================
            //ворота
            if (dialog.isDialog())
            {
                if (server.isExpedMerch() || server.isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;
                else
                    return 8;                       //если диалог, связанный с получением задания
            }

            //город Ребольдо
            if (server.isTown())
            {
                return 6;
            }

            if (server.isWork())
                if (server.isBridge())
                    return 7;
                else
                    return 9;
            //=========================================================================================================
            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы
            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage1();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        //server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        //server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;

                    case 6:                                         // 
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);

                        //если есть ивент, то бежим к квестодателю за заданием

                        //если нет ивента, то на стадию 4 (там летим на мост)

                        botParam.Stage = 4;

                        break;
                    case 7:     //если уже на мосту
                        botParam.Stage = 5;
                        break;
                    case 8:                                         //квестодатель (dialog --> getting a job)


                        break;
                    case 9:                                         //в миссии
                        botParam.Stage = 5;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                                                                //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                        IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Bridge Stage 2 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage2()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с оленем
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город Ребольдо
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage2()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage2();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        //server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        //server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;

                    case 6:                                         // town --> олень (диалог)
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 3;
                            break;
                        }
                        if (server.isAuch())
                        {
                            botParam.Stage = 5;
                            break;
                        }
                        if (server.isReboldo())
                        {
                            server.OpenMapReboldo();
                            if ((!server.GotTask()) && (!server.GotTask2()))    //нет задания ни у рудольфа, ни у первого клиента.
                                                                                //значит сегодня уже сходили. переходим к след. аккаунту
                            {
                                server.RemoveSandboxieBH();                 //закрываем песочницу
                                botParam.Stage = 1;
                                botParam.HowManyCyclesToSkip = 1;
                                break;
                            }
                            if (server.GotTask2())
                            {
                                botParam.Stage = 2;
                                break;
                            }
                            server.GoToRudolph();
                        }
                        break;
                    case 8:                                         //Rudolph (dialog --> getting a job)
                        dialog.PressOkButton(10);                   //получили задание у оленя Рудольфа

                        if (!server.GotTaskRudolph())               //такая ситуация возникает, когда остается непогашенное задание с прошлого дня
                        {
                            server.RemoveSandboxieBH();                 //закрываем песочницу
                            botParam.Stage = 1;
                            botParam.HowManyCyclesToSkip = 1;
                        }
                        else
                            botParam.Stage = 2;

                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                                                                //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                        IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Bridge Stage 3 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage3()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с оленем
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //город Ребольдо
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();             //27-10-2021
                return 6;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage3()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage3();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        //server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        //server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;

                    case 6:                                         // town --> олень (диалог)
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        if (server.isCoimbra())
                        {
                            botParam.Stage = 3;
                            break;
                        }
                        if (server.isAuch())
                        {
                            botParam.Stage = 5;
                            break;
                        }
                        if (server.isReboldo())
                        {
                            server.OpenMapReboldo();
                            if ((!server.GotTask()) && (!server.GotTask2()))    //нет задания ни у рудольфа, ни у первого клиента.
                                                                                //значит сегодня уже сходили. переходим к след. аккаунту
                            {
                                server.RemoveSandboxieBH();                 //закрываем песочницу
                                botParam.Stage = 1;
                                botParam.HowManyCyclesToSkip = 1;
                                break;
                            }
                            if (server.GotTask2())
                            {
                                botParam.Stage = 2;
                                break;
                            }
                            server.GoToRudolph();
                        }
                        break;
                    case 8:                                         //Rudolph (dialog --> getting a job)
                        dialog.PressOkButton(10);                   //получили задание у оленя Рудольфа

                        if (!server.GotTaskRudolph())               //такая ситуация возникает, когда остается непогашенное задание с прошлого дня
                        {
                            server.RemoveSandboxieBH();                 //закрываем песочницу
                            botParam.Stage = 1;
                            botParam.HowManyCyclesToSkip = 1;
                        }
                        else
                            botParam.Stage = 2;

                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                                                                //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                        IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Bridge Stage 4 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage4()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;
            
            //ворота
            if (dialog.isDialog())
            {
                return 8;                       //если диалог с квестодателем на мосту
            }



            //город Ребольдо
            if (server.isTown())
            {
                return 6;
            }

            //если на мосту
            if (server.isWork())
            {
                if (server.isOpenMapBridge())
                    return 4;
                else
                    return 5;
            }

            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage4()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage4();

                switch (numberOfProblem)
                {
                    case 1:                                         //логаут
                    case 2:                                         //если стоят в бараке
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят в бараке на странице создания нового персонажа
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим
                    case 24:                                        //если нет стима, значит удалили песочницу

                        botParam.Stage = 1;
                        break;
                    case 4:     //мы на мосту и открыта карта Alt+Z, значит идём к Терезии пополнять Activity
                        server.GotoTeresia();
                        break;
                    case 5:
                        server.MinHeight(10);
                        server.OpenMapBridge();
                        break;
                    case 6:                                         // town --> bridge
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botParam.HowManyCyclesToSkip = 4;           
                        break;
                    case 8:                                         //Терезия. Получение Activity
                        dialog.PressStringDialog(1);
                        dialog.PressOkButton(1);
                        botParam.Stage = 5;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Bridge Stage 5 (мост. вход в миссию) ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage5()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //==================================================================================
            //диалог
            if (dialog.isDialog())
            {
                if (server.isActivityOut())
                    return 15;
                else
                    return 8;                       //если диалог с квестодателем на мосту
            }
            //город Ребольдо
            if (server.isTown())
            {
                return 6;
            }
            //если на мосту
            if (server.isWork())
            {
                if (server.isBridge())
                {
                    if (server.isOpenMapBridge())
                        return 4;
                    else
                        return 5;
                }
                else
                    return 7;
            }
            //==================================================================================

            //в логауте
            if (server.isLogout()) return 1;
            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage5()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage5();

                switch (numberOfProblem)
                {
                    case 1:                                         //логаут
                    case 2:                                         //если стоят в бараке
                    case 6:                                         // если в городе. не должно быть
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят в бараке на странице создания нового персонажа
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;
                    case 4:     //мы на мосту и открыта карта Alt+Z, значит идём к солдату получать задание и выполнять миссию
                        server.GotoIndividualRaid();
                        break;
                    case 5:
                        //server.MinHeight(10);
                        server.OpenMapBridge();
                        break;
                    case 7:                                         // в миссии. начало
                        server.BeginAttack(WeekDay);                      
                        botParam.Stage = 6;
                        break;
                    case 8:                                         //Солдат. Диалог. Получение задания
                        server.GotoIndividualMission(1, 3, WeekDay);         //ранг (1-7), тип миссии (1 - плюсовая, 3 - обычная), и день недели 
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 15:                                         // закончилась активность
                        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующего бота в работу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts == 1) Pause(2000);
                //server.WriteToLogFileBH("Пауза 1000");
                //server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== Bridge Stage 6 (прохождение миссии) ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBridgeMultiStage6()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //диалог
            if (dialog.isDialog())
                if (server.isActivityOut())
                    return 15;

            //город Ребольдо (не должно)
            if (server.isTown())
            {
                return 6;
            }
            //=====================================================================================================
            //если в миссии
            if (server.isWork())
            {
                if (server.isBridge())      // если на мосту, значит мисссия завершилась и нас выкинуло из неё
                {
                    return 4;
                }
                else  //не на мосту, значит в миссии
                {
                    if (server.isAssaultMode())
                        return 9;
                    if (server.isBattleMode())
                        if (server.isBossOrMobBridge())
                            return 8;
                        else
                            return 7;
                    else
                        return 10;  //не боевой и не ассаульт режим
                }
            }

            //=====================================================================================================
            //в логауте
            if (server.isLogout()) return 1;

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionBridgeMultiStage6()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemBridgeMultiStage6();

                switch (numberOfProblem)
                {
                    case 1:                                         //логаут
                    case 2:                                         //если стоят в бараке
                    case 6:                                         //если в городе. не должно быть
                    case 16:                                        //в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        //в бараках на стадии выбора группы
                    case 20:                                        //если стоят в бараке на странице создания нового персонажа
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;
                    case 4:
                        botParam.Stage = 5;
                        break;
                    case 7:     //миссия.завершение
                        server.GoToChest(WeekDay);                        

                        // летим на мост
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт на мост (первый телепорт в списке)        
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        botParam.HowManyCyclesToSkip = 2;

                        botParam.Stage = 5;
                        break;
                    case 8:
                        server.SkillAll();
                        break;
                    case 9:
                        server.SkillAll();
                        break;
                    case 10:
                        server.BattleModeOnDem();
                        botParam.HowManyCyclesToSkip = 4;
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 15:                                         // на мосту в диалоге с солдатом. Закончилась активность
                        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующего бота в работу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts == 1) Pause(2000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion


        #region  =================================== PureOtiteNew Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в миссии Pure Otite и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemPureOtiteNewStage1()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();
            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;
            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;
            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;
            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;
            //служба Steam
            if (server.isSteamService()) return 11;
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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }
            //в логауте
            if (server.isLogout()) return 1;
            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;
            //=================================================================================================
            // диалог
            if (dialog.isDialog())      
            {
                if (server.isExpedMerch() || server.isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;
                else
                    return 8;           //у мамона            
            }

            //город Ребольдо
            if (server.isTown())
            {
                if (server.isReboldo()) 
                    return 6;   //в ребольдо
                else
                {       //В ЛосТолдосе
                    if (otit.isNearOldMan())
                    {
                        if (otit.isTaskDone()) return 5;    //в Лос Толдосе и задание выполнено
                        if (otit.isGetTask()) return 4;     //в Лос Толдосе, задание уже получено, но не выполнено
                        else return 3;                      //в Лос Толдосе, задание не получено и не выполнено
                    }
                    else return 10;         //в Лос Толдосе, но не около ОлдМэна
                }
             
            }

            if (server.isWork())
                if (server.isDesertedQuay())
                    return 7;
                else
                    return 9;   //в миссии
            //=================================================================================================
            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в миссии Pure Otite
        /// </summary>
        public void problemResolutionPureOtiteNewStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemPureOtiteNewStage1();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        //server.WriteToLogFileBH("case 1");
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        //server.WriteToLogFileBH("case 2");
                        driver.StateFromBarackToTownBH();           // barack --> town              //сделано
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    case 3: //в Лос Толдосе, задание не получено и не выполнено
                        botParam.Stage = 2;
                        break;
                    case 4: //в Лос Толдосе, задание уже получено, но не выполнено
                        botParam.Stage = 3;
                        break;
                    case 5: //в Лос Толдосе и задание выполнено
                        botParam.Stage = 5;
                        break;
                    case 6:        //в ребольдо
                        botwindow.PressEscThreeTimes();
                        botwindow.Pause(500);
                        server.Teleport(1, true);                   // телепорт к Мамону        
                        botParam.HowManyCyclesToSkip = 4;           // даём время, чтобы загрузилась местность
                        break;
                    case 7:     //у Мамона
                        driver.FromMamonsToMamonsDialog();          //Mamons --> MaMons(Dialog)
                        break;
                    case 8:    //диалог у Мамона
                        dialog.PressStringDialog(1);
                        break;
                    case 9:                                         //в миссии
                        botParam.Stage = 4;
                        break;
                    case 10:                                        //в Лос Толдосе, но не около старика. Идём к старику
                        botwindow.PressEscThreeTimes();
                        botwindow.FirstHero();
                        Pause(500);
                        otit.GoToOldManMulti();
                        break;
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                                                                //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                        IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  =================================== PureOtiteNew Stage 2 (Лос. Толдос. Задание не получено) ==========================

        /// <summary>
        /// проверяем, если ли проблемы при работе в миссии Pure Otite и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemPureOtiteNewStage2()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            //if (server.isExpedMerch()) return 12;                         //  22-11-23

            //в логауте
            if (server.isLogout()) return 1;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //ворота
            if (dialog.isDialog())      //диалог со стариком
            {
                return 8;
            }

            //город Ребольдо
            if (server.isTown())
            {
                if (server.isReboldo())
                    return 6;   //в ребольдо
                else
                {                                       //В ЛосТолдосе    
                    if (otit.isNearOldMan())
                        return 3;                       //в Лос Толдосе, около старика. Задание не получено
                    else 
                        return 4;                       //в Лос Толдосе, но не около ОлдМэна
                }

            }
            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в миссии Pure Otite
        /// </summary>
        public void problemResolutionPureOtiteNewStage2()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemPureOtiteNewStage2();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //если стоят в бараке
                    case 4:                                         //в Лос Толдосе, но не около ОлдМэна
                    case 6:                                         //в ребольдо
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа,
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим, есть окно с игрой
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;

                    case 3:                                         //в Лос Толдосе, около старика. задание не получено
                        otit.PressOldMan();                         // OldMan --> OldMan (dialog)
                        break;
                    case 8:                                         //диалог со стариком. получение задания
                        otit.GetTask();
                        botParam.Stage = 3;
                        break;
                    case 11:                                        // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 19:                                        // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  ====================== PureOtiteNew Stage 3 (Лос. Толдос. Задание получено. Идём в миссию) ======================

        /// <summary>
        /// проверяем, если ли проблемы при работе в миссии Pure Otite и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemPureOtiteNewStage3()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //в логауте
            if (server.isLogout()) return 1;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //ворота
            if (dialog.isDialog())      //диалог со стариком. направляемся в миссию
            {
                return 8;
            }

            //город Ребольдо
            if (server.isTown())
            {
                if (server.isReboldo())
                    return 6;   //в ребольдо
                else
                {                                       //В ЛосТолдосе    
                    if (otit.isNearOldMan())
                        return 3;                       //в Лос Толдосе, около старика. Задание получено
                    else
                        return 4;                       //в Лос Толдосе, но не около ОлдМэна
                }

            }

            // в миссии. только что вошли
            if (server.isWork())
                return 7;


            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в миссии Pure Otite
        /// </summary>
        public void problemResolutionPureOtiteNewStage3()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemPureOtiteNewStage3();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //если стоят в бараке
                    case 4:                                         //в Лос Толдосе, но не около ОлдМэна
                    case 6:                                         //в ребольдо
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа,
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим, есть окно с игрой
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;

                    case 3:                                         //в Лос Толдосе, около старика. задание получено
                        otit.PressOldMan();                         // OldMan --> OldMan (dialog)
                        break;
                    case 7:                                         // в миссии. только что вошли
                        driver.FromMissionToFight();                //Mission-- > Mission (Fight begin)
                        botParam.HowManyCyclesToSkip = 2;
                        botParam.Stage = 4;
                        break;
                    case 8:                                         //диалог со стариком. направляемся в миссию
                        otit.EnterToTierraDeLosMuertus();
                        break;
                    case 11:                                        // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 19:                                        // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  ===================== PureOtiteNew Stage 4 (Лос. Толдос. Выполнение миссии) ===================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в миссии Pure Otite и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemPureOtiteNewStage4()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //в логауте
            if (server.isLogout()) return 1;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            // в миссии.
            if (server.isWork())
                if (!otit.isOpenMap())      //карта не открыта
                    if (otit.isTaskDone())
                        return 3;           //если карта закрыта и задание уже выполнено
                    else
                        return 4;           //если карта закрыта, но задание еще не выполнено
                else                        // если карта уже открыта, то смотрим далее
                {
                    if (!otit.isTaskDone())
                        if (!server.isAssaultMode())
                            return 5;       //бежим к следующей точке маршрута с атакой
                        else
                            return 0;       //ничего не делаем, так как уже бежим и атакуем
                    else
                        return 6;           //карта открыта, но задание уже выполнено
                }



            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в миссии Pure Otite
        /// </summary>
        public void problemResolutionPureOtiteNewStage4()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemPureOtiteNewStage4();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //если стоят в бараке
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа,
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим, есть окно с игрой
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;
                    //============================================================================================
                    case 3:                                         //если карта закрыта и задание уже выполнено                            
                        driver.TeleportToMamut();
                        botParam.HowManyCyclesToSkip = 1;
                        botParam.Stage = 1;
                        break;
                    case 4:                                         //если карта закрыта, но задание еще не выполнено
                        botwindow.PressEscThreeTimes();             //открываем карту в миссии
                        server.TopMenu(12, 2, true);
                        break;
                    case 5:                                         //если карта уже открыта, бежим к следующей точке маршрута с атакой
                        otit.GotoNextPointRouteMulti();             //Mission(Fight)-- > Fight To Next Point
                        break;
                    case 6:                                         //карта открыта, но задание уже выполнено
                        driver.FightIsFinished();                   //бежим к следующей точке маршрута без атаки
                        break;
                    //============================================================================================
                    case 11:                                        // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 19:                                        // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region  ==================== PureOtiteNew Stage 5 (Лос. Толдос. Задание выполнено. Получаем награду) ======================

        /// <summary>
        /// проверяем, если ли проблемы при работе в миссии Pure Otite и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemPureOtiteNewStage5()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

            //если нет окна
            if (!server.isHwnd())        //если нет окна с hwnd таким как в файле HWND.txt
            {
                if (!server.FindWindowSteamBool())  //если Стима тоже нет
                {
                    return 24;
                }
                else    //если Стим уже загружен
                {
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //в логауте
            if (server.isLogout()) return 1;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //ворота
            if (dialog.isDialog())      //диалог со стариком. получаем награду
            {
                return 8;
            }

            //город Ребольдо
            if (server.isTown())
            {
                if (server.isReboldo())
                    return 6;   //в ребольдо
                else
                {                                       //В ЛосТолдосе    
                    if (otit.isNearOldMan())
                        return 3;                       //в Лос Толдосе, около старика. задание выполнено
                    else
                        return 4;                       //в Лос Толдосе, но не около ОлдМэна
                }

            }

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в миссии Pure Otite
        /// </summary>
        public void problemResolutionPureOtiteNewStage5()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemPureOtiteNewStage5();

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:                                         //в логауте
                    case 2:                                         //если стоят в бараке
                    case 4:                                         //в Лос Толдосе, но не около ОлдМэна
                    case 6:                                         //в ребольдо
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                    case 17:                                        // в бараках на стадии выбора группы
                    case 20:                                        //если стоят на странице создания нового персонажа,
                    case 22:                                        //если нет окна ГЭ в текущей песочнице
                    case 23:                                        //есть окно стим, есть окно с игрой
                    case 24:                                        //если нет стима, значит удалили песочницу
                        botParam.Stage = 1;
                        break;
                    //============================================================================================
                    case 3:                                         //в Лос Толдосе, около старика. задание выполнено
                        otit.PressOldMan();                         // OldMan --> OldMan (dialog)
                        break;
                    case 8:                                         //диалог со стариком. получаем награду
                        otit.TakePureOtite();                       //Oldman(Dialog) --> Get Reward
                        botParam.Stage = 1;
                        break;
                    case 11:                                        // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    //============================================================================================
                    case 19:                                        // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 33:                                        //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                Pause(1000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion


        #region  =================================== Farm Stage 1 ==============================================

        /// <summary>
        /// проверяем, если ли проблемы при работе в Demonic и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemFarmStage1()
        {
            //если открыто окно Стим
            if (server.isOpenSteamWindow()) server.CloseSteamWindow();
            if (server.isOpenSteamWindow2()) server.CloseSteamWindow2();
            if (server.isOpenSteamWindow3()) server.CloseSteamWindow3();
            if (server.isOpenSteamWindow4()) server.CloseSteamWindow4();

            //если ошибка 820 (зависло окно ГЭ при загрузке)
            if (server.isError820()) return 33;

            //если выскочило сообщение о пользовательском соглашении
            if (server.isNewSteam()) return 34;

            //если ошибка Sandboxie 
            if (server.isErrorSandboxie()) return 35;

            //если ошибка Unexpected
            if (server.isUnexpectedError()) return 36;

            //служба Steam
            if (server.isSteamService()) return 11;

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
                    if (server.FindWindowGEforBHBool())
                        return 23;          //нашли окно ГЭ в текущей песочнице (и перезаписали Hwnd в функции FindWindowGEforBHBool)
                    else
                        return 22;          //если нет окна ГЭ в текущей песочнице
                }
            }
            else            //если окно с нужным HWND нашлось
            {
                if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
            }

            //случайно зашли в магазин Expedition Merchant в городе
            if (server.isExpedMerch()) return 12;                         //  22-11-23

            //в логауте
            if (server.isLogout()) return 1;

            // если неправильная стойка
            if (server.isBadFightingStance()) return 19;

            //============================================================================================================
            //диалог
            if (dialog.isDialog())
            {
                if (server.isExpedMerch() || server.isFactionMerch())  //случайно зашли в магазин Expedition Merchant или в Faction Merchant в Rebo
                    return 12;
                else
                    return 8;        //Farm Manager               
            }

            //город
            if (server.isTown())
            {
                if (server.isReboldo())
                    return 6;               
                if (server.isUstiar())
                    return 5;
            }

            //на ферме
            if (server.isWork())
                if (server.isRewardAvailable())
                    return 7;
                else
                    return 9;

            //============================================================================================================

            //в бараке
            if (server.isBarackCreateNewHero()) return 20;      //если стоят в бараке на странице создания нового персонажа
            if (server.isBarack()) return 2;                    //если стоят в бараке 
            if (server.isBarackWarningYes()) return 16;
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы


            //если проблем не найдено
            return 0;
        }

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionFarmStage1()
        {
            if (botParam.HowManyCyclesToSkip <= 0)      // проверяем, нужно ли пропустить данное окно на этом цикле.
            {
                if (server.isHwnd())        //если окно с hwnd таким как в файле HWND.txt есть, то оно сдвинется на своё место
                {
                    server.ActiveWindow();
                    Pause(1000);                        //пауза, чтобы перед оценкой проблем. Окно должно устаканиться.        10-11-2021 
                }

                //проверили, какие есть проблемы (на какой стадии находится бот)
                int numberOfProblem = NumberOfProblemFarmStage1();

                //если зависли в каком-либо состоянии, то особые действия
                if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)
                {
                    switch (numberOfProblem)
                    {
                        case 1:     //зависли в логауте
                        case 23:    //загруженное окно зависло и не смещается на нужное место (окно ГЭ есть, но isLogout() не срабатывает)
                            server.WriteToLogFileBH("зависли в состоянии 1 или 23");
                            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
                            break;
                    }
                }
                else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

                Random rand = new Random();

                switch (numberOfProblem)
                {
                    case 1:
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //ок   //сделано
                        botParam.HowManyCyclesToSkip = 2;  //1
                        break;
                    case 2:
                        server.FromBarackToTown(3);
                        botParam.HowManyCyclesToSkip = 3;  //2
                        break;
                    //=========================================================================================
                    case 5:                                         // в Юстиаре
                        botwindow.PressEscThreeTimes();
                        server.GotoFarmManager();
                        break;
                    case 6:                                         // в Ребольдо
                        server.GoToUstiar();
                        Pause(12000);
                        break;
                    case 7:                                         // на ферме. доступна награда
                        server.GetRreward();
                        Pause(1000);
                        server.RemoveSandboxieBH();                 //закрываем песочницу и берём следующего бота в работу
                        botParam.Stage = 1;
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 8:                                         //диалог (Farm Manager --> Farm)
                        dialog.PressStringDialog(1);
                        if (dialog.isDialog()) dialog.PressOkButton(1);
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    case 9:                                         //на ферме. пока не доступна награда
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    //===========================================================================================
                    case 11:                                         // закрыть службу Стим
                        server.CloseSteam();
                        break;
                    case 12:                                         // закрыть магазин 
                        server.CloseMerchReboldo();
                        break;
                    case 16:                                        // в бараках на стадии выбора группы и табличка Да/Нет
                        server.PressYesBarack();
                        break;
                    case 17:                                        // в бараках на стадии выбора группы
                        botwindow.PressEsc();                       // нажимаем Esc
                        break;
                    case 19:                                         // включить правильную стойку
                        server.ProperFightingStanceOn();
                        server.MoveCursorOfMouse();
                        break;
                    case 20:
                        server.ButtonToBarack();                    //если стоят на странице создания нового персонажа,
                                                                    //то нажимаем кнопку, чтобы войти обратно в барак
                        break;
                    case 22:
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (IsItAlreadyPossibleToUploadNewWindow == 0)     //30.10.2023
                        {
                            server.RunClientDem();                      // если нет окна ГЭ, но загружен Steam, то запускаем окно ГЭ
                            botParam.HowManyCyclesToSkip = rand.Next(6, 8);   //30.10.2023    //пропускаем следующие 6-8 циклов
                            IsItAlreadyPossibleToUploadNewWindow = this.numberOfWindow;
                        }
                        break;
                    case 23:                                    //есть окно стим
                                                                //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) 
                        IsItAlreadyPossibleToUploadNewWindow = 0;           //если только что нашли новое окно с игрой, значит можно грузить другое окно
                        break;
                    case 24:                //если нет стима, значит удалили песочницу
                                            //и надо заново проинициализировать основные объекты (но не факт, что это нужно)
                        if (IsItAlreadyPossibleToUploadNewSteam == 0)
                        {
                            botwindow = new botWindow(numberOfWindow);
                            ServerFactory serverFactory = new ServerFactory(botwindow);
                            this.server = serverFactory.create();
                            this.globalParam = new GlobalParam();
                            this.botParam = new BotParam(numberOfWindow);
                            //************************ запускаем стим ************************************************************
                            server.runClientSteamBH();              // если Steam еще не загружен, то грузим его
                            server.WriteToLogFileBH("Запустили клиент стим в окне " + numberOfWindow);
                            botParam.HowManyCyclesToSkip = rand.Next(2, 4);        //пропускаем следующие циклы (от 2 до 4)
                            IsItAlreadyPossibleToUploadNewSteam = this.numberOfWindow;
                        }
                        break;
                    case 31:
                        server.CloseSandboxieBH();              //закрываем все проги в песочнице
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewSteam) IsItAlreadyPossibleToUploadNewSteam = 0;
                        if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        break;
                    case 33:                            //ошибка 820. нажимаем два раза на кнопку Ок
                        server.CloseError820();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                    case 34:
                        server.AcceptUserAgreement();
                        break;
                    case 35:
                        server.CloseErrorSandboxie();
                        break;
                    case 36:
                        server.CloseUnexpectedError();
                        //if (this.numberOfWindow == IsItAlreadyPossibleToUploadNewWindow) IsItAlreadyPossibleToUploadNewWindow = 0;
                        IsItAlreadyPossibleToUploadNewWindow = 0; //если окна грузятся строго по одному, то ошибка будет именно в загружаемом окне
                                                                  // а значит смело можно грузить окно еще раз
                        break;
                }
            }
            else
            {
                botParam.HowManyCyclesToSkip--;
                if (globalParam.TotalNumberOfAccounts == 1) Pause(2000);
                server.WriteToLogFileBH("Пауза 1000");
                server.WriteToLogFileBH("пропускаем " + botParam.HowManyCyclesToSkip + " ходов");
            }
        }

        #endregion

        #region Гильдия охотников BH (Infinity Multi)

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
            if (BHdialog.isGateBH()) return 7;                    //если стоим в воротах, начальное состояние            //*
            if (BHdialog.isGateBH1()) return 8;                   //ворота. дневной лимит миссий еще не исчерпан            //*
            if (BHdialog.isGateBH3()) return 9;                   //ворота. дневной лимит миссий уже исчерпан
            if (BHdialog.isGateLevelLessThan11()) return 10;      //ворота. уровень миссии меньше 10                      //*
            if (BHdialog.isGateLevelFrom10to19()) return 19;      //ворота. уровень миссии от 10 до 19                    //*
            if (BHdialog.isGateLevelAbove20()) return 25;         //ворота. уровень миссии больше 20
            if (BHdialog.isInitialize()) return 26;               //ворота. форма, где надо вводить слово Initialize      //*


            //город или БХ
            if (server.isTown())
            {
                botwindow.PressEscThreeTimes();
                if (!server.isBattleMode() && !server.isAssaultMode())   //если в городе, но не в боевом режиме и не в режиме атаки
                {
                    if (server.isBH())     //в БХ   //*
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
                    if (server.isRouletteBH())                                        //если крутится рулетка    //*
                    {
                        return 20;                                                    // подбор дропа
                    }
                    else
                    {
                        //if (!server.isAtakBH()) return 14;                            //идем в барак (а можем и в БХ)
                        if (!server.isAssaultMode()) return 14;                       //если уже не атакуем босса, то идём в барак 
                                                                                      //если находимся в миссии, но уже не в начале и не атакуем босса и не крутится рулетка 
                                                                                      //(значит бой окончен, либо заблудились и надо выходить из миссии) 
                    }
                }
            }

            //в логауте
            if (server.isLogout()) return 1;               // если окно в логауте       //*

            //в бараке
            if (server.isBarack())                         //если стоят в бараке        //*
            {
                if (NumberOfState == 0)                      // если в БХ еще сегодня не ходили, то по-любому начинаем в городе
                                                             // ибо можем попасть не в ту точку БХ (например в точку около ворот Демоник)
                {
                    NumberOfState = 1;                        //отметка, что первый заход в бараки уже был
                    return 2;                                  //начинаем в Ребольдо
                }                             
                else if (server.isBarackLastPoint())            //если в БХ уже ходили и старое место тоже в БХ
                { return 16; }                                  //начинаем со старого места в БХ  
                else
                {  return 2; }
            }
            if (server.isBarackTeamSelection()) return 17;    //если в бараках на стадии выбора группы   

            //в миссии, но убиты
            if (server.isKillAllHero()) return 29;            // если убиты все            //*
            if (server.isKillHero()) return 30;               // если убиты не все          //*

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
                    //Pause(500);
                    Pause(3000);
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
                        driver.StateFromLogoutToBarackBH();         // Logout-->Barack   //*
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 2:
                        driver.StateFromBarackToTownBH();           // идем в город    //*
                        botParam.HowManyCyclesToSkip = 2;
                        break;
                    //case 3:
                    //    //временная заплатка. вместо продажи аккаунта закрываем песочницу и переходим к следующему аккаунту
                    //    botParam.StatusOfSale = 0;
                    //    server.RemoveSandboxieBH();

                    //    //проверено. работает
                    //    //int result = globalParam.Infinity;
                    //    //if (result >= 200) result = 52;
                    //    //botParam.NumberOfInfinity = result;
                    //    //globalParam.Infinity = result + 1;
                    //    //server.CloseSandboxieBH();
                    //    //server.MoveMouseDown();



                    //    //driver.StateGotoTradeStep1BH();             // BH-->Town (первый этап продажи)
                    //    //botParam.HowManyCyclesToSkip = 2;
                    //    break;
                    case 4:
                        driver.StateFromBHToGateBH();               // BH --> Gate   //*
                        break;
                    //case 5:
                    //    driver.StateGotoTradeStep2BH();             // если стоят в городе и надо продаваться, то второй этап продажи
                    //    break;
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
                    //case 11:
                    //    driver.StateGotoTradeStep3BH();             // третий этап продажи
                    //    break;
                    //case 12:
                    //    driver.StateGotoTradeStep4BH();             // четвертый этап продажи
                        //break;
                    case 13:
                        driver.StateFromMissionToFightBH();         // Mission--> Fight!!!
                        break;
                    case 14:
                        driver.StateFromMissionToBarackBH();        // в барак 
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    //case 15:
                    //    driver.StateGotoTradeStep5BH();             // пятый этап продажи
                    //    break;
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
                        if (!otit.isOpenMap())   //карта не открыта
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
        /// разрешение выявленных проблем в добыче Pure Otite Multi
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
                        server.TopMenu(12, 2, true);
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
            if (server.isBulletHalf() || server.isBulletOff()) return 15;      // если заканчиваются экспертные патроны
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
                case 0:
                    botwindow.PressBoxInLeftPanel(5);
                    botwindow.PressBoxInLeftPanel(6);
                    server.MoveCursorOfMouse();
                    server.Buff(1, 1);
                    server.Buff(1, 2);
                    server.Buff(1, 3);

                    botwindow.PressEscThreeTimes();
                    break;
                //case 1: driver.StateRecovery();                 //логаут --> работа
                //    break;
                //case 2: botwindow.CureOneWindow();              //закрываем окно
                //    break;
                //case 3: botwindow.CureOneWindow2();             //отбегаем в сторону и закрываем окно
                //    break;
                //case 4: driver.StateActivePet();
                //    break;
                //case 5: driver.StateGotoTradeKatovia();         
                //    break;
                //case 6: driver.StateGotoTrade();                //работа -> продажа -> выгрузка окна
                //    break;
                //case 7: driver.StateExitFromShop2();            //продаемся и закрытие песочницы   09-14
                //    break;
                //case 8: driver.StateExitFromShop();             //продаемся и закрытие песочницы   10-14                                                  
                //    break;
                //case 9: server.buttonExitFromBarack();          //StateExitFromBarack();
                //    break;
                //case 10: //driver.StateExitFromTown();          
                //    server.Logout();
                //    //server.GoToEnd();
                //    //server.CloseSandboxie();              //закрываем все проги в песочнице
                //    break;
                //case 11: SellProduct();                     // выставление товаров на рынок
                //    break;
                //case 12: driver.StateSelling2();          //продажа в катовичевском магазине      
                //    break;
                //case 13: driver.StateSelling4();          //продажа в катовичевском магазине
                //    break;
                //case 14: driver.StateSelling3();          //продажа в катовичевском магазине   
                //    break;
                case 15:
                    server.AddBullet10000();              //открываем коробку с патронами 10 000 штук
                    server.MoveCursorOfMouse();
                    server.Buff(1, 1);
                    server.Buff(1, 2);
                    server.Buff(1, 3);
                    break;
                //case 16:
                //    server.LeaveGame();                  //если окно три прохода подряд в логауте, значит зависло. поэтому нажимаем кнопку "Leave Game"
                //    //server.CloseSandboxie();              //закрываем все проги в песочнице
                //    break;
                //case 17:
                //    server.CloseSandboxie();              //закрываем все проги в песочнице
                //    break;
                //case 18:
                //    server.BattleModeOn();         //включаем боевой режим
                //    server.TopMenu(9, 2);          //открываем окно с петом (для включения будущей расстановки)
                //    break;
                ////case 19:
                ////    driver.StateSerendibyteToTrade();
                ////    break;
                //case 20:
                //    server.ButtonToBarack(); //если стоят на странице создания нового персонажа, то нажимаем кнопку, чтобы войти обратно в барак
                //    break;
            }
        }

        #endregion

        #region общие методы

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

        #endregion

        #region смена аккаунтов

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





        #endregion

        #region смена аккаунтов (ЧИСТОЕ ОКНО)

        /// <summary>
        /// смена ботов в чистом окне (загрузка аккаунта-логаут-барак-город-выгрузка аккаунта)
        /// нужно для того, чтобы на новом компе войти каждым аккаунтом в игру
        /// </summary>
        public void ChangingAccountsCW()
        {
            //botwindow = new botWindow(numberOfWindow);
            //ServerFactory serverFactory = new ServerFactory(botwindow);
            //this.server = serverFactory.create();
            //this.globalParam = new GlobalParam();
            //this.botParam = new BotParam(numberOfWindow);
            //this.isActiveServer = server.IsActiveServer;

            if (isActiveServer) driver.StateInputOutputCW();
            else server.RemoveSandboxieCW();
            //}
        }

        #endregion

        #region  =================================== All in One Stage ==============================================

        /// <summary>
        /// разрешение выявленных проблем в БХ
        /// </summary>
        public void problemResolutionAllinOneStage(int NumberOfStage)
        {
            switch (NumberOfStage)
            {  
                case 1:
                    server.problemResolutionAllinOneStage1();
                    break;
                case 2:
                    server.problemResolutionAllinOneStage2();
                    break;
                case 3:
                    server.problemResolutionAllinOneStage3();
                    break;
                case 4:
                    server.problemResolutionAllinOneStage4();
                    break;
                case 5:
                    server.problemResolutionAllinOneStage5();
                    break;
                case 6:
                    server.problemResolutionAllinOneStage6();
                    break;
                case 7:
                    server.problemResolutionAllinOneStage7();
                    break;
                case 8:
                    server.problemResolutionAllinOneStage8();
                    break;
                case 9:
                    server.problemResolutionAllinOneStage9();
                    break;
            }
        }

        #endregion =================================================================================================

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
            BHDialog BHdialog = new BHDialogSing(botwindow);
            //KatoviaMarket kMarket = new KatoviaMarketSing (botwindow);
            //Market market = new MarketSing(botwindow);
            //Pet pet = new PetSing(botwindow);

            //server.ReOpenWindow();


            #region  доп проверки

            //if (!server.isBottlesOnLeftPanel()) server.MoveBottlesToTheLeftPanel();
            //MessageBox.Show("половина патронов? " + server.isBulletHalf());
            //MessageBox.Show("закончились патроны? " + server.isBulletOff());
            //MessageBox.Show("наследие3 " + botwindow.FindAncientBlessing(3));

            //int sdvig = 40;
            //server.OpenSpecInventory();
            //Pause(500);
            //server.SpecInventoryBookmark(2);
            //Pause(500);
            //server.MoveSpecInventory(sdvig);
            //Pause(500);

            //server.Logout();
            //if (server.isLogout()) server.CloseExpMerch();
            //если открыто окно Стим в правом нижнем углу
            //if (server.isOpenSteamWindow()) server.CloseSteamWindow();

            //MessageBox.Show("логаут?" + server.isLogout());
            //MessageBox.Show("есть ошибка?" + server.FindWindowError());
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

            //MessageBox.Show("есть концентрация у первого? " + server.FindConcentracion(1));
            //MessageBox.Show("есть концентрация у второго? " + server.FindConcentracion(2));
            //MessageBox.Show("есть концентрация у третьего? " + server.FindConcentracion(3));

            //MessageBox.Show("есть гончая у первого? " + server.FindHound(1));
            //MessageBox.Show("есть гончая у второго? " + server.FindHound(2));
            //MessageBox.Show("есть гончая у третьего? " + server.FindHound(3));

            //MessageBox.Show("есть лорчX у первого? " + server.FindShareFlint(1));
            //MessageBox.Show("есть лорчX у второго? " + server.FindShareFlint(2));
            //MessageBox.Show("есть лорчX у третьего? " + server.FindShareFlint(3));

            //MessageBox.Show("есть джайнаY у первого? " + server.FindMysophoia(1));
            //MessageBox.Show("есть джайнаY у второго? " + server.FindMysophoia(2));
            //MessageBox.Show("есть джайнаY у третьего? " + server.FindMysophoia(3));

            //MessageBox.Show("есть баф стероид у первого? " + botwindow.FindSteroid(1));
            //MessageBox.Show("есть баф стероид у второго? " + botwindow.FindSteroid(2));
            //MessageBox.Show("есть баф стероид у третьего? " + botwindow.FindSteroid(3));

            //MessageBox.Show("есть баф принципал у первого? " + botwindow.FindPrincipal(1));
            //MessageBox.Show("есть баф принципал у второго? " + botwindow.FindPrincipal(2));
            //MessageBox.Show("есть баф принципал у третьего? " + botwindow.FindPrincipal(3));

            //MessageBox.Show("командный режим " + botwindow.isCommandMode());               //проверено 22-11
            //MessageBox.Show("боевой режим?" + server.isBattleMode());               //проверено  22-11
            //MessageBox.Show(" " + town.isOpenTownTeleport());
            //MessageBox.Show(" " + pet.isOpenMenuPet());
            //MessageBox.Show(" " + pet.isSummonPet());
            //MessageBox.Show(" " + pet.isActivePet());
            //MessageBox.Show(" " + server.isLogout());
            //MessageBox.Show("около старика " + otit.isNearOldMan());
            //MessageBox.Show("в городе?" + server.isTown());   //22-11
            //MessageBox.Show("на работе?" + server.isWork());   //22-11
            //MessageBox.Show("убит первый перс?" + server.isKillFirstHero());   //22-11
            //MessageBox.Show("появился сундук?" + server.isTreasureChest());   //22-11

            //botwindow.Pause(1000);
            //server.WARP(3);
            //MessageBox.Show("ош? " + server.isAuch());
            //MessageBox.Show("Коимбра? " + server.isCoimbra());
            //MessageBox.Show("Ребольдо? " + server.isReboldo());
            //MessageBox.Show("Юстиар ??? " + server.isUstiar());
            //MessageBox.Show("Мост? " + server.isBridge());
            //MessageBox.Show("неправильная стойка? " + server.isBadFightingStance());  //22-11
            //MessageBox.Show("пользовательское соглашение? " + server.isNewSteam());
            //MessageBox.Show("ошибка 820? " + server.isError820());
            //MessageBox.Show("открыт Стим в углу? " + server.isOpenSteamWindow3());
            //MessageBox.Show("служба Стим? " + server.isSteamService());
            // MessageBox.Show("Left Panel? " + server.isBottlesOnLeftPanel());
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
            //MessageBox.Show("открыта карта??? " + otit.isOpenMap());
            //MessageBox.Show("красное слово? " + dialog.isRedSerendbite());
            //MessageBox.Show("есть бутылки?" + server.isBottlesOnLeftPanel());
            //MessageBox.Show("Открыта карта Юстиара ??? " + server.isOpenMapUstiar());

            //server.OpenDetailInfo();
            //MessageBox.Show("Открыт Detail Info? " + server.isOpenDetailInfo(1));
            //MessageBox.Show("Штурмовой режим ? " + server.isAssaultMode());               //проверено
            //MessageBox.Show("Undead " + server.isUndead());
            //MessageBox.Show("выбор команды " + server.isBarackTeamSelection());    //22-11
            //MessageBox.Show("в бараках? " + server.isBarack());  //22-11
            //MessageBox.Show("создание героя " + server.isBarackCreateNewHero());  //22-11
            //MessageBox.Show("Demon " + server.isDemon());
            //MessageBox.Show("Human " + server.isHuman());
            //MessageBox.Show("isLogout " + server.isLogout());
            //MessageBox.Show("телепорты? " + server.isOpenTopMenu(5));
            //MessageBox.Show("карты? " + server.isOpenTopMenu(12));
            //MessageBox.Show("Переполнение??? " + server.isBoxOverflow());
            //MessageBox.Show("Первый канал??? " + server.CurrentChannel_is_1());
            //MessageBox.Show("есть стим??? " + server.FindWindowSteamBool());
            //MessageBox.Show("мы в диалоге? " + dialog.isDialog());
            //MessageBox.Show("миссия не доступна? " + server.isMissionNotAvailable());
            //MessageBox.Show("Mission Lobby? " + server.isMissionLobby());
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
            //MessageBox.Show("BH  " + server.isBH());                                 //22-11
            //MessageBox.Show("11-19" + BHdialog.isGateLevelFrom10to19());
            //MessageBox.Show("лимит не исчерпан  " + BHdialog.isGateBH1());
            //MessageBox.Show("лимит исчерпан    " + BHdialog.isGateBH3());
            //MessageBox.Show("до 11  " + BHdialog.isGateLevelLessThan11());
            //MessageBox.Show("Initialize  " + BHdialog.isInitialize());
            //MessageBox.Show("первый герой=" + server.WhatsHero(1));
            //MessageBox.Show("второй герой=" + server.WhatsHero(2));
            //MessageBox.Show("третий герой=" + server.WhatsHero(3));
            //MessageBox.Show("закончилась активность?" + server.isActivityOut());
            //MessageBox.Show("магазин?" + (server.isExpedMerch() || server.isFactionMerch()));
            //server.CloseMerchReboldo();
            //server.TopMenu(12, 3,true);

            //server.isBarackLastPoint();

            //int[] x = { 0, 0, 130, 260, 390, -70, 60, 190, 320, 450 };
            //int[] y = { 0, 0, 0, 0, 0, 340, 340, 340, 340, 340 };

            //int[] aa = new int[17] { 0, 1644051, 725272, 6123117, 3088711, 1715508, 1452347, 6608314, 14190184, 1319739, 2302497, 5275256, 2830124, 1577743, 525832, 2635325, 2104613 };
            //bool ff = aa.Contains(725272);
            //int tt = Array.IndexOf(aa, 7272);
            //MessageBox.Show(" " + ff + " " + tt);
            //MessageBox.Show(" " + server.isExpedMerch());

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

            #endregion

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

            //Pause(4000);
            //
            //server.ActiveWindow();


            //iPoint pointLeaveGame = new Point(1045, 705);
            //pointLeaveGame.Move();
            //Pause(400);
            //pointLeaveGame.PressMouseLL();

            //server.systemMenu(4,true);


            //int xxx = 5;
            //int yyy = 5;
            //PointColor point1 = new PointColor(1223, 613, 1, 1);
            //PointColor point2 = new PointColor(1224, 613, 1, 1);
            //PointColor point3 = new PointColor(1151, 603, 1, 1);

            //for (int ii = 4; ii <= 17; ii++)
            //{
            //PointColor point1 = new PointColor(837 - 5 + xx, 674 - 5 + yy - (ii - 1) * 19, 0, 0);
            //PointColor point2 = new PointColor(843 - 5 + xx, 674 - 5 + yy - (ii - 1) * 19, 0, 0);
            //PointColor point3 = new PointColor(840 - 5 + xx, 684 - 5 + yy - (ii - 1) * 19, 0, 0);
            //int dx = 3;
            //int dy = 2;

            PointColor point1 = new PointColor(378 - 5 + xx, 539 - 5 + yy, 0, 0);
            PointColor point2 = new PointColor(378 - 5 + xx, 540 - 5 + yy, 0, 0);
            PointColor point3 = new PointColor(983 - 5 + xx, 252 - 5 + yy, 0, 0);

            color1 = point1.GetPixelColor();
            color2 = point2.GetPixelColor();
            color3 = point3.GetPixelColor();

            MessageBox.Show("цвет 1 = " + color1);
            MessageBox.Show("цвет 2 = " + color2);
            MessageBox.Show("цвет 3 = " + color3);
            //}

            //for (int xxx = 133; xxx < 146; xxx++)
            //    for (int yyy = 596; yyy < 598; yyy++)
            //    {
            //        PointColor point1 = new PointColor(xxx - 5 + xx, yyy - 5 + yy, 0, 0);
            //        color1 = point1.GetPixelColor();
            //        if (color1 == 13067318)
            //        { MessageBox.Show("xxx=" + xxx + " yyy=" + yyy); }
            //    }
            //MessageBox.Show("проверка завершена");


            //server.WriteToLogFile("цвет " + color1);
            //server.WriteToLogFile("цвет " + color2);

            //if (server.isBadFightingStance())
            //{
            //    server.ProperFightingStanceOn();
            //    //Pause(200);
            //    server.MoveCursorOfMouse();
            //}


            //for (int x = 29; x <= 52; x++)
            //    for (int y = 205; y <= 216; y++)
            //    {
            //        if (new PointColor(x, y, 0, 0).GetPixelColor() == 5144242 )
            //        MessageBox.Show("x = "+ x + "   y = " + y );
            //    }
            //MessageBox.Show("ВСЁ");

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
