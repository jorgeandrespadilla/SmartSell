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
    public partial class OfertasPage : ContentPage
    {
        public OfertasPage()
        {
            InitializeComponent();
            BindingContext = new OfertasViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((OfertasViewModel)BindingContext).Initialize();
        }
    }
}