using Config.EmulationStation;
using Config.RetroArch;
using ConfigurationStation.WPF.Models;
using ConfigurationStation.WPF.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ConfigurationStation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SystemSelected> _systems;
        private bool _hasExisting;

        private RootFolders _rootFoldersPage;
        private Systems _systemsPage;
        private Roms _romsPage;

        private EmulationStationConfiguration _emulationStationConfiguration;
        private RetroArchConfiguration _retroArchConfiguration;

        private string _retroArchPath;
        private string _emulationStationPath;
        private string _appPath;

        private Dictionary<string, GameSystem> _gameSystems;


        public MainWindow()
        {
            InitializeComponent();

            _appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "");

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            _retroArchPath = Path.Combine(appData, "RetroArch");
            _emulationStationPath = Path.Combine(userProfile, ".emulationstation");

            _retroArchConfiguration = new RetroArchConfiguration(_retroArchPath, @"%APPDATA%\RetroArch");
            _emulationStationConfiguration = new EmulationStationConfiguration(_emulationStationPath);



            _systems = _emulationStationConfiguration.GetPlatforms(Path.Combine(_appPath, "Resources\\platforms.xml")).Select(x => new SystemSelected() { System = x.Fullname, Platform = x.Name }).ToList();

            _rootFoldersPage = new RootFolders();
            _rootFoldersPage.Controller.SetDetectedPaths(_emulationStationPath, _retroArchPath);
            _rootFoldersPage.Controller.OnSelectSystems = () =>
            {
                _emulationStationConfiguration.UpdatePath(_rootFoldersPage.Model.EmulationStationPath);
                _retroArchConfiguration.UpdatePath(_rootFoldersPage.Model.RetroArchPath);
                ShowSystems();
            };

            _systemsPage = new Systems();
            _systemsPage.Controller.OnRomsCommand = () =>
            {
                ShowRoms();
            };
            _systemsPage.Controller.OnRootFolderCommand = () =>
            {
                ShowRootFolders();
            };

            _systemsPage.Controller.LoadDefaultSystems(_systems);


            _romsPage = new Roms();
            _romsPage.Controller.OnSystemsCommand = () =>
            {
                ShowSystems();
            };

            _romsPage.Controller.OnGenerateCommand = () =>
            {
                var systems = _romsPage.Model.GameSystems.Select(system => new GameSystem()
                {
                    Name = system.Platform,
                    Fullname = system.Fullname,
                    Extension = system.Extension,
                    Theme = system.Platform,
                    Platform = system.Platform,
                    Command  = system.CommandTemplate?.Replace("{exe}", "retroarch.exe").Replace("{path}", _retroArchPath)
                }).ToList();

                var config = _emulationStationConfiguration.BuildConfig(systems);

                File.WriteAllText(_emulationStationConfiguration.ConfigFilePath, config);
            };

            ShowRootFolders();

            //BuildSystems(gameSystems, _systems);
        }

        private void ShowRoms()
        {
            var systems = BuildSystems(_systemsPage.Model.SelectedSystems);
            _romsPage.Controller.SetGameSystems(systems);
            Frame.Content = _romsPage;
        }

        private void ShowRootFolders()
        {
            Frame.Content = _rootFoldersPage;
        }

        private void ShowSystems()
        {
            _gameSystems = _emulationStationConfiguration.ReadConfig().ToDictionary(x => x.Platform);
            _hasExisting = false;

            foreach (var system in _systems)
            {
                if (_gameSystems.ContainsKey(system.Platform))
                {
                    system.Selected = true;
                    _hasExisting = true;
                }
            }

            _systemsPage.Controller.SetHasExisting(_hasExisting);

            Frame.Content = _systemsPage;
        }

        private List<GameSystem> BuildSystems(IEnumerable<SystemSelected> systems)
        {
            var extensions = _emulationStationConfiguration.GetExtensions(Path.Combine(_appPath, "Resources\\extensions.xml")).ToDictionary(x => x.Platform, x => x.Extension);
            var cores = _retroArchConfiguration.GetCores(Path.Combine(_appPath, "Resources\\cores.xml")).Where(x => !string.IsNullOrEmpty(x.Platform)).ToLookup(x => x.Platform);

            var _systems = new List<GameSystem>();

            foreach (var system in systems.Where(x => x.Selected))
            {
                extensions.TryGetValue(system.Platform, out string extension);
                var coreGroup = cores.FirstOrDefault(x => x.Key == system.Platform);
                var core = coreGroup?.FirstOrDefault(x => x.Default);

                if (_gameSystems.TryGetValue(system.Platform, out GameSystem gameSystem))
                {
                    if (string.IsNullOrEmpty(gameSystem.Command))
                    {
                        gameSystem.CommandTemplate = core?.Command;
                    }
                    _systems.Add(gameSystem);
                }
                else
                {

                    _systems.Add(new GameSystem()
                    {
                        Name = system.Platform,
                        Fullname = system.System,
                        Extension = extension,
                        Theme = system.Platform,
                        Platform = system.Platform,
                        CommandTemplate = core?.Command,
                        Emulator = core?.Emulator
                    });
                }
            }

            return _systems;
        }

    }
}
