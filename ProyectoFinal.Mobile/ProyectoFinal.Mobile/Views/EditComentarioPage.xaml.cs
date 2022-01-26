using ProyectoFinal.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ComentarioID), "id")]

    public partial class EditComentarioPage : ContentPage
    {
        public int ComentarioID { get; set; }

        public EditComentarioPage()
        {
            InitializeComponent();
            BindingContext = new EditComentarioViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((EditComentarioViewModel)BindingContext).CargarComentario(ComentarioID);
        }
        
    }

}