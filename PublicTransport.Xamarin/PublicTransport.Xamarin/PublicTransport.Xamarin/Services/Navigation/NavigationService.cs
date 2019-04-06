using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.Views;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;
using Device = Xamarin.Forms.Device;

namespace PublicTransport.Xamarin.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private INavigation _navigation 
        {
            get
            {
                var mainPage = Application.Current.MainPage;
                if (mainPage is NavigationPage navigationPage)
                {
                    return navigationPage.Navigation;
                }
                else if (mainPage is MasterDetailPage masterDetailPage)
                {
                    return (masterDetailPage.Detail as NavigationPage)?.Navigation;
                }
                return null;
            }
        }

        private readonly EventWaitHandle _lockerEventHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
        private int _lockTimeout { get; set; } = 0;

        public async Task<TViewModel> OpenAsync<TViewModel>(object parameter, Action<TViewModel> initCallback = null, bool asPrimary = false)
            where TViewModel : BaseViewModel
        {
            void CastCallback(BaseViewModel model)
            {
                initCallback?.Invoke((TViewModel)model);
            }

            BaseViewModel task = await OpenAsync(typeof(TViewModel), parameter, CastCallback, asPrimary);
            return (TViewModel)task;
        }

        public async Task<BaseViewModel> OpenAsync(Type vm, object parameter = null, Action<BaseViewModel> initCallback = null, bool asPrimary = false)
        {
            BaseContentPage page = CreatePage(vm);
            if (!(page.BindingContext is BaseViewModel pageViewModel))
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;
            //if (Orienataion.IsLandscapeMode && asPrimary)
            //{
            //    _lockerEventHandle.Set();
            //}

            //if (!IsNotBarrier())
            //    return pageViewModel;


            //var topPage = UpdatePage(asPrimary, page);
            //if (topPage != null)
            //{
            //    page = topPage;

            //}

            initCallback?.Invoke(pageViewModel);
            
            pageViewModel.Initialize(parameter);
            await page.Initialize();
            //await SplitPageController.OpenPage(page, asPrimary);
            await _navigation.PushAsync(page);
            //if (topPage == null)
            //{
            //    initCallback?.Invoke(pageViewModel);
            //    page.Initialize();
            //    pageViewModel.Initialize(parameter);
            //    await SplitPageController.OpenPage(page, asPrimary);
            //}
            //else
            //{
            //    initCallback?.Invoke(page.ViewModel);
            //    page.Initialize();
            //    page.ViewModel.Initialize(parameter);
            //}

            await pageViewModel.InitializeAsync(parameter);

            return pageViewModel;
        }

        //private static BaseContentPage UpdatePage(bool asPrimary, Page page)
        //{
        //    Page topPage = SplitPageController.DetailNavigationPage?.CurrentPage;

        //    if (topPage?.GetType() == page.GetType() && asPrimary && Device.RuntimePlatform == Device.Android && Orienataion.IsLandscapeMode)
        //    {
        //        //topPage.BindingContext = page.BindingContext;
        //        return (BaseContentPage)topPage;
        //    }

        //    return null;
        //}


        public async Task<TViewModel> OpenModalAsync<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null)
            where TViewModel : BaseViewModel
        {
            BaseContentPage page = CreatePage(typeof(TViewModel));
            TViewModel pageViewModel = page.BindingContext as TViewModel;
            if (pageViewModel == null)
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;
            //if (!IsNotBarrier())
            //    return pageViewModel;

            initCallback?.Invoke(pageViewModel);
            page.Initialize();
            pageViewModel.Initialize(parameter);

            await _navigation.PushModalAsync(page);

            await pageViewModel.InitializeAsync(parameter);

            return pageViewModel;
        }

        public async Task<TViewModel> OpenPopupAsync<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null)
            where TViewModel : BaseViewModel
        {
            PopupBase popupPage = CreatePopupPage(typeof(TViewModel));
            TViewModel pageViewModel = popupPage.BindingContext as TViewModel;
            if (pageViewModel == null)
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;
            //if (!IsNotBarrier())
            //    return pageViewModel;

            initCallback?.Invoke(pageViewModel);
            popupPage.Initialize();
            pageViewModel.Initialize(parameter);

            await PopupNavigation.PushAsync(popupPage);

            await pageViewModel.InitializeAsync(parameter);

            return pageViewModel;
        }

        public async Task CloseAsync(BaseViewModel sender, bool isCallbackInvoke = true, object param = null)
        {
            if (sender == null)
                return;

            _lockTimeout = 500;
            //if (!IsNotBarrier())
            //    return;

            Page currentPage = GetPageByViewModel(sender);

            if (currentPage == null)
                return;

            if (currentPage is PopupBase)
                await PopupNavigation.PopAsync();

            else if (_navigation.ModalStack.LastOrDefault() == currentPage)
                await _navigation.PopModalAsync();
            else if (_navigation.NavigationStack.LastOrDefault() == currentPage)
                await _navigation.PopAsync();

            //if (Orienataion.IsLandscapeMode)
            //{
            //    if (SplitPageController.DetailNavigationPage?.Navigation?.NavigationStack?.LastOrDefault() == currentPage)
            //    {
            //        if (SplitPageController.DetailNavigationPage?.Navigation?.NavigationStack.Count > 1)
            //            await SplitPageController.DetailNavigationPage.Navigation.PopAsync();
            //        else
            //            return;
            //    }

            //    ;
            //}

            Device.BeginInvokeOnMainThread(() =>
            {
                if (isCallbackInvoke)
                {
                    if (param != null)
                        sender.CloseCallbackParam?.Invoke(param);
                    else
                        sender.CloseCallback?.Invoke();
                }

                sender.Dispose();

                if (currentPage is BaseContentPage baseContentPage)
                    baseContentPage.Dispose();
                else if (currentPage is PopupBase popupBase)
                    popupBase.Dispose();
            });
        }

        public async Task<TNewViewModel> InsertPageBeforePageAsync<TNewViewModel, TOldViewModel>(object parameter = null) where TOldViewModel : BaseViewModel
            where TNewViewModel : BaseViewModel
        {
            BaseContentPage newPage = CreatePage(typeof(TNewViewModel));
            TNewViewModel newPageViewModel = newPage.BindingContext as TNewViewModel;
            if (newPageViewModel == null)
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;
            //if (!IsNotBarrier())
            //    return newPageViewModel;

            newPageViewModel.Initialize();
            newPageViewModel.Initialize(parameter);

            BaseContentPage oldPage = null;
            foreach (var page in _navigation.NavigationStack.AsEnumerable())
            {
                if (page.BindingContext?.GetType() == typeof(TOldViewModel))
                {
                    oldPage = page as BaseContentPage;
                    break;
                }
            }

            if (oldPage == null)
                return null;

            _navigation.InsertPageBefore(newPage, oldPage);

            await newPageViewModel.InitializeAsync(parameter);

            return newPageViewModel;
        }

        public async Task<TViewModel> ChangeRootPageBy<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
        {
            BaseContentPage newRoot = CreatePage(typeof(TViewModel));
            TViewModel newRootViewModel = newRoot.BindingContext as TViewModel;
            if (newRootViewModel == null)
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;
            //if (!IsNotBarrier())
            //    return newRootViewModel;

            newRootViewModel.Initialize();
            newRootViewModel.Initialize(parameter);

            _navigation.InsertPageBefore(newRoot, _navigation.NavigationStack[0]);

            return newRootViewModel;
        }

        public async Task GoToRoot()
        {
            await _navigation.PopToRootAsync();
        }

        public async Task RemovePreviousIf<TViewModel>(BaseViewModel sender, int deepOffset = 0)
        {
            Page currentPage = GetPageByViewModel(sender);

            //if (Orienataion.IsLandscapeMode && SplitPageController.DetailNavigationPage != null)
            //{
            //    if (currentPage is PopupBase || _navigation.ModalStack?.LastOrDefault() == currentPage)
            //        deepOffset--;

            //    if (deepOffset < 0)
            //        deepOffset = 0;


            //    if (deepOffset == 0)
            //    {
            //        BaseContentPage baseContentPage = SplitPageController.DetailNavigationPage.Navigation.NavigationStack.LastOrDefault() as BaseContentPage;

            //        if (baseContentPage?.ViewModel?.GetType() == typeof(TViewModel))
            //        {
            //            if (SplitPageController.DetailNavigationPage.Navigation.NavigationStack.Count == 1)
            //                SplitPageController.DetailNavigationPage.Navigation.InsertPageBefore(new StubEmptyPage(), baseContentPage);

            //            await SplitPageController.DetailNavigationPage.Navigation.PopAsync();
            //        }
            //    }

            //    return;
            //}

            int backIndex = 0;

            //if try to remove PopupBase and current page is PopupBase as well then backIndex will be 2
            backIndex = (currentPage is PopupBase) ? 2 : 1 + deepOffset;

            PopupBase popopPage = PopupNavigation.PopupStack.Count >= backIndex
                ? PopupNavigation.PopupStack[PopupNavigation.PopupStack.Count - backIndex] as PopupBase
                : null;
            if (popopPage?.ViewModel?.GetType() == typeof(TViewModel))
            {
                await PopupNavigation.RemovePageAsync(popopPage);
                return;
            }

            if (_navigation.ModalStack.Count > 0)
            {
                //if try to remove modal page and current page is modal page as well then backIndex will be 2
                backIndex = (_navigation.ModalStack.LastOrDefault()?.Equals(currentPage) ?? false) ? 2 : 1 + deepOffset;

                BaseContentPage modalPage = _navigation.ModalStack.Count >= backIndex
                    ? _navigation.ModalStack[_navigation.ModalStack.Count - backIndex] as BaseContentPage
                    : null;
                if (modalPage?.ViewModel?.GetType() == typeof(TViewModel))
                {
                    _navigation.RemovePage(modalPage);
                    return;
                }
            }

            //if try to remove page and current page is not included in main stack then backIndex will be 2
            backIndex = (_navigation.NavigationStack.LastOrDefault()?.Equals(currentPage) ?? false) ? 2 : 1 + deepOffset;
            BaseContentPage page = _navigation.NavigationStack.Count >= backIndex
                ? _navigation.NavigationStack[_navigation.NavigationStack.Count - backIndex] as BaseContentPage
                : null;
            if (page?.ViewModel?.GetType() == typeof(TViewModel))
            {
                _navigation.RemovePage(page);
                return;
            }
        }

        public async Task OpenMasterDetailPage<TDetailPageViewModel, TMasterPageViewModel>(object parameter = null, Action<TDetailPageViewModel> initCallback = null, Action<TMasterPageViewModel> initMasterPageCallback = null) where TDetailPageViewModel: BaseViewModel
            where TMasterPageViewModel : BaseViewModel
        {
            var detailPage = await CreatePageFromViewModel<TDetailPageViewModel>(parameter, initCallback);
            var masterDetailPage = CreateMasterDetailPage(detailPage, initMasterPageCallback);
            Application.Current.MainPage = masterDetailPage;
        }

        private MasterDetailPage CreateMasterDetailPage<TMasterViewModel>(BaseContentPage detailPage ,Action<TMasterViewModel> initCallback) where TMasterViewModel : BaseViewModel
        {
            var masterDetailPage = new MainMenuPage(detailPage);
            var masterPageViewModel = (masterDetailPage.Master.BindingContext as TMasterViewModel);
            masterPageViewModel?.InitializeAsync();
            initCallback?.Invoke(masterPageViewModel);

            return masterDetailPage;
        }

        public async Task ChangeDetailPage<TDetailPageViewModel>(object parameter = null, Action<TDetailPageViewModel> initCallback = null) where TDetailPageViewModel : BaseViewModel
        {
            if (Application.Current.MainPage is MainMenuPage masterDetail)
            {
                var existingDetailPageType = masterDetail.Detail.GetType();
                if (!IsClosedPageEqualToNewPage(existingDetailPageType, typeof(TDetailPageViewModel)))
                {
                    var detailPage = await CreatePageFromViewModel(parameter, initCallback);
                    masterDetail.ChangeDetailPage(detailPage);
                }
            }
            else
            {
                throw new Exception("Current MainPage is not master detail");
            }
        }

        public async Task CloseMasterDetail<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null) where TViewModel : BaseViewModel
        {
            var page = await CreatePageFromViewModel(parameter, initCallback);
            Application.Current.MainPage = new NavigationPage(page);
        }

        private bool IsClosedPageEqualToNewPage(Type previousPageType, Type newPageViewModelType)
        {
            var newPageTypeName = newPageViewModelType.FullName.Replace("ViewModels", "Views").Replace("ViewModel", "Page");
            if (previousPageType.FullName.Equals(newPageTypeName))
            {
                return true;
            }

            return false;
        }

        private async Task<BaseContentPage> CreatePageFromViewModel<TDetailPageViewModel>(object parameter, Action<TDetailPageViewModel> initCallback) where TDetailPageViewModel : BaseViewModel
        {
            BaseContentPage page = CreatePage(typeof(TDetailPageViewModel));
            TDetailPageViewModel pageViewModel = page.BindingContext as TDetailPageViewModel;
            if (pageViewModel == null)
                return null;

            _lockTimeout = Device.RuntimePlatform == Device.iOS ? 1000 : 3000;

            pageViewModel.Initialize(parameter);
            initCallback?.Invoke(pageViewModel);
            await pageViewModel.InitializeAsync(parameter);
            await page.Initialize();
            return page;
        }

        private Page GetPageByViewModel(BaseViewModel param)
        {
            if (param == null)
                return null;

            PopupBase popopPage = PopupNavigation.PopupStack.LastOrDefault() as PopupBase;
            if (popopPage?.ViewModel?.Equals(param) ?? false)
                return popopPage;

            BaseContentPage modalPage = _navigation.ModalStack.LastOrDefault() as BaseContentPage;
            if (modalPage?.ViewModel?.Equals(param) ?? false)
                return modalPage;

            BaseContentPage page = _navigation.NavigationStack.LastOrDefault() as BaseContentPage;
            if (page?.ViewModel?.Equals(param) ?? false)
                return page;

            //if (Orienataion.IsLandscapeMode)
            //{
            //    BaseContentPage splitPage = SplitPageController.DetailNavigationPage?.Navigation?.NavigationStack.LastOrDefault() as BaseContentPage;
            //    if (splitPage?.ViewModel?.Equals(param) ?? false)
            //        return splitPage;
            //}

            return null;
        }

        public BaseContentPage CreatePage(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModels", "Views").Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var pageType = Type.GetType(viewAssemblyName);
            if (pageType == null)
                throw new Exception($"Cannot locate page type for {viewModelType}");
            BaseContentPage page = null;
            try
            {
                page = Activator.CreateInstance(pageType) as BaseContentPage;
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            if (page != null)
                page.BindingContext = Activator.CreateInstance(viewModelType);

            return page;
        }

        private PopupBase CreatePopupPage(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModels", "Views").Replace("PopupViewModel", "Popup").Replace("ViewModel", "Popup");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            Type pageType = Type.GetType(viewAssemblyName);

            if (pageType == null)
                throw new Exception($"Cannot locate page type for {viewModelType}");

            PopupBase popup = Activator.CreateInstance(pageType) as PopupBase;

            if (popup != null)
                popup.BindingContext = Activator.CreateInstance(viewModelType);

            return popup;
        }

        //private bool IsNotBarrier()
        //{
        //    if (App.UserModule.Session.IsForegroundTimeoutExpired())
        //        return false;

        //    if (!CrossConnectivity.Current.IsConnected)
        //    {
        //        App.ErrorService.BadConnectionErrorAlert();
        //        return false;
        //    }

        //    if (!_lockerEventHandle.WaitOne(1))
        //    {
        //        Console.WriteLine("NavigationService.WaitOne");
        //        return false;
        //    }

        //    //activate locker
        //    _lockerEventHandle.Reset();
        //    Console.WriteLine("NavigationService.Reset");

        //    Task.Run(async () =>
        //    {
        //        await Task.Delay(_lockTimeout);
        //        Console.WriteLine("NavigationService.Set");
        //        _lockerEventHandle.Set();
        //    });

        //    return true;
        //}

        public void ResetLocker()
        {
            Console.WriteLine("NavigationService.Set");
            _lockerEventHandle.Set();
        }
    }
}