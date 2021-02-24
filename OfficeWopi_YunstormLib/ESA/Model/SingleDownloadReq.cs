using IO.OpenDocAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASFramework.Main.Framework.Model
{
    class SingleDownloadReq : FileOsdownloadReq
    {
        String savePath;

        public string SavePath
        {
            get
            {
                return savePath;
            }

            set
            {
                savePath = value;
            }
        }
    }
}
