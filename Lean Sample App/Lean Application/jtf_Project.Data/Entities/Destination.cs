using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.Entities
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Deleted { get; set; }
    }
}
