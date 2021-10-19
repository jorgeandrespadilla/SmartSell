using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace ProyectoFinal.Web.Models
{
    public class Subasta
    {
        public int SubastaID { get; set; }
        public int? UsuarioID { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string FotoUrlProducto { get; set; }
        public float PrecioInicial { get; set; }
        public DateTime FechaLimite { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Oferta> Ofertas { get; set; }
    }
}