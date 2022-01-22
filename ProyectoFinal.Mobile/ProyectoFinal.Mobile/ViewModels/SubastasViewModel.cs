using ProyectoFinal.Mobile.Models;
using ProyectoFinal.Mobile.Views;
using ProyectoFinal.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class SubastasViewModel : BaseViewModel
    {
        public Command SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command ChangeModeCommand { get; }
        public Command NewSubastaCommand { get; }
        public Command<int> ShowSubastaCommand { get; }
        
        private string searchString;
        public string SearchString
        {
            get => searchString;
            set => SetProperty(ref searchString, value);
        }
        private List<PickerItem> filterOptions;
        public List<PickerItem> FilterOptions
        {
            get => filterOptions;
            set => SetProperty(ref filterOptions, value);
        }
        private PickerItem selectedFilter;
        public PickerItem SelectedFilter
        {
            get => selectedFilter;
            set => SetProperty(ref selectedFilter, value);
        }
        private bool hideEnded;
        public bool HideEnded
        {
            get => hideEnded;
            set => SetProperty(ref hideEnded, value);
        }
        private bool hideOwn;
        public bool HideOwn
        {
            get => hideOwn;
            set => SetProperty(ref hideOwn, value);
        }
        private string subastasMode;
        public string SubastasMode
        {
            get => subastasMode;
            set => SetProperty(ref subastasMode, value);
        }
        private string subastasModeName;
        public string SubastasModeName
        {
            get => subastasModeName;
            set => SetProperty(ref subastasModeName, value);
        }
        private bool visibleOwnFilter;
        public bool VisibleOwnFilter
        {
            get => visibleOwnFilter;
            set => SetProperty(ref visibleOwnFilter, value);
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

        private ObservableCollection<SubastaItem> subastas;
        public ObservableCollection<SubastaItem> Subastas
        {
            get => subastas;
            set => SetProperty(ref subastas, value);
        }

        public SubastasViewModel()
        {
            Title = "Subastas";
            FilterOptions = new List<PickerItem>
            {
                new PickerItem("Precio ascendente", "price_asc"),
                new PickerItem("Precio descendente", "price_desc"),
                new PickerItem("Nombre ascendente", "name_asc"),
                new PickerItem("Nombre descendente", "name_desc"),
                new PickerItem("Ninguno", "none")
            };

            Reset();

            SearchCommand = new Command(SearchSubastas);
            RefreshCommand = new Command(SearchSubastas);
            LoadMoreCommand = new Command(LoadMore);
            ChangeModeCommand = new Command(ChangeMode);
            NewSubastaCommand = null;
            ShowSubastaCommand = new Command<int>(OnSubastaClicked);
        }

        public override void Initialize()
        {
            Reset();
            ObtenerSubastas(true);
        }

        private void Reset()
        {
            currentPage = 1;
            SearchString = "";
            SubastasMode = "TodasSubastas";
            VisibleOwnFilter = true;
            SelectedFilter = FilterOptions.Find(x => x.Code == "none");
            HideEnded = true;
            HideOwn = true;
        }

        private async void OnSubastaClicked(int subastaID)
        {
            await Shell.Current.GoToAsync($"{nameof(SubastaDetailPage)}?id={subastaID}");
        }

        private void SearchSubastas()
        {
            if (IsBusy)
            {
                return;
            }
            currentPage = 1;
            ObtenerSubastas(true);
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
                ObtenerSubastas(false);
            }
        }

        private void ChangeMode()
        {
            currentPage = 1;
            if (SubastasMode == "MisSubastas")
            {
                SubastasMode = "TodasSubastas";
                VisibleOwnFilter = true;
            }
            else
            {
                SubastasMode = "MisSubastas";
                VisibleOwnFilter = false;
            }
            SearchString = "";
            ObtenerSubastas(true);
        }

        private async void ObtenerSubastas(bool resetData)
        {
            try
            {
                IsBusy = true;
                SubastasPagedData results;
                if (SubastasMode == "MisSubastas")
                {
                    results = await SmartSell.GetSubastas(
                        page: currentPage,
                        searchString: SearchString,
                        showAll: "false",
                        hideMySubastas: "false",
                        hideEnded: HideEnded.ToString().ToLower(),
                        sortOrder: SelectedFilter.Code
                    );
                }
                else
                {
                    results = await SmartSell.GetSubastas(
                        page: currentPage,
                        searchString: SearchString,
                        showAll: "true",
                        hideMySubastas: HideOwn.ToString().ToLower(),
                        hideEnded: HideEnded.ToString().ToLower(),
                        sortOrder: SelectedFilter.Code
                    );
                }
                CargarSubastas(results, resetData);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            IsBusy = false;
        }

        private void CargarSubastas(SubastasPagedData results, bool resetData)
        {
            // Update subastas list
            if (resetData)
            {
                Subastas = new ObservableCollection<SubastaItem>(SmartSell.ConvertToSubastaItems(results.Data));
            }
            else
            {
                ICollection<SubastaItem> items = SmartSell.ConvertToSubastaItems(results.Data);
                foreach (SubastaItem item in items)
                {
                    Subastas.Add(item);
                }
            }

            currentPage = results.Page;
            totalPages = results.PageCount;
            TotalResults = results.TotalResults;
            HasMore = currentPage < totalResults;

            // Setup mode button label
            if (SubastasMode == "TodasSubastas")
            {
                SubastasModeName = "Mis subastas";
            }
            else
            {
                SubastasModeName = "Ver todas";
            }
        }
    }
}