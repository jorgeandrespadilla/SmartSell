using ProyectoFinal.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class NewComentarioViewModel: BaseViewModel
    {
        public int SubastaID { get; set; }
        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public Command SaveCommand { get; }


        public NewComentarioViewModel()
        {
            Title = "Agregar comentario";
            SaveCommand = new Command(OnSave);
        }
      
        private async void OnSave()
        {
            try
            {
                await SmartSell.CreateComentario(SubastaID, Description);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
