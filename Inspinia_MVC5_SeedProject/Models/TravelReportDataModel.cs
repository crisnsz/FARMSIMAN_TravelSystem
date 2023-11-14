using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    public class TravelReportDataModel
    {
        public int Travel_ID { get; set; }

        public DateTime Departure_Date_and_Time { get; set; }

        public decimal Total_distance_Kilometers { get; set; }
        public decimal Total_travel_Cost_per_Date { get; set; }
        public decimal TotalTravelCost { get; set; }
        public string Subsidiary_Name { get; set; }
        public string Subsidiary_Direction { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Direction { get; set; }
        public decimal Distance_Kilometers { get; set; }
        public string Transporter_Name { get; set; }
        public decimal Transporter_Fee { get; set; }

        public DateTime Initial_Date { get; set; }
        public DateTime Final_Date { get; set; }

    }
}