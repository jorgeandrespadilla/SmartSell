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
        public Command RegisterCommand { get; }
        public Command GoLoginCommand { get; }

        private string nombreCompletoTxt;
        public string NombreCompletoTxt
        { 
            get => nombreCompletoTxt;
            set => SetProperty(ref nombreCompletoTxt, value);
        }
        private string nombreTxt;
        public string NombreTxt {
            get => nombreTxt;
            set => SetProperty(ref nombreTxt, value);
        }
        private string apellidoTxt;
        public string ApellidoTxt {
            get => apellidoTxt;
            set => SetProperty(ref apellidoTxt, value);
        }
        private string userTxt;
        public string UserTxt {
            get => userTxt;
            set => SetProperty(ref userTxt, value);
        }
        private string calificacionTxt;
        public string CalificacionTxt
        {
            get => calificacionTxt;
            set => SetProperty(ref calificacionTxt, value);
        }

        private ICollection<OfertaDto> ofertas;
        public ICollection<OfertaDto> Ofertas
        {
            get => ofertas;
            set => SetProperty(ref ofertas, value);
        }

        

        public PerfilViewModel()
        {
            
        }

        public async void CargarInformacion()
        {
            PerfilDto usuarioActual = await SmartSell.GetPerfil();
            NombreCompletoTxt = $"{usuarioActual.Nombres} {usuarioActual.Apellidos}";
            NombreTxt = usuarioActual.Nombres;
            ApellidoTxt = usuarioActual.Apellidos;
            UserTxt = usuarioActual.Correo;
            CalificacionTxt = $"{CalificacionTxt = Math.Round(usuarioActual.AvgRating, 2).ToString("F2")}/{5:F2}";
            UpdateOfertas("Participacion");
            
        }

        public async void UpdateOfertas(string type)
        {
            Ofertas = await SmartSell.GetPerfilOfertas(type);
        }
    }
}
