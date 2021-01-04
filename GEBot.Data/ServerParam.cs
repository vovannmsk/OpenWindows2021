namespace GEBot.Data
{

    /// <summary>
    /// Параметры ботов, зависящие от сервера (сингапур, америка или европа)
    /// </summary>
    public abstract class ServerParam
    {
        protected GlobalParam globalParam;

        #region параметры, зависящие от сервера

        protected string pathClient;    //путь к клиенту игры
        protected bool isActiveServer;  //активен ли данный сервер сейчас или профилактика 

        public string PathClient { get => pathClient; }
        public bool IsActiveServer { get => isActiveServer;  }

        #endregion

        //===================== методы ===============================

        protected abstract string path_Client();
        protected abstract bool isActive();




    }
}
