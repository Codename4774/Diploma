using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool ShowDetailsPage { get; set; }

        private ICommand _openDetailsPage;
        public ICommand OpenDetailsPageCommand
        {
            get
            {
                _openDetailsPage = _openDetailsPage ?? new Command(async () =>
                {
                    ShowDetailsPage = true;
                });
                return _openDetailsPage;
            }
        }
    }
}
