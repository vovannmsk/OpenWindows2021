using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using GEBot.Data;


namespace OpenGEWindows
{
    public class botWindow
    {
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(UIntPtr myhWnd, int myhwndoptional, int xx, int yy, int cxx, int cyy, uint flagus); // Перемещает окно в заданные координаты с заданным размером

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(UIntPtr hWnd); // Делает окно активным

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(UIntPtr hWnd, int nCmdShow);  //раскрывает окно, если оно было скрыто в трей

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(UIntPtr hWnd); // Перемещает окно в верхний список Z порядка


        // ================ переменные класса =================

        /// <summary>
        /// номер окна
        /// </summary>
        private int numberWindow;       
        private const int WIDHT_WINDOW = 1024;
        private const int HIGHT_WINDOW = 700;

        private BotParam botParam;              //начальные данные для бота (заданные пользователем)
        private GlobalParam globalParam;
        //private IScriptDataBot scriptDataBot;
        //private int statusOfAtk;             //статус атаки (для BH)

        private Server server;
        private ServerFactory serverFactory;

        //private int counterMitridat;
        //private System.DateTime timeMitridat = System.DateTime.Now;

        private iPoint pointButtonClose;
        private iPoint pointOneMode;


        //enum TypeLoadUserData {txt, db}
        

        
        /// <summary>
        /// конструктор с пустыми параметрами
        /// </summary>
        public botWindow()
        {
            MessageBox.Show("НУЖНЫ ПАРАМЕТРЫ в botwindow");
        }

        /// <summary>
        /// основной конструктор
        /// </summary>
        /// <param name="number_Window">номер бота (номер окна)</param>
        public botWindow(int number_Window)
        {
            numberWindow = number_Window;     // эта инфа поступает при создании объекта класса
            botParam = new BotParam(numberWindow);
            globalParam = new GlobalParam();

            #region Вариант 1. переменные класса подчитываются из текстовых файлов

            //this.databot = LoadUserDataBot(TypeLoadUserData.txt);

            #endregion

            #region Вариант 2. переменные класса подчитываются из БД

            //this.databot = LoadUserDataBot(TypeLoadUserData.db);

            #endregion

            //this.statusOfAtk = GetStatusOfAtk();              //значение статуса, 1 - мы уже били босса, 0 - нет 

//            serverFactory = new ServerFactory(number_Window);
            serverFactory = new ServerFactory(this);
            server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)


            // точки для тыканья. универсально для всех серверов
            this.pointButtonClose = new Point(850 - 5 + botParam.X, 625 - 5 + botParam.Y);   //(848, 620);
            this.pointOneMode = new Point(123 - 5 + botParam.X, 489 - 5 + botParam.Y);    // 118, 484
        }

        // ============================== методы ============================================



        #region геттеры и сеттеры

        


        /// <summary>
        /// сеттер для statusOfAtk. Параллельно идет запись в файл
        /// </summary>
        /// <param name="status"></param>
        public void setStatusOfAtk(int status)
        {
            botParam.StatusOfAtk = status;
        }

        ///// <summary>
        ///// геттер для statusOfAtk 
        ///// </summary>
        ///// <returns></returns>
        //public int getStatusOfAtk()
        //{ return  botParam.StatusOfAtk; }

        /// <summary>
        /// номер окна
        /// </summary>
        public int getNumberWindow()
        { return this.numberWindow; }

        public int getX()
        { return botParam.X; }

        public int getY()
        { return botParam.Y; }

        public string getParam()
        {
            //string result = botParam.Parametrs[0];
            //if (globalParam.Infinity)
            //{
            //    result = botParam.Parametrs[botParam.NumberOfInfinity];
            //}
            //return result;
            return botParam.Parametrs[botParam.NumberOfInfinity];
        }

        public int getKanal()
        { return botParam.Kanal; }

        public int[] getTriangleX()
        { return botParam.TriangleX; }

        public int[] getTriangleY()
        { return botParam.TriangleY; }

        public int getNomerTeleport()
        { return botParam.NomerTeleport; }

        //public String getLogin()
        //{ return botParam.Login; }

        //public String getPassword()
        //{ return botParam.Password; }

