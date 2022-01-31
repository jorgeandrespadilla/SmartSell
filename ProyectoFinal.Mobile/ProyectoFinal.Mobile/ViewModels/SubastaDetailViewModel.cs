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
        public Command ShowPreviewCommand { get; }
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
            ShowPreviewCommand = new Command(OnPreviewClicked);
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
                    CanEdit = true;
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

        private async void OnPreviewClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(PreviewPage)}?id={Subasta.SubastaID}");
        }

        private async void OnPerfilClicked(int usuarioID)
        {
            if (SmartSell.CurrentUser.ID == usuarioID)
            {
                await Shell.Current.GoToAsync($"//{nameof(PerfilPage)}");
            }
            else
            {
                try
                {
                    await SmartSell.GetPerfilVendedor(usuarioID);
                    await Shell.Current.GoToAsync($"{nameof(PerfilVendedorPage)}?id={usuarioID}");

                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                }
            }

        }

        public override void Dispose()
        {
            //Imagen = null;
        }

        private async void OnAddOfertaClicked()
        {
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
            await Shell.Current.GoToAsync($"{nameof(NewComentarioPage)}?id={Subasta.SubastaID}");
        }

        private async void OnEditCommentClicked(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(EditComentarioPage)}?id={id}");
        }

    }
}
