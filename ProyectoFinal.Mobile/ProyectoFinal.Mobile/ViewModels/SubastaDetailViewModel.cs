using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class SubastaDetailViewModel : BaseViewModel
    {
        public Command<int> ShowPerfilCommand { get; }

        private SubastaDto subasta;
        public SubastaDto Subasta
        {
            get => subasta;
            set => SetProperty(ref subasta, value);
        }
        private ImageSource imagen;
        public ImageSource Imagen
        {
            get => imagen;
            set => SetProperty(ref imagen, value);
        }

        private ICollection<OfertaDto> ofertas;
        public ICollection<OfertaDto> Ofertas
        {
            get => ofertas;
            set => SetProperty(ref ofertas, value);
        }

        private ICollection<ComentarioDto> comentarios;
        public ICollection<ComentarioDto> Comentarios
        {
            get => comentarios;
            set => SetProperty(ref comentarios, value);
        }

        public SubastaDetailViewModel()
        {
            Title = "Información de subasta";
            ShowPerfilCommand = new Command<int>(OnPerfilClicked);
        }

        public async void CargarSubasta(int subastaID)
        {
            Subasta = await SmartSell.GetSubasta(subastaID);
            Imagen = MediaHelper.UriToImageSource(Subasta.UriImagen);
            Ofertas = subasta.Ofertas;
            Comentarios = subasta.Comentarios;
        }

        private async void OnPerfilClicked(int usuarioID)
        {
            await Application.Current.MainPage.DisplayAlert("Perfil seleccionado", $"{usuarioID}", "Aceptar");
        }

        public override void Dispose()
        {
            //Imagen = null;
        }

    }
}
