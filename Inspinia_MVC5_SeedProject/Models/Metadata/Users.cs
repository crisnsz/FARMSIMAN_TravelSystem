using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class tbUser
    {

    }

    public class UserMetaData
    {
        [Display(Name = "Nombre de Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public string user_Username { get; set; }


        [Display(Name = "Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*El campo {0} es requerido")]
        public byte[] user_Password { get; set; }


        [Display(Name = "Activo?")]
        public bool user_IsActive { get; set; }

        [Display(Name = "Es Administrador?")]
        public bool user_IsAdmin { get; set; }
    }
}