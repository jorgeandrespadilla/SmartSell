﻿using ProyectoFinal.Mobile.ViewModels;
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
    public partial class SubastasPage : ContentPage
    {
        public SubastasPage()
        {
            InitializeComponent();
            BindingContext = new SubastasViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // ((SubastasViewModel)BindingContext).Initialize();
        }
        
    }
}