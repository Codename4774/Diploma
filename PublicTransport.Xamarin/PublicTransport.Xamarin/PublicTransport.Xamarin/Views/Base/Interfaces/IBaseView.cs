using System.Threading.Tasks;
using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.Base;

namespace PublicTransport.Xamarin.Views.Base.Interfaces
{
    public interface IBaseView
    {
        BaseViewModel ViewModel { get; }
        Task Initialize();
    }
}
