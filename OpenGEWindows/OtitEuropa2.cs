namespace OpenGEWindows
{
    public class OtitEuropa2 : Otit
    {

        public OtitEuropa2()
        {}

        public OtitEuropa2(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            //this.server = botwindow.getserver();
            ServerFactory serverFactory = new ServerFactory(botwindow);
            this.server = serverFactory.create();   // создали конкретный экземпляр класса server по паттерну "простая Фабрика" (Америка, Европа или Синг)
            this.town = server.getTown();
            DialogFactory dialogFactory = new DialogFactory(this.botwindow);
            dialog = dialogFactory.createDialog();

            this.pointOldMan1 = new PointColor(907 - 5 + xx, 675 - 5 + yy, 7800000, 5);
            this.pointOldMan2 = new PointColor(907 - 5 + xx, 676 - 5 + yy, 7800000, 5);

            this.pointTask1 = new PointColor(928 - 5 + xx, 360 - 5 + yy, 8200000, 5);
            this.pointTask2 = new PointColor(928 - 5 + xx, 361 - 5 + yy, 8200000, 5);

            this.pointMamons = new Point(526 - 5 + xx, 262 - 5 + yy);

        }

        // ===============================  Методы ==================================================



        /// <summary>
        /// получаем следующую точку маршрута
        /// </summary>
        /// <returns></returns>
        public override iPoint RouteNextPoint()
        {

            iPoint [,] route ={ 
                                  { new Point(505 - 5 + xx, 505 - 5 + yy), new Point(462 - 5 + xx, 468 - 5 + yy), new Point(505 - 5 + xx, 474 - 5 + yy) }, 
                                  { new Point(569 - 5 + xx, 414 - 5 + yy), new Point(511 - 5 + xx, 436 - 5 + yy), new Point(563 - 5 + xx, 444 - 5 + yy) }, 
                                  { new Point(334 - 5 + xx, 375 - 5 + yy), new Point(287 - 5 + xx, 350 - 5 + yy), new Point(286 - 5 + xx, 400 - 5 + yy) }, 
                                  { new Point(404 - 5 + xx, 339 - 5 + yy), new Point(362 - 5 + xx, 319 - 5 + yy), new Point(410 - 5 + xx, 289 - 5 + yy) }, 
                                  { new Point(379 - 5 + xx, 352 - 5 + yy), new Point(306 - 5 + xx, 347 - 5 + yy), new Point(350 - 5 + xx, 312 - 5 + yy) }, 
                                  { new Point(540 - 5 + xx, 479 - 5 + yy), new Point(476 - 5 + xx, 467 - 5 + yy), new Point(521 - 5 + xx, 442 - 5 + yy) }
                              };

            iPoint result = route[NumberOfRoute(), counterRouteNode];

            return result;
        }


        /// <summary>
        /// получаем время для прохождения следующего участка маршрута
        /// </summary>
        /// <returns>время в мс</returns>
        public override int RouteNextPointTime() 
        {
            int[,] routeTime = {
                                   { 20000, 35000, 25000 },
                                   { 15000, 35000, 25000 },
                                   { 20000, 35000, 25000 },
                                   { 20000, 25000, 35000 },
                                   { 20000, 30000, 30000 },
                                   { 20000, 30000, 30000 }
                               };

            int result = routeTime[NumberOfRoute(), counterRouteNode];

            return result;
        }

    }
}