        public String getNameOfFamily()
        { return botParam.NameOfFamily; }

        /// <summary>
        /// тип закупаемых патронов в городском автомате
        /// </summary>
        /// <returns></returns>
        public int getBullet()
        { 
            //return databot.Bullet;
            //return 0;  //пока возвращаем ноль, чтобы патроны не закупались автоматически
            return 1;  //временно стальные пули. Потом надо сделать файл типа "Логины", чтобы для каждого аккаунта закупался свой тип патронов
        }

        #endregion

        #region Общие методы

        /// <summary>
        /// Останавливает поток на некоторый период
        /// </summary>
        /// <param name="ms"> ms - период в милисекундах </param>
        public void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        /// <summary>
        /// эмулирует тройное нажатие кнопки "Esc"
        /// </summary>
        public void PressEscThreeTimes()
        {
            for (int i = 1; i <= 3; i++) PressEsc();
        }

        /// <summary>
        /// эмулирует нажатие кнопки "Esc"
        /// </summary>
        public void PressEsc()
        {
            TextSend.SendText2(1);           // нажимаем Esc
            Thread.Sleep(200);
        }

        #endregion

        #region No Window

        #endregion

        #region Logout

        ///// <summary>
        ///// вводим логин и пароль в соответствующие поля
        ///// </summary>
        //private void EnterLoginAndPasword()
        //{
        //    iPoint pointPassword = new Point(510 - 5 + botParam.X, 355 - 5 + botParam.Y);    //  505, 350
        //    // окно открылось, надо вставить логин и пароль
        //    pointPassword.PressMouseL();   //Кликаю в строчку с паролем
        //    //PressMouseL(505, 350);       //Кликаю в строчку с паролем
        //    Pause(500);
        //    pointPassword.PressMouseL();   //Кликаю в строчку с паролем
        //    //PressMouseL(505, 350);       //Кликаю в строчку с паролем
        //    Pause(500);
        //    SendKeys.SendWait("{TAB}");
        //    Pause(500);
        //    SendKeys.SendWait(botParam.Login);
        //    Pause(500);
        //    SendKeys.SendWait("{TAB}");
        //    Pause(500);
        //    SendKeys.SendWait(botParam.Password);
        //    Pause(500);
        //}

        ///// <summary>
        ///// нажимаем на кнопку Connect (окно в логауте)
        ///// </summary>
        ///// <returns>true=если удалось нажать,  false=если окно вылетело</returns>
        //private bool PressConnectButton()
        //{
        //    iPoint pointButtonConnect = new Point(595 - 5 + botParam.X, 485 - 5 + botParam.Y);    // кнопка "Connect" в логауте 
        //    if (server.isHwnd())
        //    {
        //        pointButtonConnect.PressMouseLL();   // Кликаю в Connect
        //        Pause(500);
        //        server.WriteToLogFileBH("Нажали на Коннект");
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        ///// <summary>
        ///// Нажимаем Коннект (переводим бота из состояния логаут в состояние казарма)
        ///// </summary>
        ///// <returns></returns>
        //public bool Connect()    // возвращает true, если успешно вошли в казарму
        //{

        //    #region новый вариант
        //    //bool result = true;
        //    //const int MAX_NUMBER_ITERATION = 4;    //максимальное количество итераций
        //    //uint count = 0;

        //    //iPointColor testColor = new PointColor(65 - 5 + databot.X, 55 - 5 + databot.Y, 7800000, 5);  //запоминаем цвет в координатах 55, 55 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
        //    //Pause(500);

        //    //do
        //    //{
        //    //    PressConnectButton();
        //    //    Pause(10000);
        //    //    if (isCheckBugs()) BugFixes();

        //    //    count++;
        //    //    if (count > MAX_NUMBER_ITERATION)
        //    //    {
        //    //        result = false;
        //    //        break;
        //    //    }
        //    //    //if (server.isBarack())
        //    //    //{
        //    //    //    result = true;
        //    //    //    break;
        //    //    //}
        //    //} while (!isChangeDisplay(testColor.GetPixelColor()));

        //    //return result;

        //    #endregion

        //    #region старый вариант (но рабочий)

