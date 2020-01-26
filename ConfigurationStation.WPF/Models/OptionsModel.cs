using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigurationStation.WPF.Models
{
    public class OptionsModel : NotifyViewModel
    {
        private ICommand generateCommand;
        private ICommand romsCommand;
        public bool downloadLibretroCores;
        public bool onlyMissing;
        public bool overwriteCores;
        public bool downloadPpssppAssets;

        public bool DownloadLibretroCores { get => downloadLibretroCores; set { downloadLibretroCores = value; OnPropertyChanged(); } }
        public bool OnlyMissingCores { get => onlyMissing; set { onlyMissing = value; OnPropertyChanged(); } }
        public bool OverwriteCores { get => overwriteCores; set { overwriteCores = value; OnPropertyChanged(); } }
        public bool DownloadPpssppAssets { get => downloadPpssppAssets; set { downloadPpssppAssets = value; OnPropertyChanged(); } }
        public ICommand RomsCommand { get => romsCommand; set { romsCommand = value; OnPropertyChanged(); } }
        public ICommand GenerateCommand { get => generateCommand; set { generateCommand = value; OnPropertyChanged(); } }
    }
}
