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
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

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

        private void ParksGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NationalParkModel park)
            {
                ShowParkDetail(park);
            }
        }

        private void ShowParkDetail(NationalParkModel park)
        {
            ParkDetailView.Park = park;
            HomeView.Visibility = Visibility.Collapsed;
            ParkDetailView.Visibility = Visibility.Visible;
        }

        private void ParkDetailView_BackRequested(object sender, EventArgs e)
        {
            ShowHome();
        }

        private void ShowHome()
        {
            ParkDetailView.Visibility = Visibility.Collapsed;
            HomeView.Visibility = Visibility.Visible;
            ViewModel.LoadParks(); // Refresh parks to update visit status
        }

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

        public string GetParkCountText(int count)
        {
            return count == 1 ? "1 park" : $"{count} parks";
        }
    }
}
