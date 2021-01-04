using System;
using System.IO;

namespace GlobalSettings2
{
    public class GlobalSet
    {
        private int nintendo;
        private int startingAccount;
        private string mmFamily;
        private bool samara;
        private string mmProduct;
        private int totalNumberOfAccounts;
        private bool statusOfSale;
        private const String KATALOG_MY_PROGRAM = "C:\\!! Суперпрограмма V&K\\";

        /// <summary>
        /// конструктор
        /// </summary>
        public GlobalSet()
        {
            this.nintendo = TypeOfNintendo();


        }

        public int Nintendo { get => nintendo; set => nintendo = value; }
        public int StartingAccount { get => startingAccount; set => startingAccount = value; }
        public string MMFamily { get => mmFamily; set => mmFamily = value; }
        public bool Samara { get => samara; set => samara = value; }
        public string MMProduct { get => mmProduct; set => mmProduct = value; }
        public int TotalNumberOfAccounts { get => totalNumberOfAccounts; set => totalNumberOfAccounts = value; }
        public bool StatusOfSale { get => statusOfSale; set => statusOfSale = value; }


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
        public int TypeOfNintendo()
        { return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + "\\Чиповка.txt")); }




    }
}
