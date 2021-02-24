using Newtonsoft.Json;

namespace OfficeWopi_YunstormLib
{
    public static class CommonHelper
    {
        /// <summary>
        /// Is Share file
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <returns>Is Share file</returns>
        public static bool IsShareFile(string token)
        {
            token = System.Web.HttpUtility.UrlDecode(token);
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(token);
            string tokenId = dynamicObj["tokenid"]?.Value;
            return string.IsNullOrEmpty(tokenId);
        }

        /// <summary>
        /// Get file rev
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <returns>File rev</returns>
        public static string GetDocRev(string token)
        {
            token = System.Web.HttpUtility.UrlDecode(token);
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(token);
            string rev = string.Empty;
            // 版本
            try
            {
                rev = dynamicObj["doc"]["rev"].Value;
            }
            catch
            {
                rev = string.Empty;
            }
            return rev;
        }
    }
}
