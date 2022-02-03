using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace WeatherApp
{
    public class Rootobject
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current current { get; set; }
        public Hourly[] hourly { get; set; }
        public Daily[] daily { get; set; }
    }

    public class Current
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        public float wind_gust { get; set; }
        public Weather[] weather { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        private string _icon;
        public string icon 
        {
            get
            {
                return $"i{_icon}.png";
            } 
            set { _icon = value; }
        }
    }

    public class Hourly
    {
        private int _dt;
        public int dt
        {
            get { return _dt; }
            set 
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(value).ToLocalTime();
                time = $"{dateTime.ToString("HH:mm")}";
                _dt = value;
            }
        }

        public string time { get; set; }
        public string temperature { get; set; }
        private float _temp;
        public float temp
        {
            get
            {
                return _temp;
            }
            set 
            { 
               temperature = $"{value.ToString("0")}°C";
               _temp = value;
            }
        }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        public float wind_gust { get; set; }
        public Weather[] weather { get; set; }
        public float pop { get; set; }
    }

    public class Daily
    {

private int _dt;
        public int dt
        {
            get { return _dt; }
            set
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(value).ToLocalTime();
                date = $"{dateTime.ToString("ddd MMM dd")}";
                _dt = value;
            }
        }
        public string temperature 
        {
            get { return $"{temp.day.ToString("0")}/{temp.night.ToString("0")}{MainPage.unitsTemp}"; }
        }
        public string date { get; set; }
        public int sunrise { get; set; }
        //string sunrise
        public string Sunrise { get { return ConvertTime(sunrise); } }
        public int sunset { get; set; }
        //string susnet
        public string Sunset { get { return ConvertTime(sunset); } }
        public int moonrise { get; set; }
        public int moonset { get; set; }
        public float moon_phase { get; set; }
        public Temp temp { get; set; }
        public Feels_Like feels_like { get; set; }
        public string Pressure { get { return $"{pressure} hpa"; } }
        public int pressure { get; set; }
        //string humidity
        public string Humidity { get { return $"{humidity}%"; } }
        public int humidity { get; set; }
        //string Dew point
        public string Dew_point { get { return $"{dew_point}{MainPage.unitsTemp}"; } }
        public float dew_point { get; set; }
        //string wind
        public string Wind { get { return $"{wind_speed}{MainPage.unitsSpeed}"; } }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        public float wind_gust { get; set; }
        public Weather[] weather { get; set; }
        //string Clouds
        public string Clouds { get { return $"{clouds}%"; } }
        public int clouds { get; set; }
        public float pop { get; set; }
        //string UV index
        public string UVindex { get { return $"{uvi}%"; } }
        public float uvi { get; set; }
        public float snow { get; set; }
        public static string ConvertTime(int time)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(time).ToLocalTime();
            return $"{dateTime.ToString("HH:mm")}";
            //_dt = value;
        }
    }

    public class Temp
    {
        public float day { get; set; }
        public float min { get; set; }
        public float max { get; set; }
        public float night { get; set; }
        public float eve { get; set; }
        public float morn { get; set; }
    }

    public class Feels_Like
    {
        public float day { get; set; }
        public float night { get; set; }
        public float eve { get; set; }
        public float morn { get; set; }
    }

    public class Location
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string name { get; set; }
    }
}
