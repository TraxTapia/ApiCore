using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models.SistemaWeb
{
    public class Rol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Rol()
        {
            Usuarios = new HashSet<Usuarios>();
        }
        [Key]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [Required]
        [StringLength(60)]
        public string Descripcion { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaRegistro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Usuarios> Usuarios { get; set; }

    }
}
