using System;
using System.IO;

namespace GEBot.Data
{
    /// <summary>
    /// Глобальные параметры, настройки. Действуют для ВСЕХ ботов
    /// </summary>
    public class GlobalParam
    {
        /// <summary>
        /// номер аккаунта в списке аккаунтов п/п (нумерация с нуля). 
        /// </summary>
        private int infinity;

        /// <summary>
        /// если равна true, то цикл похода в ворота Инфинити продолжается
        /// </summary>
        //private bool infinityGate;

        private int nintendo;                           // как зачиповать оружие
        private int startingAccount;                    // номер стартового аккаунта (для заточки, чиповки и проч)
        private bool samara;                            // если этот комп расположен в Самаре, то true
        private string[] mmProduct;                     // массив товаров для продажи через рынок (MM)
        private int totalNumberOfAccounts;              // всего аккаунтов ботов
        private int statusOfSale;                       // статус продажи (для BH)
        private bool windows10;                         // какая винда на компе. true, если Windows 10
        private string directoryOfMyProgram;
        //  private const string KATALOG_MY_PROGRAM = "C:\\!! Суперпрограмма V&K\\";

        /// <summary>
        /// конструктор
        /// </summary>
        public GlobalParam()
        {
            this.directoryOfMyProgram = "C:\\!! Суперпрограмма V&K\\";
            this.nintendo = TypeOfNintendo();
            this.startingAccount = BeginAcc();
            this.samara = IsSamara();
            this.mmProduct = LoadProduct();
            this.totalNumberOfAccounts = KolvoAkk();
            this.statusOfSale = GetStatusOfSale();
            this.windows10 = IsWindow10();
            infinity = InfinityInFile();
        }


        //  ============ Свойства ====================================================

        /// <summary>
        /// номер аккаунта в списке аккаунтов п/п (нумерация с нуля) 
        /// </summary>
        public int Infinity { get { infinity = InfinityInFile(); return infinity; }
                              set { infinity = value; SetInfinity(); }     }

        public int Nintendo { get => nintendo; set => nintendo = value; }
        public int StartingAccount { get => startingAccount; set => startingAccount = value; }
        public bool Samara { get => samara; }
        public string[] MMProduct { get => mmProduct; }
        public int TotalNumberOfAccounts { get => totalNumberOfAccounts; }
        //        public int StatusOfSale { get => GetStatusOfSale(); set { statusOfSale = value; SetStatusInFile(); } }
        //        public int StatusOfSale { get => statusOfSale; set { statusOfSale = value; SetStatusInFile(); } }
        public int StatusOfSale { get { statusOfSale = GetStatusOfSale(); return statusOfSale; }
                                  set { statusOfSale = value; SetStatusInFile(); }
                                }
        public string DirectoryOfMyProgram { get => directoryOfMyProgram; }
        public bool Windows10 { get => windows10; }

        //public bool Infinity { get => infinity;}

        // ==================================================== Методы ============================================================

        /// <summary>
        /// возвращаем тип чиповки
        /// 1 - без рассы
        /// 2 - wild
        /// 3 - LifeLess
        /// 4 - wild or Human
        /// 5 - Undeed
        /// 6 - Demon
        /// 7 - Human
        /// </summary>
        /// <returns></returns>
        private int TypeOfNintendo()
        {
            return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Чиповка.txt"));
            //return 1;
        }

        /// <summary>
        /// увеличение параметра Infinity на 1 с записью результата в текстовый файл
        /// </summary>
        public void InfinityPlusOne()
        {
            Infinity = infinity + 1;
            //Infinity++;        //вариант 2
        }


        /// <summary>
        /// метод считывает значение статуса из файла, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        /// </summary>
        /// <returns></returns>
        private int GetStatusOfSale()
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\StatusOfSale.txt")); }

        /// <summary>
        /// метод записывает значение статуса в файл, 1 - мы направляемся на продажу товара в магазин, 0 - нет (обычный режим работы)
        /// </summary>
        /// <returns></returns>
        private void SetStatusInFile()
        {
            File.WriteAllText(directoryOfMyProgram + "\\StatusOfSale.txt", this.statusOfSale.ToString());
        }

        /// <summary>
        /// возвращаем количество аккаунтов
        /// </summary>
        /// <returns>кол-во акков всего</returns>
        private int KolvoAkk()
        {
            return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Аккаунтов всего.txt"));
        }

        /// <summary>
        /// читаем из файла значение
        /// </summary>
        /// <returns>с какого аккаунта начать работу методам</returns>
        private int BeginAcc()
        {
            return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\СтартовыйАккаунт.txt"));
            //return 1;
        }

        /// <summary>
        /// читаем из текстового файла информацию о продаваемом продукте
        /// </summary>
        /// <returns></returns>
        private String[] LoadProduct()
        {
            String[] parametrs = File.ReadAllLines(directoryOfMyProgram + "Продукт.txt");
            return parametrs;
        }

        /// <summary>
        /// метод возвращает параметр, который указывает, является ли данный компьютер удаленным сервером или локальным компом (различная обработка мыши)
        /// </summary>
        /// <returns> true, если комп является удаленным сервером (из Самары) </returns>
        private bool IsSamara()
        {
            string ddd = File.ReadAllText(directoryOfMyProgram + "\\Сервер.txt");
            int result = int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Сервер.txt"));

            bool isSamara = false;
            if (result == 1) isSamara = true;

            return isSamara;
        }

        /// <summary>
        /// определяем, какая винда на компе. 
        /// </summary>
        /// <returns>true, если Windows 10</returns>
        private bool IsWindow10()
        {
            return bool.Parse(File.ReadAllText(directoryOfMyProgram + "\\Windows10.txt"));
        }

        /// <summary>
        /// читать значение свойства Инфинити из файла
        /// </summary>
        /// <returns></returns>
        private int InfinityInFile()
        {
            return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Инфинити.txt"));
        }


        /// <summary>
        /// записываем значение infinity в файл
        /// </summary>
        private void SetInfinity()
        { File.WriteAllText(directoryOfMyProgram + "\\Инфинити.txt", this.infinity.ToString()); }


    }

}
