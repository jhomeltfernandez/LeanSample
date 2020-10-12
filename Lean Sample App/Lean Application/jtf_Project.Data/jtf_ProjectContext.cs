using jtf_Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data
{
    public class jtf_ProjectContext : DbContext
    {
        public jtf_ProjectContext():base("jtf_ProjectConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<UserImage> UserImage { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<LayoutSetting> LayoutSetting { get; set; }

        public DbSet<Driver> Driver { get; set; }
        public DbSet<Destination> Destination { get; set; }
        public DbSet<OtherExpence> OtherExpence { get; set; }
        public DbSet<Rate> Rate { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Truck> Truck { get; set; }
    }
}
