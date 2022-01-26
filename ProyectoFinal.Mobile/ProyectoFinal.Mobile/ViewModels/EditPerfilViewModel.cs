using ProyectoFinal.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class EditPerfilViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        private string nombres;
        public string Nombres
        {
            get => nombres;
            set => SetProperty(ref nombres, value);
        }

        private string apellidos;
        public string Apellidos
        {
            get => apellidos;
            set => SetProperty(ref apellidos, value);
        }

        private string correo;
        public string Correo
        {
            get => correo;
            set => SetProperty(ref correo, value);
        }

        private string clave;
        public string Clave
        {
            get => clave;
            set => SetProperty(ref clave, value);
        }

        public EditPerfilViewModel()
        {
            Title = "Editar subasta";
            SaveCommand = new Command(OnSaveClicked);
            DeleteCommand = new Command(OnDeleteClicked);
        }

        public async override void Initialize()
        {
            var Perfil = await SmartSell.GetPerfil();
            Nombres = Perfil.Nombres;
            Apellidos = Perfil.Apellidos;
            Correo = Perfil.Correo;
            Clave = "";
        }


        private async void OnSaveClicked()
        {
            try
            {
                await SmartSell.EditPerfil(Nombres, Apellidos, Correo, Clave);
                await Shell.Current.GoToAsync($"..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void OnDeleteClicked()
        {
            bool response = await Application.Current.MainPage.DisplayAlert("Eliminar usuario", "¿Seguro que desea eliminar el usuario?", "Si", "No");
            if (response)
            {
                try
                {
                    await SmartSell.DeletePerfil();
                    SmartSell.Logout();
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                }
            }
        }
    }
}
