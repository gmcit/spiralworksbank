using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiralWorks.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        T FindById(int id);
        List<T> Find(Func<T, bool> match);
        void Add(T entity);
        void AddRange(List<T> list);

        void Update(T entity);

        void Delete(int id);
        void Delete(T entity);

        void Commit();
    }
}
