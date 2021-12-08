using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class CreateComentarioDto
    {
        public int UsuarioID { get; set; }
        public int SubastaID { get; set; }
        public string Descripcion { get; set; }

        public CreateComentarioDto(int usuarioID, int subastaID, string descripcion)
        {
            UsuarioID = usuarioID;
            SubastaID = subastaID;
            Descripcion = descripcion;
        }
    }
}
