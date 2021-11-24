using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public static ObservableCollection<GooglePlaceAutoCompletePrediction> PlacesList { get; set; }
        public SearchPage(MainPage main)
        {
            InitializeComponent();
            mp = main;
        }
        private MainPage mp;
        private async void PlaceSearch_Completed(object sender, EventArgs e)
        {
            Entry searchtext = (Entry)sender;
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={searchtext.Text}&key=AIzaSyAtS_ahApNcoB7BoJdDopnScf4CWySsp3I";

            API_Response data = await API_Caller.Get(url);

            if (data.Successuful)
            {
                //var info = JsonConvert.DeserializeObject<GooglePlace>(data.Response);
                var info = JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(data.Response);

                PlacesList = new ObservableCollection<GooglePlaceAutoCompletePrediction>(info.AutoCompletePlaces);
                PlacesSearchListView.ItemsSource = PlacesList;
            }
        }

        private async void viewCell_Tapped(object sender, EventArgs e)
        {
            GooglePlace info = null;
            ViewCell vc = (ViewCell)sender;
            GooglePlaceAutoCompletePrediction SelectedPlace = (GooglePlaceAutoCompletePrediction)vc.BindingContext;
            string url = $"https://maps.googleapis.com/maps/api/place/details/json?placeid={SelectedPlace.PlaceId}&key=AIzaSyAtS_ahApNcoB7BoJdDopnScf4CWySsp3I";

            API_Response data = await API_Caller.Get(url);

            if (data.Successuful)
            {
                info = new GooglePlace(JObject.Parse(data.Response));

                MainPage.longitude = info.Longitude.ToString();
                MainPage.latitude = info.Latitude.ToString();
                MainPage.placeName1 = info.Name1;
                MainPage.currentLocation = false;
                mp.GetCurrentWeather();
                await Navigation.PopAsync();
            }
        }

        private void CloseSearch_Clicked(object sender, EventArgs e)
        {
             Navigation.PopAsync();
        }

        private void FindMyLocation_Clicked(object sender, EventArgs e)
        {
            mp.GetCurrentLocation();
            mp.GetCurrentWeather();
            Navigation.PopAsync();
        }
    }
}