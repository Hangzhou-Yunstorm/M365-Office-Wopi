using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeWopi_NetCore31.Models;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace OfficeWopi_NetCore31.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// IConfiguration
        /// </summary>
        public readonly IConfiguration _configuration;

        /// <summary>
        /// Struct Method
        /// </summary>
        /// <param name="hostingEnvironment">IWebHostEnvironment</param>
        /// <param name="lockInfoHelper">ILockInfoHelper</param>
        public HomeController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns>Index Page</returns>
        public IActionResult Index()
        {
            string nlbUrls = FileLockHelper.GetNLBUrls(_hostingEnvironment.ContentRootPath);
            ViewData["NLBMsg"] = string.IsNullOrEmpty(nlbUrls) ? "未配置" : "已配置";
            return View();
        }

        /// <summary>
        /// 写入其他NLB网址
        /// </summary>
        /// <param name="url">其他NLB网址</param>
        /// <returns>写入结果</returns>
        [HttpPost]
        public IActionResult WriteNLBUrls(string urls)
        {
            log.Info($"WriteNLBUrls：{urls}");
            try
            {
                if (string.IsNullOrEmpty(urls))
                {
                    return Json(new JsonModel() { Success = false, Message = "请输入NLB 其他网址！" });
                }

                var urlList = urls.Split(",");
                string errorMsg = string.Empty;
                foreach (string url in urlList)
                {
                    try
                    {
                        string nlbUrl = url;
                        if (nlbUrl.EndsWith("/"))
                        {
                            nlbUrl = nlbUrl.TrimEnd('/');
                        }

                        HttpClient webRequest = FileLockHelper.GetHttpClient();
                        var result = webRequest.GetStringAsync($"{nlbUrl}/NLBLock/GetNLBPoint").GetAwaiter().GetResult();
                    }
                    catch
                    {
                        errorMsg += url + ",";
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new JsonModel() { Success = false, Message = $"网址: {errorMsg} 无法访问, 请重新输入！" });
                }

                FileLockHelper.SetNLBUrls(_hostingEnvironment.ContentRootPath, urls);
                log.Info($"WriteNLBUrls Set NLBUrls：{urls}");
                return Json(new JsonModel() { Success = true, Message = "修改成功！" });
            }
            catch (Exception ex)
            {
                return Json(new JsonModel() { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Error Page
        /// </summary>
        /// <returns>Error Page</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
