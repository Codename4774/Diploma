using PublicTransport.Backend.Common;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.Services.Navigation;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.Base
{
    public class BaseViewModel : BaseServiceUser, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public BaseViewModel()
        {
            //DebugService.ObjectMonitor.AddObject(this.GetType());
        }

        ~BaseViewModel()
        {
            //DebugService.ObjectMonitor.RemoveObject(this.GetType());
        }

        public INavigationService _navigationService;

        // IsBusy
        readonly object _lockerIsBusy = new object();
        bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                lock (_lockerIsBusy)
                    return _isBusy;
            }
            set
            {
                lock (_lockerIsBusy)
                    if (_isBusy != value)
                    {
                        _isBusy = value;

                        OnPropertyChanged();
                    }
            }
        }

        // IsLoading
        readonly object _lockerIsLoading = new object();
        bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                lock (_lockerIsLoading)
                    return _isLoading;
            }
            set
            {
                lock (_lockerIsLoading)
                    if (_isLoading != value)
                    {
                        _isLoading = value;

                        OnPropertyChanged();
                    }
            }
        }

        // IsDisposed
        readonly object _lockerIsDisposed = new object();
        bool _isDisposed = false;
        public bool IsDisposed
        {
            get
            {
                lock (_lockerIsDisposed)
                    return _isDisposed;
            }
            set
            {
                lock (_lockerIsDisposed)
                    if (_isDisposed != value)
                    {
                        _isDisposed = value;

                        OnPropertyChanged();
                    }
            }
        }

        // BackPageTitle
        //private string _backPageTitle = string.Empty;
        //public string BackPageTitle
        //{
        //    get => _backPageTitle;
        //    set
        //    {
        //        _backPageTitle = value;
        //        OnPropertyChanged();
        //    }
        //}

        // BarTitle
        //private string _barTitle = string.Empty;
        //public string BarTitle
        //{
        //    get => _barTitle;
        //    set
        //    {
        //        _barTitle = value;
        //        OnPropertyChanged();
        //    }
        //}

        // AssignToolbarItems
        //public Action<ObservableCollection<ToolbarItemImageModel>> AssignToolbarItems { set; get; }

        // Loaded

        public virtual void OnLoaded()
        {
            Loaded?.Invoke(this, new EventArgs());
        }
        public event EventHandler Loaded;

        // OrientationChanged
        public virtual void OnOrientationChanged()
        {
            OrientationChanged?.Invoke(this, new EventArgs());
        }
        public event EventHandler OrientationChanged;

        public Action CloseCallback;
        public Action<object> CloseCallbackParam;
        public Func<Task> CloseAsync;
        public Action OnBackButtonClicked;
        public Action<object> OnUpdateCallback;

        //#region Fields
        //protected ObservableCollection<ToolbarItemImageModel> ToolbarItems = new ObservableCollection<ToolbarItemImageModel>();
        //#endregion Fields

        public virtual void Initialize(object navigationData = null)
        {
            //App.PageOrientationChanged += PageOrientationChanged;
        }

        public virtual Task InitializeAsync(object navigationData = null)
        {
            return Task.FromResult(false);
        }

        public virtual void Dispose()
        {
            Console.WriteLine($"Dispose : {this.GetType().Name}");

            IsDisposed = true;

            CloseCallback = null;
            CloseCallbackParam = null;
            CloseAsync = null;
            OnBackButtonClicked = null;

            OnUpdateCallback = null;

            //App.PageOrientationChanged -= PageOrientationChanged;

            GC.Collect();
        }

        private void PageOrientationChanged(object sender, EventArgs e)
        {
            Console.WriteLine("PageOrientationChanged");
            OnOrientationChanged();
        }

        protected override void InitializeServices()
        {
            _navigationService = ServiceProvider.NavigationService;
        }
    }
}

