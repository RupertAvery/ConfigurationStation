using System.Collections.Generic;
using System.Xml.Serialization;

namespace Config.EmulationStation
{
    public class PlatformExtension
    {
        [XmlElement("extension")]
        public string Extension { get; set; }
        [XmlElement("platform")]
        public string Platform { get; set; }
    }

    [XmlRoot("extensions")]
    public class Extensions
    {
        [XmlElement("system")]
        public List<PlatformExtension> SystemExtensions { get; set; }
    }
}
