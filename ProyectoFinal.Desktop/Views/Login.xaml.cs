using ProyectoFinal.Desktop.Infrastructure;
using ProyectoFinal.Desktop.Infrastructure.Helpers;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private SmartSell smartSell = SmartSell.Instance;
        public Login()
        {
            this.InitializeComponent();
            
        }

        private async void EnviarBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await smartSell.Login(userTxt.Text, pwdText.Password);
                this.Frame.Navigate(typeof(DetailsSubasta), 4);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Acceso fallido", ex.Message).ShowAsync();
            }
        }

        private void HandleRegistrar(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register), null);
        }
    }
}
