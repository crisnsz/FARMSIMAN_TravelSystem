using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(TransporterMetaData))]
    public partial class tbTransporter
    {

    }

    public class TransporterMetaData
    {

        [Display(Name = "Id del Transportista")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public int transporter_ID { get; set; }

        [Display(Name = "Transportista")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string transporter_Name { get; set; }

        [Display(Name = "Tarifa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public decimal transporter_Fee { get; set; }
    }

}