using Config.EmulationStation;
using Config.RetroArch;
using ConfigurationStation.WPF.Actions;
using ConfigurationStation.WPF.Models;
using ConfigurationStation.WPF.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Path = System.IO.Path;
using SplashScreen = ConfigurationStation.WPF.Pages.SplashScreen;

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
        private InstallProgress _installProgressPage;
        private SplashScreen _splashScreen;
        private Options _optionsPage;

        private EmulationStationConfiguration _emulationStationConfiguration;
        private RetroArchConfiguration _retroArchConfiguration;

        private string _retroArchPath;
        private string _emulationStationPath;
        private string _appPath;

        private Dictionary<string, GameSystem> _gameSystems;
        private Dictionary<string, string> _extensions;
        private ILookup<string, RetroArchCore> _cores;
        private CancellationTokenSource _cancellationTokenSource;

        private DispatcherTimer _timer;

        class CoreNameComparer : IEqualityComparer<RetroArchCore>
        {
            public bool Equals(RetroArchCore x, RetroArchCore y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(RetroArchCore obj)
            {
                return obj.Name.GetHashCode();
            }
        }

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

            LoadResources(_emulationStationConfiguration, _retroArchConfiguration, _appPath);

            InitializePages(_emulationStationPath, _retroArchPath);

            _splashScreen = new SplashScreen();

            _timer = new DispatcherTimer();
            _timer.Tick += DispatcherTimer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 2);
            _timer.Start();

            Closing += MainWindow_Closing;

            ShowSplashScreen();
        }

        private void LoadResources(EmulationStationConfiguration emulationStationConfiguration, RetroArchConfiguration retroArchConfiguration, string appPath)
        {
            _extensions = emulationStationConfiguration.GetExtensions(Path.Combine(appPath, "Resources\\extensions.xml")).ToDictionary(x => x.Platform, x => x.Extension);
            _cores = retroArchConfiguration.GetCores(Path.Combine(appPath, "Resources\\cores.xml")).Where(x => !string.IsNullOrEmpty(x.Platform)).ToLookup(x => x.Platform);
            _systems = emulationStationConfiguration.GetPlatforms(Path.Combine(appPath, "Resources\\platforms.xml")).Select(x => new SystemSelected() { System = x.Fullname, Platform = x.Name }).ToList();
        }

        private void InitializePages(string emulationStationPath, string retroArchPath)
        {
            _rootFoldersPage = new RootFolders();
            _rootFoldersPage.Controller.SetDetectedPaths(emulationStationPath, retroArchPath);
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

            _romsPage.Controller.OnOptionsCommand = () =>
            {
                ShowOptions();
            };

            _optionsPage = new Options();
            _optionsPage.Controller.OnRomsCommand = () =>
            {
                ShowRoms();
            };
            _optionsPage.Controller.OnGenerateCommand = () =>
            {
                ShowInstallProgress();
                GenerateConfig();
            };

            _installProgressPage = new InstallProgress();
            _installProgressPage.Controller.OnClose = () =>
            {
                if (_installProgressPage.Model.CanClose)
                {
                    Close();
                }
                else
                {
                    _cancellationTokenSource.Cancel();
                }
            };

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            ShowRootFolders();
            _timer.Stop();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        private string BuildCommand(GameSystemModel system)
        {
            if (string.IsNullOrEmpty(system.Command) || system.Command == "\"\"")
            {
                return $"\"{system.CommandTemplate?.Replace("{exe}", "retroarch.exe").Replace("{path}", _retroArchPath)}\"";
            }
            else
            {
                return system.Command;
            }
        }

        private void WriteConfig()
        {
            var systems = _romsPage.Model.GameSystems.Select(system => new GameSystem()
            {
                Name = system.Platform,
                Fullname = system.Fullname,
                Path = system.Path,
                Extension = system.Extension,
                Theme = system.Platform,
                Platform = system.Platform,
                Command = BuildCommand(system)
            }).ToList();

            var config = _emulationStationConfiguration.BuildConfig(systems);

            var control = _installProgressPage.Controller.AddDownload();
            control.Model.Message = "Writing es__systems.cfg";
            control.Model.Value = 0;
            control.Model.Maximum = 100;
            File.WriteAllText(_emulationStationConfiguration.ConfigFilePath, config);
            control.Model.Value = 100;
        }

        private void GenerateConfig()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var tasks = new List<Task>();


            WriteConfig();


            if (_optionsPage.Model.DownloadLibretroCores)
            {
                tasks.AddRange(DownloadCores(_optionsPage.Model.OnlyMissingCores, _optionsPage.Model.OverwriteCores, _cancellationTokenSource.Token));
            }

            if (_optionsPage.Model.DownloadPpssppAssets)
            {
                tasks.Add(DownloadPPSSPPAssets(_cancellationTokenSource.Token));
            }

            Task.WhenAll(tasks.ToArray()).ContinueWith(x =>
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _installProgressPage.Controller.AddLog("Cancelled");
                }
                else
                {
                    _installProgressPage.Controller.AddLog("Complete!");
                    _installProgressPage.Controller.AddLog("");
                    _installProgressPage.Controller.AddLog("If you have not yet setup your gamepad, run RetroArch first and go to Settings > Inputs > Port 1 Binds");
                    _installProgressPage.Controller.AddLog("and map the buttons and analog sticks for your gamepad. Do the same for Port 2 as necessary");
                    _installProgressPage.Controller.AddLog("You may need to setup your gamepad for use in Emulationstation separately.");
                }
                _installProgressPage.Controller.EnableClose(true);
            });

        }

        private string GetRetroArchVersion()
        {
            var exe = Path.Combine(_retroArchPath, "retroarch.exe");
            Process process = new Process();
            process.StartInfo.FileName = exe;
            process.StartInfo.Arguments = "--version"; 
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private Task DownloadPPSSPPAssets(CancellationToken cancellationToken)
        {
            var control = _installProgressPage.Controller.AddDownload();
            
            var context = new ActionContext()
            {
                AddLog = _installProgressPage.Controller.AddLog,
                SetMessage = (s) =>
                {
                    control.Model.Message = s;
                },
                SetProgressMax = (s) =>
                {
                    control.Model.Maximum = s;
                },
                SetProgressValue = (s) =>
                {
                    control.Model.Value = s;
                },
                RetroArchPath = _retroArchPath,
            };
            var action = new PpssppAssetsDownloadAction();
            var task = Task.Run(async () => await action.Execute(context, cancellationToken), cancellationToken);
            return task;
        }

        private List<Task> DownloadCores(bool onlyMissingCores, bool overwriteCores, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            var version = GetRetroArchVersion();

            _installProgressPage.Controller.AddLog("Downloading Libretro cores");

            string coreUrl = "https://buildbot.libretro.com/nightly/windows/x86_64/latest";

            if (version.Contains("32-bit"))
            {
                coreUrl = "https://buildbot.libretro.com/nightly/windows/x86/latest";
                _installProgressPage.Controller.AddLog("32-bit Retroarch found");
            }
            else if (version.Contains("64-bit"))
            {
                coreUrl = "https://buildbot.libretro.com/nightly/windows/x86_64/latest";
                _installProgressPage.Controller.AddLog("64-bit Retroarch found");
            }

            var cores = new List<RetroArchCore>();

            foreach (var system in _romsPage.Model.GameSystems)
            {
                var platFormcores = _cores.FirstOrDefault(x => x.Key == system.Platform);
                var core = platFormcores?.FirstOrDefault(x => x.Default);
                var downloadCore = true;
                if (onlyMissingCores)
                {
                    downloadCore = !File.Exists(Path.Combine(_retroArchPath, "cores", $"{core.Name}.dll"));
                }

                if (core != null && downloadCore)
                {
                    cores.Add(core);
                }
            }


            foreach (var core in cores.Distinct(new CoreNameComparer()))
            {
                tasks.Add(DownloadCore(coreUrl, overwriteCores, core, cancellationToken));
            }


            return tasks;
        }

        private Task DownloadCore(string coreUrl, bool overwriteCores, RetroArchCore core, CancellationToken cancellationToken)
        {
            var control = _installProgressPage.Controller.AddDownload();

            var context = new ActionContext()
            {
                AddLog = _installProgressPage.Controller.AddLog,
                SetMessage = (s) =>
                {
                    control.Model.Message = s;
                },
                SetProgressMax = (s) =>
                {
                    control.Model.Maximum = s;
                },
                SetProgressValue = (s) =>
                {
                    control.Model.Value = s;
                },
                RetroArchPath = _retroArchPath,
                Core = core
            };
            var action = new DownloadCoreAction(coreUrl, overwriteCores);
            return Task.Run(async () => await action.Execute(context, cancellationToken), cancellationToken);
        }

        private void ShowRoms()
        {
            var systems = BuildSystems(_systemsPage.Model.SelectedSystems);
            _romsPage.Controller.SetGameSystems(systems);
            Frame.Content = _romsPage;
        }

        private void ShowOptions()
        {
            Frame.Content = _optionsPage;
        }

        private void ShowInstallProgress()
        {
            Frame.Content = _installProgressPage;
        }

        private void ShowRootFolders()
        {
            Frame.Content = _rootFoldersPage;
        }

        private void ShowSplashScreen()
        {
            Frame.Content = _splashScreen;
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

            var _systems = new List<GameSystem>();

            foreach (var system in systems.Where(x => x.Selected))
            {
                _extensions.TryGetValue(system.Platform, out string extension);
                var coreGroup = _cores.FirstOrDefault(x => x.Key == system.Platform);
                var core = coreGroup?.FirstOrDefault(x => x.Default);

                if (_gameSystems.TryGetValue(system.Platform, out GameSystem gameSystem))
                {
                    gameSystem.CommandTemplate = core?.Command;
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
