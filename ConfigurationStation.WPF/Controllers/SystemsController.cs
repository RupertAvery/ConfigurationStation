using ConfigurationStation.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Controllers
{
    public class SystemsController
    {
        private SystemsModel _model;
        public Action OnRootFolderCommand { get; set; }
        public Action OnRomsCommand { get; set; }
        public SystemsController(SystemsModel model)
        {
            _model = model;
            _model.RootFoldersCommand = new RelayCommand(RootFoldersCommand);
            _model.RomsCommand = new RelayCommand(RomsCommand);
        }

        private void RootFoldersCommand(object obj)
        {
            OnRootFolderCommand?.Invoke();
        }

        private void RomsCommand(object obj)
        {
            OnRomsCommand?.Invoke();
        }

        public void LoadDefaultSystems(IEnumerable<SystemSelected> systems)
        {
            _model.SelectedSystems = systems;
        }

        public IEnumerable<SystemSelected> GetSelected()
        {
            return _model.SelectedSystems;
        }

        public void SetHasExisting(bool hasExisting)
        {
            if (hasExisting)
            {
                _model.Message = "The following systems are currently setup in EmulationStation. You may add or remove systems.";
            }
            else
            {
                _model.Message = "There are no systems currently setup in EmulationStation. You may add or remove systems.";
            }
        }
    }
}
