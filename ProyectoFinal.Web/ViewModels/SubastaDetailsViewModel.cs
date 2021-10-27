using ProyectoFinal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class SubastaDetailsViewModel
    {
        public ICollection<Oferta> OfertasSubasta { get; set; }
        public Subasta Subasta { get; set; }
        public float MontoActual { get; set; }
        public bool Vigente { get; set; }
    }
}