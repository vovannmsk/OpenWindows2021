using System.Windows.Forms;
using GEBot.Data;

namespace OpenGEWindows
{
    public abstract class Market : Server2
    {

        // ============ переменные ======================

        #region Shop

        protected iPointColor pointIsSale1;
        protected iPointColor pointIsSale2;
        protected iPointColor pointIsSale21;
        protected iPointColor pointIsSale22;
        protected iPointColor pointIsClickSale1;
        protected iPointColor pointIsClickSale2;
        protected iPointColor pointIsClickPurchase1;
        protected iPointColor pointIsClickPurchase2;
        protected iPoint pointBookmarkSell;
        protected iPoint pointSaleToTheRedBottle;
        protected iPoint pointSaleOverTheRedBottle;
        protected iPoint pointWheelDown;
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
                color1 =    new PointColor(149 - 5 + xx, 219 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color2 =    new PointColor(146 - 5 + xx, 219 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                color3 =    new PointColor(165 - 5 + xx, 214 - 5 + yy + (numberOfString - 1) * 27, 3360337, 0).GetPixelColor();
                colorMega = new PointColor(174 - 5 + xx, 214 - 5 + yy + (numberOfString - 1) * 27, 10000000, 7).isColor();          //буква M в слове Mega (для отлова МегаРесурсов)
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
        /// Кликаем на строчку Sell и кнопку "Ok" в магазине   
        /// </summary>
        public void ClickSellAndOkInTrader()
        {
            dialog.PressStringDialog(1);  ////========= тыкаем в "Sell/Buy Items" ======================================
            dialog.PressOkButton(1);      ////========= тыкаем в OK =======================
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
        /// проверяет, находится ли данное окно в самом магазине (на закладке BUY или SELL)                                       
        /// </summary>
        /// <returns> true, если находится в магазине </returns>
        public bool isSale2()
        {
            return ((pointIsSale21.isColor()) && (pointIsSale22.isColor()));
        }

        /// <summary>
        /// проверяет, открыта ли закладка Purchase (BUY) в магазине 
        /// </summary>
        /// <returns> true, если закладка Purchase в магазине открыта </returns>
        public bool isClickPurchase()
        {
            return ((pointIsClickPurchase1.isColor()) && (pointIsClickPurchase2.isColor()));
        }

        /// <summary>
        /// проверяет, открыта ли закладка Sell в магазине 
        /// </summary>
        /// <returns> true, если закладка Sell в магазине открыта </returns>
        public bool isClickSell()
        {
            return ((pointIsClickSale1.isColor()) && (pointIsClickSale2.isColor()));
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
        /// либо другими красными бутылками
        /// </summary>
        /// <returns> true, если в первой строке маленькая красная бутылка </returns>
        public bool isRedBottle()
        {
            //PointColor pointFirstString = new PointColor(147 - 5 + xx, 224 - 5 + yy, 3360337, 0);
            //return pointFirstString.isColor();
            uint Color = new PointColor(149 - 5 + xx, 219 - 5 + yy, 0, 0).GetPixelColor();
            return  (Color == 5933520) ||       //500 HP     маленькая красная бутылка
                    (Color == 3947742) ||       //1500 HP
                    (Color == 2634708) ||       //2500 HP
                    (Color == 1714255) ||       //Mitridat
                    (Color == 13667914);        //600 SP
        }

        /// <summary>
        /// добавляем товар из первой строки в корзину 
        /// </summary>
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
//          if (botwindow.getIsServer())
            if (globalParam.Samara)
            {
                // вариант 1. нажатие на стрелку вниз в магазине   (для самарских серверов)
                iPoint pointArrowDown = new Point(507 - 5 + botwindow.getX(), 549 - 5 + botwindow.getY());
                pointArrowDown.PressMouseL();
            }
            else
            {
                // вариант 2. колесик вниз
                pointAddProduct.PressMouseWheelDown();
            }
        }

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
                case 13291199:    // Diamond Ore                 ** **********************Катовия*****************************
                case 1063140:     // Stone of Philos             **
                case 8486756:     // Ice Crystal                 **
                //case 8633037:     // Pure Gold Bar
                case 8289818:     // Gray Feather
                case 13068045:    // Blue Stone                
                case 13627135:    // Molar                
                case 5803426:     // Tangler               
                case 12563070:    // Marble               
                case 6380581:     // Leather         
                case 14210488:    // Spool
                case 11464703:      //Snow Silk
                case 3223857:     // Nail                        ************************************************************* 
                case 573951:      // Golden Apple
                case 4966811:     // Cabbage
                case 13164006:    // свекла  Beet                **
                case 5393227:     // Ebony Tree
                case 5131077:     // Black Sap               
                case 15575073:    // Blue sap               
                case 4143156:     // bulk of Coal                **
                case 9472397:     // Steel piece                 **
                case 7187897:     // Mustang ore
                //=================== пули ===========================
                case 10931953:    // Psychic Sphere
                case 11258069:    // пули эксп Steel Bullet
                case 2569782:     // дробь эксп Metal Shell Ammo
                case 1843234:     // Steel Bolt
                case 14404589:    // Large Calliber Bullet
                case 2635075:     // Elemental Sphere (патроны)
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
                // Чипы
                case 14278629:    // Chip 100 и ниже
                case 14542297:    // Chip Veteran
                case 1835187:     // экспертные чипы
                case 16771747:    // мастер чипы

                case 13417421:    // Octopus Arm
                case 3033453:     // Clear Rum
                case 4474675:     // Fish Flesh
                
                case 656906:      // magocal orb
                case 13748687:    // Ressurection Potion
                case 15595262:    // Small Stew обед
                case 3164547:     // Portable Greate обед
                case 3303027:     // книга со стойкой
                case 15526903:    // Elementium
                case 14146476:    // solarion
                case 16251642:    // growth booster
                case 10992324:    // Шахматы 
                case 7830683:     // Кожа улитки
                //  ============== Токены =================
                case 5205119:     // Токен 1
                case 6848915:     // Токен 2
                case 12308958:    // Токен 3
                case 13361389:    // Токен 4
                case 4345686:     // Токен Экстра
                case 12436150:     // Токен 1+
                case 14607593:     // Токен 2+
                case 15002610:     // Токен 3+

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
                case 1585221:     // Dried Maroon
                case 1527133:     // Armonia Coin
                case 1023705:     // Violet's Vaucher
                case 5002080:     // Карточка кэш персонажа 1
                case 5001823:     // Карточка кэш персонажа 2
                case 16515071:    // Devil Dream
                case 3156778:     // Gift Made (по ивенту)
                case 2040097:     // Gift Made (по ивенту)
                case 1644826:     // Gift Made (по ивенту)
                case 1357499:     // суп любой
                case 8321791:     // Camelianium
                case 482184:      // desapio Token
                case 47612:       // Triumph Fillers
                case 16367578:    // Expert Stance Book (белый прямоугольник)
                case 7255958:     // Character Ring Crystal (светло-зеленый)
                case 8175072:       //Dragon Heart
                case 5145944:       //Scale Of Siren
                case 13957608:      //Seed of Raff
                case 1316028:       //Arch Heart
                case 7966641:       //Recipe перчи
                case 16777210:      //Рецепт Хрома
                case 14605266:      //Рецепт Армония+Страта
                //================ БХ Demonic =====================
                case 11906207:    // Crest of Sacred
                case 5590609:     // Crest of Black Knight
                case 8094315:     // Crest 3
                case 33557709:    // Crest 4
                case 8410380:     // Nephthis
                case 16745983:    // Death Wraith
                case 131094:      // Will of Cortes
                case 9103099:     // Revengence
                case 1381654:     // Branch + обычное кольцо + рецепт ботинок
                case 8091251:     // Saurelle
                case 13879799:    // Shell of Arb
                case 171761:        // желтая коробка с петом
                case 8999482:       // Simbol of Naraka
                case 9867973:       // страта нож
                case 10987435:      // divine polearm
                case 10998261:      // divine special Braslet
                case 4089968:       // Divine Fire Br
                case 13030879:      // Divine Ice Br
                case 1908255:       // рецепт Enhanted Will of Argus
                case 1644569:       // рецепт Shadow of Argus


                    result = false;
                    break;
                case 14210771:    // Mega Etr, Io Talt
                case 9803667:     // Mega A
                case 7645105:     // Mega Qu
                    if (pointMega)    result = false;     //если еще совпадает и вторая точка, то это мегакварц            
//                    if (pointMega.isColor()) result = false;     //если еще совпадает и вторая точка, то это мегакварц   
                    break;
            }

            return result;
        }                                    //товары здесь !!!!!!!!!!!!!!!!

        /// <summary>
        /// определяет, анализируется ли нужный товар либо данный товар можно продавать (проверяем только оружие и броню /до красной бутылки/)
        /// </summary>
        /// <param name="color"> цвет полностью определяет товар, который поступает на анализ </param>
        /// <returns> true, если анализируемый товар нужно продавать </returns>
        public bool NeedToSellProduct2(uint color, uint color3)
        {
            bool result = true;   //по умолчанию вещь надо продавать, поэтому true

            switch (color)                                             // Хорошая вещь или нет, сверяем по картотеке
            {
                case 14607342:      // desapio Necklase
                case 15836741:      // desapio Gloves
                case 1381654:       // desapio Boots 
                case 1316118:       // desapio Boots желтые
                case 3747867:       // desapio Belt
                case 3095646:       // desapio Earrings
                    if ((color3 == 14978083) || (color3 == 3527902) || (color3 == 7040364)) result = false;     //смотрим цвет слова Desapio (синий или желтый цвет)            
                break;
                case 5933520:
                    result = false; //маленькая красная бутылка
                    break;
                case 857126:        //Elite Le Noir and Le Noir
                    if (new PointColor(151 - 5 + xx, 209 - 5 + yy, 16712191, 0).isColor())  //Elite Le Noir розовая точка
                        result = false;     
                    break;
                case 9867973:       // страта нож

                //case 1381654:     // Divine Rapier
                case 5859699:       // Divine Lute
                case 1906191:       // Divine Slayer
                case 13953779:      // Divine Sabre
                case 8231339:       // Divine Knuckle
                case 9546436:       // Divine Dagger
                case 10987435:      // divine polearm
                case 10998261:      // divine special Braslet
                case 4089968:       // Divine Fire Br
                case 13030879:      // Divine Ice Br
                case 1381398:       // Divine Light Br

                case 6056324:       // IAR-323
                case 1648992:       // Elite Le Blanc (Leather, Coat, Robe)
                case 329476:        // General Plate
                //case 15981491:    // Elite Schvarlier Armor
                    result = false;
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
            Point pointAddProduct = new Point(360 - 5 + botwindow.getX(), 220 - 5 + (numberOfString - 1) * 27 + botwindow.getY());  //305 + 30, 190 + 30)
            pointAddProduct.DoubleClickL();  //тыкаем в строчку с товаром
            Pause(150);
            SendKeys.SendWait("33000");
            Pause(250);
        }

        /// <summary>
        /// Продажа товаров в магазине вплоть до маленькой красной бутылки 
        /// </summary>
        public void SaleToTheRedBottle(int limit)
        {
            Product currentProduct;

            uint count = 0;
            while (!isRedBottle())
            {
                currentProduct = new Product(xx, yy, 1);  //создаем структуру "текущий товар" из трёх точек, которые мы берем у товара в первой строке магазина

                if (NeedToSellProduct2(currentProduct.color1, currentProduct.color3))    //проверяем товар в первой строке по двум точкам
                    AddToCart();
                else
                {
                    DownList();  //список вниз
                    Pause(250);  //пауза, чтобы ГЕ успела выполнить нажатие. Можно и увеличить  
                }
                count++;
                if (count > limit) break;   // защита от бесконечного цикла
            }
            //DownList();  //список вниз
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
            //botwindow.setStatusOfSale(0);
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
            //PressMouse(847, 663);
        }

        /// <summary>
        /// Покупка митридата в количестве 333 штук
        /// </summary>
        public void BuyingMitridat()
        {
            //botwindow.PressMouseL(360, 537);          //кликаем левой кнопкой в ячейку, где надо написать количество продаваемого товара
            pointBuyingMitridat1.PressMouseL();             //кликаем левой кнопкой в ячейку, где надо написать количество продаваемого товара
            Pause(150);

            //Press333();
            SendKeys.SendWait("333");

            Botton_BUY();                             // Нажимаем на кнопку BUY 


            pointBuyingMitridat2.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если переполнение митридата
            //botwindow.PressMouseL(1392 - 875, 438 - 5);         
            Pause(500);

            pointBuyingMitridat3.PressMouseL();           //кликаем левой кнопкой мыши в кнопку Ок, если нет денег на покупку митридата
            //botwindow.PressMouseL(1392 - 875, 428 - 5);          
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
