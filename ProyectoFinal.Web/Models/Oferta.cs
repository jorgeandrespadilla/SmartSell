using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.Models
{
    public class Oferta
    {
        public int OfertaID { get; set; }
        public int UsuarioID { get; set; }
        public int SubastaID { get; set; }
        public float Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Subasta Subasta { get; set; }
    }
}