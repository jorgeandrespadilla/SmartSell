using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class PerfilViewModel : BaseViewModel
    {
        public Command RegisterCommand { get; }
        public Command GoLoginCommand { get; }

        public string NombreCompletoTxt { get; set; }
        public string NombreTxt { get; set; }
        public string ApellidoTxt { get; set; }
        public string UserTxt { get; set; }
        public string CalificacionTxt { get; set; }
        public ICollection<OfertaDto> ofertas { get; set; }


        public PerfilViewModel()
        {
            
        }

        public async void CargarInformacion()
        {
            PerfilDto usuarioActual = await SmartSell.GetPerfil();
            //var ofertasQuery = await smartSell.GetPerfilOfertas("PARTICIPACION");
            //MisOfertas.ItemsSource = ofertasQuery;

            NombreCompletoTxt = $"{usuarioActual.Nombres} {usuarioActual.Apellidos}";
            NombreTxt = usuarioActual.Nombres;
            ApellidoTxt = usuarioActual.Apellidos;
            UserTxt = usuarioActual.Correo;
            CalificacionTxt = $"{CalificacionTxt = Math.Round(usuarioActual.AvgRating, 2).ToString("F2")}/{5:F2}";
            ofertas = await SmartSell.GetPerfilOfertas("Participacion");
        }
    }
}
