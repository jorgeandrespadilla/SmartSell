using ProyectoFinal.Desktop.Infrastructure;
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
            string correo = userTxt.Text;
            string pwd = pwdText.Password;

            try
            {
                var data = await smartSell.Authorize(correo, pwd);
                var messageDialog = new MessageDialog("");
                messageDialog.Title = "Autenticación exitosa";
                messageDialog.Content = data.Nombre;
                messageDialog.Commands.Add(new UICommand("Cerrar"));
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();

                smartSell.CurrentUser = null; // TODO: Asignar el valor
                this.Frame.Navigate(typeof(DetailsSubasta), 4);
            }
            catch (Exception a)
            {
                var messageDialog = new MessageDialog("");
                messageDialog.Title = "Autenticación ";
                messageDialog.Content = a.Message;
                messageDialog.Commands.Add(new UICommand("Cerrar"));
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }

        }

        private void HandleRegistrar(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register), null);
        }
    }
}
