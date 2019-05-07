using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PublicTransport.Backend.Services;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.Services.Navigation;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.Views.ListViewModels;
using PublicTransport.Xamarin;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels
{
    public class MainMenuMasterViewModel : BaseViewModel
    {

        #region BindProperties
        
        public ObservableCollection<MenuElement> MenuElements { get; } = new ObservableCollection<MenuElement>();

        private MenuElement _selectedMenuElement;

        public MenuElement SelectedMenuElement
        {
            get => _selectedMenuElement;
            set
            {
                _selectedMenuElement = value;
                _selectedMenuElement?.MenuAction?.Invoke();
            }
        }
        #endregion
        

        protected override void InitializeServices()
        {
            base.InitializeServices();
        }


        public override async Task InitializeAsync(object navigationData = null)
        {
            AddMenuElements();
        }

        private void AddMenuElements()
        {
            //MenuElements.Add(new MenuElement()
            //{
            //    MenuElementText = "Map",
            //    MenuAction = async () =>
            //    {
            //        await ServiceProvider.NavigationService.ChangeDetailPage<MainViewModel>();
            //    }
            //});
            MenuElements.Add(new MenuElement()
            {
                MenuElementText = "Find",
                MenuAction = async () =>
                {
                    await ServiceProvider.NavigationService.OpenAsync<FindViewModel>();
                }
            });
            MenuElements.Add(new MenuElement()
            {
                MenuElementText = "Favorite list",
                MenuAction = async() =>
                {
                    await ServiceProvider.NavigationService.OpenAsync<FavoriteListViewModel>();
                }
            });
        }
    }
}
