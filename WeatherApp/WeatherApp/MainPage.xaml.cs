﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Collections.ObjectModel;

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
            if ((string)App.Current.Properties["UnitsChoice"] == "metric")
            {
                UnitsChoice = Units.metric;
                unitsTemp = "°C";
                unitsSpeed = "m/s";
            }
            else if ((string)App.Current.Properties["UnitsChoice"] == "imperial")
            {
                UnitsChoice = Units.imperial;
                unitsTemp = "°F";
                unitsSpeed = "mph";
            }

            if ((string)App.Current.Properties["Bookmarked"] != "")
            {
                BookmarkedPage.LocationList = new ObservableCollection<Location>(JsonConvert.DeserializeObject<List<Location>>((string)App.Current.Properties["Bookmarked"]));
            }
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
        public enum Units { standard, metric, imperial };
        enum Lang { af, al, ar, az, bg, ca, cz, da, de, el, en, eu, fa, fi, fr, gl, he, hi, hr, hu, id, it, ja, kr, la, lt, mk, no, nl, pl, pt, pt_br, ro, ru, sv, sk, sl, sp, es, sr, th, tr, uk, vi, zh_cn, zu };

        public static string infostring;
        public static Rootobject info;
        public static bool currentLocation;
        public static string latitude;
        public static string longitude;
        public static string placeName1;
        public static string unitsTemp;
        public static string unitsSpeed;

        public static Units UnitsChoice = Units.metric;
        private Lang LangChoice = Lang.en;

        private static ObservableCollection<Hourly> HourlyForecastList { get; set; }
        private static ObservableCollection<Daily> DailyForecastList { get; set; }

        public async void GetCurrentWeather()
        {
            if(!refreshView.IsRefreshing)
                UpdateIndicator.IsVisible = true;

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
            if (info.current.weather[0].icon.Contains("d."))
            {
                if (info.current.weather[0].icon.Contains("01d") || info.current.weather[0].icon.Contains("02d") || info.current.weather[0].icon.Contains("03d"))
                    bdGradient.Background = new LinearGradientBrush(dayGoodWeatherGradientStops, new Point(0, 1), new Point(1, 0));
                else if (info.current.weather[0].icon.Contains("13d") || info.current.weather[0].icon.Contains("50d"))
                    bdGradient.Background = new LinearGradientBrush(dayColdWeatherGradientStops, new Point(0, 1), new Point(1, 0));
                else
                    bdGradient.Background = new LinearGradientBrush(dayBadWeatherGradientStops, new Point(0, 1), new Point(1, 0));
            }
            if (info.current.weather[0].icon.Contains("n."))
            {
                if (info.current.weather[0].icon.Contains("01n") || info.current.weather[0].icon.Contains("02n") || info.current.weather[0].icon.Contains("03n"))
                    bdGradient.Background = new LinearGradientBrush(nightGoodWeatherGradientStops, new Point(0, 1), new Point(1, 0));
                else if (info.current.weather[0].icon.Contains("13n") || info.current.weather[0].icon.Contains("50n"))
                    bdGradient.Background = new LinearGradientBrush(nightColdWeatherGradientStops, new Point(0, 1), new Point(1, 0));
                else
                    bdGradient.Background = new LinearGradientBrush(nightBadWeatherGradientStops, new Point(0, 1), new Point(1, 0));
            }
            //Current Weather
            LocationName.Text = placeName1;
            Description.Text = (info.current.weather[0].description[0].ToString().ToUpper() + info.current.weather[0].description.Substring(1));
            ActualTemp.Text = $"{info.current.temp.ToString("0")}{unitsTemp}";
            WeatherIcon.Source = info.current.weather[0].icon;
            Wind.Text = $"{info.current.wind_speed} {unitsSpeed}";
            Humidity.Text = $"{info.current.humidity}%";
            Pressure.Text = $"{info.current.pressure} hpa";
            Visibility.Text = $"{info.current.visibility / 1000} km";
            UVindex.Text = $"{info.current.uvi}%";
            DewPoint.Text = $"{info.current.dew_point}{unitsTemp}";
            FeelTemp.Text = $"Feels like {info.current.feels_like.ToString("0")}{unitsTemp}";
            MaxTemp.Text = $"{info.daily[0].temp.max.ToString("0")}{unitsTemp}";
            MinTemp.Text = $"{info.daily[0].temp.min.ToString("0")}{unitsTemp}";
            //Hourly Forecast
            HourlyForecastList = new ObservableCollection<Hourly>(info.hourly);
            HourlyForecastView.ItemsSource = HourlyForecastList;
            //Daily Forecast
            DailyForecastList = new ObservableCollection<Daily>(info.daily);
            DailyForecastView.ItemsSource = DailyForecastList;

            //refreshView.IsRefreshing = false;
            UpdateIndicator.IsVisible = false;
        }

        public async void GetCurrentLocation()
        {
            if (!refreshView.IsRefreshing)
                UpdateIndicator.IsVisible = true;

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
            App.Current.Properties["Bookmarked"] = JsonConvert.SerializeObject(BookmarkedPage.LocationList);
            App.Current.Properties["UnitsChoice"] = UnitsChoice.ToString();
        }




        //DayGradientsCollection
        GradientStopCollection dayGoodWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#fcbc4c"), 0.0f), new GradientStop(Color.FromHex("#f1df85"), 0.5f), new GradientStop(Color.FromHex("#c4ed67"), 1f) };
        GradientStopCollection dayColdWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#acb6d1"), 0.0f), new GradientStop(Color.FromHex("#91a1af"), 0.5f), new GradientStop(Color.FromHex("#5d7d9a"), 1f) };
        GradientStopCollection dayBadWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#4ca5fc"), 0.0f), new GradientStop(Color.FromHex("#85f1d3"), 0.5f), new GradientStop(Color.FromHex("#67c8ed"), 1f) };
        //NightGradientCollection
        GradientStopCollection nightGoodWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#5470dd"),0.0f), new GradientStop(Color.FromHex("#4598da"),0.5f), new GradientStop(Color.FromHex("#00d4ff"), 1f) };
        GradientStopCollection nightColdWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#c3c4e2"), 0.0f), new GradientStop(Color.FromHex("#a9b4d6"), 0.5f), new GradientStop(Color.FromHex("#5d7d9a"), 1f) };
        GradientStopCollection nightBadWeatherGradientStops = new GradientStopCollection() { new GradientStop(Color.FromHex("#416ecd"), 0.0f), new GradientStop(Color.FromHex("#6cabb6"), 0.5f), new GradientStop(Color.FromHex("#306bbb"), 1f) };

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

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage(this));
        }
        private void BookmarkedButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BookmarkedPage(this));
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage(this));
        }

        private void DailyExpander_Tapped(object sender, EventArgs e)
        {

        }
    }
}
