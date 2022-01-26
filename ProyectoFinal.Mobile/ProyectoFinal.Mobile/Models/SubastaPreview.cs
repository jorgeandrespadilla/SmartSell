using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Models
{
    public class SubastaPreview
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreProducto { get; set; }
        public ImageSource Imagen { get; set; }

        public SubastaPreview(int iD, int usuarioID, string nombreProducto, ImageSource imagen)
        {
            ID = iD;
            UsuarioID = usuarioID;
            NombreProducto = nombreProducto;
            Imagen = imagen;
        }
    }
}
