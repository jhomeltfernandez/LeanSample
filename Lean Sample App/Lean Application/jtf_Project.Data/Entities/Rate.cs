using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jtf_Project.Data.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public int TruckId { get; set; }
        public decimal Amount { get; set; }
        public decimal DriverCost { get; set; }
        public decimal HelperCost { get; set; }
        public decimal WaterCost { get; set; }
        [ForeignKey("DestinationId")]
        public virtual Destination Destination { get; set; }
        [ForeignKey("TruckId")]
        public virtual Truck Truck { get; set; }
        public Nullable<bool> Deleted { get; set; }

        [NotMapped]
        public string RateName {
            get
            {
                string result = string.Empty;
                using (UnitOfWork Context = new UnitOfWork())
                {
                    var destination = Context.DestinationRepo.GetByID(this.DestinationId);

                    result = this.Truck.Name + " - " + destination.Name + " - " + string.Format("{0:0.00#}", this.Amount);

                }
                return result;
            }
        }  

        [NotMapped]
        public SelectList DestinationSelect {
            get
            { 
                using(UnitOfWork Context = new UnitOfWork())
                {
                    return new SelectList(Context.DestinationRepo.Get(s => s.Deleted == false).OrderBy(o => o.Name).ToList(),
                    "Id", "Name", this.DestinationId);
                }
            }
        }
        [NotMapped]
        public SelectList TruckSelect
        {
            get
            {
                using (UnitOfWork Context = new UnitOfWork())
                {
                    return new SelectList(Context.TruckRepo.Get(s => s.Deleted == false).OrderBy(o => o.Name).ToList(),
                    "Id", "Name", this.TruckId);
                }
            }
        }

        public virtual ICollection<Sale> Sales { get; set; }

    }
}
