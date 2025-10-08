using Microsoft.UI.Xaml.Controls;
using NationalPark.Models;
using System;
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

        public AddVisitDialog()
        {
            this.InitializeComponent();
        }

        public VisitRecord GetVisitRecord()
        {
            return new VisitRecord
            {
                VisitDate = VisitDate.DateTime,
                Rating = (int)Rating,
                Comments = Comments
            };
        }

        public string GetRatingText(double rating)
        {
            var stars = new string('★', (int)rating) + new string('☆', 5 - (int)rating);
            return $"{rating:F0} {stars}";
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}