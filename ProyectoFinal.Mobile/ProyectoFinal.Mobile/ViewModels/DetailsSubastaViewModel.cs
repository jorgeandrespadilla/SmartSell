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
    public class DetailsSubastaViewModel : BaseViewModel
    {
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


        public DetailsSubastaViewModel()
        {
            
        }

        public async void CargarInformacion()
        {
            SubastaDto subasta = await SmartSell.GetSubasta(2);
            Imagen = MediaHelper.UriToImageSource(subasta.UriImagen);
            NombreTxt = subasta.NombreProducto;
            VendedorTxt = subasta.NombreVendedor;
            PrecioActualTxt = subasta.MontoActual.ToString("C2");
            
            if (subasta.Vigente == true)
            {
                DescripcionTxt = "Sí";
            }
            else
            {
                DescripcionTxt = "No";
            }
            FechaFinalizacionTxt = subasta.Fecha.ToString("dd MMM yyyy, HH:mm", new CultureInfo("es-ES"));
        }

    }
}
