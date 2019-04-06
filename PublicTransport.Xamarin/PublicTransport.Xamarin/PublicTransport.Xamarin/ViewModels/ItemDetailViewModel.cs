using System;

using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.ViewModels.Base;

namespace PublicTransport.Xamarin.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
