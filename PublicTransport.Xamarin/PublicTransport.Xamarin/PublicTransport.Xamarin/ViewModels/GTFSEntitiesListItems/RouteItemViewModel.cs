using GTFS.Entities;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems
{
    public class RouteItemViewModel : FindedItemViewModel
    {
        protected Route Route
        {
            get
            {
                return (Route)Entity;
            }
            set
            {
                Entity = value;
            }
        }

        public override ImageSource ImagePath
        {
            get
            {
                //todo: add custom icons for routes per type
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.ROUTE_ICON_FILE_PATH);
            }
        }
        public override string Title
        {
            get
            {
                return Route.ShortName + Environment.NewLine + Route.LongName;
            }
        }

        public RouteItemViewModel(Route stop) : base(stop)
        {
        }

        public ICommand _openDetailsCommand;
        public override ICommand OpenDetailsCommand
        {
            get
            {
                _openDetailsCommand = _openDetailsCommand ?? new Command(async () =>
                {
                    await ServiceProvider.NavigationService.OpenAsync<RouteInfoViewModel>(Route);
                });
                return _openDetailsCommand;
            }
        }
    }
}

