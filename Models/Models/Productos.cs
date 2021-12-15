using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models
{
    [Table("Productos")]
   public class Productos
    {
        [Key]
        public int Id { get; set; }
        public string Producto { get; set; }
        public double PrecioVenta { get; set; }
        public double PrecioCompra { get; set; }
        public int Stock { get; set; }

    }
}
