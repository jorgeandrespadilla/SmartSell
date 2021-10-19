using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace ProyectoFinal.Web.Models
{
    public class Producto
    {
        [Key, ForeignKey("Subasta")]
        public int ProductoID { get; set; }
        public string Nombre{ get; set; }
        public string Descripcion{ get; set; }
        public string FotoUrl { get; set; }
        public virtual Subasta Subasta { get; set; }
    }
}