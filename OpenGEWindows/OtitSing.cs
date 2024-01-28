namespace OpenGEWindows
{
    public class OtitSing : Otit
    {

        public OtitSing ()
        {}

        public OtitSing(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            DialogFactory dialogFactory = new DialogFactory(this.botwindow);
            dialog = dialogFactory.createDialog();

            this.pointOldMan1 = new PointColor(907 - 5 + xx, 675 - 5 + yy, 7800000, 5);
            this.pointOldMan2 = new PointColor(907 - 5 + xx, 676 - 5 + yy, 7800000, 5);
             
            this.pointTask1 = new PointColor(891 - 5 + xx, 152 - 5 + yy, 7800000, 5);   //цифра 1 в числе 100    
            this.pointTask2 = new PointColor(891 - 5 + xx, 153 - 5 + yy, 7800000, 5);   //23-11

            //this.pointGetTask1 = new PointColor(764 - 5 + xx, 383 - 5 + yy, 16100000, 5); 
            //this.pointGetTask2 = new PointColor(764 - 5 + xx, 384 - 5 + yy, 16100000, 5);
            this.pointGetTask1 = new PointColor(783 - 5 + xx, 151 - 5 + yy, 16100000, 5);  //буква R в слове Refined
            this.pointGetTask2 = new PointColor(783 - 5 + xx, 152 - 5 + yy, 16100000, 5);

            //this.pointIsOpenMap1 = new PointColor(245 - 5 + xx, 108 - 5 + yy, 8000000, 6);
            //this.pointIsOpenMap2 = new PointColor(246 - 5 + xx, 108 - 5 + yy, 8000000, 6);
            this.pointIsOpenMap1 = new PointColor(253 - 5 + xx, 124 - 5 + yy, 16000000, 6);  //буква М в слове Zone Map
            this.pointIsOpenMap2 = new PointColor(253 - 5 + xx, 125 - 5 + yy, 16000000, 6);

            this.pointIsPerfume1 = new PointColor(0 - 5 + xx, 0 - 5 + yy, 7000000, 6);   //не готово
            this.pointIsPerfume2 = new PointColor(0 - 5 + xx, 0 - 5 + yy, 7000000, 6);

//            this.pointIsNearOldMan = new PointColor(957 - 5 + xx, 133 - 5 + yy, 2303198, 0); старый вариант
            this.pointIsNearOldMan = new PointColor(957 - 5 + xx, 133 - 5 + yy, 2303000, 3);


            this.pointMamons = new Point(526 - 5 + xx, 262 - 5 + yy);
            this.pointOldMan = new Point(531 - 5 + xx, 313 - 5 + yy);
            //this.pointOldManOnMap = new Point(740 - 5 + xx, 212 - 5 + yy);
            this.pointOldManOnMap = new Point(740 - 5 + xx, 237 - 5 + yy);
            //this.pointButtonMoveOnMap = new Point(835 - 5 + xx, 635 - 5 + yy);
            this.pointButtonMoveOnMap = new Point(845 - 5 + xx, 645 - 5 + yy);    //23-11

            this.numberOfRoute = NumberOfRoute();
            //this.CounterRouteNode = 0;
        }

        // ===============================  Методы ==================================================

        /// <summary>
        /// получаем следующую точку маршрута
        /// </summary>
        /// <returns></returns>
        public override iPoint RouteNextPoint()
        {

            iPoint [,] route ={ 
                                  { new Point(510 - 5 + xx, 510 - 5 + yy), new Point(474 - 5 + xx, 478 - 5 + yy), new Point(510 - 5 + xx, 479 - 5 + yy) }, 
                                  { new Point(574 - 5 + xx, 419 - 5 + yy), new Point(516 - 5 + xx, 441 - 5 + yy), new Point(568 - 5 + xx, 449 - 5 + yy) }, 
                                  { new Point(339 - 5 + xx, 380 - 5 + yy), new Point(292 - 5 + xx, 355 - 5 + yy), new Point(291 - 5 + xx, 405 - 5 + yy) }, 
                                  { new Point(409 - 5 + xx, 344 - 5 + yy), new Point(367 - 5 + xx, 324 - 5 + yy), new Point(415 - 5 + xx, 294 - 5 + yy) }, 
                                  { new Point(384 - 5 + xx, 357 - 5 + yy), new Point(311 - 5 + xx, 352 - 5 + yy), new Point(355 - 5 + xx, 317 - 5 + yy) }, 
                                  { new Point(545 - 5 + xx, 484 - 5 + yy), new Point(481 - 5 + xx, 472 - 5 + yy), new Point(526 - 5 + xx, 447 - 5 + yy) }

                                  //{ new Point(505 - 5 + xx, 505 - 5 + yy), new Point(462 - 5 + xx, 468 - 5 + yy), new Point(505 - 5 + xx, 474 - 5 + yy) },
                                  //{ new Point(569 - 5 + xx, 414 - 5 + yy), new Point(511 - 5 + xx, 436 - 5 + yy), new Point(563 - 5 + xx, 444 - 5 + yy) },
                                  //{ new Point(334 - 5 + xx, 375 - 5 + yy), new Point(287 - 5 + xx, 350 - 5 + yy), new Point(286 - 5 + xx, 400 - 5 + yy) },
                                  //{ new Point(404 - 5 + xx, 339 - 5 + yy), new Point(362 - 5 + xx, 319 - 5 + yy), new Point(410 - 5 + xx, 289 - 5 + yy) },
                                  //{ new Point(379 - 5 + xx, 352 - 5 + yy), new Point(306 - 5 + xx, 347 - 5 + yy), new Point(350 - 5 + xx, 312 - 5 + yy) },
                                  //{ new Point(540 - 5 + xx, 479 - 5 + yy), new Point(476 - 5 + xx, 467 - 5 + yy), new Point(521 - 5 + xx, 442 - 5 + yy) }

                              };

            iPoint result = route[NumberOfRoute(), CounterRouteNode];

            return result;
        }

        /// <summary>
        /// получаем следующую точку маршрута для режима много окон
        /// </summary>
        /// <returns></returns>
        public override iPoint RouteNextPointMulti(int counter)
        {
            iPoint[,] routeMulti ={
                                  { new Point(510 - 5 + xx, 510 - 5 + yy), new Point(467 - 5 + xx, 473 - 5 + yy), new Point(510 - 5 + xx, 479 - 5 + yy) },
                                  { new Point(574 - 5 + xx, 419 - 5 + yy), new Point(516 - 5 + xx, 441 - 5 + yy), new Point(568 - 5 + xx, 449 - 5 + yy) },
                                  { new Point(339 - 5 + xx, 380 - 5 + yy), new Point(292 - 5 + xx, 355 - 5 + yy), new Point(291 - 5 + xx, 405 - 5 + yy) },
                                  { new Point(409 - 5 + xx, 344 - 5 + yy), new Point(367 - 5 + xx, 324 - 5 + yy), new Point(415 - 5 + xx, 294 - 5 + yy) },
                                  { new Point(384 - 5 + xx, 357 - 5 + yy), new Point(311 - 5 + xx, 352 - 5 + yy), new Point(355 - 5 + xx, 317 - 5 + yy) },
                                  { new Point(545 - 5 + xx, 484 - 5 + yy), new Point(481 - 5 + xx, 472 - 5 + yy), new Point(526 - 5 + xx, 447 - 5 + yy) }
                                  //{ new Point(505 - 5 + xx, 505 - 5 + yy), new Point(462 - 5 + xx, 468 - 5 + yy), new Point(505 - 5 + xx, 474 - 5 + yy) },
                                  //{ new Point(569 - 5 + xx, 414 - 5 + yy), new Point(511 - 5 + xx, 436 - 5 + yy), new Point(563 - 5 + xx, 444 - 5 + yy) },
                                  //{ new Point(334 - 5 + xx, 375 - 5 + yy), new Point(287 - 5 + xx, 350 - 5 + yy), new Point(286 - 5 + xx, 400 - 5 + yy) },
                                  //{ new Point(404 - 5 + xx, 339 - 5 + yy), new Point(362 - 5 + xx, 319 - 5 + yy), new Point(410 - 5 + xx, 289 - 5 + yy) },
                                  //{ new Point(379 - 5 + xx, 352 - 5 + yy), new Point(306 - 5 + xx, 347 - 5 + yy), new Point(350 - 5 + xx, 312 - 5 + yy) },
                                  //{ new Point(540 - 5 + xx, 479 - 5 + yy), new Point(476 - 5 + xx, 467 - 5 + yy), new Point(521 - 5 + xx, 442 - 5 + yy) }
                              };

            iPoint result = routeMulti[numberOfRoute, counter];

            return result;
        }

        /// <summary>
        /// получаем время для прохождения следующего участка маршрута (паузы между точками маршрута)
        /// </summary>
        /// <returns>время в мс</returns>
        public override int RouteNextPointTime() 
        {
            int[,] routeTime = {
                                   { 18000, 33000, 23000 },
                                   { 15000, 35000, 25000 },
                                   { 20000, 35000, 25000 },
                                   { 20000, 25000, 35000 },
                                   { 20000, 30000, 30000 },
                                   { 20000, 30000, 30000 }
                               };

            int result = routeTime[NumberOfRoute(), CounterRouteNode];

            return result;
        }

        /// <summary>
        /// получить задачу у старого мужика
        /// </summary>
        public override void GetTask()
        {
            dialog.PressStringDialog(3);
            botwindow.Pause(1000);
            dialog.PressStringDialog(2);
            botwindow.Pause(1000);
            dialog.PressOkButton(1);
            botwindow.Pause(1000);
        }

        /// <summary>
        /// Диалог OldMan. Войти в Земли Мертвых через старого мужика
        /// </summary>
        public override void EnterToTierraDeLosMuertus()
        {
            dialog.PressStringDialog(4);
            botwindow.Pause(1000);
            dialog.PressStringDialog(2);     // стартовая точка - в середине карты
            botwindow.Pause(1000);
            dialog.PressStringDialog(1);    //move  (в миссию)
            botwindow.Pause(1000);
        }

        /// <summary>
        /// получить чистый отит (забрать в диалоге у старого мужика)
        /// </summary>
        public override void TakePureOtite()
        {
            //dialog.PressStringDialog(3);
            //dialog.PressOkButton(1);
            //dialog.PressStringDialog(1);
            //botwindow.Pause(500);
            //dialog.PressOkButton(1);
            //botwindow.Pause(1000);
            //dialog.PressOkButton(1);
            //botwindow.Pause(1000);
            //dialog.PressOkButton(1);

            dialog.PressStringDialog(3);
            botwindow.Pause(1000);
            dialog.PressStringDialog(1);
            botwindow.Pause(1000);
            dialog.PressOkButton(1);
            botwindow.Pause(1000);
            dialog.PressOkButton(1);
            botwindow.Pause(1000);
        }

    }
}