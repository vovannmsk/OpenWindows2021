using OpenGEWindows;
using GEBot.Data;

namespace States
{
    /// <summary>
    /// класс хранит движки состояний для выполнения основных последовательностей действий (переход к продаже и обратно, продажа бота, восстановление бота из логаута и проч.)
    /// </summary>
    public class DriversOfState
    {
        private readonly botWindow botwindow;
        private readonly Server server;
        private readonly Otit otit;
        private readonly BotParam botParam;
        private readonly GlobalParam globalParam;

        public DriversOfState()
        { 
            
        }

        public DriversOfState(int numberOfWindow)
        {
            this.botwindow = new botWindow(numberOfWindow);
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            OtitFactory otitFactory = new OtitFactory(botwindow);
            this.otit = otitFactory.createOtit();
            this.botParam = new BotParam(numberOfWindow);                          //сделали экземпляр класса
            this.globalParam = new GlobalParam();     //сделали экземпляр класса
        }

        //========================================= методы ==================================================


        #region движки для запуска перехода по состояниям

        #region Demonic

        /// <summary>
        /// перевод из состояния 101 (BH) в состояние 102 (InfinityGate)
        /// </summary>
        public void StateFromBHToGateDem()
        {
            //server.WriteToLogFileBH("Движок 101-102 BH-->Gate");
            StateDriverRun(new StateGT401(botwindow), new StateGT499(botwindow));   // BH-->Gate
        }

        /// <summary>
        /// Активировать пета
        /// </summary>
        public void StateActivePetDem()
        {
            //StateDriverRun(new StateGT018(botwindow), new StateGT024(botwindow));
            StateDriverRun(new StateGT402(botwindow), new StateGT499(botwindow));    //активируем пета
        }

        #endregion


        #region Pure Otite Multi
        
        /// <summary>
        /// Town --> Mamut
        /// </summary>
        public void TeleportToMamut()
        {
            StateDriverRun(new StateGT500(botwindow), new StateGT533(botwindow));
        }

        /// <summary>
        /// Town --> Mamut
        /// </summary>
        public void FromTownToMamut()
        {
            StateDriverRun(new StateGT517(botwindow), new StateGT518(botwindow)); 
        }

        /// <summary>
        /// LosToldos --> OldMan
        /// </summary>
        public void FromLostoldosToOldman()
        {
            StateDriverRun(new StateGT075(botwindow), new StateGT076(botwindow));  
        }

        ///// <summary>
        ///// Mamut --> LosToldos
        ///// </summary>
        //public void FromMamutToLostoldos()
        //{
        //    StateDriverRun(new StateGT519(botwindow), new StateGT521(botwindow));
        //}

        /// <summary>
        /// Mamons --> Mamons (dialog)
        /// </summary>
        public void FromMamonsToMamonsDialog()
        {
            StateDriverRun(new StateGT519(botwindow), new StateGT520(botwindow));
        }

        /// <summary>
        /// Mamons (dialog) --> LosToldos
        /// </summary>
        public void FromMamonsDialogToLostolods()
        {
            StateDriverRun(new StateGT520(botwindow), new StateGT521(botwindow));
        }


        /// <summary>
        /// OldMan --> OldMan (dialog)
        /// </summary>
        public void PressOldMan()
        {
            StateDriverRun(new StateGT075a(botwindow), new StateGT076(botwindow));
        }

        /// <summary>
        /// OldMan (dialog). Get Task
        /// </summary>
        public void OldManDialogGetTask()
        {
            StateDriverRun(new StateGT522(botwindow), new StateGT523(botwindow));
        }

        /// <summary>
        /// OldMan (dialog). Get Reward
        /// </summary>
        public void OldManDialogGetReward()
        {
            StateDriverRun(new StateGT524(botwindow), new StateGT525(botwindow));
        }

        /// <summary>
        /// OldMan (dialog) --> Mission
        /// </summary>
        public void FromOldManDialogToMission()
        {
            StateDriverRun(new StateGT526(botwindow), new StateGT527(botwindow));
        }

        /// <summary>
        /// Mission --> Mission (Fight begin)
        /// </summary>
        public void FromMissionToFight()
        {
            StateDriverRun(new StateGT528(botwindow), new StateGT529(botwindow));
        }

        /// <summary>
        /// Mission (Fight) --> Fight To Next Point
        /// </summary>
        public void FightNextPoint()
        {
            StateDriverRun(new StateGT530(botwindow), new StateGT531(botwindow));
        }

        /// <summary>
        /// Mission (FightIsFinished) 
        /// </summary>
        public void FightIsFinished()
        {
            StateDriverRun(new StateGT532(botwindow), new StateGT533(botwindow));
        }


        #endregion


        #region Гильдия Охотников

