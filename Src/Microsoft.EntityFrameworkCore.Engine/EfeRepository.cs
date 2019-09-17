using Microsoft.EntityFrameworkCore.Engine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Engine
{
    public partial class EfeRepository<T> : IRepository<T> where T : class
    {
        #region Fields
        private readonly IDbContext _context;
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
        public EfeRepository(IDbContext context)
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

        #region Insert / Update / Delete
        public int Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                this.Entities.Add(entity);
                return this._context.SaveChanges();

                //ToTest
                //throw new Exception("Error!");
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entity);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entity);
                throw ex;
            }
        }
        public int Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                foreach (var entity in entities)
                    Entities.Add(entity);
                return _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entities);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entities);
                throw ex;
            }
        }

        public int Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                return this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entity);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entity);
                throw ex;
            }
        }
        public int Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entities);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entities);
                throw ex;
            }
        }

        public int Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                this.Entities.Remove(entity);
                return this._context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entity);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entity);
                throw ex;
            }
        }
        public int Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                foreach (var entity in entities)
                    this.Entities.Remove(entity);

                return _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Attach(entities);
                throw new EfeRepositoryException(dbEx);
            }
            catch (Exception ex)
            {
                Attach(entities);
                throw ex;
            }
        }
        #endregion

        #region attach / detach
        // Attach / Detach （可用于SaveChanges异常时回滚添加的实体） ToImprove

        /// <summary>
        /// 附加实体，以“未更改”的状态附加实体到仓库中。
        /// </summary>
        /// <remarks>
        /// 1、将实体以“未更改”的状态放置到仓库中（数据上下文中）
        /// 2、在SaveChanges时，附加的实体不会被存储到数据库中。
        /// </remarks>
        public void Attach(T entity)
        {
            //使用附加实体，使SaveChanges不会更新异常的实体
            //实现异常时移除目标更新实体
            this.Entities.Attach(entity);
            this._context.SaveChanges();
        }

        /// <summary>
        /// 附加实体，以“未更改”的状态附加实体到仓库中。
        /// </summary>
        /// <remarks>
        /// 1、Attaches the given entity to the context underlying the set. That is, the entity is placed into the context in the Unchanged state, just as if it had been read from the database.
        /// 2、SaveChanges will not to insert an attached entity into the database.
        /// </remarks>
        public void Attach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                if (this.Entities.Contains(entity)) this.Entities.Attach(entity);

            this._context.SaveChanges();
        }

        /// <summary>
        /// 移除实体，从仓库中移除实体。
        /// </summary>
        public void Detach(T entity)
        {
            if (entity != null)
                _context.Detach(entity);
        }

        /// <summary>
        /// 移除实体，从仓库中移除实体。
        /// </summary>
        public void Detach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                if (this.Entities.Contains(entity))
                    _context.Detach(entity);
        }
        #endregion
    }
}
