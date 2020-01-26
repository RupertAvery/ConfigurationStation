using ConfigurationStation.WPF.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;

namespace ConfigurationStation.WPF.Controllers
{
    public class RootFolderController : IDisposable
    {
        private RootFoldersModel _rootFolderModel;
        private CommonOpenFileDialog dlg = new CommonOpenFileDialog();
        public Action OnSelectSystems { get; set; }
        public Func<object, object> FindResource { get; internal set; }

        public RootFolderController(RootFoldersModel rootFolderModel)
        {
            _rootFolderModel = rootFolderModel;
            _rootFolderModel.BrowseEmulationStationCommand = new RelayCommand(BrowseES);
            _rootFolderModel.BrowseRetroArchCommand = new RelayCommand(BrowseRA);
            _rootFolderModel.SelectSystems = new RelayCommand(SelectSystems);
            _rootFolderModel.Message = "ConfigurationStation needs the following paths to be set";
            _rootFolderModel.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RootFoldersModel.EmulationStationPath) || e.PropertyName == nameof(RootFoldersModel.RetroArchPath))
            {
                UpdateCanContinue();
            }
        }

        private void SelectSystems(object obj)
        {
            OnSelectSystems?.Invoke();
        }

        public void SetDetectedPaths(string emulationStationPath, string retroArchPath)
        {
            _rootFolderModel.EmulationStationPath = emulationStationPath;
            _rootFolderModel.RetroArchPath = retroArchPath;

            UpdateCanContinue();
        }

        private void BrowseRA(object obj)
        {
            var result = ShowFolderDialog("Select folder for RetroArch", _rootFolderModel.RetroArchPath);
            if (result == CommonFileDialogResult.Ok)
            {
                _rootFolderModel.RetroArchPath = dlg.FileName;
                UpdateCanContinue();
            }
        }

        private void BrowseES(object obj)
        {
            var result = ShowFolderDialog("Select folder for EmulationStation", _rootFolderModel.EmulationStationPath);
            if (result == CommonFileDialogResult.Ok)
            {
                _rootFolderModel.EmulationStationPath = dlg.FileName;
                UpdateCanContinue();
            }
        }

        public void UpdateCanContinue()
        {
            var esFolderExists =
                !string.IsNullOrEmpty(_rootFolderModel.EmulationStationPath) &&
                Directory.Exists(_rootFolderModel.EmulationStationPath);

            var raFolderExists =
                !string.IsNullOrEmpty(_rootFolderModel.RetroArchPath) &&
                Directory.Exists(_rootFolderModel.RetroArchPath);

            var raFilesExist = raFolderExists &&
                File.Exists(Path.Combine(_rootFolderModel.RetroArchPath, "retroarch.exe"));

            _rootFolderModel.CanContinue =
                esFolderExists &&
                (raFolderExists && raFilesExist);


            if (_rootFolderModel.CanContinue)
            {
                _rootFolderModel.ESStyle = (Style)FindResource("CheckStyle");
                _rootFolderModel.ESMessage = "EmulationStation was found";

                _rootFolderModel.RAStyle = (Style)FindResource("CheckStyle");
                _rootFolderModel.RAMessage = "RetroArch was found";
            }
            else
            {
                if (!esFolderExists)
                {
                    _rootFolderModel.ESStyle = (Style)FindResource("XStyle");
                    _rootFolderModel.ESMessage = "EmulationStation folder  was  not found!";
                }
                else
                {
                    _rootFolderModel.ESStyle = (Style)FindResource("CheckStyle");
                    _rootFolderModel.ESMessage = "EmulationStation was found";
                }

                if (!raFolderExists)
                {
                    _rootFolderModel.RAStyle = (Style)FindResource("XStyle");
                    _rootFolderModel.RAMessage = "RetroArch folder was not found!";
                }
                else if (!raFilesExist)
                {
                    _rootFolderModel.RAStyle = (Style)FindResource("XStyle");
                    _rootFolderModel.RAMessage = "retroarch.exe was not found!";
                }
                else
                {
                    _rootFolderModel.RAStyle = (Style)FindResource("CheckStyle");
                    _rootFolderModel.RAMessage = "RetroArch was found";
                }

            }
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

        public void Dispose()
        {
            _rootFolderModel.PropertyChanged -= Model_PropertyChanged;
        }
    }
}
