using GTFS.Entities;
using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RouteInfoPage : BaseContentPage
    {
		public RouteInfoPage ()
		{
            InitializeComponent();
            Stops.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };
        }

        private void Stops_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RouteInfoViewModel model = (RouteInfoViewModel)(BindingContext);

            Stop stop = (Stop)(e.Item);

            ServiceProvider.NavigationService.OpenAsync<RouteStopInfoViewModel>(new RouteStopParameter() { Route = model.Route, Stop = stop, Direction = model.SelectedDirection });
        }
    }
}