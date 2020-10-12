using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jtf_Project.Data.Entities
{
    public class Truck
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#}",
                        ApplyFormatInEditMode = true)]
        public decimal Capacity { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }

        public Nullable<bool> Deleted { get; set; }

        [NotMapped]
        public string CapacityString
        {
            get
            {
                return this.Capacity > 1 ?string.Format("{0:0.#}",this.Capacity) + " tons"
                        : string.Format("{0:0.#}", this.Capacity) + " ton";
            }
        }

        [NotMapped]
        public string NameWithDriverName
        {
            get
            {
                return this.Name + " - " + this.Driver.FullName;
            }
        }
        [NotMapped]
        public SelectList DriverSelect
        {
            get
            {
                using (UnitOfWork Context = new UnitOfWork())
                {
                    return new SelectList(Context.DriverRepo.Get(s => s.Deleted == false).OrderBy(o => o.FirstName).ToList(),
                    "Id", "FullName", this.DriverId);
                }
            }
        }
    }
}
