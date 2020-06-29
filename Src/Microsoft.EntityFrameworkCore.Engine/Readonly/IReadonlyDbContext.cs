using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Engine.Readonly
{
    public interface IReadonlyDbContext : IDisposable
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        IModel Model { get; }

        DbConnection GetDbConnection();
        string DatabaseCreateScript();
        DbSet<T> Set<T>() where T : class;

        IQueryable<T> QueryFromSql<T>(string sql, params object[] parameters) where T : class;

        IQueryable<T> EntityFromSql<T>(string sql, params object[] parameters) where T : class;
    }
}
