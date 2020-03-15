using GeoSystem.Db;
using GeoSystem.Db.DAL;
using GeoSystem.Db.DAO;
using Ninject.Modules;
using System.Data.Entity;

namespace GeoSystem.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IRequestRepository>().To<RequestRepository>();
            Bind<IBrigadeRepository>().To<BrigadeRepository>();
            Bind<DbContext>().To<GeoSystemContext>().InSingletonScope();
        }
    }
}