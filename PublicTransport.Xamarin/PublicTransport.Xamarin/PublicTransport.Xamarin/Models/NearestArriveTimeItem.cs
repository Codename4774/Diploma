
using PublicTransport.Backend.Models;
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
        private NearestArriveTimeModel _nearestArriveTimeModel;

        public ImageSource ImagePath
        {
            get
            {
                //todo: add custom icons for routes per type
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.ROUTE_ICON_FILE_PATH);
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
            _nearestArriveTimeModel = nearestArriveTimeModel;
            Title = title;
        }
    }
}
