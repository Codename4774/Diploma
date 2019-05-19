using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindPage : BaseContentPage
    {
		public FindPage ()
		{
			InitializeComponent();
            ListViewFindedItem.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            FindedItemViewModel model = (FindedItemViewModel)(((ViewCell)sender).BindingContext);

            model.OpenDetailsCommand.Execute(null);
        }
    }
}