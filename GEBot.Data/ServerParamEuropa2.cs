using System.IO;

namespace GEBot.Data
{
    public class ServerParamEuropa2 : ServerParam
    {
        public ServerParamEuropa2()
        {
            this.globalParam = new GlobalParam();
            this.pathClient = path_Client();
            this.isActiveServer = isActive();
        }

        /// <summary>
        /// путь к исполняемому файлу игры (сервер Europa)
        /// </summary>
        /// <returns></returns>
        protected override string path_Client()
        { return File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Europa2_path.txt"); }

        /// <summary>
        /// считываем параметр, отвечающий за то, надо ли загружать окна на сервере Europa
        /// </summary>
        /// <returns></returns>
        protected override bool isActive()
        { return bool.Parse(File.ReadAllText(globalParam.DirectoryOfMyProgram + "\\Europa2_active.txt")); }

    }
}
