using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeWopi_YunstormLib;
using OfficeWopi_YunstormLib.Models;
using OfficeWopi_YunstormLib.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OfficeWopi_NetCore31.Controllers
{
    /// <summary>
    /// Implementation of WOPI server protocol
    /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/
    /// </summary>
    [ApiController]
    [Route("wopi/[controller]")]
    public class FilesController : ControllerBase
    {
        /// <summary>
        /// Log4Net
        /// </summary>
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(FilesController));

        /// <summary>
        /// IWebHostEnvironment
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// IConfiguration
        /// </summary>
        public readonly IConfiguration _configuration;

        /// <summary>
        /// Struct Method
        /// </summary>
        /// <param name="hostingEnvironment">IWebHostEnvironment</param>
        /// <param name="lockInfoHelper">ILockInfoHelper</param>
        public FilesController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        /// <summary>
        /// X-WOPI-Override
        /// </summary>
        private string WopiOverrideHeader => HttpContext.Request.Headers[WopiHeaders.WopiOverride];

        /// <summary>
        /// Returns the metadata about a file specified by an identifier.
        /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/files/CheckFileInfo.html
        /// Example URL path: /wopi/files/(file_id)
        /// </summary>
        /// <param name="id">File identifier.</param>
        /// <returns>File info</returns>
        [HttpGet("{id}")]
        public IActionResult GetCheckFileInfo(string id)
        {
            string token = Request.Query["access_token"];
            log.Info($"GetCheckFileInfo file id: {id}, access_token: {token}");

            string jsonString = string.Empty;
            try
            {
                bool isShare = CommonHelper.IsShareFile(token);

                string guidId = Guid.NewGuid().ToString();
                // DownloadFile
                FileInfo file = DownLoadFile(guidId, token, isShare);
                if (file != null)
                {
                    log.Info($"GetCheckFileInfo File name: {file.Name}");
                    // Get CheckFileInfo
                    ICheckFileHelper checkFileHelper = new CheckFileHelper();
                    jsonString = checkFileHelper.GetCheckFileInfo(file, token, isShare);

                    // Remove Path 
                    RemovePathWithFile(file.DirectoryName);
                }
            }
            catch (Exception ex)
            {
                log.Info($"GetCheckFileInfo Excepion: {ex.Message}");
                return new NotFoundResult();
            }
            log.Info($"GetCheckFileInfo Json: {jsonString}");
            return Content(jsonString);
        }

        /// <summary>
        /// Returns contents of a file specified by an identifier.
        /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/files/GetFile.html
        /// Example URL path: /wopi/files/(file_id)/contents
        /// </summary>
        /// <param name="id">File identifier.</param>
        /// <returns>File</returns>
        [HttpGet("{id}/contents")]
        public IActionResult GetFile(string id)
        {
            string token = Request.Query["access_token"];
            log.Info($"GetFile file id: {id}, access_token: {token}");

            try
            {
                // DownLoad File Info
                FileInfo file = DownLoadFile(id, token, CommonHelper.IsShareFile(token));

                // Transform to FileStream
                FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite);
                return new FileStreamResult(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                log.Info($"GetFile, id: {id}, Excepion: {ex.Message}");
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Updates a file specified by an identifier. (Only for non-cobalt files.)
        /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/files/PutFile.html
        /// Example URL path: /wopi/files/(file_id)/contents
        /// </summary>
        /// <param name="id">File identifier.</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}/contents")]
        [HttpPost("{id}/contents")]
        [RequestSizeLimit(1024_000_000)]
        public IActionResult PutFile(string id)
        {
            // EnableBuffering
            Request.EnableBuffering();
            string token = Request.Query["access_token"];
            log.Info($"PutFile file id: {id}, access_token: {token}");

            // Get file
            FileInfo file = GetLocalFile(id, token);
            log.Info($"PutFile file name: {file.Name}");

            // Is File Locked
            if (IsLockFile(token))
            {
                log.Info($"PutFile file is locked, name: {file.Name}");
                return new UnauthorizedResult();
            }
            else
            {
                // Acquire lock
                var lockResult = ProcessLock(id);
                log.Info($"PutFile lockResult: {lockResult}");

                if (lockResult is OkResult)
                {
                    try
                    {
                        log.Info($"PutFile Save Local File Start: {file.Name}");
                        string wholeDirectory = $"{_hostingEnvironment.ContentRootPath}/Files/{Guid.NewGuid().ToString()}/";
                        // Create directory
                        Directory.CreateDirectory(wholeDirectory);

                        // New file to local
                        string fileFullPath = wholeDirectory + file.Name;

                        // Save file to local
                        using (FileStream fs = new FileStream(fileFullPath, FileMode.OpenOrCreate))
                        {
                            const int bufferLen = 50 * 1024 * 1024;
                            byte[] buffer = new byte[bufferLen];
                            int count = 0;
                            while ((count = Request.Body.Read(buffer, 0, bufferLen)) > 0)
                            {
                                fs.Write(buffer, 0, count);
                            }
                            fs.Dispose();
                        }
                        log.Info($"PutFile Save Local File End: {file.Name}");

                        // Upload file to server
                        UploadFile(token, fileFullPath, false);

                        // Remove Path
                        RemovePathWithFile(wholeDirectory);

                        log.Info($"PutFile Save End.");
                    }
                    catch (Exception ex)
                    {
                        log.Info($"PutFile Save, id: {id}, Excepion: {ex.Message}");
                        return new ConflictResult();
                    }
                }
                return lockResult;
            }
        }

        /// <summary>
        /// Put Relative File specified by an identifier. (Only for non-cobalt files.)
        /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/files/PutRelativeFile.html
        /// Example URL path: /wopi/files/(file_id)
        /// </summary>
        /// <param name="id">File identifier.</param>
        /// <returns>Relative File</returns>
        [HttpPost("{id}"), WopiOverrideHeader(new[] { "PUT_RELATIVE" })]
        [RequestSizeLimit(1024_000_000)]
        public IActionResult PutRelativeFile(string id)
        {
            // EnableBuffering
            Request.EnableBuffering();
            string token = Request.Query["access_token"];
            log.Info($"PutRelativeFile file id: {id}, access_token: {token}");

            // New Name
            string newName = Utf7ToUtf8(Request.Headers[WopiHeaders.RelativeTarget]);

            // File Full Path
            string fileFullPath = string.Empty;

            // Save new File local path 
            string wholeDirectory = string.Empty;

            try
            {
                log.Info($"PutRelativeFile New File Start: {newName}");

                wholeDirectory = $"{_hostingEnvironment.ContentRootPath}/Files/{Guid.NewGuid().ToString()}/";
                // Create directory
                Directory.CreateDirectory(wholeDirectory);

                // New file to local
                fileFullPath = wholeDirectory + newName;

                // Save file to local
                using (FileStream fs = new FileStream(fileFullPath, FileMode.OpenOrCreate))
                {
                    const int bufferLen = 50 * 1024 * 1024;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = Request.Body.Read(buffer, 0, bufferLen)) > 0)
                    {
                        fs.Write(buffer, 0, count);
                    }
                    fs.Dispose();
                }

                log.Info($"PutRelativeFile New File End. File: {fileFullPath}");
            }
            catch (Exception ex)
            {
                log.Info($"PutRelativeFile Request Save: {ex.Message}");
            }

            // Upload file to server
            UploadRes uploadRes = UploadFile(token, fileFullPath, true);

            // Delete local path
            RemovePathWithFile(wholeDirectory);

            // new url for new file
            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(token));
            string host = dynamicObj["host"].Value;
            var port = dynamicObj["port"].Value;
            if (port > 0)
            {
                host = $"{host}:{port}";
            }
            string docId = uploadRes.DocId.Replace("://", "=").Replace("/", "%2F");

            // new url
            string fileUrl = $"{host}/#/preview?{docId}";
            string newUrl = $"{Request.Scheme}://{Request.Host.Value}/UnifyEditor.html?Url={HttpUtility.UrlEncode(fileUrl)}";

            // Return Result
            PutRelativeFile putRelativeFile = new PutRelativeFile()
            {
                Name = newName,
                Url = host,
                HostEditUrl = newUrl,
                HostViewUrl = newUrl
            };
            string jsonString = JsonConvert.SerializeObject(putRelativeFile);

            log.Info($"PutRelativeFile Json: {jsonString}");
            return Content(jsonString);
        }

        /// <summary>
        /// Processes lock-related operations.
        /// Specification: https://wopi.readthedocs.io/projects/wopirest/en/latest/files/Lock.html
        /// Example URL path: /wopi/files/(file_id)
        /// </summary>
        /// <param name="id">File identifier.</param>
        [HttpPost("{id}"), WopiOverrideHeader(new[] { "LOCK", "UNLOCK", "REFRESH_LOCK", "GET_LOCK" })]
        public IActionResult ProcessLock(string id)
        {
            log.Info($"ProcessLock file id: {id}, WopiOverrideHeader: {WopiOverrideHeader}");

            string oldLock = Request.Headers[WopiHeaders.OldLock];
            string newLock = Request.Headers[WopiHeaders.Lock];

            lock (FileLockHelper.LockStorage)
            {
                bool lockAcquired = FileLockHelper.TryGetLock(id, out var existingLock);
                log.Info($"ProcessLock lockAcquired: {lockAcquired}, oldLock: {oldLock}, newLock: {newLock}");
                switch (WopiOverrideHeader)
                {
                    case "GET_LOCK":
                        if (lockAcquired)
                        {
                            Response.Headers[WopiHeaders.Lock] = existingLock.Lock;
                        }
                        return new OkResult();

                    case "LOCK":
                    case "PUT":
                        if (string.IsNullOrEmpty(oldLock))
                        {
                            // Lock / put
                            if (lockAcquired)
                            {
                                log.Info($"ProcessLock existingLock: {existingLock.Lock}");
                                if (existingLock.Lock == newLock)
                                {
                                    // File is currently locked and the lock ids match, refresh lock
                                    FileLockHelper.SetLock(id, newLock, _hostingEnvironment.ContentRootPath);

                                    return new OkResult();
                                }
                                else
                                {
                                    // There is a valid existing lock on the file
                                    return ReturnLockMismatch(Response, existingLock.Lock);
                                }
                            }
                            else
                            {
                                // The file is not currently locked, create and store new lock information
                                FileLockHelper.SetLock(id, newLock, _hostingEnvironment.ContentRootPath);

                                return new OkResult();
                            }
                        }
                        else
                        {
                            // Unlock and relock (http://wopi.readthedocs.io/projects/wopirest/en/latest/files/UnlockAndRelock.html)
                            if (lockAcquired)
                            {
                                log.Info($"ProcessLock existingLock: {existingLock.Lock}");
                                if (existingLock.Lock == oldLock)
                                {
                                    // Replace the existing lock with the new one
                                    FileLockHelper.SetLock(id, newLock, _hostingEnvironment.ContentRootPath);

                                    return new OkResult();
                                }
                                else
                                {
                                    // The existing lock doesn't match the requested one. Return a lock mismatch error along with the current lock
                                    return ReturnLockMismatch(Response, existingLock.Lock);
                                }
                            }
                            else
                            {
                                // The requested lock does not exist which should result in a lock mismatch error.
                                return ReturnLockMismatch(Response, reason: "File not locked");
                            }
                        }

                    case "UNLOCK":
                        if (lockAcquired)
                        {
                            if (existingLock.Lock == newLock)
                            {
                                // Remove valid lock
                                FileLockHelper.SetLock(id, null, _hostingEnvironment.ContentRootPath);

                                log.Info($"ProcessLock UNLOCK: OkResult");
                                return new OkResult();
                            }
                            else
                            {
                                // The existing lock doesn't match the requested one. Return a lock mismatch error along with the current lock
                                return ReturnLockMismatch(Response, existingLock.Lock);
                            }
                        }
                        else
                        {
                            // The requested lock does not exist.
                            return ReturnLockMismatch(Response, reason: "File not locked");
                        }

                    case "REFRESH_LOCK":
                        if (lockAcquired)
                        {
                            if (existingLock.Lock == newLock)
                            {
                                // Extend the lock timeout
                                FileLockHelper.SetLock(id, newLock, _hostingEnvironment.ContentRootPath);

                                return new OkResult();
                            }
                            else
                            {
                                // The existing lock doesn't match the requested one. Return a lock mismatch error along with the current lock
                                return ReturnLockMismatch(Response, existingLock.Lock);
                            }
                        }
                        else
                        {
                            // The requested lock does not exist.  That's also a lock mismatch error.
                            return ReturnLockMismatch(Response, reason: "File not locked");
                        }
                }
            }
            return new OkResult();
        }

        #region "private method"
        /// <summary>
        /// Collection holding information about file local path. Should be persistent.
        /// </summary>
        private static Dictionary<string, string> LocalPathStorage = new Dictionary<string, string>();

        /// <summary>
        /// Return Lock Mismatch
        /// </summary>
        /// <param name="response">HttpResponse</param>
        /// <param name="existingLock">Existing Lock</param>
        /// <param name="reason">Reason</param>
        /// <returns>StatusCodeResult</returns>
        private StatusCodeResult ReturnLockMismatch(HttpResponse response, string existingLock = null, string reason = null)
        {
            log.Info($"ReturnLockMismatch existingLock: {existingLock}, reason: {reason}");

            response.Headers[WopiHeaders.Lock] = existingLock ?? string.Empty;
            if (!string.IsNullOrEmpty(reason))
            {
                response.Headers[WopiHeaders.LockFailureReason] = reason;
            }
            return new ConflictResult();
        }

        /// <summary>
        /// Remove Path With File
        /// </summary>
        /// <param name="filePath">file Path</param>
        private void RemovePathWithFile(string filePath)
        {
            try
            {
                // Delete local path
                System.IO.Directory.Delete(filePath, true);
            }
            catch (Exception ex)
            {
                log.Info($"RemovePathWithFile, path: {filePath}， Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="fileFullName">file path</param>
        private UploadRes UploadFile(string token, string fileFullName, bool isNew)
        {
            UploadRes uploadRes = null;
            try
            {
                log.Info($"UploadFile access_token: {token}, file path: {fileFullName}");

                // Upload file
                IEASAPIHelper eASHelper = new EASAPIHelper();
                uploadRes = eASHelper.FileSingleUpload(fileFullName, isNew, token);

                log.Info($"UploadFile End");
            }
            catch (Exception ex)
            {
                log.Info($"UploadFile  Exception: {ex.Message}");
            }
            return uploadRes;
        }

        /// <summary>
        /// Download file by  identifier.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <returns>File</returns>
        private FileInfo DownLoadFile(string fileId, string token, bool isShare)
        {
            try
            {
                log.Info($"DownLoadFile file id: {fileId}, access_token: {token}");
                string filePath = string.Empty;
                if (isShare)
                {
                    // Download share file
                    IEASLinkAPIHelper fileHelper = new EASLinkAPIHelper();
                    filePath = fileHelper.SingleFileDownload(_hostingEnvironment.ContentRootPath, fileId, token);
                }
                else
                {
                    string rev = CommonHelper.GetDocRev(token);

                    // Download file
                    IEASAPIHelper fileHelper = new EASAPIHelper();
                    filePath = fileHelper.SingleFileDownload(_hostingEnvironment.ContentRootPath, fileId, rev, token);

                    if (string.IsNullOrEmpty(rev))
                    {
                        SetLocalPath(fileId, filePath);
                    }
                }

                log.Info($"DownLoadFile file end.");
                return new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                SetLocalPath(fileId, null);
                log.Info($"Download file Exception: {ex.Message}");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Is Lock File
        /// </summary>
        /// <returns>Is Lock</returns>
        private bool IsLockFile(string token)
        {
            bool isLock = false;
            log.Info($"IsLockFile access_token: {token}");
            try
            {
                IEASAPIHelper fileHelper = new EASAPIHelper();
                // User info
                var userInfo = fileHelper.UserGetPost(token);
                // Lock info
                var lockInfo = fileHelper.AutolockGetlockinfoPost(token);

                // Locked & Not me
                if ((lockInfo.Islocked.HasValue && lockInfo.Islocked.Value) && (userInfo.Account != lockInfo.Lockeraccount))
                {
                    isLock = true;
                }
            }
            catch (Exception ex)
            {
                log.Info($"IsLockFile  Exception: {ex.Message}");
            }
            return isLock;
        }

        /// <summary>
        ///  Get Local file
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <returns>Local file</returns>
        private FileInfo GetLocalFile(string fileId, string token)
        {
            log.Info($"GetLocalFile file id: {fileId}, token: {token}");

            lock (LocalPathStorage)
            {
                if (LocalPathStorage.TryGetValue(fileId, out string localpath))
                {
                    if (System.IO.File.Exists(localpath))
                    {
                        log.Info($"GetLocalFile file local.");
                        return new FileInfo(localpath);
                    }
                    else
                    {
                        log.Info($"GetLocalFile file local & download.");
                        return DownLoadFile(fileId, token, false);
                    }
                }
                else
                {
                    log.Info($"GetLocalFile file download.");
                    return DownLoadFile(fileId, token, false);
                }
            }
        }

        /// <summary>
        /// Set Local path
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <param name="localpath">Local path</param>
        private void SetLocalPath(string fileId, string localpath)
        {
            lock (LocalPathStorage)
            {
                if (string.IsNullOrEmpty(localpath))
                {
                    log.Info($"SetLocalPath null, id: {fileId}");
                    LocalPathStorage.Remove(fileId);
                }
                else
                {
                    log.Info($"SetLocalPath file id: {fileId}, localpath: {localpath}");
                    LocalPathStorage[fileId] = localpath;
                }
            }
        }

        /// UTF-7 to UTF-8
        /// </summary>
        /// <param name="str7">UTF-7</param>
        /// <returns>UTF-8</returns>
        private string Utf7ToUtf8(string str7)
        {
            log.Info($"Utf7ToUtf8, UTF7: {str7}");

            string sUtf7 = str7.StartsWith("&") ? $"+{str7.Substring(1)}" : str7;
            byte[] bytes = Encoding.UTF8.GetBytes(sUtf7);
            byte[] byteReturn = Encoding.Convert(Encoding.UTF7, Encoding.UTF8, bytes);
            string str8 = Encoding.UTF8.GetString(byteReturn);

            log.Info($"Utf7ToUtf8, UTF8: {str8}");
            return str8;
        }

        #endregion
    }
}
