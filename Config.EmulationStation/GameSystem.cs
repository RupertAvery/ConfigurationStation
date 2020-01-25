using System.Xml;
using System.Xml.Serialization;

namespace Config.EmulationStation
{
    public class Platform
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "fullname")]
        public string Fullname { get; set; }
    }

    public class GameSystem
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "fullname")]
        public string Fullname { get; set; }
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
        [XmlElement(ElementName = "extension")]
        public string Extension { get; set; }
        [XmlElement(ElementName = "command")]
        public string Command { get; set; }
        [XmlElement(ElementName = "platform")]
        public string Platform { get; set; }
        [XmlElement(ElementName = "theme")]
        public string Theme { get; set; }
        [XmlIgnore]
        public string CommandTemplate { get; set; }
        [XmlIgnore]
        public string Emulator { get; set; }
    }
}
