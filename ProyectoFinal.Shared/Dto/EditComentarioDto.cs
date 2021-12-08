using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class EditComentarioDto : CreateComentarioDto
    {
        public string Descripcion { get; set; }

        public EditComentarioDto(string descripcion)
        {
            Descripcion = descripcion;
        }
    }
}
