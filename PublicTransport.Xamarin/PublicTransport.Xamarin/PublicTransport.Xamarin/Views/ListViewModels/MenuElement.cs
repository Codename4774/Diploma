using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PublicTransport.Xamarin.Views.ListViewModels
{
    public class MenuElement
    {
        //public string MenuElementImage { get; set; }
        public string MenuElementText { get; set; }
        public Func<Task> MenuAction { get; set; }
    }
}
