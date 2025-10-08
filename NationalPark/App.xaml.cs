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
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NationalPark
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += App_UnhandledException;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("App launching...");
                
                _window = new MainWindow();
                
                // Ensure the window is properly configured
                _window.Title = "National Park Tracker";
                
                System.Diagnostics.Debug.WriteLine("MainWindow created, activating...");
                
                _window.Activate();
                
                System.Diagnostics.Debug.WriteLine("MainWindow activated successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during app launch: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Try to show a basic error message
                try
                {
                    var errorWindow = new Window();
                    var errorFrame = new Frame();
                    var errorPage = new Page();
                    var errorText = new TextBlock
                    {
                        Text = $"Application failed to start: {ex.Message}",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(20)
                    };
                    errorPage.Content = errorText;
                    errorFrame.Content = errorPage;
                    errorWindow.Content = errorFrame;
                    errorWindow.Title = "National Park Tracker - Error";
                    errorWindow.Activate();
                    _window = errorWindow;
                }
                catch
                {
                    // If even the error window fails, we're in deep trouble
                    throw;
                }
            }
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Unhandled exception: {e.Exception.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {e.Exception.StackTrace}");
            
            // Mark as handled to prevent app crash
            e.Handled = true;
        }
    }
}


