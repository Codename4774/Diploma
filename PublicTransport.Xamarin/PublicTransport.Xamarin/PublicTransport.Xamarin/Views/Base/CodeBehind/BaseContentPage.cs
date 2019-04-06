using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.Views.Base.Interfaces;
using Xamarin.Forms;
using static Xamarin.Forms.Device;

namespace PublicTransport.Xamarin.Views.Base.CodeBehind
{
    public abstract class BaseContentPage : ContentPage, IBaseView, IDisposable
    {
        public virtual BaseViewModel ViewModel => BindingContext as BaseViewModel;

        public object PausedBindingContext { get; set; }

        #region Properties

        #region PageContent

        private Grid _pageContent = null;

        public View PageContent
        {
            get => _pageContent;
            set => SetPageContent(value);
        }

        #endregion PageContent

        #region CommentedProperties
        //#region AppBarItems

        //public static readonly BindableProperty
        //    AppBarItemsProperty = BindableProperty.Create(nameof(AppBarItems), typeof(IList), typeof(BaseContentPage), null);

        //public IList AppBarItems
        //{
        //    get => (IList) GetValue(AppBarItemsProperty);
        //    set => SetValue(AppBarItemsProperty, value);
        //}

        //#endregion AppBarItems

        //#region BackPageTitle

        //public static readonly BindableProperty BackPageTitleProperty =
        //    BindableProperty.Create(nameof(BackPageTitle), typeof(string), typeof(BaseContentPage), String.Empty);

        //public string BackPageTitle
        //{
        //    get { return (string) GetValue(BackPageTitleProperty); }
        //    set { SetValue(BackPageTitleProperty, value); }
        //}

        //#endregion BackPageTitle

        //#region ShowBackButton

        //public static readonly BindableProperty ShowBackButtonProperty =
        //    BindableProperty.Create(nameof(ShowBackButton), typeof(bool), typeof(BaseContentPage), false);

        //public bool ShowBackButton
        //{
        //    get { return (bool) GetValue(ShowBackButtonProperty); }
        //    set { SetValue(ShowBackButtonProperty, value); }
        //}

        //#endregion ShowBackButton

        //#region ShowBackCross

        //public static readonly BindableProperty ShowBackCrossProperty = BindableProperty.Create(nameof(ShowBackCross), typeof(bool), typeof(BaseContentPage),
        //    false,
        //    propertyChanged: (bindable, oldValue, newValue) =>
        //    {
        //        if ((bool) newValue)
        //            ((BaseContentPage) bindable).BarColor = Color.Transparent;
        //    });

        //public bool ShowBackCross
        //{
        //    get => (bool) GetValue(ShowBackCrossProperty);
        //    set => SetValue(ShowBackCrossProperty, value);
        //}

        //#endregion ShowBackCross

        //#region ShowMenuButton

        //public static readonly BindableProperty ShowMenuButtonProperty =
        //    BindableProperty.Create(nameof(ShowMenuButton), typeof(bool), typeof(BaseContentPage), false);

        //public bool ShowMenuButton
        //{
        //    get => (bool) GetValue(ShowMenuButtonProperty);
        //    set => SetValue(ShowMenuButtonProperty, value);
        //}

        //#endregion ShowMenuButton

        //#region ShowTitle

        //public static readonly BindableProperty ShowTitleProperty = BindableProperty.Create(nameof(ShowTitle), typeof(bool), typeof(BaseContentPage), false);

        //public bool ShowTitle
        //{
        //    get { return (bool) GetValue(ShowTitleProperty); }
        //    set { SetValue(ShowTitleProperty, value); }
        //}

        //#endregion ShowTitle

        //#region IsBarVisible

        //public static readonly BindableProperty IsBarVisibleProperty =
        //    BindableProperty.Create(nameof(IsBarVisible), typeof(bool), typeof(BaseContentPage), true);

        //public bool IsBarVisible
        //{
        //    get { return (bool) GetValue(IsBarVisibleProperty); }
        //    set { SetValue(IsBarVisibleProperty, value); }
        //}

        //#endregion IsBarVisible

        //#region BarTitle

        //public static readonly BindableProperty BarTitleProperty =
        //    BindableProperty.Create(nameof(BarTitle), typeof(string), typeof(BaseContentPage), string.Empty);

        //public string BarTitle
        //{
        //    get => (string) GetValue(BarTitleProperty);
        //    set => SetValue(BarTitleProperty, value);
        //}

        //#endregion BarTitle

        //#region TitleTapCommand

