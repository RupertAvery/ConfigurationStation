using System.Windows.Input;

namespace ConfigurationStation.WPF.Models
{
    public class RootFoldersModel : NotifyViewModel
    {
        private string emulationStationPath;
        private string retroArchPath;
        private string message;
        private ICommand browseEmulationStationCommand;
        private ICommand browseRetroArchCommand;
        private bool canContinue;
        private ICommand selectSystems;

        public bool CanContinue { get => canContinue; set { canContinue = value; OnPropertyChanged(); } }
        public string EmulationStationPath { get => emulationStationPath; set { emulationStationPath = value; OnPropertyChanged(); } }
        public string RetroArchPath { get => retroArchPath; set { retroArchPath = value; OnPropertyChanged(); } }
        public string Message { get => message; set { message = value; OnPropertyChanged(); } }
        public ICommand BrowseEmulationStationCommand { get => browseEmulationStationCommand; set { browseEmulationStationCommand = value; OnPropertyChanged(); } }
        public ICommand BrowseRetroArchCommand { get => browseRetroArchCommand; set { browseRetroArchCommand = value; OnPropertyChanged(); } }
        public ICommand SelectSystems { get => selectSystems; set { selectSystems = value; OnPropertyChanged(); } }
    }

}
