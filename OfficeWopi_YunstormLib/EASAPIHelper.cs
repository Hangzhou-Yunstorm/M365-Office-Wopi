using EASFramework.Main.Framework.Model;
using EASFramework.Main.Framework.Service;
using EASFramework.Main.Framework.Service.Impl;
using EASFramework.Main.Framework.Util;
using IO.OpenDocAPI.Api;
using IO.OpenDocAPI.Model;
using Newtonsoft.Json;
using OfficeWopi_YunstormLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace OfficeWopi_YunstormLib
{
    public class EASAPIHelper : IEASAPIHelper
    {
        /// <summary>
        /// Get API Model
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <returns>Model</returns>
        private EASAPIModel GetAPIModel(string access_token)
        {
            EASAPIModel model = new EASAPIModel();
            access_token = System.Web.HttpUtility.UrlDecode(access_token);
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(access_token);
            model.TokenId = dynamicObj["tokenid"].Value;
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
        /// <param name="rev">版本</param>
        /// <returns>文件信息</returns>
        public string SingleFileDownload(string rootPath, string fileId, string rev, string token)
        {
            try
            {
                EASAPIModel model = GetAPIModel(token);

                // 设置传参
                SingleDownloadReq body = new SingleDownloadReq();
                string savePath = $"{rootPath}\\Files\\{fileId}";
                if (!string.IsNullOrEmpty(rev))
                {
                    body.Rev = rev;
                    savePath = $"{savePath}/{rev}";
                }
                body.Docid = model.DocId;
                body.SavePath = savePath;
                body.Usehttps = false;

                // 文件信息
                string resBody = GetFileDownloadResponse(body, model);
                var dynamicObj = JsonConvert.DeserializeObject<dynamic>(resBody);
                string method = dynamicObj["authrequest"][0].Value;
                string url = dynamicObj["authrequest"][1].Value;
                string fileName = dynamicObj["name"].Value;
                string filePath = $"{savePath}\\{fileName}";

                //如果不存在，创建它
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                List<string> headers = new List<string>();
                for (int i = 2; i < dynamicObj["authrequest"].Count; ++i)
                {
                    headers.Add(dynamicObj["authrequest"][i].Value);
                }

                OSSHelper ossHttpHelper = new OSSHelper();
                HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(method, url, headers, null);

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
        /// 获取文件下载信息
        /// </summary>
        /// <returns>文件下载信息</returns>
        private string GetFileDownloadResponse(SingleDownloadReq downloadReq, EASAPIModel model)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/file/osdownloadext");
                request.Method = "Post";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"Bearer {model.TokenId}");

                var param = JsonConvert.SerializeObject(downloadReq);
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
        /// 文件是否有编辑权限
        /// </summary>
        /// <returns>是否有编辑权限</returns>
        public bool IsCanEditFile(string token)
        {
            try
            {
                EASAPIModel model = GetAPIModel(token);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/perm1/check");
                request.Method = "Post";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"Bearer {model.TokenId}");

                var param = "{\"docid\":\"" + model.DocId + "\",\"perm\":16}";
                var bytes = Encoding.UTF8.GetBytes(param);

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int resCode = (int)response.StatusCode;

                // 若为错误返回码则只读
                if (resCode < 200 || resCode >= 300)
                {
                    return false;
                }

                Stream ResStream = response.GetResponseStream();
                string resBody = string.Empty;
                using (StreamReader reader = new StreamReader(ResStream, Encoding.UTF8))
                {
                    resBody = reader.ReadToEnd();
                }
                resBody = System.Web.HttpUtility.UrlDecode(resBody);
                var dynamicObj = JsonConvert.DeserializeObject<dynamic>(resBody);
                return dynamicObj["result"].Value == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        public UserGetRes UserGetPost(string token)
        {
            try
            {
                EASAPIModel model = GetAPIModel(token);

                FileUploadDownloadService fileUploadService = new FileUploadDownloadServiceImpl();
                return fileUploadService.UserGetPost(APIInstanceManager.GetApiInstance(model.TokenId, model.Host));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取文件水印
        /// </summary>
        /// <returns>文件水印信息</returns>
        public string DirCheckwatermarkPost(string token)
        {
            try
            {
                EASAPIModel model = GetAPIModel(token);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{model.Host}/api/v1/dir/checkwatermark");
                request.Method = "Post";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"Bearer {model.TokenId}");

                var param = "{\"docid\":\"" + model.DocId + "\"}";
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
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取文件锁信息
        /// </summary>
        /// <returns>文件锁信息</returns>
        public AutolockGetlockinfoRes AutolockGetlockinfoPost(string token)
        {
            try
            {
                EASAPIModel model = GetAPIModel(token);
                FileUploadDownloadService fileUploadService = new FileUploadDownloadServiceImpl();
                return fileUploadService.AutolockGetlockinfoPost(model.DocId, APIInstanceManager.GetApiInstance(model.TokenId, model.Host));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  上传文件到服务器
        /// </summary>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="isNew">是否新建</param>
        public UploadRes FileSingleUpload(string filePath, bool isNew, string token)
        {
            // 调用接口
            try
            {
                EASAPIModel model = GetAPIModel(token);
                FileUploadDownloadService fileUploadService = new FileUploadDownloadServiceImpl();

                // 设置传参
                SingleUploadReq body = new SingleUploadReq();
                body.Docid = model.DocId.Substring(0, model.DocId.LastIndexOf('/'));
                body.FilePath = filePath;
                body.Ondup = isNew ? 2 : 3;

                return fileUploadService.SingleUpload(body, APIInstanceManager.GetApiInstance(model.TokenId, model.Host));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
