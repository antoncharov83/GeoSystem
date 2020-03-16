using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSystem.Db.DAO
{
    public interface IRepository<T> : IDisposable
         where T : class
    {
        T Get(int? id);
        List<T> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
