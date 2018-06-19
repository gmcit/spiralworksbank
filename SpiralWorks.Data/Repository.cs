using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpiralWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiralWorks.Data.Ef6
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException("DbContext is null");
            DbContext.Database.SetCommandTimeout(300);
            DbSet = dbContext.Set<T>();
        }


        #region IRepository<T>
        public void Add(T entity)
        {
            SetProperty(entity, "CreatedDate", DateTime.UtcNow);


            EntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }
        public void AddRange(List<T> list)
        {
            list.ForEach(x =>
            {
                SetProperty(x, "CreatedDate", DateTime.UtcNow);

            });
            DbSet.AddRange(list);
        }

        public void Delete(int id)
        {
            var entity = FindById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public IQueryable<T> FindAll()
        {
            return DbSet;
        }

        public T FindById(int id)
        {
            return DbSet.Find(id);
        }

        public void Update(T entity)
        {

            EntityEntry dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
        public List<T> Find(Func<T, bool> match)
        {
            return DbSet.Where(match).ToList();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        #endregion

        #region Helper
        private void SetProperty(T entity, string propertyName, object value)
        {
            if (entity.GetType().GetProperty(propertyName) != null)
            {
                entity.GetType().GetProperty(propertyName).SetValue(entity, value);
            }
        }
        #endregion
    }
}
