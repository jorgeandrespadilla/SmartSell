using ProyectoFinal.Mobile.Views;
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

        private int ratingUser;
        public int RatingUser
        {
            get => ratingUser;
            set => SetProperty(ref ratingUser, value);
        }

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

        public async void Initialize(int usuarioID)
        {
            try
            {
                UsuarioID = usuarioID;
                Perfil = await SmartSell.GetPerfilVendedor(UsuarioID);
                NombreCompletoTxt = $"{Perfil.Nombres} {Perfil.Apellidos}";
                RatingUser = Perfil.Rating;
            }
            catch(Exception ex)
            {
                await Shell.Current.GoToAsync($"..");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void SetRating()
        {
            try
            {
                await SmartSell.SetRatingUsuario(UsuarioID, RatingUser);
                Perfil = await SmartSell.GetPerfilVendedor(UsuarioID);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
