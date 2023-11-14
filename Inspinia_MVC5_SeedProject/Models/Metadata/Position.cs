using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(PositionMetaData))]
    public partial class tbPosition
    {

    }

    public class PositionMetaData
    {
        [Display(Name = "Posicion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string position_Name { get; set; }
    }
}