using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Engine.Readonly
{
    public class EfeReadonlyDbContext : DbContext, IReadonlyDbContext
    {
        private readonly string _connectionString;
        #region Fields
        private const string IGNORE_ASSEMBLY_PATTERN = "^System|^mscorlib|^Microsoft";
        #endregion

        #region Ctor
        public EfeReadonlyDbContext(string connectionString, DbOptions useSqlType = DbOptions.UseSqlServer)
            : this(UseOptions(connectionString, useSqlType))
        {

        }

        public EfeReadonlyDbContext(DbContextOptions options) : base(options)
        {

        }

        public EfeReadonlyDbContext(DbContextOptions<EfeObjectContext> options) : base(options)
        {

        }

        #endregion

        #region Utilities
        private static DbContextOptions UseOptions(string connectionString, DbOptions useSqlType)
        {
            var builder = new DbContextOptionsBuilder(new DbContextOptions<EfeReadonlyDbContext>());

            switch (useSqlType)
            {
                case DbOptions.UseMySql:
                    return builder.UseMySql(connectionString).Options;
                case DbOptions.UseInMemory:
                    return builder.UseInMemoryDatabase("Surfing-db").Options;
                case DbOptions.UseSqlServer:
                default:
                    return builder.UseSqlServer(connectionString).Options;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 不应依赖.SqlServer 或 MySql

            // 需要引入 Microsoft.EntityFrameworkCore.SqlServer
            //optionsBuilder.UseSqlServer(_connectionString);

            //需要引入 Pomelo.EntityFrameworkCore.MySql
            //optionsBuilder.UseMySql(_connectionString);

        }

        // Todo ,is typeof(Metadata.Internal.DbFunction)
        protected virtual bool IsTypeConfiguration(Type type)
        {
            var types = type.FindInterfaces((iType, criteria)
                => iType.IsGenericType
                    && (iType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                        || iType.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>))
                    , null);

            return types.Length > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Assembly assembly in GetAssemblies())
            {
                var typesToRegister = assembly.GetTypes()
                    .Where(IsTypeConfiguration);

                foreach (var type in typesToRegister)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.ApplyConfiguration(configurationInstance);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected virtual Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        #endregion

        #region Methods
        public DbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }

        public string DatabaseCreateScript()
        {
            return this.Database.GenerateCreateScript();
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public virtual IQueryable<T> QueryFromSql<T>(string sql, params object[] parameters) where T : class
        {
            return Query<T>().FromSql(sql, parameters);
        }

        public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return Set<TEntity>().FromSql(sql, parameters);
        }
        #endregion
    }
}
