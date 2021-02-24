using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Linq;

namespace OfficeWopi_NetCore31
{
    public class HttpHeaderAttribute : Attribute, IActionConstraint
    {
        /// <summary>
        /// Header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Values
        /// </summary>
        public string[] Values { get; set; }

        public HttpHeaderAttribute(string header, params string[] values)
        {
            Header = header;
            Values = values;
        }

        /// <summary>
        /// ActionConstraint
        /// </summary>
        /// <param name="context">ActionConstraintContext</param>
        /// <returns></returns>
        public bool Accept(ActionConstraintContext context)
        {
            return context == null
                ? false
                : context.RouteContext.HttpContext.Request.Headers.TryGetValue(Header, out var value) ? Values.Contains(value[0]) : false;
        }

        /// <summary>
        /// Order
        /// </summary>
        public int Order => 0;
    }
}
