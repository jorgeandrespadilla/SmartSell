using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class CreateSubastaDto
    {
        public int UsuarioID { get; set; }
        public String NombreProducto { get; set; }
        public String DescripcionProducto { get; set; }
        public string UriImagen { get; set; }
        public float PrecioInicial { get; set; }
        public DateTime FechaLimite { get; set; }

        public CreateSubastaDto(int usuarioID, string nombreProducto, string descripcionProducto, string uriImagen, float precioInicial, DateTime fechaLimite)
        {
            UsuarioID = usuarioID;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            UriImagen = uriImagen;
            PrecioInicial = precioInicial;
            FechaLimite = fechaLimite;
        }
    }
}
