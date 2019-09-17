using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Engine.Services
{
    public interface IEfeService<T>
    {
        T GetById(object Id);
        bool Insert(T t);
        bool Update(T t);
        bool Delete(T t);
    }
}
