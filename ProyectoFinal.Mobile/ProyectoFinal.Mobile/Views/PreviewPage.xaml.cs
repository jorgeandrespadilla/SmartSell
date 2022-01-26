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
    public partial class PreviewPage : ContentPage
    {
        public int SubastaID { get; set; }
        public PreviewPage()
        {
            InitializeComponent();
            BindingContext = new PreviewViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PreviewViewModel)BindingContext).Initialize(SubastaID);
        }
    }
}