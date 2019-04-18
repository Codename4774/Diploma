using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Services.ImageResourceManager
{
    public interface IImageResourceManager
    {
        ImageSource GetImageSourceFromCache(string imagePath);
        ImageSource GetImageSourceStream(string imagePath);
        Stream GetImageStream(string imagePath);
        Stream GetImageFromCache(string imagePath);
    }
}
