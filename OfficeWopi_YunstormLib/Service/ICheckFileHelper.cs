using System.IO;

namespace OfficeWopi_YunstormLib
{
    public interface ICheckFileHelper
    {
        /// <summary>
        /// Get CheckFile Info
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <param name="token">access_token</param>
        /// <param name="isShare">is Share file</param>
        /// <returns>Json</returns>
        string GetCheckFileInfo(FileInfo file, string token, bool isShare);
    }
}
