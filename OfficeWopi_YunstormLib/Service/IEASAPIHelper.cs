using IO.OpenDocAPI.Model;
using OfficeWopi_YunstormLib.Models;

namespace OfficeWopi_YunstormLib
{
    public interface IEASAPIHelper
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="rootPath">根目录</param>
        /// <param name="fileId">文件Id</param>
        /// <param name="rev">版本历史</param>
        /// <returns>下载文件路径</returns>
        string SingleFileDownload(string rootPath, string fileId, string rev, string token);

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isNew">是否新建</param>
        /// <returns>上传结果</returns>
        UploadRes FileSingleUpload(string filePath, bool isNew, string token);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        UserGetRes UserGetPost(string token);

        /// <summary>
        /// 获取文件锁信息
        /// </summary>
        /// <returns>文件锁信息</returns>
        AutolockGetlockinfoRes AutolockGetlockinfoPost(string token);

        /// <summary>
        ///  获取文件水印信息
        /// </summary>
        /// <returns>文件水印信息</returns>
        string DirCheckwatermarkPost(string token);

        /// <summary>
        /// 文件是否有编辑权限
        /// </summary>
        /// <returns>是否有编辑权限</returns>
        bool IsCanEditFile(string token);
    }
}
