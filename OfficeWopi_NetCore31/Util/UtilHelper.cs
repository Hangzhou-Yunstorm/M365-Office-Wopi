using Hangfire;
using log4net;
using System;
using System.IO;
using System.Linq;

namespace OfficeWopi_NetCore31
{
    public class UtilHelper
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private static ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(UtilHelper));

        /// <summary>
        /// Start Hanfire Work
        /// </summary>
        public static void StartHanfireWork(string rootPath)
        {
            try
            {
                TimeZoneInfo timeZoneInfo = GetTimeZoneInfo();
                RecurringJob.AddOrUpdate(() => DeleteCacheLogs(rootPath), Cron.Daily(0), timeZoneInfo);
                RecurringJob.AddOrUpdate(() => DeleteCacheFiles(rootPath), Cron.Daily(1), timeZoneInfo);
            }
            catch (Exception ex)
            {
                log.Info($"StartHanfireWork Exception：{ex.Message}");
            }
        }

        /// <summary>
        ///  获取时区（东八区）
        /// </summary>
        /// <returns></returns>
        private static TimeZoneInfo GetTimeZoneInfo()
        {
            TimeZoneInfo timeZoneInfo = null;
            try
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            }
            catch
            {
                timeZoneInfo = TimeZoneInfo.Local;
            }
            return timeZoneInfo;
        }

        /// <summary>
        /// Delete Cache Files
        /// </summary>
        /// <param name="rootPath">Root path</param>
        public static void DeleteCacheFiles(string rootPath)
        {
            try
            {
                string path = $"{rootPath}/Files";
                log.Info($"DeleteCacheFiles path: {path}");

                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dii = dir.GetDirectories();
                foreach (DirectoryInfo d in dii)
                {
                    try
                    {
                        FileInfo subFile = d.GetFiles().OrderByDescending(T => T.LastWriteTime).FirstOrDefault();
                        if (subFile != null && subFile.LastWriteTime.AddDays(1) < DateTime.Now)
                        {
                            Directory.Delete(d.FullName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info($"DeleteCacheFiles Excepion1: {ex.Message}");
                    }
                }
                log.Info($"DeleteCacheFiles End");
            }
            catch (Exception ex)
            {
                log.Info($"DeleteCacheFiles Excepion2: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete Cache Logs
        /// </summary>
        /// <param name="rootPath">Root path</param>
        public static void DeleteCacheLogs(string rootPath)
        {
            try
            {
                string path = $"{rootPath}/Log";
                log.Info($"DeleteCacheLogs path: {path}");

                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    try
                    {
                        if (file.LastWriteTime.AddDays(7) < DateTime.Now)
                        {
                            file.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info($"DeleteCacheLogs Excepion1: {ex.Message}");
                    }
                }
                log.Info($"DeleteCacheLogs End");
            }
            catch (Exception ex)
            {
                log.Info($"DeleteCacheLogs Excepion2: {ex.Message}");
            }
        }
    }
}
