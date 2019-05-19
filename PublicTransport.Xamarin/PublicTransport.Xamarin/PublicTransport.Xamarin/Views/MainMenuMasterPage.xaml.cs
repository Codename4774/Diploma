using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
	public partial class MainMenuMasterPage : ContentPage
	{
		public MainMenuMasterPage()
		{
			InitializeComponent();

            listViewMenu.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };

            BindingContext = new MainMenuMasterViewModel();
		}
	}
}