using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.BookShop.IDAl
{
    public interface IBaseDAL<T> where T : class,new()
    {
        IQueryable<T> LoadEntities(Func<T, bool> whereLamda);

        IQueryable<T> LoadPagedEntities<s>(int pageIndex, int pageSize, out int pageCount, Func<T, bool> whereLamda, Func<T, s> orderByLamda, bool isAsc);

        T AddEntity(T entity);

        bool DeleteEntity(T entity);

        bool UpdateEntity(T entity);
    }
}
