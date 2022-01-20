using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Mobile.Services;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command GoLoginCommand { get; }

        public string NombreTxt { get; set; }
        public string ApellidoTxt { get; set; }
        public string UserTxt { get; set; }
        public string ClaveTxt { get; set; }


        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
            GoLoginCommand = new Command(OnGoLoginClicked);
        }

        private async void OnRegisterClicked()
        {
            try
            {
                IsBusy = true;
                await SmartSell.CreateAccount(NombreTxt, ApellidoTxt, UserTxt, ClaveTxt);
                await Shell.Current.GoToAsync($"..", true);
            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Acceso fallido", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private async void OnGoLoginClicked()
        {
            await Shell.Current.GoToAsync($"..", true);
        }
    }
}
