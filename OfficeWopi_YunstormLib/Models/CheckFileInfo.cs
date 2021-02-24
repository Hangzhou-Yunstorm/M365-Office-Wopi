
namespace OfficeWopi_YunstormLib.Models
{
    /// <summary>
    /// Model according to https://wopirest.readthedocs.io/en/latest/files/CheckFileInfo.html#checkfileinfo and https://msdn.microsoft.com/en-us/library/hh622920.aspx
    /// </summary>
    public class CheckFileInfo
    {
        public string BaseFileName { get; set; }

        public string OwnerId { get; set; }

        public long Size { get; set; }

        public string UserId { get; set; }

        public string Version { get; set; }

        public bool UserCanWrite { get; set; }

        public bool ReadOnly { get; set; }

        public bool SupportsLocks { get; set; }

        public bool SupportsGetLock { get; set; }

        public bool SupportsUpdate { get; set; }

        public bool UserCanNotWriteRelative { get; set; }

        public string UserFriendlyName { get; set; }

        public string SHA256 { get; set; }

        public bool DisablePrint { get; set; }

        public bool ProtectInClient { get; set; }

        public bool RestrictedWebViewOnly { get; set; }

        public string CopyPasteRestrictions { get; set; }        

    }
}
