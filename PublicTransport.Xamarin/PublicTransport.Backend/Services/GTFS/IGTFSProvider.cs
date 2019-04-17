using GTFS;
using GTFS.Entities;
using GTFS.Entities.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.GTFS
{
    public interface IGTFSProvider
    {
        GTFSFeed GTFSFeed { get; }
        bool IsInited { get; }
        event EventHandler InitCompleted;
    }
}
