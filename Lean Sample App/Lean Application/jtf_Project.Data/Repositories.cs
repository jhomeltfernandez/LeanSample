using jtf_Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.Repositories
{
    public class DestinationRepo : RepositoryBase<Destination>
    {
        public DestinationRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class DriverRepo : RepositoryBase<Driver>
    {
        public DriverRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class OtherExpenceRepo : RepositoryBase<OtherExpence>
    {
        public OtherExpenceRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class RateRepo : RepositoryBase<Rate>
    {
        public RateRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class SaleRepo : RepositoryBase<Sale>
    {
        public SaleRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class TruckRepo : RepositoryBase<Truck>
    {
        public TruckRepo(jtf_ProjectContext context) : base(context) { }
    }
    public class LayoutSettingRepo : RepositoryBase<LayoutSetting>
    {
        public LayoutSettingRepo(jtf_ProjectContext context) : base(context) { }
    }

    public class LayoutSettingsRepo : RepositoryBase<LayoutSetting>
    {
        public LayoutSettingsRepo(jtf_ProjectContext context) : base(context) { }
    }

    public class UserProfileRepo : RepositoryBase<UserProfile>
    {
        public UserProfileRepo(jtf_ProjectContext context) : base(context) { }
    }

    public class UserImageRepo : RepositoryBase<UserImage>
    {
        public UserImageRepo(jtf_ProjectContext context) : base(context) { }
    }
}
