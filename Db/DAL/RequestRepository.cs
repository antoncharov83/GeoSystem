using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GeoSystem.Db.DAL;
using GeoSystem.Models;

namespace GeoSystem.Db.DAO
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(DbContext context) : base(context) { }
        public Brigade GetBrigade(int id) {
            return Get(id)?.Brigade;
        }

        protected override DbSet<Request> GetTable()
        {
            return (GeoSystemContext as GeoSystemContext).request;
        }
    }
}