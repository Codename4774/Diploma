using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PublicTransport.Xamarin
{
    internal static class PropertiesHelper
    {
        internal static bool IsContains(string key) => Application.Current.Properties.ContainsKey(key);

        internal static void ChangeValue(string forKey, object valueToChange) => Application.Current.Properties[forKey] = valueToChange;

        internal static T Value<T>(string forKey) => (T) Application.Current.Properties[forKey];

        internal static async Task SaveProperties() => await Application.Current.SavePropertiesAsync();
    }
}