using GeoSystem.Db.DAO;
using GeoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSystem.Db.DAL
{
    public interface IBrigadeRepository : IRepository<Brigade>
    {
        ICollection<Request> GetRequests(int id);
    }
}
