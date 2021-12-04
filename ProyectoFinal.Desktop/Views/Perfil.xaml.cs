using ProyectoFinal.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.Desktop.Views
{
    /*Necesario implementar que le diseño sea reactivo*/
    public sealed partial class Perfil : Page
    {
        Usuario usuarioActual;
        private SmartSell smartSell = SmartSell.Instance;
        

        public Perfil()
        {
            this.InitializeComponent();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null) 
            {
                usuarioActual = smartSell.GetUsuarios().FirstOrDefault(u => u.UsuarioID== int.Parse(e.Parameter.ToString()));
                CargarInformacion();
            }
        }

        private void CargarInformacion()
        {
            var id = usuarioActual.UsuarioID;
            if (usuarioActual.UsuarioID == smartSell.CurrentUser.UsuarioID)
            {
                buttonWrapper.Visibility = Visibility.Visible;
                ratingWrapper.Visibility = Visibility.Collapsed;
                var ofertasQuery = smartSell.GetOfertas().Where(o => o.UsuarioID == id).GroupBy(o => o.SubastaID).Select(g => new
                {
                    OfertaActual = g.OrderByDescending(x => x.Monto).Select(x => x).FirstOrDefault()
                }).ToList();

                List<Oferta> ofertasUsuario = new List<Oferta>();
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) > 0).ToList();
                foreach (var oferta in ofertasQuery)
                {
                    ofertasUsuario.Add(oferta.OfertaActual);
                }
                ofertasUsuario = ofertasUsuario.OrderByDescending(o => o.Monto).ToList();
                MisOfertas.ItemsSource = ofertasUsuario;
            }
            else
            {
                opSelected.Visibility = Visibility.Collapsed;
                buttonWrapper.Visibility = Visibility.Collapsed;
                ratingWrapper.Visibility = Visibility.Visible;
                tableWrapper.Visibility = Visibility.Collapsed; 
            }
            nombreCompletoTxt.Text = usuarioActual.Nombres + " " + usuarioActual.Apellidos;
            nombresTxt.Text = usuarioActual.Nombres;
            apellidosTxt.Text = usuarioActual.Apellidos;
            correoTxt.Text = usuarioActual.Correo;
            

            var ratings = smartSell.GetRatingUsuario().Where(u => u.UsuarioCalificadoID == id).ToList();
            double avgRating = 0;
            if (ratings.Count != 0)
            {
                avgRating = ratings.Average(ru => ru.Rating);
            }
            calificacionTxt.Text = avgRating.ToString();
        }

        private void ActualizarTabla(object sender, SelectionChangedEventArgs e)
        {
            var id = usuarioActual.UsuarioID;
            int op = opSelected.SelectedIndex;
            List<Oferta> ofertasUsuario = new List<Oferta>();
            var ofertasQuery = smartSell.GetOfertas().Where(o => o.UsuarioID == id).GroupBy(o => o.SubastaID).Select(g => new
            {
                OfertaActual = g.OrderByDescending(x => x.Monto).Select(x => x).FirstOrDefault()
            }).ToList();

            if (op == 0)
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) > 0).ToList();
                foreach (var oferta in ofertasQuery)
                {
                    ofertasUsuario.Add(oferta.OfertaActual);
                }
                ofertasUsuario = ofertasUsuario.OrderByDescending(o => o.Monto).ToList();
            }
            else if(op==1)
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) <= 0).ToList();
                foreach (var oferta in ofertasQuery)
                {
                    var subasta = oferta.OfertaActual.Subasta;
                    var highestOferta = smartSell.FindOfertasBySubastaID(subasta.SubastaID).OrderByDescending(o => o.Monto).FirstOrDefault();
                    if (highestOferta != null && highestOferta.OfertaID == oferta.OfertaActual.OfertaID)
                    {
                        ofertasUsuario.Add(oferta.OfertaActual);
                    }
                }
                ofertasUsuario = ofertasUsuario.OrderByDescending(o => o.Subasta.FechaLimite).ToList();
            }
            MisOfertas.ItemsSource = ofertasUsuario;
        }
    }
    
    
}
