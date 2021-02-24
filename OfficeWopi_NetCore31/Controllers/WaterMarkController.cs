using log4net;
using Microsoft.AspNetCore.Mvc;
using OfficeWopi_NetCore31.Models;
using OfficeWopi_YunstormLib;
using System;

namespace OfficeWopi_NetCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaterMarkController : ControllerBase
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(WaterMarkController));

        /// <summary>
        /// Get WaterMark Info（Post）
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        [HttpPost("GetWaterMark")]
        public ActionResult<JsonModel> GetWaterMark([FromForm]string words)
        {
            try
            {
                log.Info($"GetWaterMark Start, access_token: {words}");

                IWaterMarkHelper waterMarkHelper = new WaterMarkHelper();
                // Get Base64 Png
                string imgString = waterMarkHelper.GetBase64Png(words, out bool repeat);

                log.Info($"GetWaterMark End, repeat:{repeat}");
                return new JsonModel() { Success = true, Data = imgString, Repeat = repeat };
            }
            catch (Exception ex)
            {
                log.Info($"GetWaterMark Exception, Message:{ex.Message}");
                return new JsonModel() { Success = false, Message = ex.Message };
            }
        }
    }
}