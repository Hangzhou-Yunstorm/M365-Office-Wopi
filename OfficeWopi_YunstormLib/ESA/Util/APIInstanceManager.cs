using IO.OpenDocAPI.Api;
using IO.OpenDocAPI.Client;

namespace EASFramework.Main.Framework.Util
{
    public class APIInstanceManager
    {
        /// <summary>
        /// Get ApiInstance
        /// </summary>
        /// <param name="tokenId">Access token</param>
        /// <param name="serviceUrl">Service Url</param>
        /// <returns>ApiInstance</returns>
        public static DefaultApi GetApiInstance(string tokenId, string serviceUrl)
        {
            string basePath = serviceUrl + "/api/v1";
            Configuration configuration = new Configuration();
            configuration.BasePath = basePath;
            configuration.SetVerifyingSsl(false);
            configuration.AccessToken = tokenId;
            return new DefaultApi(configuration);
        }
    }
}
