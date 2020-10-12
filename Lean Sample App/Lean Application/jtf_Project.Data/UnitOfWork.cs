using jtf_Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly jtf_ProjectContext context = new jtf_ProjectContext();
        private bool disposed;

        private TruckRepo _TruckRepo;
        private SaleRepo _SaleRepo;
        private RateRepo _RateRepo;
        private OtherExpenceRepo _OtherExpenceRepo;
        private DriverRepo _DriverRepo;
        private DestinationRepo _DestinationRepo;


        public DestinationRepo DestinationRepo
        {
            get
            {
                if (_DestinationRepo == null)
                {
                    _DestinationRepo = new DestinationRepo(context);
                }
                return _DestinationRepo;
            }
        }

        public DriverRepo DriverRepo
        {
            get
            {
                if (_DriverRepo == null)
                {
                    _DriverRepo = new DriverRepo(context);
                }
                return _DriverRepo;
            }
        }
        public OtherExpenceRepo OtherExpenceRepo
        {
            get
            {
                if (_OtherExpenceRepo == null)
                {
                    _OtherExpenceRepo = new OtherExpenceRepo(context);
                }
                return _OtherExpenceRepo;
            }
        }
        public RateRepo RateRepo
        {
            get
            {
                if (_RateRepo == null)
                {
                    _RateRepo = new RateRepo(context);
                }
                return _RateRepo;
            }
        }
        public SaleRepo SaleRepo
        {
            get
            {
                if (_SaleRepo == null)
                {
                    _SaleRepo = new SaleRepo(context);
                }
                return _SaleRepo;
            }
        }
        public TruckRepo TruckRepo
        {
            get
            {
                if (_TruckRepo == null)
                {
                    _TruckRepo = new TruckRepo(context);
                }
                return _TruckRepo;
            }
        }



        #region User Management
        private LayoutSettingRepo _LayoutSettingRepo;
        private UserProfileRepo _UserProfileRepo;
        private UserImageRepo _UserImageRepo;

        public UserImageRepo UserImageRepo
        {
            get
            {
                if (_UserImageRepo == null)
                {
                    _UserImageRepo = new UserImageRepo(context);
                }
                return _UserImageRepo;
            }
        }
        public LayoutSettingRepo LayoutSettingRepo
        {
            get
            {
                if (_LayoutSettingRepo == null)
                {
                    _LayoutSettingRepo = new LayoutSettingRepo(context);
                }
                return _LayoutSettingRepo;
            }
        }

        public UserProfileRepo UserProfileRepo
        {
            get
            {
                if (_UserProfileRepo == null)
                {
                    _UserProfileRepo = new UserProfileRepo(context);
                }
                return _UserProfileRepo;
            }
        }
        #endregion
        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
