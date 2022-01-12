using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Mobile.Models
{
    public class Comentario
    {
        public int ComentarioID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool IsAuthor { get; set; }

        public Comentario(int comentarioID, int usuarioID, string nombreUsuario, string descripcion, DateTime fechaCreacion, bool isAuthor)
        {
            ComentarioID = comentarioID;
            UsuarioID = usuarioID;
            NombreUsuario = nombreUsuario;
            Descripcion = descripcion;
            FechaCreacion = fechaCreacion;
            IsAuthor = isAuthor;
        }
    }
}
