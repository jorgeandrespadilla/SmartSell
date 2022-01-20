using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class PerfilViewModel : BaseViewModel
    {
        public Command<int> ShowOfertaCommand { get; }
        public Command<string> UpdateOfertasCommand { get; }
        public Command LogoutCommand { get; }

        private PerfilDto perfil;
        public PerfilDto Perfil
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

        private string ofertasType;
        public string OfertasType
        {
            get => ofertasType;
            set => SetProperty(ref ofertasType, value);
        }

        private ICollection<OfertaDto> ofertas;
        public ICollection<OfertaDto> Ofertas
        {
            get => ofertas;
            set => SetProperty(ref ofertas, value);
        }

        public PerfilViewModel()
        {
            Title = "Perfil de usuario";
            UpdateOfertasCommand = new Command<string>(UpdateOfertas);
            ShowOfertaCommand = new Command<int>(OnOfertaClicked);
            LogoutCommand = new Command(OnLogoutClicked);
        }

        public override async void Initialize()
        {
            Perfil = await SmartSell.GetPerfil();
            NombreCompletoTxt = $"{Perfil.Nombres} {Perfil.Apellidos}";
            UpdateOfertas("Participacion");
        }

        private async void UpdateOfertas(string type)
        {
            Ofertas = await SmartSell.GetPerfilOfertas(type);
            if (type == "Participacion")
            {
                OfertasType = "Ofertas más altas";
            }
            else
            {
                OfertasType = "Subastas ganadas";
            }
        }

        private async void OnOfertaClicked(int ofertaID)
        {
            await Application.Current.MainPage.DisplayAlert("Oferta seleccionada", $"{ofertaID}", "Aceptar");
        }

        private async void OnLogoutClicked()
        {
            SmartSell.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
