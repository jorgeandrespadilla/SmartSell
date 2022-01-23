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
    public partial class NewComentarioPage : ContentPage
    {
        public int SubastaID { get; set; }
        public NewComentarioPage()
        {
            InitializeComponent();
            BindingContext = new NewComentarioViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((NewComentarioViewModel)BindingContext).SubastaID = SubastaID;
        }
    }
}