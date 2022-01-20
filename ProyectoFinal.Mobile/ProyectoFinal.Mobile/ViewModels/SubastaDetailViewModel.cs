using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class SubastaDetailViewModel : BaseViewModel
    {
        public Command<int> ShowPerfilCommand { get; }

        private ImageSource imagen;
        public ImageSource Imagen
        {
            get => imagen;
            set => SetProperty(ref imagen, value);
        }
        private string nombreTxt;
        public string NombreTxt
        {
            get => nombreTxt;
            set => SetProperty(ref nombreTxt, value);
        }
        private string vendedorTxt;
        public string VendedorTxt
        {
            get => vendedorTxt;
            set => SetProperty(ref vendedorTxt, value);
        }
        private string precioActualTxt;
        public string PrecioActualTxt
        {
            get => precioActualTxt;
            set => SetProperty(ref precioActualTxt, value);
        }
        private string descripcionTxt;
        public string DescripcionTxt
        {
            get => descripcionTxt;
            set => SetProperty(ref descripcionTxt, value);
        }
        private string vigenteTxt;
        public string VigenteTxt
        {
            get => vigenteTxt;
            set => SetProperty(ref vigenteTxt, value);
        }
        private string fechaFinalizacionTxt;
        public string FechaFinalizacionTxt
        {
            get => fechaFinalizacionTxt;
            set => SetProperty(ref fechaFinalizacionTxt, value);
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

        public override async void Initialize()
        {
            SubastaDto subasta = await SmartSell.GetSubasta(5);
            Imagen = MediaHelper.UriToImageSource(subasta.UriImagen);
            NombreTxt = subasta.NombreProducto;
            VendedorTxt = subasta.NombreVendedor;
            PrecioActualTxt = subasta.MontoActual.ToString("C2");
            DescripcionTxt = subasta.DescripcionProducto;
            if (subasta.Vigente == true)
            {
                VigenteTxt = "Sí";
            }
            else
            {
                VigenteTxt = "No";
            }
            FechaFinalizacionTxt = subasta.Fecha.ToString("dd MMM yyyy, HH:mm");
            Ofertas = subasta.Ofertas;
            Comentarios = subasta.Comentarios;
        }

        private async void OnPerfilClicked(int usuarioID)
        {
            await Application.Current.MainPage.DisplayAlert("Perfil seleccionado", $"{usuarioID}", "Aceptar");
        }

        public override void Dispose()
        {
            Imagen = null;
        }

    }
}
