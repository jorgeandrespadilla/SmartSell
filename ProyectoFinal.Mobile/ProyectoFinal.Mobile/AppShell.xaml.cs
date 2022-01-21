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
            Routing.RegisterRoute(nameof(SubastaDetailPage), typeof(SubastaDetailPage));
            Routing.RegisterRoute(nameof(EditSubastaPage), typeof(EditSubastaPage));

            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
