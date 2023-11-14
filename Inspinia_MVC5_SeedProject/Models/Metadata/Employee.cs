using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class tbEmployee
    {

    }

    public class EmployeeMetaData
    {
       
        [Display(Name = "Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string employee_Name { get; set; }

        [Display(Name = "Dirección")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string employee_Direction { get; set; }

    }
}