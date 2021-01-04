using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GEDataBot.BL
{
    public class ScriptDataBotText : IScriptDataBot
    {
        private const String KATALOG_MY_PROGRAM = "C:\\!! Суперпрограмма V&K\\";
        private int numberOfWindow;
        private DataBot databot;

        public ScriptDataBotText(int numberOfWindow)
        {
            this.numberOfWindow = numberOfWindow;
            this.databot = new DataBot();
            //databot.NumberOfAccaunts = KolvoAkk();
            databot.x = Koord_X();
            databot.y = Koord_Y();
            databot.Login = Login();
            databot.Password = Pass();
            databot.hwnd = Hwnd_in_file();
            databot.param = Parametr();
            databot.Kanal = Channal();
            databot.nomerTeleport = NomerTeleporta();
            databot.nameOfFamily = NameOfFamily();
            databot.triangleX = LoadTriangleX();
            databot.triangleY = LoadTriangleY();
        }

        public ScriptDataBotText()
        {
            throw new NotImplementedException("Номер окна должен быть указан обязательно!!!");
        }


        /// <summary>
        /// возвращает данные для бота, заданные пользователем
        /// </summary>
        /// <returns></returns>
        public DataBot GetDataBot()
        {
            return databot;
        }

        /// <summary>
        /// изменяем Hwnd окна и записываем в файл
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(UIntPtr hwnd)
        {
            databot.hwnd = hwnd;
            File.WriteAllText(KATALOG_MY_PROGRAM + numberOfWindow + "\\HWND.txt", hwnd.ToString());
        }

        /// <summary>
        /// функция возвращает номер телепорта, по которому летим продаваться
        /// </summary>
        /// <returns></returns>
        private int NomerTeleporta()
        { return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + this.numberOfWindow + "\\ТелепортПродажа.txt")); }

        /// <summary>
        /// функция возвращает логин бота
        /// </summary>
        /// <returns></returns>
        private String Login()   // каталог и номер окна
        { return File.ReadAllText(KATALOG_MY_PROGRAM + this.numberOfWindow + "\\Логины.txt"); }

        /// <summary>
        /// функция возвращает пароль от бота
        /// </summary>
        /// <returns></returns>
        private String Pass()   // каталог и номер окна
        { return File.ReadAllText(KATALOG_MY_PROGRAM + this.numberOfWindow + "\\Пароли.txt"); }

        /// <summary>
        /// функция возвращает hwnd бота
        /// </summary>
        /// <returns></returns>
        private UIntPtr Hwnd_in_file()
        {
            UIntPtr ff;
            String ss = File.ReadAllText(KATALOG_MY_PROGRAM + this.numberOfWindow + "\\HWND.txt");
            if (ss.Equals(""))
            { ff = (UIntPtr)2222222; }   //если пусто в файле, то hwnd = 2222222;
            else
            {
                uint dd = uint.Parse(ss);
                ff = (UIntPtr)dd;
            }
            return ff;
        }

        /// <summary>
        /// функция возвращает смещение по оси X окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_X()
        {
            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 875, 850, 825, 800, 775, 750, 725, 700 };
            return koordX[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает смещение по оси Y окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_Y()   // каталог и номер окна
        {
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 5, 30, 55, 80, 105, 130, 155, 180 };
            return koordY[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает имя сервера (Америка, европа или Синг)
        /// </summary>
        /// <returns></returns>
        private String Parametr()
        { return File.ReadAllText(KATALOG_MY_PROGRAM + this.numberOfWindow + "\\Параметр.txt"); }

        /// <summary>
        /// функция возвращает номер канала, где стоит бот
        /// </summary>
        /// <returns></returns>
        private int Channal()
        { return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + numberOfWindow + "\\Каналы.txt")); }

        /// <summary>
        /// считываем из файла координаты Х расстановки треугольником
        /// </summary>
        /// <returns></returns>
        private int[] LoadTriangleX()
        {
            int SIZE_OF_ARRAY = 3;
            String[] Koord_X = new String[SIZE_OF_ARRAY];
            int[] intKoord_X = new int[SIZE_OF_ARRAY];        //координаты для расстановки треугольником
            Koord_X = File.ReadAllLines(KATALOG_MY_PROGRAM + numberOfWindow + "\\РасстановкаX.txt"); // Читаем файл с Координатами Х в папке с номером Number_Window
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
            Koord_Y = File.ReadAllLines(KATALOG_MY_PROGRAM + numberOfWindow + "\\РасстановкаY.txt"); // Читаем файл с Координатами Y в папке с номером Number_Window
            for (int i = 0; i < SIZE_OF_ARRAY; i++) { intKoord_Y[i] = int.Parse(Koord_Y[i]); }
            return intKoord_Y;
        }

        /// <summary>
        /// функция возвращает имя семьи для функции создания новых ботов
        /// </summary>
        /// <returns></returns>
        public string NameOfFamily()                                                                                                    
        { return File.ReadAllText(KATALOG_MY_PROGRAM + numberOfWindow + "\\Имя семьи.txt"); }                                          

        ///// <summary>
        ///// возвращает количесто аккаунтов ботов 
        ///// </summary>
        ///// <returns></returns>
        //public int KolvoAkk()
        //{ return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + "\\Аккаунтов всего.txt")); }

        ///// <summary>
        ///// возвращает количесто аккаунтов ботов 
        ///// </summary>
        ///// <returns></returns>
        //public static int KolvoAkk()
        //{ return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + "\\Аккаунтов всего.txt")); }

    }
}
