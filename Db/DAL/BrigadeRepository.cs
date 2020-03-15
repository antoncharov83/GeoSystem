using GeoSystem.Db.DAL;
using GeoSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeoSystem.Db.DAO
{
    public class BrigadeRepository : Repository<Brigade>, IBrigadeRepository
    {
        public BrigadeRepository(DbContext context) : base(context)
        {
        }

        public ICollection<Request> GetRequests(int id)
        {
            return Get(id)?.Requests;
        }
        protected override DbSet<Brigade> GetTable()
        {
            return (GeoSystemContext as GeoSystemContext).brigade;
        }
    }
}