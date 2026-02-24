using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NationalPark.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Globalization;

namespace NationalPark.Dialogs
{
    public sealed partial class AddVisitDialog : ContentDialog, INotifyPropertyChanged
    {
        private string _parkName = string.Empty;
        private DateTimeOffset _visitDate = DateTimeOffset.Now;
        private double _rating = 3;
        private string _comments = string.Empty;
        private readonly ObservableCollection<string> _selectedPhotos = new ObservableCollection<string>();

        public string ParkName 
        { 
            get => _parkName; 
            set 
            { 
                if (_parkName != value) 
                { 
                    _parkName = value; 
                    OnPropertyChanged(); 
                } 
            } 
        }

        public DateTimeOffset VisitDate 
        { 
            get => _visitDate; 
            set 
            { 
                if (_visitDate != value) 
                { 
                    _visitDate = value; 
                    OnPropertyChanged(); 
                } 
            } 
        }

        public double Rating 
        { 
            get => _rating; 
            set 
            { 
                if (_rating != value) 
                { 
                    _rating = value; 
                    OnPropertyChanged(); 
                } 
            } 
        }

        public string Comments 
        { 
            get => _comments; 
            set 
            { 
                if (_comments != value) 
                { 
                    _comments = value; 
                    OnPropertyChanged(); 
                } 
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> SelectedPhotos => _selectedPhotos;

        public bool HasSelectedPhotos => _selectedPhotos.Count > 0;

        public AddVisitDialog()
        {
            this.InitializeComponent();
            _selectedPhotos.CollectionChanged += (s, e) => OnPropertyChanged(nameof(HasSelectedPhotos));
        }

        public VisitRecord GetVisitRecord()
        {
            return new VisitRecord
            {
                VisitDate = VisitDate.DateTime,
                Rating = (int)Rating,
                Comments = Comments,
                Photos = new System.Collections.Generic.List<string>(_selectedPhotos)
            };
        }

        public string GetRatingText(double rating)
        {
            var stars = new string('★', (int)rating) + new string('☆', 5 - (int)rating);
            return $"{rating:F0} {stars}";
        }

        public Visibility GetPhotosVisibility(bool hasPhotos)
        {
            return hasPhotos ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void SelectPhotos_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(
                picker,
                WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow!));

            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            foreach (var ext in new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" })
            {
                picker.FileTypeFilter.Add(ext);
            }

            var files = await picker.PickMultipleFilesAsync();
            if (files is null || files.Count == 0) return;
            foreach (var file in files)
            {
                if (!_selectedPhotos.Contains(file.Path))
                    _selectedPhotos.Add(file.Path);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}