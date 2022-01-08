using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(MainPage main)
        {
            InitializeComponent();
            MainPage mp = main;

            if (MainPage.UnitsChoice == MainPage.Units.metric)
                MetricChoice.IsChecked = true;
            else if (MainPage.UnitsChoice == MainPage.Units.imperial)
                ImperialChoice.IsChecked = true;
        }
        private void CloseSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Choice_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (MetricChoice.IsChecked)
            {
                MainPage.UnitsChoice = MainPage.Units.metric;
                MainPage.unitsTemp = "°C";
                MainPage.unitsSpeed = "m/s";
            }

            if (ImperialChoice.IsChecked)
            {
                MainPage.UnitsChoice = MainPage.Units.imperial;
                MainPage.unitsTemp = "°F";
                MainPage.unitsSpeed = "mph";
            }
                
        }
    }
}