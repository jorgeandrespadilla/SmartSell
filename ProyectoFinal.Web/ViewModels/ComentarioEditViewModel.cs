using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class ComentarioEditViewModel
    {
        public int SubastaId { get; set; }
        public int ComentarioId { get; set; }
        [Required]
        public string DescripcionComentario { get; set; }
    }
}