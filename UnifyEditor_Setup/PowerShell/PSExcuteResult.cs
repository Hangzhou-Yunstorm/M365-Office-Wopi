using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifyEditor_Setup.CloudAPI
{
    public class PSExcuteResult
    {
        /// <summary>
        /// 脚本执行结果状态，OK or ERROR
        /// </summary>
        public string STATE { get; set; }

        /// <summary>
        /// 脚本执行结果
        /// </summary>
        public string DATA { get; set; }
    }
}
