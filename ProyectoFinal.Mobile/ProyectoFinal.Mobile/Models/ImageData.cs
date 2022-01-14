using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Models
{
    public class ImageData
    {
        public ImageSource ImageSource { get; set; }
        public string Base64Uri { get; set; }

        public ImageData(ImageSource imageSource, string base64Uri)
        {
            ImageSource = imageSource;
            Base64Uri = base64Uri;
        }
    }
}
