using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Models.SistemaWeb
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required]
        [StringLength(60)]
        public string Descripcion { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaRegistro { get; set; }

    }
}
