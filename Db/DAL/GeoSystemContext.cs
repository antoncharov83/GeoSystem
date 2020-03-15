using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GeoSystem.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GeoSystem.Db
{
    public class GeoSystemContext : DbContext
    {
        public DbSet<Brigade> brigade { get; set; }
        public DbSet<Request> request { get; set; }
        public GeoSystemContext() : base("GeoDb")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        static GeoSystemContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GeoSystemContext>());
        }
    }
}