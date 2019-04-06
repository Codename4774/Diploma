using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PublicTransport.Xamarin.Views;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PublicTransport.Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            ServiceProvider.Initialize();

            ServiceProvider.NavigationService.OpenMasterDetailPage<MainViewModel, MainMenuMasterViewModel>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #region GlobalAppData

        public static new App Current
        {
            get
            {
                return (App)Application.Current;
            }
        }

        #endregion

        #region StaticResourceManager

        public static object GetResourceValue(string key, Application appInstanse)
        {
            object result = new object();

            appInstanse.Resources.TryGetValue(key, out result);

            return result;
        }

        public static string GetResourceValueString(string key, Application appInstanse)
        {
            return GetResourceValue(key, appInstanse).ToString();
        }

        public static double GetResourceValueInt(string key, Application appInstanse)
        {
            return Convert.ToInt32(GetResourceValue(key, appInstanse));
        }

        public static double GetResourceValueDouble(string key, Application appInstanse)
        {
            return Convert.ToDouble(GetResourceValue(key, appInstanse));
        }

        #endregion

        #region Styles

        #region Colors Hex

        private const string MainAppColor = "#122048";
        private const string MainAppColorKey = "AppColor";


        private const string TextColorOnMainAppColor = "#FFFFFF";
        private const string TextColorOnMainAppColorKey = "TextColorOnMainAppColor";

        #endregion

        private void InitializeStyles()
        {
            //colors
            AddColorFromHex(MainAppColor, MainAppColorKey);
            AddColorFromHex(TextColorOnMainAppColor, TextColorOnMainAppColorKey);
            //labels
            var baseLabelStyle = BaseLabelStyle();
            var titleLabel = TitleLabelStyle(baseLabelStyle, "TitleLabelStyle");
            var remarkLabelStyle = RemarkLabelStyle(baseLabelStyle, "RemarkLabelStyle");
            var menuLabelStyle = MenuNameLabelStyle(baseLabelStyle, "MenuNameLabelStyle");

            //buttons
            var baseButtonStyle = BaseButtonStyle();
            var loginButtonStyle = LoginButtonStyle(baseButtonStyle, "LoginButtonStyle");
            var approveButtonStyle = ApproveButtonStyle(baseButtonStyle, "ApproveButtonStyle");

            //entries
            var baseEntryStyle = BaseEntryStyle();
        }

        private void AddColorFromHex(string hex, string key)
        {
            var color = Color.FromHex(hex);
            Resources.Add(key, color);
        }

        private static Style GetStyleObjectBasedOnParent(Style parentStyle)
        {
            return new Style(parentStyle.TargetType)
            {
                BasedOn = parentStyle
            };
        }

        private static Setter GetNewSetter(BindableProperty property, object value)
        {
            return new Setter()
            {
                Property = property,
                Value = value
            };
        }

        private Style SetStyleProperties(Style parentStyle, string styleName, Action<Style> setterCallback)
        {
            var style = GetStyleObjectBasedOnParent(parentStyle);
            setterCallback(style);
            Resources.Add(styleName, style);
            return style;
        }

        #region Labels

        private Style BaseLabelStyle()
        {
            var style = new Style(typeof(Label))
            {
                Setters =
                {
                    GetNewSetter(Label.FontSizeProperty, 14),
                    GetNewSetter(Label.TextColorProperty, Color.Black)
                }
            };

            Resources.Add("BaseLabelStyle", style);

            return style;
        }

        private Style MenuNameLabelStyle(Style parentStyle, string styleName)
        {
            return SetStyleProperties(parentStyle, styleName, style =>
            {
                style.Setters.Add(GetNewSetter(Label.FontAttributesProperty, FontAttributes.Bold));
                style.Setters.Add(GetNewSetter(Label.HorizontalTextAlignmentProperty, TextAlignment.Start));
                style.Setters.Add(GetNewSetter(Label.LineBreakModeProperty, LineBreakMode.TailTruncation));
                style.Setters.Add(GetNewSetter(View.MarginProperty, new Thickness(10, 10, 0, 20)));
                style.Setters.Add(GetNewSetter(Label.TextColorProperty, Resources[TextColorOnMainAppColorKey]));
            });
        }

        private Style TitleLabelStyle(Style parentStyle, string styleName)
        {
            return SetStyleProperties(parentStyle, styleName, (style) =>
            {
                style.Setters.Add(GetNewSetter(Label.FontSizeProperty, 20));
                style.Setters.Add(GetNewSetter(Label.FontAttributesProperty, FontAttributes.Bold));
            });
        }

        private Style RemarkLabelStyle(Style parentStyle, string styleName)
        {
            return SetStyleProperties(parentStyle, styleName, (style) =>
            {
                style.Setters.Add(GetNewSetter(Label.FontSizeProperty, 12));
                style.Setters.Add(GetNewSetter(Label.FontAttributesProperty, FontAttributes.Italic));
                style.Setters.Add(GetNewSetter(Label.FontAttributesProperty, FontAttributes.Italic));
            });
        }

        #endregion

        #region Buttons

        private Style BaseButtonStyle()
        {
            var style = new Style(typeof(Button))
            {
                Setters =
                {
                    GetNewSetter(Button.BackgroundColorProperty, Color.LightSkyBlue),
                    GetNewSetter(Button.TextColorProperty, Color.Black),
                    GetNewSetter(Button.FontSizeProperty, 20),
                }
            };
            Resources.Add("BaseButtonStyle", style);
            return style;
        }

        private Style LoginButtonStyle(Style parentStyle, string styleName)
        {
            return SetStyleProperties(parentStyle, styleName, (style) =>
            {
                style.Setters.Add(GetNewSetter(Button.BackgroundColorProperty, Color.Blue)); style.Setters.Add(GetNewSetter(Button.TextColorProperty, Color.White));
            });
        }


        private Style ApproveButtonStyle(Style parentStyle, string styleName) => SetStyleProperties(parentStyle, styleName, (style) =>
        {
            style.Setters.Add(GetNewSetter(Button.BackgroundColorProperty, Color.Green)); style.Setters.Add(GetNewSetter(Button.TextColorProperty, Color.White));
        });



        #endregion

        #region Entries

        private Style BaseEntryStyle()
        {
            var style = new Style(typeof(Entry))
            {
                Setters =
                {
                    GetNewSetter(Entry.TextColorProperty, Color.Black),
                    GetNewSetter(Entry.PlaceholderColorProperty, Color.Gray)
                }
            };
            Resources.Add("BaseEntryStyle", style);
            return style;
        }

        #endregion

        #endregion
    }
}
