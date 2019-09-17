using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Engine
{
    public partial interface IRepository<T> where T : class
    {
        #region Properties
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        #endregion

        #region Get Methods
        T GetById(object id);
        #endregion

        #region Insert / Update / Delete
        int Insert(T entity);
        int Insert(IEnumerable<T> entities);

        int Update(T entity);
        int Update(IEnumerable<T> entities);

        int Delete(T entity);
        int Delete(IEnumerable<T> entities);
        #endregion

        #region attach / detach
        // Attach / Detach （可用于SaveChanges异常时回滚添加的实体） ToImprove

        /// <summary>
        /// 附加实体,以"未更改"的状态附加实体到仓库中.
        /// </summary>
        /// <remarks>
        /// 1、将实体以“未更改”的状态放置到仓库中（数据上下文中）
        /// 2、在SaveChanges时，附加的实体不会被存储到数据库中。
        /// </remarks>
        void Attach(T entity);

        /// <summary>
        /// 附加实体,以"未更改"的状态附加实体到仓库中.
        /// </summary>
        /// <remarks>
        /// 1、Attaches the given entity to the context underlying the set. That is, the entity is placed into the context in the Unchanged state, just as if it had been read from the database.
        /// 2、SaveChanges will not to insert an attached entity into the database.
        /// </remarks>
        void Attach(IEnumerable<T> entities);

        /// <summary>移除实体,从仓库中移除实体</summary>
        /// <remarks>另外 可以把实体设为非NoTracking，以节省部分内存的开销</remarks>
        void Detach(T entity);

        void Detach(IEnumerable<T> entities);
        #endregion
    }
}
