using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jtf_Project.Admin.Models
{
    public class TruckReportModel
    {
        public DateTime Date{ get; set; }
        public string TruckName { get; set; }
        public string Destination { get; set; }
        public decimal Gross { get; set; }
        public decimal Less { get; set; }
        public decimal Fuel { get; set; }
        public decimal OtherExpenses { get; set; }
        public decimal NetIncome { get; set; }
        public int SaleId { get; set; }
    }


    public class OtherExpenseReportModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int SaleId { get; set; }

    }
}