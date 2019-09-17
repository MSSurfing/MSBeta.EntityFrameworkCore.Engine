using Autofac;
using Autofac.Engine;
using EntityFramework.Engine;
using EntityFramework.Engine.Configuration;
using MSSurfing.ThriftServer.Processors;
using System.Configuration;

namespace MSSurfing.ThriftServer.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // match
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service")
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // entity framework register
            //var connectionString = "Data Source=47.98.165.82;Initial Catalog=Surfing10;Persist Security Info=True;User ID=sa;Password=z;Connect Timeout=360";
            var config = ConfigurationManager.GetSection("EfeConfig") as EfeConfig;
            builder.Register<IDbContext>(e => new EfeObjectContext(config.ConnectionString)).InstancePerLifetimeScope();

            // repository generic type
            //builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();
            builder.RegisterGeneric(typeof(EfeRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // processor
            builder.RegisterType<UserProcessor>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
