using System;
using System.IO;
using System.Linq;
using System.Collections;

namespace GEBot.Data
{
    /// <summary>
    /// Индивидуальные параметры конкретного бота
    /// </summary>
    public class BotParam
    {
        
        ///// <summary>
        ///// номер аккаунта в списке аккаунтов п/п (нумерация с нуля). 
        ///// </summary>
        //private static int infinity;
        private string directoryOfMyProgram;
        private int numberOfWindow;

        private string login;
        private string[] logins;
        private string password;
        private string[] passwords;
        private int x;
        private int y;
        //********отключаем временно для проверки*********************************************************
        /// <summary>
        /// текущий сервер: синг, америка или европа
        /// </summary>
        private string param;
        /// <summary>
        /// список параметров: синг, америка или европа
        /// </summary>
        private string[] parametrs;              
        private UIntPtr hwnd;
        private int kanal;
        /// <summary>
        /// номер телепорта, где продаваться
        /// </summary>
        private int nomerTeleport;          
        private string nameOfFamily;
        private int[] triangleX;
        private int[] triangleY;
        //private int bullet;                     //пока не используется
        private int statusOfAtk;
        /// <summary>
        /// номер бота в списке ботов (хранится в файле Инфинити.txt)
        /// </summary>
        private int numberOfInfinity;
        //private bool infinity;
        private int howManyCyclesToSkip;           //сколько пропустить циклов. Для БХ 
        private int lengthOfList;
        private int statusOfSale;                       // статус продажи (для BH)
        private int numberOfRoute;
        /// <summary>
        /// стадия выполнения миссии Демоник
        /// </summary>
        private int stage;
        /// <summary>
        /// на каком месте стоит скаут (для классик)  (0 - если без скаута)
        /// </summary>
        private int scout;
        /// <summary>
        /// карта, на которой стоим на боте (для классик)
        /// TetraCatacomb (0)    PortoBello (1)   LagoDeTres (2)   GaviGivi (3)   Torsche (4)    Rion(5)  Crater (6)   Lava (7)
        /// </summary>
        private int map;
        /// <summary>
        /// текущий логин
        /// </summary>
        public string Login { get => LoginFromFile(); }
        public string[] Logins { get => logins; }
        /// <summary>
        /// текущий пароль
        /// </summary>
        public string Password { get => PasswordFromFile(); }
        public string[] Passwords { get => passwords; }
        /// <summary>
        /// смещение окна относительно левого верхнего угла
        /// </summary>
        public int X { get => x; }
        /// <summary>
        /// смещение окна относительно левого верхнего угла
        /// </summary>
        public int Y { get => y; }
        /// <summary>
        /// текущий сервер: синг, америка или европа
        /// </summary>
        public string Param { get => ParamFromFile(); }
        public string[] Parametrs { get => parametrs; }
//        public UIntPtr Hwnd { get => hwnd; set { hwnd = value; SetHwnd(); } }
        public UIntPtr Hwnd { get { hwnd = GetHwnd(); return hwnd; }        set { hwnd = value; SetHwnd(); } }
        public int Kanal { get => kanal; }
        public int NomerTeleport { get => nomerTeleport; }
        public string NameOfFamily { get => nameOfFamily; }
        public int[] TriangleX { get => triangleX; }
        public int[] TriangleY { get => triangleY; }
        //public int Bullet { get => bullet; }
//        public int StatusOfAtk { get => statusOfAtk; set { statusOfAtk = value; SetStatusAtkInFile(); } }
        public int StatusOfAtk { get { statusOfAtk = GetStatusOfAtk(); return statusOfAtk;} set { statusOfAtk = value; SetStatusAtkInFile(); } }

