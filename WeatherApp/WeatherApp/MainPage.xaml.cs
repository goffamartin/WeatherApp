using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Windows.Input;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Task.Run(AnimateBackGround);
            infostring = (string)App.Current.Properties["Data"];
            longitude = (string)App.Current.Properties["Longitude"];
            latitude = (string)App.Current.Properties["Latitude"];
            placeName1 = (string)App.Current.Properties["Name"];

            if ((string)App.Current.Properties["FirstStart"] == "FirstStart")
                GetCurrentLocation();
            else
                GetCurrentWeather();

            refreshView.Command = new Command(() =>
            {
            var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    InternetConnection.IsVisible = false;
                    if (currentLocation)
                        GetCurrentLocation();
                    else
                        GetCurrentWeather();
                }
                else
                {
                    InternetConnection.IsVisible = true;
                    SaveData();
                }
                

                refreshView.IsRefreshing = false;
            }); 
        }
        enum Units { standard, metric, imperial };
        enum Lang { af, al, ar, az, bg, ca, cz, da, de, el, en, eu, fa, fi, fr, gl, he, hi, hr, hu, id, it, ja, kr, la, lt, mk, no, nl, pl, pt, pt_br, ro, ru, sv, sk, sl, sp, es, sr, th, tr, uk, vi, zh_cn, zu };

        public static string infostring;
        public static Rootobject info;
        public static bool currentLocation;
        public static string latitude;
        public static string longitude;
        public static string placeName1;

        private Units UnitsChoice = Units.metric;
        private Lang LangChoice = Lang.en;

        public async void GetCurrentWeather()
        {
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&exclude=minutely&units={UnitsChoice}&appid={Constants.WeatherApiKey}&lang={LangChoice}";
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                InternetConnection.IsVisible = false;
                API_Response data = await API_Caller.Get(url);
                if (data.Successuful)
                {
                    info = JsonConvert.DeserializeObject<Rootobject>(data.Response);
                }
            }
            else
            {
                info = JsonConvert.DeserializeObject<Rootobject>(infostring);
                placeName1 = (string)App.Current.Properties["Name"];
            }
            LocationName.Text = placeName1;
            Description.Text = (info.current.weather[0].description[0].ToString().ToUpper() + info.current.weather[0].description.Substring(1));
            ActualTemp.Text = $"{info.current.temp.ToString("0")}°C";
            WeatherIcon.Source = $"i{info.current.weather[0].icon}.png";
            Wind.Text = $"{info.current.wind_speed} m/s";
            Humidity.Text = $"{info.current.humidity}%";
            Pressure.Text = $"{info.current.pressure} hpa";
            Visibility.Text = $"{info.current.visibility / 1000} km";
            UVindex.Text = $"{info.current.uvi}%";
            FeelTemp.Text = $"Feels like {info.current.feels_like.ToString("0")}°C";
            MaxTemp.Text = $"{info.daily[0].temp.max.ToString("0")}°C";
            MinTemp.Text = $"{info.daily[0].temp.min.ToString("0")}°C";
         }

        public async void GetCurrentLocation()
        {
            currentLocation = true;
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });   
                }
                if (location != null)
                {
                    latitude = location.Latitude.ToString();
                    longitude = location.Longitude.ToString();
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        if (placemark.SubLocality != null)
                            placeName1 = placemark.SubLocality;
                        else
                            placeName1 = placemark.SubAdminArea;
                    }
                        
                }
                else
                {
                    Console.WriteLine("No GPS");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Error", fnsEx.Message, "OK");
                currentLocation = false;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Error", fneEx.Message, "OK");
                currentLocation = false;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Error", pEx.Message, "OK");
                currentLocation = false;
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Error", ex.Message, "OK");
                currentLocation = false;
            }
            GetCurrentWeather();
        }

        public void SaveData()
        {
            App.Current.Properties["Name"] = placeName1;
            App.Current.Properties["Data"] = JsonConvert.SerializeObject(info);
            App.Current.Properties["Latitude"] = latitude;
            App.Current.Properties["Longitude"] = longitude;
        }


        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage(this));
        }

        private async void AnimateBackGround()
        {
            Action<double> forward = input => bdGradient.AnchorY = input;
            Action<double> backward = input => bdGradient.AnchorY = input;

            while (true)
            {
                bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
                bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
            }
        }
    }
}
