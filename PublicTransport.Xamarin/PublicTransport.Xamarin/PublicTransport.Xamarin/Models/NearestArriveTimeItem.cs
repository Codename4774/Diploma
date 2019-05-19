
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Shedule;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Models
{
    public class NearestArriveTimeItem : BaseViewModel
    {
        private ISheduleManager _sheduleManager;

        private NearestArriveTimeModel _nearestArriveTimeModel;

        public ImageSource ImagePath
        {
            get
            {
                //todo: add custom icons for routes per type
                return CommonMethods.GetIconForRouteType(_sheduleManager.GetRouteTypeSimple(_nearestArriveTimeModel.Route.Type));
            }
        }

        public string Title { get; set; }

        public string MinutesToNextArriveTime
        {
            get
            {
                return _nearestArriveTimeModel.MinutesToArrive.ToString();
            }
            set
            {
                _nearestArriveTimeModel.MinutesToArrive = Convert.ToInt32(value);
                OnPropertyChanged();
            }
        }

        public NearestArriveTimeItem(NearestArriveTimeModel nearestArriveTimeModel, string title)
        {
            _sheduleManager = ServiceProvider.SheduleManager;
            _nearestArriveTimeModel = nearestArriveTimeModel;
            Title = title;
        }
    }
}
