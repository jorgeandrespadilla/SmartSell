using ProyectoFinal.Desktop.Models;
using System;
using System.Collections.Generic;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace ProyectoFinal.Desktop
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var smartSell = SmartSell.Instance;
            var usuarios = smartSell.GetUsuarios((App.Current as App).ConnectionString);
            usuarios.Count();
            var comentarios = smartSell.GetComentarios((App.Current as App).ConnectionString);
            comentarios.Count();
            var rating = smartSell.GetRatinUsuario((App.Current as App).ConnectionString);
            rating.Count();
            var subasta = smartSell.GetSubastas((App.Current as App).ConnectionString);
            subasta.Count();
            var oferta = smartSell.GetOfertas((App.Current as App).ConnectionString);
            oferta.Count();
        }
    }
}
