using System.Threading;

namespace OpenGEWindows
{
    public class Point : iPoint
    {
        private int x;
        private int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Point()
        { 
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="x"> координата Х </param>
        /// <param name="y"> координата Y </param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int getX()
        {
            return this.X;
        }
        public int getY()
        {
            return this.Y;
        }


        /// <summary>
        /// Останавливает поток на некоторый период
        /// </summary>
        /// <param name="ms"> ms - период в милисекундах </param>
        public void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        /// <summary>
        /// нажать мышью в конкретную точку только правой кнопкой
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouseR()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 2);
            Pause(200);
        }

        /// <summary>
        /// нажать мышью в конкретную точку только левой кнопкой
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouseL()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 1);
            Pause(200);
        }
         
        /// <summary>
        /// два последовательных нажатия левой кнопкой мыши через паузу
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouseLL()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 1);
            Pause(500);
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 1);
            Pause(200);
        }

        /// <summary>
        /// нажать мышью в конкретную точку
        /// дважды будет нажиматься правая кнопка и однажды левая, также к координатам будет прибавляться смещение окна от края монитора getX и getY
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouse()
        {
            PressMouseR();
            PressMouseR();
            PressMouseL();
        }

        /// <summary>
        /// переместить мышь в координаты и покрутить колесо вверх
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouseWheelUp()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 9);
            Pause(400);   //было 200. сделал для Лаврово
        }

        /// <summary>
        /// переместить мышь в координаты и покрутить колесо вниз
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void PressMouseWheelDown()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 3);
            //Pause(200);
        }

        /// <summary>
        /// двойной клик в указанных координатах
        /// </summary>
        public void DoubleClickL()
        {
            //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(x, y, 1);
            //Pause(50);
            //Click_Mouse_and_Keyboard.Mouse_Move_and_Click(x, y, 1);
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 6);
            Pause(200);

        }

        /// <summary>
        /// перетаскивание из текущей точки в указанную
        /// </summary>
        /// <param name="point"></param>
        public void Drag(iPoint point)
        {
            Click_Mouse_and_Keyboard.MMC(X, Y, point.getX(), point.getY());
            Pause(200);
        }

        /// <summary>
        /// нажимаем правую кнопку мыши в указанных координатах и перемещаем мышь в другие координаты, а там отпускаем правую кнопку
        /// </summary>
        /// <param name="point"></param>
        public void Turn(iPoint point)
        {
            Click_Mouse_and_Keyboard.MMCR(X, Y, point.getX(), point.getY());
            Pause(200);
        }

        /// <summary>
        /// перемещение мыши в указанные координаты
        /// </summary>
        public void Move ()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 5);
            Pause(200);
        }

        /// <summary>
        /// нажать мышью в конкретную точку только левой кнопкой
        /// </summary>
        /// <param name="x"> x - первая координата точки, куда нужно ткнуть мышью </param>
        /// <param name="y"> y - вторая координата точки, куда нужно ткнуть мышью </param>
        public void FastPressMouseL()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 7);
        }

        /// <summary>
        /// перемещение мыши в указанные координаты
        /// </summary>
        public void FastMove()
        {
            Click_Mouse_and_Keyboard.Mouse_Move_and_Click(X, Y, 8);
//            Pause(200);
        }

    }
}
