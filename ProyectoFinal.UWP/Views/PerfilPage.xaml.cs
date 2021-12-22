using System;

using ProyectoFinal.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class PerfilPage : Page
    {
        public PerfilViewModel ViewModel { get; } = new PerfilViewModel();

        public PerfilPage()
        {
            InitializeComponent();
        }
    }
}
