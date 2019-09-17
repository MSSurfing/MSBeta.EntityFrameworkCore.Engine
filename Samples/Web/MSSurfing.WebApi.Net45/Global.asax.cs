using Autofac.Engine;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MSSurfing.WebApi.Net45
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            EngineContext.Initialize(ScopeTag.Http);            //用于 Web / Api
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(EngineContext.Scope);        //用于Web Api,需引入 Install-Package Autofac.WebApi2
        }
    }
}
