using jtf_Project.Admin.Models;
using jtf_Project.Data;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace jtf_Project.Admin.Reports
{
    public partial class TruckSaleReport : System.Web.UI.Page
    {
        private UnitOfWork context = new UnitOfWork();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int truckid = Request.QueryString["truckid"] != null ? Convert.ToInt32(Request.QueryString["truckid"]) : 0;
                Nullable<DateTime> startDate = Request.QueryString["startDate"] != null ? (Nullable<DateTime>)Convert.ToDateTime(Request.QueryString["startDate"]) : null;
                Nullable<DateTime> endDate = Request.QueryString["endDate"] != null ? (Nullable<DateTime>)Convert.ToDateTime(Request.QueryString["endDate"]) : null;

                using (UnitOfWork context = new UnitOfWork())
                {
                    var truck = context.TruckRepo.GetByID(truckid);

                    var sales = context.SaleRepo.Get(s => s.Rate.TruckId == truckid).ToList();

                    string startDatestring = startDate.HasValue ? startDate.Value.ToString("MMM dd,yyyy") : string.Empty;
                    string endDateString = endDate.HasValue ? endDate.Value.ToString("MMM dd,yyyy") : string.Empty;

                    string reportDateString = string.Empty;

                    if (startDate.HasValue && !endDate.HasValue)
                    {
                        reportDateString = "Date " + startDatestring;

                        sales = sales.Where(s => s.Date.Value == startDate.Value).ToList();
                    }
                    else if (startDate.HasValue && endDate.HasValue)
                    {
                        reportDateString = "From " + startDatestring + " to " + startDatestring;

                        sales = sales.Where(s => s.Date.Value >= startDate.Value && s.Date.Value <= endDate.Value).ToList();
                    }

                    var param1 = new ReportParameter();
                    param1.Name = "TName";
                    param1.Values.Add(truck.Name);
                    ReportViewer1.LocalReport.SetParameters(param1);

                    var param2 = new ReportParameter();
                    param2.Name = "SlctdDate";
                    param2.Values.Add(reportDateString);
                    ReportViewer1.LocalReport.SetParameters(param2);


                    List<TruckReportModel> truckReports = new List<TruckReportModel>();
                    //List<OtherExpenseReportModel> otherExpReports = new List<OtherExpenseReportModel>();

                    foreach (var sale in sales)
                    {
                        TruckReportModel trm = new TruckReportModel()
                        {
                            Date = sale.Date.Value,
                            TruckName = sale.Rate.Truck.Name,
                            Destination = sale.Rate.Destination.Name,
                            Gross = sale.Gross,
                            Less = sale.Less,
                            Fuel = sale.FuelCost,
                            OtherExpenses = sale.OtherExpences.Sum(s => s.Amount),
                            NetIncome = sale.Net,
                            SaleId = sale.Id
                        };

                        //foreach (var oex in sale.OtherExpences)
                        //{
                        //    OtherExpenseReportModel oerm = new OtherExpenseReportModel()
                        //    {
                        //        Name = oex.Name,
                        //        Amount = oex.Amount,
                        //        SaleId = sale.Id
                        //    };

                        //    otherExpReports.Add(oerm);
                        //}


                        truckReports.Add(trm);
                    }

                    ReportDataSource truckReportSource = new ReportDataSource("DataSet1", truckReports);
                    //ReportDataSource otherExpenseReportSource = new ReportDataSource("DataSet2", otherExpReports);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(truckReportSource);


                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OtherExpenses);

                    ReportViewer1.LocalReport.Refresh();
                }

            }
        }


        void OtherExpenses(object sender, SubreportProcessingEventArgs e)
        {
            int saleId = 0;

            int.TryParse(e.Parameters["SaleId"].Values[0], out saleId);

            string dataSourceName = e.DataSourceNames[0];

            IEnumerable<OtherExpenseReportModel> otherexpenses = context.OtherExpenceRepo.Get(s => s.SaleId == saleId).Select(x => jtf_Project.Admin.Models.ModelFactory.ToReportModel(x));

            e.DataSources.Add(new ReportDataSource(dataSourceName, otherexpenses));
        }
    }
}