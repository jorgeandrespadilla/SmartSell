using ProyectoFinal.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}