        /// <summary>
        /// номер аккаунта в списке аккаунтов п/п (нумерация с нуля). 
        /// </summary>
        public int NumberOfInfinity { get { numberOfInfinity = NumberFromFile(); return numberOfInfinity; }
                                      set { numberOfInfinity = value; NumberToFile(); } }

//        public int HowManyCyclesToSkip { get => howManyCyclesToSkip; set => howManyCyclesToSkip = value; }
        public int HowManyCyclesToSkip { get { howManyCyclesToSkip = Get_HowManyCyclesToSkip(); return howManyCyclesToSkip; }
                                         set { howManyCyclesToSkip = value; Set_HowManyCyclesToSkip(); } }
        public int LengthOfList { get => lengthOfList; }
        /// <summary>
        /// статус продажи. Мы на стадии продажи инвентаря или нет (для Инфинити)
        /// </summary>
        public int StatusOfSale
        {
            get { statusOfSale = GetStatusOfSale(); return statusOfSale; }
            set { statusOfSale = value; SetStatusInFile(); }
        }
        /// <summary>
        /// номер маршрута (для добычи отита)
        /// </summary>
        public int NumberOfRoute { get => numberOfRoute; set => numberOfRoute = value; }
        /// <summary>
        /// стадия выполнения миссии Демоник
        /// </summary>
        public int Stage    {  
                                get { stage = GetStage(); return stage; }
                                set { stage = value; SetStage(); }
        }
        /// <summary>
        /// на каком месте стоит баффер (для ГЕ классик)
        /// </summary>
        public int Scout { get => scout; }
        /// <summary>
        /// карта, на которой стоим на боте (для классик)
        /// TetraCatacomb (0)    PortoBello (1)   LagoDeTres (2)   GaviGivi (3)   Torsche (4)    Rion(5)  Crater (6)   Lava (7)
        /// </summary>
        public int Map { get => map; }

        ///// <summary>
        ///// номер аккаунта в списке аккаунтов п/п (нумерация с нуля) 
        ///// </summary>
        //public static int Infinity { get => infinity; set => infinity = value; }



        //public bool Infinity { get => infinity; set => infinity = value; }

        //**********************************************************************************************
        /// <summary>
        /// конструктор
        /// </summary>
        public BotParam(int numberOfWindow)
        {
            //globalParam = new GlobalParam();
            //Infinity = globalParam.Infinity;
            //directoryOfMyProgram = globalParam.DirectoryOfMyProgram;
            this.directoryOfMyProgram = "C:\\!! Суперпрограмма V&K\\";
            this.numberOfWindow = numberOfWindow;

            this.x = Koord_X();
            this.y = Koord_Y();
            numberOfInfinity = NumberFromFile();
            login = LoginFromFile();
            logins = LoginsFromFile();
            password = PasswordFromFile();
            passwords = PasswordsFromFile();
            hwnd = GetHwnd();
            param = ParamFromFile();
            parametrs = ParametrsFromFile();
            kanal = Channal();
            nomerTeleport = NomerTeleporta();     
            nameOfFamily = LoadNameOfFamily();
            triangleX = LoadTriangleX();
            triangleY = LoadTriangleY();
            statusOfAtk = GetStatusOfAtk();
            //this.bullet = NumberOfBullets();                       //пока не используется
            lengthOfList = logins.Length;
            HowManyCyclesToSkip = 0;
            //infinity = numberOfInfinity;
            stage = GetStage();
            scout = GetScout();
            map = GetMap();
        }

        //===================================== методы ==========================================
        //===================================== методы ==========================================
        //===================================== методы ==========================================

        /// <summary>
        /// читаем значение из файла
        /// </summary>
        /// <returns></returns>
        private int GetMap()
        {
            string number = File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\Карта.txt");
            if (number.Length == 0) return 0;
            else return int.Parse(number);
        }

        /// <summary>
        /// читаем значение из файла
        /// </summary>
        /// <returns></returns>
        private int GetScout()
        {
            string number = File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\Скаут.txt");
            if (number.Length == 0) return 0;
            else return int.Parse(number);
        }

