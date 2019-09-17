using Autofac;
using Autofac.Engine;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web;
using System.Linq;
using System.Configuration;
using EntityFramework.Engine.Configuration;
using EntityFramework.Engine;

namespace MSSurfing.WebApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region Http & Web
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();
            #endregion

            #region Controllers
            //register mvc controller
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            //register api controller
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());
            #endregion


            //builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service").AsImplementedInterfaces().InstancePerLifetimeScope();


            //repository 
            var config = ConfigurationManager.GetSection("EfeConfig") as EfeConfig;
            builder.Register<IDbContext>(e => new EfeObjectContext(config.ConnectionString)).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfeRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}