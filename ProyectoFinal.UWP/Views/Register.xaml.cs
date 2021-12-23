using ProyectoFinal.Desktop.Infrastructure;
using ProyectoFinal.Desktop.Infrastructure.Helpers;
using ProyectoFinal.Shared.Helpers;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.Desktop.Views
{

    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private SmartSell smartSell = SmartSell.Instance;
        public Register()
        {
            this.InitializeComponent();
        }

        private async void RegisterHandle(object sender, RoutedEventArgs e)
        {
            try
            {
                await smartSell.CreateAccount(
                    nombresTxt.Text,
                    apellidoTxt.Text,
                    correoTxt.Text,
                    pwdText.Password
                );
                await Dialog.InfoMessage("Registro exitoso", "Usuario registrado con éxito.").ShowAsync();
                this.Frame.Navigate(typeof(Login), null);
            }
            catch(Exception ex)
            {
                await Dialog.InfoMessage("Registro fallido", ex.Message).ShowAsync();
            }
        }

        private void LoginHandler(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login), null);
        }
    }
}
