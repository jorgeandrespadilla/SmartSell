using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class PerfilDto
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public float AvgRating { get; set; }

        public PerfilDto(string nombres, string apellidos, string correo, float avgRating)
        {
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            AvgRating = avgRating;
        }
    }
}
