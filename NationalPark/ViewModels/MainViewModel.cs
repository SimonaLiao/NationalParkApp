using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using NationalPark.Models;
using NationalPark.Services;

namespace NationalPark.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly NationalParkService _parkService;
        private NationalParkModel? _selectedPark;
        private string _selectedRegion = "All Regions";
        private string _searchText = string.Empty;

        public MainViewModel()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MainViewModel: Starting initialization...");
                
                _parkService = new NationalParkService();
                System.Diagnostics.Debug.WriteLine("MainViewModel: NationalParkService created");
                
                LoadRegions();
                System.Diagnostics.Debug.WriteLine("MainViewModel: Regions loaded");
                
                LoadParks();
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Parks loaded - {Parks.Count} parks");
                
                System.Diagnostics.Debug.WriteLine("MainViewModel: Initialization completed successfully");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Error during initialization: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public ObservableCollection<NationalParkModel> Parks { get; } = new ObservableCollection<NationalParkModel>();
        public ObservableCollection<string> Regions { get; } = new ObservableCollection<string>();

        public NationalParkModel? SelectedPark
        {
            get => _selectedPark;
            set
            {
                _selectedPark = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (_selectedRegion != value)
                {
                    _selectedRegion = value;
                    OnPropertyChanged();
                    LoadParks(); // Reload parks when region filter changes
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    LoadParks(); // Reload parks when search text changes
                }
            }
        }

        public void LoadRegions()
        {
            try
            {
                Regions.Clear();
                Regions.Add("All Regions"); // Add option to show all parks
                
                var regions = _parkService.GetAllRegions();
                foreach (var region in regions)
                {
                    Regions.Add(region);
                }
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Loaded {Regions.Count} regions");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Error loading regions: {ex.Message}");
                throw;
            }
        }

        public void LoadParks()
        {
            try
            {
                Parks.Clear();
                
                IEnumerable<NationalParkModel> parks = _selectedRegion == "All Regions" 
                    ? _parkService.GetAllParks() 
                    : _parkService.GetParksByRegion(_selectedRegion);

                if (!string.IsNullOrWhiteSpace(_searchText))
                {
                    var search = _searchText.Trim();
                    parks = parks.Where(p =>
                        p.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase) ||
                        p.State.Contains(search, System.StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(search, System.StringComparison.OrdinalIgnoreCase));
                }

                foreach (var park in parks)
                {
                    Parks.Add(park);
                }
                
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Loaded {Parks.Count} parks for region '{_selectedRegion}'");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainViewModel: Error loading parks: {ex.Message}");
                throw;
            }
        }

        public void AddVisitRecord(int parkId, VisitRecord visitRecord)
        {
            visitRecord.NationalParkId = parkId;
            _parkService.AddVisitRecord(visitRecord);
            LoadParks(); // Refresh the parks to update visit status
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        
            
    }
}