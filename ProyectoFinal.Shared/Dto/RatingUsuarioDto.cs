using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class RatingUsuarioDto
    {
        public int UsuarioCalificadoID { get; set; }
        public int UsuarioCalificadorID { get; set; }
        public int Rating { get; set; }

        public RatingUsuarioDto(int usuarioCalificadoID, int usuarioCalificadorID, int rating)
        {
            UsuarioCalificadoID = usuarioCalificadoID;
            UsuarioCalificadorID = usuarioCalificadorID;
            Rating = rating;
        }
    }
}
