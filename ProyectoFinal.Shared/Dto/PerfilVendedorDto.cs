using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class PerfilVendedorDto
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public float AvgRating { get; set; }
        public int Rating { get; set; }

        public PerfilVendedorDto(int iD, string nombres, string apellidos, string correo, float avgRating, int rating)
        {
            ID = iD;
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            AvgRating = avgRating;
            Rating = rating;
        }
    }
}
