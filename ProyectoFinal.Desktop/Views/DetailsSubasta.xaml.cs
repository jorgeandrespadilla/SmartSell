using ProyectoFinal.Desktop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.Desktop.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class DetailsSubasta : Page
    {
        private Subasta subasta;
        private SmartSell smartSell = SmartSell.Instance;

        public DetailsSubasta()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            subasta = smartSell.GetSubastas().Where(s => s.SubastaID == Int32.Parse(e.Parameter.ToString())).FirstOrDefault();
            CargarInformacion();
        }

        public async void CargarInformacion()
        {
            string imgString = subasta.FotoUrlProducto.Split(", ").Last();
            var bytes = Convert.FromBase64String(imgString);
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            imagenProducto.Source = image;
            nombreTxt.Text = subasta.NombreProducto;
            nombreVendedor.Text = $"{subasta.Usuario.Nombres} {subasta.Usuario.Apellidos}";
            precioTxt.Text = smartSell.FindOfertasBySubastaID(subasta.SubastaID).OrderByDescending(u => u.Monto).FirstOrDefault().Monto.ToString();
            descripcionTxt.Text = subasta.DescripcionProducto;
            if (DateTime.Compare(DateTime.Now, subasta.FechaLimite) < 0)
            {
                vigenteTxt.Text = "Sí";
            }
            else
            {
                vigenteTxt.Text = "No";
            }
            fechaTxt.Text = subasta.FechaLimite.ToString();
            CargarTablaOfertas();
            CargarComentarios();
        }

        public void CargarTablaOfertas()
        {
            ICollection<Oferta> ofertas = smartSell.GetOfertas().Where(u => u.SubastaID == subasta.SubastaID).OrderByDescending(o => o.Monto).ToList();
            OfertasSubasta.ItemsSource = ofertas;
        }

        public void CargarComentarios()
        {
            ICollection<Comentario> comentarios = smartSell.GetComentarios().Where(u => u.SubastaID == subasta.SubastaID).OrderByDescending(o => o.FechaCreacion).ToList();
            ComentariosGrid.ItemsSource = comentarios;
        }

        private void NavigatePerfilVendedor(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Perfil), subasta.UsuarioID);
        }
    }
}
