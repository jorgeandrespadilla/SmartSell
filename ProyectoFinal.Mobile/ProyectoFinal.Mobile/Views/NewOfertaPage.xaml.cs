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
    public partial class NewOfertaPage : ContentPage
    {
        public int SubastaID { get; set; }
        public NewOfertaPage()
        {
            InitializeComponent();
            BindingContext = new NewOfertaViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((NewOfertaViewModel)BindingContext).SubastaID = SubastaID;
        }
    }
}