        /// <summary>
        /// закрываем песочницу и переходим к следующему аккаунту
        /// </summary>
        public void StateCloseSandboxieBH()
        {
            StateDriverRun(new StateGT103a(botwindow), new StateGT108(botwindow));
        }


        /// <summary>
        /// вводим слово Initialize и ок
        /// </summary>
        public void StateInitialize()
        {
            StateDriverRun(new StateGT106(botwindow), new StateGT108(botwindow));
        }


        /// <summary>
        /// уровень миссии больше 20
        /// </summary>
        public void StateLevelAbove20()
        {
            StateDriverRun(new StateGT105(botwindow), new StateGT108(botwindow));
        }

        /// <summary>
        /// уровень миссии от 10 до 19
        /// </summary>
        public void StateLevelFrom11to19()
        {
            StateDriverRun(new StateGT104(botwindow), new StateGT108(botwindow));
        }

        /// <summary>
        /// уровень миссии меньше 10 
        /// </summary>
        public void StateLevelLessThan11()
        {
            StateDriverRun(new StateGT104a(botwindow), new StateGT108(botwindow));
        }

        /// <summary>
        /// если лимит дневных миссий закончился, то нажимаем Ок и удаляем песочницу
        /// </summary>
        public void StateLimitOff()
        {
            StateDriverRun(new StateGT103a(botwindow), new StateGT108(botwindow));
        }


        /// <summary>
        /// перевод из состояния 01 (в БХ) в состояние 03 (в городе продажи). 
        /// </summary>
        public void StateGotoTradeStep1BH()
        {
            StateDriverRun(new StateGT201(botwindow), new StateGT203(botwindow));
            server.WriteToLogFileBH("new StateGT201(botwindow), new StateGT203(botwindow)");
        }

        /// <summary>
        /// перевод из состояния 03 (в городе продажи) в состояние 09 (в магазине). 
        /// </summary>
        public void StateGotoTradeStep2BH()
        {
            StateDriverRun(new StateGT003(botwindow), new StateGT009(botwindow));
            server.WriteToLogFileBH("new StateGT03(botwindow), new StateGT09(botwindow)");
        }

        /// <summary>
        /// перевод из состояния 09 (в магазине) в состояние 10 (в магазине на закладке BUY или SELL). 
        /// </summary>
        public void StateGotoTradeStep3BH()
        {
            StateDriverRun(new StateGT009(botwindow), new StateGT010(botwindow));
            server.WriteToLogFileBH("new StateGT09(botwindow), new StateGT10(botwindow)");
        }

        /// <summary>
        /// перевод из состояния 10 (в магазине на закладке BUY) в состояние 11 (закладка SELL). 
        /// </summary>
        public void StateGotoTradeStep4BH()
        {
            StateDriverRun(new StateGT010(botwindow), new StateGT011(botwindow));
            server.WriteToLogFileBH("StateDriverRun(new StateGT10(botwindow), new StateGT11(botwindow));");
        }

        /// <summary>
        /// перевод из состояния 211 (в магазине на закладке BUY) в состояние 214 (логаут). 
        /// </summary>
        public void StateGotoTradeStep5BH()
        {
//            StateDriverRun(new StateGT11(botwindow), new StateGT14(botwindow));
            StateDriverRun(new StateGT211(botwindow), new StateGT214(botwindow));
            server.WriteToLogFileBH("StateDriverRun(new StateGT211(botwindow), new StateGT214(botwindow));");
        }

        /// <summary>
        /// перевод из состояния 108 (в миссии) в состояние 129 (бой) 
        /// </summary>
        public void StateFromMissionToFightBH()
        {
            server.WriteToLogFileBH("Движок 108-129 mission-->Fight");
            StateDriverRun(new StateGT108(botwindow), new StateGT129(botwindow));   // mission-->Fight
        }

        /// <summary>
        /// перевод из состояния 129 (после победы в бою) в состояние 130 (BH) 
        /// </summary>
        public void StateFromMissionToBH()
        {
            server.WriteToLogFileBH("Движок 129-130 mission --> BH");
            StateDriverRun(new StateGT129(botwindow), new StateGT130(botwindow));   // mission (Win)-->BH   (отбежать в сторону и телепортнуться)
        }

        /// <summary>
        /// перевод из состояния 250 (после победы в бою) в состояние 251 (барак) 
        /// </summary>
        public void StateFromMissionToBarackBH()
        {
            server.WriteToLogFileBH("Движок 250-251 mission --> Barack");
            StateDriverRun(new StateGT250(botwindow), new StateGT251(botwindow));   // mission (Win)-->Barack   (вызываем системное меню и в барак)
        }


        /// <summary>
        /// перевод из состояния 100 (город) в состояние 101 (в Гильдии Охотников) 
        /// </summary>
        public void StateFromTownToBH()
        {
            server.WriteToLogFileBH("Движок 100-101 Town-->BH");
            StateDriverRun(new StateGT100(botwindow), new StateGT101(botwindow));   // Town-->BH
        }

