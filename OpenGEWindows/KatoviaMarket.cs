﻿using System.Windows.Forms;
using GEBot.Data;

namespace OpenGEWindows
{
    public abstract class KatoviaMarket : Server2
    {

        // ============ переменные ======================

        #region Shop

        protected iPointColor pointIsSale1;
        protected iPointColor pointIsSale2;
        protected iPointColor pointIsSale3;
        protected iPointColor pointIsSale4;
        protected iPointColor pointIsSaleIn1;
        protected iPointColor pointIsSaleIn2;
        protected iPointColor pointIsClickSell1;
        protected iPointColor pointIsClickSell2;
        protected iPoint pointBookmarkSell;
        //protected iPoint pointSaleToTheRedBottle;
        //protected iPoint pointSaleOverTheRedBottle;
        //protected iPoint pointWheelDown;
        protected iPoint pointButtonBUY;
        protected iPoint pointButtonSell;
        protected iPoint pointButtonClose;
        protected iPoint pointBuyingMitridat1;
        protected iPoint pointBuyingMitridat2;
        protected iPoint pointBuyingMitridat3;
        protected iPoint pointAddProduct;

        /// <summary>
        /// структура для сравнения товаров в магазине
        /// </summary>
        protected struct Product
        {
            public uint color1;
            public uint color2;
            public uint color3;
            public bool colorMega;

