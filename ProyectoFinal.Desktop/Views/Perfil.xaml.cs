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
        private SmartSell smartSell = SmartSell.Instance;
        private List<Oferta> ofertas = new List<Oferta>();
        public List<Oferta> Ofertas { get { return this.ofertas; } }

        public Perfil()
        {

            this.InitializeComponent();
            ChargeInformation();
        }

        private void ChargeInformation()
        {
            nombreCompletoTxt.Text = smartSell.CurrentUser.Nombres + " " + smartSell.CurrentUser.Apellidos;
            nombresTxt.Text = smartSell.CurrentUser.Nombres;
            apellidosTxt.Text = smartSell.CurrentUser.Apellidos;
            correoTxt.Text = smartSell.CurrentUser.Correo;
            var id = smartSell.CurrentUser.UsuarioID;

            var ratings = smartSell.GetRatingUsuario((App.Current as App).ConnectionString).Where(u => u.UsuarioCalificadoID == id).ToList();
            double avgRating = 0;
            if (ratings.Count != 0)
            {
                avgRating = ratings.Average(ru => ru.Rating);
            }
            calificacionTxt.Text = avgRating.ToString();

            //Recuperar últimas ofertas
            var ofertasQuery = smartSell.GetOfertas((App.Current as App).ConnectionString).Where(o => o.UsuarioID == id).GroupBy(o => o.SubastaID).Select(g => new
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
            ofertas = ofertasUsuario;
        }
    }
    
    
}
