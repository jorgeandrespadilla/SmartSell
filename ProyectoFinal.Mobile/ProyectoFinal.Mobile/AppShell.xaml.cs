using ProyectoFinal.Mobile.ViewModels;
using ProyectoFinal.Mobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(NewSubastaPage), typeof(NewSubastaPage));
            Routing.RegisterRoute(nameof(EditComentarioPage), typeof(EditComentarioPage));
            Routing.RegisterRoute(nameof(EditSubastaPage), typeof(EditSubastaPage));
            Routing.RegisterRoute(nameof(SubastaDetailPage), typeof(SubastaDetailPage));
            Routing.RegisterRoute(nameof(PreviewPage), typeof(PreviewPage));
            Routing.RegisterRoute(nameof(NewComentarioPage), typeof(NewComentarioPage));
            Routing.RegisterRoute(nameof(NewOfertaPage), typeof(NewOfertaPage));
            Routing.RegisterRoute(nameof(PerfilVendedorPage), typeof(PerfilVendedorPage));
        }

    }
}
