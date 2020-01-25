using Config.EmulationStation;
using ConfigurationStation.WPF.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConfigurationStation.WPF.Controllers
{
    public class RomsController
    {
        private CommonOpenFileDialog dlg = new CommonOpenFileDialog();
        private RomsModel _romsModel;
        public Action OnSystemsCommand { get; set; }
        public Action OnGenerateCommand { get; set; }
        public RomsController(RomsModel romsModel)
        {
            _romsModel = romsModel;
            _romsModel.IsESPath = true;
            _romsModel.SystemsCommand = new RelayCommand(SystemsCommand);
            _romsModel.GenerateConfigCommand = new RelayCommand(GenerateConfigCommand);
            _romsModel.BrowseFolderCommand = new RelayCommand(BrowseFolderCommand);
            _romsModel.PropertyChanged += Model_PropertyChanged;
        }

        private void BrowseFolderCommand(object obj)
        {
            var result = ShowFolderDialog("Select folder for EmulationStation", _romsModel.RomPath);
            if (result == CommonFileDialogResult.Ok)
            {
                _romsModel.RomPath = dlg.FileName;
                if (_romsModel.IsCustomPath)
                {
                    _romsModel.SelectedSystem.Path = _romsModel.RomPath;
                }
                else
                {
                    foreach (var system in _romsModel.GameSystems)
                    {
                        system.Path = Path.Combine(_romsModel.RomPath, system.Platform);
                    }
                }
            }
            UpdateCanGenerate();
        }

        private void UpdateCanGenerate()
        {
            var isEnabled = true;
            foreach (var system in _romsModel.GameSystems)
            {
                isEnabled &= !string.IsNullOrEmpty(system.Path);
            };
            _romsModel.CanGenerate = isEnabled;
        }

        private void GenerateConfigCommand(object obj)
        {
            OnGenerateCommand?.Invoke();
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(RomsModel.SelectedSystem))
            {
                if (_romsModel.IsCustomPath)
                {
                    _romsModel.RomPath = _romsModel.SelectedSystem.Path;
                }
            }
        }

        private void SystemsCommand(object obj)
        {
            OnSystemsCommand?.Invoke();
        }

        internal void SetGameSystems(IEnumerable<GameSystem> gameSystems)
        {
            _romsModel.GameSystems = new System.Collections.ObjectModel.ObservableCollection<GameSystemModel>(gameSystems.Select(x => new GameSystemModel()
            {
                Command = x.Command,
                CommandTemplate = x.CommandTemplate,
                Emulator = x.Emulator,
                Extension = x.Extension,
                Fullname = x.Fullname,
                Name = x.Name,
                Path = x.Path,
                Platform = x.Platform,
                Theme = x.Theme
            }));
        }


        private CommonFileDialogResult ShowFolderDialog(string title, string currentDirectory)
        {
            dlg.Title = title;
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = currentDirectory;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = currentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            return dlg.ShowDialog();
        }
    }
}
