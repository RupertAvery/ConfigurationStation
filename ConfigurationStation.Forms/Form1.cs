using Config.EmulationStation;
using Config.RetroArch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigurationStation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var retroArch = new RetroArchConfiguration(Path.Combine(appData, "RetroArch"), @"%APPDATA%\RetroArch");
            var emustation = new EmulationStationConfiguration(Path.Combine(userProfile, ".emulationstation"));

            var systemsList = new List<GameSystem> { new GameSystem() {  } };

            var ext = emustation.GetExtensions(@"C:\Users\ruper\.emulationstation\extensions.cfg");

            var config = emustation.BuildConfig(systemsList);

        }
    }
}
