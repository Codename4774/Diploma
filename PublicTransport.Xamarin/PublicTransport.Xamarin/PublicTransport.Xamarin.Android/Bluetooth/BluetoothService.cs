using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using PublicTransport.Xamarin.Services.Bluetooth;
using Xamarin.Forms;

[assembly: Dependency(typeof(PublicTransport.Xamarin.Droid.Bluetooth.BluetoothService))]
namespace PublicTransport.Xamarin.Droid.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        private string _connGuid = "483A1980-991C-11E6-BDF4-0800200C9A66";

        public bool SendDataToWearableDevice(string data, out string error)
        {
            error = "";

            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null)
            {
                error = "No Bluetooth adapter found.";

                return false;
            }

            if (!adapter.IsEnabled)
            {
                error = "Bluetooth adapter is not enabled.";

                return false;
            }

            BluetoothDevice device = adapter.BondedDevices
                .Where(deviceToSearch => deviceToSearch.BluetoothClass.DeviceClass == DeviceClass.WearableWristWatch)
                .FirstOrDefault();

            if (device != default(BluetoothDevice))
            {
                try
                {
                    BluetoothSocket socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(_connGuid));

                    socket.Connect();

                    byte[] dataToSend = System.Text.Encoding.UTF8.GetBytes(data + "{END}");
                    socket.OutputStream.Write(dataToSend, 0, dataToSend.Length);
                }
                catch (Exception e)
                {
                    error = "Error. Cannot connect to app on wearable device.";

                    return false;
                }
            }

            return true;
        }
    }
}