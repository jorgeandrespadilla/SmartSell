using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class OfertaDto
    {
        public int UsuarioID { get; set; }
        public int OfertaID { get; set; }
        public int SubastaID { get; set; }
        public string Nombre { get; set; } // NombreOfertante o NombreProducto
        public float Monto { get; set; }
        public DateTime Fecha { get; set; }

        public OfertaDto(int usuarioID,int ofertaID, int subastaID, string nombre, float monto, DateTime fecha)
        {
            UsuarioID = usuarioID;
            OfertaID = ofertaID;
            SubastaID = subastaID;
            Nombre = nombre;
            Monto = monto;
            Fecha = fecha;
        }
    }
}
