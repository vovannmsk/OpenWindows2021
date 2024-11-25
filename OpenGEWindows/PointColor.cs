using System;
using System.Runtime.InteropServices;


namespace OpenGEWindows
{
    public class PointColor : iPointColor
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        private int x;
        private int y;
        private uint color;
        private int accuracy;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public uint Color { get => color; set => color = value; }
        public int Accuracy { get => accuracy; set => accuracy = value; }

        /// <summary>
        /// конструктор без арументов
        /// </summary>
        public PointColor()
        { 
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="x"> координата Х </param>
        /// <param name="y"> координата Y </param>
        /// <param name="color"> эталонный цвет пикселя </param>
        /// <param name="accuracy"> точность округления цвета </param>
        public PointColor(int x, int y,uint color, int accuracy)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Accuracy = accuracy;
        }

        /// <summary>
        /// округление вверх числа a на количество разрядов b
        /// если a = 1655, b = 2, то результат равен 1600
        /// </summary>
        /// <param name="a"> округляемое число </param>
        /// <param name="b"> количество разрядов для округления </param>
        /// <returns> если a = 1655, b = 2, то результат равен 1600 </returns>
        private uint Okruglenie(uint a, int b)
        {
            uint bb = 1;
            //for (int j = 1; j <= b; j++) bb = bb * 10;

            switch (b)
            {
                case 0:
                    bb = 1;
                    break;
                case 1:
                    bb = 10;
                    break;
                case 2:
                    bb = 100;
                    break;
                case 3:
                    bb = 1000;
                    break;
                case 4:
                    bb = 10000;
                    break;
                case 5:
                    bb = 100000;
                    break;
                case 6:
                    bb = 1000000;
                    break;
                case 7:
                    bb = 10000000;
                    break;
            }

            return a - a % bb;
        }

        /// <summary>
        /// возвращает цвет пикселя экрана в координатах (X,Y), /координаты не конкретного окна с игрой, а общего экрана 1920х1080/
        /// </summary>
        /// <returns> цвет пикселя экрана </returns>
        public uint GetPixelColor()
        {
            IntPtr hwnd = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hwnd, X, Y);
            ReleaseDC(IntPtr.Zero, hwnd);
            return pixel;
        }

        /// <summary>
        /// возвращает цвет пикселя экрана в координатах (X + deltaX, Y + deltaY)
        /// </summary>
        /// <param name="deltaX">смещение относительно начальной точки по оси Х</param>
        /// <param name="deltaY">смещение относительно начальной точки по оси Y</param>
        /// <returns>цвет пикселя</returns>
        public uint GetPixelColor(int deltaX, int deltaY)
        {
            IntPtr hwnd = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hwnd, X + deltaX, Y + deltaY);
            ReleaseDC(IntPtr.Zero, hwnd);
            return pixel;
        }

        /// <summary>
        /// проверяет цвет в указанных координатах и сверяет с эталонным (с переменной класса color)
        /// </summary>
        /// <returns> true, если цвет совпадает с указанной точностью </returns>
        public bool isColor()
        {
            uint currentColor = Okruglenie(GetPixelColor(), Accuracy);
            uint myColor = Okruglenie(Color, Accuracy);
            return (currentColor == myColor);
            //return (currentColor == Color);
        }

        /// <summary>
        /// проверяет цвет в указанных координатах и сверяет с эталонным (с переменной класса color)
        /// </summary>
        /// <returns> true, если цвет совпадает с указанной точностью </returns>
        public bool isColor(int deltaX, int deltaY)
        {
            uint currentColor = Okruglenie(GetPixelColor(deltaX, deltaY), Accuracy);
            uint myColor = Okruglenie(Color, Accuracy);
            return (currentColor == myColor);
        }

        /// <summary>
        /// проверяет цвет в указанных координатах и сверяет с эталонным (с переменной класса color)
        /// </summary>
        /// <returns> true, если цвет совпадает с указанной точностью </returns>
        public bool isColor2()
        {
            return (GetPixelColor() > Color);
        }




    }
}
