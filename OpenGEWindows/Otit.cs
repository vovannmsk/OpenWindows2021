using System.IO;


namespace OpenGEWindows
{
    public abstract class Otit : Server2
    {

        protected iPointColor pointOldMan1;
        protected iPointColor pointOldMan2;
        protected iPointColor pointTask1;
        protected iPointColor pointTask2;
        protected iPointColor pointGetTask1;
        protected iPointColor pointGetTask2;
        protected Dialog dialog;
        protected Server server;
        protected iPointColor pointIsOpenMap1;
        protected iPointColor pointIsOpenMap2;
        protected iPointColor pointIsNearOldMan;
        protected iPointColor pointIsPerfume1;
        protected iPointColor pointIsPerfume2;

        /// <summary>
        /// номер следующей точки маршрута
        /// </summary>
        private int counterRouteNode;

        /// <summary>
        /// голова Мамуна
        /// </summary>
        protected iPoint pointMamons;
        /// <summary>
        /// голова Старого мужика
        /// </summary>
        protected iPoint pointOldMan;
        /// <summary>
        /// строка на карте Alt+Z со старым мужиком (OldMan)
        /// </summary>
        protected iPoint pointOldManOnMap;
        /// <summary>
        /// кнопка "Move" на карте Alt+Z
        /// </summary>
        protected iPoint pointButtonMoveOnMap;

        protected int numberOfRoute;

        /// <summary>
        /// номер следующей точки маршрута
        /// </summary>
        protected int CounterRouteNode {
                                            get { counterRouteNode = CounterFromFile(); return counterRouteNode; }
                                            set { counterRouteNode = value; CounterToFile(counterRouteNode); }
                                        }

        // ============  методы  ========================

        /// <summary>
        /// проверяем, есть ли доступ в Лос Толдос. Применён ли парфюм в данный момент?
        /// </summary>
        /// <returns></returns>
        public bool isPerfume()
        {
            return pointIsPerfume1.isColor() && pointIsPerfume2.isColor();
        }

        /// <summary>
        /// применяем парфюм (открываем кэш инвентарь, ищем парфюм,применяем его, подтверждаем)
        /// </summary>
        public void PerfumeOn()
        {

        }

        /// <summary>
        /// открыта ли карта Alt+Z
        /// </summary>
        /// <returns></returns>
        public bool isOpenMap()
        {
            return pointIsOpenMap1.isColor() && pointIsOpenMap2.isColor();

        }

        /// <summary>
        /// стоим ли мы около ОлдМэна (проверяем красный ободок на миникарте)
        /// </summary>
        /// <returns>true, если стоим</returns>
        public bool isNearOldMan()
        {
            return pointIsNearOldMan.isColor();
        }

        /// <summary>
        /// чтение значения счётчика counterRouteNode из файла
        /// </summary>
        /// <returns>номер текущей точки маршрута</returns>
        protected int CounterFromFile()
        { return int.Parse(File.ReadAllText(KATALOG_MY_PROGRAM + botwindow.getNumberWindow() + "\\Точка маршрута.txt")); }

        /// <summary>
        /// метод записывает в файл номер маршрута для фарма чистого отита
        /// </summary>
        protected void CounterToFile(int number)
        {
            File.WriteAllText(KATALOG_MY_PROGRAM + botwindow.getNumberWindow() + "\\Точка маршрута.txt", number.ToString());
        }

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
        public abstract void GetTask();

        /// <summary>
        /// проверяем, выполнено ли задание (проверяется по тому, что строчка с заданием посерела )
        /// будет работать только на этом компе. на других компах нужно переписать с этого компа клиент игры
        /// </summary>
        /// <returns></returns>
        public bool isTaskDone()
        {
            return (pointTask1.isColor() && pointTask2.isColor());
        }

        /// <summary>
        /// проверяем, получено ли задание (проверяется по тому, что строчка с заданием белая )
        /// </summary>
        /// <returns></returns>
        public bool isGetTask()
        {
            return (pointGetTask1.isColor() && pointGetTask2.isColor());
        }

