using PublicTransport.Xamarin.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Services.ImageResourceManager
{
    public class ImageResourceManager : IImageResourceManager
    {
        private Assembly _assembly;

        private Dictionary<string, Stream> _cache;

        private Dictionary<string, ImageSource> _cacheImageSource;

        public ImageResourceManager(Assembly assembly)
        {
            _assembly = assembly;
            _cache = new Dictionary<string, Stream>();
            _cacheImageSource = new Dictionary<string, ImageSource>();
            InitCache();
        }

        private void InitCache()
        {
            AddItemToCache(Constants.STOP_ICON_FILE_PATH);
            //AddItemToCache(Constants.ROUTE_ICON_FILE_PATH);
        }

        private void AddItemToCache(string filePath)
        {
            _cache.Add(filePath, GetResourceStream(filePath));
            _cacheImageSource.Add(filePath, ImageSource.FromStream(() => _cache[filePath]));
        }

        private Stream GetResourceStream(string fileName)
        {
            return _assembly.GetManifestResourceStream($"PublicTransport.Xamarin.Images.{fileName}") ?? 
                _assembly.GetManifestResourceStream($"PublicTransport.Xamarin.local.{fileName}");
        }

        private ImageSource GetResourceImageSourceStream(string fileName)
        {
            return ImageSource.FromStream(() => GetResourceStream(fileName));
        }

        public Stream GetImageFromCache(string imagePath)
        {
            return _cache[imagePath];
        }

        public Stream GetImageStream(string imagePath)
        {
            return GetResourceStream(imagePath);
        }

        public ImageSource GetImageSourceFromCache(string imagePath)
        {
            return GetResourceImageSourceStream(imagePath);
        }

        public ImageSource GetImageSourceStream(string imagePath)
        {
            return _cacheImageSource[imagePath];
        }
    }
}
