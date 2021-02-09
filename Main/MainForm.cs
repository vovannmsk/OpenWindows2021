using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using States;
using GEBot.Data;

namespace Main
{
    public partial class MainForm : Form
    {
        private static uint NumberBlueButton = 0;       //сколько раз нажали голубую(красную) кнопку
        //private const int MAX_NUMBER_OF_ACCOUNTS = 20;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        //        private static string DataVersion = "08-02-2020";
        private static string DataVersion = DateTime.Now.ToString("D");
        private int numberOfAcc;                                              // количество аккаунтов ботов
        private int startAcc;
        private GlobalParam globalParam;
        //private BotParam botParam;

        //public UIntPtr[] arrayOfHwnd = new UIntPtr[21];   //используется в методе "Найти окна"

        /// <summary>
        /// конструктор
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.globalParam = new GlobalParam();                          //сделали экземпляр класса
            //this.botParam = new BotParam(1);
            numberOfAcc = globalParam.TotalNumberOfAccounts;
            startAcc = globalParam.StartingAccount;
            this.Text = "Программа от " + DataVersion + "    " + numberOfAcc + " окон";
            this.Location = new System.Drawing.Point(1315, 1080 - this.Height - 40);
            this.numberOfAccounts.Value = numberOfAcc;
            this.startAccount.Value = startAcc;
            //this.labelNomer.Text = "Текущий № аккаунта " + botParam.NumberOfInfinity;

            string typeOfNintendo;
            switch (globalParam.Nintendo)
            {
                case 1: typeOfNintendo = "без расы"; break;
                case 2: typeOfNintendo = "Wild"; break;
                case 3: typeOfNintendo = "LifeLess"; break;
                case 4: typeOfNintendo = "Wild or Human"; break;
                case 5: typeOfNintendo = "Undeed"; break;
                case 6: typeOfNintendo = "Demon"; break;
                case 7: typeOfNintendo = "Human"; break;
                case 10: typeOfNintendo = "все расы"; break;
                default: typeOfNintendo = "без расы"; break;
            }
            this.labelEnchanting.Text = "Чиповка - " + typeOfNintendo;

        }

        /// <summary>
        /// действия при закрытии формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// присваиваем переменной класса значение, выбранное пользователем в форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numberOfAccouts_Leave(object sender, EventArgs e)
        {
            numberOfAcc = (int)this.numberOfAccounts.Value;
        }


        #region Light Coral Button "Увеличение казармы"

        /// <summary>
        /// найти окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)                                                                          //кнопка "найти окна"
        {
            button5.Visible = false;

            Thread myThreadCoral = new Thread(funcCoral);
            myThreadCoral.Start();

            button5.Visible = true;
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого Coral кнопкой
        /// </summary>
        private void funcCoral()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
                if (check[j].IsActiveServer)
                {
                    check[j].ReOpenWindow();
                    check[j].Pause(1000);

                    DriversOfState drive = new DriversOfState(j);
                    drive.StateBarackPlus();
                }

        }

        #endregion

        #region Оранжевая кнопка (выравнивание окон)

        /// <summary>
        /// ВОССТАНОВЛЕНИЕ ОКОН                                                                                                                 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReOpenWindowGE_Click(object sender, EventArgs e)                                                        
        {
            buttonReOpenWindowGE.Visible = false;

            Thread mythread = new Thread(funcOrange);
            mythread.Start();
            
            buttonReOpenWindowGE.Visible = true;
         }

        /// <summary>
        /// метод задает функционал для потока, организуемого оранжевой кнопкой
        /// </summary>
        private void funcOrange()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
                if (check[j].IsActiveServer) check[j].OrangeButton();


