using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using ProyectoFinal.Mobile.Views;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command GoLoginCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
            GoLoginCommand = new Command(OnGoLoginClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"..", true);
        }

        private async void OnGoLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"..", true);
        }
    }
}
