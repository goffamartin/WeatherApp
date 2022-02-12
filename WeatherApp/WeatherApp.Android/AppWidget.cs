using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherApp.Droid
{
	[BroadcastReceiver(Label = "WeatherApp Widget")]
	[IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
	// The "Resource" file has to be all in lower caps
	[MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
	public class AppWidget : AppWidgetProvider
	{
		private static string AnnouncementClick = "AnnouncementClickTag";

		/// <summary>
		/// This method is called when the 'updatePeriodMillis' from the AppwidgetProvider passes,
		/// or the user manually refreshes/resizes.
		/// </summary>
		public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
		{
			var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
			appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
		}

		private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
		{
			// Retrieve the widget layout. This is a RemoteViews, so we can't use 'FindViewById'
			var widgetView = new RemoteViews(context.PackageName, Resource.Layout.weather_widget);

			SetTextViewText(widgetView);
			RegisterClicks(context, appWidgetIds, widgetView);

			return widgetView;
		}

		private void SetTextViewText(RemoteViews widgetView)
		{
			MainPage mp = new MainPage();
			mp.GetCurrentWeather();
			widgetView.SetTextViewText(Resource.Id.textWeather, MainPage.info.current.weather[0].description[0].ToString().ToUpper() + MainPage.info.current.weather[0].description.Substring(1));
			widgetView.SetTextViewText(Resource.Id.textPlace, GetShortPlaceName(MainPage.placeName1));
			widgetView.SetTextViewText(Resource.Id.textTemperature, MainPage.info.current.temp.ToString("0") + MainPage.unitsTemp);
			widgetView.SetImageViewResource(Resource.Id.imageWeather, GetImageId(MainPage.info.current.weather[0].icon));
		}
		private string GetShortPlaceName(string placeName)
        {
			if(placeName.Length <= 17)
				return placeName;
            else
            {
				int space = placeName.IndexOf(" ");
				return placeName.Substring(0, space);
            }
        }
		
		private int GetImageId(string name)
        {
            switch (name)
            {
				case "i01d.png":
					return Resource.Drawable.i01d;
				case "i01n.png":
					return Resource.Drawable.i01n;
				case "i02d.png":
					return Resource.Drawable.i02d;
				case "i02n.png":
					return Resource.Drawable.i02n;
				case "i03d.png":
					return Resource.Drawable.i03d;
				case "i03n.png":
					return Resource.Drawable.i03n;
				case "i04d.png":
					return Resource.Drawable.i04d;
				case "i04n.png":
					return Resource.Drawable.i04n;
				case "i09d.png":
					return Resource.Drawable.i09d;
				case "i09n.png":
					return Resource.Drawable.i09n;
				case "i10d.png":
					return Resource.Drawable.i10d;
				case "i10n.png":
					return Resource.Drawable.i10n;
				case "i11d.png":
					return Resource.Drawable.i11d;
				case "i11n.png":
					return Resource.Drawable.i11n;
				case "i13d.png":
					return Resource.Drawable.i13d;
				case "i13n.png":
					return Resource.Drawable.i13n;
				case "i50d.png":
					return Resource.Drawable.i50d;
				case "i50n.png":
					return Resource.Drawable.i50n;
					default:
					return 0;

			}
        }

		private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
		{
			SetTextViewText(widgetView);
			var intent = new Intent(context, typeof(AppWidget));
			intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
			intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

			// Register click event for the Background
			var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
			widgetView.SetOnClickPendingIntent(Resource.Id.widgetBackground, piBackground);
			// Register click event for Weather image
			widgetView.SetOnClickPendingIntent(Resource.Id.imageWeather, GetPendingSelfIntent(context, AnnouncementClick));
		}

		private PendingIntent GetPendingSelfIntent(Context context, string action)
		{
			var intent = new Intent(context, typeof(AppWidget));
			intent.SetAction(action);
			return PendingIntent.GetBroadcast(context, 0, intent, 0);
		}

		/// <summary>
		/// This method is called when clicks are registered.
		/// </summary>
		public override void OnReceive(Context context, Intent intent)
		{
			base.OnReceive(context, intent);

            // Check if the click is from the "Announcement" button
            if (AnnouncementClick.Equals(intent.Action))
            {
                var pm = context.PackageManager;
                try
                {
                    var packageName = "com.companyname.weatherapp";
                    var launchIntent = pm.GetLaunchIntentForPackage(packageName);
                    context.StartActivity(launchIntent);
                }
                catch
                {
                    // Something went wrong :)
                }
            }
        }
	}
}