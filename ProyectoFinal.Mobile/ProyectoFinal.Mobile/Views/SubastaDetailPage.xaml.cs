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
    public partial class SubastaDetailPage : ContentPage
    {
        public int SubastaID { get; set; }

        public SubastaDetailPage()
        {
            InitializeComponent();
            BindingContext = new SubastaDetailViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            bool canEdit = await ((SubastaDetailViewModel)BindingContext).CargarSubasta(SubastaID);
            if (!canEdit)
            {
                ToolbarItems.Clear();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((SubastaDetailViewModel)BindingContext).Dispose();
        }
    }
}