using Autofac;
using Autofac.Engine;
using Microsoft.EntityFrameworkCore.Engine;
using Microsoft.EntityFrameworkCore.Engine.Readonly;
using MSSurfing.ThriftServer.NetCore.Processors;

namespace MSSurfing.ThriftServer.NetCore.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // match
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.NetCore.dll", "Service")
                .AsImplementedInterfaces().InstancePerDependency();

            // entity framework register
            var connectionString = "Database=MDStructure;Server=.;User ID=sa;Password=sa;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";
            var connectionString2 = "Database=MDStructure;Server=.;User ID=sa;Password=sa;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";
            //var config = ConfigurationManager.GetSection("EfeConfig") as EfeConfig;

            builder.Register<IDbContext>(e => new EfeObjectContext(connectionString)).InstancePerDependency();
            builder.Register<IReadonlyDbContext>(e => new EfeReadonlyDbContext(connectionString2)).InstancePerDependency();

            // repository generic type
            //builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();
            builder.RegisterGeneric(typeof(EfeRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(EfeReadonlyRepository<>)).As(typeof(IReadonlyRepository<>)).InstancePerDependency();

            // processor
            builder.RegisterType<UserProcessor>().AsSelf().InstancePerDependency();
        }
    }
}
