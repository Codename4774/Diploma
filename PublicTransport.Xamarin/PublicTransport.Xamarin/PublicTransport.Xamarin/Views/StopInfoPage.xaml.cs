﻿using PublicTransport.Xamarin.Services.MapManager;
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
        private string mapStyle = "[{\"featureType\": \"transit.station\",\"stylers\": [{\"visibility\": \"off\"}]}]";

        public StopInfoPage()
		{         
            InitializeComponent();

            ListViewNearestArrive.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };

            mapStop.MapStyle = MapStyle.FromJson(mapStyle);

            BindingContext = new StopInfoViewModel(mapStop);
        }

        public void MoveCamera()
        {
            mapStop.MoveCamera(CameraUpdateFactory.NewPositionZoom(
                mapStop.Pins.First().Position, 16d));
            mapStop.UiSettings.RotateGesturesEnabled = false;
            mapStop.UiSettings.ScrollGesturesEnabled = false;
            mapStop.UiSettings.ZoomGesturesEnabled = false;
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

        private void DisplayNearestArrive_Clicked(object sender, EventArgs e)
        {
            StackLayoutTripsItem.IsVisible = false;
            StackLayoutNearestArrive.IsVisible = true;

            StackLayoutTripsItem.ForceLayout();
            StackLayoutNearestArrive.ForceLayout();
        }



        private void DisplayRoutes_Clicked(object sender, EventArgs e)
        {
            StackLayoutNearestArrive.IsVisible = false;
            StackLayoutTripsItem.IsVisible = true;

            StackLayoutNearestArrive.ForceLayout();
            StackLayoutTripsItem.ForceLayout();
        }
    }
}