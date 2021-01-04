using System;
using System.IO;

namespace GlobalParametrs
{

    public  class GlobalParam
    {
        private  int nintendo;                           // как зачиповать оружие
        private  int startingAccount;                    // номер стартового аккаунта (для заточки, чиповки и проч)
        private  bool samara;                            // если этот комп расположен в Самаре, то true
        private  string[] mmProduct;                     // массив товаров для продажи через рынок (MM)
        private  int totalNumberOfAccounts;              // всего аккаунтов ботов
        private  int statusOfSale;                       // статус продажи (для BH)
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
        }

        // ==================================================== Методы ============================================================

        //  ====== геттеры и сеттеры =============
        public int Nintendo { get => nintendo; set => nintendo = value; }
        public int StartingAccount { get => startingAccount; set => startingAccount = value; }
        public bool Samara { get => samara; }
        public string[] MMProduct { get => mmProduct; }
        public int TotalNumberOfAccounts { get => totalNumberOfAccounts; }
        public int StatusOfSale { get => GetStatusOfSale(); set { statusOfSale = value; SetStatusInFile(); } }
        public string DirectoryOfMyProgram { get => directoryOfMyProgram; }

        //        public int StatusOfSale { get => statusOfSale; set { statusOfSale = value; SetStatusInFile(); } }

        // =================== прочие методы ===============================

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
        { return int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Чиповка.txt")); }

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
        /// возвращаеи количество аккаунтов
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
            int result = int.Parse(File.ReadAllText(directoryOfMyProgram + "\\Сервер.txt"));

            bool isSamara = false;
            if (result == 1) isSamara = true;

            return isSamara;
        }

    }

}