            //for (int j = 1; j <= numberOfAcc; j++)
            //{
            //    Check check = new Check(j);
            //    if (check.IsActiveServer)
            //    {
            //        check.OrangeButton();
            //        //check.serverSelection();
            //    }
            //}   
        }

        #endregion

        #region ЛАЙМ КНОПКА (бежим до кратера)

        /// <summary>
        /// новый аккаунт бежит в кратер, сохраняет там телепорт                                                                         ЛАЙМ КНОПКА
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunToCrater_Click(object sender, EventArgs e)
        {
            RunToCrater.Visible = false;
            Thread myThreadLime = new Thread(funcLime);
            myThreadLime.Start();

            RunToCrater.Visible = true;

        }

        /// <summary>
        /// метод задает функционал для потока, организуемого лайм кнопкой
        /// </summary>
        private void funcLime()
        {
            for (int j = 1; j <= numberOfAcc; j++)
//            for (int j = 2; j <= 2; j++)
            {
                Check check = new Check(j);
                if (check.IsActiveServer)
                {
                    DriversOfState driver = new DriversOfState(j);
                    driver.StateToCrater();
                }
            }
        }


        #endregion

        #region РОЗОВАЯ КНОПКА (новые аккаунты)

        /// <summary>
        /// создание новых аккаунтов (создание потока)              
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNewAcc_Click(object sender, EventArgs e)
        {
            buttonNewAcc.Visible = false;
            Thread myThreadPink = new Thread(funcPink);
            myThreadPink.Start();
            buttonNewAcc.Visible = true;
        }


        /// <summary>
        /// создание новых аккаунтов
        /// </summary>
        private void funcPink()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            { 
                if (check[j].IsActiveServer)
                {
                    check[j].NewAccountsTwo();
                }
            }
        }

        #endregion

        #region AQUA кнопка (продажа всех окон, стоящих в магазине)

        /// <summary>
        /// СУПЕР ПРОДАЖА
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSuperSell_Click(object sender, EventArgs e)                                                              //        кнопка цвета морской волны (AQUA)
        {
            buttonSuperSell.Visible = false;
            Thread mythread = new Thread(funcAqua);
            mythread.Start();
            buttonSuperSell.Visible = true;
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого аква кнопкой
        /// </summary>
        private void funcAqua()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                //Check check = new Check(j);
                if (check[j].IsActiveServer)
                {
                    check[j].ReOpenWindow();
                    check[j].Pause(1000);
                    DriversOfState driver = new DriversOfState(j);
                    driver.StateSelling();             // продаёт всех ботов, которые стоят в данный момент в магазине (через движок состояний)
                }
            }
        }
        
        #endregion

        #region Test Button
        /// <summary>
        /// тестовая кнопка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)                                                                                             // Тестовая кнопка
        {
            button1.Visible = false;


            Check check = new Check(1);

            check.TestButton();


            button1.Visible = true;
        }

        #endregion

        #region Green button (исправление проблем)

        /// <summary>
        /// проверка проблем у ботов и исправление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_StandUp_Click(object sender, EventArgs e)         
        {
            button_StandUp.Visible = false;
            Thread myThreadGreen = new Thread(funcGreen);
            myThreadGreen.Start();

            button_StandUp.Visible = true;
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого аква кнопкой
        /// </summary>
        private void funcGreen()
        {
            //Check[] check = new Check[numberOfAcc + 1];
            //for (int j = startAccount; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                Check check = new Check(j);
                if (check.IsActiveServer) check.problemResolution();
            }
            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                Check check = new Check(j);
                if (check.IsActiveServer)   check.ReOpenWindow();
            }   
        }

        #endregion

        #region Blue button  (запускает по таймеру проверку состояния ботов)

        /// <summary>
        /// Процедура периодически (раз в минуту) запускает проверку (зеленую кнопку)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWarning_Click(object sender, EventArgs e)                                                //      Голубая кнопка
        {
            buttonWarning.Visible = false;

           NumberBlueButton++;
           if ((NumberBlueButton % 2) == 1)
           {
               this.buttonWarning.Text = "ВКЛЮЧЕН АВТОРЕЖИМ !!!!!!!!!!!!";
               this.buttonWarning.BackColor = Color.OrangeRed;

               // добавлено 08-09-2016
               if (MainForm.NumberBlueButton == 1)    
               {
                   myTimer.Tick += new EventHandler(TimerEventProcessor);
                   myTimer.Interval = 30000;
                   myTimer.Start();
               }
               else myTimer.Start();
               // коенц добавленного 08-09-2016
           }
           else
           {
               this.buttonWarning.Text = "== АВТОРЕЖИМ ВЫКЛЮЧЕН ==";
               this.buttonWarning.BackColor = Color.SkyBlue;
               // добавлено 08-09-2016
               myTimer.Stop();
               // конец добавленного 08-09-2016
           }
            buttonWarning.Visible = true;
        }

        /// <summary>
        /// именно в этом методе организуется поток для выполнения функционала синей кнопки
        /// </summary>
        /// <param name="myObject"> не имею понятия, что это </param>
        /// <param name="myEventArgs"> не имею понятия, что это </param>
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)                                    //ВАЖНЫЙ МЕТОД (ПРОДОЛЖЕНИЕ ГОЛУБОЙ КНОПКИ) (используется)
        {
            myTimer.Stop();

            funcGreen();
            //Thread myThreadGreen = new Thread(funcGreen);    //запускаем новый процесс
            //myThreadGreen.Start();


            myTimer.Enabled = true;
        }

        #endregion

        #region New white button (отправляет все окна на продажу)

        /// <summary>
        /// метод продаёт все окна по очереди и потом каждое окно выставляет опят на работу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGotoTradeTest_Click(object sender, EventArgs e)                                                     //новая белая кнопка (продажа). 
        {
            buttonGotoTradeTest.Visible = false;

            Thread mythreadNewWhite = new Thread(funcNewWhite);
            mythreadNewWhite.Start();

            buttonGotoTradeTest.Visible = true;

        }

        /// <summary>
        /// метод задает функционал для потока, организуемого White Button (Sale)
        /// </summary>*
        private void funcNewWhite()
        {
            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                DriversOfState driver = new DriversOfState(j);
                driver.StateGotoTradeAndWork();
                
            }
        }
        
        #endregion

        #region золотая кнопка (ФЕРМА)

        /// <summary>
        /// Золотая кнопка. Алхимия на ферме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenWindow_Click(object sender, EventArgs e)
        {
            ButtonOpenWindow.Visible = false;

            Thread myThreadGold = new Thread(funcGold);
            myThreadGold.Start();

            ButtonOpenWindow.Visible = true;
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого gold кнопкой
        /// </summary>
        private void funcGold()
        {

            Check[] check = new Check[numberOfAcc + 1];
            DriversOfState[] driver = new DriversOfState[numberOfAcc + 1];

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор
                driver[j] = new DriversOfState(j);
            }

            for (int j = startAcc; j <= numberOfAcc; j++)
                if (check[j].IsActiveServer) driver[j].Farm();


        }

        #endregion

        #region Magenta button (Sharpening)

        /// <summary>
        /// запускает новый процесс по обработке Фиолетовой кнопки (заточка оружия и брони на +6 у Иды)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sharpening_Click(object sender, EventArgs e)
        {
            Thread mythreadMagenta = new Thread(funcMagenta);
            mythreadMagenta.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого Magenta Button (Sharpening)
        /// </summary>
        private void funcMagenta()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                if (check[j].IsActiveServer)
                {
                    check[j].ReOpenWindow();
                    check[j].Pause(1000);
                    if (check[j].isIda())   //если окно находится в магазине Иды
                    {
                        DriversOfState drive = new DriversOfState(j);
                        drive.StateSharpening();
                    }
                }
            }
        }

        #endregion

        #region Chocolate button (чиповка)

        /// <summary>
        /// Шоколадная кнопка (чиповка экипировки)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nintendo_Click(object sender, EventArgs e)
        {
            Thread myThreadChocolate = new Thread(funcChoco);
            myThreadChocolate.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого gold кнопкой
        /// </summary>
        private void funcChoco()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                
                if (check[j].IsActiveServer)
                {
                    check[j].ReOpenWindow();
                    check[j].Pause(1000);
                    if (check[j].isEnchant())   //если окно находится в магазине чиповки
                    {
                        DriversOfState drive = new DriversOfState(j);
                        drive.StateNintendo();
                    }
                }
            }
        }


        #endregion

        #region Green Button (TransferVis)

        /// <summary>
        /// создание мушкетеров в казарме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferVis_Click(object sender, EventArgs e)
        {
            Thread myThreadTransfer = new Thread(funcTransfer);
            myThreadTransfer.Start();

        }

        /// <summary>
        /// метод задает функционал для потока, организуемого gold кнопкой (создание мушкетеров в казарме)
        /// </summary>
        private void funcTransfer()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            for (int j = startAcc; j <= numberOfAcc; j++)
            {
                if (check[j].IsActiveServer)
                {
                    check[j].ReOpenWindow();
                    check[j].Pause(1000);

                    DriversOfState drive = new DriversOfState(j);
                    drive.StateMuskPlus();
                }
            }
        }

        #endregion

        #region Silver Button (Pure Otite)   **************************

        private void PureOtite_Click(object sender, EventArgs e)
        {
            Thread myThreadSilver = new Thread(funcSilver);
            myThreadSilver.Start();

        }

        /// <summary>
        /// метод задает функционал для потока, организуемого аква кнопкой
        /// </summary>
        private void funcSilver()
        {
        //    int NumberOfWindow = numberOfAccglobalParam.TotalNumberOfAccounts + 1;
            Check check = new Check(1);
            //Check[] check = new Check[numberOfAcc + 1];
            //for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор


            DriversOfState drive = new DriversOfState(1);
            
            drive.StateGotoOldMan();  //подходим в Old Man

            while (true)
            {
                //check.ReOpenWindow();
                //if (check.isLogout())   //если окно находится в логауте
                //{
                //DriversOfState drive = new DriversOfState(NumberOfWindow);
                drive.StateOtitRun2();
                //}
            }

        }

        #endregion

        #region Golden Button (Кукуруза)

        /// <summary>
        /// добыча кукурузы на ферме (начало в диалоге с Эстебаном)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoldenEggFruit_Click(object sender, EventArgs e)
        {
            Thread myThreadGold2 = new Thread(funcGold2);
            myThreadGold2.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого золотой кнопкой
        /// </summary>
        private void funcGold2()
        {
            //int NumberOfWindow = globalParam.TotalNumberOfAccounts + 1;
            Check check = new Check(numberOfAcc+1);
            DriversOfState drive = new DriversOfState(numberOfAcc+1);

            check.ReOpenWindow();
            check.Pause(500);
            drive.StateGoldenEggFarm();
        }

        #endregion

        #region Undressing in Barack

        private void undressing_Click(object sender, EventArgs e)
        {
            Thread myThreadUndressing = new Thread(funcUndressing);
            myThreadUndressing.Start();
        }

        private void funcUndressing()
        {
            int currentAccount = numberOfAcc;

            Check check = new Check(currentAccount);
            check.ReOpenWindow();

            check.UnDressing();

        }

        #endregion

        #region Алхимия

        /// <summary>
        /// кнопка "Алхимия"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alchemy_Click(object sender, EventArgs e)
        {
            Thread myAlchemy = new Thread(funcAlchemy);
            myAlchemy.Start();

        }

        /// <summary>
        /// метод задает функционал для потока, организуемого кнопкой цвета "Коралл"
        /// </summary>
        private void funcAlchemy()
        {
            for (int j = 1; j <= numberOfAcc; j++)
            {
                Check check = new Check(j);
                if (check.IsActiveServer)            //надо ли грузить окно (по умолчанию)
                {
                    check.ReOpenWindow();
                    check.Pause(1000);
                    DriversOfState driver = new DriversOfState(j);
                    driver.StateAlchemy();             // продаёт всех ботов, которые стоят в данный момент в магазине (через движок состояний)
                }
            }
        }

        #endregion

        #region Гильдия охотников BH

        /// <summary>
        /// Кнопка BH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BH_Click(object sender, EventArgs e)
        {
            BH.Visible = false;

            Thread myBH = new Thread(funcBH);
            myBH.Start();

            BH.Visible = true;
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого кнопкой цвета "ForestGreen" (темно-зеленая) Infinity в BH
        /// </summary>
        private void funcBH()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            DateTime Data1 = DateTime.Now;

            while (true)
            {
                for (int j = startAcc; j <= numberOfAcc; j++)
                {
                    check[j].problemResolutionBH();
                }
                DateTime Data2 = DateTime.Now;
                if ((Data2 - Data1).Seconds < 5)            //если один проход программы был короче 5 сек, 
                    check[1].Pause(5000 - (Data2 - Data1).Milliseconds);    // то делаем паузу на недостающий промежуток времени
                Data1 = Data2;
            }


            
        }

        #endregion

        #region Загрузка Стимов

        private void LoadSteams_Click(object sender, EventArgs e)
        {
            Thread myLoadSteams = new Thread(funcLS);
            myLoadSteams.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого кнопкой цвета "ForestGreen" (темно-зеленая)
        /// </summary>
        private void funcLS()
        {
            for (int j = 1; j <= numberOfAcc; j++)
            {
                Check check = new Check(j);
                if (check.IsActiveServer)
                {
                    check.YellowButton();
                }
            }

        }

        #endregion

        #region Смена аккаунтов для Инфинити (или потоковая продажа всех аккаунтов)

        /// <summary>
        /// кнопка "Смена аккаунтов"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangingAccounts_Click_1(object sender, EventArgs e)
        {
            this.ChangingAccounts.BackColor = Color.OrangeRed;
            Thread myCA = new Thread(funcCA);
            myCA.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого кнопкой цвета 
        /// </summary>
        private void funcCA()
        {
            Check[] check = new Check[numberOfAcc + 1];
//            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор
            for (int j = 1; j <= 1; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            while (true)
            {
//                for (int j = startAcc; j <= numberOfAcc; j++)
                for (int j = 1; j <= 1; j++)
                {
                    check[j].ChangingAccounts();
                    if (globalParam.Infinity >= 423) globalParam.Infinity = 0;
                }
            }
        }

        #endregion

        public void labelNomer_Click(object sender, EventArgs e)
        {


        }

        private void numberOfAccouts_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// изменяет значение переменной startAcc при изменении счетчика в главной форме
        /// (присваиваем переменной класса значение, выбранное пользователем в форме)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAccount_Leave(object sender, EventArgs e)
        {
            startAcc = (int)this.startAccount.Value;
        }

        /// <summary>
        /// изменяет значение переменной startAcc при изменении счетчика в главной форме
        /// (присваиваем переменной класса значение, выбранное пользователем в форме)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAccount_ValueChanged(object sender, EventArgs e)
        {
            startAcc = (int)this.startAccount.Value;
        }

        #region Silver Button (Pure Otite Multi)

        private void PureOtiteMulti_Click(object sender, EventArgs e)
        {
            Thread myThreadSilverMulti = new Thread(funcSilverMulti);
            myThreadSilverMulti.Start();
        }

        /// <summary>
        /// метод задает функционал для потока, организуемого серебряной кнопкой
        /// </summary>
        private void funcSilverMulti()
        {
            Check[] check = new Check[numberOfAcc + 1];
            for (int j = startAcc; j <= numberOfAcc; j++) check[j] = new Check(j);   //проинициализировали check[j]. Сработал конструктор

            //DateTime Data1 = DateTime.Now;

            while (true)
            {
                for (int j = startAcc; j <= numberOfAcc; j++)
                {
                    check[j].problemResolutionOtite();
                }
                //DateTime Data2 = DateTime.Now;
                //if ((Data2 - Data1).Seconds < 15)                           //если один проход программы был короче 5 сек, 
                //    check[1].Pause(15000 - (Data2 - Data1).Milliseconds);   // то делаем паузу на недостающий промежуток времени
                //Data1 = Data2;
            }



        }



        #endregion
    }// END class MainForm 
}// END namespace OpenGEWindows




//        //=================================================== PostMessage ===============================================================
//        //===============================================================================================================================

//        private void button2_Click(object sender, EventArgs e)
//        {
//            button2.Visible = false;
////            BOOL WINAPI PostMessage(
////  _In_opt_ HWND   hWnd,
////  _In_     UINT   Msg,
////  _In_     WPARAM wParam,
////  _In_     LPARAM lParam
////);
//            //const int BM_SETSTATE = 243;
//            //const int WM_LBUTTONDOWN = 513;
//            //const int WM_LBUTTONUP = 514;
//            const int WM_KEYDOWN = 0x0100;
//            //const int WM_CHAR = 0x0102;
//            const int WM_KEYUP = 0x0101;
//            //const int WM_SETFOCUS = 7;
//            //const int WM_SYSCOMMAND = 274;
//            //const int SC_MINIMIZE = 32;


//            IntPtr VK_NUMPAD2 = (IntPtr)0x62;
////            IntPtr wParam = new IntPtr();
//            UIntPtr lParam = new UIntPtr();


//            //SendMessage(HWND, WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);

//            //wParam = VK_NUMPAD2;
//            //lParam = (IntPtr)0x00500001;
//            //PostMessage(HWND, WM_KEYDOWN, (IntPtr)0x62, (IntPtr)0x00500001);

//            ////wParam = (IntPtr)0x050;
//            ////lParam = (IntPtr)0x00500001;
//            ////PostMessage(HWND, WM_CHAR, wParam, lParam);
//            //wParam = VK_NUMPAD2;


//            Class_Timer.Pause(3000);

//            ////////// 1 /////////////////////////
//            //UIntPtr HWND = (UIntPtr)0x151B0372;
//            UIntPtr HWND2 = (UIntPtr)1769954;
//            UIntPtr HWND = FindWindowEx(HWND2, UIntPtr.Zero , "Edit", "");
//            //SendMessage(HWND, WM_SETFOCUS, UIntPtr.Zero, UIntPtr.Zero);

//            uint dd = 0x00500001;
//            lParam = (UIntPtr)dd;
//            PostMessage(HWND, WM_KEYDOWN, (UIntPtr)Keys.D2, lParam);

//            Class_Timer.Pause(150);

//            dd = 0xC0500001;
//            lParam = (UIntPtr)dd;
//            PostMessage(HWND, WM_KEYUP, (UIntPtr)Keys.D2, lParam);

//            ////////// 3 /////////////////////////
//            //UIntPtr HWND = (UIntPtr)853492;


//            //uint dd = 0x00170001;
//            //lParam = (UIntPtr)dd;

//            //PostMessage(HWND, WM_CHAR, (UIntPtr)Keys.I, lParam);
//            //Class_Timer.Pause(150);

//            //dd = 0xC0170001;
//            //lParam = (UIntPtr)dd;
//            //PostMessage(HWND, WM_CHAR, (UIntPtr)Keys.I, lParam);


//            ////////////////6//////////////////////////
//            //SendKeys.SendWait("i");                    //идёт неправильный сигнал в программу, но что-то идёт

//            ////////// 4 /////////////////////////
//            //Click_Mouse_and_Keyboard.ClickKey(0x50);
//            //Class_Timer.Pause(150);
//            //Click_Mouse_and_Keyboard.UnClickKey(0x50);

//            ////////// 5 /////////////////////////                // в прогу(ге) ничего не идёт
//            //UIntPtr HWND = (UIntPtr)853492;
//            //SendMessage(HWND, WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);
//            //Click_Mouse_and_Keyboard.GenerateKey(1,true);



//    /////////2/////////

//    //Class_Timer.Pause(2500);
//    //StandUp.PressKey(Keys.I);
//    //Class_Timer.Pause(500);
//    //StandUp.PressKey(Keys.K);



//    //PostMessage(HWND, WM_KEYUP, (IntPtr)0x72, (IntPtr)0xC03D0001);

//    button2.Visible = true;
//}

