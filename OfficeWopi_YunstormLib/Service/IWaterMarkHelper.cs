
namespace OfficeWopi_YunstormLib
{
    public interface IWaterMarkHelper
    {
        /// <summary>
        /// Get Base64 PNG image & repeat
        /// </summary>
        /// <param name="markReq">WaterMarkReq</param>
        /// <param name="repeat">repeat</param>
        string GetBase64Png(string token, out bool repeat);
    }
}
