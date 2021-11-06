using ProyectoFinal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class ComentarioCreateViewModel
    {
        public Subasta Subasta { get; set; }
        [Required]
        public string Comentario { get; set; }
        


    }
}