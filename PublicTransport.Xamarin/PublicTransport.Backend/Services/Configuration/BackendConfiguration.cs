using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace PublicTransport.Backend.Services.Configuration
{
    public class BackendConfiguration : IBackendConfiguration
    {
        private static string fileName = "PublicTransport.Backend.BackendConfig.xml";


        private IDictionary<string, string> _configDictionary;


        public BackendConfiguration()
        {
            _configDictionary = new Dictionary<string, string>();

            ReadPropertiesFromXmlFile(fileName);
        }

        public string GetProperty(string key)
        {
            return _configDictionary[key];
        }

        public void ReadPropertiesFromXmlFile(string fileName)
        {
            XmlDocument configDocument = new XmlDocument();

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream(fileName);

                configDocument.Load(stream);

                XmlNode rootNode = configDocument.DocumentElement;

                ReadNodeData(rootNode);

            }
            catch (Exception e)
            {
                //some actions
                throw e;
            }
        }

        private void ReadNodeData(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    ReadNodeData(childNode);
                }
            }

            if (node.Attributes["value"] != null)
            {
                _configDictionary.Add(node.Name, node.Attributes["value"].Value);
            }
        }
    }
}
