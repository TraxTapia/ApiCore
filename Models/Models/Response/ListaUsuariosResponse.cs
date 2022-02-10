using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Response
{
    public class ListaUsuariosResponse
    {
        public int Id { get; set; } 
        public  string NombreCompleto { get; set; } 
        public string Correo { get; set; }  
        public string DescripcionRol { get; set; }  
        public bool Activo { get; set; }    
    }
}
