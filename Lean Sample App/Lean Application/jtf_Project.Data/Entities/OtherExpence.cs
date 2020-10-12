using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.Entities
{
    public class OtherExpence
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }

        public string NameFormatedString {
            get
            {
                return this.Name + " : " + string.Format("{0:0.00#}", this.Amount);
            }
        }
    }
}
