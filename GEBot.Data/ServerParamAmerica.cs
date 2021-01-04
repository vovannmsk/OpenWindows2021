using System.IO;

namespace GEBot.Data
{
    public class ServerParamAmerica : ServerParam
    {
        public ServerParamAmerica()
        {
            this.globalParam = new GlobalParam();
            this.pathClient = path_Client();
            this.isActiveServer = isActive();
        }

        /// <summary>
        /// путь к исполняемому файлу игры (сервер Америка2)
        /// </summary>
        /// <returns></returns>
        protected override string path_Client()
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\America_path.txt"); }

        /// <summary>
        /// считываем параметр, отвечающий за то, надо ли загружать окна на сервере Америка2
        /// </summary>
        /// <returns></returns>
        protected override bool isActive()
        { return bool.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\America_active.txt")); }


    }
}
