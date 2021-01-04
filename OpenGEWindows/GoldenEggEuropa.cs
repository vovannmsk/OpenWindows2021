namespace OpenGEWindows
{
    public class GoldenEggEuropa : GoldenEgg
    {

        public GoldenEggEuropa ()
        {}

        public GoldenEggEuropa(botWindow botwindow)
        {
            #region общие

            this.botwindow = botwindow;
            this.xx = botwindow.getX();
            this.yy = botwindow.getY();

            #endregion

            DialogFactory dialogFactory = new DialogFactory(this.botwindow);
            dialog = dialogFactory.createDialog();

        }

        // ===============================  Методы ==================================================



        /// <summary>
        /// получаем следующую точку маршрута
        /// </summary>
        /// <returns></returns>
        public override iPoint RouteNextPoint()
        {

            iPoint[] route ={ 
                                   new Point(387 - 5 + xx, 461 - 5 + yy), 
                                   new Point(311 - 5 + xx, 455 - 5 + yy), 
                                   new Point(290 - 5 + xx, 352 - 5 + yy), 
                                   new Point(308 - 5 + xx, 249 - 5 + yy), 
                                   new Point(412 - 5 + xx, 267 - 5 + yy), 
                                   new Point(498 - 5 + xx, 311 - 5 + yy), 
                                   new Point(498 - 5 + xx, 388 - 5 + yy), 
                                   new Point(487 - 5 + xx, 448 - 5 + yy)
                              };

            iPoint result = route[counterRouteNode];

            return result;
        }


        /// <summary>
        /// получаем время для прохождения следующего участка маршрута
        /// </summary>
        /// <returns>время в мс</returns>
        public override int RouteNextPointTime()
        {
            int[] routeTime = { 7000, 7000, 7000, 7000, 7000, 7000, 7000, 7000 };

            int result = routeTime[counterRouteNode];

            return result;
        }

    }
}
