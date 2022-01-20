using ProyectoFinal.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((LoginViewModel)BindingContext).Initialize();
            bool loggedIn = false;
            if (loggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            ((LoginViewModel)BindingContext).Dispose();
        }
    }
}