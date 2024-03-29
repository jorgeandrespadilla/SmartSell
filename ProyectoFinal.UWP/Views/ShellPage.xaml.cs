﻿using System;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProyectoFinal.UWP.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public SmartSell SmartSell = SmartSell.Instance;
        public ShellPage()
        {   
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }
    }
}
