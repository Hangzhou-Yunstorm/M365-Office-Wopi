
namespace OfficeWopi_YunstormLib
{
    public interface IEASLinkAPIHelper
    {
        /// <summary>
        /// 分享文件下载
        /// </summary>
        /// <param name="rootPath">根目录</param>
        /// <param name="fileId">文件Id</param>
        /// <returns>文件下载路径</returns>
        string SingleFileDownload(string rootPath, string fileId, string token);

        /// <summary>
        /// 获取文件水印信息
        /// </summary>
        /// <returns>文件水印信息</returns>
        string GetShareFileCheckWaterMark(string token);

        /// <summary>
        /// 获取分享文件信息
        /// </summary>
        /// <returns>分享文件信息</returns>
        string GetShareFileInfo(string token);
    }
}
