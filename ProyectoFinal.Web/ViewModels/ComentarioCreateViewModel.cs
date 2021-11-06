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
        public int SubastaId { get; set; }
        [Required]
        public string DescripcionComentario { get; set; }
    }
}