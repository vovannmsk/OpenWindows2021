using System;
using System.IO;


namespace GEBot.Data
{
    /// <summary>
    /// =========================================================== класс не используется =====================================
    /// </summary>
    public class ScriptDataBotText : IScriptDataBot
    {
        private int numberOfWindow;
        private BotParam databot;
        private GlobalParam globalParam;

        public ScriptDataBotText(int numberOfWindow)
        {
            this.globalParam = new GlobalParam();
            this.numberOfWindow = numberOfWindow;
            this.databot = new BotParam(this.numberOfWindow);
            //this.databot.X = Koord_X();
            //this.databot.Y = Koord_Y();
            //this.databot.Login = Login();
            //this.databot.Password = Pass();
            //this.databot.Hwnd = Hwnd_in_file();
            //this.databot.Param = Parametr();
            //this.databot.Kanal = Channal();
            //this.databot.NomerTeleport = NomerTeleporta();
            //this.databot.NameOfFamily = NameOfFamily();
            //this.databot.TriangleX = LoadTriangleX();
            //this.databot.TriangleY = LoadTriangleY();
            //this.databot.StatusOfAtk = GetStatusOfAtk();
            //databot.Bullet = NumberOfBullets();                                               //пока не используется, но всё готово для использования
        }

        public ScriptDataBotText()
        {
            throw new NotImplementedException("Номер окна должен быть указан обязательно!!!");
        }

        /// <summary>
        /// метод считывает значение статуса из файла, 1 - мы уже били босса, 0 - нет 
        /// </summary>
        /// <returns></returns>
        private int GetStatusOfAtk()
        { return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\StatusOfAtk.txt")); }

        /// <summary>
        /// возвращает данные для бота, заданные пользователем
        /// </summary>
        /// <returns></returns>
        public BotParam GetDataBot()
        {  return databot;   }

        /// <summary>
        /// изменяем Hwnd окна и записываем в файл
        /// </summary>
        /// <param name="hwnd"></param>
        public void SetHwnd(UIntPtr hwnd)
        {
            databot.Hwnd = hwnd;
            File.WriteAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\HWND.txt", hwnd.ToString());
        }

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
        { return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Патроны.txt")); }

        /// <summary>
        /// функция возвращает номер телепорта, по которому летим продаваться
        /// </summary>
        /// <returns></returns>
        private int NomerTeleporta()
        { return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\ТелепортПродажа.txt")); }

        /// <summary>
        /// функция возвращает логин бота
        /// </summary>
        /// <returns></returns>
        private String Login()   // каталог и номер окна
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Логины.txt"); }

        /// <summary>
        /// функция возвращает пароль от бота
        /// </summary>
        /// <returns></returns>
        private String Pass()   // каталог и номер окна
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Пароли.txt"); }

        /// <summary>
        /// функция возвращает hwnd бота
        /// </summary>
        /// <returns></returns>
        private UIntPtr Hwnd_in_file()
        {
            UIntPtr ff;
            String ss = File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\HWND.txt");
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
            int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };
            //int[] koordX = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 5, 255, 280, 305, 875, 850, 825, 800, 775, 750, 875 };
            
            return koordX[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает смещение по оси Y окна бота на мониторе
        /// </summary>
        /// <returns></returns>
        private int Koord_Y()   // каталог и номер окна
        {
            int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 230, 255, 280, 305, 5, 30, 55, 80, 105, 130, 5 };
            //int[] koordY = { 5, 30, 55, 80, 105, 130, 155, 180, 205, 5, 255, 280, 305, 5, 30, 55, 80, 105, 130, 55 };
            return koordY[this.numberOfWindow - 1];
        }

        /// <summary>
        /// функция возвращает имя сервера (Америка, европа или Синг)
        /// </summary>
        /// <returns></returns>
        private String Parametr()
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Параметр.txt"); }

        /// <summary>
        /// функция возвращает номер канала, где стоит бот
        /// </summary>
        /// <returns></returns>
        private int Channal()
        { return int.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Каналы.txt")); }

        /// <summary>
        /// считываем из файла координаты Х расстановки треугольником
        /// </summary>
        /// <returns></returns>
        private int[] LoadTriangleX()
        {
            int SIZE_OF_ARRAY = 3;
            String[] Koord_X = new String[SIZE_OF_ARRAY];
            int[] intKoord_X = new int[SIZE_OF_ARRAY];        //координаты для расстановки треугольником
            Koord_X = File.ReadAllLines(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\РасстановкаX.txt"); // Читаем файл с Координатами Х в папке с номером Number_Window
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
            Koord_Y = File.ReadAllLines(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\РасстановкаY.txt"); // Читаем файл с Координатами Y в папке с номером Number_Window
            for (int i = 0; i < SIZE_OF_ARRAY; i++) { intKoord_Y[i] = int.Parse(Koord_Y[i]); }
            return intKoord_Y;
        }

        /// <summary>
        /// функция возвращает имя семьи для функции создания новых ботов
        /// </summary>
        /// <returns></returns>
        public string NameOfFamily()                                                                                                    
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + this.numberOfWindow + "\\Имя семьи.txt"); }                                          


    }
}
