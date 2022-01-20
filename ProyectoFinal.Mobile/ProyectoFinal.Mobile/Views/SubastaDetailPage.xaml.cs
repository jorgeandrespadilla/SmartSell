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
    public partial class SubastaDetailPage : ContentPage
    {
        public SubastaDetailPage()
        {
            InitializeComponent();
            BindingContext = new SubastaDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((SubastaDetailViewModel)BindingContext).Initialize();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((SubastaDetailViewModel)BindingContext).Dispose();
        }
    }
}