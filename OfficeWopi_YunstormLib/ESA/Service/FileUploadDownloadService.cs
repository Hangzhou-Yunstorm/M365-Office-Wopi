using EASFramework.Main.Framework.Model;
using IO.OpenDocAPI.Api;
using IO.OpenDocAPI.Model;
using OfficeWopi_YunstormLib.Models;

namespace EASFramework.Main.Framework.Service
{
    interface FileUploadDownloadService
    {
        /// <summary>
        /// 文件单次上传
        /// </summary>
        /// <param name="uploadBigDataReq">上传参数</param>
        UploadRes SingleUpload(SingleUploadReq uploadReq, DefaultApi apiInstance);

        /// <summary>
        /// 单文件下载
        /// </summary>
        /// <param name="uploadBigDataReq">上传参数</param>
        void SingleDownload(SingleDownloadReq downloadReq,DefaultApi apiInstance);

        /// <summary>
        /// 单文件下载
        /// </summary>
        /// <param name="uploadBigDataReq">上传参数</param>
        string SingleFileDownload(SingleDownloadReq downloadReq,DefaultApi apiInstance);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        UserGetRes UserGetPost(DefaultApi apiInstance);

        /// <summary>
        /// 获取文件锁信息
        /// </summary>
        /// <param name="docId">文件ID</param>
        /// <returns>文件锁信息</returns>
        AutolockGetlockinfoRes AutolockGetlockinfoPost(string docId,DefaultApi apiInstance);

        /// <summary>
        /// 检查文件夹水印信息
        /// </summary>
        /// <param name="docId">文件ID</param>
        /// <returns></returns>
        DirCheckwatermarkRes DirCheckwatermarkPost(string docId,DefaultApi apiInstance);
    }
}
