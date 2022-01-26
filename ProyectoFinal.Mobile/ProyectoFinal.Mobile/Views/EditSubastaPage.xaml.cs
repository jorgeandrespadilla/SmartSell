using ProyectoFinal.Mobile.Helpers;
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
    [QueryProperty(nameof(SubastaID), "id")]

    public partial class EditSubastaPage : ContentPage
    {
        public int SubastaID { get; set; }
        public EditSubastaPage()
        {
            InitializeComponent();
            BindingContext = new EditSubastaViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((EditSubastaViewModel)BindingContext).Initialize(SubastaID);
        }

    }
}          