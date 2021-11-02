using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class SubastaEditViewModel
    {
        public int SubastaID { get; set; }
        [Required(ErrorMessage ="El nombre del producto es necesario")]
        public String NombreProducto { get; set; }
        [Required(ErrorMessage = "La descripción del producto es necesaria")]
        public String DescripcionProducto { get; set; }
        public String FotoUrlProducto { get; set; }
        public HttpPostedFileBase Imagen { get; set; }
    }
}