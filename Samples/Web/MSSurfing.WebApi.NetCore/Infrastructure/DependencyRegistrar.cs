using Autofac;
using Autofac.Engine;
using Microsoft.EntityFrameworkCore.Engine;

namespace MSSurfing.WebApi.NetCore.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // match
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.NetCore.dll", "Service")
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            var useType = DbOptions.UseSqlServer;
            var connectionString = "";
            // entity framework register
            if (useType == DbOptions.UseSqlServer)
                connectionString = "Data Source=47.98.165.82;Initial Catalog=Surfing10;Persist Security Info=True;User ID=sa;Password=z;Connect Timeout=360";
            else if (useType == DbOptions.UseMySql)
                connectionString = "Data Source=172.17.10.144;Database=Surfing15;User ID=zeroing;Password=123456;pooling=true;port=3306;sslmode=none;CharSet=utf8;";
            //var config = ConfigurationManager.GetSection("EfeConfig") as EfeConfig;

            builder.Register<IDbContext>(e => new EfeObjectContext(connectionString, useType)).InstancePerLifetimeScope();

            // repository generic type
            //builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();
            builder.RegisterGeneric(typeof(EfeRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // controllers

        }
    }
}
