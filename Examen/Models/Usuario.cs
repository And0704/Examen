using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Usuario.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El ID de usuario es requerido.")]
        [StringLength(10, ErrorMessage = "El ID de usuario no puede tener más de 10 caracteres.")]
        public string id_user { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 10 caracteres.")]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Primer nombre es requerido")]
        public int first_name { get; set; }

        [Required(ErrorMessage = "Apellido es requerido")]
        public int last_name { get; set; }

        [Required(ErrorMessage = "")]
        public int active { get; set; }

        [Required(ErrorMessage = "El ID es requerido")]
        public int staff_id { get; set; }

        [Required(ErrorMessage = "El correo es requerido")]
        public int email { get; set; }
    }
}