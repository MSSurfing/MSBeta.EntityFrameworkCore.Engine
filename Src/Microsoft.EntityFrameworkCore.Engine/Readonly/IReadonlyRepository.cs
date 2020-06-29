using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Engine.Readonly
{
    public partial interface IReadonlyRepository<T> : IDisposable where T : class
    {
        #region Properties
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        #endregion

        #region Get Methods
        T GetById(object id);
        #endregion
    }
}
