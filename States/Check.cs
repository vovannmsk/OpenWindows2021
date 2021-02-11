using System;
using System.Windows.Forms;
using OpenGEWindows;
using GEBot.Data;

namespace States
{
    public class Check
    {
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
        /// <summary>
        /// номер состояния бота (место, где бот сейчас находится)
        /// </summary>
        private int numberOfState;
        /// <summary>
        /// выполнено ли задание OldMan для получения отита
        /// </summary>
        private bool taskCompleted;

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

        public Check()
        { 
        }

        public Check(int numberOfWindow)
        {
            numberOfState = 0;
            taskCompleted = false;
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
        }

        /// <summary>
        /// возвращает номер телепорта для продажи
        /// </summary>
        /// <returns></returns>
        public int getNumberTeleport()
        {
            return botwindow.getNomerTeleport();
        }

        /// <summary>
        /// если находимся на алхимическом столе, то true
        /// </summary>
        /// <returns></returns>
        public bool isAlchemy()
        {
            return server.isAlchemy();
        }

        /// <summary>
        /// выполняет действия по открытию окна с игрой
        /// </summary>
        public void OpenWindow ()
        {
            server.ReOpenWindow();
        }

        public bool EndOfList()
        {
            return botParam.EndOfList();
        }

        #region Гильдия охотников BH

        /// <summary>
        /// начальные присваивания переменной Инфинити для БХ
        /// </summary>
        public void InitialAssignment()
        {
            botParam.NumberOfInfinity = globalParam.Infinity;
            globalParam.InfinityPlusOne();
            //globalParam.Infinity = globalParam.Infinity + 1;
        }


