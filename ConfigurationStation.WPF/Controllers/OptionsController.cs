using ConfigurationStation.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Controllers
{
    public class OptionsController
    {
        private OptionsModel _model;
        public Action OnGenerateCommand { get; set; }
        public Action OnRomsCommand { get; set; }

        public OptionsController(OptionsModel model)
        {
            _model = model;
            _model.RomsCommand = new RelayCommand(RomsCommand);
            _model.GenerateCommand = new RelayCommand(GenerateCommand);
            _model.OnlyMissingCores = true;
        }

        private void RomsCommand(object obj)
        {
            OnRomsCommand?.Invoke();
        }

        private void GenerateCommand(object obj)
        {
            OnGenerateCommand?.Invoke();
        }
    }
}
