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
    public class Sale
    {
        public int Id { get; set; }
        public int RateId { get; set; }
        public decimal Gross { get; set; }
        public decimal FuelCost { get; set; }
        public decimal Less { get; set; }
        public decimal Net { get; set; }
        [Required]
        public Nullable<DateTime> Date { get; set; }

        [ForeignKey("RateId")]
        public virtual Rate Rate { get; set; }
        //The user who enter a sales record
        public string UserId { get; set; }

        [NotMapped]
        public string LessFormattedString {
            get
            {
                string result = string.Empty;

                if (this.Rate != null)
                {
                    result = string.Format("{0:0.00#}", this.Rate.HelperCost + this.Rate.WaterCost + this.Rate.DriverCost);
                }
                return result;
            }
        }


        [NotMapped]
        public SelectList RateSelect
        {
            get
            {
                using (UnitOfWork Context = new UnitOfWork())
                {
                    return new SelectList(Context.RateRepo.Get(s => s.Deleted == false).OrderBy(o => o.RateName).ToList(),
                    "Id", "RateName", this.RateId);
                }
            }
        }

        public void CalculateThenSet()
        {
            using (UnitOfWork Context = new UnitOfWork())
            {
                var rate = Context.RateRepo.GetByID(this.RateId);
                decimal lessTotal = 0;
                if (rate != null)
                {
                    this.Gross = rate.Amount;
                    this.Less = Convert.ToDecimal(rate.DriverCost + rate.HelperCost + rate.WaterCost);

                    lessTotal += this.Less + this.FuelCost;
                }

                var otherExpences = Context.OtherExpenceRepo.Get(s => s.SaleId == this.Id);
                foreach (var exp in otherExpences)
                {
                    lessTotal += exp.Amount;
                }


                this.Net = this.Gross - lessTotal;
            }
        }

        public virtual ICollection<OtherExpence> OtherExpences { get; set; }
    }
}
