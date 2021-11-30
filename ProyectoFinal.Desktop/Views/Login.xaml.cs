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

        private void enviarBtn_Click(object sender, RoutedEventArgs e)
        {
            string correo = userTxt.Text;
            string pwd = pwdText.Password;
            string passwordHash = smartSell.ToSHA256(pwd.ToString());
            Usuario usuario = smartSell.GetUsuarios().Where(u => u.Correo == correo.ToString().ToLower() && u.Clave == passwordHash).FirstOrDefault();
            if (usuario == null)
            {
                /*ModelState.AddModelError("loginError", "Las credenciales ingresadas no son válidas.");
                return View();*/
                return;
            }
            if (!usuario.Activo)
            {
                /*ModelState.AddModelError("loginError", "La cuenta ya no se encuentra disponible.");
                return View();*/
                return;
            }
            smartSell.CurrentUser = usuario;
            this.Frame.Navigate(typeof(DetailsSubasta),5);
            

        }

        private void HandleRegistrar(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register), null);
        }
    }
}
