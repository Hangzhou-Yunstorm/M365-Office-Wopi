using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASFramework.Main.Framework.Model
{
    class OSSException : Exception
    {
        private int errCode;
        private String errBody;
        private String errHeaders;

        public OSSException(int errCode, String errBody, String errHeaders)
        {
            this.errCode = errCode;
            this.errBody = errBody;
            this.errHeaders = errHeaders;
        }

        public int ErrCode
        {
            get
            {
                return errCode;
            }

            set
            {
                errCode = value;
            }
        }

        public string ErrBody
        {
            get
            {
                return errBody;
            }

            set
            {
                errBody = value;
            }
        }

        public string ErrHeaders
        {
            get
            {
                return errHeaders;
            }

            set
            {
                errHeaders = value;
            }
        }

    }
}
