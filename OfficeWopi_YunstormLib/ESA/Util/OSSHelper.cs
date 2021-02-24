using EASFramework.Main.Framework.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EASFramework.Main.Framework.Util
{
    class OSSHelper
    {
        public OSSHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };
        }

        public HttpWebResponse SendReqToOSS(String method, String url, List<String> headers, byte[] postData)
        {
            // set url
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // set method
            request.Method = method;
            // set body
            if (postData != null)
            {
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
            }
            // set headers
            for (int i = 0; i < headers.Count; i++)
            {
                string[] authHeaderArray = headers[i].Split(new string[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);
                // 标准头使用属性修改
                if (String.Equals(authHeaderArray[0], "Content-Length", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.ContentLength = Convert.ToInt64(authHeaderArray[1]);
                }
                else if (String.Equals(authHeaderArray[0], "Content-Type", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.ContentType = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Expect", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Expect = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Accept", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Accept = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Referer", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Referer = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "User-Agent", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.UserAgent = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Date", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Date = Convert.ToDateTime(authHeaderArray[1]);
                }
                else if (String.Equals(authHeaderArray[0], "If-Modified-Since", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.IfModifiedSince = Convert.ToDateTime(authHeaderArray[1]);
                }
                else if (String.Equals(authHeaderArray[0], "Connection", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Connection = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Host", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.Host = authHeaderArray[1];
                }
                else if (String.Equals(authHeaderArray[0], "Transfer-Encoding", StringComparison.CurrentCultureIgnoreCase))
                {
                    request.TransferEncoding = authHeaderArray[1];
                }
                else
                {
                    request.Headers.Add(headers[i]);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            int resCode = (int)response.StatusCode;

            // 若为错误返回码则抛出异常
            if (resCode < 200 || resCode >= 300)
            {
                String errHeaders = "";
                String key = "";
                for (int i = 0; i < response.Headers.Count; i++)
                {
                    key = response.Headers.GetKey(i);
                    errHeaders += key + ":" + response.Headers[key] + " ";
                }
                Stream ResStream = response.GetResponseStream();
                String errBody = "";
                using (StreamReader reader = new StreamReader(ResStream, Encoding.UTF8))
                {
                    errBody = reader.ReadToEnd();
                }
                throw new OSSException(resCode, errBody, errHeaders);
            }

            return response;
        }
    }
}