            /// <summary>
            /// создаем структуру объекта, состоящую из трех точек
            /// </summary>
            /// <param name="xx">сдвиг окна по оси X</param>
            /// <param name="yy">сдвиг окна по оси Y</param>
            /// <param name="numderOfString">номер строки в магазине, где берется товар</param>
            public Product(int xx, int yy, int numberOfString)
            {
                //color1 = new PointColor(149 - 5 + xx, 219 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                //color2 = new PointColor(146 - 5 + xx, 219 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                //color3 = new PointColor(165 - 5 + xx, 214 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                //colorMega = new PointColor(174 - 5 + xx, 214 - 5 + yy + (numberOfString - 1) * 27, 10000000, 7).isColor();          //буква M в слове Mega (для отлова МегаРесурсов)
                color1 = new PointColor(149 + 3 - 5 + xx, 219 + 31 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color2 = new PointColor(146 + 3 - 5 + xx, 219 + 31 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color3 = new PointColor(165 + 3 - 5 + xx, 214 + 31 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                colorMega = new PointColor(174 + 3 - 5 + xx, 214 + 31 - 5 + yy + (numberOfString - 1) * 27, 10000000, 7).isColor();          //буква M в слове Mega (для отлова МегаРесурсов)
            }

            /// <summary>
            /// сравнение двух товаров по трем точкам
            /// </summary>
            /// <param name="product">товар для сравнения</param>
            /// <returns>true, если два товара одинаковы</returns>
            public bool EqualProduct(Product product)
            {
                return ((product.color1 == color1) &&
                        (product.color2 == color2) &&
                        (product.color3 == color3));
            }
        };

        #endregion


        protected Dialog dialog;
        protected GlobalParam globalParam;
        protected BotParam botParam;

        // ============  методы  ========================

        #region Shop

        /// <summary>
        /// проверяет, находится ли данное окно на входе в магазин (но в кармане филосовские камни)
        /// </summary>
        /// <returns> true, если находится в магазине </returns>
        public bool isSalePhiloStone()
        {
            return ((pointIsSale3.isColor()) && (pointIsSale4.isColor()));
        }


        /// <summary>
        /// действия по входу в магазин до закладки "Purchase"
        /// </summary>
        public void ClickSellAndOkInTrader()
        {
            if (isSalePhiloStone())
            {
                dialog.PressStringDialog(1);
                dialog.PressOkButton(1);
            }
            else
            {
                dialog.PressOkButton(2);
            }
        }

        /// <summary>
        /// проверяет, находится ли данное окно в магазине (а точнее на странице входа в магазин)  
        /// </summary>
        /// <returns> true, если находится в магазине </returns>
        public bool isSale()
        {
            return ((pointIsSale1.isColor()) && (pointIsSale2.isColor()));
        }                                                                                       

        /// <summary>
        /// проверяет, находится ли данное окно внутри магазина (на закладке BUY или SELL)                                       
        /// </summary>
        /// <returns> true, если находится в магазине </returns>
        public bool isSaleIn()
        {
            return ((pointIsSaleIn1.isColor()) && (pointIsSaleIn2.isColor()));
        }


        /// <summary>
        /// проверяет, открыта ли закладка Sell в магазине 
        /// </summary>
        /// <returns> true, если закладка Sell в магазине открыта </returns>
        public bool isClickSell()
        {
            return ((pointIsClickSell1.isColor()) && (pointIsClickSell2.isColor()));
        }

        /// <summary>
        /// Кликаем в закладку Sell  в магазине 
        /// </summary>
        public void Bookmark_Sell()
        {
            //            pointBookmarkSell.DoubleClickL();
            pointBookmarkSell.PressMouseLL();
            Pause(1500);
        }

        /// <summary>
        /// проверяем, является ли товар в первой строке магазина маленькой красной бутылкой
        /// </summary>
        /// <returns> true, если в первой строке маленькая красная бутылка </returns>
        public bool isRedBottle()
        {
            PointColor pointFirstString = new PointColor(152 - 5 + xx, 250 - 5 + yy, 5933520, 0);
            return pointFirstString.isColor();
        }                                                                //исправлено 23.01.2019

        /// <summary>
        /// добавляем товар из указанной строки в корзину 
        /// </summary>
        /// <param name="numberOfString">номер строки</param>
        public void AddToCart()
        {
            AddProduct();

            Pause(200);

            DownList();  
        }

        /// <summary>
        /// добавить продукт в первой строчке списка в магазине
        /// </summary>
        private void AddProduct()
        {
            pointAddProduct.PressMouseL();
        }

        /// <summary>
        /// в магазине сдвигаем список продаж вниз
        /// </summary>
        private void DownList()
        {
            if (globalParam.Samara)
//            if (botwindow.getIsServer())
            {
                // вариант 1. нажатие на стрелку вниз в магазине   (для самарских серверов)
//                iPoint pointArrowDown = new Point(507 - 5 + botwindow.getX(), 549 - 5 + botwindow.getY());            старый вариант
                iPoint pointArrowDown = new Point(505 - 5 + botwindow.getX(), 555 - 5 + botwindow.getY());
                pointArrowDown.PressMouseL();
            }
            else
            {
                // вариант 2. колесик вниз
                //pointAddProduct.PressMouseR();
                pointAddProduct.PressMouseWheelDown();
            }
        }

        /// <summary>
        /// определяет, анализируется ли нужный товар либо данный товар можно продавать (проверяем только оружие и броню /до красной бутылки/)
        /// </summary>
        /// <param name="color"> цвет полностью определяет товар, который поступает на анализ </param>
        /// <returns> true, если анализируемый товар нужный и его нельзя продавать </returns>
        public bool NeedToSellProduct2(uint color, uint color3)
        {
            bool result = true;   //по умолчанию вещь надо продавать, поэтому true

            switch (color)                                             // Хорошая вещь или нет, сверяем по картотеке
            {
                case 14607342:     // desapio Necklase
                case 15836741:     // desapio Gloves
                case 1381654:     // desapio Boots     
                case 2959141:     // desapio Boots 
                case 3747867:     // desapio Belt
                case 3095646:     // desapio Earrings
                //case 482184:     // desapio Token
                    if ((color3 == 15109412) || (color3 == 3593951)) result = false;     //смотрим цвет слова Desapio (синий или желтый цвет)            
                    break;
            }

            return result;
        }                                    //товары здесь !!!!!!!!!!!!!!!!



        //        /// <summary>
        //        /// определяет, анализируется ли нужный товар либо данный товар можно продавать
        //        /// </summary>
        //        /// <param name="color"> цвет полностью определяет товар, который поступает на анализ </param>
        //        /// <returns> true, если анализируемый товар нужный и его нельзя продавать </returns>
        //        public bool NeedToSellProduct(uint color, int numberOfString)
        //        {
        //            bool result = true;   //по умолчанию вещь надо продавать, поэтому true
        ////            iPointColor pointMega = new PointColor(174 - 5 + xx, 214 - 5 + yy + (numberOfString - 1) * 27, 10000000, 7);  //буква M в слове Mega
        //            iPointColor pointMega = new PointColor(177 - 5 + xx, 245 - 5 + yy + (numberOfString - 1) * 27, 10000000, 7);  //буква M в слове Mega

        //            switch (color)                                             // Хорошая вещь или нет, сверяем по картотеке
        //            {
        //                case 394901:      // soul crystal                **
        //                case 3947742:     // красная бутылка 1200HP      **
        //                case 2634708:     // красная бутылка 2500HP      **
        //                case 7171437:     // devil whisper               **
        //                case 5933520:     // маленькая красная бутылка   **
        //                case 1714255:     // митридат                    **
        //                case 7303023:     // чугун                       **
        //                case 4487528:     // honey                       **
        //                case 1522446:     // green leaf                  **
        //                case 2112641:     // red leaf                    **
        //                case 1533304:     // yelow leaf                  **
        //                case 13408291:    // shiny                       **
        //                case 3303827:     // карта перса                 **
        //                case 6569293:     // warp                        **
        //                case 662558:      // head of Mantis              **
        //                case 4497887:     // Mana Stone                  **
        //                case 7305078:     // Ящики для джеков            **
        //                case 15420103:    // Бутылка хрина               **
        //                case 9868940:     // композитная сталь           **
        //                case 5334831:     // магическая сфера            ** Magic sphere
        //                case 16777215:    // Wheat flour                 **
        //                case 13565951:    // playtoken                   **
        //                case 10986144:    // Hinge                       **
        //                case 3481651:     // Tube                        **
        //                case 6593716:     // Clock                       **
        //                case 13288135:    // Spring                      **
        //                case 7233629:     // Cogwheel                    **
        //                case 13820159:    // Family Support Token        **
        //                case 4222442:     // Wolf meat                   **
        //                case 6719975:     // Wild Boar Meat              **
        //                case 5072004:     // Bone Stick                  **
        //                case 3559777:     // Dragon Lether               **
        //                case 1712711:     // Dragon Horn                 **
        //                case 4435935:     // Yellow ore                  **
        //                case 4448154:     // Green ore                   **
        //                case 13865807:    // blue ore                    **
        //                case 4670431:     // Red ore                     **
        //                case 13291199:    // Diamond Ore                 ** *******************************************************
        //                //case 1063140:     // Stone of Philos             **
        //                //case 8486756:     // Ice Crystal                 **
        //                //case 8633037:     // Pure Gold Bar
        //                //case 8289818:     // Gray Feather
        //                //case 13068045:    // Blue Stone                
        //                //case 13627135:    // Molar                
        //                //case 5803426:     // Tangler               
        //                //case 12563070:    // Marble               
        //                //case 6380581:     // Leather         
        //                //case 14210488:    // Spool
        //                //case 3223857:     // Nail                        ************************************************************* 
        //                case 573951:      // Golden Apple
        //                case 4966811:     // Cabbage
        //                case 13164006:    // свекла  Beet                **
        //                case 5393227:     // Ebony Tree
        //                case 5131077:     // Black Sap               
        //                case 15575073:    // Blue sap               
        //                case 4143156:     // bulk of Coal                **
        ////                case 9472397:     // Steel piece                 **
        //                //case 7187897:     // Mustang ore
        //                //=================== пули ===========================
        //                case 1381654:     // стрелы эксп
        //                case 11258069:    // пули эксп Steel Bullet
        //                case 2569782:     // дробь эксп Metal Shell Ammo
        //                case 1843234:     // Steel Bolt
        //                case 14404589:    // Large Calliber Bullet
        //                //=================== пули конец =====================
        //                case 5137276:     // сундук деревянный как у сфер древней звезды
        //                //case 3031912:     // Reinforced Lether
        //                case 13667914:    // 600 SP
        //                case 2831927:     // Sign G, D
        //                case 2828377:     // Ancient Orb
        //                case 8363835:     // Icicle
        //                case 12642302:    // Bone pick
        //                case 15790834:    // Soft Cotton
        //                case 4543325:     // Pebbles
        //                case 1457773:     // Old Journal
        //                case 10401925:    // Sharp Horn
        //                case 6270101:     // Cabosse
        //                case 14344416:    // Tough Cotton
        //                case 13079681:    // Silk
        //                case 14278629:    // Chip 100
        //                case 14542297:    // Chip Veteran
        //                case 13417421:    // Octopus Arm
        //                case 3033453:     // Clear Rum
        //                case 4474675:     // Fish Flesh
        //                case 10931953:    // Psychic Sphere
        //                case 656906:      // magocal orb
        //                case 13748687:    // Ressurection Potion
        //                case 15595262:    // Small Stew обед
        //                case 3164547:     // Portable Greate обед
        //                case 3303027:     // книга со стойкой
        //                case 11906207:    // Crest of Sacred
        //                case 5590609:     // Crest of Black Knight
        //                case 15526903:    // Elementium
        //                case 1835187:     // экспертные чипы
        //                case 14146476:    // solarion
        //                case 16251642:    // growth booster
        //                case 10992324:    // Шахматы 
        //                case 7830683:     // Кожа улитки
        //                case 5205119:     // Токен 1
        //                case 6848915:     // Токен 2
        //                case 12308958:    // Токен 3
        //                case 13361389:    // Токен 4
        //                case 4345686:     // Токен Экстра
        //                case 6519909:     // Elemental Jewel
        //                case 16435563:    // Spell Key
        //                case 12697783:    // Wheel of Time
        //                case 8078490:     // Stone of Soul
        //                case 15521462:    // Piece of Naraka Lu
        //                case 12835061:    // Piece of Naraka Beel
        //                case 12566387:    // Piece of Naraka Al
        //                case 9737463:     // Piece of Naraka Mo
        //                case 13742236:    // Piece of Naraka Ma
        //                case 10000596:    // Piece of Naraka Than
        //                case 9686241:     // Piece of Naraka He
        //                case 5195666:     // Sedative
        //                case 2635075:     // Elemental Sphere (патроны)
        //                case 1585221:     // Dried Maroon
        // //               case 8633037:     // Pure Gold Bar
        //                case 1527133:     // Armonia Coin
        //                case 1023705:     // Violet's Vaucher
        //                case 5002080:     // Карточка кэш персонажа 1
        //                case 5001823:     // Карточка кэш персонажа 2
        //                case 16515071:    // Devil Dream
        //                case 3156778:     // Gift Made (по ивенту)
        //                case 47612:       // Triumph Fillers
        //                    result = false;
        //                    break;
        //                case 14210771:    // Mega Etr, Io Talt
        //                case 9803667:     // Mega A
        //                case 7645105:     // Mega Qu
        //                    if (pointMega.isColor())    result = false;     //если еще совпадает и вторая точка, то это мегакварц              не работает
        //                    break;
        //            }

        //            return result;
        //        }                         //товары здесь !!!!!!!!!!!!!!!!

        /// <summary>
        /// определяет, анализируется ли нужный товар либо данный товар можно продавать
        /// </summary>
        /// <param name="color"> цвет полностью определяет товар, который поступает на анализ </param>
        /// <returns> true, если анализируемый товар нужный и его нельзя продавать </returns>
        public bool NeedToSellProduct(uint color, bool pointMega)
        {
            bool result = true;   //по умолчанию вещь надо продавать, поэтому true

            switch (color)                                             // Хорошая вещь или нет, сверяем по картотеке
            {
                case 394901:      // soul crystal                **
                case 3947742:     // красная бутылка 1200HP      **
                case 2634708:     // красная бутылка 2500HP      **
                case 7171437:     // devil whisper               **
                case 5933520:     // маленькая красная бутылка   **
                case 1714255:     // митридат                    **
                case 7303023:     // чугун                       **
                case 4487528:     // honey                       **
                case 1522446:     // green leaf                  **
                case 2112641:     // red leaf                    **
                case 1533304:     // yelow leaf                  **
                case 13408291:    // shiny                       **
                case 3303827:     // карта перса                 **
                case 6569293:     // warp                        **
                case 662558:      // head of Mantis              **
                case 4497887:     // Mana Stone                  **
                case 7305078:     // Ящики для джеков            **
                case 15420103:    // Бутылка хрина               **
                case 9868940:     // композитная сталь           **
                case 5334831:     // магическая сфера            ** Magic sphere
                case 16777215:    // Wheat flour                 **
                case 13565951:    // playtoken                   **
                case 10986144:    // Hinge                       **
                case 3481651:     // Tube                        **
                case 6593716:     // Clock                       **
                case 13288135:    // Spring                      **
                case 7233629:     // Cogwheel                    **
                case 13820159:    // Family Support Token        **
                case 4222442:     // Wolf meat                   **
                case 6719975:     // Wild Boar Meat              **
                case 5072004:     // Bone Stick                  **
                case 3559777:     // Dragon Lether               **
                case 1712711:     // Dragon Horn                 **
                case 4435935:     // Yellow ore                  **
                case 4448154:     // Green ore                   **
                case 13865807:    // blue ore                    **
                case 4670431:     // Red ore                     **
                case 13291199:    // Diamond Ore                 ** *******************************************************
                //case 1063140:     // Stone of Philos             **
                //case 8486756:     // Ice Crystal                 **
                //case 8633037:     // Pure Gold Bar
                //case 8289818:     // Gray Feather
                //case 13068045:    // Blue Stone                
                //case 13627135:    // Molar                
                //case 5803426:     // Tangler               
                //case 12563070:    // Marble               
                //case 6380581:     // Leather         
                //case 14210488:    // Spool
                //case 3223857:     // Nail                        ************************************************************* 
                case 573951:      // Golden Apple
                case 4966811:     // Cabbage
                case 13164006:    // свекла  Beet                **
                case 5393227:     // Ebony Tree
                case 5131077:     // Black Sap               
                case 15575073:    // Blue sap               
                case 4143156:     // bulk of Coal                **
                                  //  case 9472397:     // Steel piece                 **
                                  //    case 7187897:     // Mustang ore
                                  //=================== пули ===========================

                case 11258069:    // пули эксп Steel Bullet
                case 2569782:     // дробь эксп Metal Shell Ammo
                case 1843234:     // Steel Bolt
                case 14404589:    // Large Calliber Bullet
                //=================== пули конец =====================
                case 5137276:     // сундук деревянный как у сфер древней звезды
                case 3031912:     // Reinforced Lether
                case 13667914:    // 600 SP
                case 2831927:     // Sign G, D
                case 2828377:     // Ancient Orb
                case 8363835:     // Icicle
                case 12642302:    // Bone pick
                case 15790834:    // Soft Cotton
                case 4543325:     // Pebbles
                case 1457773:     // Old Journal
                case 10401925:    // Sharp Horn
                case 6270101:     // Cabosse
                case 14344416:    // Tough Cotton
                case 13079681:    // Silk
                case 14278629:    // Chip 100
                case 14542297:    // Chip Veteran
                case 13417421:    // Octopus Arm
                case 3033453:     // Clear Rum
                case 4474675:     // Fish Flesh
                case 10931953:    // Psychic Sphere
                case 656906:      // magocal orb
                case 13748687:    // Ressurection Potion
                case 15595262:    // Small Stew обед
                case 3164547:     // Portable Greate обед
                case 3303027:     // книга со стойкой
                case 11906207:    // Crest of Sacred
                case 5590609:     // Crest of Black Knight
                case 15526903:    // Elementium
                case 1835187:     // экспертные чипы
                case 14146476:    // solarion
                case 16251642:    // growth booster
                case 10992324:    // Шахматы 
                case 7830683:     // Кожа улитки
                case 5205119:     // Токен 1
                case 6848915:     // Токен 2
                case 12308958:    // Токен 3
                case 13361389:    // Токен 4
                case 4345686:     // Токен Экстра
                case 6519909:     // Elemental Jewel
                case 16435563:    // Spell Key
                case 12697783:    // Wheel of Time
                case 8078490:     // Stone of Soul
                case 15521462:    // Piece of Naraka Lu
                case 12835061:    // Piece of Naraka Beel
                case 12566387:    // Piece of Naraka Al
                case 9737463:     // Piece of Naraka Mo
                case 13742236:    // Piece of Naraka Ma
                case 10000596:    // Piece of Naraka Than
                case 9686241:     // Piece of Naraka He
                case 5195666:     // Sedative
                case 2635075:     // Elemental Sphere (патроны)
                case 1585221:     // Dried Maroon
                                  //               case 8633037:     // Pure Gold Bar
                case 1527133:     // Armonia Coin
                case 1023705:     // Violet's Vaucher
                case 5002080:     // Карточка кэш персонажа 1
                case 5001823:     // Карточка кэш персонажа 2
                case 16515071:    // Devil Dream
                case 3156778:     // Gift Made (по ивенту)
                case 2040097:     // Gift Made (по ивенту)
                case 1644826:     // Gift Made (по ивенту)
                //case 14607342:     // desapio Necklase
                //case 15836741:     // desapio Gloves
                //case 1381654:     // desapio Boots     
                //case 3747867:     // desapio Belt
                //case 3095646:     // desapio Earrings
                case 482184:     // desapio Token
                case 47612:       // Triumph Fillers
                //case 1381654:       // обычное кольцо
                case 14722118:       // ring crystal
                    result = false;
                    break;
                case 14210771:    // Mega Etr, Io Talt
                case 9803667:     // Mega A
                case 7645105:     // Mega Qu
                    if (pointMega) result = false;     //если еще совпадает и вторая точка, то это мегакварц            
                                                       //                    if (pointMega.isColor()) result = false;     //если еще совпадает и вторая точка, то это мегакварц   
                    break;
            }

            return result;
        }                                    //товары здесь !!!!!!!!!!!!!!!!

        /// <summary>
        /// добавляем товар из указанной строки в корзину 
        /// </summary>
        /// <param name="numberOfString">номер строки</param>
        public void AddToCartLotProduct(int numberOfString)
        {
            //Point pointAddProduct = new Point(360 - 5 + botwindow.getX(), 220 - 5 + (numberOfString - 1) * 27 + botwindow.getY());
            Point pointAddProduct = new Point(363 - 5 + botwindow.getX(), 251 - 5 + (numberOfString - 1) * 27 + botwindow.getY());
            pointAddProduct.DoubleClickL();  //тыкаем в строчку с товаром
            Pause(150);
            SendKeys.SendWait("33000");
            Pause(250);
        }

        /// <summary>
        /// Продажа товаров в магазине вплоть до маленькой красной бутылки 
        /// </summary>
        public void SaleToTheRedBottle()
        {
            Product currentProduct;

            uint count = 0;
            while (!isRedBottle())
            {
                currentProduct = new Product(xx, yy, 1);  //создаем структуру "текущий товар" из трёх точек, которые мы берем у товара в первой строке магазина

                if (NeedToSellProduct2(currentProduct.color1, currentProduct.color3))    //проверяем товар в первой строке по двум точкам
                {
                    AddToCart();
                }
                else
                {
                    DownList();  //список вниз
                }
                Pause(250);  //пауза, чтобы ГЕ успела выполнить нажатие. Можно и увеличить  
                count++;
                if (count > 220) break;   // защита от бесконечного цикла
            }
        }

        /// <summary>
        /// Продажа товара после маленькой красной бутылки, до момента пока прокручивается список продажи
        /// </summary>
        public void SaleOverTheRedBottle()
        {
            Product previousProduct;
            Product currentProduct;

            currentProduct = new Product(xx, yy, 1);  //создаем структуру "текущий товар" из трёх точек, которые мы берем у товара в первой строке магазина

            do
            {
                previousProduct = currentProduct;
                //                if (NeedToSellProduct(currentProduct.color1, 1))    //проверяем товар в первой строке
                if (NeedToSellProduct(currentProduct.color1, currentProduct.colorMega))    //проверяем товар в первой строке по двум точкам
                    AddToCartLotProduct(1);

                DownList();  //список вниз
                Pause(250);  //пауза, чтобы ГЕ успела выполнить нажатие. Можно и увеличить     

                currentProduct = new Product(xx, yy, 1);
            } while (!currentProduct.EqualProduct(previousProduct));          //идет проверка по трем точкам
        }

        /// <summary>
        /// Продажа товара, когда список уже не прокручивается 
        /// </summary>
        public void SaleToEnd()
    {
        Product currentProduct;

        for (int j = 2; j <= 12; j++)
        {
            currentProduct = new Product(xx, yy, j);  //создаем структуру "текущий товар" из четырех точек, которые мы берем у товара в j-й строке магазина
            if (NeedToSellProduct(currentProduct.color1, currentProduct.colorMega))       //если нужно продать товар в строке j
                AddToCartLotProduct(j);                                         //добавляем в корзину весь товар в строке j
        }
    }

        /// <summary>
        /// Кликаем в кнопку BUY  в магазине 
        /// </summary>
        public void Botton_BUY()
        {
            pointButtonBUY.PressMouseL();
            pointButtonBUY.PressMouseL();
            Pause(2000);
        }

        /// <summary>
        /// Кликаем в кнопку Sell  в магазине 
        /// </summary>
        public void Botton_Sell()
        {
            pointButtonSell.PressMouseL();
            pointButtonSell.PressMouseL();
//            botwindow.setStatusOfSale(0);
            //globalParam.StatusOfSale = 0;
            botParam.StatusOfSale = 0;
            Pause(2000);
        }

        /// <summary>
        /// Кликаем в кнопку Close в магазине
        /// </summary>
        public void Button_Close()
        {
            pointButtonClose.PressMouse();
            Pause(2000);
        }

        /// <summary>
        /// Покупка митридата в количестве 333 штук
        /// </summary>
        public void BuyingMitridat()
        {
            pointBuyingMitridat1.PressMouseL();             //кликаем левой кнопкой в ячейку, где надо написать количество продаваемого товара
            Pause(150);

            SendKeys.SendWait("333");

            Botton_BUY();                             // Нажимаем на кнопку BUY 


            pointBuyingMitridat2.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если переполнение митридата
            Pause(500);

            pointBuyingMitridat3.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если нет денег на покупку митридата
            Pause(500);
        }


        // закомментировано
        ///// <summary>
        ///// Посылаем нажатие числа 333 в окно с ботом с помощью команды PostMessage
        ///// </summary>
        //public void Press333()
        //{
        //    const int WM_KEYDOWN = 0x0100;
        //    const int WM_KEYUP = 0x0101;
        //    UIntPtr lParam = new UIntPtr();
        //    UIntPtr HWND = FindWindowEx(botwindow.getHwnd(), UIntPtr.Zero, "Edit", "");   // это handle дочернего окна ге, т.е. области, где можно писать циферки

        //    for (int i = 1; i <= 3; i++)
        //    {
        //        uint dd = 0x00400001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYDOWN, (UIntPtr)Keys.D3, lParam);
        //        Pause(150);

        //        dd = 0xC0400001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYUP, (UIntPtr)Keys.D3, lParam);
        //        Pause(150);
        //    }
        //}

        ///// <summary>
        ///// Посылаем нажатие числа 44444 в окно с ботом с помощью команды PostMessage
        ///// </summary>
        //public void Press44444()
        //{
        //    const int WM_KEYDOWN = 0x0100;
        //    const int WM_KEYUP = 0x0101;
        //    UIntPtr lParam = new UIntPtr();
        //    UIntPtr HWND = FindWindowEx(botwindow.getHwnd(), UIntPtr.Zero, "Edit", "");   // это handle дочернего окна ге, т.е. области, где можно писать циферки

        //    for (int i = 1; i <= 5; i++)
        //    {
        //        uint dd = 0x00500001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYDOWN, (UIntPtr)Keys.D4, lParam);
        //        Pause(150);

        //        dd = 0xC0500001;
        //        lParam = (UIntPtr)dd;
        //        PostMessage(HWND, WM_KEYUP, (UIntPtr)Keys.D4, lParam);
        //        Pause(150);
        //    }
        //}


        #endregion


        ///// <summary>
        ///// выход из магазина путем нажатия кнопки Exit
        ///// </summary>
        //public void PressExitFromShop()                   
        //{
        //    dialog.PressOkButton(1);      ////========= тыкаем в OK =======================
        //}


    }
}
