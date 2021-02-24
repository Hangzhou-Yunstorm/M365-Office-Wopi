using log4net;
using Newtonsoft.Json;
using OfficeWopi_NetCore31.Models;
using OfficeWopi_YunstormLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace OfficeWopi_NetCore31
{
    public static class FileLockHelper
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private static ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(FileLockHelper));

        /// <summary>
        /// Collection holding information about locks. Should be persistent.
        /// </summary>
        public static Dictionary<string, LockInfo> LockStorage = new Dictionary<string, LockInfo>();

        /// <summary>
        /// Try Get Lock
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="lockInfo">Lock Info</param>
        /// <returns>Lock Info</returns>
        public static bool TryGetLock(string fileId, out LockInfo lockInfo)
        {
            if (LockStorage.TryGetValue(fileId, out lockInfo))
            {
                if (lockInfo.Expired)
                {
                    LockStorage.Remove(fileId);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set Lock
        /// Set Other NLB Lock while rootPath not null
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="lockInfo">Lock Info</param>
        /// <param name="rootPath">Root path</param>
        public static void SetLock(string fileId, string lockInfo, string rootPath)
        {
            if (string.IsNullOrEmpty(lockInfo))
            {
                LockStorage.Remove(fileId);
            }
            else
            {
                LockStorage[fileId] = new LockInfo { DateCreated = DateTime.UtcNow, Lock = lockInfo };

                // Remove Expire Lock
                RemoveExpireLock();
            }

            // Set Other NLB Lock while rootPath not null
            if (!string.IsNullOrEmpty(rootPath))
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    SetOtherNLBLock(fileId, lockInfo, rootPath);
                });
            }
        }

        /// <summary>
        /// Set Other NLB Lock
        /// </summary>
        /// <param name="fileId">File Id</param>
        /// <param name="lockInfo">Lock Info</param>
        /// <param name="rootPath">Root Path</param>
        private static void SetOtherNLBLock(string fileId, string lockInfo, string rootPath)
        {
            var nlbUrls = GetNLBUrls(rootPath);
            if (!string.IsNullOrEmpty(nlbUrls))
            {
                log.Info($"SetOtherNLBLock Start.");

                var nlbUrlList = nlbUrls.Split(",");
                foreach (string url in nlbUrlList)
                {
                    try
                    {
                        string nlbUrl = url;
                        if (nlbUrl.EndsWith("/"))
                        {
                            nlbUrl = nlbUrl.TrimEnd('/');
                        }

                        PostLock postLock = new PostLock()
                        {
                            FileId = fileId,
                            FileLock = lockInfo
                        };
                        HttpContent formdata = new StringContent(JsonConvert.SerializeObject(postLock));

                        HttpClient client = GetHttpClient();
                        var reqResult = client.PostAsync($"{nlbUrl}/NLBLock/SetNLBLock", formdata).GetAwaiter().GetResult();
                        var result = reqResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        log.Info($"SetOtherNLBLock End, url: {url}, Message: {result}");
                    }
                    catch (Exception ex)
                    {
                        log.Info($"SetOtherNLBLock url: {url}, Exception: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// HttpClient
        /// </summary>
        private static HttpClient httpClient;

        /// <summary>
        /// Get HttpClient
        /// </summary>
        /// <returns>HttpClient</returns>
        public static HttpClient GetHttpClient()
        {
            if (httpClient == null)
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
                    ClientCertificateOptions = ClientCertificateOption.Manual
                };
                httpClient = new HttpClient(handler);
            }
            return httpClient;
        }

        /// <summary>
        /// Remove Expire Lock
        /// </summary>
        private static void RemoveExpireLock()
        {
            var expireLocks = LockStorage.Where(T => T.Value.Expired);
            foreach (KeyValuePair<string, LockInfo> lockInfo in expireLocks)
            {
                LockStorage.Remove(lockInfo.Key);
            }
        }

        /// <summary>
        /// Get NLB Urls
        /// </summary>
        /// <param name="rootPath">Root path</param>
        /// <returns>NLB Urls</returns>
        public static string GetNLBUrls(string rootPath)
        {
            try
            {
                var filePath = $"{rootPath}\\Files";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string NLbUrlPath = $"{filePath}\\NLBUrls.txt";
                if (!File.Exists(NLbUrlPath))
                {
                    return string.Empty;
                }

                using (FileStream fs = new FileStream(NLbUrlPath, FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info($"GetNLBUrls Exception: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Set NLB Urls
        /// </summary>
        /// <param name="rootPath">Root path</param>
        /// <param name="urls">NLB Urls</param>
        public static void SetNLBUrls(string rootPath, string urls)
        {
            try
            {
                string NLbUrlPath = $"{rootPath}\\Files\\NLBUrls.txt";
                using (FileStream fs = new FileStream(NLbUrlPath, FileMode.OpenOrCreate))
                {
                    fs.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.Write(urls);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info($"SetNLBUrls Exception: {ex.Message}");
            }
        }
    }
}