        //public static BindableProperty TitleTapCommandProperty = BindableProperty.Create(nameof(TitleTapCommand), typeof(ICommand), typeof(PopupBase), null);

        //public ICommand TitleTapCommand
        //{
        //    get => (ICommand) this.GetValue(TitleTapCommandProperty);
        //    set => SetValue(TitleTapCommandProperty, value);
        //}

        //#endregion TitleTapCommand

        //#region BarColor

        //public static readonly BindableProperty BarColorProperty = BindableProperty.Create(nameof(BarColor), typeof(Color), typeof(BaseContentPage),
        //    Application.Current.Resources[ColorStyle.BlueTextColor]);

        //public Color BarColor
        //{
        //    get => (Color) GetValue(BarColorProperty);
        //    set => SetValue(BarColorProperty, value);
        //}

        //#endregion BarColor

        //#region DisablePopOnBackButton

        //public static readonly BindableProperty DisablePopOnBackButtonProperty =
        //    BindableProperty.Create(nameof(DisablePopOnBackButton), typeof(bool), typeof(BaseContentPage), false);

        //public bool DisablePopOnBackButton
        //{
        //    get => (bool) GetValue(DisablePopOnBackButtonProperty);
        //    set => SetValue(DisablePopOnBackButtonProperty, value);
        //}

        //#endregion DisablePopOnBackButton

        //#region AnalyticsId

        //public static readonly BindableProperty AnalyticsIdProperty =
        //    BindableProperty.Create(nameof(AnalyticsId), typeof(string), typeof(BaseContentPage), string.Empty);

        //public string AnalyticsId
        //{
        //    get => (string) GetValue(AnalyticsIdProperty);
        //    set => SetValue(AnalyticsIdProperty, value);
        //}

        //#endregion AnalyticsId

        #endregion CommentedProterties

        #endregion Properties

        #region Actions

        #region CommentedActions

        //public Action OnBackButtonClicked { get; set; }

        //public Action OnMenuButtonClicked { get; set; }

        //public Func<bool> OnBackButtonTapped;

        #endregion CommentedActions

        #endregion Actions

        public BaseContentPage()
        {
            //DebugService.ObjectMonitor.AddObject(this.GetType());

            //NavigationPage.SetHasNavigationBar(this, false);

            //_pageContent = new Grid()
            //{
            //    RowDefinitions =
            //    {
            //        new RowDefinition {Height = GridLength.Auto},
            //        new RowDefinition {Height = GridLength.Star},
            //    },
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    RowSpacing = 0,
            //    BackgroundColor = Color.White
            //};

            //CreateToolbar();

            //Appearing += AnalyticsOnAppearing;
            base.Content = _pageContent;
        }

        ~BaseContentPage()
        {
            //DebugService.ObjectMonitor.RemoveObject(this.GetType());
        }

        //private void AnalyticsOnAppearing(object o, EventArgs eventArgs)
        //{
        //    string screenId = !string.IsNullOrWhiteSpace(AnalyticsId) ? AnalyticsId : this.GetType().Name;
        //    DependencyService.Get<IAnalyticsProvider>().TrackScreenActivation(screenId);
        //}

        private void SetPageContent(View view)
        {
            Grid.SetRow(view, 1);
            _pageContent.Children.Add(view);
        }

        public void InitializeComponent()
        {
            
        }

        //private void CreateToolbar()
        //{
        //    BackgroundColor = Color.White;

        //    CachedImage backButton = new CachedImage {BackgroundColor = Color.Transparent, Source = "app_bar_back.png", VerticalOptions = LayoutOptions.Center,};
        //    backButton.SetBinding(IsVisibleProperty, "ShowBackButton");
        //    backButton.AutomationId = AutomationIds.NavigationBackBtnId;

        //    CachedImage menuButton = new CachedImage {BackgroundColor = Color.Transparent, Source = "side_menu.png"};
        //    menuButton.SetBinding(IsVisibleProperty, "ShowMenuButton");
        //    menuButton.AutomationId = AutomationIds.NavigationOpenDrawerActionId;

        //    Label backPagetitleLabel = new Label()
        //    {
        //        FontAttributes = FontAttributes.None,
        //        LineBreakMode = LineBreakMode.TailTruncation,
        //        Margin = 0,
        //        TextColor = Color.White,
        //        HorizontalTextAlignment = TextAlignment.Start,
        //        VerticalTextAlignment = TextAlignment.End,
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        VerticalOptions = LayoutOptions.Center
        //    };
        //    backPagetitleLabel.FontSize = GetNamedSize(NamedSize.Medium, backPagetitleLabel);
        //    backPagetitleLabel.SetBinding(Label.TextProperty, "BackPageTitle");
        //    backPagetitleLabel.SetBinding(IsVisibleProperty, "ShowBackButton");
        //    backPagetitleLabel.AutomationId = AutomationIds.NavigationBackBtnId;

