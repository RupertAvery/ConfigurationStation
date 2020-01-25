using Config.EmulationStation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigurationStation.WPF.Models
{
    public enum FolderOptionEnum
    {
        Default,
        Custom
    }

    public class GameSystemModel : NotifyViewModel
    {
        private string _name;
        private string _fullname;
        private string _path;
        private string _extension;
        private string _command;
        private string _platform;
        private string _theme;
        private string _commandTemplate;
        private string _emulator;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Fullname { get => _fullname; set { _fullname = value; OnPropertyChanged(); } }
        public string Path { get => _path; set { _path = value; OnPropertyChanged(); } }
        public string Extension { get => _extension; set { _extension = value; OnPropertyChanged(); } }
        public string Command { get => _command; set { _command = value; OnPropertyChanged(); } }
        public string Platform { get => _platform; set { _platform = value; OnPropertyChanged(); } }
        public string Theme { get => _theme; set { _theme = value; OnPropertyChanged(); } }
        public string CommandTemplate { get => _commandTemplate; set { _commandTemplate = value; OnPropertyChanged(); } }
        public string Emulator { get => _emulator; set { _emulator = value; OnPropertyChanged(); } }
    }

    public class SystemRomFolder : NotifyViewModel
    {
        private string system;
        private string romFolder;

        public string System { get => system; set { system = value; OnPropertyChanged(); } }
        public string RomFolder { get => romFolder; set { romFolder = value; OnPropertyChanged(); } }
    }

    public class RomsModel : NotifyViewModel
    {
        private string romPath;
        private ICommand systemsCommand;
        private ICommand generateConfigCommand;
        private ICommand browseFolderCommand;
        private bool canGenerate;
        private ObservableCollection<GameSystemModel> gameSystems;
        private GameSystemModel selectedSystem;

        private bool isESPath;
        private bool isCustomPath;

        public bool IsESPath { get => isESPath; set { isESPath = value; OnPropertyChanged(); } }
        public bool IsCustomPath { get => isCustomPath; set { isCustomPath = value; OnPropertyChanged(); } }

        public bool CanGenerate { get => canGenerate; set { canGenerate = value; OnPropertyChanged(); } }
        public ICommand SystemsCommand { get => systemsCommand; set { systemsCommand = value; OnPropertyChanged(); } }
        public ICommand GenerateConfigCommand { get => generateConfigCommand; set { generateConfigCommand = value; OnPropertyChanged(); } }
        public ICommand BrowseFolderCommand { get => browseFolderCommand; set { browseFolderCommand = value; OnPropertyChanged(); } }

        public string RomPath { get => romPath; set { romPath = value; OnPropertyChanged(); } }
        public ObservableCollection<GameSystemModel> GameSystems { get => gameSystems; set { gameSystems = value; OnPropertyChanged(); } }
        public GameSystemModel SelectedSystem { get => selectedSystem; set { selectedSystem = value; OnPropertyChanged(); } }
    }

}
