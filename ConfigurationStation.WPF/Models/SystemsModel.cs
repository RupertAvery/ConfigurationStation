using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigurationStation.WPF.Models
{

    public class SystemSelected : NotifyViewModel
    {
        private string system;
        private bool selected;

        public string Platform { get; set; }
        public string System { get => system; set { system = value; OnPropertyChanged(); } }
        public bool Selected { get => selected; set { selected = value; OnPropertyChanged(); } }
    }

    public class SystemsModel : NotifyViewModel
    {
        private IEnumerable<SystemSelected> selectedSystems;
        private string message;
        private ICommand rootFoldersCommand;
        private ICommand romsCommand;
        private bool canContinue;

        public bool CanContinue { get => canContinue; set { canContinue = value; OnPropertyChanged(); } }
        public ICommand RootFoldersCommand { get => rootFoldersCommand; set { rootFoldersCommand = value; OnPropertyChanged(); } }
        public ICommand RomsCommand { get => romsCommand; set { romsCommand = value; OnPropertyChanged(); } }

        public IEnumerable<SystemSelected> SelectedSystems { get => selectedSystems; set { selectedSystems = value; OnPropertyChanged(); } }

        public string Message { get => message; set { message = value; OnPropertyChanged(); } }
    }
}
