using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class EditSubastaDto
    {
        public String NombreProducto { get; set; }
        public String DescripcionProducto { get; set; }
        public string UriImagen { get; set; }

        public EditSubastaDto(string nombreProducto, string descripcionProducto, string uriImagen)
        {
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            UriImagen = uriImagen;
        }
    }
}
