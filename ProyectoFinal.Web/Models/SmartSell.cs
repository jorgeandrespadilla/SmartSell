using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ProyectoFinal.Web.Models
{
    public class SmartSell: DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<RatingUsuario> RatingUsuario { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Subasta> Subasta { get; set; }
        public DbSet<Oferta> Oferta { get; set; }
    }
}