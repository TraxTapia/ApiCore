using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models.SistemaWeb
{
    [Table("Usuarios")]
    public class Usuarios
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(250)]
        public string ApellidoPaterno { get; set; }

        [Required]
        [StringLength(250)]
        public string ApellidoMaterno { get; set; }

        [Required]
        [StringLength(250)]
        public string Correo { get; set; }

        [Required]
        [StringLength(250)]
        public string Contrasena { get; set; }

        public int IdRol { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool Activo { get; set; }

        //public virtual Rol Rol { get; set; }
    }
}
