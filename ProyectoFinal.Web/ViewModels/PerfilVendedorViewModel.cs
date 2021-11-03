using ProyectoFinal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class PerfilVendedorViewModel
    {
        public int UsuarioCalificadoId { get; set; }
        public Usuario Usuario { get; set; } 
        public float AvgRating { get; set; }
        public String Rating { get; set; }

    }
}