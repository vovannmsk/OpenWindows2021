using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace OpenGEWindows
{
    public abstract class MM : Server2
    {

        // ============ переменные ======================

        protected iPointColor pointIsMMSell1;
        protected iPointColor pointIsMMSell2;
        protected iPointColor pointIsMMBuy1;
        protected iPointColor pointIsMMBuy2;
        protected iPoint pointGotoBuySell;
        protected iPoint pointInitializeButton;
        protected iPoint pointSearchButton;
        protected iPoint pointSearchString;
        protected iPointColor pointIsFirstString1;
        protected iPointColor pointIsFirstString2;
        protected iPoint pointQuantity1;
        protected iPoint pointQuantity2;
        protected iPoint pointPrice;
        protected iPoint pointShadow;
        protected iPoint pointTime;
        protected iPoint pointTime48Hours;
        protected iPointColor pointIsHideFamily1;
        protected iPointColor pointIsHideFamily2;
        protected iPoint pointButtonRegistration;
        protected iPoint pointYesRegistration;
        protected iPoint pointOkRegistration;

        protected Product product;

        /// <summary>
        /// структура для хранения информации о товаре, продаваемом на рынке
        /// </summary>
        protected struct Product
        {
            public String Name;
            public int Quantity;
            public int MinPrice;
            public int Row;
            public int Column;
            public int numberOfDigit;       //количество цифр в цене товара
        }

        protected struct ProductColor
        {
            public uint color1;
            public uint color2;
        }

        // ============  методы  ========================

        /// <summary>
        /// выставляем на рынок продукт
        /// </summary>
        public void SellProduct()
        {
            String fileName = "C:\\!! Суперпрограмма V&K\\Продукт.txt";
            String[] ggg = LoadProduct(fileName);
            int numberOfparameters = 6;   //количество параметров товара 

            int i = 0;
            while (i * numberOfparameters < ggg.Length)
            {
                product.Name          =           ggg[0 + i * numberOfparameters];
                product.Quantity      = int.Parse(ggg[1 + i * numberOfparameters]);
                product.MinPrice      = int.Parse(ggg[2 + i * numberOfparameters]);
                product.Row           = int.Parse(ggg[3 + i * numberOfparameters]);
                product.Column        = int.Parse(ggg[4 + i * numberOfparameters]);
                product.numberOfDigit = int.Parse(ggg[5 + i * numberOfparameters]);

                if (!isMMBuy()) GotoPageBuy();   //если на странице Sell то переход на страницу Buy

                ProductSearch();
                if (!isMyFirstString())
                {
                    AddProduct();
                }

                i++;
                Pause(1000);
            }
            GotoPageSell();
            ToRemoveDuplicates();        

        }

        /// <summary>
        /// округление вверх числа a на количество разрядов b
        /// если a = 1655, b = 2, то результат равен 1600
        /// </summary>
        /// <param name="a"> округляемое число </param>
        /// <param name="b"> количество разрядов для округления </param>
        /// <returns> если a = 1655, b = 2, то результат равен 1600 </returns>
        private uint RoundColor(uint a, int b)
        {
            uint bb = 1;
            for (int j = 1; j <= b; j++) bb = bb * 10;
            uint result = a - a % bb;
            return result;
        }

        /// <summary>
        /// убираем дубликаты товаров с неактуальными ценами (только на видимом списке, без прокрутки)
        /// </summary>
        public void ToRemoveDuplicates()
        {
            List<ProductColor> listProduct = new List<ProductColor>(10);    //список товаров, которые выставлены на рынке
            ProductColor pr;
            iPoint pointCancelProduct;
            iPoint pointButtonCancel = new Point(70 - 5 + xx, 608 - 5 + yy);
            iPoint pointButtonYesCancel = new Point(465 - 5 + xx, 418 - 5 + yy);
            iPoint pointMM = new Point(405 - 5 + xx, 605 - 5 + yy);

            for (int j = 9; j >= 0; j--)
            {
                pointMM.PressMouseL(); //отводим мышку в сторону и активируем список

                pr.color1 = RoundColor(new PointColor(31 - 5 + xx, 333 - 5 + yy + j * 27, 2000000, 6).GetPixelColor(), 6);   // номер цвета округленный до 6 разряда
                pr.color2 = RoundColor (new PointColor(38 - 5 + xx, 333 - 5 + yy + j * 27, 2000000, 6).GetPixelColor(), 6);

                bool notProduct = ( (pr.color1 < 2000000) && (pr.color2 < 2000000) );   //нет товара на этом месте
                if (!notProduct)
                {
                    if (listProduct.IndexOf(pr) > 0)  //если такой продукт уже в массиве
                    {
                        pointCancelProduct = new Point(75 - 5 + xx, 333 - 5 + yy + j * 27);
                        pointCancelProduct.DoubleClickL();      //тыкаем в товар
                        Pause(500);
                        pointButtonCancel.DoubleClickL();       //тыкаем в Cancel, удаляя из списка
                        Pause(1500);
                        pointButtonYesCancel.PressMouseL();    // тыкаем в Yes
                        Pause(1500);
                        pointMM.PressMouseL();  //отводим мышку в сторону и активируем список
                    }
                    else
                    {
                        listProduct.Add(pr);
                    }
                }
            }



            //int currentProduct;

            //GotoEndList();  //идем в конец списка (в конце списка товары с актуальными ценами)

            //currentProduct = SelectLastItem();   //выбираем последний элемент списка товаров (номер строки с помледним товаром)

            //while (!isFirstStringOfList())            //пока не первая строка списка
            //{
            //    if (isDublicatProduct(currentProduct))   //если текущий товар является дубликатом
            //    {
            //        CancelProduct(currentProduct);     //отбиваем дубликат

            //    }

            //    NextProduct();      //переходим к следующему товару
            //}



        }

        /// <summary>
        /// читаем из текстового файла информацию о продаваемом продукте
        /// </summary>
        /// <returns></returns>
        protected String[] LoadProduct(String fileName)
        {
            String[] parametrs = File.ReadAllLines(fileName); // Читаем файл 
            return parametrs;
        }

        /// <summary>
        /// определяет, находимся ли мы на рынке на первой странице, где можно выставить товар на продажу  (MarketManager)
        /// </summary>
        /// <returns></returns>
        public bool isMMSell()
        {
            return ( pointIsMMSell1.isColor() && pointIsMMSell2.isColor() );
        }

        /// <summary>
        /// определяет, находимся ли мы на рынке на странице со списком товаров для покупки (MarketManager)
        /// </summary>
        /// <returns></returns>
        public bool isMMBuy()
        {
            return (pointIsMMBuy1.isColor() && pointIsMMBuy2.isColor());
        }

        /// <summary>
        /// определяет, наша ли строчка первая на рынке (наш ли товар самый дешевый). смотрим, по фамилии выставляющего
        /// </summary>
        /// <returns></returns>
        public bool isMyFirstString()
        {
            return (pointIsFirstString1.isColor() && pointIsFirstString2.isColor());
        }

        /// <summary>
        /// переходим на страницу рынка со списком товаров для покупки
        /// </summary>
        public void GotoPageBuy()
        {
            while (!isMMBuy())
            {
                pointGotoBuySell.PressMouseL();
                Pause(1000);
            }
        }

        /// <summary>
        /// переход на страницу, гже можно выставить товар на продажу
        /// </summary>
        private void GotoPageSell()
        {
            while (!isMMSell())
            {
                pointGotoBuySell.PressMouseL();
                Pause(1000);
            }
        }

        /// <summary>
        /// применяем фильтр (поиск отслеживаемого товара на рынке)
        /// </summary>
        public void ProductSearch()
        {
            if (isMMBuy())    //если мы на странице Buy
            {
                PressButtonInitialize();
                EnterSearchString(product.Name);
                PressButtonSearch();
            }
        }

        /// <summary>
        /// нажимаем кнопку Initialize, чтобы очистить параметры поиска
        /// </summary>
        private void PressButtonSearch()
        {
            pointSearchButton.DoubleClickL();
            Pause(3000);
        }

        /// <summary>
        /// нажимаем кнопку Search, чтобы запустить поиск продукта
        /// </summary>
        private void PressButtonInitialize()
        {
            pointInitializeButton.DoubleClickL();
            Pause(1500);
        }

        /// <summary>
        /// ввести в поисковую строку поисковый запрос
        /// </summary>
        /// <param name="searchString"></param>
        private void EnterSearchString(String searchString)
        {
            pointSearchString.DoubleClickL();
            SendKeys.SendWait(searchString);
            Pause(1000);
        }

        /// <summary>
        /// возвращает цифру от 0 до 9, которая соответствует i-той цифре с конца в цене товара
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Numeral(int i)
        {
            int x;

            switch (product.numberOfDigit)
            {
                case 3:
                    x = 477;
                    break;
                case 4:
                    x = 483;  //зел
                    break;
                case 5:
                    x = 487;  //жел
                    break;
                case 6:
                    x = 491;  //коричн
                    break;
                case 7:
                    x = 497;
                    break;
                case 8:
                    x = 501;
                    break;
                case 9:
                    x = 505;
                    break;
                case 10:
                    x = 511;
                    break;
                default:
                    x = 491;
                    break;
            }
            int[] koordX = { x - 0 , x - 8 , x - 16, 
                             x - 28, x - 36, x - 44,
                             x - 56, x - 64, x - 72,
                             x - 84, x - 92, x - 100 };

            int koordY = 292;
            return Numeral(koordX[i],koordY);
        }

        /// <summary>
        /// возвращает цифру от 0 до 9, расположенную в указанных координатах (левый верхний угол)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Numeral(int x, int y)
        {
            iPointColor pointdigit1 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 2, 65000, 3);  //1
            iPointColor pointdigit2 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 5, 63000, 3);  //2
            iPointColor pointdigit3 = new PointColor(x - 5 + xx + 2, y - 5 + yy + 4, 65000, 3);  //3
            iPointColor pointdigit4 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 6, 59000, 3);  //4
            iPointColor pointdigit5 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 0, 56000, 3);  //5
            iPointColor pointdigit6 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 1, 60000, 3);  //6
            iPointColor pointdigit7 = new PointColor(x - 5 + xx + 0, y - 5 + yy + 0, 63000, 3);  //7
            iPointColor pointdigit8 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 4, 63000, 3);  //8
            iPointColor pointdigit9 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 4, 65000, 3);  //9
            iPointColor pointdigit0 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 8, 56000, 3);  //0

            switch (product.numberOfDigit)
            {
                case 3:
                    break;
                case 4:         //зеленые цифры
                    pointdigit1 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 2, 64000, 3);  //1
                    pointdigit2 = new PointColor(x - 5 + xx + 0, y - 5 + yy + 9, 65000, 3);  //2
                    pointdigit3 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 4, 64000, 3);  //3
                    pointdigit4 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 6, 58000, 3);  //4
                    pointdigit5 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 0, 50000, 4);  //5
                    pointdigit6 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 1, 59000, 3);  //6
                    pointdigit7 = new PointColor(x - 5 + xx + 0, y - 5 + yy + 0, 60000, 4);  //7
                    pointdigit8 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 4, 63000, 3);  //8
                    pointdigit9 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 4, 65000, 3);  //9
                    pointdigit0 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 8, 56000, 3);  //0
                    break;
                case 5:        //желтые цифры
                    pointdigit1 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 2, 65000, 3);  //1
                    pointdigit2 = new PointColor(x - 5 + xx + 0, y - 5 + yy + 9, 65000, 3);  //2
                    pointdigit3 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 6, 65000, 3);  //3
                    pointdigit4 = new PointColor(x - 5 + xx + 3, y - 5 + yy + 6, 50000, 4);  //4              если 3 и 6, то 59000
                    pointdigit5 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 0, 50000, 4);  //5
                    pointdigit6 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 1, 60000, 3);  //6
                    pointdigit7 = new PointColor(x - 5 + xx + 0, y - 5 + yy + 0, 63000, 3);  //7
                    pointdigit8 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 4, 63000, 3);  //8
                    pointdigit9 = new PointColor(x - 5 + xx + 5, y - 5 + yy + 4, 65000, 3);  //9
                    pointdigit0 = new PointColor(x - 5 + xx + 1, y - 5 + yy + 8, 56000, 3);  //0
                    break;
                case 6:        //коричневые цифры
                    pointdigit1  = new PointColor(x - 5 + xx + 3, y - 5 + yy + 2, 4090000, 4);  //1
                    pointdigit2  = new PointColor(x - 5 + xx + 3, y - 5 + yy + 5, 4030000, 4);  //2
                    pointdigit3  = new PointColor(x - 5 + xx + 3, y - 5 + yy + 4, 4030000, 4);  //3
                    pointdigit4  = new PointColor(x - 5 + xx + 3, y - 5 + yy + 6, 3760000, 4);  //4
                    pointdigit5  = new PointColor(x - 5 + xx + 5, y - 5 + yy + 0, 3560000, 4);  //5
                    pointdigit6  = new PointColor(x - 5 + xx + 5, y - 5 + yy + 1, 3760000, 4);  //6
                    pointdigit7  = new PointColor(x - 5 + xx + 0, y - 5 + yy + 0, 4030000, 4);  //7
                    pointdigit8  = new PointColor(x - 5 + xx + 1, y - 5 + yy + 4, 4030000, 4);  //8
                    pointdigit9  = new PointColor(x - 5 + xx + 5, y - 5 + yy + 4, 4090000, 4);  //9
                    pointdigit0  = new PointColor(x - 5 + xx + 1, y - 5 + yy + 8, 3560000, 4);  //0
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
            }

            if ((pointdigit1.isColor()) || ( (pointdigit1.GetPixelColor() > 120000) && (pointdigit1.GetPixelColor() < 130000) ) ) return 1;
            if ((pointdigit2.isColor()) || ( (pointdigit2.GetPixelColor() > 120000) && (pointdigit2.GetPixelColor() < 130000) ) ) return 2;
            if ((pointdigit4.isColor()) || ( (pointdigit4.GetPixelColor() > 120000) && (pointdigit4.GetPixelColor() < 130000) ) ) return 4;
            if ((pointdigit7.isColor()) || ( (pointdigit7.GetPixelColor() > 120000) && (pointdigit7.GetPixelColor() < 130000) ) ) return 7;
            if ((pointdigit5.isColor()) || ( (pointdigit5.GetPixelColor() > 120000) && (pointdigit5.GetPixelColor() < 130000) ) ) return 5;
            if ((pointdigit6.isColor()) || ( (pointdigit6.GetPixelColor() > 120000) && (pointdigit6.GetPixelColor() < 130000) ) ) return 6;
            if ((pointdigit0.isColor()) || ( (pointdigit0.GetPixelColor() > 120000) && (pointdigit0.GetPixelColor() < 130000) ) ) return 0;
            if ((pointdigit9.isColor()) || ( (pointdigit9.GetPixelColor() > 120000) && (pointdigit9.GetPixelColor() < 130000) ) ) return 9;
            if ((pointdigit8.isColor()) || ( (pointdigit8.GetPixelColor() > 120000) && (pointdigit8.GetPixelColor() < 130000) ) ) return 8;
            if ((pointdigit3.isColor()) || ( (pointdigit3.GetPixelColor() > 120000) && (pointdigit3.GetPixelColor() < 130000) ) ) return 3;

            return 0;  //возвращаем незанчащий ноль, если никакой цифры на этом месте нет
        }

        /// <summary>
        /// возвращает цену товара в первой строке
        /// </summary>
        /// <returns></returns>
        private int FirstStringPrice()
        {
            int total = 0;                    //итоговое число - цена товара, получаем как 
            int digit = 1;                       //разряд получаемой цифры
            for (int i = 0; i < 7; i++ )
            {
                total = total + Numeral(i) * digit;    
                digit *= 10;
            }
            return total;
        }

        /// <summary>
        /// вводим указанное число в поле количество
        /// </summary>
        /// <param name="quantity"></param>
        private void EnterQuantity(int quantity)
        {
            pointQuantity1.DoubleClickL();
            Pause(500);
            pointQuantity1.Drag(pointQuantity2);
            SendKeys.SendWait(quantity.ToString());
            Pause(500);
        }

        /// <summary>
        /// вводим указанное число в поле цена
        /// </summary>
        /// <param name="quantity"></param>
        private void EnterPrice(int price)
        {
            pointPrice.DoubleClickL();
            Pause(500);
            SendKeys.SendWait(price.ToString());
            Pause(500);
        }

        /// <summary>
        /// положить товар на продажу
        /// </summary>
        private void MoveProductToSell()
        {
            int row = product.Row;
            int column = product.Column;
            iPoint pointInventorty = new Point(840 - 5 + xx, 150 - 5 + yy);     //третья закладка инвентаря
            iPoint pointMM = new Point(405 - 5 + xx, 605 - 5 + yy);
            iPoint pointProductBegin = new Point(700 - 5 + xx + (column - 1) * 39, 180 - 5 + yy + (row - 1) * 38);
            iPoint pointproductEnd = new Point(70 - 5 + xx, 225 - 5 + yy);

            pointInventorty.DoubleClickL();   //делаем инвентарь активным
            Pause(1500);
            pointProductBegin.Drag(pointproductEnd);
            Pause(1000);
            pointMM.PressMouseLL();
        }

        /// <summary>
        /// проверяем, есть ли галка в поле  Hide family name
        /// </summary>
        /// <returns></returns>
        private bool isHideFamily()
        {
            return (pointIsHideFamily1.isColor() && pointIsHideFamily2.isColor());
        }

        /// <summary>
        /// убираем галку "Hide famile name"
        /// </summary>
        private void RemoveShadow()
        {
            if (isHideFamily())
            {
                pointShadow.PressMouse();
                Pause(1000);
            }
        }

        /// <summary>
        /// вводим время, на которое ставим товар на продажу (48 часов)
        /// </summary>
        private void EnterTime()
        {
            pointTime.PressMouseL();
            Pause(1000);
            pointTime48Hours.PressMouseLL();
            Pause(1000);
        }

        /// <summary>
        /// нажимаем кнопку регистрация продукта для продажи
        /// </summary>
        private void PressButtonRegistration()
        {
            pointButtonRegistration.DoubleClickL();
            Pause(2000);
            pointYesRegistration.DoubleClickL();
            Pause(2000);
            pointOkRegistration.DoubleClickL();
            Pause(1000);
        }

        /// <summary>
        /// выставить продукт на продажу
        /// </summary>
        public void AddProduct ()
        {
            int myPrice = FirstStringPrice() - 1;

            if (myPrice > product.MinPrice)
            {
                GotoPageSell();
                MoveProductToSell();
                EnterQuantity(product.Quantity);
                EnterPrice(myPrice);
                EnterTime();
                RemoveShadow();
                PressButtonRegistration();
            }

            

        }
    }
}
