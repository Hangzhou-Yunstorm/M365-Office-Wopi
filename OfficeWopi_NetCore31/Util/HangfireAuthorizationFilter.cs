﻿using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeWopi_NetCore31
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        //这里需要配置权限规则
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
