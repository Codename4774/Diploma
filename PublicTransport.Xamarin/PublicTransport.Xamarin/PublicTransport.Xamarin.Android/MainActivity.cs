using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.GoogleMaps.Android;
using Android.Content.Res;
using System.IO;

namespace PublicTransport.Xamarin.Droid
{
    [Activity(Label = "PublicTransport.Xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //this.Assets.
            // Override default BitmapDescriptorFactory by your implementation. 
            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            WriteGTFSFiles();
            global::Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig); // initialize for Xamarin.Forms.GoogleMaps
            LoadApplication(new App());
        }

        private void WriteGTFSFiles()
        {
            AssetManager assets = this.Assets;

            string dirDest = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            Directory.CreateDirectory(Path.Combine(dirDest, "GTFS"));

            CopyFileFromAsset(assets, "GTFS/agency.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/calendar.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/calendar.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/routes.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/shapes.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/stop_times.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/stops.txt", dirDest);
            CopyFileFromAsset(assets, "GTFS/trips.txt", dirDest);
        }

        private void CopyFileFromAsset(AssetManager assets, string fileName, string dirDestPath)
        {
            string fileDestPath = Path.Combine(dirDestPath, fileName);
            //if (!File.Exists(fileDestPath))
            //{
            File.Delete(fileDestPath);
            using (StreamReader sr = new StreamReader(assets.Open(fileName)))
            {
                using (StreamWriter streamWriter = File.CreateText(fileDestPath))
                {
                    streamWriter.Write(sr.ReadToEnd());
                }
            }
            //}
        }
    }
}