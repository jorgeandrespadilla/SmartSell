using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class ComentarioDto
    {
        public int ComentarioID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int SubastaID { get; set; }

        public ComentarioDto(int comentarioID, int usuarioID, string nombreUsuario, string descripcion, DateTime fechaCreacion, int subastaID)
        {
            ComentarioID = comentarioID;
            UsuarioID = usuarioID;
            NombreUsuario = nombreUsuario;
            Descripcion = descripcion;
            FechaCreacion = fechaCreacion;
            SubastaID = subastaID;
        }
    }
}
