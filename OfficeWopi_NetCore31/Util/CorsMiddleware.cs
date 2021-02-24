using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OfficeWopi_NetCore31
{
    public class CorsMiddleware
    {
        /// <summary>
        /// RequestDelegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Struct Method
        /// </summary>
        /// <param name="next">RequestDelegate</param>
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Access-Control-Allow-Origin
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>*</returns>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            await _next(context);
        }
    }
}
