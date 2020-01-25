using System.Collections.Generic;
using System.Xml.Serialization;

namespace Config.RetroArch
{
    [XmlRoot("cores")]
    public class RetroArchCoreList
    {
        [XmlElement("core")]
        public List<RetroArchCore> Cores { get; set; }
    }


    // Add a description
    public class RetroArchCore
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("command")]
        public string Command { get; set; }
        [XmlElement("emulator")]
        public string Emulator { get; set; }
        [XmlElement("platform")]
        public string Platform { get; set; }
        [XmlElement("default")]
        public bool Default { get; set; }
    }
}
