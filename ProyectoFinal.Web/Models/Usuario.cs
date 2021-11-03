using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<Subasta> Subastas { get; set; }
        public virtual ICollection<Oferta> Ofertas { get; set; }
        [InverseProperty("UsuarioCalificado")]
        public virtual ICollection<RatingUsuario> RatingsUsuarioCalificado { get; set; }
        [InverseProperty("UsuarioCalificador")]
        public virtual ICollection<RatingUsuario> RatingsUsuarioCalificador { get; set; }
    }
}