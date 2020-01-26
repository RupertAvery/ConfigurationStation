using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConfigurationStation.WPF.Models
{
    public class InstallProgressModel : NotifyViewModel
    {
        private string logs;
        private ICommand closeCommand;
        private bool canClose;
        private string buttonText;
        private FrameworkElement controls;

        public FrameworkElement Controls { get => controls; set { controls = value; OnPropertyChanged(); } }
        public bool CanClose { get => canClose; set { canClose = value; OnPropertyChanged(); } }
        public ICommand CloseCommand { get => closeCommand; set { closeCommand = value; OnPropertyChanged(); } }
        public string Logs { get => logs; set { logs = value; OnPropertyChanged(); } }
        public string ButtonText { get => buttonText; set { buttonText = value; OnPropertyChanged(); } }
    }
}