        /// <summary>
        /// проверяем, если ли проблемы при работе в БХ и возвращаем номер проблемы
        /// </summary>
        /// <returns>порядковый номер проблемы</returns>
        public int NumberOfProblemBH()
        {
//            int statusOfSale = globalParam.StatusOfSale;          //не отлажено
            int statusOfSale = botParam.StatusOfSale;          //не отлажено
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
            if (server.isTown())   //если в городе
            {
                //if (server.isBH2()) return 18;   //стоим в БХ в неправильном месте
                if (server.isBH())     //в БХ 
                {
                    if (server.isBH2()) return 18;   //стоим в БХ в неправильном месте
                    if (statusOfSale == 1)
                    {
                        // если нужно бежать продаваться
                        return 3;                                              ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                    else
                    {
                        // если не нужно бежать продаваться и стоим в правильном месте (около ворот Инфинити)
                        return 4;
                    }
                }
                else   // в городе, но не в БХ
                {
                    if (statusOfSale == 1)
                    {
                        // если нужно бежать продаваться
                        return 5;                                              ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                    else
                    {
                        // если не нужно бежать продаваться
                        return 6;
                    }
                }
            }

            //магазин
            if (market.isSale())
            {
                if (!server.isTown() || server.isWork()) return 11;        //если стоим в магазине на экране входа, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            }
            if (market.isClickPurchase())
            {
                if (!server.isTown() || server.isWork()) return 12;         //если стоим в магазине на закладке Purchase, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            }
            if (market.isClickSell())
            {
                if (!server.isTown() || server.isWork()) return 15;         //если стоим в магазине на закладке Purchase, но не проходит проверка, что мы в городе и что мы на работе (защита от ложных срабатываний)
            }

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
            if (server.isKillAllHero()) result = 29;            // если убиты все
            if (server.isKillHero()) result = 30;               // если убиты не все 


            // Город (Ребольдо или ЛосТолдос)
            if (server.isTown() && result == 0)   
            {
                switch (NumberOfState)
                {
                    case 1:
                        result = 3;         // мы в Ребольдо
                        break;
                    case 2:
                        result = 4;         // мы в ЛосТолдосе
                        break;
                    case 3:             //стоим около OldMan задание не взято
                    case 4:             //стоим около OldMan задание взято
                        result = 5;
                        break;
                    default:
                        ////стоим около OldMan 
                        //result = 5;
                        NumberOfState = 2;
                        result = 4;         // мы в ЛосТолдосе
                        break;
                }
                //if (result != 0) return result;
            }

            //в миссии или около Мамута
            if (server.isWork() && result == 0)
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
                        if (!otit.isTaskDone())
                            result = 13;    //бежим к следующей точке маршрута с атакой
                        else
                            result = 14;    //бежим к следующей точке маршрута без атаки
                        break;
                    case 7:
                        result = 15;        //улетаем из миссии к Мамуну
                        break;
                    default:                //по умолчанию считаем, что если в бою, то в миссии 
                                            //(не проверено. как вариант сделать  result = 12; )
                        NumberOfState = 6;
                        if (!otit.isTaskDone())
                            result = 13;    //бежим к следующей точке маршрута с атакой
                        else
                            result = 14;    //бежим к следующей точке маршрута без атаки
                        break;
                }
                //if (result != 0) return result;
            }

            if (dialog.isDialog() && result == 0)
            {
                switch (NumberOfState)
                {
                    case 1:
                    case 2:
                        result = 8;             //Mamons
                        break;
                    case 3:
                        if (!TaskCompleted)
                            result = 9;         //OldMan (задание не взято)
                        else
                            result = 10;        //OldMan (получение награды)
                        break;
                    case 4:
                    case 5:                   //на всякий случай. для надежности
                    case 6:                   //на всякий случай. для надежности
                        result = 11;            //OldMan (задание взято, переходим в миссию)
                        break;
                    default:
                        NumberOfState = 3;
                        result = 10;            //OldMan (получение награды) 
                        break;
                }
                //if (result != 0) return result;
            }

            if (result == 0 && server.isBarack()) result = 2;                  // если стоят в бараке 


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

            //============== если зависли в каком-либо состоянии, то особые действия ========
            //if (numberOfProblem == prevProblem && numberOfProblem == prevPrevProblem)  
            //{
            //    switch (numberOfProblem)
            //    {
            //        case 1:  //зависли в логауте
            //        case 23:  //загруженное окно зависло и не смещается на нужное место
            //            numberOfProblem = 31;  //закрываем песочницу без перехода к следующему аккаунту
            //            break;
            //        case 4:  //зависли в БХ
            //            numberOfProblem = 18; //переходим в стартовый город через системное меню
            //            break;
            //    }
            //}
            //else { prevPrevProblem = prevProblem; prevProblem = numberOfProblem; }

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
                        otit.GoToOldManMulti();                    // Lostoldos --> OldMan
                        //driver.FromLostoldosToOldman();             // Lostoldos --> OldMan
                    if (otit.isTaskDone()) TaskCompleted = true;   //на всякий случай
                        NumberOfState = 3;                          //обязательно нужно, что разделить состояния в ЛосТолдосе
                    break;
                    case 5:
                    otit.PressOldMan();                             // OldMan --> OldMan (dialog)
                    //driver.PressOldMan();                       // OldMan --> OldMan (dialog)
                    break;
                    case 6:
                        driver.FromMamonsToMamonsDialog();          //Mamons --> MaMons(Dialog)
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
                    break;
                    case 11:
                        otit.EnterToTierraDeLosMuertus();           //Oldman(Dialog) --> Mission
                        //driver.FromOldManDialogToMission();         //Oldman(Dialog) --> Mission
                        botParam.HowManyCyclesToSkip = 2;
                        NumberOfState = 5;
                    break;
                    case 12:
                        driver.FromMissionToFight();                //Mission-- > Mission (Fight begin)
                        botParam.HowManyCyclesToSkip = 1;
                        NumberOfState = 6;
                    break;
                    case 13:
                        otit.GotoNextPointRouteMulti();             //Mission(Fight)-- > Fight To Next Point
                        //driver.FightNextPoint();                    //Mission(Fight)-- > Fight To Next Point
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 14:
                        driver.FightIsFinished();                   // Mission (FightIsFinished) 
                        TaskCompleted = true;
                        NumberOfState = 7;
                    break;
                    case 15:
                        server.Teleport(1);                         // телепорт к Мамуну
                        botParam.HowManyCyclesToSkip = 1;
                        NumberOfState = 1;
                    break;
                    //case 17:
                    //    botwindow.PressEsc();                       // нажимаем Esc
                    //    break;
                    case 25:
                        server.ReOpenWindow();
                        break;
                    case 29:
                        botwindow.CureOneWindow();              // идем в logout
                        botParam.HowManyCyclesToSkip = 1;
                        break;
                    case 30:
                        botwindow.CureOneWindow2();             // отбегаем в сторону и логаут
                        botParam.HowManyCyclesToSkip = 1;
                        break;
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
                    if (server.is248Items())                       //проверяем реально ли карман переполнился
                    {
                        //if (numberTeleport >= 100)                 // продажа в снежке
                        //    {  server.WriteToLogFile("Продажа в снежке"); return 5; }
                        //else                                 
                        //    { 
                            server.WriteToLogFile("Продажа не в снежке"); return 6; // продажа в городах
                        //}

                    }
                    else  
                    {
                        //если выскочило сообщение о переполнении кармана, но количество вещей меньше 248, значит надо обменивать какашки на монеты
                        return 19;
                    }
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
            if (server.isWork() && !server.isBattleMode()) return 18;       //если на стоим работе, но не в боевом режиме
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
                case 1: driver.StateRecovery();
                    break;
                case 2: botwindow.CureOneWindow();              //закрываем окно
                    break;
                case 3: botwindow.CureOneWindow2();             //отбегаем в сторону и закрываем окно
                    break;
                case 4: driver.StateActivePet();
                    break;
                case 5: driver.StateGotoTradeKatovia();         //Pause(2000);
                    break;
                case 6: driver.StateGotoTrade();                //Pause(2000);                        // по паттерну "Состояние".  01-14       (работа-продажа-выгрузка окна)
                    break;
                case 7: driver.StateExitFromShop2();            //продаемся и закрытие песочницы   09-14
                    break;
                case 8: driver.StateExitFromShop();             //продаемся и закрытие песочницы   10-14                                                  
                    break;
                case 9: server.buttonExitFromBarack();          //StateExitFromBarack();
                    break;
                case 10: //driver.StateExitFromTown();          
                    server.GoToEnd();
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
                case 19:
                    driver.StateSerendibyteToTrade();
                    break;
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

        /// <summary>
        /// поиск новых окон с игрой для кнопки "Найти окна"
        /// </summary>
        /// <returns></returns>
        public UIntPtr FindWindow()
        {
            return server.FindWindowGE();
        }

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

        /// <summary>
        /// проверка, находится ли окно в состоянии Logout
        /// </summary>
        /// <returns></returns>
        public bool isLogout()
        {
            return server.isLogout();
        }

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

        /// <summary>
        /// номер аккаунта, номер логина по порядку
        /// </summary>
        /// <returns></returns>
        public string NumberOfInfinity()
        {
            return botParam.NumberOfInfinity.ToString();
        }


        /// <summary>
        /// тестовая кнопка
        /// </summary>
        public void TestButton()
        {
            int i = 2;   //номер проверяемого окна

            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 5, 30, 55, 80, 105, 130, 5 };

            botWindow botwindow = new botWindow(i);
            Server server = new ServerSing(botwindow);
            server.ReOpenWindow();

            //MessageBox.Show(" " + botwindow.getNomerTeleport());
            //botwindow.Pause(1000);

            //Dialog dialog = new DialogSing(botwindow);
            //MessageBox.Show("Диалог??? " + dialog.isDialog());

            //Server server = new ServerSing(botwindow);
            //server.isBottlesOnLeftPanel();
            //MessageBox.Show("есть бутылки?" + server.isBottlesOnLeftPanel());
            //Town town = new SingTownArmonia(botwindow);
            //MessageBox.Show("Открыта карта города ??? " + town.isOpenMap());
            //server.MoveBottlesToTheLeftPanel();
            //Server server = new ServerEuropa2(botwindow);


            //MessageBox.Show("Нужный диалог? " + server.isBeginOfMission());
            //MessageBox.Show("Undead " + server.isUndead());
            //MessageBox.Show("Wild " + server.isWild());
            //MessageBox.Show("Demon " + server.isDemon());
            //MessageBox.Show("Human " + server.isHuman());
            //MessageBox.Show("isLogout " + server.isLogout());
            //MessageBox.Show("Переполнение??? " + server.isBoxOverflow());
            //MessageBox.Show("Первый канал??? " + server.CurrentChannel_is_1());
            //MessageBox.Show("есть стим??? " + server.FindWindowSteamBool());

            //Server server = new ServerAmerica2(botwindow);

            //BHDialog BHdialog = new BHDialogSing(botwindow);
            //Dialog dialog = new DialogSing(botwindow);
            //bool ttt = dialog.isDialog();
            //MessageBox.Show(" " + ttt);

            //KatoviaMarket kMarket = new KatoviaMarketSing (botwindow);
            //Market market = new MarketSing(botwindow);

            //            Pet pet = new PetAmerica2(botwindow);
            //Pet pet = new PetAmerica(botwindow);
            //MessageBox.Show(" " + pet.isSummonPet());

            //Otit otit = new OtitSing(botwindow);
            //MessageBox.Show(" " + server.is248Items());

            //server.MissionNotFoundBH();
            //bool iscolor1 = pet.isActivePet();
            //MessageBox.Show(" " + iscolor1);

            //bool iscolor1 = kMarket.isSaleIn();
            //MessageBox.Show(" " + iscolor1);

            //bool iscolor1 = market.isClickPurchase();
            //MessageBox.Show(" " + iscolor1);
            //bool iscolor2 = market.isClickSell();
            //MessageBox.Show(" " + iscolor2);

            //bool iscolor1 = server.isUndead();
            //MessageBox.Show(" " + iscolor1);
            //bool ttt;
            //ttt = BHdialog.isBottonGateBH();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH1();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH2();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH3();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH4();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH5();
            //MessageBox.Show(" " + ttt);
            //ttt = BHdialog.isGateBH6();
            //MessageBox.Show(" " + ttt);

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



            int xx, yy;
            xx = koordX[i - 1];
            yy = koordY[i - 1];
            uint color1;
            uint color2;
            //uint color3;
            //int x = 483;
            //int y = 292;
            //int i = 4;

            //int j = 12;
            //PointColor point1 = new PointColor(149 - 5 + xx, 219 - 5 + yy + (j - 1) * 27, 1, 1);       // новый товар в магазине в городе
            // PointColor point1 = new PointColor(152 - 5 + xx, 250 - 5 + yy + (j - 1) * 27, 1, 1);       // новый товар в магазине в Катовии

            //PointColor point1 = new PointColor(1042, 551, 1, 1);
            //PointColor point2 = new PointColor(1043, 551, 1, 1);
            PointColor point1 = new PointColor(929-30 + xx, 400-30 + yy, 0, 0);
            PointColor point2 = new PointColor(929-30 + xx, 403-30 + yy, 0, 0);
            //PointColor point3 = new PointColor(348 - 5 + xx, 213 - 5 + yy, 0, 0);


            color1 = point1.GetPixelColor();
            color2 = point2.GetPixelColor();
            //color3 = point3.GetPixelColor();

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
