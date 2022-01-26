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
        public int SubastaID { get; set; }
        private float monto;
        public float Monto
        {
            get => monto;
            set => SetProperty(ref monto, value);
        }

        public NewOfertaViewModel()
        {
            Title = "Crear oferta";
            SaveCommand = new Command(OnSave);
        }        
        
        private async void OnSave()
        {
            try
            {
                float MontoRedondeado = (float)(Math.Round(Monto, 2));
                await SmartSell.CreateOferta(SubastaID, MontoRedondeado);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
