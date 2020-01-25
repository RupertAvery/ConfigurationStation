using ConfigurationStation.WPF.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace ConfigurationStation.WPF.Controllers
{
    public class RootFolderController : IDisposable
    {
        private RootFoldersModel _rootFolderModel;
        private CommonOpenFileDialog dlg = new CommonOpenFileDialog();
        public Action OnSelectSystems { get; set; }

        public RootFolderController(RootFoldersModel rootFolderModel)
        {
            _rootFolderModel = rootFolderModel;
            _rootFolderModel.BrowseEmulationStationCommand = new RelayCommand(BrowseES);
            _rootFolderModel.BrowseRetroArchCommand = new RelayCommand(BrowseRA);
            _rootFolderModel.SelectSystems = new RelayCommand(SelectSystems);
            _rootFolderModel.Message = "One or more folders could not be found. You may need to install EmulationStation and/or RetroArch. If you have already done so, please point the paths below to their correct locations";
            _rootFolderModel.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(RootFoldersModel.EmulationStationPath) || e.PropertyName == nameof(RootFoldersModel.RetroArchPath))
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
            if (string.IsNullOrEmpty(emulationStationPath) || string.IsNullOrEmpty(retroArchPath))
            {
                _rootFolderModel.Message = "One or more folders could not be found. You may need to install EmulationStation and/or RetroArch. If you have already done so, please point the paths below to their correct locations";
            }
            else
            {
                _rootFolderModel.Message = "EmulationStation and RetroArch were found at the locations below. If this is correct, proceed with selecting your systems";
            }
            UpdateCanContinue();
        }

        private void BrowseRA(object obj)
        {
            var result = ShowFolderDialog("Select folder for EmulationStation", _rootFolderModel.RetroArchPath);
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
            _rootFolderModel.CanContinue = !string.IsNullOrEmpty(_rootFolderModel.EmulationStationPath) && !string.IsNullOrEmpty(_rootFolderModel.RetroArchPath);
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
