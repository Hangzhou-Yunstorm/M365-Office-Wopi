using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeWopi_NetCore31.Models;
using OfficeWopi_YunstormLib;
using System;
using System.IO;
using System.Text;

namespace OfficeWopi_NetCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NLBLockController : ControllerBase
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(NLBLockController));

        /// <summary>
        /// Set NLB Lock Info（Post）
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost("SetNLBLock")]
        public ActionResult<string> SetNLBLock()
        {
            try
            {
                string postString = string.Empty;
                using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    postString = sr.ReadToEndAsync().Result;
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    var postLock = JsonConvert.DeserializeObject<PostLock>(postString);

                    FileLockHelper.SetLock(postLock.FileId, postLock.FileLock, null);
                    return "Set Success.";
                }
                else
                {
                    log.Info($"SetNLBLock Exception: Post Data Is Null.");
                    return "Post Data Is Null.";
                }
            }
            catch (Exception ex)
            {
                log.Info($"SetNLBLock Exception: {ex.Message}");
                return ex.Message;
            }
        }

        /// <summary>
        /// Get NLB Point
        /// </summary>
        /// <returns>Result</returns>
        [HttpGet("GetNLBPoint")]
        public ActionResult<string> GetNLBPoint()
        {
            return "NLB Point";
        }

    }
}