        //    //server.WriteToLogFileBH("вошли в процедуру коннект");
        //    //server.WriteToLogFile(botParam.NumberOfInfinity + " " + botParam.Logins[botParam.NumberOfInfinity] + " " + botParam.Passwords[botParam.NumberOfInfinity] + " " + botParam.Parametrs[botParam.NumberOfInfinity]);
        //    //server.serverSelection();          //выбираем из списка свой сервер

        //    iPointColor point5050 = new PointColor(50 - 5 + botParam.X, 50 - 5 + botParam.Y, 7800000, 5);  //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
        //    iPoint pointButtonOk = new Point(525 - 5 + botParam.X, 410 - 5 + botParam.Y);    // кнопка Ok в логауте
        //    iPoint pointButtonOk2 = new Point(525 - 5 + botParam.X, 445 - 5 + botParam.Y);    // кнопка Ok в логауте

        //    uint Tek_Color1;
        //    uint Test_Color = 0;
        //    bool ColorBOOL = true;
        //    uint currentColor = 0;
        //    const int MAX_NUMBER_ITERATION = 4;  //максимальное количество итераций

        //    bool aa = true;

        //    Test_Color = point5050.GetPixelColor();       //запоминаем цвет в координатах 50, 50 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
        //    Tek_Color1 = Test_Color;

        //    ColorBOOL = (Test_Color == Tek_Color1);
        //    int counter = 0; //счетчик

        //    pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
        //    Pause(500);
        //    pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
        //    Pause(500);

        //    bool IsServerSelected = server.serverSelection();          //выбираем из списка свой сервер
        //    if (!IsServerSelected) return false; //если не смогли выбрать сервер, то выход из метода с результатом false

        //    server.WriteToLogFileBH("дошли до while");
        //    while ((aa | (ColorBOOL)) & (counter < MAX_NUMBER_ITERATION))
        //    {
        //        counter++;  //счетчик

        //        Tek_Color1 = point5050.GetPixelColor();
        //        ColorBOOL = (Test_Color == Tek_Color1);

        //        server.WriteToLogFileBH("нажимаем на кнопку connect");

        //        //pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
        //        //Pause(500);
        //        //pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
        //        //Pause(500);
        //        //server.serverSelection();          //выбираем из списка свой сервер
        //        if (PressConnectButton())
        //        {
        //            Pause(1500);
        //        }
        //        else
        //            return false;

        //        //pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"
        //        //Pause(500);
        //        //pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min
        //        //Pause(500);

        //        //если есть ошибки в логине-пароле, то возникает сообщение с кнопкой "OK". 

        //        if (server.isPointConnect())                                         // Обработка Ошибок.
        //        {
        //            pointButtonOk.PressMouse();  //кликаю в кнопку  "ОК"
        //            Pause(500);

        //            if (server.isPointConnect())   //проверяем, выскочила ли форма с кнопкой ОК
        //            {
        //                pointButtonOk.PressMouse();  //кликаю в кнопку  "ОК"
        //                Pause(500);
        //            }
        //            pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"

        //            pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" 3 min

        //            EnterLoginAndPasword();
        //        }
        //        else
        //        {
        //            aa = false;
        //        }

        //    }

        //    bool result = true;
        //    Pause(5000);
        //    currentColor = point5050.GetPixelColor();
        //    if (currentColor == Test_Color)      //проверка входа в казарму. 
        //    {
        //        //тыкнуть в Quit 
        //        //PressMouseL(600, 530);          //если не вошли в казарму, то значит зависли и жмем кнопку Quit
        //        //PressMouseL(600, 530);
        //        result = false;
        //    }
        //    return result;

        //    #endregion

        //}

        //#region методы для нового варианта Connect

        ///// <summary>
        ///// исправление ошибок при нажатии кнопки Connect (бот в логауте)
        ///// </summary>
        //private void BugFixes()
        //{
        //    iPoint pointButtonOk = new Point(525 - 5 + botParam.X, 410 - 5 + botParam.Y);    // кнопка Ok в логауте
        //    iPoint pointButtonOk2 = new Point(525 - 5 + botParam.X, 445 - 5 + botParam.Y);    // кнопка Ok в логауте

