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
    public class StopItemViewModel : FindedItemViewModel
    {
        protected Stop Stop
        {
            get
            {
                return (Stop)Entity;
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
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.STOP_ICON_FILE_PATH);
            }
        }
        public override string Title
        {
            get
            {
                return Stop.Name;
            }
        }

        public StopItemViewModel(Stop stop) : base(stop)
        {
        }

        public ICommand _openDetailsCommand;
        public override ICommand OpenDetailsCommand
        {
            get
            {
                _openDetailsCommand = _openDetailsCommand ?? new Command(async () =>
                {
                    //open details page for Stops
                });
                return _openDetailsCommand;
            }
        }
    }
}
