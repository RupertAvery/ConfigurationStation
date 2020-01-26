using ConfigurationStation.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationStation.WPF.Controls
{
    public class DownloadProgressModel : NotifyViewModel
    {
        private string message;
        private long _value;
        private long maximum;

        public string Message { get => message; set { message = value; OnPropertyChanged(); } }
        public long Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public long Maximum { get => maximum; set { maximum = value; OnPropertyChanged(); } }

    }
}
