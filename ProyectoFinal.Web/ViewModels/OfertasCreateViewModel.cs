using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Web.ViewModels
{
    public class OfertaCreateViewModel
    {
        public int SubastaID { get; set; }
        [Required]
        public float Monto { get; set; }
    }
}
