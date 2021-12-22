using System;

using ProyectoFinal.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class IndexOfertasPage : Page
    {
        public IndexOfertasViewModel ViewModel { get; } = new IndexOfertasViewModel();

        public IndexOfertasPage()
        {
            InitializeComponent();
        }
    }
}
