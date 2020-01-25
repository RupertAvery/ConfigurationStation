using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Config.EmulationStation
{
    [XmlRoot("systemList")]
    public class SystemList
    {
        [XmlElement("system")]
        public List<GameSystem> Systems { get; set; }
    }

    [XmlRoot("platforms")]
    public class PlatformsList
    {
        [XmlElement("platform")]
        public List<Platform> Platforms { get; set; }
    }

}
