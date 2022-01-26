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
    [QueryProperty(nameof(UsuarioID), "id")]
    public partial class PerfilVendedorPage : ContentPage
    {
        public int UsuarioID { get; set; }

        public PerfilVendedorPage()
        {
            InitializeComponent();
            BindingContext = new PerfilVendedorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PerfilVendedorViewModel)BindingContext).Initialize(UsuarioID);
        }
    }
}