using GeoSystem.Db.DAO;
using GeoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSystem.Db.DAL
{
    public interface IRequestRepository : IRepository<Request>
    {
        Brigade GetBrigade(int id);

        List<Request> GetAllWithBrigade();
    }
}