        //    pointButtonOk.PressMouse();   //кликаю в кнопку  "ОК"
        //    Pause(500);

        //    pointButtonOk.PressMouseL();  //кликаю в кнопку  "ОК"  второй раз (их может быть две)
        //    Pause(500);

        //    pointButtonOk2.PressMouseL();  //кликаю в кнопку  "ОК" другой формы (где написано про 3 min)

        //    EnterLoginAndPasword();        //вводим логин и пароль заново
        //}

        ///// <summary>
        ///// проверяем, есть ли проблемы после нажатия кнопки Connect (выскачила форма с кнопкой ОК)
        ///// </summary>
        ///// <returns></returns>
        //private bool isCheckBugs()
        //{ return server.isPointConnect(); }

        ///// <summary>
        ///// проверяем, сменилось ли изображение на экране
        ///// </summary>
        ///// <param name="testColor">тестовая точка</param>
        ///// <returns>true, если сменился экран</returns>
        //private bool isChangeDisplay(uint testColor)
        //{
        //    iPointColor currentColor = new PointColor(65 - 5 + botParam.X, 55 - 5 + botParam.Y, 7800000, 5);
        //    uint color = currentColor.GetPixelColor();
        //    bool result = (color == testColor);
        //    return !result;
        //}

        //#endregion

        ///// <summary>
        ///// Нажимаем Коннект (переводим бота из состояния логаут в состояние казарма)
        ///// </summary>
        ///// <returns></returns>
        //public bool ConnectBH()    // возвращает true, если успешно вошли в казарму
        //{

        //    bool result = true;
        //    const int MAX_NUMBER_ITERATION = 4;    //максимальное количество итераций
        //    uint count = 0;

        //    iPointColor testColor = new PointColor(65 - 5 + botParam.X, 55 - 5 + botParam.Y, 7800000, 5);  //запоминаем цвет в координатах 55, 55 для проверки того, сменился ли экран (т.е. принят ли логин-пароль)
        //    Pause(500);

        //    do
        //    {
        //        PressConnectButton();
        //        Pause(10000);
        //        if (isCheckBugs()) BugFixes();

        //        count++;
        //        if (count > MAX_NUMBER_ITERATION)
        //        {
        //            result = false;
        //            break;
        //        }
        //        //if (server.isBarack())
        //        //{
        //        //    result = true;
        //        //    break;
        //        //}
        //    } while (!isChangeDisplay(testColor.GetPixelColor()));

        //    return result;

        //}

        ///// <summary>
        ///// нажимаем кнопку Leave Game в Логауте
        ///// </summary>
        //public void LeaveGame()
        //{
        //    new Point(605 - 5 + botParam.X, 535 - 5 + botParam.Y).PressMouseL();    // кнопка LeaveGame в логауте
        //}


        #endregion

        #region Pet
        #endregion

        #region TopMenu
        #endregion

        #region Shop
        #endregion

        #region atWork

        /// <summary>
        /// расстановка героев треугольником
        /// </summary>
        public void Placement()
        {
            iPoint pointFirstHero = new Point(botParam.TriangleX[0] + botParam.X, botParam.TriangleY[0] + botParam.Y);
            iPoint pointSecondHero = new Point(botParam.TriangleX[1] + botParam.X, botParam.TriangleY[1] + botParam.Y);
            iPoint pointThirdHero = new Point(botParam.TriangleX[2] + botParam.X, botParam.TriangleY[2] + botParam.Y);

            // ============= нажимаем на первого перса (обязательно на точку ниже открытой карты)
            FirstHero();
            Pause(500);  //1000
            pointFirstHero.PressMouseL();
            Pause(500);  //1000
            //PressMouseL(databot.TriangleX[0], databot.TriangleY[0]);

            // ============= нажимаем на третьего перса (обязательно на точку ниже открытой карты)
            ThirdHero();
            Pause(500);  //1000
            pointThirdHero.PressMouseL();
            Pause(500);
            //PressMouseL(databot.TriangleX[2], databot.TriangleY[2]);

            // ============= нажимаем на второго перса (обязательно на точку ниже открытой карты)
            SecondHero();
            Pause(500);  //1000
            pointSecondHero.PressMouseL();
            Pause(500);
            //PressMouseL(databot.TriangleX[1], databot.TriangleY[1]);

            // ============= закрыть карту через Esc =======================
            CloseMap();
            //Pause(1500);
        }

