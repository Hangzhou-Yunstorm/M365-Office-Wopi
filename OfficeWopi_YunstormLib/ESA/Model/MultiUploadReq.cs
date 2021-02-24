using IO.OpenDocAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASFramework.Main.Framework.Model
{
    class MultiUploadReq : FileOsinitmultiuploadReq
    {
        String filePath;

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }
    }
}
