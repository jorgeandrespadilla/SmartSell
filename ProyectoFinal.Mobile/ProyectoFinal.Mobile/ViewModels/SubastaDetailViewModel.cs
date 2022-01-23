using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Mobile.Models;
using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class SubastaDetailViewModel : BaseViewModel
    {
        public Command EditCommand { get; }
        public Command CreateOfertaCommand { get; }
        public Command DeleteOfertaCommand { get; }
        public Command<int> ShowPerfilCommand { get; }
        public Command AddCommentCommand { get; }
        public Command AddOfertaCommand { get; }
        public Command<int> EditCommentCommand { get; }

        private bool canOffer;
        public bool CanOffer
        {
            get => canOffer;
            set => SetProperty(ref canOffer, value);
        }

        private bool canEdit;
        public bool CanEdit
        {
            get => canEdit;
            set => SetProperty(ref canEdit, value);
        }

        private bool canDeleteOferta;
        public bool CanDeleteOferta
        {
            get => canDeleteOferta;
            set => SetProperty(ref canDeleteOferta, value);
        }

        private SubastaDto subasta;
        public SubastaDto Subasta
        {
            get => subasta;
            set => SetProperty(ref subasta, value);
        }
        private ImageSource imagen;
        public ImageSource Imagen
        {
            get => imagen;
            set => SetProperty(ref imagen, value);
        }

        private ICollection<OfertaDto> ofertas;
        public ICollection<OfertaDto> Ofertas
        {
            get => ofertas;
            set => SetProperty(ref ofertas, value);
        }

        private ICollection<Comentario> comentarios;
        public ICollection<Comentario> Comentarios
        {
            get => comentarios;
            set => SetProperty(ref comentarios, value);
        }

        public SubastaDetailViewModel()
        {
            Title = "Información de subasta";
            EditCommand = new Command(OnEditClicked);
            CreateOfertaCommand = new Command(OnAddOfertaClicked);
            DeleteOfertaCommand = new Command(OnDeleteOfertaClicked);
            ShowPerfilCommand = new Command<int>(OnPerfilClicked);
            AddCommentCommand = new Command(OnAddCommentClicked);
            EditCommentCommand = new Command<int>(OnEditCommentClicked);
            AddOfertaCommand  = new Command(OnAddOfertaClicked);
        }

        public async Task<bool> CargarSubasta(int subastaID)
        {
            Subasta = await SmartSell.GetSubasta(subastaID);
            Imagen = MediaHelper.UriToImageSource(Subasta.UriImagen);
            Ofertas = subasta.Ofertas;
            Comentarios = SmartSell.ComentariosDtoToComentarios(subasta.Comentarios);
            if (Subasta.Vigente)
            {
                CanOffer = false;
                if (Subasta.UsuarioID == SmartSell.CurrentUser.ID)
                {
                    CanEdit = DateTime.Compare(Subasta.Fecha.AddDays(-1), DateTime.Now) >= 0;
                }
                else
                {
                    CanEdit = false;
                    CanOffer = true;
                    if (subasta.Ofertas.Count != 0)
                    {
                        CanDeleteOferta = subasta.Ofertas.FirstOrDefault().UsuarioID == SmartSell.CurrentUser.ID;
                    }
                    else
                    {
                        CanDeleteOferta = false;
                    }
                }
            }
            else
            {
                CanOffer = false;
                CanEdit = false;
            }
            return CanEdit;
        }

        private async void OnPerfilClicked(int usuarioID)
        {
            await Application.Current.MainPage.DisplayAlert("Perfil seleccionado", $"{usuarioID}", "Aceptar");
        }

        public override void Dispose()
        {
            //Imagen = null;
        }

        private async void OnAddOfertaClicked()
        {
            //await Application.Current.MainPage.DisplayAlert("Agregar oferta", $"Ha seleccionado añadir una oferta para la subasta {Subasta.SubastaID}", "Aceptar");
            await Shell.Current.GoToAsync($"{nameof(NewOfertaPage)}?id={Subasta.SubastaID}");
        }

        private async void OnDeleteOfertaClicked()
        {
            try
            {
                bool response = await Application.Current.MainPage.DisplayAlert("Eliminar oferta", "¿Seguro que desea eliminar su oferta?", "Si", "No");
                if (response)
                {
                    await SmartSell.DeleteOferta(Subasta.Ofertas.FirstOrDefault().OfertaID);
                    await CargarSubasta(Subasta.SubastaID);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void OnEditClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSubastaPage)}?id={Subasta.SubastaID}");
        }

        private async void OnAddCommentClicked()
        {
            //await Application.Current.MainPage.DisplayAlert("Agregar comentario", "Ha seleccionado añadir un comentario", "Aceptar");
            await Shell.Current.GoToAsync($"{nameof(NewComentarioPage)}?id={Subasta.SubastaID}");
        }

        private async void OnEditCommentClicked(int id)
        {
            await Application.Current.MainPage.DisplayAlert("Editar comentario", $"{id}", "Aceptar");
            //await Shell.Current.GoToAsync($"{nameof(EditSubastaPage)}?id={Subasta.SubastaID}");
        }

        // Mover a la pantalla de edición del comentario
        //private async void OnDeleteCommentClicked(int id)
        //{
        //    bool response = await Application.Current.MainPage.DisplayAlert("Eliminar comentario", "¿Seguro que desea eliminar el comentario?", "Si", "No");
        //    if (response)
        //    {
        //        try
        //        {
        //            await SmartSell.DeleteComentario(id);
        //        }
        //        catch (Exception ex)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        //        }
        //    }
        //}


    }
}