        /// <summary>
        /// перевод из состояния 15 (логаут) в состояние 16 (Barack) 
        /// </summary>
        public void StateFromLogoutToBarackBH()
        {
            server.WriteToLogFileBH("Движок 215-216 Logout-->Barack");
            StateDriverRun(new StateGT215(botwindow), new StateGT216(botwindow));     // Logout-->Barack
        }

        /// <summary>
        /// перевод из состояния 216 (Barack) --> 217 (Town)
        /// </summary>
        public void StateFromBarackToTownBH()
        {
            server.WriteToLogFileBH("Движок 216-217 Barack-->Town");
            StateDriverRun(new StateGT216(botwindow), new StateGT217(botwindow));     // Barack-->Town
        }


        /// <summary>
        /// перевод из состояния 101 (BH) в состояние 102 (InfinityGate)
        /// </summary>
        public void StateFromBHToGateBH()
        {
            server.WriteToLogFileBH("Движок 101-102 BH-->Gate");
            //botwindow.setStatusOfAtk(0);           //обнулили статус атаки
            StateDriverRun(new StateGT101(botwindow), new StateGT102(botwindow));   // BH-->Gate
        }

        /// <summary>
        /// перевод из состояния 104 (ворота состояние 2) в состояние 108 (миссия) 
        /// </summary>
        public void StateFromGate2ToMissionBH()
        {
            server.WriteToLogFileBH("Движок 104-108  Gate --> Mission");
            StateDriverRun(new StateGT104(botwindow), new StateGT108(botwindow));   // Gate --> Mission
        }

        /// <summary>
        /// перевод из состояния 102 (InfinityGate) в состояние 108 (миссия) 
        /// </summary>
        public void StateFromGateToMissionBH()
        {
            server.WriteToLogFileBH("Движок 102-108  Gate --> Mission");
            StateDriverRun(new StateGT102(botwindow), new StateGT108(botwindow));   // Gate --> Mission


        }

        /// <summary>
        /// перевод из состояния 106 (состояние ворот 4) в состояние 107 (состояние ворот 5) 
        /// </summary>
        public void StateFromGate4ToGate5BH()
        {
            server.WriteToLogFileBH("Движок 106-107  Gate4 --> Gate5");
            StateDriverRun(new StateGT106(botwindow), new StateGT107(botwindow));   // Gate4 --> Gate5
        }

        /// <summary>
        /// перевод из состояния 107 (состояние ворот 5) в состояние 108 (BH) 
        /// </summary>
        public void StateFromGate5ToBH()
        {
            server.WriteToLogFileBH("Движок 107-108  Gate5 --> BH");
            StateDriverRun(new StateGT107(botwindow), new StateGT108(botwindow));   // Gate5 --> BH
        }


        #endregion

        /// <summary>
        /// обмен какашек на серебрянные монеты
        /// </summary>
        public void StateSerendibyteToTrade()
        {
            //server.ReOpenWindow();
            //StateDriverRun(new StateGT151(botwindow), new StateGT152(botwindow));  //летим в город
            StateDriverRun(new StateGT318(botwindow), new StateGT331(botwindow));
        }

        /// <summary>
        /// создание мушкетеров в казарме до 38 штук
        /// </summary>
        public void StateMuskPlus()
        {
            //if (server.isTown())
            //    StateDriverRun(new StateGT304(botwindow), new StateGT307(botwindow));

            if (server.isLogout())
                StateDriverRun(new StateGT304(botwindow), new StateGT307(botwindow));

        }

        /// <summary>
        /// увеличение казармы с 4 до 46 мест
        /// </summary>
        public void StateBarackPlus()
        {
            StateDriverRun(new StateGT300(botwindow), new StateGT303(botwindow));
        }

        /// <summary>
        /// Алхимия на ферме. Яблоки и мед
        /// </summary>
        public void Farm()
        {
            StateDriverRun(new StateGT130(this.botwindow), new StateGT150(this.botwindow));  // заходим на ферму
            //StateDriverRun(new StateGT132(this.botwindow), new StateGT150(this.botwindow));  // заходим на ферму
            StateDriverRun(new StateGT091(this.botwindow), new StateGT092(this.botwindow));  // открываем карту

        }

