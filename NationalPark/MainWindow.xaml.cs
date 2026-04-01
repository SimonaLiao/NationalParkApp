using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using NationalPark.Models;
using NationalPark.ViewModels;
using NationalPark.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NationalPark
{
    /// <summary>
    /// The main application window that hosts the home view and park detail view,
    /// managing navigation between them via visibility toggling.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the view model that provides data and commands for the main window.
        /// </summary>
        public MainViewModel ViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class,
        /// sets up the UI components, and creates the associated view model.
        /// </summary>
        public MainWindow()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MainWindow: Starting initialization...");
                
                this.InitializeComponent();
                System.Diagnostics.Debug.WriteLine("MainWindow: InitializeComponent completed");
                
                ViewModel = new MainViewModel();
                System.Diagnostics.Debug.WriteLine("MainWindow: ViewModel created successfully");
                
                System.Diagnostics.Debug.WriteLine("MainWindow: Initialization completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainWindow: Error during initialization: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"MainWindow: Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Handles item click events on the parks grid view and navigates to the park detail view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data containing the clicked item.</param>
        private void ParksGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NationalParkModel park)
            {
                ShowParkDetail(park);
            }
        }

        /// <summary>
        /// Navigates to the park detail view for the specified park.
        /// </summary>
        /// <param name="park">The national park to display in the detail view.</param>
        private void ShowParkDetail(NationalParkModel park)
        {
            ParkDetailView.Park = park;
            HomeView.Visibility = Visibility.Collapsed;
            ParkDetailView.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the back-navigation request from the park detail view and returns to the home view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ParkDetailView_BackRequested(object sender, EventArgs e)
        {
            ShowHome();
        }

        /// <summary>
        /// Returns to the home view and refreshes the parks list to reflect any updated visit status.
        /// </summary>
        private void ShowHome()
        {
            ParkDetailView.Visibility = Visibility.Collapsed;
            HomeView.Visibility = Visibility.Visible;
            ViewModel.LoadParks(); // Refresh parks to update visit status
        }

        /// <summary>
        /// Handles the visit request from the park detail view by showing the add-visit dialog
        /// and recording the visit when confirmed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="park">The national park for which the visit is being recorded.</param>
        private async void ParkDetailView_VisitRequested(object sender, NationalParkModel park)
        {
            var dialog = new AddVisitDialog
            {
                ParkName = park.Name,
                XamlRoot = Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                var visitRecord = dialog.GetVisitRecord();
                ViewModel.AddVisitRecord(park.Id, visitRecord);
                
                // Refresh the park detail view
                var updatedPark = ViewModel.Parks.FirstOrDefault(p => p.Id == park.Id);
                if (updatedPark != null)
                {
                    ParkDetailView.Park = updatedPark;
                }
            }
        }

        /// <summary>
        /// Returns a human-readable string representing the number of parks.
        /// </summary>
        /// <param name="count">The number of parks.</param>
        /// <returns>
        /// <c>"1 park"</c> when <paramref name="count"/> is 1; otherwise <c>"{count} parks"</c>.
        /// </returns>
        public string GetParkCountText(int count)
        {
            return count == 1 ? "1 park" : $"{count} parks";
        }
    }
}
