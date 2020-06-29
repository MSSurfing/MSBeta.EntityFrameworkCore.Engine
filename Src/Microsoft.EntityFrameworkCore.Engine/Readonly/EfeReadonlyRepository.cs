using Microsoft.EntityFrameworkCore.Engine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Engine.Readonly
{
    public class EfeReadonlyRepository<T> : IReadonlyRepository<T> where T : class
    {
        #region Fields
        private IReadonlyDbContext _context;
        private DbSet<T> _dbSet;
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = _context.Set<T>();
                return _dbSet;
            }
        }
        #endregion

        #region Ctor
        public EfeReadonlyRepository(IReadonlyDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Properties
        public virtual IQueryable<T> Table => this.Entities;
        public virtual IQueryable<T> TableNoTracking => this.Entities.AsNoTracking();
        #endregion

        #region Get Methods
        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        #endregion

        public void Dispose()
        {
            _context.Dispose();
            _context = null;
            _dbSet = null;
        }
    }
}
