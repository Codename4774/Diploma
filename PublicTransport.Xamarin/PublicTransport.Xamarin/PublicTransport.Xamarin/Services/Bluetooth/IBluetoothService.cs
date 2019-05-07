using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Services.Bluetooth
{
    public interface IBluetoothService
    {
        bool SendDataToWearableDevice(string data, out string error);
    }
}
