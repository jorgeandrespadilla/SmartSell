using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class PerfilVendedorViewModel : BaseViewModel
    {
        public Command SetRatingCommand { get; }
        public Command LogoutCommand { get; }

        private PerfilVendedorDto perfil;
        public PerfilVendedorDto Perfil
        {
            get => perfil;
            set => SetProperty(ref perfil, value);
        }
        private string nombreCompletoTxt;
        public string NombreCompletoTxt
        {
            get => nombreCompletoTxt;
            set => SetProperty(ref nombreCompletoTxt, value);
        }
        public int UsuarioID { get; set; }

        public PerfilVendedorViewModel()
        {
            Title = "Perfil de vendedor";
            SetRatingCommand = new Command(SetRating);
        }

        public override async void Initialize()
        {
            Perfil = await SmartSell.GetPerfilVendedor(UsuarioID);
            NombreCompletoTxt = $"{Perfil.Nombres} {Perfil.Apellidos}";
        }

        private async void SetRating()
        {
            int rating = 1;
            try
            {
                await SmartSell.SetRatingUsuario(UsuarioID, rating);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
