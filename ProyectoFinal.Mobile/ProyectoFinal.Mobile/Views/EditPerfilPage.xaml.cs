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
    public partial class EditPerfilPage : ContentPage
    {
        public EditPerfilPage()
        {
            InitializeComponent();
            BindingContext = new EditPerfilViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((EditPerfilViewModel)BindingContext).Initialize();
        }
    }
}