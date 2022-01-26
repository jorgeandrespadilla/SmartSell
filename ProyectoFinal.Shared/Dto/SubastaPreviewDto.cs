using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class SubastaPreviewDto
    {
        public int SubastaID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreProducto { get; set; }
        public string UriImagen { get; set; }

        public SubastaPreviewDto(int subastaID, int usuarioID, string nombreProducto, string uriImagen)
        {
            SubastaID = subastaID;
            UsuarioID = usuarioID;
            NombreProducto = nombreProducto;
            UriImagen = uriImagen;
        }
    }
}