        //    // Title label

        //    Label titleLabel = new Label()
        //    {
        //        Style = (Style) Application.Current.Resources[LabelStyle.LightBigSemiboldLabelStyle],
        //        LineBreakMode = LineBreakMode.TailTruncation,
        //        HorizontalTextAlignment = TextAlignment.Center,
        //        VerticalTextAlignment = TextAlignment.End,
        //        TextColor = Color.White,
        //        HorizontalOptions = LayoutOptions.CenterAndExpand,
        //        VerticalOptions = LayoutOptions.Center
        //    };

        //    titleLabel.SetBinding(Label.TextProperty, "BarTitle");
        //    titleLabel.SetBinding(IsVisibleProperty, "ShowTitle");

        //    var crossStackLayout = new StackLayout()
        //    {
        //        HeightRequest = 30,
        //        WidthRequest = 30,
        //        Padding = new Thickness(0, 10, OnPlatform(15, 8, 0), 0),
        //        BackgroundColor = Color.Transparent,
        //        VerticalOptions = LayoutOptions.Center,
        //    };

        //    TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        //    tapGesture.Tapped += CrossButton_Clicked;
        //    crossStackLayout.GestureRecognizers.Add(tapGesture);
        //    CachedImage crossImage = new CachedImage
        //    {
        //        BackgroundColor = Color.Transparent,
        //        Source = "close_icon_gray.png",
        //        VerticalOptions = LayoutOptions.Center,
        //    };
        //    //crossButton.Clicked += CrossButton_Clicked;
        //    crossStackLayout.SetBinding(IsVisibleProperty, "ShowBackCross");
        //    crossStackLayout.Children.Add(crossImage);
        //    crossStackLayout.AutomationId = AutomationIds.NavigationBackBtnId;
        //    //padding top

        //    AbsoluteLayout mainBarLayout = new AbsoluteLayout
        //    {
        //        Padding = OnPlatform(new Thickness(0, 0, 0, 0), new Thickness(0, 7, 0, 0), new Thickness()),
        //        HeightRequest = 45
        //    };
        //    mainBarLayout.Effects.Add(new SafeAreaPaddingEffect());
        //    //mainBarLayout.PropertyChanged += MainBarLayout_PropertyChanged;
        //    mainBarLayout.SetBinding(BackgroundColorProperty, "BarColor");
        //    mainBarLayout.SetBinding(IsVisibleProperty, "IsBarVisible");

        //    #region LeftStack

        //    StackLayout leftStack = new StackLayout()
        //    {
        //        BackgroundColor = Color.Transparent,
        //        Orientation = StackOrientation.Horizontal,
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        Padding = new Thickness(12, 0, 10, 0)
        //    };

        //    leftStack.Children.Add(backButton);
        //    leftStack.Children.Add(menuButton);
        //    leftStack.Children.Add(backPagetitleLabel);

        //    AbsoluteLayout.SetLayoutBounds(leftStack, new Rectangle(1, 0, 1, 1));
        //    AbsoluteLayout.SetLayoutFlags(leftStack, AbsoluteLayoutFlags.All);
        //    mainBarLayout.Children.Add(leftStack);

        //    #endregion LeftStack

        //    #region CenterStack

        //    StackLayout centerStack = new StackLayout()
        //    {
        //        BackgroundColor = Color.Transparent,
        //        Orientation = StackOrientation.Horizontal
        //    };
        //    centerStack.Children.Add(titleLabel);

        //    TapGestureRecognizer centerStackGesture = new TapGestureRecognizer();
        //    centerStackGesture.Tapped += (sender, args) => { TitleTapCommand?.Execute(this); };
        //    centerStack.GestureRecognizers.Add(centerStackGesture);

        //    AbsoluteLayout.SetLayoutBounds(centerStack, new Rectangle(.5, 0, 0.5, 1));
        //    AbsoluteLayout.SetLayoutFlags(centerStack, AbsoluteLayoutFlags.All);
        //    mainBarLayout.Children.Add(centerStack);

        //    #endregion CenterStack

        //    #region RightStack

        //    StackLayout rightStack = new StackLayout()
        //    {
        //        Spacing = 0,
        //        BackgroundColor = Color.Transparent,
        //        Orientation = StackOrientation.Horizontal,
        //        Padding = new Thickness(10, 0, 0, 0)
        //    };

