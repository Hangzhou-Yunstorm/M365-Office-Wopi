﻿
namespace OfficeWopi_YunstormLib.Util
{
    public class WopiHeaders
    {
        public const string WopiOverride = "X-WOPI-Override";

        /// <summary>
        /// A string value identifying the current lock on the file. 
        /// This header must always be included when responding to the request with 409 Conflict. 
        /// It should not be included when responding to the request with 200 OK.
        /// </summary>
		public const string Lock = "X-WOPI-Lock";

        public const string OldLock = "X-WOPI-OldLock";

        public const string LockFailureReason = "X-WOPI-LockFailureReason";

        public const string SuggestedTarget = "X-WOPI-SuggestedTarget";

        public const string RelativeTarget = "X-WOPI-RelativeTarget";

        public const string OverwriteRelativeTarget = "X-WOPI-OverwriteRelativeTarget";

        public const string CorrelationId = "X-WOPI-CorrelationID";

        public const string MaxExpectedSize = "X-WOPI-MaxExpectedSize";

        public const string WopiSrc = "X-WOPI-WopiSrc";

        public const string EcosystemOperation = "X-WOPI-EcosystemOperation";

        public const string ServerError = "X-WOPI-ServerError";
    }
}