        /// <summary>
        /// выбрать первого (левого) бойца из тройки
        /// </summary>
        public void FirstHero()
        {
            //iPoint pointFirstHeroUp = new Point(155 - 5 + botParam.X, 640 - 5 + botParam.Y);       // верхняя точка
            //iPoint pointFirstHeroDown = new Point(155 - 5 + botParam.X, 682 - 5 + botParam.Y);     // нижняя точка
            iPoint pointFirstHeroUp = new Point(213 - 5 + botParam.X, 636 - 5 + botParam.Y);       // верхняя точка
            iPoint pointFirstHeroDown = new Point(213 - 5 + botParam.X, 701 - 5 + botParam.Y);     // нижняя точка
            pointFirstHeroDown.PressMouseL();  
            pointFirstHeroDown.PressMouseL();
            Pause(500);
            pointFirstHeroUp.PressMouseL();
        }

        /// <summary>
        /// выбрать второго (среднего) бойца из тройки
        /// </summary>
        public void SecondHero()
        {
            //iPoint pointSecondHeroUp = new Point(408 - 5 + botParam.X, 640 - 5 + botParam.Y);  
            //iPoint pointSecondHeroDown = new Point(408 - 5 + botParam.X, 682 - 5 + botParam.Y);
            iPoint pointSecondHeroUp = new Point(472 - 5 + botParam.X, 636 - 5 + botParam.Y);    
            iPoint pointSecondHeroDown = new Point(472 - 5 + botParam.X, 701 - 5 + botParam.Y);  

            pointSecondHeroDown.PressMouseL();
            pointSecondHeroDown.PressMouseL();
            Pause(500);
            pointSecondHeroUp.PressMouseL();
        }

        /// <summary>
        /// выбрать третьего (правого) бойца из тройки
        /// </summary>
        public void ThirdHero()
        {
            //iPoint pointThirdHeroUp = new Point(663 - 5 + botParam.X, 640 - 5 + botParam.Y);    // 670, 635
            //iPoint pointThirdHeroDown = new Point(663 - 5 + botParam.X, 682 - 5 + botParam.Y);    // 670, 664
            iPoint pointThirdHeroUp = new Point(723 - 5 + botParam.X, 636 - 5 + botParam.Y);    // 670, 635
            iPoint pointThirdHeroDown = new Point(723 - 5 + botParam.X, 701 - 5 + botParam.Y);    // 670, 664

            pointThirdHeroDown.PressMouseL();
            pointThirdHeroDown.PressMouseL();
            Pause(500);
            pointThirdHeroUp.PressMouseL();
        }

        /// <summary>
        /// проверяет включен ли командный режим или нет 
        /// </summary>
        /// <returns> true, если командный режим включен </returns>
        public bool isCommandMode()
        {
            iPointColor pointCommandMode = new PointColor(123 - 5 + botParam.X, 479 - 5 + botParam.Y, 8000000, 6);
            return pointCommandMode.isColor2();
        }

        /// <summary>
        /// перевод бота в одиночный режим 
        /// </summary>
        public void OneMode()
        {
            if (isCommandMode())
            {
                // если включен командный режим, то надо нажать 1 раз
                pointOneMode.PressMouse();
                //PressMouse(118, 484); 
            }
            else
            {   // если включен одиночный режим, то надо нажать два раза (в командный режим и обратно)
                pointOneMode.PressMouse();
                //PressMouse(118, 484);
                Pause(500);
                //PressMouse(118, 484);
                pointOneMode.PressMouse();
            }
        }

        /// <summary>
        /// закрыть карту Alt+Z
        /// </summary>
        private void CloseMap()
        {
            PressEscThreeTimes();
        }

        /// <summary>
        /// перевод в командный режим из любого положения 
        /// </summary>
        public void CommandMode()
        {
            if (!isCommandMode())
            {   // если включен одиночный режим
                pointOneMode.PressMouse();
                //PressMouse(118, 484);
                Pause(500);
            }
        }

