using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.UWP.Models
{
    public class RatingUsuario
    {
        public int RatingUsuarioID { get; set; }
        public int? UsuarioCalificadoID { get; set; }
        public int? UsuarioCalificadorID { get; set; }
        public int Rating { get; set; }
        public Usuario UsuarioCalificado { get; set; }
        public Usuario UsuarioCalificador { get; set; }
    }
}
