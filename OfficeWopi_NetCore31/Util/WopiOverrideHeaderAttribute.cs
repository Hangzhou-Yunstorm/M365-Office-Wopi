using OfficeWopi_YunstormLib.Util;

namespace OfficeWopi_NetCore31
{
    public class WopiOverrideHeaderAttribute : HttpHeaderAttribute
    {
        /// <summary>
        /// WopiOverrideHeaderAttribute
        /// </summary>
        /// <param name="values">values</param>
        public WopiOverrideHeaderAttribute(string[] values) : base(WopiHeaders.WopiOverride, values)
        {
        }
    }
}