        /// <summary>
        /// Диалог OldMan. Войти в Земли Мертвых через старого мужика
        /// </summary>
        public abstract void EnterToTierraDeLosMuertus();

        public abstract void TakePureOtite();

        /// <summary>
        /// нажимаем на кнопку "Move" в открытой карте Alt+Z             
        /// </summary>
        public void ClickMoveMap()
        {
            pointButtonMoveOnMap.DoubleClickL();
        }

        /// <summary>
        /// подходим к старому человеку после перехода из казарм и нажимаем на него для диалога
        /// </summary>
        public void GoToOldMan()
        {
            GoToOldManBegin();

            PressOldMan();
        }

        /// <summary>
        /// тыкаем на открытой карте в строчку со старым мужиком
        /// </summary>
        public void PressOldManonMap()
        {
            pointOldManOnMap.DoubleClickL();
        }

        /// <summary>
        /// подходим к старому человеку после перехода из казарм
        /// </summary>
        public void GoToOldManBegin()
        {
            //server.OpenMapForState();
            server.TopMenu(6, 2, true);       //12-02-2021
            Pause(1000);

            PressOldManonMap();
            ClickMoveMap();

            botwindow.PressEscThreeTimes();
            Pause(4000);

        }

        /// <summary>
        /// подходим к старому человеку после перехода из казарм
        /// </summary>
        public void GoToOldManMulti()
        {
            //server.OpenMapForState();
            server.TopMenu(6, 2, true);       //12-02-2021
            Pause(1000);

            PressOldManonMap();
            ClickMoveMap();

            botwindow.PressEscThreeTimes();
        }

        /// <summary>
        /// тыкаем на старого мужика
        /// </summary>
        public void PressOldMan()
        {
            botwindow.FirstHero();
            Pause(1000);
            pointOldMan.PressMouseLL();
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
        public void GotoWorkMulti()
        {
            CounterRouteToNull();
            RouteNextPointMulti(CounterRouteNode).PressMouseL();
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
            CounterRouteNode++; if (CounterRouteNode > 2) CounterRouteNode = 0;
        }

        /// <summary>
        /// переход по карте Земли мертвых к следующей точке маршрута с атакой
        /// </summary>
        public void GotoNextPointRouteMulti()
        {
            CounterRouteNode++; if (CounterRouteNode > 2) CounterRouteNode = 0;
            RouteNextPointMulti(CounterRouteNode).PressMouseRR();
        }

        /// <summary>
        /// обнуляем счетчик, чтобы начать с начала маршрута
        /// </summary>
        public void  CounterRouteToNull()
        {
            CounterRouteNode = 0;
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
            for (int j = 1; j <= 8; j++)
            {
                pointMamons.PressMouseWheelDown();
                Pause(400);
            }
        }

        /// <summary>
        /// получаем следующий пункт маршрута
        /// </summary>
        /// <returns>следующая точка</returns>
        public abstract iPoint RouteNextPoint();

        /// <summary>
        /// получаем следующий пункт маршрута
        /// </summary>
        /// <returns>следующая точка</returns>
        public abstract iPoint RouteNextPointMulti(int counter);

        /// <summary>
        /// получаем время для прохождения следующего участка маршрута
        /// </summary>
        /// <returns>время в мс</returns>
        public abstract int RouteNextPointTime();

        /// <summary>
        /// нажимаем на указанную ячейку панели в левом верхнем углу
        /// </summary>
        /// <param name="NumberOfBox">номер ячейки</param>
        public void PressBoxInLeftPanel(int NumberOfBox)
        {
            new Point(31 - 5 + xx, 114 - 5 + (NumberOfBox - 1) * 32 + yy).PressMouseL();
        }

        /// <summary>
        /// Нажать на ячейки в левой панели
        /// </summary>
        public void PressMitridat()
        {
            PressBoxInLeftPanel(1);
            PressBoxInLeftPanel(2);
            PressBoxInLeftPanel(3);
            PressBoxInLeftPanel(4);
        }

    }

}
