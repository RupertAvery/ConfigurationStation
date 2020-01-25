using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Config.RetroArch
{

    public class RetroArchConfiguration
    {
        string _path;
        string _commandPath;
        const string _coresUrl = @"https://buildbot.libretro.com/nightly/windows/x86_64/latest/";
        IEnumerable<RetroArchCore> _cores;

        // probably move this to a configuration file
        public RetroArchConfiguration(string path, string commandPath)
        {
            _path = path;
            _commandPath = commandPath;
        }


        public string CommandPath => _commandPath;
        public IEnumerable<RetroArchCore> Cores => _cores;
        public string CoresUrl => _coresUrl;
        public string CoresFolder => Path.Combine(_path, "cores");


        public List<RetroArchCore> GetCores(string path)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(RetroArchCoreList));

            var settings = new XmlReaderSettings()
            {

            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var s = File.ReadAllText(path);
            using (var sww = new StringReader(s))
            {
                var coreList = (RetroArchCoreList)xsSubmit.Deserialize(sww);
                return coreList.Cores;
            }
        }

        public void UpdatePath(string path)
        {
            _path = path;
        }
    }
}
