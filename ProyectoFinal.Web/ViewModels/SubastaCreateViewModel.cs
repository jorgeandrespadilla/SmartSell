using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class SubastaCreateViewModel
    {
        [Required(ErrorMessage ="El nombre del producto es necesario")]
        public String NombreProducto { get; set; }
        [Required(ErrorMessage = "La descripción del producto es necesaria")]
        public String DescripcionProducto { get; set; }
        [Required(ErrorMessage = "La imagen del producto es necesaria")]
        public HttpPostedFileBase Imagen { get; set; }
        [Required(ErrorMessage = "El precio inicial es obligatorio")]
        [Range(0, float.MaxValue, ErrorMessage ="Ingrese un valor positivo")]
        public float PrecioInicial { get; set; }

        [Required(ErrorMessage = "La fecha límite del producto es necesaria")]
        [DataType(DataType.Date)]
        public DateTime fechaLimite { get; set; }
    }
}