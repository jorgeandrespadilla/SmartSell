using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProyectoFinal.Web.Models;

namespace ProyectoFinal.Web.ViewModels
{
    public class PerfilViewModel
    {

        public Usuario Usuario { get; set; }

        public int Rating { get; set; }

        public ICollection<Oferta> misSubastas { get; set; }


    }
}