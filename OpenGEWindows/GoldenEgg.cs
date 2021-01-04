namespace OpenGEWindows
{
    public abstract class GoldenEgg : Server2
    {

        protected Dialog dialog;
        //protected Server server;
        protected static int counterRouteNode = 0;


        // ============  методы  ========================


        /// <summary>
        /// входим на ферму (диалог у Эстебана)
        /// </summary>
        public void EnterToFarm()
        {
            dialog.PressOkButton(1);

            dialog.PressStringDialog(1);
            dialog.PressOkButton(1);
            botwindow.Pause(5000);
        }


        /// <summary>
        /// переход по карте Земли мертвых к месту начала маршрута для набивания андидов (100 шт.)
        /// </summary>
        public void GotoNextPointRoute()
        {
            RouteNextPoint().PressMouseR();
            Pause(500);
            RouteNextPoint().PressMouseR();
            Pause(RouteNextPointTime());
            counterRouteNode++; if (counterRouteNode > 7) counterRouteNode = 0;
        }

        /// <summary>
        /// получаем следующий пункт маршрута
        /// </summary>
        /// <returns>следующая точка</returns>
        public abstract iPoint RouteNextPoint();

        /// <summary>
        /// получаем время для прохождения следующего участка маршрута
        /// </summary>
        /// <returns>время в мс</returns>
        public abstract int RouteNextPointTime();




    }

}
