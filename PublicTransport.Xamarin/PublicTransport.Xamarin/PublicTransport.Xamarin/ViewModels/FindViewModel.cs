using GTFS.Entities;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels
{
    public class FindViewModel : BaseViewModel
    {
        public ObservableCollection<FindedItemViewModel> FindedItems { get; } = new ObservableCollection<FindedItemViewModel>();

        public string FindByText
        {
            get;
            set;
        }

        private IGTFSProvider _GTFSProvider;

        public FindViewModel()
        {
            _GTFSProvider = ServiceProvider.GTFSProvider;
        }

        public ICommand _findCommand;
        public ICommand FindCommand
        {
            get
            {
                _findCommand = _findCommand ?? new Command(async () =>
                {
                    FindElements(FindByText);
                });
                return _findCommand;
            }
        }

        private void FindElements(string textForSearch)
        {
            List<FindedItemViewModel> resultedList = new List<FindedItemViewModel>();

            resultedList.AddRange(FindInRoutes(textForSearch));
            resultedList.AddRange(FindInStops(textForSearch));

            IOrderedEnumerable<FindedItemViewModel> orderedResult = resultedList.OrderBy<FindedItemViewModel, string>(item => item.Title);

            FindedItems.Clear();

            foreach (FindedItemViewModel item in orderedResult)
            {
                FindedItems.Add(item);
            }
        }

        private IEnumerable<FindedItemViewModel> FindInRoutes(string textForSearch)
        {
            List<FindedItemViewModel> result = new List<FindedItemViewModel>();

            foreach (Route route in _GTFSProvider.GTFSFeed.Routes)
            {
                if (route.ShortName.Contains(textForSearch) || route.LongName.Contains(textForSearch))
                {
                    result.Add(new RouteItemViewModel(route));
                }
            }

            return result;
        }

        private IEnumerable<FindedItemViewModel> FindInStops(string textForSearch)
        {
            List<FindedItemViewModel> result = new List<FindedItemViewModel>();

            foreach (Stop stop in _GTFSProvider.GTFSFeed.Stops)
            {
                if (stop.Name.Contains(textForSearch))
                {
                    result.Add(new StopItemViewModel(stop));
                }
            }

            return result;
        }
    }
}
