using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class OfertasViewModel : BaseViewModel
    {
        public Command SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command<int> ShowSubastaCommand { get; }

        private string searchString;
        public string SearchString
        {
            get => searchString;
            set => SetProperty(ref searchString, value);
        }

        private int currentPage;

        private int totalPages;

        private int totalResults;
        public int TotalResults
        {
            get => totalResults;
            set => SetProperty(ref totalResults, value);
        }
        private bool hasMore;
        public bool HasMore
        {
            get => hasMore;
            set => SetProperty(ref hasMore, value);
        }

        private ObservableCollection<OfertaDto> ofertas;
        public ObservableCollection<OfertaDto> Ofertas
        {
            get => ofertas;
            set => SetProperty(ref ofertas, value);
        }

        public OfertasViewModel()
        {
            Title = "Mis ofertas";

            SearchCommand = new Command(SearchOfertas);
            RefreshCommand = new Command(SearchOfertas);
            LoadMoreCommand = new Command(LoadMore);
            ShowSubastaCommand = new Command<int>(OnOfertaClicked);

            Initialize();
        }

        public override void Initialize()
        {
            Reset();
            ObtenerOfertas(true);
        }

        private void Reset()
        {
            currentPage = 1;
            SearchString = "";
        }

        private async void OnOfertaClicked(int subastaID)
        {
            try
            {
                await SmartSell.GetSubasta(subastaID);
                await Shell.Current.GoToAsync($"{nameof(SubastaDetailPage)}?id={subastaID}");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private void SearchOfertas()
        {
            if (IsBusy)
            {
                return;
            }
            currentPage = 1;
            ObtenerOfertas(true);
        }

        private void LoadMore()
        {
            if (IsBusy)
            {
                return;
            }
            bool hasMorePages = currentPage < totalPages;
            if (hasMorePages)
            {
                currentPage += 1;
                ObtenerOfertas(false);
            }
        }

        private async void ObtenerOfertas(bool resetData)
        {
            try
            {
                IsBusy = true;
                PagedData<OfertaDto> results = await SmartSell.GetOfertas(currentPage, SearchString);
                CargarOfertas(results, resetData);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private void CargarOfertas(PagedData<OfertaDto> results, bool resetData)
        {
            // Update subastas list
            if (resetData)
            {
                Ofertas = new ObservableCollection<OfertaDto>(results.Data);
            }
            else
            {
                IEnumerable<OfertaDto> items = results.Data;
                foreach (OfertaDto item in items)
                {
                    Ofertas.Add(item);
                }
            }

            currentPage = results.Page;
            totalPages = results.PageCount;
            TotalResults = results.TotalResults;
            HasMore = currentPage < totalResults;
        }
    }
}
