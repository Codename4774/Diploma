using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
    public partial class MainMenuPage : MasterDetailPage
    {
        public MainMenuPage(ContentPage detailPage)
        {
            InitializeComponent();      
            Detail = new NavigationPage(detailPage);
            IsPresented = false;
        }

        public void ChangeDetailPage(ContentPage detailPage)
        {
            Detail = new NavigationPage(detailPage);
            IsPresented = false;
        }
    }
}