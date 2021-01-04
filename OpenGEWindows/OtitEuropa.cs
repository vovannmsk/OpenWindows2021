namespace OpenGEWindows
{
    public class OtitEuropa : Otit
    {
        public OtitEuropa()
        { }

        public OtitEuropa(botWindow botwindow)
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
            iPoint[] route = { new Point(505 - 5 + xx, 505 - 5 + yy), 
                               new Point(462 - 5 + xx, 468 - 5 + yy), 
                               new Point(492 - 5 + xx, 437 - 5 + yy), 
                               new Point(539 - 5 + xx, 486 - 5 + yy), 
                               new Point(462 - 5 + xx, 468 - 5 + yy) };

            iPoint result = route[counterRouteNode];
            counterRouteNode++; if (counterRouteNode > 4) counterRouteNode = 0;

            return result;
        }


        /// <summary>
        /// получаем время для прохождения следующего участка маршрута
        /// </summary>
        /// <returns>время в мс</returns>
        public override int RouteNextPointTime()
        {
            int[] routeTime = { 10000, 15000, 15000, 15000, 15000 };

            int result = routeTime[counterRouteNode];

            return result;
        }


    }
}
