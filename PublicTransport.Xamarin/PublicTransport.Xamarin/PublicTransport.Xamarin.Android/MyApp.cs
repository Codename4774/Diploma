using System;
using Android.App;
using Android.Runtime;

namespace PublicTransport.Xamarin.Droid
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY",
              Value = "KEY")]
    public class MyApp : Application
    {
        public MyApp(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}

