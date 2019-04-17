using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : BaseContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainViewModel(map);
        }
    }
}