using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : BaseContentPage
    {
        private string mapStyle = "[{\"featureType\": \"transit.station\",\"stylers\": [{\"visibility\": \"off\"}]}]";

        public MainPage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainViewModel(map);
            map.UiSettings.MyLocationButtonEnabled = true;
            map.MapStyle = MapStyle.FromJson(mapStyle);
        }

        private void map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            if (e.Position.Zoom >= 14)
            {
                ((MainViewModel)(ViewModel)).ShowAllStops();
                
            }
            else
            {
                ((MainViewModel)(ViewModel)).HideAllStops();
            }
        }

        private void map_PinClicked(object sender, PinClickedEventArgs e)
        {
            ((MainViewModel)(ViewModel)).OpenStopInfo(e.Pin);
        }
    }
}