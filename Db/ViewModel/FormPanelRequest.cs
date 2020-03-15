using GeoSystem.Db.DAL;
using GeoSystem.Db.DAO;
using GeoSystem.Models;
using GeoSystem.Util;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSystem.Db.ViewModel
{
    public class FormPanelRequest
    {
        public Request request { get; set; }
        static IBrigadeRepository repository;

        public static List<Brigade> getBrigades() {           
            var kernel = new StandardKernel(new NinjectRegistrations());
            repository = kernel.Get<IBrigadeRepository>();
            List<Brigade> brigades = repository.GetAll();
            return brigades;
        }
    }
}