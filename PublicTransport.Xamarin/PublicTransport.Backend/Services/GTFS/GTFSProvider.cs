using GTFS;
using GTFS.Entities;
using GTFS.Entities.Collections;
using GTFS.IO;
using GTFS.IO.CSV;
using Newtonsoft.Json;
using PublicTransport.Backend.Services.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PublicTransport.Backend.Services.GTFS
{
    public class GTFSProvider : IGTFSProvider
    {
        private string _dirPath;

        private GTFSReader<GTFSFeed> _GTFSReader;

        private PublicTransport.Backend.Services.GTFS.GTFSDirectorySource _GTFSDirectorySource;

        private GTFSFeed _GTFSFeed;

        public GTFSProvider(IBackendConfiguration configurationManager, object GTFSFeedFromProerties = null)
        {
            _isInited = false;
            _dirPath = configurationManager.GetProperty("GTFSFolderPath");
            _GTFSDirectorySource = new PublicTransport.Backend.Services.GTFS.GTFSDirectorySource(_dirPath);
            _GTFSReader = new GTFSReader<GTFSFeed>();

            Task initTask = new Task(() =>
            {
                _GTFSFeed = _GTFSReader.Read(_GTFSDirectorySource);
            });
            initTask.GetAwaiter().OnCompleted(() =>
            {
                _isInited = true;
                if (InitCompleted != null)
                {
                    InitCompleted(this, new EventArgs());
                }
            });

            initTask.Start();
        }

        #region IGTFSProvider Implementation
        
        public GTFSFeed GTFSFeed
        {
            get
            {
                return _GTFSFeed;
            }
        }

        private bool _isInited;

        public bool IsInited
        {
            get
            {
                return _isInited;
            }
        }

        public event EventHandler InitCompleted;

        #endregion
    }
}
