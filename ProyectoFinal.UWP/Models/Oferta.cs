using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.UWP.Models
{
    public class Oferta
    {
        public int OfertaID { get; set; }
        public int UsuarioID { get; set; }
        public int SubastaID { get; set; }
        public float Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Usuario Usuario { get; set; }
        public Subasta Subasta { get; set; }
        public IObservable<Subasta> Subastas { get; set; }
    }
}
