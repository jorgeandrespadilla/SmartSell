using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class SubastaCreateViewModel
    {
        [Required]
        public String NombreProducto { get; set; }
        [Required]
        public String DescripcionProducto { get; set; }
        [Required]
        public HttpPostedFileBase Imagen { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage ="Ingrese un valor positivo")]
        public float PrecioInicial { get; set; }
    }
}