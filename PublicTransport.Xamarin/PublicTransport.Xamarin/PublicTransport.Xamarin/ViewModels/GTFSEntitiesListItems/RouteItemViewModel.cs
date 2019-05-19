using GTFS.Entities;
using PublicTransport.Backend.Services.Shedule;
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
        private readonly ISheduleManager _sheduleManager;


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
                return CommonMethods.GetIconForRouteType(_sheduleManager.GetRouteTypeSimple(Route.Type));
            }
        }
        public override string Title
        {
            get
            {
                return "Route number: " + Route.ShortName + Environment.NewLine + Route.LongName;
            }
        }

        public RouteItemViewModel(Route stop) : base(stop)
        {
            _sheduleManager = ServiceProvider.SheduleManager;
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

