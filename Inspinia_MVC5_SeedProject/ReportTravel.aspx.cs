using ERP_GMEDINA.Controllers;
using ERP_GMEDINA.Models;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP_GMEDINA.Views.Travel.Reports
{
    public partial class ReportTravel : System.Web.UI.Page
    {
        private readonly TravelController travelController = new TravelController();
        protected void Page_Load(object sender, EventArgs e)
        {

            string transporter_ID = Request.QueryString["transporter_ID"];
            string initialDate = Request.QueryString["initialDate"];
            string finalDate = Request.QueryString["finalDate"];


            ReportParameter[] reportParameters = new ReportParameter[2];
            reportParameters[0] = new ReportParameter("Initial_Date", initialDate);
            reportParameters[1] = new ReportParameter("Final_Date", finalDate);


            if (!IsPostBack)
            {
                ReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ReportTravel.rdlc");
                ReportViewer.LocalReport.SetParameters(reportParameters);

                var reportData = travelController.GetReportData(Convert.ToInt32(transporter_ID), initialDate, finalDate); // Retrieve your data source
                ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsReportTravel", reportData));

                ReportViewer.LocalReport.Refresh();
            }
        }

        
    }
}