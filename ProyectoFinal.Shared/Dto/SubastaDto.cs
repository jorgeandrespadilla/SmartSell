using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class SubastaDto
    {
        public int SubastaID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string NombreVendedor { get; set; }
        public float MontoActual { get; set; }
        public DateTime Fecha { get; set; }

        public bool Vigente { get; set; }
        public string UriImagen { get; set; }

        public ICollection<OfertaDto> Ofertas { get; set; }
        public ICollection<ComentarioDto> Comentarios { get; set; }

        public SubastaDto(int subastaID, int usuarioID, string nombreProducto, string descripcionProducto, string nombreVendedor, float montoActual, DateTime fecha, bool vigente, string uriImagen, ICollection<OfertaDto> ofertas, ICollection<ComentarioDto> comentarios)
        {
            SubastaID = subastaID;
            UsuarioID = usuarioID;
            NombreProducto = nombreProducto;
            DescripcionProducto = descripcionProducto;
            NombreVendedor = nombreVendedor;
            MontoActual = montoActual;
            Fecha = fecha;
            Vigente = vigente;
            UriImagen = uriImagen;
            Ofertas = ofertas;
            Comentarios = comentarios;
        }
    }
}
