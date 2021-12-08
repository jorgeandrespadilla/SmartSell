using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class CreateOfertaDto
    {
        public int UsuarioID { get; set; }
        public int SubastaID { get; set; }
        public float Monto { get; set; }

        public CreateOfertaDto(int usuarioID, int subastaID, float monto)
        {
            UsuarioID = usuarioID;
            SubastaID = subastaID;
            Monto = monto;
        }
    }
}
