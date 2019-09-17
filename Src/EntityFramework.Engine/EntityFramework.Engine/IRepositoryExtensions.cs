/* 弃用
20181206_2001
	删除了 InsertLine的以Line结尾的格式，因为做为核心类库不必要/不应提供返回bool的扩展。
	所为返回bool的扩展是，默认Insert返回int,有时候我们代码中只需要返回ture.
    因此在RepositoryExtensions中实现了该扩展，但想了想，还是不应该提供，应由应用者自己进行扩展。

    现增加了 *If,虽然觉得没必要，但还是加了。
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Engine
{
    public static class IRepositoryExtensions
    {
        public static bool InsertIf<T>(this IRepository<T> repository, T entity) where T : class
        {
            return repository.Insert(entity) > 0;
        }
        public static bool InsertIf<T>(this IRepository<T> repository, IEnumerable<T> entities) where T : class
        {
            return repository.Insert(entities) > 0;
        }
        public static bool UpdateIf<T>(this IRepository<T> repository, T entity) where T : class
        {
            return repository.Update(entity) > 0;
        }
        public static bool UpdateIf<T>(this IRepository<T> repository, IEnumerable<T> entities) where T : class
        {
            return repository.Update(entities) > 0;
        }
        public static bool DeleteIf<T>(this IRepository<T> repository, T entity) where T : class
        {

            return repository.Delete(entity) > 0;
        }
        public static bool DeleteIf<T>(this IRepository<T> repository, IEnumerable<T> entities) where T : class
        {
            return repository.Delete(entities) > 0;
        }
    }
}
