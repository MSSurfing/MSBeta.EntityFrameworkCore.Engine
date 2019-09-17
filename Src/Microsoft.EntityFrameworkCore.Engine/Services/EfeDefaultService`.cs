using System;

namespace Microsoft.EntityFrameworkCore.Engine.Services
{
    public abstract class EfeDefaultService<T> : IEfeService<T> where T : class
    {
        #region Fields
        protected readonly IRepository<T> _repository;
        #endregion

        public EfeDefaultService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T GetById(object Id)
        {
            return _repository.GetById(Id);
        }

        public bool Insert(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(T));

            return _repository.Insert(t) > 0;
        }

        public bool Update(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(T));

            return _repository.Update(t) > 0;
        }

        public bool Delete(T t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(T));

            return _repository.Delete(t) > 0;
        }
    }
}
