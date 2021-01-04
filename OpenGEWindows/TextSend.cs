using System;
using System.Threading;

namespace OpenGEWindows
{
    class TextSend
    {
        /// <summary>
        /// эмулирует нажатие клавиатуры с заданным сканкодом
        /// </summary>
        /// <param name="scanCode"> сканкод клавиши (по старой системе IBM XT) </param>
        public static void SendText2(ushort scanCode)
        {
            KeyDown(scanCode);
            Thread.Sleep(100);
            KeyUp(scanCode);
        }

        /// <summary>
        /// эмулирует нажатие клавиши
        /// </summary>
        /// <param name="scanCode"> сканкод клавиши (по старой системе IBM XT) </param>
        public static void KeyDown(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_SCANCODE;
            inputs[0].ki.wScan = (ushort)(scanCode & 0xff);

            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        /// <summary>
        /// эмулирует отжатие клавиши
        /// </summary>
        /// <param name="scanCode"> сканкод клавиши (по старой системе IBM XT) </param>
        public static void KeyUp(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.wScan = scanCode;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_SCANCODE | WindowsAPI.KEYEVENTF_KEYUP;
            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }
 
    }
}
