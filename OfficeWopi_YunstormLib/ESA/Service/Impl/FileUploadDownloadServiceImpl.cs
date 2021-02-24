using EASFramework.Main.Framework.Model;
using EASFramework.Main.Framework.Util;
using IO.OpenDocAPI.Api;
using IO.OpenDocAPI.Model;
using OfficeWopi_YunstormLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EASFramework.Main.Framework.Service.Impl
{
    class FileUploadDownloadServiceImpl : FileUploadDownloadService
    {
        private OSSHelper ossHttpHelper;

        public FileUploadDownloadServiceImpl( )
        {
            ossHttpHelper = new OSSHelper();
        }


        public UserGetRes UserGetPost(DefaultApi apiInstance)
        {
            return apiInstance.UserGetPost();
        }

        public DirCheckwatermarkRes DirCheckwatermarkPost(string docId, DefaultApi apiInstance)
        {
            DirCheckwatermarkReq req = new DirCheckwatermarkReq()
            {
                Docid = docId
            };
            return apiInstance.DirCheckwatermarkPost(req);
        }

        public AutolockGetlockinfoRes AutolockGetlockinfoPost(string docId, DefaultApi apiInstance)
        {
            AutolockGetlockinfoReq lockReq = new AutolockGetlockinfoReq()
            {
                Docid = docId
            };
            return apiInstance.AutolockGetlockinfoPost(lockReq);
        }

        public void SingleDownload(SingleDownloadReq downloadReq, DefaultApi apiInstance)
        {
            // 调用 osdownload API
            FileOsdownloadReq osdownloadBody = new FileOsdownloadReq();
            osdownloadBody = downloadReq;
            FileOsdownloadRes osdownloadResult = apiInstance.FileOsdownloadPost(osdownloadBody);
            Console.WriteLine(osdownloadResult);

            // 根据服务器返回的对象存储请求，向对象存储下载数据
            String filePath = downloadReq.SavePath + "\\" + osdownloadResult.Name;

            if (!Directory.Exists(downloadReq.SavePath))
            {
                //如果不存在，创建它
                Directory.CreateDirectory(downloadReq.SavePath);
            }

            List<String> headers = new List<String>();
            List<String> authRequestList = osdownloadResult.Authrequest;
            for (int i = 2; i < authRequestList.Count; ++i)
            {
                String header = authRequestList[i];
                headers.Add(header);
            }
            HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(authRequestList[0], authRequestList[1], headers, null);

            //写入数据
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {

                Stream fileContent = ossResult.GetResponseStream();
                const int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int count = 0;
                while ((count = fileContent.Read(buffer, 0, bufferLen)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }
            }
        }

        public string SingleFileDownload(SingleDownloadReq downloadReq, DefaultApi apiInstance)
        {
            // 调用 osdownload API
            FileOsdownloadReq osdownloadBody = new FileOsdownloadReq();
            osdownloadBody = downloadReq;
            FileOsdownloadRes osdownloadResult = apiInstance.FileOsdownloadPost(osdownloadBody);
            string filePath = $"{downloadReq.SavePath}\\{osdownloadResult.Name}";

            if (!Directory.Exists(downloadReq.SavePath))
            {
                //如果不存在，创建它
                Directory.CreateDirectory(downloadReq.SavePath);
            }

            List<String> headers = new List<String>();
            List<String> authRequestList = osdownloadResult.Authrequest;
            for (int i = 2; i < authRequestList.Count; ++i)
            {
                String header = authRequestList[i];
                headers.Add(header);
            }
            HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(authRequestList[0], authRequestList[1], headers, null);

            //写入数据
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {

                Stream fileContent = ossResult.GetResponseStream();
                const int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int count = 0;
                while ((count = fileContent.Read(buffer, 0, bufferLen)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }
            }
            return filePath;
        }

        public UploadRes SingleUpload(SingleUploadReq uploadReq, DefaultApi apiInstance)
        {
            // 调用 osbeginupload API
            FileInfo fi = new FileInfo(uploadReq.FilePath);
            uploadReq.Length = fi.Length;
            uploadReq.Name = fi.Name;
            FileOsbeginuploadReq osbeginuploadBody = new FileOsbeginuploadReq();
            osbeginuploadBody = uploadReq;

            FileOsbeginuploadRes osbeginuploadResult = apiInstance.FileOsbeginuploadPost(osbeginuploadBody);

            // 根据服务器返回的对象存储请求，向对象存储上传数据
            byte[] body = CommonUtil.FileToBytes(uploadReq.FilePath);
            List<String> headers = new List<String>();
            List<String> authRequestList = osbeginuploadResult.Authrequest;
            for (int i = 2; i < authRequestList.Count; ++i)
            {
                String header = authRequestList[i];
                headers.Add(header);
            }
            var res = ossHttpHelper.SendReqToOSS(authRequestList[0], authRequestList[1], headers, body);

            // 调用osendupload API
            FileOsenduploadReq osenduploadBody = new FileOsenduploadReq();
            osenduploadBody.Docid = osbeginuploadResult.Docid;
            osenduploadBody.Rev = osbeginuploadResult.Rev;
            FileOsenduploadRes osenduploadResult = apiInstance.FileOsenduploadPost(osenduploadBody);

            UploadRes uploadRes = new UploadRes();
            uploadRes.DocId = osbeginuploadResult.Docid;
            uploadRes.Rev = osbeginuploadResult.Rev;
            return uploadRes;
        }

    }
}
