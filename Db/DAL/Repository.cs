using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace GeoSystem.Db.DAO
{
    public abstract class Repository<T> : IRepository<T>
        where T : class
    {
        protected readonly DbContext GeoSystemContext;

        protected Repository(DbContext context) {
            GeoSystemContext = context;
            GeoSystemContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public void Create(T item)
        {
            GetTable().Add(item);
        }

        public void Delete(int id)
        {
            T item = Get(id);
            if(item != null)
                GetTable().Remove(item);
        }
        public T Get(int? id)
        {
            return GetTable().Find(id);
        }

        public List<T> GetAll()
        {
            return GetTable().ToList();
        }

        public void Save()
        {
            GeoSystemContext.SaveChanges();
        }

        public void Update(T item)
        {
            //GeoSystemContext.Entry(item).State = EntityState.Modified;
            GetTable().AddOrUpdate(item);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    GeoSystemContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract DbSet<T> GetTable();
    }
}