        //    HorizontalStack horizontalIcons = new HorizontalStack()
        //    {
        //        HorizontalOptions = LayoutOptions.EndAndExpand,
        //        BackgroundColor = Color.Transparent,
        //        Padding = new Thickness(0, 0, 10, 0)
        //    };
        //    DataTemplate dataTemplate = new DataTemplate(typeof(ToolbarItemCell));

        //    horizontalIcons.ItemTemplate = dataTemplate;
        //    horizontalIcons.SetBinding(HorizontalStack.ItemsSourceProperty, "AppBarItems");

        //    rightStack.Children.Add(horizontalIcons);
        //    rightStack.Children.Add(crossStackLayout);

        //    AbsoluteLayout.SetLayoutBounds(rightStack, new Rectangle(1, 0, .5, 1));
        //    AbsoluteLayout.SetLayoutFlags(rightStack, AbsoluteLayoutFlags.All);
        //    mainBarLayout.Children.Add(rightStack);

        //    #endregion RightStack

        //    TapGestureRecognizer leftStackTapGesture = new TapGestureRecognizer();
        //    leftStackTapGesture.Tapped += async (sender, e) =>
        //    {
        //        try
        //        {
        //            if (ShowBackButton)
        //            {
        //                await AnimationUtility.ShowTappedAnimation((View) sender);
        //                await BackPress();
        //            }
        //            else if (ShowMenuButton)
        //            {
        //                await AnimationUtility.ShowTappedAnimation((View) sender);
        //                OnMenuButtonClicked?.Invoke();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.StackTrace);
        //        }
        //    };
        //    leftStack.GestureRecognizers.Add(leftStackTapGesture);

        //    mainBarLayout.BindingContext = this;

        //    Grid.SetRow(mainBarLayout, 0);
        //    _pageContent.Children.Add(mainBarLayout);
        //}


        //private async Task BackPress()
        //{
        //    try
        //    {
        //        if (App.UserModule.Session.IsForegroundTimeoutExpired())
        //            return;

        //        if (!DisablePopOnBackButton)
        //        {
        //            if (Navigation.ModalStack.LastOrDefault() == this)
        //            {
        //                await Navigation.PopModalAsync();
        //            }
        //            else if (Navigation.NavigationStack.LastOrDefault() == this)
        //            {
        //                await Navigation.PopAsync();
        //            }
        //            else
        //            {
        //                await Navigation.PopAsync();
        //            }
        //        }

        //        ViewModel.OnBackButtonClicked?.Invoke();
        //        OnBackButtonClicked?.Invoke();

        //        if (!DisablePopOnBackButton)
        //            Dispose();
        //    }
        //    catch
        //    {
        //    }
        //}

        //private async void CrossButton_Clicked(object sender, EventArgs e)
        //{
        //    if (App.UserModule.Session.IsForegroundTimeoutExpired())
        //        return;

        //    var closeAsync = ViewModel?.CloseAsync;
        //    if (closeAsync != null)
        //        await closeAsync.Invoke();

        //    await BackPress();
        //}

        //public virtual void OnPagePaused()
        //{
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        PausedBindingContext = BindingContext;
        //        BindingContext = null;
        //    }
        //}

        public virtual void Dispose()
        {
            Console.WriteLine($"Dispose : {this.GetType().Name}");

            //Parent = null;

            //OnBackButtonClicked = null;
            //OnMenuButtonClicked = null;
            //OnBackButtonTapped = null;

            ViewModel?.Dispose();
            BindingContext = null;
            PausedBindingContext = null;

            if (_pageContent?.Children != null)
                foreach (var item in _pageContent.Children)
                    item.BindingContext = null;


            GC.Collect();
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine($"OnAppearing : {this.GetType().Name}");
            if (PausedBindingContext != null)
            {
                BindingContext = PausedBindingContext;
                PausedBindingContext = null;
            }
        }

        public virtual /*override*/ void OnLoaded()
        {
            Console.WriteLine($"OnLoaded : {this.GetType().Name}");
            //base.OnLoaded();

            Task.Run(async () =>
            {
                await Task.Delay(500);
                Device.BeginInvokeOnMainThread(() =>
                {
                    ServiceProvider.NavigationService.ResetLocker();
                    ViewModel?.OnLoaded();
                });
            });
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    if (OnBackButtonTapped != null)
        //    {
        //        bool result = OnBackButtonTapped?.Invoke() ?? false;
        //        if (!result)
        //            return true;

        //        Dispose();
        //    }

        //    Dispose();
        //    return base.OnBackButtonPressed();
        //}
    }
}