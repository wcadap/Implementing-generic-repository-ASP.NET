using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContactBook.Core.Repository
{
    public interface IGenericRepo : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        IQueryable<T> QueryIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
        T GetById<T>(int id) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(int id) where T : class;
        bool Save();
    }
}
