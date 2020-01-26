using ConfigurationStation.WPF.Controls;
using ConfigurationStation.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConfigurationStation.WPF.Controllers
{
    public class InstallProgressController
    {
        private InstallProgressModel _model;
        private StackPanel container;

        public Action OnClose { get; set; }
        public InstallProgressController(InstallProgressModel model)
        {
            _model = model;
            _model.CanClose = false;
            _model.CloseCommand = new RelayCommand(CloseCommand);
            _model.ButtonText = "Cancel";

            var uc = new UserControl();
            uc.DataContext = _model;
            //var gr = new Grid();
            container = new StackPanel();
            uc.Content = container;
            _model.Controls = uc;
        }

        private void CloseCommand(object obj)
        {
            OnClose?.Invoke();
        }

        public void EnableClose(bool value)
        {
            _model.CanClose = value;
            _model.ButtonText = value ? "Close" : "Cancel";
        }

        public void AddLog(string text)
        {
            _model.Logs += $"{text}\r\n";
        }

        public DownloadProgressControl AddDownload()
        {
            var control = new DownloadProgressControl();
            container.Children.Add(control);
            return control;
        }
    }
}
