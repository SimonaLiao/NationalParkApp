using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NationalPark.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NationalPark.Controls
{
    public sealed partial class ParkDetailControl : UserControl, INotifyPropertyChanged
    {
        private NationalParkModel? _park;
        
        public NationalParkModel? Park 
        { 
            get => _park; 
            set 
            { 
                if (_park != value)
                {
                    _park = value;
                    OnPropertyChanged();
                    // Trigger updates for all dependent properties
                    OnPropertyChanged(nameof(IsVisited));
                    OnPropertyChanged(nameof(ParkName));
                    OnPropertyChanged(nameof(State));
                    OnPropertyChanged(nameof(ImageUrl));
                    OnPropertyChanged(nameof(EstablishedYear));
                    OnPropertyChanged(nameof(AreaInAcres));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(VisitRecords));
                }
            } 
        }

        // Helper properties for binding
        public bool IsVisited => Park?.IsVisited ?? false;
        public string ParkName => Park?.Name ?? string.Empty;
        public string State => Park?.State ?? string.Empty;
        public string ImageUrl => Park?.ImageUrl ?? string.Empty;
        public int EstablishedYear => Park?.EstablishedDate.Year ?? 0;
        public int AreaInAcres => Park?.AreaInAcres ?? 0;
        public string Description => Park?.Description ?? string.Empty;
        public System.Collections.Generic.List<VisitRecord> VisitRecords => Park?.VisitRecords ?? new System.Collections.Generic.List<VisitRecord>();

        public event EventHandler? BackRequested;
        public event EventHandler<NationalParkModel>? VisitRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ParkDetailControl()
        {
            this.InitializeComponent();
            this.DataContext = this; // Set DataContext to self for binding
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }

        private void MarkVisitedButton_Click(object sender, RoutedEventArgs e)
        {
            if (Park != null)
            {
                VisitRequested?.Invoke(this, Park);
            }
        }

        public string GetVisitStatusIcon(bool isVisited)
        {
            return isVisited ? "\uE73E" : "\uE7C3"; // CheckMark or EmptyCircle
        }

        public SolidColorBrush GetVisitStatusColor(bool isVisited)
        {
            return new SolidColorBrush(isVisited ? Microsoft.UI.Colors.Green : Microsoft.UI.Colors.Gray);
        }

        public string GetVisitStatusText(bool isVisited)
        {
            return isVisited ? "Visited" : "Not Visited";
        }

        public Visibility GetMarkVisitedVisibility(bool isVisited)
        {
            return isVisited ? Visibility.Collapsed : Visibility.Visible;
        }

        public Visibility GetAddAnotherVisitVisibility(bool isVisited)
        {
            return isVisited ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility GetVisitRecordsVisibility(bool isVisited)
        {
            return isVisited ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}