        /// <summary>
        /// читаем значение из файла
        /// </summary>
        /// <returns></returns>
        private int GetStage()
        { 
            //return int.Parse(File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\Stage.txt"));
            
            string number = File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\Stage.txt");
            if (number.Length == 0)
                return 1;
            else
                return int.Parse(number);
        }

        /// <summary>
        /// записываем значение в файл
        /// </summary>
        private void SetStage()
        {
            File.WriteAllText(directoryOfMyProgram + numberOfWindow + "\\Stage.txt", this.stage.ToString());
        }

        /// <summary>
        /// читаем значение из файла
        /// </summary>
        /// <returns></returns>
        private int Get_HowManyCyclesToSkip()
        { 
            //return int.Parse(File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\HowManyCyclesToSkip.txt"));

            string number = File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\HowManyCyclesToSkip.txt");
            if (number.Length == 0)
            {
                Set_HowManyCyclesToSkip("0");     
                return 0;
            }
            else
                return int.Parse(number);
        }

        /// <summary>
        /// записываем значение в файл
        /// </summary>
        private void Set_HowManyCyclesToSkip()
        {
            File.WriteAllText(directoryOfMyProgram + numberOfWindow + "\\HowManyCyclesToSkip.txt", this.howManyCyclesToSkip.ToString());
        }

        /// <summary>
        /// записываем значение в файл
        /// </summary>
        private void Set_HowManyCyclesToSkip(string HowManyCycles)
        {
            File.WriteAllText(directoryOfMyProgram + numberOfWindow + "\\HowManyCyclesToSkip.txt", HowManyCycles);
        }


