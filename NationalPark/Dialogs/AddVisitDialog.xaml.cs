using Microsoft.UI.Xaml.Controls;
using NationalPark.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Globalization;

namespace NationalPark.Dialogs
{
    /// <summary>
    /// A dialog that allows users to record a visit to a national park,
    /// including the visit date, a star rating, and optional comments.
    /// </summary>
    public sealed partial class AddVisitDialog : ContentDialog, INotifyPropertyChanged
    {
        private string _parkName = string.Empty;
        private DateTimeOffset _visitDate = DateTimeOffset.Now;
        private double _rating = 3;
        private string _comments = string.Empty;

        /// <summary>
        /// Gets or sets the name of the national park being visited.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the date of the visit.
        /// Defaults to the current date and time.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the star rating for the visit, on a scale from 1 to 5.
        /// Defaults to 3.
        /// </summary>
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

        /// <summary>
        /// Gets or sets optional comments or notes about the visit.
        /// </summary>
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

        /// <summary>
        /// Creates a <see cref="VisitRecord"/> from the current dialog values.
        /// </summary>
        /// <returns>
        /// A <see cref="VisitRecord"/> populated with the selected <see cref="VisitDate"/>,
        /// the integer-truncated <see cref="Rating"/>, and the entered <see cref="Comments"/>.
        /// </returns>
        public VisitRecord GetVisitRecord()
        {
            return new VisitRecord
            {
                VisitDate = VisitDate.DateTime,
                Rating = (int)Rating,
                Comments = Comments
            };
        }

        /// <summary>
        /// Returns a display string that combines a numeric rating with a star-symbol representation.
        /// </summary>
        /// <param name="rating">The numeric rating value (1–5).</param>
        /// <returns>
        /// A string in the format <c>"N ★★★☆☆"</c> where filled stars (★) represent the
        /// integer portion of <paramref name="rating"/> and empty stars (☆) fill the remainder
        /// up to five.
        /// </returns>
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