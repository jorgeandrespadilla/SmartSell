using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class PreviewViewModel : BaseViewModel
    {
        private SubastaPreview previewData;
        public SubastaPreview PreviewData
        {
            get => previewData;
            set => SetProperty(ref previewData, value);
        }

        public PreviewViewModel()
        {
            Title = "Previsualización";
        }

        public async void Initialize(int subastaID)
        {
            var data = await SmartSell.GetSubastaPreview(subastaID);
            PreviewData = new SubastaPreview(
                data.SubastaID,
                data.UsuarioID,
                data.NombreProducto,
                MediaHelper.UriToImageSource(data.UriImagen)
            );
            Title = PreviewData.NombreProducto;
        }
    }
}
