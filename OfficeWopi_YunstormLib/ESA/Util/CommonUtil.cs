using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EASFramework.Main.Framework.Util
{
    class CommonUtil
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content">待加密内容</param>
        /// <returns>加密后内容</returns>
        public static string RSAEncode(string content)
        {
            string publickey = @"<RSAKeyValue><Modulus>uyS9A3GjFB7pknYcV08aogAQQgxEYUSSLADwfvs8dSDYEhCjxm3sQ7daI3DQHNHyPhv8k7kHIB9RFvKaLIFJ4tJnExOgp45FW7/CC4ArocvuHrvtpQKQ8EDw/U6+ifJNtUbrtrFleWdVUbkBahpv3Ob2kzkBOVRTiFz1U2mtuZk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);

            byte[] cipherbytes;
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            // 由于windows 使用的\r\n对 base64进行换行，而服务器rsa只支持 \n换行的base64 串，故将\r\n替换成 \n
            return Convert.ToBase64String(cipherbytes, Base64FormattingOptions.InsertLineBreaks).Replace("\r\n", "\n");
        }

        /// <summary>
        /// 读取文件内容到byte数组
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容</returns>
        public static byte[] FileToBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("文件路径错误，该路径下不存在文件。");
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Dispose();

                return bytes;
            }            
        }

    }
}
