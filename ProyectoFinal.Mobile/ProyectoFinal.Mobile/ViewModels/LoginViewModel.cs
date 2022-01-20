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
        private string correoTxt;
        public string CorreoTxt
        {
            get => correoTxt;
            set => SetProperty(ref correoTxt, value);
        }
        private string claveTxt;
        public string ClaveTxt
        {
            get => claveTxt;
            set => SetProperty(ref claveTxt, value);
        }

        public Command LoginCommand { get; }
        public Command GoRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            GoRegisterCommand = new Command(OnGoRegisterClicked);
        }

        private async void OnLoginClicked()
        {
            try
            {
                IsBusy = true;
                await SmartSell.Login(CorreoTxt.Trim(), ClaveTxt);
                await Shell.Current.GoToAsync($"//{nameof(SubastasPage)}", true);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Acceso fallido", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private async void OnGoRegisterClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        public override void Initialize()
        {
            CorreoTxt = "";
            ClaveTxt = "";
        }

        public override void Dispose()
        {
            CorreoTxt = "";
            ClaveTxt = "";
        }
    }
}
