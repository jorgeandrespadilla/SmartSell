using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.UWP.Models
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
        public Usuario Usuario { get; set; }
    }
}