        /// <summary>
        /// метод считывает значение статуса из файла, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        /// </summary>
        /// <returns></returns>
        private int GetStatusOfSale()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + numberOfWindow + "\\StatusOfSale.txt")); }

        /// <summary>
        /// метод записывает значение статуса в файл, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        /// </summary>
        /// <returns></returns>
        private void SetStatusInFile()
        {
            File.WriteAllText(directoryOfMyProgram + numberOfWindow + "\\StatusOfSale.txt", this.statusOfSale.ToString());
        }




        //#region global getters
        //public int getNintendo()
        //{
        //    return globalParam.Nintendo;
        //}
        //public string getDirectory()
        //{
        //    return globalParam.DirectoryOfMyProgram;
        //}

        //#endregion

        ///// <summary>
        ///// прочитать из файла список логинов
        ///// </summary>
        ///// <returns></returns>
        //private int LengthOfFileLogin()
        //{
        //    return File.length    (directoryOfMyProgram + this.numberOfWindow + "\\Логины.txt");
        //}


        ///// <summary>
        ///// закончился ли список ботов для данного окна?
        ///// </summary>
        ///// <returns>true, если закончился</returns>
        //public bool EndOfList()
        //{
        //    bool result = false;
        //    if (NumberOfInfinity >= LengthOfList)    result = true;
        //    return result;
        //}

        /// <summary>
        /// прочитать из файла текущий параметр (америка, европа, синг) для текущего окна
        /// </summary>
        /// <returns>список параметров</returns>
        private string ParamFromFile()
        {
            string[] result = File.ReadAllLines(directoryOfMyProgram + numberOfWindow + "\\Параметр.txt");
            if (result[numberOfInfinity].Length == 0)    //если искомое значение оказалось пустым, то читаем массив из файла еще раз (надеюсь поможет)
            {
                result = File.ReadAllLines(directoryOfMyProgram + numberOfWindow + "\\Параметр.txt");
            }
            return result[numberOfInfinity];
        }


        /// <summary>
        /// прочитать из файла список параметров (америка, европа, синг) /возвращает весь массив/
        /// </summary>
        /// <returns>список параметров</returns>
        private string[] ParametrsFromFile()
        {
            return File.ReadAllLines(directoryOfMyProgram + numberOfWindow + "\\Параметр.txt");
        }

        /// <summary>
        /// запись в файл числа Инфинити (номер строки с логином и паролем в файле Логины.txt и Пароли.txt)
        /// </summary>
        public void NumberToFile()
        { File.WriteAllText(directoryOfMyProgram + numberOfWindow + "\\Инфинити.txt", numberOfInfinity.ToString()); }

        /// <summary>
        /// прочитать из файла номер инфинити (номер бота в списке ботов)
        /// </summary>
        /// <returns>номер бота в списке ботов</returns> 
        private int NumberFromFile()   // каталог и номер окна
        {
            string result = File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Инфинити.txt");
            if (result.Length == 0)    //получили из файла пустую строку
                result = File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Инфинити.txt");    //читаем еще раз

            if (result.Length == 0)    //получили из файла пустую строку
                return 0;               // можно возвращать ДемоникОт
            else
                return int.Parse(result);
        }

        /// <summary>
        /// прочитать из файла список логинов
        /// </summary>
        /// <returns></returns>
        private string[] LoginsFromFile()
        {
            return File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Логины.txt");
        }

        /// <summary>
        /// прочитать из файла список паролей
        /// </summary>
        /// <returns></returns>
        private string[] PasswordsFromFile()
        {
            return File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Пароли.txt");
        }

        /// <summary>
        /// изменяем Hwnd окна и записываем в файл
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd()
        { File.WriteAllText(directoryOfMyProgram + this.numberOfWindow + "\\HWND.txt", this.hwnd.ToString()); }

        /// <summary>
        /// записываем в файл Hwnd
        /// </summary>
        
        public void SetHwnd(String HwndStr)
        { File.WriteAllText(directoryOfMyProgram + this.numberOfWindow + "\\HWND.txt", HwndStr); }


        /// <summary>
        /// метод записывает значение статуса в файл, 1 - мы уже приступили у убиванию босса, 0 - нет 
        /// </summary>
        /// <returns> 1 - мы уже приступили у убиванию босса, 0 - нет </returns>
        private void SetStatusAtkInFile()
        {
            File.WriteAllText(directoryOfMyProgram + this.numberOfWindow + "\\StatusOfAtk.txt", this.statusOfAtk.ToString());
        }

        /// <summary>
        /// функция возвращает смещение по оси X окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_X()
        {
            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };
            //int[] koordX = { 5, 5, 55, 80, 105, 130, 155, 180, 205, 5, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };

            return koordX[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает смещение по оси Y окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_Y()   // каталог и номер окна
        {
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 5, 30, 55, 80, 105, 130, 5 };
            //int[] koordY = { 5, 5, 55, 80, 105, 130, 155, 180, 205, 5, 255, 280, 305, 5, 30, 55, 80, 105, 130, 55 };
            return koordY[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает логин бота
        /// </summary>
        /// <returns></returns>
        private string LoginFromFile()   
        {
            string[] result = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Логины.txt");
            //return result[0];    //старый ненужный вариант
            if (result[numberOfInfinity].Length == 0)    //если искомое значение оказалось пустым, то читаем массив из файла еще раз (надеюсь поможет)
            {
                result = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Логины.txt");
            }

            return result[numberOfInfinity];
            //return File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Логины.txt");
        }

        /// <summary>
        /// функция возвращает пароль от бота
        /// </summary>
        /// <returns></returns>
        private string PasswordFromFile()   // каталог и номер окна
        {
            string[] result = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Пароли.txt");

            if (result[numberOfInfinity].Length == 0)    //если искомое значение оказалось пустым, то читаем массив из файла еще раз (надеюсь поможет)
                result = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Пароли.txt");

            return result[numberOfInfinity];
        }

        /// <summary>
        /// функция возвращает hwnd бота
        /// </summary>
        /// <returns></returns>
        private UIntPtr GetHwnd()
        {
            String HwndFromFile = File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\HWND.txt");
            if (HwndFromFile.Equals(""))
            {
                SetHwnd("222222");
                return (UIntPtr)2222222;   //если пусто в файле, то hwnd = 2222222;
            }
            else
            {
                uint uintHwnd = uint.Parse(HwndFromFile);   //преобразование из текста в целое положительное
                return (UIntPtr)uintHwnd;                   //преобразование из целого в формат UIntPtr (родной Hwnd)
            }
        }

        ///// <summary>
        ///// функция возвращает имя сервера (Америка, европа или Синг)
        ///// </summary>
        ///// <returns></returns>
        //private String ParametrFromFile()
        //{
        //    return File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\Параметр.txt")[0];
        //}

        /// <summary>
        /// функция возвращает номер канала, где стоит бот
        /// </summary>
        /// <returns></returns>
        private int Channal()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Каналы.txt")); }

        /// <summary>
        /// функция возвращает номер телепорта, по которому летим продаваться
        /// </summary>
        /// <returns></returns>
        private int NomerTeleporta()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\ТелепортПродажа.txt")); }

        /// <summary>
        /// функция возвращает имя семьи для функции создания новых ботов
        /// </summary>
        /// <returns></returns>
        public string LoadNameOfFamily()
        { return File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Имя семьи.txt"); }

        /// <summary>
        /// функция возвращает тип патронов, которые нужно покупать в автомате патронов
        /// 0 - не нужно покупать
        /// 1 - стальные пули
        /// 2 - картечь
        /// 3 - Болты
        /// 4 - болшой калибр 
        /// 5 - элементальные сферы
        /// 6 - психические сферы
        /// </summary>
        /// <returns></returns>
        private int NumberOfBullets()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\Патроны.txt")); }

        /// <summary>
        /// считываем из файла координаты Х расстановки треугольником
        /// </summary>
        /// <returns></returns>
        private int[] LoadTriangleX()
        {
            int SIZE_OF_ARRAY = 3;
            String[] Koord_X = new String[SIZE_OF_ARRAY];
            int[] intKoord_X = new int[SIZE_OF_ARRAY];        //координаты для расстановки треугольником
            Koord_X = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\РасстановкаX.txt"); // Читаем файл с Координатами Х в папке с номером Number_Window
            for (int i = 0; i < SIZE_OF_ARRAY; i++) { intKoord_X[i] = int.Parse(Koord_X[i]); }
            return intKoord_X;
        }

        /// <summary>
        /// считываем из файла координаты Y расстановки треугольником
        /// </summary>
        /// <returns></returns>
        private int[] LoadTriangleY()
        {
            int SIZE_OF_ARRAY = 3;
            String[] Koord_Y = new String[SIZE_OF_ARRAY];
            int[] intKoord_Y = new int[SIZE_OF_ARRAY];        //координаты для расстановки треугольником
            Koord_Y = File.ReadAllLines(directoryOfMyProgram + this.numberOfWindow + "\\РасстановкаY.txt"); // Читаем файл с Координатами Y в папке с номером Number_Window
            for (int i = 0; i < SIZE_OF_ARRAY; i++) { intKoord_Y[i] = int.Parse(Koord_Y[i]); }
            return intKoord_Y;
        }

        /// <summary>
        /// метод считывает значение статуса из файла, 1 -  мы уже приступили у убиванию босса, 0 - нет 
        /// </summary>
        /// <returns> 1 -  мы уже приступили у убиванию босса, 0 - нет </returns>
        private int GetStatusOfAtk()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + this.numberOfWindow + "\\StatusOfAtk.txt")); }



        //public String Login { get; set; }
        //public String Password { get; set; }
        //public int x { get; set; }
        //public int y { get; set; }
        //public String param { get; set; }    
        //public UIntPtr hwnd { get; set; }
        //public int Kanal { get; set; }
        //public int nomerTeleport { get; set; }
        //public String nameOfFamily { get; set; }
        //public int[] triangleX { get; set; }
        //public int[] triangleY { get; set; }
        //public int Bullet { get; set; }                              //используемы тип патронов
        //public int statusOfAtk { get; set; }


        //public int NumberOfAccaunts { get; set; }

    }
}
