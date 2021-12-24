using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ProyectoFinal.UWP.Models
{
    public class SubastaItem
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public BitmapImage Imagen { get; set; }
        public string NombreProducto { get; set; }
        public float MontoActual { get; set; }
        public DateTime Fecha { get; set; }
        public bool Vigente { get; set; }

        public SubastaItem(int id, int usuarioID, BitmapImage imagen, string nombreProducto, float montoActual, DateTime fecha, bool vigente)
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
