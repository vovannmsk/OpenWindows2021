using System.IO;


namespace OpenGEWindows
{
    public abstract class Otit : Server2
    {

        protected iPointColor pointOldMan1;
        protected iPointColor pointOldMan2;
        protected iPointColor pointTask1;
        protected iPointColor pointTask2;
        protected Dialog dialog;
        protected Server server;
        protected static int counterRouteNode;
        protected iPoint pointMamons;


        // ============  методы  ========================

        ///// <summary>
        ///// проверяем, находимся ли мы в диалоге со старым мужиком в Лос Толдосе
        ///// </summary>
        //public bool isOldMan()
        //{
        //    return (pointOldMan1.isColor() && pointOldMan2.isColor());
        //}

        /// <summary>
        /// меняем маршрут на дополнительный
        /// </summary>
        public void ChangeNumberOfRoute() 
        {
            int result = 0;
            switch (NumberOfRoute())
            {
                case 0:
                    result = 1;
                    break;
                case 1:
                    result = 0;
                    break;
                case 2:
                    result = 3;
                    break;
                case 3:
                    result = 2;
                    break;
                case 4:
                    result = 5;
                    break;
                case 5:
                    result = 4;
                    break;
            }
            SaveNumberOfRoute(result);
        }

        /// <summary>
        /// метод записывает в файл номер маршрута для фарма чистого отита
        /// </summary>
        protected void SaveNumberOfRoute(int number)
        { 
            File.WriteAllText(KATALOG_MY_PROGRAM + botwindow.getNumberWindow() + "\\Номер маршрута.txt", number.ToString()); 
        }

        /// <summary>
        /// функция возвращает номер маршрута для фарма чистого отита
        /// </summary>
        /// <returns>номер маршрута от 0 до 3</returns>
        protected int NumberOfRoute()
        { return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + botwindow.getNumberWindow() + "\\Номер маршрута.txt")); }

        /// <summary>
        /// получить задачу у старого мужика
        /// </summary>
        public void GetTask()
        {
            dialog.PressStringDialog(1);
            dialog.PressOkButton(1);

            dialog.PressStringDialog(2);
            dialog.PressOkButton(2);

        }

        /// <summary>
        /// проверяем, выполнено ли задание
        /// </summary>
        /// <returns></returns>
        public bool isTaskDone()
        {
            return (pointTask1.isColor() && pointTask2.isColor());
        }

        /// <summary>
        /// войти в Земли Мертвых через старого мужика
        /// </summary>
        public void EnterToTierraDeLosMuertus()
        {
            dialog.PressStringDialog(2);
            dialog.PressOkButton(1);

            if ((NumberOfRoute() == 0) || (NumberOfRoute() == 1) || (NumberOfRoute() == 5))
            {
                dialog.PressStringDialog(3);     // стартовая точка - около входа
                dialog.PressOkButton(1);
            }
            else
            {
                dialog.PressStringDialog(2);     // стартовая точка - центр карты (для маршрутов 2 и 3)
                dialog.PressOkButton(1);
            }

            dialog.PressStringDialog(1);    //move
            dialog.PressOkButton(1);

        }

        /// <summary>
        /// получить чистый отит (забрать в диалоге у старого мужика)
        /// </summary>
        public void TakePureOtite()
        {
            dialog.PressStringDialog(1);
            dialog.PressOkButton(1);

            dialog.PressStringDialog(1);
            dialog.PressOkButton(3);
        }

        /// <summary>
        /// подходим к старому человеку после перехода из казарм
        /// </summary>
        public void GoToOldMan()
        {
            //while (!town.isOpenMap())
            //{
                server.OpenMapForState();
                Pause(1000);
            //}

            town.PressOldManonMap();
            town.ClickMoveMap();

            botwindow.PressEscThreeTimes();
            Pause(5000);

            town.PressOldMan1();
            Pause(2000);
        }

        /// <summary>
        /// подходим к старому человеку после перехода из казарм
        /// </summary>
        public void GoToOldManBegin()
        {
            server.OpenMapForState();
            Pause(1000);

            town.PressOldManonMap();
            town.ClickMoveMap();

            botwindow.PressEscThreeTimes();
            Pause(4000);

        }

        /// <summary>
        /// тыкаем в старого человека для диалога
        /// </summary>
        public void GoToOldManEnd()
        {
            town.PressOldMan1();
            Pause(2000);
        }


        /// <summary>
        /// переход по карте Земли мертвых к месту начала маршрута для набивания андидов (100 шт.)
        /// </summary>
        public void GotoWork()
        {
            CounterRouteToNull();
            RouteNextPoint().PressMouseL();
            Pause(RouteNextPointTime());
            //pointWorkOnMap.PressMouseL();
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
            counterRouteNode++; if (counterRouteNode > 2) counterRouteNode = 0;
        }

        /// <summary>
        /// обнуляем счетчик, чтобы начать с начала маршрута
        /// </summary>
        public void  CounterRouteToNull()
        {
            counterRouteNode = 0;
        }

        /// <summary>
        /// нажимаем на Мамона
        /// </summary>
        public void PressMamons()
        {
            pointMamons.PressMouseL();
        }

        /// <summary>
        /// поговорить с Мамоном для перехода в Лос Толдос
        /// </summary>
        public void TalkMamons()
        {
            dialog.PressStringDialog(1);
            dialog.PressOkButton(1);
        }

        /// <summary>
        /// приближаем камеру (опускаем максимально вниз)
        /// </summary>
        public void MinHeight()
        {
            for (int j = 1; j <= 10; j++)
            {
                pointMamons.PressMouseWheelDown();
                Pause(1000);
            }
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
