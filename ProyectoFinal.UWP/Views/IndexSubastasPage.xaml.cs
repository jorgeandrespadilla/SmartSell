using System;

using ProyectoFinal.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class IndexSubastasPage : Page
    {
        public IndexSubastasViewModel ViewModel { get; } = new IndexSubastasViewModel();

        public IndexSubastasPage()
        {
            InitializeComponent();
        }
    }
}
