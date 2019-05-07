using GTFS.Entities;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Shedule;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems
{
    public class NearestArriveTimeViewModel : BaseViewModel
    {
        private ISheduleManager _sheduleManager;

        private NearestArriveTimeModel _nearestArriveTimeModel;

        private Route _route;

        public NearestArriveTimeModel NearestArriveTimeModel
        {
            get
            {
                return _nearestArriveTimeModel;
            }
        }

        public void UpdateFields()
        {
            OnPropertyChanged("MinutesToNextArriveTime");
        }

        public string Title
        {
            get
            {
                return
                    "Route: " + _route.ShortName + Environment.NewLine +
                    "Route type: " + CommonMethods.GetRouteTypeStr(_sheduleManager.GetRouteTypeSimple(_route.Type));
            }
        }

        public string MinutesToNextArriveTime
        {
            get
            {
                return _nearestArriveTimeModel.MinutesToArrive.ToString() + " min";
            }
            set
            {
                _nearestArriveTimeModel.MinutesToArrive = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        public ImageSource ImagePath
        {
            get
            {
                //todo: add custom icons for routes per type
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.ROUTE_ICON_FILE_PATH);
            }
        }

        public NearestArriveTimeViewModel(ISheduleManager sheduleManager, NearestArriveTimeModel nearestArriveTimeModel, Route route)
        {
            _sheduleManager = sheduleManager;
            _nearestArriveTimeModel = nearestArriveTimeModel;
            _route = route;
        }
    }
}