        /// <summary>
        /// идем из состояния "нет окна" до состояния "в бараке", потом идем в город
        /// </summary>
        public void ChangingAccounts2()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT016(botwindow));  // заходим в казарму
            StateDriverRun(new StateGT220(botwindow), new StateGT235(botwindow));
        }

        /// <summary>
        /// идем из состояния "нет окна" до состояния "в бараке", потом идем в город
        /// </summary>
        public void ChangingAccounts3()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT017(botwindow));  // заходим в казарму
            StateDriverRun(new StateGT235(botwindow), new StateGT250(botwindow));
        }

        /// <summary>
        /// идем из состояния "нет окна" до состояния "в бараке", потом идем в город
        /// </summary>
        public void ChangingAccounts4()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT015(botwindow));  // загружаем окно ГЭ
            StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // удаляем песочницу
        }


        /// <summary>
        /// идем из состояния "нет окна" до состояния "в городе", потом получаем подарок и удаляем песочницу
        /// </summary>
        public void StateInputOutput()
        {
            //Server.rr = false;
            StateDriverRun(new StateGT014(botwindow), new StateGT017(botwindow));  // заходим в ребольдо

            //StateDriverRun(new StateGT014(botwindow), new StateGT016(botwindow));  // заходим в казарму
            if (Server.AccountBusy)   
            {
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // удаляем песочницу
            }
            else
            {
//               StateDriverRun(new StateGT165(botwindow), new StateGT170(botwindow));  // бежим к бабе ГМ + удаляем песочницу
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));   // удаляем песочницу


            }
        }

        /// <summary>
        /// идем из состояния "нет окна" до состояния "в стартонии", потом закрываем прогу в песочнице (для Европы)
        /// </summary>
        public void StateInputOutput2()
        {
            StateDriverRun(new StateGT014a(botwindow), new StateGT016(botwindow));  // reOpen + connect
            
            if (Server.AccountBusy)
            {
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
            }
            else
            {
                StateDriverRun(new StateGT032(botwindow), new StateGT035(botwindow));   // создаем семью и идем в стартонию
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));   // закрываем проги в песочнице
            }
        }

        /// <summary>
        /// ЭТО ЕВРОКВЕСТЫ от Стартонии до получения 100 еды с Кокошкой
        /// идем из состояния "нет окна" до состояния "пет Коко взят у Петмастера", потом закрываем прогу в песочнице (для Европы)
        /// </summary>
        public void StateInputOutput3()
        {
            //StateDriverRun(new StateGT014a(botwindow), new StateGT016new(botwindow));  // reOpen + connect
            StateDriverRun(new StateGT030(botwindow), new StateGT031(botwindow)); //reopen

            if (Server.AccountBusy)
            {
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
            }
            else
            {
                //StateDriverRun(new StateGT016new(botwindow), new StateGT017(botwindow));  // Barack. New Place

                StateDriverRun(new StateGT031(botwindow), new StateGT041(botwindow));   // делаем квесты для новичков
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));   // закрываем проги в песочнице
            }
        }

        /// <summary>
        /// получение журнала по ивенту на фонтане для всех серверов
        /// </summary>
        public void StateInputOutput4()
        {
            StateDriverRun(new StateGT014a(botwindow), new StateGT016new(botwindow));  // reOpen + connect

            if (Server.AccountBusy)
            {
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
            }
            else
            {
                StateDriverRun(new StateGT016new2(botwindow), new StateGT017(botwindow));  // Barack. New Place
                if (server.isPioneerJournal())
                {
                    //получаем ежедневную награду
                    server.GetGifts();
                }
                //else 
                //{
                //    //получаем журнал
                //    StateDriverRun(new StateGT093(botwindow), new StateGT099(botwindow));   // идем на фонтан и получаем вкусняшки
                //    StateDriverRun(new StateGT099(botwindow), new StateGT100(botwindow));   // применить журнал
                //    botwindow.Pause(1000);
                //    if (server.isPioneerJournal()) server.GetGifts();
                //}
                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
                
            }
        }

        /// <summary>
        /// получение журнала по ивенту на фонтане для всех серверов
        /// </summary>
        public void StateInputOutput5()
        {
            if (!server.isHwnd())   //если окно еще не загружено, то грузим и идём в миссию
            {
                botParam.NumberOfInfinity = globalParam.Infinity;
                globalParam.Infinity++;
                StateDriverRun(new StateGT314(botwindow), new StateGT316(botwindow));  // reOpen + connect + barack + команда №2 (ХАЙМАСТЕРА) + город 

                StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // временно поставил эту строку   // закрываем проги в песочнице

                //if (Server.AccountBusy)
                //{
                //    StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
                //}
                //else
                //{
                //    StateDriverRun(new StateGT251(botwindow), new StateGT259(botwindow));  // Фонтан-Диалог-Миссия
                //}

            }
            //else      // а если окно уже есть, то значит миссия уже пройдена и надо получить награду и удалить песочницу
            //{
            //    botwindow.ReOpenWindow();
            //    if (server.isTown())  //если уже вышли из миссии
            //    {
            //        StateDriverRun(new StateGT257(botwindow), new StateGT259(botwindow));  // Фонтан-Диалог-Фонтан
            //        StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
            //    }
            //}

        }

        /// <summary>
        /// продажа всех аккаунтов в Катовии один за другим
        /// </summary>
        public void StateInputOutput6()
        {
            StateDriverRun(new StateGT314(botwindow), new StateGT317(botwindow));  // reOpen + connect + barack + команда №2 (ХАЙМАСТЕРА) + город 
            botwindow.PressEscThreeTimes();

            //StateDriverRun(new StateGT275(botwindow), new StateGT277(botwindow));  //   надеваем бижутерию
            //botwindow.PressEscThreeTimes();

            //if (botParam.NomerTeleport == 1)        //если надо продаваться в Ребольдо, то перелетать не надо и начинаем с Состояния 003
            //    StateDriverRun(new StateGT003(botwindow), new StateGT012(botwindow));  // переход к магазину + продажа + выход в город из магазина
            //else
            //    StateDriverRun(new StateGT001(botwindow), new StateGT012(botwindow));  // переход к магазину + продажа + выход в город из магазина

            //StateDriverRun(new StateGT260(botwindow), new StateGT267(botwindow));  //   переход к аппарату патронов+покупка патронов+выход в город
            //StateDriverRun(new StateGT266a(botwindow), new StateGT271(botwindow)); //   экипируем розовые крылья
            //StateDriverRun(new StateGT271(botwindow), new StateGT274(botwindow));  //   получение наград в Achievement (Alt+L)
            //botwindow.PressEscThreeTimes();

            //закрываем проги в песочнице


                 StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем проги в песочнице
        }



        /// <summary>
        ///                // коралл кнопка (алхимия)
        /// </summary>
        public void StateAlchemy()
        {
            botwindow.Pause(300);
            if (server.isAlchemy())
            {
                StateDriverRun(new StateGT092(botwindow), new StateGT093(botwindow));

            }
        }

        /// <summary>
        /// идем из состояния логаут до старого человека в Лос Толдосе
        /// </summary>
        public void StateGoldenEggFarm()
        {
            StateDriverRun(new StateGT074(this.botwindow), new StateGT075(this.botwindow));  // заходим на ферму
            StateDriverRun(new StateGT091(this.botwindow), new StateGT092(this.botwindow));  // открываем карту

            StateDriverRun(new StateGT085(this.botwindow), new StateGT086(this.botwindow));  // бегаем по кругу и собираем GoldenEggFruit
        }

        /// <summary>
        /// перевод из состояния 75 в состояние 90. Цель  - добыча отита. Исполнитель - барон
        /// </summary>
        public void StateOtitRunBaron()
        {
            StateDriverRun(new StateGT015(botwindow), new StateGT017(botwindow));  // переход из состояния "Логаут" в состояние "В городе" (Los Toldos)
            StateDriverRun(new StateGT075(botwindow), new StateGT082(botwindow));  // до выполнения задания на мертвых землях
            StateDriverRun(new StateGT015(botwindow), new StateGT017(botwindow));  // переход из состояния "Логаут" в состояние "В городе" (Los Toldos)
            StateDriverRun(new StateGT082(botwindow), new StateGT085(botwindow));  // получаем отит и логаут
        }

        /// <summary>
        /// идем из состояния "нет окна" до старого человека в Лос Толдосе
        /// </summary>
        public void StateGotoOldMan ()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT018(botwindow));  // переход из состояния "нет окна" в состояние "Около Мамона" 
            StateDriverRun(new StateGT086(botwindow), new StateGT088(botwindow));  // Говорим с Мамоном и переходим в Лос Толдос 
            StateDriverRun(new StateGT075(botwindow), new StateGT076(botwindow));  // подбегаем к старому мужику
        }

        /// <summary>
        ///  Цель  - добыча отита. Исполнитель - не барон, но с отитовыми духами
        /// </summary>
        public void StateOtitRun2()
        {
            StateDriverRun(new StateGT075a(botwindow), new StateGT081(botwindow));  // берем задание и выполняем его на мертвых землях
            if (!server.isKillHero())        //если никого не убили
            {
                StateDriverRun(new StateGT088(botwindow), new StateGT089(botwindow));  // отбегаем в сторону (на мертвых землях)
                StateDriverRun(new StateGT089(botwindow), new StateGT090(botwindow));  // летим к Мамону
                StateDriverRun(new StateGT086(botwindow), new StateGT088(botwindow));  // Говорим с Мамоном и переходим в Лос Толдос 
                StateDriverRun(new StateGT082(botwindow), new StateGT084(botwindow));  // получаем отит и остаёмся в городе (Лос Толдосе) около старого мужика
            }
            else         //если в процессе выполнения задания кто-то из персов был убит
            {
                otit.ChangeNumberOfRoute();  //сменить маршрут, чтобы в следующий раз не попасть в ту же ловушку
                StateDriverRun(new StateGT081(botwindow), new StateGT082(botwindow));  // отбегаем в сторону (на случай, если кто-то выжил)  и логаут
                StateDriverRun(new StateGT015(botwindow), new StateGT018(botwindow));  // переход из состояния "Логаут" в состояние "Около Мамона" 
                StateDriverRun(new StateGT086(botwindow), new StateGT088(botwindow));  // Говорим с Мамоном и переходим в Лос Толдос 
                StateDriverRun(new StateGT075(botwindow), new StateGT075a(botwindow));  // подбегаем к старому мужику
            }
        }

        /// <summary>
        /// перевод из состояния 75 в состояние 90. Цель  - добыча отита. Исполнитель - не барон, но с отитовыми духами
        /// </summary>
        public void StateOtitRun()
        {
            StateDriverRun(new StateGT089(this.botwindow), new StateGT090(this.botwindow));  // летим из любого города к Мамону (первая строка в телепортах)
            StateDriverRun(new StateGT086(this.botwindow), new StateGT088(this.botwindow));  // Говорим с Мамоном и переходим в Лос Толдос 
            StateDriverRun(new StateGT075(this.botwindow), new StateGT081(this.botwindow));  // берем задание и выполняем его на мертвых землях
            if (!server.isKillHero())        //если никого не убили
            {
                StateDriverRun(new StateGT088(this.botwindow), new StateGT089(this.botwindow));  // отбегаем в сторону (на мертвых землях)
                StateDriverRun(new StateGT089(this.botwindow), new StateGT090(this.botwindow));  // летим к Мамону
                StateDriverRun(new StateGT086(this.botwindow), new StateGT088(this.botwindow));  // Говорим с Мамоном и переходим в Лос Толдос 
                StateDriverRun(new StateGT082(this.botwindow), new StateGT084(this.botwindow));  // получаем отит и остаёмся в городе (Лос Толдосе)
            }
            else         //если в процессе выполнения задания кто-то из персов был убит
            {
                otit.ChangeNumberOfRoute();  //сменить маршрут, чтобы в следующий раз не попасть в ту же ловушку
                StateDriverRun(new StateGT081(this.botwindow), new StateGT082(this.botwindow));  // отбегаем в сторону (на случай, если кто-то выжил)  и логаут
                StateDriverRun(new StateGT015(this.botwindow), new StateGT017(this.botwindow));  // переход из состояния "Логаут" в состояние "В городе" 
                StateDriverRun(new StateGT090(this.botwindow), new StateGT091(this.botwindow));  // лечение и патроны в городе
            }
        }

        /// <summary>
        /// перевод из состояния 20 (пет не выпущен) в состояние 01 (на работе). Цель  - выпустить пета и расставить треугольником
        /// </summary>
        public void StateActivePet()
        {
            StateDriverRun(new StateGT020(botwindow), new StateGT001(botwindow));
//            StateDriverRun(new StateGT20(botwindow), new StateGT28(botwindow));
        }

        /// <summary>
        /// перевод из состояния 60 в состояние 80. Цель  - передача песо торговцу
        /// </summary>
        public void StateTransferVisChapter1()
        {
            botWindow dealer = new botWindow(20);  //торговец

            StateDriverRun(new StateGT060(dealer), new StateGT065(dealer));   // торговец из логаута в город  (канал 3) и далее на место передачи песо
        }

        /// <summary>
        /// перевод из состояния 60 в состояние 80. Цель  - передача песо торговцу
        /// </summary>
        public void StateTransferVis()
        {
            botWindow dealer = new botWindow(20);  //торговец

            StateDriverRun(new StateGT060(this.botwindow), new StateGT063(this.botwindow));   // бот из логаута в город  (канал 3)

            StateDriverRun(new StateGT068(this.botwindow), new StateGT069(this.botwindow));   // бот бежит на место торговли и предлагает торговлю торговцу
            StateDriverRun(new StateGT069(dealer), new StateGT070(dealer));                   // торговец перекладывает фесо и ок-обмен

            StateDriverRun(new StateGT070(this.botwindow), new StateGT072(this.botwindow));   // бот перекладывает песо, закрывает сделку, покупает еду и логаут
            StateDriverRun(new StateGT072(dealer), new StateGT073(dealer));                   // торговец закрывает все лишние окна с экрана
        }

        /// <summary>
        /// перевод из состояния 60 в состояние 80. Цель  - передача песо торговцу.  старый вариант
        /// </summary>
        public void StateTransferVis2()
        {
            Server server;
            ServerFactory serverFactory;
            serverFactory = new ServerFactory(this.botwindow);
            server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)

            if (server.isLogout())
            {
                StateDriverRun(new StateGT060(this.botwindow), new StateGT074(this.botwindow));
            }
        }

        /// <summary>
        /// перевод из состояния 60 () в состояние 67 (). Цель  - заточка оружия и брони на +6 у Иды в Ребольдо
        /// </summary>
        public void StateSharpening()
        {
            for (int numberOfEquipment = 1; numberOfEquipment <= 6; numberOfEquipment++)
            {
                StateDriverRun(new StateGTI60(botwindow, numberOfEquipment), new StateGTI67(botwindow, numberOfEquipment));
            }
        }

        /// <summary>
        /// перевод из состояния 70 () в состояние 80 (). Цель  - чиповка оружия и брони 
        /// </summary>
        public void StateNintendo()
        {
            for (int numberOfEquipment = 1; numberOfEquipment <= 6; numberOfEquipment++)
            {
                StateDriverRun(new StateGTI70(botwindow, numberOfEquipment), new StateGTI80(botwindow, numberOfEquipment));
            }
        }

        /// <summary>
        /// перевод из состояния 151 (на работе) в состояние 170 (нет окна). Цель  - продажа после переполнения инвентаря в Катовии (снежка)
        /// </summary>
        public void StateGotoTradeKatovia()
        {
            server.ReOpenWindow();
            StateDriverRun(new StateGT151(botwindow), new StateGT162(botwindow));
        } 

        /// <summary>
        /// перевод из состояния 01 (на работе) в состояние 14 (нет окна). Цель  - продажа после переполнения инвентаря
        /// </summary>
        public void StateGotoTrade()
        {
            server.ReOpenWindow();
            //StateDriverRun(new StateGT001(botwindow), new StateGT003(botwindow));   // с работы в город
            ////StateDriverRun(new StateGT318(botwindow), new StateGT330(botwindow));   // меняем какашки на серебряные монеты
            //StateDriverRun(new StateGT003(botwindow), new StateGT014(botwindow));   // продаёмся в магазине
            StateDriverRun(new StateGT329(botwindow), new StateGT330(botwindow));   // с работы в Ребольдо
            StateDriverRun(new StateGT318(botwindow), new StateGT330(botwindow));   // меняем какашки на серебряные монеты
            if (botParam.NomerTeleport == 1)    //если надо продаваться в Ребольдо, то перелетать не надо и начинаем с Состояния 003
                StateDriverRun(new StateGT003(botwindow), new StateGT014(botwindow)); 
            else
                StateDriverRun(new StateGT001(botwindow), new StateGT014(botwindow));  

        }

        /// <summary>
        /// перевод из состояния 14 (нет окна бота) в состояние 01 (на работе)
        /// </summary>
        public void StateGotoWork()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT001(botwindow)); 
