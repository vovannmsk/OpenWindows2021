using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenGEWindows
{
    public abstract class Server2

    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(UIntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern UIntPtr FindWindowEx(UIntPtr hwndParent, UIntPtr hwndChildAfter, string className, string windowName);



        #region общие

        protected botWindow botwindow;
        protected int xx;
        protected int yy;

        #endregion

        #region общие 2

        protected const String KATALOG_MY_PROGRAM = "C:\\!! Суперпрограмма V&K\\";
        protected Town town;
        protected Town town_begin;
        protected TownFactory townFactory;
        protected String pathClient;

        #endregion


        // ===========================================  Методы ==========================================

        #region общие методы

        /// <summary>
        /// Останавливает поток на некоторый период (пауза)
        /// </summary>
        /// <param name="ms"> ms - период в милисекундах </param>
        protected void Pause(int ms)
        {
            Thread.Sleep(ms);
        }

        #endregion

        #region Getters

        /// <summary>
        /// геттер
        /// </summary>
        /// <returns></returns>
        public Town getTown()
        { return this.town; }

        #endregion

    }
}
