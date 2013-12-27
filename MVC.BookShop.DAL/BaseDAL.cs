using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC.BookShop.Model;
using MVC.BookShop.IDAl;

namespace MVC.BookShop.DAL
{
    public class BaseDAL<T> where T : class,new()
    {
        book_shopEntities db = new book_shopEntities();

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="whereLamda">条件</param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Func<T, bool> whereLamda)
        {
            return db.CreateObjectSet<T>().Where<T>(whereLamda).AsQueryable();
        }

        /// <summary>
        /// 分页加载数据
        /// </summary>
        /// <typeparam name="s">排序字段的类型</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录大小</param>
        /// <param name="totalCount">总共页数</param>
        /// <param name="whereLambda">条件Lamda表达式</param>
        /// <param name="orderbyLambda">排序Lamda表达式</param>
        /// <param name="isAsc">true 升序，false 降序</param>
        /// <returns></returns>
        public IQueryable<T> LoadPagedEntities<s>(int pageIndex, int pageSize, out int pageCount, Func<T, bool> whereLamda, Func<T, s> orderByLamda, bool isAsc)
        {
            var temp = db.CreateObjectSet<T>().Where<T>(whereLamda);
            pageCount = temp.Count();
            if (isAsc)
            {
                temp.OrderBy<T, s>(orderByLamda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp.OrderByDescending<T, s>(orderByLamda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp.AsQueryable();
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            db.CreateObjectSet<T>().AddObject(entity);
            db.SaveChanges();
            return entity;//不会将新生成的id放到entity中
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            db.CreateObjectSet<T>().Attach(entity);
            db.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Deleted);
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity)
        {
            db.CreateObjectSet<T>().Attach(entity);
            db.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Modified);
            return db.SaveChanges() > 0;
        }
    }
}
