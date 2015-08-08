using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WitBird.Sex.WebV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void Application_BeginRequest()
        {
            if (Context.Request.FilePath.Equals("/"))
            {
                if (Request.Url.Host == WitBird.Sex.WebV2.Cache.WebConfig.DomainMobile)
                {
                    Context.RewritePath("/m/");
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            WitBird.Sex.Common.LogService.Log("系统发生未处理异常", ex.ToString());
            Server.ClearError();
            Server.TransferRequest("/error/index", true);
        }
    }
}