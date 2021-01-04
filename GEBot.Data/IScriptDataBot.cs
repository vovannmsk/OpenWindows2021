using System;

namespace GEBot.Data
{
    public interface IScriptDataBot
    {
        /// <summary>
        /// выдача данных бота с номером окна numberOfWindow
        /// </summary>
        /// <param name="numberOfWindow"> номер окна бота</param>
        /// <returns>данные, необходимые для создания бота в формате DataBot </returns>
        BotParam GetDataBot();

        /// <summary>
        /// запись hwnd в текстовый файл или в БД
        /// </summary>
        /// <param name="hwnd"></param>
        void SetHwnd(UIntPtr hwnd);

    }
}
