using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class CredencialesUsuarioDto
    {
        public string Correo { get; set; }
        public string Clave { get; set; }

        public CredencialesUsuarioDto(string correo, string clave)
        {
            Correo = correo;
            Clave = clave;
        }
    }
}
