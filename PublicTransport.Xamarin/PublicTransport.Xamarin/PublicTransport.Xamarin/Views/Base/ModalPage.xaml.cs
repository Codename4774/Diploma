using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PublicTransport.Xamarin.Views.Base
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModalPage : BaseContentPage
    {
        public Action OnUpdate { set; get; }
        public ModalPageViewModel ViewModel => this.BindingContext as ModalPageViewModel;
        //private Color _navBarColor = Color.Transparent;

        public ModalPage()
		{
		    InitializeComponent();

            //ShowBackCross = true;
            // set VM
            this.BindingContext = new ModalPageViewModel();
        }

        protected void Init()
        {
            ViewModel.OnClose = async () =>
            {
                await Navigation.PopModalAsync();
                OnUpdate?.Invoke();
            };
        }

        public override Task Initialize()
        {
            return Task.FromResult(false);
        }

        protected override void OnAppearing()
        {
            //base.OnAppearing();
            //Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
        }

        protected override void OnDisappearing()
        {
            //base.OnDisappearing();
            //Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
        }
    }
}