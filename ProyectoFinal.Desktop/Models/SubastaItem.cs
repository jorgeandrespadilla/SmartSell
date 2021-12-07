using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ProyectoFinal.Desktop.Models
{
    public class SubastaItem
    {
        public int ID { get; set; }

        public BitmapImage Imagen { get; set; }
        public string NombreProducto { get; set; }
        public float MontoActual { get; set; }
        public DateTime Fecha { get; set; }
        public bool Vigente { get; set; }

        public SubastaItem(int id, BitmapImage imagen, string nombreProducto, float montoActual, DateTime fecha, bool vigente)
        {
            ID = id;
            Imagen = imagen;
            NombreProducto = nombreProducto;
            MontoActual = montoActual;
            Fecha = fecha;
            Vigente = vigente;
        }
    }
}
