using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.HelperModel
{
    public class GenericTableVModel
    {
        public List<GenericTableRowVmodel> Rows { get; set; }
        public GenericTableSetting Setting { get; set; }
    }

    //Row
    public class GenericTableRowVmodel : GenericTableBase
    {
        public string Id { get; set; }
        public List<GenericTableCellVmodel> Cells { get; set; }
        public GenericTableSetting Setting { get; set; }
    }

    //Cell
    public class GenericTableCellVmodel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


    public class GenericTableSetting : GenericTableBase
    {
        public GenericTableSetting()
        {
            this.Class = "table";
            this.CreateEditButtonURL = "/CreateEdit";
            this.DeleteButtonURL = "/Delete";
            this.IncludeActionColumn = true;
            this.IncludeRowNumber = false;
        }
        public string Id { get; set; }
        public string Class { get; set; }
        public string PropertiesNotToShow { get; set; }

        //public string ForceToId { get; set; }
    }

    public abstract class GenericTableBase
    {
        public string CreateEditButtonURL { get; set; }
        public string DeleteButtonURL { get; set; }
        public bool IncludeActionColumn { get; set; }
        public bool IncludeRowNumber { get; set; }
    }
}