        /// <summary>
        /// нажимаем пробел (переход в боевой режим)
        /// </summary>
        public void ClickSpace()
        {
            iPoint pointBattleMode = new Point(190 - 5 + botParam.X, 530 - 5 + botParam.Y);    //  185, 525
            pointBattleMode.PressMouse();  // Кликаю на кнопку "боевой режим"
        }

        /// <summary>
        /// нажимаем пробел (переход в боевой режим)
        /// </summary>
        public void ClickSpaceBH()
        {
            iPoint pointBattleMode = new Point(190 - 5 + botParam.X, 530 - 5 + botParam.Y);    //  185, 525
            pointBattleMode.PressMouseL();  // Кликаю на кнопку "боевой режим"
        }

        /// <summary>
        /// Лечение одного окна, если побили всех персов (лечение состоит в закрытии окна с ботом)
        /// </summary>
        public void CureOneWindow()
        {
            // ================================= убирает все лишние окна с экрана =========================================
            PressEscThreeTimes();
            Pause(1000);

            if (globalParam.Windows10)
            {
                server.GoToEnd();
                Pause(10000);  
            }
            else
            {
                server.Logout();
            }
        }

        /// <summary>
        /// Лечение одного окна, если побили часть персов (лечение состоит в закрытии окна с ботом)
        /// </summary>
        public void CureOneWindow2()
        {
            iPoint pointEnterBattleMode = new Point(205 - 5 + botParam.X, 205 - 5 + botParam.Y);    // 200, 200
            // ================================= убирает все лишние окна с экрана =========================================
            PressEscThreeTimes();
            Pause(1000);

            //========================================= в командный режим =================================================
            CommandMode();
            Pause(1000);

            //=============================== выйти из боевого режима (бежим в сторону) ============================================

            pointEnterBattleMode.PressMouseL();//отбегаю ботами в сторону, чтобы они вышли из боевого режима
            pointEnterBattleMode.PressMouseL();
            Pause(2000);

            if (globalParam.Windows10)
            {
                server.GoToEnd();
                Pause(10000);  
            }
            else
            {
                server.Logout();
            }
        }

        /// <summary>
        /// Нажать на бутылку митридата, которая лежит во второй ячейке                                перенести в server
        /// </summary>
        public void PressMitridat()
        {
            iPoint pointPanel = new Point(38 - 5 + botParam.X, 486 - 5 + botParam.Y);    // 33, 481
            //iPoint pointFirstBox = new Point(31 - 5 + botParam.X, 110 - 5 + botParam.Y);
            //iPoint pointSecondBox = new Point(31 - 5 + botParam.X, 140 - 5 + botParam.Y);
            //iPoint pointThirdBox = new Point(31 - 5 + botParam.X, 170 - 5 + botParam.Y);
            //iPoint pointFourthBox = new Point(31 - 5 + botParam.X, 200 - 5 + botParam.Y);
            //iPoint pointFifthBox = new Point(31 - 5 + botParam.X, 230 - 5 + botParam.Y);

            //System.DateTime timeNow = DateTime.Now;  //текущее время
            //System.TimeSpan PeriodMitridat = timeNow.Subtract(timeMitridat);   //сколько времени прошло с последнего применения митридата
            //uint PeriodMitridatSeconds = (uint)PeriodMitridat.TotalSeconds;          //сколько времени прошло с последнего применения митридата в секундах
            //if ((PeriodMitridatSeconds >= 600) | (counterMitridat == 0))
            //{

            pointPanel.PressMouseR();                   // Кликаю правой кнопкой в панель с бытылками, чтобы сделать ее активной и поверх всех окон (группа может мешать)
            PressBoxInLeftPanel(2);
            PressBoxInLeftPanel(3);
            //pointSecondBox.PressMouseL();               // тыкаю в митридат (вторая ячейка)
            //Pause(200);
            //pointThirdBox.PressMouseL();                // тыкаю в  (третья ячейка)
            //Pause(200);
            //pointFourthBox.PressMouseL();                // тыкаю в  (4-ю ячейку)
            //pointFifthBox.PressMouseL();                // тыкаю в  (5-ю ячейку)


            //if (server.isSecondHero())                  //если есть второй перс ()
            //{
            //    SecondHero();                           //выбираю главным второго героя (это нужно, чтобы не было тормозов) типа надо нажать в экран после митридата
            //    Pause(500);
            //}
            //else                                        //если бегаем одним персом
            //{
            //    FirstHero();
            //    Pause(500);
            //}

            //    timeMitridat = DateTime.Now;              //обновляю время, когда был применен митридат
            //    counterMitridat++;
            //}
        }

