using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ProyectoFinal.Web.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre{ get; set; }
        public string Descripcion{ get; set; }
        public string FotoUrl { get; set; }
        public virtual Subasta Subasta { get; set; }
    }
}