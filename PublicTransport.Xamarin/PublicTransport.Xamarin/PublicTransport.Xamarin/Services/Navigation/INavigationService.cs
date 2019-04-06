using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Threading.Tasks;
using PublicTransport.Xamarin.Views.Base.CodeBehind;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Services.Navigation
{
    public interface INavigationService
    {
        Task<TViewModel> OpenAsync<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null, bool asPrimary = false)
            where TViewModel : BaseViewModel;

        Task<BaseViewModel> OpenAsync(Type vm, object parameter = null, Action<BaseViewModel> initCallback = null, bool asPrimary = false);

        Task<TViewModel> OpenModalAsync<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null) where TViewModel : BaseViewModel;

        Task<TViewModel> OpenPopupAsync<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null) where TViewModel : BaseViewModel;

        Task CloseAsync(BaseViewModel sender, bool isCallbackInvoke = true, object param = null);

        Task<TNewViewModel> InsertPageBeforePageAsync<TNewViewModel, TOldViewModel>(object parameter = null) where TOldViewModel : BaseViewModel
            where TNewViewModel : BaseViewModel;

        Task<TViewModel> ChangeRootPageBy<TViewModel>(object parametr = null) where TViewModel : BaseViewModel;

        Task GoToRoot();

        Task RemovePreviousIf<TViewModel>(BaseViewModel sender, int deepOffset = 0);

        Task OpenMasterDetailPage<TDetailPageViewModel, TMasterViewModelPage>(object parameter = null,
            Action<TDetailPageViewModel> initCallback = null,
            Action<TMasterViewModelPage> initMasterPageCallback = null) where TDetailPageViewModel : BaseViewModel
            where TMasterViewModelPage : BaseViewModel;

        Task ChangeDetailPage<TDetailPageViewModel>(object parameter = null,
            Action<TDetailPageViewModel> initCallback = null) where TDetailPageViewModel : BaseViewModel;

        Task CloseMasterDetail<TViewModel>(object parameter = null, Action<TViewModel> initCallback = null) where TViewModel : BaseViewModel;

        void ResetLocker();

    }
}