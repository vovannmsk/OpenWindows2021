using System.IO;

namespace GEBot.Data
{
    public class ServerParamSing : ServerParam
    {

        public ServerParamSing()
        {
            this.globalParam = new GlobalParam();
            this.pathClient = path_Client();
            this.isActiveServer = isActive();
        }

        /// <summary>
        /// путь к исполняемому файлу игры (сервер Sing)
        /// </summary>
        /// <returns></returns>
        protected override string path_Client()
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Singapoore_path.txt"); }

        /// <summary>
        /// считываем параметр, отвечающий за то, надо ли загружать окна на сервере Sing
        /// </summary>
        /// <returns></returns>
        protected override bool isActive()
        { return bool.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Singapoore_active.txt")); }


    }
}
