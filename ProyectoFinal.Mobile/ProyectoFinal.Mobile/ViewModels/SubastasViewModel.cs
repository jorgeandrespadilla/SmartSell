using ProyectoFinal.Mobile.Models;
using ProyectoFinal.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class SubastasViewModel : BaseViewModel
    {
        public Command<int> ShowSubastaCommand { get; }

        private SubastasPagedData results;
        private string mode = "TodasSubastas";
        private string filtroSeleccionado = "none";
        private string searchstring = "";

        private int page = 1;

        private ICollection<SubastaItem> subastas;
        public ICollection<SubastaItem> Subastas
        {
            get => subastas;
            set => SetProperty(ref subastas, value);
        }

        public SubastasViewModel()
        {
            ShowSubastaCommand = new Command<int>(OnSubastaClicked);
        }

        public override void Initialize()
        {
            ObtenerSubastas();
        }

        private async void OnSubastaClicked(int subastaID)
        {
            await Application.Current.MainPage.DisplayAlert("Subasta seleccionada", $"{subastaID}", "Aceptar");
        }

        private async void ObtenerSubastas()
        {
            try
            {
                IsBusy = true;
                if (mode == "MisSubastas")
                {
                    var resp = await SmartSell.GetSubastas(
                        page: page,
                        searchString: searchstring,
                        showAll: "false",
                        hideMySubastas: "false",
                        hideEnded: "false",
                        sortOrder: filtroSeleccionado
                    ); ;
                    results = resp;
                }
                else
                {
                    var resp = await SmartSell.GetSubastas(
                        page: page,
                        searchString: searchstring,
                        hideEnded: "false",
                        showAll: "true",
                        hideMySubastas: "true",
                        sortOrder: filtroSeleccionado
                    );
                    results = resp;
                }
                CargarSubastas();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private void CargarSubastas()
        {
            Subastas = SmartSell.ConvertToSubastaItems(results.Data);
            // cantSubastasTxt.Text = $"{results.TotalResults} resultados encontrados";
            page = results.Page;
        }
    }
}