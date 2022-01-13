using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string UserTxt { get; set; }
        public string ClaveTxt { get; set; }

        public Command LoginCommand { get; }
        public Command GoRegisterCommand { get; }

        public LoginViewModel()
        {
            UserTxt = "";
            ClaveTxt = "";

            LoginCommand = new Command(OnLoginClicked);
            GoRegisterCommand = new Command(OnGoRegisterClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                IsBusy = true;
                await SmartSell.Login(UserTxt.Trim(), ClaveTxt);
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}", true);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Acceso fallido", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private async void OnGoRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
