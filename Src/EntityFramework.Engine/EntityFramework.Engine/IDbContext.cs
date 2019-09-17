using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace EntityFramework.Engine
{
    public interface IDbContext
    {
        string CreateDatabaseScript();
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters) where T : class, new();
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);
        void Detach(object entity);

        #region Configuration
        /// <summary>
        /// 延迟加载,默认为使用状态
        /// 延迟加载：即在使用的时候才加载数据，如延迟加载状态下导航属性会在使用时独立查询。
        /// 注：如导航属性需要大量直接循环，可以使用System.Data.Entity命名空间下IQueryable&lt;T&gt;.Include方法预加载导航属性。
        /// </summary>
        bool LazyLoadingEnabled { get; set; }
        bool ProxyCreationEnabled { get; set; }
        bool AutoDetectChangesEnabled { get; set; }
        #endregion
    }
}
