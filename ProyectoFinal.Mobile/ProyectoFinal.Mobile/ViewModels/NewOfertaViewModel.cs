using ProyectoFinal.Mobile.Models;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class NewOfertaViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public int SubastaID { get; set; }
        private float monto;
        public float Monto
        {
            get => Monto;
            set => SetProperty(ref monto, value);
        }

        public NewOfertaViewModel()
        {
            Title = "Crear oferta";
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
        }        

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        
        private async void OnSave()
        {
            try
            {
                await SmartSell.CreateOferta(SubastaID, (float)Monto);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
