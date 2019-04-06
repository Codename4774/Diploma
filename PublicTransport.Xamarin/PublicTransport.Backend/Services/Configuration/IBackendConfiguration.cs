using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.Configuration
{
    public interface IBackendConfiguration
    {
        string GetProperty(string key);
    }
}
