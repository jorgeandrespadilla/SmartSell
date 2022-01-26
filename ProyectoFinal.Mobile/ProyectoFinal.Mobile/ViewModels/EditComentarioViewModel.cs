using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class EditComentarioViewModel : BaseViewModel
    {
        private ComentarioDto comentario;
        public ComentarioDto Comentario
        {
            get => comentario;
            set => SetProperty(ref comentario, value);
        }
        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public EditComentarioViewModel()
        {
            Title = "Editar comentario";
            SaveCommand = new Command(OnSaveClicked);
            DeleteCommand = new Command(OnDeleteClicked);
        }


        public async void Initialize(int ComentarioID)
        {
            Comentario = await SmartSell.GetComentario(ComentarioID);
            Description = Comentario.Descripcion;
        }

        public async void OnSaveClicked()
        {
            try
            {
                await SmartSell.EditComentario(Comentario.ComentarioID, Description);
                await Shell.Current.GoToAsync("..");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

       
        public async void OnDeleteClicked()
        {
            bool response = await Application.Current.MainPage.DisplayAlert("Eliminar comentario", "¿Seguro que desea eliminar el comentario?", "Si", "No");
            if (response)
            {
                try
                {
                    await SmartSell.DeleteComentario(Comentario.ComentarioID);
                    await Shell.Current.GoToAsync($"..");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                }
            }
        }

    }
}
