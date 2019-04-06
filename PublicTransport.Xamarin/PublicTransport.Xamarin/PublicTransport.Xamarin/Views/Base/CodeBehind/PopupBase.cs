using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.Views.Base.Interfaces;
using PublicTransport.Xamarin;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Views.Base.CodeBehind
{
	public abstract class PopupBase : PopupPage, IBaseView, IDisposable
	{
		public BaseViewModel ViewModel => BindingContext as BaseViewModel;

        #region Properties

        #region ShowSaveButton
        public static readonly BindableProperty ShowSaveButtonProperty = BindableProperty.Create(nameof(ShowSaveButton), typeof(bool), typeof(PopupBase), false);

	    public bool ShowSaveButton
	    {
	        get => (bool)GetValue(ShowSaveButtonProperty);
	        set => SetValue(ShowSaveButtonProperty, value);
	    }
	    #endregion ShowSaveButton

        //   public static readonly BindableProperty SaveButtonTextProperty = BindableProperty.Create("SaveButtonText", typeof(string), typeof(PopupBase), AppStrings.Save);

	    //public string SaveButtonText
	    //{
	    //    get { return (string)GetValue(SaveButtonTextProperty); }
	    //    set { SetValue(SaveButtonTextProperty, value); }
	    //}

     //   public static readonly BindableProperty SaveButtonTextColorProperty = BindableProperty.Create("SaveButtonTextColor", typeof(Color), typeof(PopupBase), Color.Default);

	    //public Color SaveButtonTextColor
	    //{
	    //    get { return (Color)GetValue(SaveButtonTextColorProperty); }
	    //    set { SetValue(SaveButtonTextColorProperty, value); }
	    //}

     //   public static readonly BindableProperty ShowCancelButtonProperty = BindableProperty.Create("ShowCancelButton", typeof(bool), typeof(PopupBase), true);

	    //public bool ShowCancelButton
	    //{
	    //    get { return (bool)GetValue(ShowCancelButtonProperty); }
	    //    set { SetValue(ShowCancelButtonProperty, value); }
	    //}

     //   public static BindableProperty CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(PopupBase), AppStrings.Cancel);

	    //public string CancelButtonText
	    //{
	    //    get { return (string)GetValue(CancelButtonTextProperty); }
	    //    set { SetValue(CancelButtonTextProperty, value); }
	    //}

	    //public static readonly BindableProperty CancelButtonTextColorProperty = BindableProperty.Create("CancelButtonTextColor", typeof(Color), typeof(PopupBase), Color.Default);

	    //public Color CancelButtonTextColor
	    //{
	    //    get { return (Color)GetValue(CancelButtonTextColorProperty); }
	    //    set { SetValue(CancelButtonTextColorProperty, value); }
	    //}

     //   public static readonly BindableProperty ShowHeaderProperty = BindableProperty.Create("ShowHeader", typeof(bool), typeof(PopupBase), true);

	    //public bool ShowHeader
	    //{
	    //    get { return (bool)GetValue(ShowHeaderProperty); }
	    //    set { SetValue(ShowHeaderProperty, value); }
	    //}

     //   #region SaveCommand
     //   public static BindableProperty SaveCommandProperty = BindableProperty.Create("SaveCommand", typeof(ICommand), typeof(PopupBase), null);

	    //public ICommand SaveCommand
	    //{
	    //    get => (ICommand)this.GetValue(SaveCommandProperty);
	    //    set { SetValue(SaveCommandProperty, value); }
	    //}
	    //#endregion SaveCommand

	    //#region CancelCommand
	    //public static BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(PopupBase), null);

	    //public ICommand CancelCommand
	    //{
	    //    get => (ICommand)this.GetValue(CancelCommandProperty);
	    //    set { SetValue(CancelCommandProperty, value); }
	    //}
     //   #endregion CancelCommand

	    //#region AnalyticsId
	    //public static readonly BindableProperty AnalyticsIdProperty = BindableProperty.Create(nameof(AnalyticsId), typeof(string), typeof(PopupBase), string.Empty);
	    //public string AnalyticsId
	    //{
	    //    get => (string)GetValue(AnalyticsIdProperty);
	    //    set => SetValue(AnalyticsIdProperty, value);
	    //}
     //   #endregion AnalyticsId

     //   #region HeightPopup
     //   public static readonly BindableProperty HeightPopupProperty = BindableProperty.Create(nameof(HeightPopup), typeof(double), typeof(PopupBase), (double)App.ScreenSize.Height);
	    //public double HeightPopup
	    //{
	    //    get => (double)GetValue(HeightPopupProperty);
	    //    set => SetValue(HeightPopupProperty, value);
	    //}
     //   #endregion HeightPopup

	    //#region MarginPopup
	    //public static readonly BindableProperty MarginPopupProperty = BindableProperty.Create(nameof(MarginPopup), typeof(Thickness), typeof(PopupBase), new Thickness(0, 0, 0, 0));
	    //public Thickness MarginPopup
     //   {
	    //    get => (Thickness)GetValue(MarginPopupProperty);
	    //    set => SetValue(MarginPopupProperty, value);
	    //}
     //   #endregion MarginPopup

        #endregion Properties

        public PopupBase(bool animate = false)
		{
		    //DebugService.ObjectMonitor.AddObject(this.GetType());

		    this.ControlTemplate = new ControlTemplate(typeof(PopupTemplate));

			//if (animate && Device.Idiom != TargetIdiom.Tablet)
			//{
			//	this.Animation = AnimationUtility.GetPopupBaseAnimation();
			//}

		    //Appearing += AnalyticsOnAppearing;
		}

	    ~PopupBase()
	    {
	        //DebugService.ObjectMonitor.RemoveObject(this.GetType());
	    }

	    //private void AnalyticsOnAppearing(object o, EventArgs eventArgs)
	    //{
	    //    string screenId = !string.IsNullOrWhiteSpace(AnalyticsId) ? AnalyticsId : this.GetType().Name;
	    //    DependencyService.Get<IAnalyticsProvider>().TrackScreenActivation(screenId);
	    //}

        public virtual void Dispose()
	    {
	        Console.WriteLine($"Dispose : {this.GetType().Name}");

	        ViewModel?.Dispose();
	        BindingContext = null;

	        //OnSaveClicked = null;
	        //OnCancelClicked = null;
        }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
	        ServiceProvider.NavigationService.ResetLocker();
	    }

		//public Action OnSaveClicked
		//{
		//	get; set;
		//}

  //      public Func<Task<bool>> OnCancelClicked
		//{
		//	get; set;
		//}

	    public abstract Task Initialize();

	    public static void Update()
	    {
	        //CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(PopupBase), AppStrings.Cancel);
        }

	    //protected override bool OnBackButtonPressed()
	    //{
     //       Dispose();
	    //    return base.OnBackButtonPressed();
	    //}
	}

    /// <summary>
    /// Popup template.
    /// </summary>
    public class PopupTemplate : ContentView
	{
		private StackLayout _contentStack;

		public PopupTemplate()
		{
			SetPopupLayout();
			SetMainStack();
			AddContent();
		}

		void SetPopupLayout()
		{
			VerticalOptions = LayoutOptions.Start;
			HorizontalOptions = LayoutOptions.Start;
		}

		void SetMainStack()
		{
		    _contentStack = new StackLayout {
		        Spacing = 0,
			    VerticalOptions = LayoutOptions.Start,
			    HorizontalOptions = LayoutOptions.Start,
			    BackgroundColor = (Color)App.Current.Resources["PopupTitleHeaderBackground"],

                //todo: get display size from resources

			    WidthRequest = App.GetResourceValueDouble("ScreenSizeWidth", App.Current)
			};

		    _contentStack.SetBinding(HeightRequestProperty, new TemplateBinding("HeightPopup"));
		    _contentStack.SetBinding(MarginProperty, new TemplateBinding("MarginPopup"));

   //         if (Device.Idiom == TargetIdiom.Tablet)
			//{
			//	if (DeviceService.IsLandscapeMode)
			//	{
			//		//do nothing
			//	}
			//	else
			//	{
			//		WidthRequest = App.ScreenWidth * 0.6;
			//		HeightRequest = App.ScreenHeight * 0.6;
			//	}
			//}
			WidthRequest = App.GetResourceValueDouble("ScreenSizeWidth", App.Current);
			HeightRequest = App.GetResourceValueDouble("ScreenSizeHeight", App.Current);
		}

		void AddContent()
		{
            #region Commented
            //Button cancelButton = new Button { Text = AppStrings.Cancel, BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.Center };
            //   cancelButton.SetBinding(IsVisibleProperty, new TemplateBinding("ShowCancelButton"));
            //cancelButton.SetBinding(Button.TextProperty, new TemplateBinding("CancelButtonText") { Converter = new PopupBaseCancelButtonTextConverter() });
            //cancelButton.SetBinding(Button.TextColorProperty, new TemplateBinding("CancelButtonTextColor"));
            //cancelButton.AutomationId = AutomationIds.NavigationBackBtnId;

            //Button saveButton = new Button { Text = AppStrings.Save, BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.Center };
            //saveButton.SetBinding(IsVisibleProperty, new TemplateBinding("ShowSaveButton"));
            //saveButton.SetBinding(Button.TextProperty, new TemplateBinding("SaveButtonText") { Converter = new PopupBaseSaveButtonTextConverter() });
            //saveButton.SetBinding(Button.TextColorProperty, new TemplateBinding("SaveButtonTextColor"));
            //saveButton.AutomationId = AutomationIds.NavigationSaveBtnId;

            //Button rightButton = new Button { Text = "       ", BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.Center };
            //rightButton.SetBinding(IsVisibleProperty, new TemplateBinding("ShowSaveButton") { Converter = new InvertBusyConverter() });

            //Label titleLabel = new Label()
            //{
            //	Margin = 10,
            //	HorizontalTextAlignment = TextAlignment.Center,
            //	VerticalTextAlignment = TextAlignment.End,
            //	HorizontalOptions = LayoutOptions.CenterAndExpand,
            //	VerticalOptions = LayoutOptions.Center,
            //	Style = (Style)App.Current.Resources["DarkNormalSemiboldStyle"]
            //};
            //titleLabel.SetBinding(Label.TextProperty, new TemplateBinding("Title"));

            //StackLayout horizontalStack = new StackLayout()
            //{
            //	Orientation = StackOrientation.Horizontal,
            //	Padding = new Thickness(10, 2, 10, 0)
            //};
            //horizontalStack.SetBinding(IsVisibleProperty, new TemplateBinding("ShowHeader"));
            //horizontalStack.Children.Add(cancelButton);
            //horizontalStack.Children.Add(titleLabel);
            //horizontalStack.Children.Add(saveButton);
            //horizontalStack.Children.Add(rightButton);

            //saveButton.Clicked += (sender, e) =>
            //{
            //	try
            //	{
            //		var popup = (PopupBase)Parent;
            //	    if (popup != null)
            //	    {
            //	        popup.OnSaveClicked?.Invoke();
            //                     popup.SaveCommand?.Execute(null);
            //	    }
            //	}
            //	catch (Exception ex)
            //	{
            //		Console.WriteLine(ex.StackTrace);
            //	}
            //};

            //         cancelButton.Clicked += async (sender, e) =>
            //{
            //	try
            //	{
            //	    bool isCanceled = false;

            //		var popup = (PopupBase)Parent;
            //	    if (popup != null)
            //	    {
            //                     if (popup.OnCancelClicked != null)
            //                     {
            //                         isCanceled = await popup.OnCancelClicked?.Invoke();   
            //                     }
            //                     else 
            //                     {
            //                         isCanceled = true;
            //                     }
            //	        popup.CancelCommand?.Execute(null);
            //                 }

            //	    if (isCanceled)
            //	    {
            //	        await PopupNavigation.PopAsync();
            //	        popup.Dispose();
            //                 }
            //	}
            //	catch (Exception ex)
            //	{
            //		Console.WriteLine(ex.StackTrace);
            //	}
            //};

            //var contentPresenter = new ContentPresenter
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            //   _contentStack.Children.Add(horizontalStack);
            //   _contentStack.Children.Add(contentPresenter);
            //Content = _contentStack;
            #endregion Commented
        }
    }
}