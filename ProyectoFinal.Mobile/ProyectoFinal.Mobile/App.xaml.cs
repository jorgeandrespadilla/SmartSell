using ProyectoFinal.Mobile.Services;
using ProyectoFinal.Mobile.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            ValidateConectivity();
            Connectivity.ConnectivityChanged += ConnectivityChanged;
        }

        protected override void OnSleep()
        {
            Connectivity.ConnectivityChanged -= ConnectivityChanged;
        }

        protected override void OnResume()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
        }

        void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ValidateConectivity();
        }

        private async void ValidateConectivity()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                await Current.MainPage.DisplayAlert("Problemas de conectividad", "No se pudo acceder a la red actual. Compruebe su conexión a Internet.", "Aceptar");
            }
        }
    }
}
