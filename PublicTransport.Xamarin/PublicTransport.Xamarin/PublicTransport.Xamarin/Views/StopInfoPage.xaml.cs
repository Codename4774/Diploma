using PublicTransport.Xamarin.Services.MapManager;
using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StopInfoPage : BaseContentPage
	{
		public StopInfoPage()
		{         
            InitializeComponent();
            BindingContext = new StopInfoViewModel(mapStop);
        }

        public void MoveCamera()
        {
            mapStop.MoveCamera(CameraUpdateFactory.NewPositionZoom(
                mapStop.Pins.First().Position, 16d));
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            FindedItemViewModel model = (FindedItemViewModel)(((ViewCell)sender).BindingContext);

            model.OpenDetailsCommand.Execute(null);
        }

        private void mapStop_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
        {
            MoveCamera();
        }
    }
}