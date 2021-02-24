using EASFramework.Main.Framework.Util;
using Newtonsoft.Json;
using OfficeWopi_YunstormLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OfficeWopi_YunstormLib
{
    public class EASLinkAPIHelper : IEASLinkAPIHelper
    {
        /// <summary>
        /// Get API Model
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <returns>Model</returns>
        private EASLinkAPIModel GetAPIModel(string access_token)
        {
            EASLinkAPIModel model = new EASLinkAPIModel();
            access_token = System.Web.HttpUtility.UrlDecode(access_token);
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(access_token);
            model.Link = dynamicObj["link"]["link"].Value;
            model.Password = dynamicObj["link"]["password"].Value;
            model.DocId = dynamicObj["doc"]["docid"].Value;

            model.Host = dynamicObj["host"].Value;
            var port = dynamicObj["port"].Value;
            if (port > 0)
            {
                model.Host = $"{model.Host}:{port}";
            }
            return model;
        }

        /// <summary>
        /// 获取文件信息(路径)
        /// </summary>
        /// <param name="rootPath">文件根目录</param>
        /// <param name="fileId">文件Id</param>
        /// <returns>文件信息</returns>
        public string SingleFileDownload(string rootPath, string fileId, string token)
        {
            // 调用接口
            try
            {
                string resBody = GetShareFileDownloadResponse(token);
                var dynamicObj = JsonConvert.DeserializeObject<dynamic>(resBody);
                string method = dynamicObj["authrequest"][0].Value;
                string url = dynamicObj["authrequest"][1].Value;
                string fileName = dynamicObj["name"].Value;

                string savePath = $"{rootPath}\\Files\\{fileId}";
                string filePath = $"{savePath}\\{fileName}";

                //如果不存在，创建它
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                OSSHelper ossHttpHelper = new OSSHelper();
                HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(method, url, new List<string>(), null);

                Stream fileContent = ossResult.GetResponseStream();
                const int bufferLen = 50 * 1024 * 1024;

                if (File.Exists(filePath))
                {
                    //写入数据
                    using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
                    {
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        while ((count = fileContent.Read(buffer, 0, bufferLen)) > 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        fs.Dispose();
                    }
                }
                else
                {
                    //写入数据
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        while ((count = fileContent.Read(buffer, 0, bufferLen)) > 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        fs.Dispose();
                    }
                }

                return filePath;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取分享文件下载信息
        /// </summary>
        /// <returns>分享文件下载信息</returns>
        private string GetShareFileDownloadResponse(string token)
        {
            try
            {
                EASLinkAPIModel model = GetAPIModel(token);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/link/osdownloadext");
                request.Method = "Post";
                request.ContentType = "application/json";

                var param = "{\"link\":\"" + model.Link + "\",\"password\":\"" + model.Password + "\",\"docid\":\"" + model.DocId + "\"}";
                var bytes = Encoding.UTF8.GetBytes(param);

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int resCode = (int)response.StatusCode;

                // 若为错误返回码则抛出异常
                if (resCode < 200 || resCode >= 300)
                {
                    throw new Exception();
                }

                Stream ResStream = response.GetResponseStream();
                string resBody = string.Empty;
                using (StreamReader reader = new StreamReader(ResStream, Encoding.UTF8))
                {
                    resBody = reader.ReadToEnd();
                }
                return resBody;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取分享文件水印信息
        /// </summary>
        /// <returns>分享文件水印信息</returns>
        public string GetShareFileCheckWaterMark(string token)
        {
            try
            {
                EASLinkAPIModel model = GetAPIModel(token);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/link/checkwatermark");
                request.Method = "Post";
                request.ContentType = "application/json";

                var param = GetShareFileInfo(token);
                var bytes = Encoding.UTF8.GetBytes(param);

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int resCode = (int)response.StatusCode;

                // 若为错误返回码则抛出异常
                if (resCode < 200 || resCode >= 300)
                {
                    throw new Exception();
                }

                Stream ResStream = response.GetResponseStream();
                string resBody = string.Empty;
                using (StreamReader reader = new StreamReader(ResStream, Encoding.UTF8))
                {
                    resBody = reader.ReadToEnd();
                }
                return resBody;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取分享文件信息
        /// </summary>
        /// <returns>分享文件信息</returns>
        public string GetShareFileInfo(string token)
        {
            try
            {
                EASLinkAPIModel model = GetAPIModel(token);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/link/get");
                request.Method = "Post";
                request.ContentType = "application/json";

                var param = "{\"link\":\"" + model.Link + "\",\"password\":\"" + model.Password + "\",\"docid\":\"" + model.DocId + "\"}";
                var bytes = Encoding.UTF8.GetBytes(param);

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int resCode = (int)response.StatusCode;

                // 若为错误返回码则抛出异常
                if (resCode < 200 || resCode >= 300)
                {
                    throw new Exception();
                }

                Stream ResStream = response.GetResponseStream();
                string resBody = string.Empty;
                using (StreamReader reader = new StreamReader(ResStream, Encoding.UTF8))
                {
                    resBody = reader.ReadToEnd();
                }
                return resBody;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