        /// <summary>
        /// нажимаем на указанную ячейку панели в левом верхнем углу
        /// </summary>
        /// <param name="NumberOfBox">номер ячейки</param>
        public void PressBoxInLeftPanel(int NumberOfBox)
        {
            new Point(31 - 5 + botParam.X, 114 - 5 + (NumberOfBox - 1) * 32 + botParam.Y).PressMouseL();
        }

        #endregion

        #region inTown

        /// <summary>
        /// отодвинуть мышку в сторону, чтобы она не загораживала проверяемые точки
        /// </summary>
        public void ToMoveMouse()
        {
            iPoint pointToMoveMouse = new Point(205 - 5 + botParam.X, 575 - 5 + botParam.Y);    //
            pointToMoveMouse.PressMouseR();
        }

        /// <summary>
        /// бафаемся (хрин+принципал+элики славы)                    
        /// </summary>
        public void ActiveAllBuffBH()
        {
            //iPoint pointPanel = new Point(38 - 5 + botParam.X, 486 - 5 + botParam.Y);    // 33, 481
            //pointPanel.PressMouseR();                   // Кликаю правой кнопкой в панель с бытылками, чтобы сделать ее активной и поверх всех окон (группа может мешать)

            PressBoxInLeftPanel(5);
            PressBoxInLeftPanel(6);
            PressBoxInLeftPanel(7);

        }

        #endregion

        #region Barack

        ///// <summary>
        ///// начать с выхода в город (нажать на кнопку "начать с нового места")
        ///// </summary>
        //public void NewPlace()
        //{
        //    iPoint pointNewPlace = new Point(85 + databot.X, 670 + databot.Y); //85, 670);
        //    pointNewPlace.DoubleClickL();
        //}

        /// <summary>
        /// Нажимаем Выбор канала и группы персов в казарме 
        /// </summary>
        public void SelectChannel()
        {
            iPoint pointChoiceOfChannel = new Point(125 + botParam.X, 660 + (botParam.Kanal - 1) * 15 + server.sdvig() + botParam.Y);    //переход на нужный канал в казарме
            iPoint pointButtonSelectChannel = new Point(125 + botParam.X, 705 + botParam.Y); //   125, 705);

            pointButtonSelectChannel.PressMouseL();
            Pause(500);
            pointChoiceOfChannel.PressMouseL();
            Pause(500);
        }

        /// <summary>
        /// Нажимаем Выбор канала и группы персов в казарме 
        /// </summary>
        public void SelectChannel(int channel)
        {
            iPoint pointChoiceOfChannel = new Point(125 + botParam.X, 660 + (channel - 1) * 15 + server.sdvig() + botParam.Y);    //переход на указанный канал
            iPoint pointButtonSelectChannel = new Point(125 + botParam.X, 705 + botParam.Y); //   125, 705);
            pointButtonSelectChannel.PressMouseL();
            pointChoiceOfChannel.PressMouseL();
        }

        #endregion

        #region новые боты
        #endregion

        #region кратер
        #endregion

        #region заточка
        #endregion

        #region чиповка
        #endregion

        #region Personal Trade 
        #endregion

        #region BH

        /// <summary>
        /// активируем окно для БХ
        /// </summary>
        public void ActiveWindowBH()
        {
            ShowWindow(botParam.Hwnd, 9);                                       // Разворачивает окно если свернуто  было 9
            SetForegroundWindow(botParam.Hwnd);                                 // Перемещает окно в верхний список Z порядка     
            //перемещаем окно в заданные для него координаты. не учитываются ширина и высота окна
            SetWindowPos(botParam.Hwnd, 0, botParam.X, botParam.Y, WIDHT_WINDOW, HIGHT_WINDOW, 0x0001);
        }


        #endregion
    }
}
