using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.Models
{
    public class RatingUsuario
    {
        public int RatingUsuarioID { get; set; }
        [ForeignKey("UsuarioCalificado")]
        public int? UsuarioCalificadoID { get; set; }
        [ForeignKey("UsuarioCalificador")]
        public int? UsuarioCalificadorID { get; set; }
        public int Rating { get; set; }
        public virtual Usuario UsuarioCalificado { get; set; }
        public virtual Usuario UsuarioCalificador { get; set; }
    }
}