using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class AuthorizedUsuarioDto
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public AuthorizedUsuarioDto(int id, string nombre)
        {
            ID = id;
            Nombre = nombre;
        }
    }
}
