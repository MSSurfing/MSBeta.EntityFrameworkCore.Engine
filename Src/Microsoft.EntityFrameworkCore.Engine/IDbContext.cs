using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Engine
{
    public interface IDbContext
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        IModel Model { get; }

        DbConnection GetDbConnection();
        string DatabaseCreateScript();
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();

        IQueryable<T> QueryFromSql<T>(string sql, params object[] parameters) where T : class;

        IQueryable<T> EntityFromSql<T>(string sql, params object[] parameters) where T : class;

        int ExecuteSqlCommand(string sql, bool doInTransaction = true, params object[] parameters);

        /// "timeout" 指定为0时，表示无限制
        int ExecuteSqlCommand(string sql, int? timeout, bool doInTransaction = true, params object[] parameters);

        //[Obsolete("")]
        //int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        void Detach(object entity);
    }
}