//            StateDriverRun(new StateGT14(botwindow), new StateGT28(botwindow));
        }

        /// <summary>
        /// перевод из состояния 15 (логаут) в состояние 01 (на работе)
        /// </summary>
        public void StateRecovery()
        {
//            StateDriverRun(new StateGT015(botwindow), new StateGT001(botwindow));
            StateDriverRun(new StateGT015(botwindow), new StateGT023(botwindow));  //выставляем до пета (меню пета остается на экране)

            //только на время ивента
            //StateDriverRun(new StateGT015(botwindow), new StateGT017(botwindow));   // доходим до ребольдо, получаем подарки
            //StateDriverRun(new StateGT093(botwindow), new StateGT096(botwindow));   // идем на фонтан, тыкаем в Лорайн
            //StateDriverRun(new StateGT096a(botwindow), new StateGT097(botwindow));  // получаем бафф
            //StateDriverRun(new StateGT017(botwindow), new StateGT001(botwindow));
        }

        /// <summary>
        /// перевод из состояния 14 (нет окна) в состояние 15 (логаут)              
        /// </summary>
        public void StateReOpen()
        {
            StateDriverRun(new StateGT014(botwindow), new StateGT015(botwindow));
            //StateDriverRun(new StateGT14(botwindow), new StateGT14(botwindow));
        }

        /// <summary>
        /// перевод из состояния 09 (в магазине) в состояние 12 (всё продано, в городе)                 // аква кнопка
        /// </summary>
        public void StateSelling()
        {

            botwindow.Pause(300);
            if (botwindow.getNomerTeleport() >= 100)
            {
                KatoviaMarketFactory marketFactory = new KatoviaMarketFactory(botwindow);
                KatoviaMarket kMarket = marketFactory.createMarket();
                if (kMarket.isSale())                                                            //проверяем, находимся ли в магазине
                    StateDriverRun(new StateGT157(botwindow), new StateGT161(botwindow));

            }
            else
            {
                MarketFactory marketFactory = new MarketFactory(botwindow);
                Market market = marketFactory.createMarket();

                if (market.isSale())                                 //проверяем, находимся ли в магазине
                    StateDriverRun(new StateGT009(botwindow), new StateGT012(botwindow));
            }
           
        }

        /// <summary>
        /// перевод из состояния 157 (в магазине Катовии на странице входа) в состояние 162 (всё продано, в логауте)    
        /// </summary>
        public void StateSelling2()
        {
            StateDriverRun(new StateGT157(botwindow), new StateGT162(botwindow));
        }

        /// <summary>
        /// перевод из состояния 158 (в магазине в Катовии на странице BUY) в состояние 162 (всё продано, в логауте)    
        /// </summary>
        public void StateSelling3()
        {
            StateDriverRun(new StateGT158(botwindow), new StateGT162(botwindow));
        }

        /// <summary>
        /// перевод из состояния 160 (в магазине в Катовии на странице SELL) в состояние 162 (всё продано, в логауте)    
        /// </summary>
        public void StateSelling4()
        {
            StateDriverRun(new StateGT160(botwindow), new StateGT162(botwindow));
        }

        /// <summary>
        /// создание новой семьи, выход в ребольдо, получение и надевание брони 35 лвл, выполнение квеста Доминго, разговор с Линдоном, получение Кокошки и еды 100 шт.
        /// перевод из состояния 30 (логаут) в состояние 41 (пет Кокошка получен)          // розовая кнопка
        /// </summary>
        public void StateNewAcc()
        {
            StateDriverRun(new StateGT030(botwindow), new StateGT041(botwindow));
        }
        
        /// <summary>
        /// создание новой семьи, выход в ребольдо, получение и надевание брони 35 лвл, выполнение квеста Доминго, разговор с Линдоном, получение Кокошки и еды 100 шт.
        /// перевод из состояния 30 (логаут) в состояние 41 (пет Кокошка получен)          
        /// </summary>
        public void StateNewAcc2()
        {
            StateDriverRun(new StateGT030(botwindow), new StateGT041(botwindow));
            StateDriverRun(new StateGT165(botwindow), new StateGT169(botwindow));  // бежим к бабе
            StateDriverRun(new StateGT169(botwindow), new StateGT170(botwindow));  // закрываем стим
        }

        /// <summary>
        /// вновь созданный аккаунт бежит через лавовое плато в кратер, становится на выделенное ему место, сохраняет телепорт и начинает ботить
        /// </summary>
        public void StateToCrater()
        {
            StateDriverRun(new StateGT042(botwindow), new StateGT058(botwindow));
        }

        /// <summary>
        /// перевод из состояния 10 (в магазине) в состояние 14 (нет окна)
        /// </summary>
        public void StateExitFromShop()
        {
            StateDriverRun(new StateGT010(botwindow), new StateGT014(botwindow));
        }

        /// <summary>
        /// перевод из состояния 09 (в магазине, на странице входа) в состояние 14 (нет окна)
        /// </summary>
        public void StateExitFromShop2()
        {
            StateDriverRun(new StateGT009(botwindow), new StateGT014(botwindow));
            //StateDriverRun(new StateGT09(botwindow), new StateGT12(botwindow));
        }

        /// <summary>
        /// перевод из состояния 12 (в городе) в состояние 14 (нет окна) 
        /// </summary>
        public void StateExitFromTown()
        {
            StateDriverRun(new StateGT012(botwindow), new StateGT014(botwindow));
            //StateDriverRun(new StateGT12(botwindow), new StateGT12(botwindow));
        }

        /// <summary>
        /// если бот стоит на работе, то он направляется на продажу
        /// </summary>
        public void StateGotoTradeAndWork()
        {
            
            //if (server.IsActiveServer)
            //{
                server.ReOpenWindow();
                botwindow.Pause(1000);

                if (botParam.NomerTeleport != 0 )           // если нужно продаваться
                {
                    //if (server.is248Items())
                    //{
                        //if (botParam.NomerTeleport >= 100)           // продажа в снежке
                        //{
                        //    StateGotoTradeKatovia();
                        //    botwindow.Pause(2000);
                        //}
                        //else                                         // продажа в городах
                        //{
                            StateGotoTrade();                       // по паттерну "Состояние".  01-14       (работа-продажа-выгрузка окна)
                            botwindow.Pause(2000);
                        //}
                        //StateGotoWork();            // по паттерну "Состояние".  14-28       (нет окна - логаут - казарма - город - работа)
                        //botwindow.Pause(2000);
                    //}
                }
            //}
        }


        

        /// <summary>
        /// запускает движок состояний StateDriver от пункта stateBegin до stateEnd
        /// </summary>
        /// <param name="stateBegin"> начальное состояние </param>
        /// <param name="stateEnd"> конечное состояние </param>
        public void StateDriverRun(IState stateBegin, IState stateEnd)
        {
            StateDriver stateDriver = new StateDriver(this.botwindow, stateBegin, stateEnd);
            while (!stateDriver.Equals(stateEnd))
            {
                stateDriver.run();
                stateDriver.setState();
            }
            //do
            //{
            //    stateDriver.run();
            //    stateDriver.setState();
            //} while (!stateDriver.Equals(stateEnd));
        }




        #endregion





    }
}
