using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookmarkedPage : ContentPage
    {
        public static ObservableCollection<Location> LocationList = new ObservableCollection<Location>();
        public BookmarkedPage(MainPage main)
        {
            InitializeComponent();
            mp = main;
            LocationListView.ItemsSource = LocationList;
        }
        private MainPage mp;
        private void AddLocation_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage(mp, true));
        }

        private void CloseBookmarked_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void viewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell vc = (ViewCell)sender;
            Location SelectedLocation = (Location)vc.BindingContext;
            MainPage.longitude = SelectedLocation.longitude;
            MainPage.latitude = SelectedLocation.latitude;
            MainPage.placeName1 = SelectedLocation.name;
            MainPage.currentLocation = false;
            mp.GetCurrentWeather();
            await Navigation.PopAsync();
        }

        private void DeleteLocation_Clicked(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            LocationList.Remove(ib.CommandParameter as Location);
        }
    }
}