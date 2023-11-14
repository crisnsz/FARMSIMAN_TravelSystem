using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(TravelMetaData))]

    public partial class tbTravel
    {

    }

    public class TravelMetaData
    {
        [Display(Name = "Subsidiaria")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int subsidiary_ID { get; set; }

        [Display(Name = "Transportista")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int transporter_ID { get; set; }

        [Display(Name = "Empleado")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int employee_ID { get; set; }

        [Display(Name = "Fecha y hora de salida")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public System.DateTime departure_Date_and_Time { get; set; }

        [Display(Name = "Distancia Total del Viaje")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public decimal distance_Kilometers { get; set; }

        [Display(Name = "Coste Total del Viaje")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public decimal total_travel_Cost { get; set; }

    }
}