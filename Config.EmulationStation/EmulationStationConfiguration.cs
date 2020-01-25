using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Config.EmulationStation
{


    public class EmulationStationConfiguration
    {
        string _path;

        public EmulationStationConfiguration(string path)
        {
            _path = path;
        }

        public List<Platform> GetPlatforms(string path)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(PlatformsList));

            var settings = new XmlReaderSettings()
            {

            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var s = File.ReadAllText(path);
            using (var sww = new StringReader(s))
            {
                var platforms = (PlatformsList)xsSubmit.Deserialize(sww);
                return platforms.Platforms;
            }
        }

        public List<PlatformExtension> GetExtensions(string path)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Extensions));

            var settings = new XmlReaderSettings()
            {

            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var s = File.ReadAllText(path);
            using (var sww = new StringReader(s))
            {
                var extensions = (Extensions)xsSubmit.Deserialize(sww);
                return extensions.SystemExtensions;
            }
        }

        public List<GameSystem> ReadConfig()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(SystemList));
            var xml = "";

            var settings = new XmlReaderSettings()
            {
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            if (File.Exists(ConfigFilePath))
            {
                var s = File.ReadAllText(ConfigFilePath);
                using (var sww = new StringReader(s))
                {
                    var systemList = (SystemList)xsSubmit.Deserialize(sww);
                    return systemList.Systems;
                }
            }

            return new List<GameSystem>();
        }

        public string BuildConfig(List<GameSystem> systems)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(SystemList));
            var subReq = new SystemList()
            {
                Systems = systems
            };
            var xml = "";

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = true,
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    xsSubmit.Serialize(writer, subReq, ns);
                    xml = sww.ToString(); // Your XML
                }
            }

            return xml;
        }

        public void UpdatePath(string path)
        {
            _path = path;
        }

        public string ConfigFilePath => Path.Combine(_path, "es_systems.cfg");
    }
}
