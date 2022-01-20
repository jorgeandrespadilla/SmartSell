using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Models
{
    public class SubastaItem
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public ImageSource Imagen { get; set; }
        public string NombreProducto { get; set; }
        public float MontoActual { get; set; }
        public DateTime Fecha { get; set; }
        public bool Vigente { get; set; }

        public SubastaItem(int id, int usuarioID, ImageSource imagen, string nombreProducto, float montoActual, DateTime fecha, bool vigente)
        {
            ID = id;
            UsuarioID = usuarioID;
            Imagen = imagen;
            NombreProducto = nombreProducto;
            MontoActual = montoActual;
            Fecha = fecha;
            Vigente = vigente;
        }
    }
}
