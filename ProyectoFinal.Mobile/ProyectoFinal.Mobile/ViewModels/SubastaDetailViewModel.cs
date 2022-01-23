using ProyectoFinal.Mobile.Helpers;
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

        private ICollection<ComentarioDto> comentarios;
        public ICollection<ComentarioDto> Comentarios
        {
            get => comentarios;
            set => SetProperty(ref comentarios, value);
        }

        public SubastaDetailViewModel()
        {
            Title = "Información de subasta";
            ShowPerfilCommand = new Command<int>(OnPerfilClicked);
            EditCommand = new Command(OnEditClicked);
        }

        public async Task<bool> CargarSubasta(int subastaID)
        {
            Subasta = await SmartSell.GetSubasta(subastaID);
            Imagen = MediaHelper.UriToImageSource(Subasta.UriImagen);
            Ofertas = subasta.Ofertas;
            Comentarios = subasta.Comentarios;
            if (Subasta.Vigente)
            {
                CanOffer = false;
                if (Subasta.UsuarioID == SmartSell.CurrentUser.ID)
                {
                    if (DateTime.Compare(Subasta.Fecha.AddDays(-1), DateTime.Now) >= 0)
                    {
                        CanEdit = true;
                    }
                    else
                    {
                        CanEdit = false;
                    }
                }
                else
                {
                    CanEdit = false;
                    CanOffer = true;
                    if (subasta.Ofertas.Count != 0)
                    {
                        if (subasta.Ofertas.FirstOrDefault().UsuarioID == SmartSell.CurrentUser.ID)
                        {
                            CanDeleteOferta = true;
                        }
                        else
                        {
                            CanDeleteOferta = false;
                        }
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

        private async void OnEditClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSubastaPage)}?id={Subasta.SubastaID}");
        }

    }
}
