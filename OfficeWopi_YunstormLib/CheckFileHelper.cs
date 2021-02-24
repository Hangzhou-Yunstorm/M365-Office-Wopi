using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using OfficeWopi_YunstormLib.Models;

namespace OfficeWopi_YunstormLib
{
    public class CheckFileHelper : ICheckFileHelper
    {
        /// <summary>
        /// Get CheckFile Info
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <param name="token">access_token</param>
        /// <param name="isShare">Is Share file</param>
        /// <returns>Json</returns>
        public string GetCheckFileInfo(FileInfo file, string token, bool isShare)
        {
            try
            {
                CheckFileInfo checkFileInfo = new CheckFileInfo();

                bool onlyRead = false;
                if (isShare)
                {
                    onlyRead = true;
                    checkFileInfo.UserId = "Anonymous";
                    checkFileInfo.OwnerId = "Anonymous";
                    checkFileInfo.UserFriendlyName = "Anonymous";
                }
                else
                {
                    IEASAPIHelper eASAPI = new EASAPIHelper();
                    var userInfo = eASAPI.UserGetPost(token);

                    // History file or No edit permission
                    if (!string.IsNullOrEmpty(CommonHelper.GetDocRev(token)) || !eASAPI.IsCanEditFile(token) || (userInfo.Freezestatus.HasValue && userInfo.Freezestatus.Value))
                    {
                        onlyRead = true;
                    }
                    else
                    {
                        // Locked Info
                        try
                        {                          
                            var lockInfo = eASAPI.AutolockGetlockinfoPost(token);
                            // Locked & Not me
                            if ((lockInfo.Islocked.HasValue && lockInfo.Islocked.Value) && (userInfo.Account != lockInfo.Lockeraccount))
                            {
                                onlyRead = true;
                            }
                        }
                        catch
                        {
                            onlyRead = true;
                        }
                    }

                    // User Info
                    try
                    {
                        checkFileInfo.UserId = userInfo.Account;
                        checkFileInfo.OwnerId = userInfo.Userid;
                        checkFileInfo.UserFriendlyName = userInfo.Name;
                    }
                    catch
                    {
                        checkFileInfo.UserId = "Anonymous";
                        checkFileInfo.OwnerId = "Anonymous";
                        checkFileInfo.UserFriendlyName = "Anonymous";
                    }
                }

                // Old Office file
                string fileExt = Path.GetExtension(file.Name).ToLower();
                if (".doc".Equals(fileExt) || ".xls".Equals(fileExt) || ".ppt".Equals(fileExt))
                {
                    onlyRead = true;
                }

                string fileName = file.Name;
                if (fileName.Contains("\\"))
                {
                    fileName = fileName.Split("\\").LastOrDefault();
                }

                // Basic info
                checkFileInfo.BaseFileName = fileName;
                checkFileInfo.Version = file.LastWriteTimeUtc.ToString("s", CultureInfo.InvariantCulture);
                checkFileInfo.Size = file.Length;

                if (".doc".Equals(fileExt) || ".docx".Equals(fileExt))
                {
                    // SHA256
                    try
                    {
                        using (FileStream stream = file.OpenRead())
                        {
                            using (var sha = SHA256.Create())
                            {
                                checkFileInfo.SHA256 = Convert.ToBase64String(sha.ComputeHash(stream));
                            }
                            stream.Dispose();
                        }
                    }
                    catch
                    {
                        checkFileInfo.SHA256 = string.Empty;
                    }
                }

                // File attribute
                checkFileInfo.ReadOnly = onlyRead;
                checkFileInfo.UserCanWrite = !onlyRead;
                checkFileInfo.UserCanNotWriteRelative = onlyRead;
                checkFileInfo.SupportsLocks = !onlyRead;
                checkFileInfo.SupportsGetLock = !onlyRead;
                checkFileInfo.SupportsUpdate = !onlyRead;
                checkFileInfo.ProtectInClient = onlyRead;
                checkFileInfo.RestrictedWebViewOnly = onlyRead;
                checkFileInfo.DisablePrint = true;
                checkFileInfo.CopyPasteRestrictions = "BlockAll";

                return JsonConvert.SerializeObject(checkFileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
