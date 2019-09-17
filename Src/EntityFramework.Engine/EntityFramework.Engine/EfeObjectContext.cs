using EntityFramework.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Engine
{
    public class EfeObjectContext : DbContext, IDbContext
    {
        #region Fields
        private const string IGNORE_ASSEMBLY_PATTERN = "^System|^mscorlib|^Microsoft";
        #endregion

        #region Ctor
        public EfeObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        #endregion

        #region Utilities
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (Assembly assembly in GetAssemblies())
            {
                var typesToRegister = assembly.GetTypes()
                    .Where(type => type.BaseType != null && type.BaseType.IsGenericType)
                    .Where(type => type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

                foreach (var type in typesToRegister)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstance);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Equals(entity));
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                return alreadyAttached;
            }
        }

        protected virtual Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        #endregion

        #region Methods
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class, new()
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }

        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion

        #region Properties
        public virtual bool LazyLoadingEnabled
        {
            get { return this.Configuration.LazyLoadingEnabled; }
            set { this.Configuration.LazyLoadingEnabled = value; }
        }
        public virtual bool ProxyCreationEnabled
        {
            get { return this.Configuration.ProxyCreationEnabled; }
            set { this.Configuration.ProxyCreationEnabled = value; }
        }

        public virtual bool AutoDetectChangesEnabled
        {
            get { return this.Configuration.AutoDetectChangesEnabled; }
            set { this.Configuration.AutoDetectChangesEnabled = value; }
        }
        #endregion
    }
}
