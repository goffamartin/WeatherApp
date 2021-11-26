using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    public partial class App : Application
    {
        MainPage mp;
        public App()
        {
            InitializeComponent();
            OnStart();
            Device.SetFlags(new[] { "Brush_Experimental" });
            MainPage = new NavigationPage(mp = new MainPage()) 
            {
                BarBackgroundColor = Color.FromHex("#E5E5E5")
            };
        }

        protected override void OnStart()
        {
            try
            {
                string firstStart = (string)Application.Current.Properties["FirstStart"];

            }
            catch
            {
                App.Current.Properties["Name"] = "London";
                App.Current.Properties["Latitude"] = "51.509865";
                App.Current.Properties["Longitude"] = "-0.118092";
                Application.Current.Properties["FirstStart"] = "Start";
            }     
        }

        protected override void OnSleep()
        {
             mp.SaveData();
        }

        protected override void OnResume()
        {
        }
    }
}
