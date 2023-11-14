using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{

    [MetadataType(typeof(SubsidiaryMetaData))]

    public partial class tbSubsidiary
    {

    }


    public class SubsidiaryMetaData
    {

        [Display(Name = "Nombre de la Subsidiaria")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string subsidiary_Name { get; set; }

        [Display(Name = "Direccion de la Subsidiaria")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string subsidiary_Direction { get; set; }
    }
}